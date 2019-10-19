using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Threading.Tasks;
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

        // Show the course info in evaluation mode
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
                TempData["SuccessMessage"] = "Successfully submited for Verification";
            }
            else
            {
                await LogActivity(Modules.Learning, "Error submit Course For Verification. Course : " + title);
                TempData["ErrorMessage"] = "Error submitting the course for Verification";
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
                TempData["ErrorMessage"] = "Error submitting the course for Verification";
                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var response = await WepApiMethod.SendApiAsync<CourseApprovalLogModel>(HttpVerbs.Post, CourseApprovalApiUrl.SubmitForApproval, model);

            if (response.isSuccess)
            {
                // Approval level is updated on the response Data
                model = response.Data;

                await LogActivity(Modules.Learning, "Successfully submit Course For approval. Course : " + model.CourseTitle);

                if (!model.IsApproved)
                {
                    TempData["SuccessMessage"] = "Your request for amendment to the course is succesfully submitted.";
                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                if (model.ApprovalLevel == ApprovalLevel.Verifier)
                {
                    TempData["SuccessMessage"] = "Successfully Verified.";
                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                if ((model.ApprovalLevel == ApprovalLevel.Approver1 || model.ApprovalLevel == ApprovalLevel.Approver2) && model.IsNextLevelRequired)
                {
                    TempData["SuccessMessage"] = "Successfully Approved. Further approval to the next level.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Course is Approved.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error submitting the course for Verification";
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