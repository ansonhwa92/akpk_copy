using AutoMapper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/CourseModules")]
    public class CourseModulesApiController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseModulesApiController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditModuleModel, CourseModule>();

                cfg.CreateMap<CourseModule, CreateOrEditModuleModel>();
            });

            _mapper = config.CreateMapper();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseModules/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseModel request)
        {
            return Ok();
        }

        /// <summary>
        /// To create a mdoule for a course. Require courseId
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseModules/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Create([FromBody] CreateOrEditModuleModel request)
        {
            if (ModelState.IsValid)
            {
                var course = await db.Courses
                        .Include(x => x.Modules)
                        .FirstOrDefaultAsync(x => x.Id.Equals(request.CourseId));

                if (course == null)
                    return BadRequest();

                if (course.Modules == null)
                    course.Modules = new List<CourseModule>();

                var module = new CourseModule
                {
                    CourseId = request.CourseId,
                    Objectives = request.Objectives,
                    Description = request.Description,
                    Title = request.Title,
                    Order = course.Modules.Count() > 0 ? course.Modules.Max(x => x.Order) + 1 : 1
                };

                course.Modules.Add(module);

                await db.SaveChangesAsync();

                course.UpdateCourseStat();
                db.SetModified(course);

                await db.SaveChangesAsync();

                return Ok(module.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("api/eLearning/CourseModules/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Edit([FromBody] CreateOrEditModuleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = db.CourseModules.Where(x => x.Id == model.Id).FirstOrDefault();

                if (entity != null)
                {
                    entity.Title = model.Title;
                    entity.Description = model.Description;
                    entity.Objectives = model.Objectives;

                    db.Entry(entity).State = EntityState.Modified;

                    db.SaveChanges();

                    return model.Title;
                }
            }
            return "";
        }

        [Route("api/eLearning/CourseModules/Delete")]
        public string Delete(int id)
        {
            var module = db.CourseModules
                        .Include(x => x.ModuleContents)
                        .Where(p => p.Id == id).FirstOrDefault();

            if (module != null)
            {
                string ptitle = module.Title;

                db.CourseContents.RemoveRange(module.ModuleContents);

                db.SaveChanges();

                db.CourseModules.Remove(module);

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseModules
                .Include(x => x.ModuleContents)
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var course = await db.Courses.FindAsync(entity.CourseId);

            if (course == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditModuleModel>(entity);
            model.Status = course.Status;

            model.ModuleContents = entity.ModuleContents;

            return Ok(model);
        }

        /// <summary>
        /// To save the front page of modiles, basically the order of the contents
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/eLearning/CourseModules/OrderContent")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> OrderContent(OrderModel orderModel)
        {
            string Id = orderModel.Id;

            if (String.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            var entity = await db.CourseModules
              .Include(x => x.ModuleContents)
              .FirstOrDefaultAsync(x => x.Id.ToString() == Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.ModuleContents = entity.ModuleContents.OrderBy(x => x.Order).ToList();

            for (int i = 0; i < orderModel.Order.Count(); i++)
            {
                var content = entity.ModuleContents.FirstOrDefault(x => x.Id.ToString() == orderModel.Order[i]);

                if (content != null)
                {
                    content.Order = i;
                }
            }

            db.SetModified(entity);

            await db.SaveChangesAsync();

            return Ok();
        }

        /// <returns></returns>
        [Route("api/eLearning/CourseModules/Start")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Start(int id)
        {
            var entity = await db.CourseContents.Where(x => x.CourseModuleId == id).OrderBy(x => x.Order).FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
    }
}