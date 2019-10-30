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
                // find receivers
                if (model.ReceiverId == null || model.ReceiverId.Count() > 0)
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

                        default:
                            break;
                    }

                    model.ReceiverId = receivers;
                }
            }

            if (model.Type == typeof(Course))
            {
                var course = await db.Courses.FindAsync(model.Id);

                if (course != null)
                {
                    model.ParameterListToSend.CourseTitle = course.Title;
                }
            }

            CreateAutoReminder createdAutoReminder = new CreateAutoReminder
            {
                NotificationCategory = model.NotificationCategory,
                NotificationType = model.NotificationType,
                ParameterListToSend = model.ParameterListToSend,
                ReceiverId = model.ReceiverId,
                StartNotificationDate = DateTime.Now
            };

            //var emailToSend = new CourseEmailQueue
            //{
            //    NotificationCategory = model.NotificationCategory.ToString(),
            //    NotificationType = model.NotificationType.ToString(),
            //    CourseId = model.Id,
            //    Parameters = JsonConvert.SerializeObject(model.ParameterListToSend),
            //    Receivers = model.ReceiverId.ToString(),
            //};

            //db.CourseEmailQueue.Add(emailQueue);

            await db.SaveChangesAsync();

            //return Ok();

            // TEMPORARILY DISABLE BELOW BECAUSE IT REQUIRES CONNECTION TO THE EMAIL SERVER WHICH WILL
            // SLOW DOWN THE TESTING.. SO..., TO ENABLE comment Return Ok above

            var controller = new SLAReminderController();

            try
            {
                if (model.IsNeedRemainder)
                {
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

                    if(response == null)
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

            return BadRequest();
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
    }
}