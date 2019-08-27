using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    public class AccessController : ApiController
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
            var role = db.Access.Select(s => new { Module = s.Module, UserAccess = s.UserAccess }).ToList();

            return Ok(role);
        }

        public IHttpActionResult Get(Modules module)
        {
            var role = db.Access.Where(a => a.Module == module).Select(s => new { Module = s.Module, UserAccess = s.UserAccess }).ToList();

            return Ok(role);
        }
    }
}
