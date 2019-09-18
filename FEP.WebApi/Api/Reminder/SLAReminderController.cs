using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FEP.WebApi.Api.Reminder
{
    [Route("api/Reminder/SLA")]
    public class SLAReminderController : ApiController
    {
        private DbEntities db = new DbEntities();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetDisplayName(Enum val)
        {
            return ((DisplayAttribute)val.GetType()
                .GetMember(Enum.GetName(typeof(NotificationType), val).ToString())
                                .First()
                                .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
        }
        // GET: api/SLAReminder
        [HttpGet]
        public IHttpActionResult Get()
        {
            var model = db.SLAReminder.Select(s => new SLAReminderModel
            {
                Id = s.Id,
                SLAEventType = s.SLAEventType,
                NotificationType = s.NotificationType,
                ETCode = s.ETCode,
                SLAResolutionTime = s.SLAResolutionTime,
                IntervalDuration = s.IntervalDuration,
                SLADurationType = s.SLADurationType,

            }).ToList();

            foreach(var item in model)
            {
                item.StagesName = GetDisplayName(item.NotificationType);
            }

            return Ok(model);
        }

        // GET: api/SLAReminder/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/Reminder/SLA/GetParameterList")]
        [HttpGet]
        public IHttpActionResult GetParameterList(int id)
        {
            var getNotifyTpe = db.SLAReminder.Where(n => (int)n.NotificationType == id).FirstOrDefault();
            var paramList = db.ParameterGroup.Where(p => p.SLAEventType == getNotifyTpe.SLAEventType).ToList();

            List<TemplateParameterType> paramTypeList = new List<TemplateParameterType>();
            foreach(var item in paramList)
            {
                paramTypeList.Add(item.TemplateParameterType);
            }

            return Ok(paramTypeList);
        }

        [NonAction]
        public SLAReminderStatus StartNotificationFunc(CreateSLAReminderStatusModel request)
        {
            SLAReminderStatus model = new SLAReminderStatus
            {
                NotificationType = request.NotificationType,
                NotificationReminderStatusType = request.NotificationReminderStatusType,
                StartDate = request.StartDate
            };
            db.SLAReminderStatus.Add(model);
            db.SaveChanges();

            return model;
        }

        [Route("api/Reminder/SLA/StartNotification")]
        [HttpPost]
        // POST: api/SLAReminder
        public IHttpActionResult StartNotification(CreateSLAReminderStatusModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var model = StartNotificationFunc(request);
            return Ok(model);

        }
        
        [NonAction]
        public BulkNotification RegisterBulkNotificationGroupFunc (BulkNotificationModel request)
        {
            BulkNotification model = new BulkNotification
            {
                NotificationMedium = request.NotificationMedium,
                SLAReminderStatusId = request.SLAReminderStatusId,
                NotificationId = request.NotificationId
            };
            db.BulkNotification.Add(model);
            db.SaveChanges();

            return model;
        }

        [Route("api/Reminder/SLA/RegisterBulkNotificationGroup")]
        [HttpPost]
        public IHttpActionResult RegisterBulkNotificationGroup(BulkNotificationModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var model = RegisterBulkNotificationGroupFunc(request);
            return Ok(model);

        }

        [Route("api/Reminder/SLA/StopNotification")]
        [HttpPut]
        // POST: api/SLAReminder
        public IHttpActionResult StopNotification(int NotificationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            BulkNotification BN = db.BulkNotification.Where(b => b.NotificationId == NotificationId).FirstOrDefault();
            
            //get list of notification related to the same Email reminder
            var NotificationList = db.BulkNotification.Where(b => b.SLAReminderStatusId == BN.SLAReminderStatusId).ToList();
            foreach(var item in NotificationList)
            {
                if(item.NotificationMedium == NotificationMedium.Email)
                {
                    // --> CALL EMail API utk delete future email
                }
                if (item.NotificationMedium == NotificationMedium.SMS)
                {
                    // --> CALL SMS API utk delete future SMS
                }
                if (item.NotificationMedium == NotificationMedium.Web)
                {
                    // --> CALL WEB API utk delete future WEB MSG
                }
            }

            SLAReminderStatus SLA = db.SLAReminderStatus.Where(s => s.Id == BN.SLAReminderStatusId).FirstOrDefault();
            SLA.NotificationReminderStatusType = NotificationReminderStatusType.Closed;
            SLA.closeDate = DateTime.Now;

            db.Entry(SLA).State = EntityState.Modified;
            db.Entry(SLA).Property(x => x.NotificationReminderStatusType).IsModified = true;
            db.Entry(SLA).Property(x => x.closeDate).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(SLA);

        }

        [HttpPut]
        // PUT: api/SLAReminder/5
        public IHttpActionResult Put(int id, EditSLAReminderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(id != model.Id)
            {
                return BadRequest();
            }

            SLAReminder obj = db.SLAReminder.Find(model.Id);
            obj.ETCode = model.ETCode;
            obj.IntervalDuration = model.IntervalDuration;
            obj.SLADurationType = model.SLADurationType;
            obj.SLAResolutionTime = model.SLAResolutionTime;

            db.Entry(obj).State = EntityState.Modified;
            db.Entry(obj).Property(x => x.ETCode).IsModified = true;
            db.Entry(obj).Property(x => x.IntervalDuration).IsModified = true;
            db.Entry(obj).Property(x => x.SLADurationType).IsModified = true;
            db.Entry(obj).Property(x => x.SLAResolutionTime).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/SLAReminder/5
        public void Delete(int id)
        {
        }

        //--------------------------------------------------------------------------------------------------
        //                  BIG CHUNK OF CODE TO AUTOMATE SLA REMINDER
        //--------------------------------------------------------------------------------------------------
        //1 create ParamListToSend
        //2 generate body message
        //3 generate subject message
        //4 generate schedule to send email
        //5 call email API
        [Route("api/Reminder/SLA/GenerateAutoNotificationReminder")]
        [HttpPost]
        public IHttpActionResult GenerateAutoNotificationReminder(CreateAutoReminder reminder)
        {
            List<DateTime> ScheduleMessage = GetSLAReminder(reminder.NotificationType, reminder.StartNotificationDate);
            var template = db.NotificationTemplates.Where(t => t.NotificationType == reminder.NotificationType).FirstOrDefault();

            // --> CALL StartNotification API (register SLAReminder) -> return [SLAReminderStatusId]
            CreateSLAReminderStatusModel objReminder = new CreateSLAReminderStatusModel
            {
                NotificationType = reminder.NotificationType,
                NotificationReminderStatusType = NotificationReminderStatusType.Open,
                StartDate = reminder.StartNotificationDate
            };
            var responseStartNotification = StartNotificationFunc(objReminder);
            /*var responseStartNotification = await WepApiMethod.SendApiAsync<CreateSLAReminderStatusModel>
                (HttpVerbs.Post, $"Reminder/SLA/StartNotification/", objReminder);*/

            if (responseStartNotification != null)
            {
                var SLAReminderId = responseStartNotification.Id;

                if (template.enableEmail)
                {
                    string emailSubject = generateBodyMessage(template.TemplateSubject, reminder.NotificationType, reminder.ParameterListToSend);
                    string emailBody = generateBodyMessage(template.TemplateMessage, reminder.NotificationType, reminder.ParameterListToSend);

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

                        var responseEmailNotificationGroup = RegisterBulkNotificationGroupFunc(objEmailNotification);

                        /*var responseEmailNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objEmailNotification);*/
                    }

                }

                if (template.enableSMSMessage)
                {
                    foreach (var notifyDate in ScheduleMessage)
                    {
                        string SMSToSend = generateSMSMessage(template.SMSMessage, template.NotificationType, reminder.ParameterListToSend);
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

                        var responseSMSNotificationGroup = RegisterBulkNotificationGroupFunc(objSMSNotification);

                        /*var responseSMSNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objSMSNotification);*/
                    }
                }

                if (template.enableWebMessage)
                {
                    foreach (var notifyDate in ScheduleMessage)
                    {
                        string WebTextToSend = generateWEBMessage(template.WebMessage, template.NotificationType, reminder.ParameterListToSend);
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

                        var responseWEBNotificationGroup = RegisterBulkNotificationGroup(objWEBNotification);

                        /*var responseWEBNotificationGroup = await WepApiMethod.SendApiAsync<BulkNotificationModel>
                            (HttpVerbs.Post, $"Reminder/SLA/RegisterBulkNotificationGroup/", objWEBNotification);*/
                    }
                }
            }

            return Ok("success");
        }
        [NonAction]
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
        [NonAction]
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

        [NonAction]
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
        [NonAction]
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
        [NonAction]
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
        [NonAction]
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

        //--------------------------------------------------------------------------------------------------
    }
}
