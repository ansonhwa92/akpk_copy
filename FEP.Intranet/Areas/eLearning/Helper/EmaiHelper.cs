using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FEP.Helper;
using FEP.WebApiModel.eLearning;

namespace FEP.Intranet.Areas.eLearning.Helper
{
    public class EmailHelperApiUrl
    {
        public static string SendNotification = "eLearning/Notification/SendNotification";
    }

    public static class EmaiHelper
    {
 
        // Notification type determine the object
        // If no receivers defined in parameterlisttosend, the api will try to find accoring to notificationtype
      
        public static async Task<ReminderResponse> SendNotification(NotificationModel model)
        {
            try
            {
                var response = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, EmailHelperApiUrl.SendNotification, model);
                if (response.isSuccess)
                {
                    return response.Data;
                }

                await new FEPController().LogError(Modules.Learning, $"Error sending Email For {model.NotificationType.GetDisplayName()} " +
                    $"Title : " +  $"{model.ParameterListToSend.CourseTitle}");

                return response.Data;
            }
            catch(Exception e)
            {
                await new FEPController().LogError(Modules.Learning, $"Error sending Email For {model.NotificationType.GetDisplayName()} " +
                    $"Title : " + $"{model.ParameterListToSend.CourseTitle}");

                return new ReminderResponse
                {
                    Status = "Failed"
                };
            }
        }
    }
}