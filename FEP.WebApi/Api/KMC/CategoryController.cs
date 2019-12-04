using FEP.Model;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.KMC
{
    [Route("api/KMC/Category")]
    public class CategoryController : ApiController
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

            var categories = db.KMCCategory.Where(u => u.Display).Select(s => new CategoryModel
            {
                Id = s.Id,
                Title = s.Title
            }).ToList();

            return Ok(categories);

        }

        public IHttpActionResult Get(int id)
        {
            var category = db.KMCCategory.Where(u => u.Display && u.Id == id).Select(s => new CategoryModel
            {
                Id = s.Id,
                Title = s.Title
            }).FirstOrDefault();

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }


        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateCategoryModel model)
        {

            var category = new KMCCategory
            {
                Title = model.Title,
                Display = true
            };

            db.KMCCategory.Add(category);
            db.SaveChanges();

            return Ok(category.Id);

        }

        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditCategoryModel model)
        {

            var category = db.KMCCategory.Where(s => s.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Title = model.Title;

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
            var category = db.KMCCategory.Where(u => u.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Display = false;

                db.KMCCategory.Attach(category);
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

        [Route("api/KMC/Category/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string title)
        {
            if (id == null)
            {
                if (db.KMCCategory.Any(u => u.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.KMCCategory.Any(u => u.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }


    }
}
