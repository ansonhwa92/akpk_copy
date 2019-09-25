using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
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
                var courseModules = db.CourseModules.Where(x => x.CourseId == request.CourseId);

                var module = new CourseModule
                {
                    CourseId = request.CourseId,
                    Objectives = request.Objectives,
                    Description = request.Description,
                    Title = request.Title,
                    Order = courseModules != null ? (courseModules.Count() + 1) : 0
                };


                var vm = db.CourseModules.Add(module);

                await db.SaveChangesAsync();

                return Ok(vm.Id.ToString());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseModules.FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditModuleModel>(entity);

            return Ok(model);
        }


    }
}