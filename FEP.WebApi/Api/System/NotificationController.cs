using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Systems
{
    [Route("api/System/Notification")]
    public class NotificationController : ApiController
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

        [Route("api/System/Notification/GetAll")]
        [HttpPost]
        public IHttpActionResult Post([FromUri] int userId, FilterNotificationModel request)
        {

            var query = db.Notification.Where(n => n.UserId == userId).Select(s => new NotificationModel
            {
                Id = s.Id,
                Category = s.Category,
                NotificationType = s.NotificationType,
                Message = s.Message,
                Link = s.Link,
                CreatedDate = s.SendDate,
                IsRead = s.IsRead
            });

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Message == null || s.Message.Contains(request.Message))
               && (request.Category == null || s.Category == request.Category)              
               && (request.DateFrom == null || request.DateTo == null || DbFunctions.TruncateTime(s.CreatedDate) <= DbFunctions.TruncateTime(request.DateTo) && DbFunctions.TruncateTime(s.CreatedDate) >= DbFunctions.TruncateTime(request.DateFrom))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Message.Contains(value)               
                );
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Message":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Message);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Message);
                        }

                        break;

                    case "CategoryDesc":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Category);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Category);
                        }

                        break;

                    case "CreatedDate":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CreatedDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CreatedDate);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.CreatedDate);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.CreatedDate);
            }

            var data = query.Skip(request.start).Take(request.length).ToList();

            data.ForEach(s =>
            {
                s.CategoryDesc = s.Category.GetDisplayName();
                s.Link = s.Link == null ? "" : s.Link;
            });

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        [HttpGet]
        public IHttpActionResult Get(int userId)
        {
            var notifications = db.Notification.Where(u => u.UserId == userId && !u.IsRead && u.SendDate <= DateTime.Now)
                .Select(s => new NotificationModel
                {
                    Id = s.Id,
                    Category = s.Category,
                    NotificationType = s.NotificationType,
                    Message = s.Message,
                    Link = s.Link,
                    CreatedDate = s.SendDate,
                    IsRead = s.IsRead
                })
                .OrderByDescending(o => o.CreatedDate).Take(5).ToList();
                
            return Ok(notifications);
        }
         
        [HttpGet]
        public IHttpActionResult Get(long id)
        {
            var notification = db.Notification.Where(u => u.Id == id)
                .Select(s => new NotificationModel
                {
                    Id = s.Id,
                    Category = s.Category,
                    NotificationType = s.NotificationType,
                    Message = s.Message,
                    Link = s.Link,
                    CreatedDate = s.SendDate,
                    IsRead = s.IsRead
                })
                .FirstOrDefault();

            if (notification == null)
            {
                return NotFound();
            }


            return Ok(notification);
        }

        [HttpPost]
        public IHttpActionResult Post(CreateNotificationModel model)
        {
            if (model.SendDate == null) model.SendDate = DateTime.Now;

            var user = db.User.Where(u => u.Id == model.UserId).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            var notification = new Model.Notification
            {
                UserId = model.UserId,
                Category = model.Category,
                NotificationType = model.NotificationType,
                Message = model.Message,
                Link = model.Link,
                SendDate = model.SendDate.Value,
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            db.Notification.Add(notification);
            db.SaveChanges();

            return Ok(notification.Id);

        }

        [Route("api/System/Notification/MarkRead")]
        [HttpPut]
        public IHttpActionResult MarkRead(long id)
        {
            var notification = db.Notification.Where(u => u.Id == id).FirstOrDefault();

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;

            db.Entry(notification).Property(m => m.IsRead).IsModified = true;

            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/System/Notification/MarkReadAll")]
        [HttpPut]
        public IHttpActionResult MarkReadAll(int userId)
        {
            var notifications = db.Notification.Where(u => u.UserId == userId).ToList();

            foreach(var notification in notifications)
            {
                notification.IsRead = true;

                db.Entry(notification).Property(m => m.IsRead).IsModified = true;
            }

            db.SaveChanges();

            return Ok(true);
        }


        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var notification = db.Notification.Where(n => n.Id == id).FirstOrDefault();

            if (notification == null)
            {
                return NotFound();
            }

            db.Notification.Remove(notification);

            db.SaveChanges();

            return Ok(true);

        }

    }
}
