using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Notification
{
    [Route("api/Notification/Email")]
    public class EmailController : ApiController
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

        [Route("api/Notification/Email/SendEmailToRecepient")]
        [HttpPost]
        public IHttpActionResult Post(EmailRecipientModel model)
        {
            if (model.SendDate == null) model.SendDate = DateTime.Now;

            if (model.To == null)
            {
                model.To = new EmailAddress();
            }

            if (model.UserId != null)
            {
                var user = db.User.Where(u => u.Id == model.UserId).FirstOrDefault();

                model.To.Address = user.Email;
                model.To.DisplayName = user.Name;
            }

            if (!string.IsNullOrEmpty(model.To.Address))
            {
                var EmailToSend = new EmailToSend
                {
                    Body = model.Body,
                    Subject = model.Subject,
                    SendDate = model.SendDate.Value,
                    IsSent = false,
                    CreatedDate = DateTime.Now,
                    EmailAddress = new List<EmailToSendAddress>(new EmailToSendAddress[] { new EmailToSendAddress { EmailAddress = model.To.Address, DisplayName = model.To.DisplayName } }),
                    Retry = 0
                };

                db.EmailToSend.Add(EmailToSend);
                db.SaveChanges();

                return Ok(EmailToSend.Id);
            }

            return NotFound();
        }

        [Route("api/Notification/Email/SendEmailToRecepients")]
        [HttpPost]
        public IHttpActionResult Post(EmailRecipientsModel model)
        {
            if (model.SendDate == null) model.SendDate = DateTime.Now;

            if (model.To == null)
            {
                model.To = new List<EmailAddress>();
            }

            if (model.UserId != null)
            {

                foreach (int recipientid in model.UserId)
                {
                    var user = db.User.Where(u => u.Id == recipientid).FirstOrDefault();

                    if (user != null)
                    {
                        model.To.Add(new EmailAddress { Address = user.Email, DisplayName = user.Name });
                    }
                }

            }

            var recipients = model.To.Select(t => new EmailToSendAddress { EmailAddress = t.Address, DisplayName = t.DisplayName }).ToList();

            if (recipients.Count > 0)
            {
                var EmailToSend = new EmailToSend
                {
                    Body = model.Body,
                    Subject = model.Subject,
                    SendDate = model.SendDate.Value,
                    IsSent = false,
                    CreatedDate = DateTime.Now,
                    EmailAddress = recipients,
                    Retry = 0
                };

                db.EmailToSend.Add(EmailToSend);
                db.SaveChanges();

                return Ok(EmailToSend.Id);
            }

            return NotFound();
        }

    }
}
