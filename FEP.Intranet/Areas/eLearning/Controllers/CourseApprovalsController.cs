using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
using System;
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

                // -- send email
                var notifyModel = new NotificationModel
                {
                    Id = id,
                    Type = typeof(Course),
                    NotificationType = NotificationType.Verify_Courses_Creation,
                    NotificationCategory = NotificationCategory.Learning,
                    StartNotificationDate = DateTime.Now,
                    ParameterListToSend = new ParameterListToSend
                    {
                        CourseAuthor = model.CreatedByName,
                        CourseTitle = title,
                        CourseApproval = "Course Verification",
                        Link = this.Url.AbsoluteAction("View", "CoursesController", new { id = id})
                    },
                };

                var emailResponse = await EmaiHelper.SendNotification(notifyModel);

                if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) || 
                    !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
                {
                    await LogError(Modules.Learning, $"Error Sending Email For Course Verification. Course Title (Id) : {title} - {id}");
                }

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

                if (!model.IsApproved)
                {
                    await LogActivity(Modules.Learning, "Course NOT APPROVED. Course : " + model.CourseTitle);

                    TempData["SuccessMessage"] = "Your request for amendment to the course is succesfully submitted.";
                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                if (model.ApprovalLevel == ApprovalLevel.Verifier)
                {
                    await LogActivity(Modules.Learning, "Successfully VERIFIED. Further approval to the next level. Course : " + model.CourseTitle);
                    TempData["SuccessMessage"] = "Successfully Verified.";

                    // -- send email to approverlevel1
                    var notifyModel = new NotificationModel
                    {
                        Id = model.CourseId,
                        Type = typeof(Course),
                        NotificationType = NotificationType.Approve_Courses_Creation_Approver1,
                        NotificationCategory = NotificationCategory.Learning,
                        StartNotificationDate = DateTime.Now,
                        ParameterListToSend = new ParameterListToSend
                        {
                            CourseAuthor = model.CreatedByName,
                            CourseTitle = model.CourseTitle,
                            CourseApproval = "Course Approval",
                            Link = this.Url.AbsoluteAction("View", "CourseController", new { id = model.CourseId })
                        },
                    };

                    var emailResponse = await EmaiHelper.SendNotification(notifyModel);

                    if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                        !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
                    {
                        await LogError(Modules.Learning, $"Error Sending Email For Course Approval. Course Title (Id) : {model.CourseTitle} - {model.CourseId}");
                    }
                                       
                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }

                if ((model.ApprovalLevel == ApprovalLevel.Approver1 || model.ApprovalLevel == ApprovalLevel.Approver2) && model.IsNextLevelRequired)
                {
                    await LogActivity(Modules.Learning, "Successfully APPROVED. Further approval to the next level. Course : " + model.CourseTitle);
                    TempData["SuccessMessage"] = "Successfully ssfully submit Course For approvalApproved. Further approval to the next level.";

                    // -- send email to approvers
                    NotificationType notifyType = NotificationType.Approve_Courses_Creation_Approver1;
                    if(model.ApprovalLevel == ApprovalLevel.Approver1)
                    {
                        notifyType = NotificationType.Approve_Courses_Creation_Approver1;
                    }
                    else
                    if(model.ApprovalLevel == ApprovalLevel.Approver2)
                    {
                        notifyType = NotificationType.Approve_Courses_Creation_Approver2;
                    }
                    else
                    if (model.ApprovalLevel == ApprovalLevel.Approver3)
                    {
                        notifyType = NotificationType.Approve_Courses_Creation_Approver3;
                    }


                    var notifyModel = new NotificationModel
                    {
                        Id = model.CourseId,
                        Type = typeof(Course),
                        NotificationType = notifyType,
                        NotificationCategory = NotificationCategory.Learning,
                        StartNotificationDate = DateTime.Now,
                        ParameterListToSend = new ParameterListToSend
                        {
                            CourseAuthor = model.CreatedByName,
                            CourseTitle = model.CourseTitle,
                            CourseApproval = "Course Approval",
                            Link = this.Url.AbsoluteAction("View", "CoursesController", new { id = model.CourseId })
                        },
                    };

                    var emailResponse = await EmaiHelper.SendNotification(notifyModel);

                    if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                        !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
                    {
                        await LogError(Modules.Learning, $"Error Sending Email For Course Approval. Course Title (Id) : {model.CourseTitle} - {model.CourseId}");
                    }
                }
                else
                {
                    await LogActivity(Modules.Learning, "Successfully APPROVED. Course - " + model.CourseTitle);
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