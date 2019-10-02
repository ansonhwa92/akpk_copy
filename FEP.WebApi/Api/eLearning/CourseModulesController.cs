using AutoMapper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/CourseModules")]
    public class CourseModulesController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseModulesController()
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
                    Order = course.Modules.Max(x => x.Order) + 1
                };

                course.Modules.Add(module);

                await db.SaveChangesAsync();

                return Ok(module.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseModules
                .Include(x => x.ModuleContents)
                .FirstOrDefaultAsync(x => x.Id == id.Value );

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditModuleModel>(entity);
            model.ModuleContents = entity.ModuleContents;

            return Ok(model);
        }

        /// <summary>
        /// To save the front page of modiles, basically the order of the contents
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/eLearning/CourseModules/Content")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Content(int? Id, int CreatedBy, string order)
        {
            if (Id == null)
            {
                return BadRequest();
            }

            var entity = await db.CourseModules
              .Include(x => x.ModuleContents)
              .FirstOrDefaultAsync(x => x.Id == Id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            entity.ModuleContents = entity.ModuleContents.OrderBy(x => x.Order).ToList();

            var splitOrder = order.Split(',').ToArray();

            if (entity.ModuleContents.Count() == splitOrder.Count())
            {
                int i = 0;
                foreach (var module in entity.ModuleContents)
                {
                    module.Order = int.Parse(splitOrder[i]);

                    i++;
                }
            }

            db.SetModified(entity);

            await db.SaveChangesAsync();

            var model = _mapper.Map<CreateOrEditModuleModel>(entity);                
            model.ModuleContents = entity.ModuleContents;

            return Ok(model);
        }
    }
}