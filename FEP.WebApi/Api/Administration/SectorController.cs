using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FEP.Helper;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Sector")]
    public class SectorController : ApiController
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

        // GET: api/Sector
        public IHttpActionResult Get()
        {
            var sectors = db.Sector.Where(u => u.Display).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(sectors);
        }

        [HttpPost]
        public List<SectorModel> GetTable()
        {
            //var sectors = db.Sector.Where(u => u.Display).Select(s => new SectorModel
            //{
            //    Id = s.Id,
            //    Name = s.Name
            //}).ToList();

            //return sectors;
            return null;
        }

        // GET: api/Sector/5
        public IHttpActionResult Get(int id)
        {
            var sector = db.Sector.Where(u => u.Display && u.Id == id).Select(s => new SectorModel
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

        // POST: api/Sector
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateSectorModel model)
        {

            var sector = new Sector
            {
                Name = model.Name,
                Display = true
            };

            db.Sector.Add(sector);
            db.SaveChanges();

            return Ok(sector.Id);
            
        }

        // PUT: api/Sector/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditSectorModel model)
        {

            var sector = db.Sector.Where(s => s.Id == id).FirstOrDefault();

            if (sector != null)
            {
                sector.Name = model.Name;

                db.Entry(sector).State = EntityState.Modified;
                db.Entry(sector).Property(x => x.Display).IsModified = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        // DELETE: api/Sector/5
        public IHttpActionResult Delete(int id)
        {
            var sector = db.Sector.Where(u => u.Id == id).FirstOrDefault();

            if (sector != null)
            {
                sector.Display = false;

                db.Sector.Attach(sector);
                db.Entry(sector).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
