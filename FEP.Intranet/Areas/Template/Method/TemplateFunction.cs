using FEP.Helper;
using FEP.Intranet.Areas.Template.Models;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Template.Method
{
    public class TemplateFunction : FEPController
    {
        private DbEntities db = new DbEntities();

        public async void GenerateAutoNotificationReminder(NotificationType NotificationType, 
            ParameterListToSend paramToSend, DateTime NotificationStartDate)
        {
            List<DateTime> ScheduleMessage = GetSLAReminder(NotificationType, NotificationStartDate);
            var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType).FirstOrDefault();

            // --> CALL StartNotification API (register SLAReminder) -> return [SLAReminderStatusId]
            CreateSLAReminderStatusModel objReminder = new CreateSLAReminderStatusModel
            {
                NotificationType = NotificationType,
                NotificationReminderStatusType = NotificationReminderStatusType.Open,
                StartDate = NotificationStartDate
            };
            var responseStartNotification = await WepApiMethod.SendApiAsync<CreateSLAReminderStatusModel>
                (HttpVerbs.Post, $"Reminder/SLA/StartNotification/", objReminder);

            if (responseStartNotification.isSuccess)
            {
                var SLAReminderId = responseStartNotification.Data.Id;

                if (template.enableEmail)
                {
                    string emailSubject = generateBodyMessage(template.TemplateSubject, NotificationType, paramToSend);
                    string emailBody = generateBodyMessage(template.TemplateMessage, NotificationType, paramToSend);

                    //send notification mengikut jadual
                    foreach (var notifyDate in ScheduleMessage)
                    {
                        // --> CALL EMAIL API HERE---
                        //                          |   send received notificationId here
                        //                         \|/
                        int EmailNotificationId = 100; //assumed returned Id
                                                       // --> CALL insert BulkNotificationGroup API (NotificationMedium : Email, int [SLAReminderStatusId])
                        BulkNotificationModel objEmailNotification = new BulkNotificationModel
                        {
                            SLAReminderStatusId = SLAReminderId,
                            NotificationMedium = NotificationMedium.Email,
                            NotificationId = EmailNotificationId
                        };

                        var responseEmailNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objEmailNotification);
                    }

                }

                if (template.enableSMSMessage)
                {
                    foreach (var notifyDate in ScheduleMessage)
                    {
                        string SMSToSend = generateSMSMessage(template.SMSMessage, template.NotificationType, paramToSend);
                        // --> CALL SMS API HERE-----
                        //                          |   send received notificationId here
                        //                         \|/
                        int SMSNotificationId = 101; //assumed returned Id
                                                     // --> CALL insert BulkNotificationGroup API (NotificationMedium : SMS, [SLAReminderStatusId])
                        BulkNotificationModel objSMSNotification = new BulkNotificationModel
                        {
                            SLAReminderStatusId = SLAReminderId,
                            NotificationMedium = NotificationMedium.SMS,
                            NotificationId = SMSNotificationId
                        };
                        var responseSMSNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objSMSNotification);
                    }
                }

                if (template.enableWebMessage)
                {
                    foreach (var notifyDate in ScheduleMessage)
                    {
                        string WebTextToSend = generateWEBMessage(template.WebMessage, template.NotificationType, paramToSend);
                        // --> CALL WEB API HERE-----
                        //                          |   send received notificationId here
                        //                         \|/
                        int WEBNotificationId = 102; //assumed returned Id
                                                     // --> CALL insert BulkNotificationGroup API (NotificationMedium : Web, [SLAReminderStatusId])
                        BulkNotificationModel objWEBNotification = new BulkNotificationModel
                        {
                            SLAReminderStatusId = SLAReminderId,
                            NotificationMedium = NotificationMedium.Web,
                            NotificationId = WEBNotificationId
                        };
                        var responseWEBNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objWEBNotification);
                    }
                }
            }

            
        }

        public List<DateTime> GetSLAReminder(NotificationType NotificationType, DateTime StartTime)
        {
            var SLAReminder = db.SLAReminder.Where(s => s.NotificationType == NotificationType).FirstOrDefault();
            List<DateTime> reminderList = new List<DateTime>();
            DateTime NewTime = StartTime;
            DateTime TempTime;
            reminderList.Add(StartTime);
            TempTime = StartTime;
            for (int i = 0; i < SLAReminder.SLAResolutionTime; i++)
            {
                if (SLAReminder.SLADurationType == SLADurationType.Hours)
                {
                    NewTime = TempTime.AddHours(SLAReminder.IntervalDuration);
                }
                else if (SLAReminder.SLADurationType == SLADurationType.Days)
                {
                    NewTime = TempTime.AddDays(SLAReminder.IntervalDuration);
                }
                reminderList.Add(NewTime);
                TempTime = NewTime;
            }

            return reminderList;
        }

        public string GetPropertyValues(Object obj, string propertyName)
        {
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            string value = "";
            foreach (var prop in props)
                if (prop.Name == propertyName)
                {
                    value = (prop.GetValue(obj))?.ToString();
                    break;
                }
                else
                    value = "";

            return value;
        }

        public string generateSMSMessage(string SMSText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string SMSToSend = SMSText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                SMSToSend = SMSToSend.Replace(textToReplace, theValue);
            }
            return SMSToSend;
        }

        public string generateWEBMessage(string WebText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WEBTextToSend = WebText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WEBTextToSend = WEBTextToSend.Replace(textToReplace, theValue);
            }
            return WEBTextToSend;
        }

        public string generateBodyMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }

        public string generateSubjectMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }
    }
}