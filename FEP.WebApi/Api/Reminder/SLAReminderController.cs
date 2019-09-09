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

        [Route("api/Reminder/SLA/StartNotification")]
        [HttpPost]
        // POST: api/SLAReminder
        public IHttpActionResult StartNotification(CreateSLAReminderStatusModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            SLAReminderStatus model = new SLAReminderStatus
            {
                NotificationId = request.NotificationId,
                NotificationType = request.NotificationType,
                NotificationReminderStatusType = request.NotificationReminderStatusType
            };
            db.SLAReminderStatus.Add(model);
            db.SaveChanges();

            return Ok(model);

        }

        [Route("api/Reminder/SLA/UpdateNotificationStatus")]
        [HttpPut]
        public IHttpActionResult UpdateNotificationStatus(EditSLAReminderStatusModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var model = db.SLAReminderStatus.Where(s => s.NotificationId == request.NotificationId).FirstOrDefault();
            model.NotificationReminderStatusType = request.NotificationReminderStatusType;

            db.Entry(model).State = EntityState.Modified;
            db.Entry(model).Property(x => x.NotificationReminderStatusType).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
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
    }
}
