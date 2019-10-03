using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Ministry")]
    public class MinistryController : ApiController
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
            var sectors = db.Ministry.Where(u => u.Display).Select(s => new MinistryModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(sectors);
        }
               
        public IHttpActionResult Get(int id)
        {
            var sector = db.Ministry.Where(u => u.Display && u.Id == id).Select(s => new MinistryModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            if (sector != null)
            {
                return Ok(sector);
            }

            return NotFound();
        }

        
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateMinistryModel model)
        {

            var ministry = new Ministry
            {
                Name = model.Name,
                Display = true
            };

            db.Ministry.Add(ministry);
            db.SaveChanges();

            return Ok(ministry.Id);

        }

        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditMinistryModel model)
        {

            var ministry = db.Ministry.Where(s => s.Id == id).FirstOrDefault();

            if (ministry != null)
            {
                ministry.Name = model.Name;

                db.Entry(ministry).State = EntityState.Modified;
                db.Entry(ministry).Property(x => x.Display).IsModified = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        public IHttpActionResult Delete(int id)
        {
            var ministry = db.Ministry.Where(u => u.Id == id).FirstOrDefault();

            if (ministry != null)
            {
                ministry.Display = false;

                db.Ministry.Attach(ministry);
                db.Entry(ministry).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/Administration/Ministry/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.Ministry.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.Ministry.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }

    }
}
