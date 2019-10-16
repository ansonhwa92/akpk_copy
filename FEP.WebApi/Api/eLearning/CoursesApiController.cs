using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
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

                cfg.CreateMap<TrainerCourseModel, TrainerCourse>();
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
            //var query = db.Courses.Where(x => (String.IsNullOrEmpty(request.Title) || x.Title.Contains(request.Title)) &&
            //                        (String.IsNullOrEmpty(request.Code) || x.Title.Contains(request.Code)) && x.IsDeleted != true);
            var query = db.Courses.Where(x => x.IsDeleted == false);

            if (!String.IsNullOrEmpty(request.Title))
                query = query.Where(x => x.Title.ToLower().Contains(request.Title.ToLower()));

            if (!String.IsNullOrEmpty(request.Code))
                query = query.Where(x => x.Code.ToLower().Contains(request.Code.ToLower()));

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
                course.IsDeleted = false;

                course.CreatedBy = request.CreatedBy;

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
                   .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditCourseModel>(entity);
            model.Description = HttpUtility.HtmlDecode(model.Description);
            model.Objectives = HttpUtility.HtmlDecode(model.Objectives);

            return Ok(model);
        }

        [Route("api/eLearning/Courses/GetTrainerCourse")]
        [HttpPost]
        public async Task<IHttpActionResult> GetTrainerCourse([FromUri]int courseId, [FromBody] FilterUserModel request)
        {
            var users = await db.TrainerCourses.Where(x => x.CourseId == courseId).Include(x => x.Trainer.User).Select(x => x.Trainer.User).ToListAsync();

            if (users == null)
                return NotFound();

            //List<User> users = new List<User>();

            //foreach(var item in entity)
            //{
            //    var user = db.User.FirstOrDefault(x => x.Id == item.TrainerId);
            //    users.Add(user);
            //}

            var totalCount = users.Count();

            //advance search
            var query = users.Where(s => (request.Name == null || s.Name.Contains(request.Name))
               && (request.Email == null || s.Email.Contains(request.Email))
               && (request.UserType == null || s.UserType == request.UserType)
               );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Name.Contains(value)
                || p.Email.Contains(value)
                );
            }

            var filteredCount = query.Count();

            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Name":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Name);
                        }

                        break;

                    case "UserType":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.UserType);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.UserType);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Email);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.Name);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new UserModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    UserType = s.UserType
                }).ToList();

            data.ForEach(item => { item.UserTypeDesc = item.UserType.GetDisplayName(); });

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        [Route("api/eLearning/Courses/GetAllTrainers")]
        [HttpPost]
        public IHttpActionResult GetAllTrainers(FilterIndividualModel request)
        {
            var users = db.UserRole.Where(u => u.UserAccount.User.Display && u.Role.Name == RoleNames.eLearningTrainer).Select(x => x.UserAccount.User);

            if (users == null)
                return NotFound();

            var totalCount = users.Count();

            //advance search
            var query = users.Where(s => (request.Name == null || s.Name.Contains(request.Name))
               && (request.ICNo == null || s.ICNo.Contains(request.ICNo))
               && (request.Email == null || s.Email.Contains(request.Email))
               && (request.MobileNo == null || s.MobileNo.Contains(request.MobileNo))
               );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Name.Contains(value)
                || p.ICNo.Contains(value)
                || p.Email.Contains(value)
                || p.MobileNo.Contains(value)
                );
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Name":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Name);
                        }

                        break;

                    case "ICNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.ICNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ICNo);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Email);
                        }

                        break;

                    case "MobileNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.MobileNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.MobileNo);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.Name);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new IndividualModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    ICNo = s.ICNo,
                    MobileNo = s.MobileNo,
                    Status = s.UserAccount.IsEnable
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }



        [Route("api/eLearning/Courses/AddUser")]
        [HttpPost]
        public IHttpActionResult AddUser(UpdateTrainerCourseModel model)
        {
            var course = db.Courses.Where(r => r.Id == model.CourseId).FirstOrDefault();

            if (course.Trainers == null)
            {
                course.Trainers = new List<Trainer>();
            }

            foreach (var item in model.UserId)
            {
                var user = db.User.FirstOrDefault(x => x.Id == item);
                var trainer = db.Trainers.FirstOrDefault(x => x.UserId == item);

                if (trainer == null)
                {
                    db.TrainerCourses.Add(new TrainerCourse
                    {
                        Trainer = new Trainer { User = user },
                        Course = course
                    });
                }
                else
                {
                    db.TrainerCourses.Add(new TrainerCourse
                    {
                        Trainer = trainer,
                        Course = course
                    });
                }
            }
            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/eLearning/Courses/DeleteUser")]
        [HttpPost]
        public IHttpActionResult DeleteUser(UpdateTrainerCourseModel model)
        {
            var course = db.Courses.Where(r => r.Id == model.CourseId).FirstOrDefault();

            if (course.Trainers == null)
            {
                course.Trainers = new List<Trainer>();
            }

            foreach (var item in model.UserId)
            {
                var user = db.User.FirstOrDefault(x => x.Id == item);
                var trainer = db.Trainers.FirstOrDefault(x => x.UserId == item);
                var trainerCourse = db.TrainerCourses.FirstOrDefault(x => x.TrainerId == trainer.Id && x.CourseId == model.CourseId);

                if (trainer != null)
                {
                    db.TrainerCourses.Remove(trainerCourse);
                    db.SetDeleted(trainerCourse);
                }
            }

            db.SaveChanges();

            return Ok(true);
        }


        [Route("api/eLearning/Courses/GetFrontCourse")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFrontCourse(int? id)
        {
            var entity = await db.Courses
                                .Include(x => x.CourseApprovalLog)
                                .Include(x => x.Modules)
                                .Include(x => x.CourseEvents)
                                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CreateOrEditCourseModel>(entity);
            model.CourseApprovalLogs = entity.CourseApprovalLog;

            model.Modules = entity.Modules;

            var courseEvent = new CourseEvent();

            if (entity.Status == CourseStatus.Trial)
            {
                courseEvent = entity.CourseEvents.Where(x => x.Status == CourseEventStatus.Trial).FirstOrDefault();
                model.CourseEventId = courseEvent.Id;
            }

            return Ok(model);
        }

        [Route("api/eLearning/Courses/Delete")]
        public string Delete(int id)
        {
            Course course = db.Courses.Find(id);

            if (course != null)
            {
                string ptitle = course.Title;

                course.IsDeleted = true;

                db.SetModified(course);

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        [Route("api/eLearning/Courses/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Edit([FromBody] CreateOrEditCourseModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Courses.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity != null)
                {
                    entity.CategoryId = model.CategoryId;
                    entity.Title = model.Title;
                    entity.Code = model.Code;
                    entity.Description = model.Description;
                    entity.Objectives = model.Objectives;
                    entity.Medium = model.Medium;
                    entity.Duration = model.Duration;
                    entity.DurationType = model.DurationType;
                    entity.Language = model.Language;
                    entity.IsFree = model.IsFree;
                    entity.ViewCategory = model.ViewCategory;
                    entity.Price = model.Price;

                    entity.UpdatedBy = model.UpdatedBy;
                    entity.UpdatedDate = DateTime.Now;

                    db.SetModified(entity);

                    db.SaveChanges();

                    model.Id = entity.Id;

                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
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

        /// <summary>
        /// To save the front page of course, basically the order of the modules
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/eLearning/Courses/OrderContent")]
        [HttpPost]
        public async Task<IHttpActionResult> OrderContent([FromBody] OrderModel orderModel)
        {
            string Id = orderModel.Id;


            if (String.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            var entity = await db.Courses
              .Include(x => x.CourseApprovalLog)
              .Include(x => x.Modules)
              .FirstOrDefaultAsync(x => x.Id.ToString() == Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Modules = entity.Modules.OrderBy(x => x.Order).ToList();

            for (int i = 0; i < orderModel.Order.Count(); i++)
            {
                var module = entity.Modules.FirstOrDefault(x => x.Id.ToString() == orderModel.Order[i]);

                if (module != null)
                {
                    module.Order = i;
                }
            }

            db.SetModified(entity);

            await db.SaveChangesAsync();

            return Ok();
        }

        [Route("api/eLearning/Courses/SaveCertificate")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> SaveCertificate([FromBody] CertificatesModel model)
        {
            var entity = await db.Courses.FirstOrDefaultAsync(x => x.Id == model.courseId);

            if (entity != null)
            {
                entity.CourseCertificateId = model.selectedBackground;
                entity.CourseCertificateTemplateId = model.selectedTemplate;
                db.Entry(entity).State = EntityState.Modified;

                db.SaveChanges();

                return Ok(entity.Id);
            }

            return BadRequest(ModelState);
        }

        [Route("api/eLearning/Courses/UpdateIntroImg")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> UpdateIntroImg(ImageModel model)
        {
            var entity = await db.Courses.FirstOrDefaultAsync(x => x.Id.ToString() == model.Id);

            if (entity == null)
                return BadRequest();

            entity.IntroImageFileName = model.FileName;
            db.SetModified(entity);

            await db.SaveChangesAsync();

            return Ok();
        }


        /// <returns></returns>
        [Route("api/eLearning/Courses/Start")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Start(int id)
        {
            var entity = await db.CourseModules.Where(x => x.CourseId == id).OrderBy(x => x.Order).FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
    }

    public class ImageModel
    {
        public string Id { get; set; }
        public string FileName { get; set; }
    }
}