using AutoMapper;
using FEP.Helper;
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
    [Route("api/eLearning/CourseContents")]
    public class CourseContentsController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseContentsController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditContentModel, CourseContent>();

                cfg.CreateMap<CourseContent, CreateOrEditContentModel>();

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
        [Route("api/eLearning/CourseContents/Upload")]
        [HttpPost]
        public IHttpActionResult Upload(FilterCourseModel request)
        {

            return Ok();
        }


        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseModel request)
        {

            return Ok();
        }

        /// <summary>
        /// To create a content for a course. Require courseId
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Create([FromBody] CreateOrEditContentModel request)
        {
            if (ModelState.IsValid)
            {




                // check if the request come from front page, then create a new module then create the content.
                // if it comes from the module, then create the content there
                // differentiate by CreateContentFrom
                var content = _mapper.Map<CourseContent>(request);

                if (request.CreateContentFrom == CreateContentFrom.CourseFrontPage)
                {
                    

                    var course = await db.Courses
                        .Include(x => x.Modules)
                        .FirstOrDefaultAsync(x => x.Id.Equals(request.CourseId));

                    if (course == null)
                        return BadRequest();

                    var module = new CourseModule
                    {
                        CourseId = request.CourseId,
                        Objectives = "Objective",
                        Description = "Description",
                        Title = request.Title,
                        Order = course.Modules != null ? (course.Modules.Max(x => x.Order) + 1) : 1
                    };

                    content.Order = 1;

                    module.ModuleContents = new List<CourseContent>();
                    module.ModuleContents.Add(content);

                    course.Modules.Add(module);

                    await db.SaveChangesAsync();

                    return Ok(course.Id);
                    
                }
                else
                {
                    var module = await db.CourseModules
                        .Include(x => x.ModuleContents)
                        .FirstOrDefaultAsync(x => x.Id.Equals(request.CourseModuleId));

                    if (module == null)
                        return BadRequest();

                    if(module.ModuleContents == null)
                    {
                        module.ModuleContents = new List<CourseContent>();
                    }

                    content.Order = module.ModuleContents.Max(x => x.Order) + 1;                    
                    module.ModuleContents.Add(content);


                    //var contents = db.CourseContents.Where(x => x.CourseModuleId == request.CourseModuleId);

                    //if (contents == null)
                    //    return BadRequest();


                    //content.Order = contents.Max(x => x.Order) + 1;

                    //contents.


                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch(Exception e)
                    {
                        return BadRequest(e.Message);
                    }

                    return Ok(module.Id);                    
                }                
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        
        /// <summary>
        /// Mark complete this content, should put the mark in progress
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/Commplete")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Complete([FromBody] CreateOrEditContentModel request)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.CourseContents.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (entity == null)
                    return BadRequest();


                var nextEntity = await db.CourseContents.FirstOrDefaultAsync(x => x.Id == request.Id && 
                        x.Order == (entity.Order + 1));

                if (nextEntity == null)
                    return Ok("-1");

                //TODO: MARK THE USER PROGRESS. IF TRIAL, IGNORE

                return Ok(nextEntity.Id);
               

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseContents.FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditContentModel>(entity);

            return Ok(model);
        }


    }
}