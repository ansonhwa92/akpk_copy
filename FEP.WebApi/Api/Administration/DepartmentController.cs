using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Department")]
    public class DepartmentController : ApiController
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

        public IHttpActionResult Get()
        {
            var sectors = db.Branch.Where(u => u.Display).Select(s => new DepartmentModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(sectors);
        }


    }
}
