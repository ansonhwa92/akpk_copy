using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/EventCategory")]
	public class EventCategoryController : ApiController
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
            var categories = db.EventCategory.Where(u => u.Display).Select(s => new EventCategoryModel
            {
                Id = s.Id,
                Name = s.CategoryName
            }).ToList();

            return Ok(categories);
        }


        public IHttpActionResult Get(int id)
        {
            var category = db.EventCategory.Where(u => u.Display && u.Id == id).Select(s => new EventCategoryModel
            {
                Id = s.Id,
                Name = s.CategoryName
            }).FirstOrDefault();

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }


        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateEventCategoryModel model)
        {

            var category = new EventCategory
            {
                CategoryName = model.Name,
                Display = true
            };

            db.EventCategory.Add(category);
            db.SaveChanges();

            return Ok(category.Id);

        }


        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditEventCategoryModel model)
        {

            var category = db.EventCategory.Where(s => s.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.CategoryName = model.Name;

                db.Entry(category).State = EntityState.Modified;
                db.Entry(category).Property(x => x.Display).IsModified = false;

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
            var category = db.EventCategory.Where(u => u.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Display = false;

                db.EventCategory.Attach(category);
                db.Entry(category).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/eEvent/EventCategory/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.EventCategory.Any(u => u.CategoryName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.EventCategory.Any(u => u.CategoryName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }
    }
}