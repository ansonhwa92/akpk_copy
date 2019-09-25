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
using System.Web;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Courses")]
    public class CoursesController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CoursesController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditCourseModel, Course>();

                cfg.CreateMap<Course, CreateOrEditCourseModel>();

                cfg.CreateMap<CourseRuleModel, Course>();
            });

            _mapper = config.CreateMapper();
        }

        public CoursesController(IMapper mapper)
        {
            _mapper = mapper;
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
        [Route("api/eLearning/Courses/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseModel request)
        {
            var query = db.Courses.Where(x => (String.IsNullOrEmpty(request.Title) || x.Title.Contains(request.Title)) &&
                                    (String.IsNullOrEmpty(request.Code) || x.Title.Contains(request.Code)));

            var totalCount = query.Count();

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Title.Contains(value) || p.Code.Contains(value));
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Title":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Title);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Title);
                        }

                        break;

                    case "Code":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Code);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Code);
                        }

                        break;

                    case "Status":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Status);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Status);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.Category.Name).OrderBy(o => o.Title);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(o => o.Category.Name).OrderBy(o => o.Title);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(x => new ReturnBriefCourseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Code = x.Code,
                    CategoryId = x.CategoryId,
                    Status = x.Status,
                    Price = x.IsFree ? "Free" : x.Price.ToString()
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/Courses/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Create([FromBody] CreateOrEditCourseModel request)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(request);

                course.Description = HttpUtility.HtmlEncode(request.Description);
                course.Objectives = HttpUtility.HtmlEncode(request.Objectives);

                course.CreatedDate = DateTime.Now;

                // all course activity is log to table courseapprovallog

                course.CourseApprovalLog = new List<CourseApprovalLog>
                {
                    new CourseApprovalLog
                    {
                        CreatedByName = request.CreatedByName,
                        ActionDate = DateTime.Now,
                        Remark = "Course " + request.Title + " created.",
                    },
                };

                db.Courses.Add(course);

                await db.SaveChangesAsync();

                return Ok(course.Id.ToString());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.Courses
                   .Include(x => x.CourseApprovalLog)
                   .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditCourseModel>(entity);
            model.CourseApprovalLogs = new List<CourseApprovalLog>();
            model.CourseApprovalLogs = entity.CourseApprovalLog;

            return Ok(model);
        }

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/Courses/EditRules")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> EditRules([FromBody] CourseRuleModel request)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (entity == null)
                    return NotFound();

                entity.TraversalRule = request.TraversalRule;
                entity.CompletionCriteriaType = request.CompletionCriteriaType;
                entity.ModulesCompleted = request.ModulesCompleted;
                entity.LearningPath = request.LearningPath;
                entity.PercentageCompletion = request.PercentageCompletion;
                entity.ScoreCalculation = request.ScoreCalculation;
                entity.TestsPassed = request.TestsPassed;
                entity.TraversalRule = request.TraversalRule;

                db.Entry(entity).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(entity.Id.ToString());
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}