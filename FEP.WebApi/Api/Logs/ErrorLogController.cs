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
    [Route("api/Logs/ErrorLog")]
    public class ErrorLogController : ApiController
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

        [Route("api/Logs/ErrorLog/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterErrorLogModel request)
        {

            var query = db.ErrorLog.Select(s => new ErrorLogModel
            {
                Id = s.Id,
                UserName = s.User.Name,
                Module = s.Module,
                UserId = s.UserId,
                LogDate = s.CreatedDate,
                Source = s.Source,
                Description = s.ErrorDescription,
                Details = s.ErrorDetails,
                IPAddress = s.IPAddress

            });

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.UserName == null || s.UserName.Contains(request.UserName))
               && (request.Module == null || s.Module == request.Module)
               && (request.Source == null || s.Source.Contains(request.Source))
               && (request.Description == null || s.Details.Contains(request.Description))
               && (request.IPAddress == null || s.IPAddress.Contains(request.IPAddress))
               && (request.LogDateFrom == null || request.LogDateTo == null || DbFunctions.TruncateTime(s.LogDate) < DbFunctions.TruncateTime(request.LogDateTo) && DbFunctions.TruncateTime(s.LogDate) > DbFunctions.TruncateTime(request.LogDateFrom))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.UserName.Contains(value)
                || p.Source.Contains(value)
                || p.Description.Contains(value)
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

                    case "Source":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Source);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Source);
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var log = db.ErrorLog.Where(u => u.Id == id)
                .Select(s => new ErrorLogModel
                {
                    Id = s.Id,
                    UserName = s.User.Name,
                    Module = s.Module,
                    UserId = s.UserId,
                    LogDate = s.CreatedDate,
                    Source = s.Source,
                    Description = s.ErrorDescription,
                    Details = s.ErrorDetails,
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
        public IHttpActionResult Post(CreateErrorLogModel model)
        {
            var log = new ErrorLog
            {
                CreatedDate = DateTime.Now,
                UserId = model.UserId,
                Module = model.Module,
                Source = model.Source,
                ErrorDescription = model.Description,
                ErrorDetails = model.Details,
                IPAddress = model.IPAddress
            };

            db.ErrorLog.Add(log);

            db.SaveChanges();

            return Ok(log.Id);

        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            var log = db.ErrorLog.Where(u => u.Id == id).FirstOrDefault();

            if (log == null)
            {
                return NotFound();
            }

            db.ErrorLog.Remove(log);
            db.SaveChanges();

            return Ok(true);
        }
    }
}
