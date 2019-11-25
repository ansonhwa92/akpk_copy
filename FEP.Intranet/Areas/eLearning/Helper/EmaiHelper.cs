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

                await new FEPController().LogError(Modules.Learning, $"Error sending Email For {model.NotificationType.ToString()} " +
                    $"Title : " + $"{model.ParameterListToSend.CourseTitle}");

                return response.Data;
            }
            catch (Exception e)
            {
                await new FEPController().LogError(Modules.Learning, $"Error sending Email For {model.NotificationType.ToString()} " +
                    $"Title : " + $"{model.ParameterListToSend.CourseTitle}");

                return new ReminderResponse
                {
                    Status = "Failed"
                };
            }
        }
    }

    public static class Notifier
    {
        public async static Task UserWithdrawFromCourse(int id, int userId, string userName = "", string link = "")
        {
            // Notify Admin
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
                IsNeedRemainder = false
            };

            var emailResponse = await EmaiHelper.SendNotification(notifyModel);

            if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
            {
                await FEPHelperMethod.LogError(Modules.Learning, userId,
                    "RequestWithdraw", "CourseEnrollment", "eLearning", null, "Sending Email",
                    $"Error Sending Email To Admin For Course Withdrawal. User - {userName}, CourseId {id}");
            }

            // Notify self?
            notifyModel = new NotificationModel
            {
                Id = id,
                Type = typeof(Course),
                NotificationType = NotificationType.Notify_Self_Withdraw_From_Course,
                NotificationCategory = NotificationCategory.Learning,
                StartNotificationDate = DateTime.Now,
                ParameterListToSend = new ParameterListToSend
                {
                    CourseTitle = "",
                    LearnerName = userName,
                    Link = link,
                },
                ReceiverType = ReceiverType.UserIds,
                LearnerUserId = userId,
                IsNeedRemainder = false
            };

            emailResponse = await EmaiHelper.SendNotification(notifyModel);

            if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
            {
                await FEPHelperMethod.LogError(Modules.Learning, userId,
                    "RequestWithdraw", "CourseEnrollment", "eLearning", null, "Sending Email",
                    $"Error Sending Email To Learner For Course Withdrawal. User - {userName}, CourseId {id}");
            }
        }
    }
}