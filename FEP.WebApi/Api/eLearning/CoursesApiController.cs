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

        // GET: api/eLearning/Courses/GetCourses (list) - CURRENTLY USED FOR ANONYMOUS BROWSING
        [Route("api/eLearning/Courses/GetCourses")]
        [HttpGet]
        public BrowseElearningModel GetCourses(string keyword, string sorting, bool cashflow, bool car, bool house, bool investment, bool protection, /*bool beginner, bool intermediate, bool advanced,*/ bool english, bool malay, bool chinese, bool tamil, bool multiLanguage)
        {
            // active (for now = elearning) only

            var query = db.Courses.Where(p => p.Status == CourseStatus.Published);

            var totalCount = query.Count();

            query = query.Where(p => (keyword == null || keyword == ""
                || p.Title.Contains(keyword)
                || p.Description.Contains(keyword) || p.Objectives.Contains(keyword)
                || p.Code.Contains(keyword)));

            //Course Category
            if (!cashflow) { query = query.Where(p => p.CategoryId != 1); }
            if (!car) { query = query.Where(p => p.CategoryId != 2); }
            if (!house) { query = query.Where(p => p.CategoryId != 3); }
            if (!investment) { query = query.Where(p => p.CategoryId != 4); }
            if (!protection) { query = query.Where(p => p.CategoryId != 5); }

            //Skill Level
            //if (beginner) { query = query.Where(p => p.SkillLevel == SkillLevel.Beginner); }
            //if (intermediate) { query = query.Where(p => p.SkillLevel == SkillLevel.Intermediate); }
            //if (advanced) { query = query.Where(p => p.SkillLevel == SkillLevel.Advanced); }

            if (english) { query = query.Where(p => p.Language == CourseLanguage.English); }
            if (malay) { query = query.Where(p => p.Language == CourseLanguage.Malay); }
            if (chinese) { query = query.Where(p => p.Language == CourseLanguage.Chinese); }
            if (tamil) { query = query.Where(p => p.Language == CourseLanguage.Tamil); }
            if (multiLanguage) { query = query.Where(p => p.Language == CourseLanguage.MultiLanguage); }

            var filteredCount = query.Count();

            if (sorting == "title")
            {
                query = query.OrderBy(o => o.Title).OrderByDescending(o => o.CreatedDate);
            }
            //else if (sorting == "year")
            //{
            //    query = query.OrderByDescending(o => o.Year).OrderBy(o => o.Title);
            //}
            else if (sorting == "added")
            {
                query = query.OrderByDescending(o => o.CreatedDate).OrderBy(o => o.Title);
            }
            else
            {
                query = query.OrderBy(o => o.Title).OrderByDescending(o => o.CreatedDate);
            }

            //var trainer = GetInstructor();

            var data = query.Skip(0).Take(filteredCount).Select(s => new ReturnElearningModel
            {
                Id = s.Id,
                CategoryId = s.CategoryId,
                Title = s.Title,
                Description = s.Description,
                Language = s.Language,
                Price = s.Price.Value,
                //Instructor = GetInstructor(s.Id).ToString(),
                //Instructor = db.TrainerCourses.Where(x => x.CourseId == s.Id).Include(x => x.Trainer).FirstOrDefault().Trainer.User.Name,
                TotalModules = s.Modules.Count(),
                TotalStudent = db.Enrollments.Where(x => x.CourseId == s.Id).Count(),
                Status = s.Status,
                IntroImageFileName = s.IntroImageFileName
            }).ToList();

            var browser = new BrowseElearningModel
            {
                Keyword = keyword,
                Sorting = sorting,
                LastIndex = filteredCount,
                ItemCount = totalCount,
                Courses = data
            };

            return browser;
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

        // Function to get publication details for review before submission. The details retrieved include action
        // log history. This function is called for the third page of publication creation/editing.
        // GET: api/RnP/Publication/GetForReview/5
        [Route("api/eLearning/Courses/GetForReview")]
        public CreateOrEditCourseModel GetForReview(int id)
        {
            var course = db.Courses.Where(p => p.Id == id).Select(model => new CreateOrEditCourseModel
            {
                CategoryId = model.CategoryId,
                Title = model.Title,
                Code = model.Code,
                Description = model.Description,
                Objectives = model.Objectives,
                Medium = model.Medium,
                Duration = model.Duration.Value,
                DurationType = model.DurationType,
                Language = model.Language,
                IsFree = model.IsFree,
                ViewCategory = model.ViewCategory,
                Price = model.Price.Value,
                UpdatedBy = model.UpdatedBy,
                CreatedBy = model.CreatedBy
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == course.CreatedBy).FirstOrDefault();
            if (puser != null)
            {
                course.CreatedByName = puser.Name;
            }

            course.Description = HttpUtility.HtmlDecode(course.Description);
            course.Objectives = HttpUtility.HtmlDecode(course.Objectives);

            return course;
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
                        ApprovalStatus = ApprovalStatus.None
                    },
                };

                db.Courses.Add(course);

                await db.SaveChangesAsync();

                course.UpdateCourseStat();

                db.SetModified(course);

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
            model.CourseApprovalLogModel = new CourseApprovalLogModel();
            model.CourseApprovalLogModel.Status = model.Status;

            model.Description = HttpUtility.HtmlDecode(model.Description);
            model.Objectives = HttpUtility.HtmlDecode(model.Objectives);

            return Ok(model);
        }

        [HttpGet]
        public string GetInstructor(int? courseId)
        {
            var instructor = db.TrainerCourses.Where(x => x.CourseId == courseId).Include(x => x.Trainer).FirstOrDefault().Trainer.User.Name;

            return instructor;
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
            var users = db.UserRole.Where(u => u.UserAccount.User.Display &&
                (u.Role.Name == RoleNames.eLearningTrainer ||
                u.Role.Name == RoleNames.eLearningFacilitator)).Select(x => x.UserAccount.User);

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

        /// <summary>
        /// Misleading function Name. This is actually for adding trainer to the course.
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
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
            model.CourseApprovalLogModel = new CourseApprovalLogModel();
            model.CourseApprovalLogModel.Status = model.Status;

            var courseEvent = new CourseEvent();

            if (entity.Status == CourseStatus.Trial)
            {
                courseEvent = entity.CourseEvents.Where(x => x.Status == CourseEventStatus.Trial).FirstOrDefault();
                model.CourseEventId = courseEvent.Id;
            }

            return Ok(model);
        }

        [Route("api/eLearning/Courses/CancelCourse")]
        [HttpPost]
        public async Task<IHttpActionResult> CancelCourse(int id)
        {
            Course course = await db.Courses.FindAsync(id);

            if (course != null)
            {
                string ptitle = course.Title;

                course.Status = CourseStatus.Cancelled;

                db.SetModified(course);

                db.SaveChanges();

                return Ok(ptitle);
            }

            return BadRequest();
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

                    // check if change is for ViewCategory, if change to Public and the status is Published,
                    // create a default Public course event
                    var publicEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == entity.Id && x.ViewCategory == ViewCategory.Public);

                    if (model.ViewCategory == ViewCategory.Public)
                    {
                        if (entity.Status == CourseStatus.Published)
                        {
                            // check whether a public event already exists
                            if (publicEvent == null)
                            {
                                var newEvent = new CourseEvent
                                {
                                    Name = "Public Course",
                                    CourseId = entity.Id,
                                    AllowablePercentageBeforeWithdraw = entity.DefaultAllowablePercentageBeforeWithdraw,
                                    CreatedBy = model.UpdatedBy,
                                    EnrollmentCode = $"PUBLIC({entity.Code})",
                                    ViewCategory = ViewCategory.Public,
                                    Status = entity.ViewCategory == ViewCategory.Public ? CourseEventStatus.AvailableToPublic : CourseEventStatus.AvailableToPrivate,
                                    Start = DateTime.Now,
                                    IsDisplayed = entity.ViewCategory == ViewCategory.Public ? true : false
                                };

                                db.CourseEvents.Add(newEvent);
                            }
                            else // change the status only
                            {
                                publicEvent.Status = CourseEventStatus.AvailableToPublic;
                                publicEvent.ViewCategory = ViewCategory.Public;
                                publicEvent.IsDisplayed = true;

                                db.SetModified(publicEvent);
                            }
                        }
                        await db.SaveChangesAsync();
                    }

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
        public async Task<IHttpActionResult> SaveCertificate([FromBody] ReviewCertificateModel model)
        {
            var entity = await db.Courses.FirstOrDefaultAsync(x => x.Id == model.CourseId);

            if (entity != null)
            {
                entity.CourseCertificateId = model.Background.Id;
                entity.CourseCertificateTemplateId = model.Template.Id;
                db.Entry(entity).State = EntityState.Modified;

                db.SaveChanges();

                return Ok(entity.Id);
            }

            return BadRequest(ModelState);
        }

        [Route("api/eLearning/Courses/ReviewCertificate")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> ReviewCertificate([FromBody] CertificatesModel model)
        {
            var course = await db.Courses.FirstOrDefaultAsync(x => x.Id == model.courseId);
            var bg = await db.CourseCertificates.FirstOrDefaultAsync(x => x.Id == model.selectedBackground);
            var temp = await db.CourseCertificateTemplates.FirstOrDefaultAsync(x => x.Id == model.selectedTemplate);

            if (bg != null)
            {
                var review = new ReviewCertificateModel
                {
                    Background = bg,
                    Template = temp,
                    CourseId = course.Id
                };

                return Ok(review);
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

        [Route("api/eLearning/Courses/GetCoursesList")]
        [HttpGet]
        public IHttpActionResult GetCoursesList(int id)
        {
            var courses = db.Courses.Where(u => u.IsDeleted == false && u.Id != id).Select(s => new CourseListModel
            {
                Id = s.Id,
                Name = s.Title,
            }).ToList();

            return Ok(courses);
        }

        /// <returns></returns>
        [Route("api/eLearning/Courses/Start")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Start(int id, string userId = null)
        {
            var entity = await db.CourseModules.Where(x => x.CourseId == id).OrderBy(x => x.Order).FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        /// <returns></returns>
        [Route("api/eLearning/Courses/IsUserEnrolled")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> IsUserEnrolled(int id, int userId, string enrollmentCode = "")
        {
            if (!String.IsNullOrEmpty(enrollmentCode))
            {
                var courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id &&
                    x.EnrollmentCode.Equals(enrollmentCode, StringComparison.OrdinalIgnoreCase));

                if (courseEvent != null)
                {
                    var enrollment = await db.Enrollments.FirstOrDefaultAsync(x => x.CourseId == id &&
                        x.CourseEventId == courseEvent.Id && x.Learner.User.Id == userId &&
                        (x.Status == EnrollmentStatus.Enrolled || x.Status == EnrollmentStatus.Completed));

                    if (enrollment != null)
                        return Ok(true);
                }
            }
            else
            {
                // public course
                var enrollment = await db.Enrollments.FirstOrDefaultAsync(x => x.CourseId == id &&
                    x.Learner.User.Id == userId &&
                    !x.CourseEvent.IsTrial &&
                    (x.Status == EnrollmentStatus.Enrolled || x.Status == EnrollmentStatus.Completed));

                if (enrollment != null)
                    return Ok(true);
            }

            return Ok(false);
        }

        [Route("api/eLearning/Courses/IsUserCompleted")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> IsUserCompleted(int id, int userId)
        {
            if (ModelState.IsValid)
            {
                var enrollment = await db.Enrollments.FirstOrDefaultAsync(x => x.Learner.User.Id == userId &&
                    x.CourseId == id && !x.CourseEvent.IsTrial &&
                   (x.Status == EnrollmentStatus.Enrolled || x.Status == EnrollmentStatus.Completed));

                var entity = new UserCourseEnrollmentModel();

                if (enrollment != null)
                {
                    entity = new UserCourseEnrollmentModel
                    {
                        Id = enrollment.Id,
                        StudentName = enrollment.Learner.User.Name,
                        Status = enrollment.Status,
                        CompletionDate = enrollment.CompletionDate.ToString(),
                        IsUserEnrolled = true,
                        CourseEventId = enrollment.CourseEventId
                    };

                    return Ok(entity);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("api/eLearning/Courses/ViewCertificate")]
        [HttpGet]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> ViewCertificate(int id)
        {
            var enrollment = await db.Enrollments.Include(x => x.Course).Include(x => x.Learner).FirstOrDefaultAsync(x => x.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            var entity = new ViewCertificateModel
            {
                CourseId = enrollment.CourseId,
                CourseName = enrollment.Course.Title,
                StudentName = enrollment.Learner.User.Name,
                EnrollmentStatus = enrollment.Status,
                DateCompleted = DateTime.Parse(enrollment.CompletionDate.ToString()).ToShortDateString(),
            };

            var bg = db.CourseCertificates.FirstOrDefault(x => x.Id == enrollment.Course.CourseCertificateId);

            if (bg != null)
            {
                entity.Background = bg;
            }

            var temp = db.CourseCertificateTemplates.FirstOrDefault(x => x.Id == enrollment.Course.CourseCertificateTemplateId);

            if (temp != null)
            {
                temp.Template = temp.Template.Replace("{{StudentName}}", entity.StudentName);
                temp.Template = temp.Template.Replace("{{CourseName}}", entity.CourseName);
                temp.Template = temp.Template.Replace("{{DateCompleted}}", entity.DateCompleted);

                entity.Template = temp;
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