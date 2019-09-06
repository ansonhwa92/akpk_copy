using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Logs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Logs
{
    [Route("api/Logs/UserLog")]
    public class UserLogController : ApiController
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

        [Route("api/Logs/UserLog/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterUserLogModel request)
        {

            var query = db.UserLog.Select(s => new UserLogModel
            {
                Id = s.Id,
                UserName = s.User.Name,
                Module = s.Module,
                UserId = s.UserId,
                LogDate = s.LogDate,
                Activity = s.Activity,
                Details = s.Details,
                IPAddress = s.IPAddress
            });

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.UserName == null || s.UserName.Contains(request.UserName))
               && (request.Module == null || s.Module == request.Module)
               && (request.Activity == null || s.Activity.Contains(request.Activity))
               && (request.Details == null || s.Details.Contains(request.Details))
               && (request.IPAddress == null || s.IPAddress.Contains(request.IPAddress))
               && (request.LogDateFrom == null || request.LogDateTo == null || DbFunctions.TruncateTime(s.LogDate) <= DbFunctions.TruncateTime(request.LogDateTo) && DbFunctions.TruncateTime(s.LogDate) >= DbFunctions.TruncateTime(request.LogDateFrom))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.UserName.Contains(value)
                || p.Activity.Contains(value)
                || p.Details.Contains(value)
                || p.IPAddress.Contains(value)
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
                    case "UserName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.UserName);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.UserName);
                        }

                        break;

                    case "Module":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Module);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Module);
                        }

                        break;

                    case "Activity":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Activity);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Activity);
                        }

                        break;

                    case "LogDate":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.LogDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.LogDate);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.LogDate);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.LogDate);
            }

            var data = query.Skip(request.start).Take(request.length).ToList();

            data.ForEach(s =>
            {
                s.ModuleDesc = s.Module.GetDisplayName();
            });

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }


        public IHttpActionResult Get(int id)
        {
            var log = db.UserLog.Where(u => u.Id == id)
                .Select(s => new UserLogModel
                {
                    Id = s.Id,
                    UserName = s.User.Name,
                    Module = s.Module,
                    UserId = s.UserId,
                    LogDate = s.LogDate,
                    Activity = s.Activity,
                    Details = s.Details,
                    IPAddress = s.IPAddress
                })
                .FirstOrDefault();

            if (log == null)
            {
                return NotFound();
            }

            log.ModuleDesc = log.Module.GetDisplayName();

            return Ok(log);
        }

        [HttpPost]
        public IHttpActionResult Post(CreateUserLogModel model)
        {
            var log = new UserLog
            {
                LogDate = DateTime.Now,
                UserId = model.UserId,
                Module = model.Module,
                Activity = model.Activity,
                Details = model.Details,
                IPAddress = model.IPAddress,
                GeoLocation = model.GeoLocation
            };

            db.UserLog.Add(log);

            db.SaveChanges();

            return Ok(log.Id);

        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {

            var log = db.UserLog.Where(u => u.Id == id).FirstOrDefault();

            if (log == null)
            {
                return NotFound();
            }

            db.UserLog.Remove(log);
            db.SaveChanges();

            return Ok(true);
        }

    }
}
