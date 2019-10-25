using FEP.Model;
using FEP.WebApiModel.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Logs
{
    [Route("api/Logs/ReportLog")]
    public class ReportLogController : ApiController
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

        [Route("api/Logs/ReportLog/Share")]
        [HttpPost]
        public IHttpActionResult Post(ShareLogModel model)
        {
            var log = new ShareLog
            {
                CreatedDate = DateTime.Now,
                Category = model.Category,
                CategoryId = model.CategoryId                
            };

            db.ShareLog.Add(log);

            db.SaveChanges();

            return Ok(log.Id);

        }

        [Route("api/Logs/ReportLog/Page")]
        [HttpPost]
        public IHttpActionResult Post(PageLogModel model)
        {
            var log = new PageLog
            {
                CreatedDate = DateTime.Now,
                Category = model.Category,
                CategoryId = model.CategoryId
            };

            db.PageLog.Add(log);

            db.SaveChanges();

            return Ok(log.Id);

        }

    }
}
