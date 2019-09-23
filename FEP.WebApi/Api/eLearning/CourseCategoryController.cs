﻿using FEP.Model;
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
            var categories = db.LearningCourseCategory.Where(u => u.Display).Select(s => new CourseCategoryModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(categories);
        }

                
        public IHttpActionResult Get(int id)
        {
            var category = db.LearningCourseCategory.Where(u => u.Display && u.Id == id).Select(s => new CourseCategoryModel
            {
                Id = s.Id,
                Name = s.Name
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

            var category = new LearningCourseCategory
            {
                Name = model.Name,
                Display = true
            };

            db.LearningCourseCategory.Add(category);
            db.SaveChanges();

            return Ok(category.Id);

        }

        
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditCourseCategoryModel model)
        {

            var category = db.LearningCourseCategory.Where(s => s.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Name = model.Name;

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
            var category = db.LearningCourseCategory.Where(u => u.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.Display = false;

                db.LearningCourseCategory.Attach(category);
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

        [Route("api/eLearning/CourseCategory/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.LearningCourseCategory.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.LearningCourseCategory.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }
    }
}