using FEP.Intranet;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using NonActionAttribute = System.Web.Http.NonActionAttribute;
using System.Web;
using System.Threading.Tasks;
using FEP.WebApiModel.Notification;

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

        public string GetDisplayNameEventType(Enum val)
        {
            return ((DisplayAttribute)val.GetType()
                .GetMember(Enum.GetName(typeof(SLAEventType), val).ToString())
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

            foreach (var item in model)
            {
                item.StagesName = GetDisplayName(item.NotificationType);
                item.SLAEventTypeName = GetDisplayNameEventType(item.SLAEventType);
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
            if (getNotifyTpe == null)
            {
                return Ok();
            }
            var paramList = db.ParameterGroup.Where(p => p.SLAEventType == getNotifyTpe.SLAEventType).ToList();

            List<TemplateParameterType> paramTypeList = new List<TemplateParameterType>();
            foreach (var item in paramList)
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
        public BulkNotification RegisterBulkNotificationGroupFunc(BulkNotificationModel request)
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
        [HttpGet]
        // POST: api/SLAReminder
        public IHttpActionResult StopNotification(int SLAReminderStatusId)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest();
            }*/

            //get list of notification related to the same Email reminder
            var NotificationList = db.BulkNotification.Where(b => b.SLAReminderStatusId == SLAReminderStatusId).ToList();
            foreach (var item in NotificationList)
            {
                if (item.NotificationMedium == NotificationMedium.Email)
                {
                    // --> CALL EMail API utk delete future email
                    var responEmail = StopEmailUsingAPI(item.NotificationId);
                }
                if (item.NotificationMedium == NotificationMedium.SMS)
                {
                    // --> CALL SMS API utk delete future SMS
                    var responseSMS = StopSMSUsingAPI(item.NotificationId);
                }
                if (item.NotificationMedium == NotificationMedium.Web)
                {
                    // --> CALL WEB API utk delete future WEB MSG
                    var responseWebNotify = StopWebNotifyUsingAPI(item.NotificationId);
                }
            }

            List<SLAReminderStatus> SLAList = db.SLAReminderStatus.Where(s => s.Id == SLAReminderStatusId).ToList();
            foreach (var sla in SLAList)
            {
                sla.NotificationReminderStatusType = NotificationReminderStatusType.Closed;
                sla.closeDate = DateTime.Now;

                db.Entry(sla).State = EntityState.Modified;
                db.Entry(sla).Property(x => x.NotificationReminderStatusType).IsModified = true;
                db.Entry(sla).Property(x => x.closeDate).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = true;
                db.SaveChanges();
            }
            return Ok(NotificationList);
        }

        [HttpPut]
        // PUT: api/SLAReminder/5
        public IHttpActionResult Put(int id, EditSLAReminderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
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
        public async Task<IHttpActionResult> GenerateAutoNotificationReminder(CreateAutoReminder reminder)
        {
            List<DateTime> ScheduleMessage = GetSLAReminder(reminder.NotificationType, reminder.StartNotificationDate);
            var template = db.NotificationTemplates.Where(t => t.NotificationType == reminder.NotificationType).FirstOrDefault();
            int SLAReminderId = 0;

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
                SLAReminderId = responseStartNotification.Id;

                if (template.enableEmail)
                {
                    string emailSubject = generateBodyMessage(template.TemplateSubject, reminder.NotificationType, reminder.ParameterListToSend);
                    string emailBody = generateBodyMessage(template.TemplateMessage, reminder.NotificationType, reminder.ParameterListToSend);

                    //send email ke setiap reciever
                    foreach (var receiver in reminder.ReceiverId)
                    {
                        string receiverEmailAddress = db.User.Find(receiver).Email;
                        int counter = 1;
                        //send notification mengikut jadual
                        foreach (var notifyDate in ScheduleMessage)
                        {
                            // --> CALL EMAIL API HERE---
                            //                          |   send received notificationId here
                            //                         \|/
                            var response = await sendEmailUsingAPIAsync(notifyDate, (int)reminder.NotificationCategory, (int)reminder.NotificationType, receiverEmailAddress, emailSubject, emailBody, counter);
                            if (response != null)
                            {
                                string EmailNotificationId = response.datID; //assumed returned Id
                                // --> CALL insert BulkNotificationGroup API (NotificationMedium : Email, int [SLAReminderStatusId])
                                BulkNotificationModel objEmailNotification = new BulkNotificationModel
                                {
                                    SLAReminderStatusId = SLAReminderId,
                                    NotificationMedium = NotificationMedium.Email,
                                    NotificationId = EmailNotificationId
                                };

                                var responseEmailNotificationGroup = RegisterBulkNotificationGroupFunc(objEmailNotification);
                            }
                            counter++;
                        }
                    }
                }

                if (template.enableSMSMessage)
                {
                    //send sms ke setiap reciever
                    foreach (var receiver in reminder.ReceiverId)
                    {
                        string receiverPhoneNo = db.User.Find(receiver).MobileNo;
                        int counter = 1;
                        foreach (var notifyDate in ScheduleMessage)
                        {
                            string SMSToSend = generateSMSMessage(template.SMSMessage, template.NotificationType, reminder.ParameterListToSend);
                            // --> CALL SMS API HERE-----
                            //                          |   send received notificationId here
                            //                         \|/
                            var response = await sendSMSUsingAPIAsync(notifyDate, (int)reminder.NotificationCategory, (int)reminder.NotificationType, receiverPhoneNo, null, SMSToSend, counter);
                            if (response != null)
                            {
                                string SMSNotificationId = response.datID; //assumed returned Id
                                // --> CALL insert BulkNotificationGroup API (NotificationMedium : SMS, [SLAReminderStatusId])
                                BulkNotificationModel objSMSNotification = new BulkNotificationModel
                                {
                                    SLAReminderStatusId = SLAReminderId,
                                    NotificationMedium = NotificationMedium.SMS,
                                    NotificationId = SMSNotificationId
                                };

                                var responseSMSNotificationGroup = RegisterBulkNotificationGroupFunc(objSMSNotification);
                            }
                            counter++;
                        }
                    }
                }

                if (template.enableWebMessage)
                {
                    foreach (var receiver in reminder.ReceiverId)
                    {
                        int counter = 1;

                        foreach (var notifyDate in ScheduleMessage)
                        {
                            string WebTextToSend = generateWEBMessage(template.WebMessage, template.NotificationType, reminder.ParameterListToSend);
                            string WebLinkTextToSend = generateWEBLinkMessage(template.WebNotifyLink, template.NotificationType, reminder.ParameterListToSend);
                            // --> CALL WEB API HERE-----
                            //                          |   send received notificationId here
                            //                         \|/
                            CreateNotificationModel model = new CreateNotificationModel
                            {
                                UserId = receiver,
                                NotificationType = reminder.NotificationType,
                                Category = reminder.NotificationCategory,
                                Message = WebTextToSend,
                                Link = WebLinkTextToSend,
                                SendDate = notifyDate
                            };
                            var response = await sendWebNotifyAPI(model);
                            if (response != -1)
                            {
                                string WEBNotificationId = response.ToString(); //assumed returned Id
                                // --> CALL insert BulkNotificationGroup API (NotificationMedium : Web, [SLAReminderStatusId])
                                BulkNotificationModel objWEBNotification = new BulkNotificationModel
                                {
                                    SLAReminderStatusId = SLAReminderId,
                                    NotificationMedium = NotificationMedium.Web,
                                    NotificationId = WEBNotificationId
                                };
                                var responseWEBNotificationGroup = RegisterBulkNotificationGroup(objWEBNotification);
                            }
                            
                            counter++;
                        }
                    }
                    
                }

                ReminderResponse result = new ReminderResponse
                {
                    Status = "Success",
                    SLAReminderStatusId = SLAReminderId
                };
                return Ok(result);
            }
            else
            {
                ReminderResponse result = new ReminderResponse
                {
                    Status = "Failed",
                    SLAReminderStatusId = SLAReminderId
                };
                return Ok(result);
            }
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
            int numberOrReminder = 0;
            if (SLAReminder.SLADurationType == SLADurationType.Days)
                numberOrReminder = SLAReminder.SLAResolutionTime;
            else if (SLAReminder.SLADurationType == SLADurationType.Hours)
                numberOrReminder = (SLAReminder.SLAResolutionTime * 24) / SLAReminder.IntervalDuration;

            for (int i = 0; i < numberOrReminder; i++)
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
        public string generateWEBLinkMessage(string WebLinkText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WEBLinkTextToSend = WebLinkText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WEBLinkTextToSend = WEBLinkTextToSend.Replace(textToReplace, theValue);
            }
            return WEBLinkTextToSend;
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

        [NonAction]
        public async Task<EmailClass> sendEmailUsingAPIAsync
            (DateTime emailDate, int notifyCategory, int notifyType, string emailAddress, string emailSubject, string emailBody, int counter)
        {
            DateTime myTimeNow = DateTime.Now;
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            EmailClass emailObj = new EmailClass
            {
                datID = emailAddress + "-email" + counter + "-" + epoch.ToString(),
                datType = notifyCategory,
                datNotify = notifyType,
                dtInsert = myTimeNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtSchedule = emailDate.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtExpired = emailDate.AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                emailTo = emailAddress,
                subject = HttpUtility.HtmlDecode(emailSubject),
                body = HttpUtility.HtmlDecode(emailBody)
            };
            var response = await WepApiMethod.SendApiAsync<EmailClass>
                (HttpVerbs.Post, $"BulkEmail", emailObj, WepApiMethod.APIEngine.EmailSMSAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return null;
        }
        
        [NonAction]
        public async Task<long> sendWebNotifyAPI (CreateNotificationModel model)
        {
            var response = await WepApiMethod.SendApiAsync<long>
                (HttpVerbs.Post, $"System/Notification", model, WepApiMethod.APIEngine.IntranetAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return -1;
        }

        [NonAction]
        public async Task<SMSClass> sendSMSUsingAPIAsync
            (DateTime smsDate, int notifyCategory, int notifyType, string phoneNo, string smsSubject, string smsBody, int counter)
        {
            DateTime myTimeNow = DateTime.Now;
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            SMSClass smsObj = new SMSClass
            {
                datID = phoneNo + "-sms" + counter + "-" + epoch.ToString(),
                datType = notifyCategory,
                datNotify = notifyType,
                dtInsert = myTimeNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtSchedule = smsDate.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtExpired = smsDate.AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                smsTo = phoneNo,
                subject = smsSubject,
                body = smsBody
            };
            var response = await WepApiMethod.SendApiAsync<SMSClass>
                (HttpVerbs.Post, $"BulkSMS", smsObj, WepApiMethod.APIEngine.EmailSMSAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return null;
        }

        [NonAction]
        public async Task<EmailClass> StopEmailUsingAPI(string emailId)
        {
            var response = await WepApiMethod.SendApiAsync<EmailClass>
                (HttpVerbs.Delete, $"BulkEmail/" + emailId, null, WepApiMethod.APIEngine.EmailSMSAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return null;
        }

        [NonAction]
        public async Task<EmailClass> StopSMSUsingAPI(string smsId)
        {
            var response = await WepApiMethod.SendApiAsync<EmailClass>
                (HttpVerbs.Delete, $"BulkSMS/" + smsId, null, WepApiMethod.APIEngine.EmailSMSAPI);

			if (response.isSuccess)
				return response.Data;
			else
				return null;
		}

        [NonAction]
        public async Task<bool> StopWebNotifyUsingAPI(string webNotifyId)
        {
            var response = await WepApiMethod.SendApiAsync<bool>
                (HttpVerbs.Delete, $"System/Notification?id={webNotifyId}");

            if (response.isSuccess)
                return response.Data;
            else
                return false;
        }
        //--------------------------------------------------------------------------------------------------
    }
}
