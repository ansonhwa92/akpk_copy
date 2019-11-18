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
    [Route("api/Administration/Branch")]
    public class BranchController : ApiController
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
            var sectors = db.Branch.Where(u => u.Display).Select(s => new BranchModel
            {
                Id = s.Id,
                Name = s.Name,
                State = s.State.Name,
                StateId = s.StateId
            }).ToList();

            return Ok(sectors);
        }

        public IHttpActionResult Get(int id)
        {
            var sector = db.Branch.Where(u => u.Display && u.Id == id).Select(s => new BranchModel
            {
                Id = s.Id,
                Name = s.Name,
                State = s.State.Name,
                StateId = s.StateId
            }).FirstOrDefault();

            if (sector != null)
            {
                return Ok(sector);
            }

            return NotFound();
        }


        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateBranchModel model)
        {

            var branch = new Branch
            {
                Name = model.Name,
                StateId = model.StateId,
                Display = true
            };

            db.Branch.Add(branch);
            db.SaveChanges();

            return Ok(branch.Id);

        }

        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditBranchModel model)
        {

            var branch = db.Branch.Where(s => s.Id == id).FirstOrDefault();

            if (branch != null)
            {
                branch.Name = model.Name;
                branch.StateId = model.StateId;

                db.Entry(branch).State = EntityState.Modified;
                db.Entry(branch).Property(x => x.Display).IsModified = false;

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
            var branch = db.Branch.Where(u => u.Id == id).FirstOrDefault();

            if (branch != null)
            {
                branch.Display = false;

                db.Branch.Attach(branch);
                db.Entry(branch).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/Administration/Branch/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.Branch.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.Branch.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }

    }
}
