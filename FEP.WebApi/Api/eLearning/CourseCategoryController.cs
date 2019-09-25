using FEP.Model.eLearning;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{

    [Route("api/eLearning/CourseCategory")]
    public class CourseCategoryController : ApiController
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
            var categories = db.RefCourseCategories.Where(u => u.IsDisplayed).Select(s => new CourseCategoryModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();

            return Ok(categories);
        }

                
        public IHttpActionResult Get(int id)
        {
            var category = db.RefCourseCategories.Where(u => u.IsDisplayed && u.Id == id).Select(s => new CourseCategoryModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).FirstOrDefault();

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }

        
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateCourseCategoryModel model)
        {

            var category = new RefCourseCategory
            {
                Name = model.Name,
                Description = model.Description,
                IsDisplayed = true
            };

            db.RefCourseCategories.Add(category);
            db.SaveChanges();

            return Ok(category.Id);

        }

        
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditCourseCategoryModel model)
        {

            var category = db.RefCourseCategories.Where(s => s.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Name = model.Name;
                category.Description = model.Description;

                db.Entry(category).State = EntityState.Modified;
                db.Entry(category).Property(x => x.IsDisplayed).IsModified = false;

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
            var category = db.RefCourseCategories.Where(u => u.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.IsDisplayed = false;

                db.RefCourseCategories.Attach(category);
                db.Entry(category).Property(m => m.IsDisplayed).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/eLearning/CourseCategory/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.RefCourseCategories.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.IsDisplayed))
                    return Ok(true);
            }
            else
            {
                if (db.RefCourseCategories.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.IsDisplayed))
                    return Ok(true);
            }

            return NotFound();
        }
    }
}
