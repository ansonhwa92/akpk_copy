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
        public List<SectorModel> Get()
        {
            var sectors = db.Sector.Where(u => u.Display).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name                
            }).ToList();

            return sectors;
        }

        [HttpPost]
        public List<SectorModel> GetTable(DataTableModel model)
        {
            var sectors = db.Sector.Where(u => u.Display).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return sectors;
        }

        // GET: api/Sector/5
        public SectorModel Get(int id)
        {
            var sector = db.Sector.Where(u => u.Display && u.Id == id).Select(s => new SectorModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            return sector;          
        }

        // POST: api/Sector
        public int? Post([FromBody]CreateSectorModel model)
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

                return sector.Id;
            }

            return null;

        }

        // PUT: api/Sector/5
        public bool Put(int id, [FromBody]EditSectorModel model)
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

                    return true;
                }
                else
                {
                    return false;
                }

            }

            return false;
        }

        // DELETE: api/Sector/5
        public bool Delete(int id)
        {
            var sector = db.Sector.Where(u => u.Id == id).FirstOrDefault();

            if (sector != null)
            {
                sector.Display = false;

                db.Sector.Attach(sector);
                db.Entry(sector).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
