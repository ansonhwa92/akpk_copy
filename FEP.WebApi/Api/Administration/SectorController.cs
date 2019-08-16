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
        public HttpResponseMessage Get(DataTableModel dataTableModel)
        {
            var sectors = db.Sector.Where(u => u.Display).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name                
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, sectors);

        }

        // GET: api/Sector/5
        public HttpResponseMessage Get(int id)
        {
            var sector = db.Sector.Where(u => u.Display && u.Id == id).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, sector);            
        }

        // POST: api/Sector
        public HttpResponseMessage Post([FromBody]CreateSectorModel model)
        {
            if (ModelState.IsValid)
            {
                var sector = new Sector
                {
                    Name = model.Name,
                    Display = true
                };

                db.Sector.Add(sector);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

        }

        // PUT: api/Sector/5
        public HttpResponseMessage Put(int id, [FromBody]EditSectorModel model)
        {

            if (ModelState.IsValid)
            {
                var sector = db.Sector.Where(s => s.Id == id).FirstOrDefault();

                if (sector != null)
                {
                    sector.Name = model.Name;

                    db.Entry(sector).State = EntityState.Modified;
                    db.Entry(sector).Property(x => x.Display).IsModified = false;

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { isSuccess = false });
                }

            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE: api/Sector/5
        public HttpResponseMessage Delete(int id)
        {
            var sector = db.Sector.Where(u => u.Id == id).FirstOrDefault();

            if (sector != null)
            {
                sector.Display = false;

                db.Sector.Attach(sector);
                db.Entry(sector).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { isSuccess = false });
            }

        }

    }
}
