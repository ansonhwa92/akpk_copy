using FEP.Helper;
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
    [Route("api/Administration/State")]
    public class StateController : ApiController
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
            var states = db.State.Where(u => u.Display).Select(s => new StateModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(states);
        }
        

        public IHttpActionResult Get(int id)
        {
            var state = db.State.Where(u => u.Display && u.Id == id).Select(s => new StateModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            if (state != null)
            {
                return Ok(state);
            }

            return NotFound();
        }

        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateStateModel model)
        {

            var state = new State
            {
                Name = model.Name,
                Display = true
            };

            db.State.Add(state);
            db.SaveChanges();

            return Ok(state.Id);

        }


        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditStateModel model)
        {

            var state = db.State.Where(s => s.Id == id).FirstOrDefault();

            if (state != null)
            {
                state.Name = model.Name;

                db.Entry(state).State = EntityState.Modified;
                db.Entry(state).Property(x => x.Display).IsModified = false;

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
            var state = db.State.Where(u => u.Id == id).FirstOrDefault();

            if (state != null)
            {
                state.Display = false;

                db.State.Attach(state);
                db.Entry(state).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/Administration/State/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.State.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.State.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }

    }
}
