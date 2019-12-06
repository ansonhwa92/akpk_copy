using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FEP.Helper;
using FEP.WebApiModel.eLearning;
using FEP.Model.eLearning;

using FEP.Helper;

using System.Collections.Generic;

namespace FEP.Intranet.Areas.eLearning.Helper
{
    public class EmailHelperApiUrl
    {
        public static string SendNotification = "eLearning/Notification/SendNotification";
    }

    public static class EmaiHelper
    {
        // Notification type determine the object
        public static async Task<ReminderResponse> SendNotification(NotificationModel model)
        {
            try
            {
                var response = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, EmailHelperApiUrl.SendNotification, model);
                if (response.isSuccess)
                {
                    return response.Data;
                }

                await FEPHelperMethod.LogError(Modules.Learning, model.SenderId, "", "", "eLearning", "",
                    "Sending Email", $"Error sending Email For {model.NotificationType.ToString()} " +
                        $"Title : " + $"{model.ParameterListToSend.CourseTitle}");

                return response.Data;
            }
            catch (Exception e)
            {
                await FEPHelperMethod.LogError(Modules.Learning, model.SenderId, "", "", "eLearning", "",
                    "Sending Email", $"Error sending Email For {model.NotificationType.ToString()} " +
                    $"Title : " + $"{model.ParameterListToSend.CourseTitle} {e.Message} {e.InnerException}");

                return new ReminderResponse
                {
                    Status = "Failed"
                };
            }
        }
    }

    public static class Notifier
    {
        public async static Task UserWithdrawFromCourse(int id, int senderId,
            string receiverEmails = "", string userName = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = NotificationType.Notify_Admin_Participant_Withdraw,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseTitle = "",
                    LearnerName = userName,
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                IsNeedRemainder = false,
                Emails = receiverEmails,
                SenderId = senderId,
            };
            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Notify_Self_Withdraw_From_Course;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task SubmitCourseForVerication(int id, int senderId,
            string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = NotificationType.Verify_Courses_Creation,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Verification",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = true,
            };

            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Verify_Courses_Creation_Self;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task SubmitCourseForApproval(NotificationType notifyType, int id, int senderId,
            string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = notifyType,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Approval",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = true,
            };

            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Approve_Courses_Creation_Approver_Self;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task SubmitCourseForAmendment(NotificationType notifyType, int id, int senderId,
                string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = notifyType,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Amendment",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = false,
            };

            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Course_Amendment_Self;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task SubmitCourseApproved(NotificationType notifyType, int id, int senderId,
            string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = notifyType,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Approved",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = false,
            };

            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Course_Approved_Others;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);

            notifyModel.NotificationType = NotificationType.Course_Approved_Self;
            notifyModel.SenderId = senderId;
            notifyModel.IsNeedRemainder = false;
            notifyModel.ParameterListToSend.Link = FEPHelperMethod.GetBaseURL() + $"/eLearning/Courses/{id}";

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task NotifyCoursePublish(NotificationType notifyType, int id, int senderId,
            string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = notifyType,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Published",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = false,
            };

            await EmaiHelper.SendNotification(notifyModel);
        }

        public async static Task NotifyCourseCancelled(NotificationType notifyType, int id, int senderId,
            string receiverEmails = "", string author = "", string title = "", string link = "")
        {
            var notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = notifyType,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseAuthor = author,
                    CourseTitle = title,
                    CourseApproval = "Course Cancelled",
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                SenderId = senderId,
                Emails = receiverEmails,
                IsNeedRemainder = false,
            };

            await EmaiHelper.SendNotification(notifyModel);
        }
    }
}