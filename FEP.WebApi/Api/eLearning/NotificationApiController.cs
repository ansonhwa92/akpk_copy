using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApi.Api.Reminder;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Notification")]
    public class NotificationApiController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Route("api/eLearning/Notification/SendNotification")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> SendNotification(NotificationModel model)
        {
            var receivers = new List<int>();

            if (model.ReceiverType == ReceiverType.UserIds || model.ReceiverType == ReceiverType.Both)
            {
                switch (model.NotificationType)
                {
                    case NotificationType.Verify_Courses_Creation:
                        receivers = await GetUserIds(UserAccess.CourseVerify);

                        break;

                    case NotificationType.Approve_Courses_Creation_Approver1:
                        receivers = await GetUserIds(UserAccess.CourseApproval1);
                        break;

                    case NotificationType.Approve_Courses_Creation_Approver2:
                        receivers = await GetUserIds(UserAccess.CourseApproval2);
                        break;

                    case NotificationType.Approve_Courses_Creation_Approver3:
                        receivers = await GetUserIds(UserAccess.CourseApproval3);
                        break;

                    case NotificationType.Course_Amendment:
                        receivers = await GetUserIds(UserAccess.CourseCreate);
                        break;

                    case NotificationType.Course_Approved:
                        receivers = await GetUserIds(UserAccess.CourseCreate);
                        break;

                    case NotificationType.Course_Assigned_To_Facilitator:
                        //get trainer assigned to the course
                        receivers = model.Receivers;
                        break;

                    case NotificationType.Course_Student_Enrolled:
                        receivers = await GetCourseTrainers(model.Id);

                        // get student name
                        var user = await db.User.FindAsync(model.SenderId);
                        model.ParameterListToSend.LearnerName = user != null ? user.Name : "";

                        break;

                    case NotificationType.Notify_Admin_Participant_Withdraw:
                        var admin = await db.Courses.FindAsync(model.Id);

                        if (admin != null)
                            receivers.Add(admin.CreatedBy);

                        break;

                    case NotificationType.Notify_Self_Withdraw_From_Course:
                    case NotificationType.Verify_Courses_Creation_Self:
                    case NotificationType.Approve_Courses_Creation_Approver_Self:
                    case NotificationType.Course_Amendment_Self:

                        receivers.Add(model.SenderId);

                        break;

                    case NotificationType.Course_Approved_Others:

                        receivers.AddRange(await GetUserIds(UserAccess.CourseVerify));

                        break;

                    case NotificationType.Course_Approved_Self:

                        receivers.Add(model.SenderId);
                        break;

                    case NotificationType.Course_Publish:

                        receivers.Add(model.SenderId);
                        receivers.AddRange(await GetUserIds(UserAccess.CourseVerify));
                        receivers.AddRange(await GetUserIds(UserAccess.CourseApproval1));
                        receivers.AddRange(await GetUserIds(UserAccess.CourseApproval2));
                        receivers.AddRange(await GetUserIds(UserAccess.CourseApproval3));
                        break;

                    case NotificationType.Course_Cancelled:

                        receivers.Add(model.SenderId);

                        var approval = db.CourseApprovals.Where(x => x.CourseId == model.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                        if (approval != null)
                        {
                            if (approval.ApprovalLevel == ApprovalLevel.Verifier)
                            {
                                receivers.AddRange(await GetUserIds(UserAccess.CourseVerify));
                            }

                            if (approval.ApprovalLevel == ApprovalLevel.Approver1)
                            {
                                receivers.AddRange(await GetUserIds(UserAccess.CourseApproval1));
                            }

                            if (approval.ApprovalLevel == ApprovalLevel.Approver2)
                            {
                                receivers.AddRange(await GetUserIds(UserAccess.CourseApproval2));
                            }

                            if (approval.ApprovalLevel == ApprovalLevel.Approver3)
                            {
                                receivers.AddRange(await GetUserIds(UserAccess.CourseApproval3));
                            }
                        }

                        break;

                    default:
                        break;
                }

                if (model.ReceiverId == null)
                    model.ReceiverId = new List<int>();

                model.ReceiverId.AddRange(receivers);
            }

            if (model.Type == typeof(Course))
            {
                var course = await db.Courses.FindAsync(model.Id);

                if (course != null)
                {
                    model.ParameterListToSend.CourseTitle = course.Title;
                }
            }

            await db.SaveChangesAsync();

            var controller = new SLAReminderController();

            try
            {
                if (model.IsNeedRemainder)
                {
                    CreateAutoReminder createdAutoReminder = new CreateAutoReminder
                    {
                        NotificationCategory = model.NotificationCategory,
                        NotificationType = model.NotificationType,
                        ParameterListToSend = model.ParameterListToSend,
                        ReceiverId = model.ReceiverId,
                        StartNotificationDate = DateTime.Now
                    };

                    var result = await controller.GenerateAutoNotificationReminder(createdAutoReminder);

                    var response = result as OkNegotiatedContentResult<ReminderResponse>;

                    if (response != null)
                    {
                        if (model.Type == typeof(Course))
                        {
                            var course = await db.Courses.FindAsync(model.Id);

                            if (course != null)
                            {
                                course.SLAReminderId = response.Content.SLAReminderStatusId;

                                db.SetModified(course);
                                await db.SaveChangesAsync();

                                return Ok();
                            }
                        }
                    }
                    else
                    {
                        var log = new ErrorLog
                        {
                            CreatedDate = DateTime.Now,
                            UserId = null,
                            Module = null,
                            Source = " Controller: eLearning/NotificationApi Action: SendNotification",
                            ErrorDescription = "Error sending email",
                            ErrorDetails = "Null response",
                            IPAddress = "",
                        };

                        db.ErrorLog.Add(log);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var result = await controller.GenerateAndSendEmails(model);

                    var response = result as OkNegotiatedContentResult<ReminderResponse>;

                    if (response == null)
                    {
                        var log = new ErrorLog
                        {
                            CreatedDate = DateTime.Now,
                            UserId = null,
                            Module = null,
                            Source = " Controller: eLearning/NotificationApi Action: SendNotification",
                            ErrorDescription = "Error sending email",
                            ErrorDetails = "Null response",
                            IPAddress = "",
                        };

                        db.ErrorLog.Add(log);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                var log = new ErrorLog
                {
                    CreatedDate = DateTime.Now,
                    UserId = null,
                    Module = null,
                    Source = " Controller: eLearning/NotificationApi Action: SendNotification",
                    ErrorDescription = e.Message,
                    ErrorDetails = e.InnerException + " | " + e.StackTrace,
                    IPAddress = "",
                };

                db.ErrorLog.Add(log);
                db.SaveChanges();
            }

            return Ok();
        }

        private async Task<List<int>> GetUserIds(UserAccess userAccess)
        {
            List<int> ids = new List<int>();

            var roleIds = db.RoleAccess.Where(x => x.UserAccess == userAccess).Select(x => x.RoleId).ToList();

            foreach (var roleId in roleIds)
            {
                var userIds = db.UserRole.Where(x => x.RoleId == roleId).Select(x => x.UserId).Distinct().ToList();

                foreach (var userId in userIds)
                {
                    var user = await db.User.FirstOrDefaultAsync(x => x.Display == true && x.Id == userId);

                    if (user != null && user.UserAccount.IsEnable)
                        ids.Add(user.Id);
                }
            }

            return ids.Distinct().ToList();
        }

        private async Task<List<int>> GetCourseTrainers(int courseId)
        {
            List<int> ids = new List<int>();

            var courseTrainers = db.TrainerCourses.Where(x => x.CourseId == courseId);
            {
                foreach (var trainer in courseTrainers)
                {
                    var userId = trainer.Trainer.UserId;

                    var user = await db.User.FirstOrDefaultAsync(x => x.Display == true && x.Id == userId);

                    if (user != null && user.UserAccount.IsEnable)
                    {
                        ids.Add(user.Id);
                    }
                }
            }

            return ids.Distinct().ToList();
        }
    }
}