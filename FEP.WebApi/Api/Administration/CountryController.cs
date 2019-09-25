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
    [Route("api/Administration/Country")]
    public class CountryController : ApiController
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
            var states = db.Country.Where(u => u.Display).Select(s => new CountryModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(states);
        }


        public IHttpActionResult Get(int id)
        {
            var country = db.Country.Where(u => u.Display && u.Id == id).Select(s => new CountryModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            if (country != null)
            {
                return Ok(country);
            }

            return NotFound();
        }

        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateCountryModel model)
        {

            var country = new Country
            {
                Name = model.Name,
                Display = true
            };

            db.Country.Add(country);
            db.SaveChanges();

            return Ok(country.Id);

        }


        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditCountryModel model)
        {

            var country = db.Country.Where(s => s.Id == id).FirstOrDefault();

            if (country != null)
            {
                country.Name = model.Name;

                db.Entry(country).State = EntityState.Modified;
                db.Entry(country).Property(x => x.Display).IsModified = false;

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
            var country = db.Country.Where(u => u.Id == id).FirstOrDefault();

            if (country != null)
            {
                country.Display = false;

                db.Country.Attach(country);
                db.Entry(country).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/Administration/Country/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.Country.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.Country.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }

    }
}
