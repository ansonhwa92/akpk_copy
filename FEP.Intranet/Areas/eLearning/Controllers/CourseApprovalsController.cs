using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseApprovalApiUrl
    {
        // approvals
        public const string SubmitForVerification = "eLearning/CourseApprovals/SubmitForVerification";

        public const string SubmitForApproval = "eLearning/CourseApprovals/SubmitForApproval";
    }

    public class CourseApprovalsController : FEPController
    {
        private readonly DbEntities db = new DbEntities();

        public async Task<ActionResult> Approve(int? id)
        {
            if (!CurrentUser.HasAccess(UserAccess.CourseVerify) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval1) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval2) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval3))
            {
                return new HttpUnauthorizedResult();
            }

            var model = await CoursesController.TryGetFrontCourse(id.Value);

            model.Description = HttpUtility.HtmlDecode(model.Description);
            model.Objectives = HttpUtility.HtmlDecode(model.Objectives);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            return View(model);
        }

        public async Task<ActionResult> SubmitForVerification(int id, string title)
        {
            if (!CurrentUser.HasAccess(UserAccess.CourseCreate) &&
                !CurrentUser.HasAccess(UserAccess.CourseEdit))
            {
                return new HttpUnauthorizedResult();
            }

            var model = new CourseApprovalLogModel
            {
                CreatedBy = CurrentUser.UserId.Value,
                CreatedByName = CurrentUser.Name,
                CourseId = id,
            };

            var response = await WepApiMethod.SendApiAsync<CourseApprovalLogModel>(HttpVerbs.Post, CourseApprovalApiUrl.SubmitForVerification, model);

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Successfully submit Course For Verification. Course : " + title);

                await Notifier.SubmitCourseForVerication(id, CurrentUser.UserId.Value, "", CurrentUser.Name,
                     title, Url.AbsoluteAction("Approve", "CourseApprovals", new { id = id }));

                TempData["SuccessMessage"] = $"The Course {title} has been successfully submitted for verification.";
            }
            else
            {
                await LogActivity(Modules.Learning, "Error submit Course For Verification. Course : " + title);
                TempData["ErrorMessage"] = $"Error submitting the course {title} for verification.";
            }
            return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitForApproval(CourseApprovalLogModel model)
        {
            if (!CurrentUser.HasAccess(UserAccess.CourseVerify) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval1) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval2) &&
                !CurrentUser.HasAccess(UserAccess.CourseApproval3))
            {
                return new HttpUnauthorizedResult();
            }

            model.CreatedBy = CurrentUser.UserId.Value;
            model.CreatedByName = CurrentUser.Name;

            if (model.CreatedBy < 1)
            {
                TempData["ErrorMessage"] = $"Error submitting the course {model.CourseTitle} for verification";
                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var response = await WepApiMethod.SendApiAsync<CourseApprovalLogModel>(HttpVerbs.Post, CourseApprovalApiUrl.SubmitForApproval, model);

            if (response.isSuccess)
            {
                // Approval level is updated on the response Data
                model = response.Data;

                if (!model.IsApproved)
                {
                    await LogActivity(Modules.Learning, "Course Require AMENDMENT. Course : " + model.CourseTitle, model.CreatedBy);

                    TempData["SuccessMessage"] = $"Course titled {model.CourseTitle} updated as Pending Amendment.";

                    await Notifier.SubmitCourseForAmendment(NotificationType.Course_Amendment,
                        model.CourseId, CurrentUser.UserId.Value, "", model.CreatedByName,
                        model.CourseTitle, Url.AbsoluteAction("Content", "Courses", new { id = model.CourseId }));

                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                // Approved
                if (model.Status == CourseStatus.Verified)
                {
                    await LogActivity(Modules.Learning, "Successfully VERIFIED. Further approval to the next level. Course : " + model.CourseTitle, model.CreatedBy);
                    TempData["SuccessMessage"] = $"Course titled {model.CourseTitle} successfully updated as Verified.";

                    await Notifier.SubmitCourseForApproval(NotificationType.Approve_Courses_Creation_Approver1,
                        model.CourseId, CurrentUser.UserId.Value, "", model.CreatedByName,
                        model.CourseTitle, Url.AbsoluteAction("Approve", "CourseApprovals", new { id = model.CourseId }));

                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                if ((model.Status == CourseStatus.FirstApproval || model.Status == CourseStatus.SecondApproval) && model.IsNextLevelRequired)
                {
                    await LogActivity(Modules.Learning, "Successfully APPROVED. Further approval to the next level. Course : " + model.CourseTitle);
                    TempData["SuccessMessage"] = $"Course {model.CourseTitle} successfully submitted for next approver.";

                    // notify next level
                    NotificationType notifyType = NotificationType.Approve_Courses_Creation_Approver1;
                    if (model.Status == CourseStatus.FirstApproval)
                    {
                        notifyType = NotificationType.Approve_Courses_Creation_Approver2;
                    }
                    else
                    if (model.Status == CourseStatus.SecondApproval)
                    {
                        notifyType = NotificationType.Approve_Courses_Creation_Approver3;
                    }

                    await Notifier.SubmitCourseForApproval(notifyType,
                         model.CourseId, CurrentUser.UserId.Value, "", model.CreatedByName,
                         model.CourseTitle, Url.AbsoluteAction("Approve", "CourseApprovals", new { id = model.CourseId }));
                }
                else
                {
                    await LogActivity(Modules.Learning, "Successfully APPROVED. Course - " + model.CourseTitle);

                    await Notifier.SubmitCourseForApproval(NotificationType.Course_Approved,
                         model.CourseId, CurrentUser.UserId.Value, "", model.CreatedByName,
                         model.CourseTitle, Url.AbsoluteAction("Approve", "CourseApprovals", new { id = model.CourseId }));

                    TempData["SuccessMessage"] = $"Course titled {model.CourseTitle} is Approved.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = $"Error submitting the course {model.CourseTitle} for Verification";
            }
            return RedirectToAction("Index", "Courses", new { area = "eLearning" });
        }

        [HttpGet]
        public ActionResult Get(int id, CourseStatus status, string title)
        {
            var model = new CourseApprovalLogModel
            {
                CourseId = id,
                Status = status,
                ApproverId = CurrentUser.UserId,
                CourseTitle = title
            };

            return PartialView("_approvals", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}