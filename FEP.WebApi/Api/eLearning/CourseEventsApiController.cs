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
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/CourseEvents")]
    public class CourseEventsController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseEventsController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseEventModel, CourseEvent>();

                cfg.CreateMap<CourseEvent, CourseEventModel>();
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

        [Route("api/eLearning/CourseEvents/")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseEvents
                   .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CourseEventModel>(entity);

            return Ok(model);
        }

        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEvents/GetAllEventsByCourse")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseEventModel request)
        {
            var query = db.CourseEvents
                .Include(x => x.Group)
                .Where(x => x.CourseId == request.CourseId && x.IsDisplayed == true);

            if (!String.IsNullOrEmpty(request.Name))
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));

            if (!String.IsNullOrEmpty(request.EnrollmentCode))
                query = query.Where(x => x.EnrollmentCode.ToLower().Contains(request.EnrollmentCode.ToLower()));

            var totalCount = query.Count();

            query = query.OrderBy(x => x.CreatedDate);

            //quick search,  and hide course where ViewCategory.Public and Status = Private
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Name.Contains(value) || p.EnrollmentCode.Contains(value));
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(x => new ReturnBriefCourseEventModel
                {
                    CourseEventId = x.Id,
                    Name = x.Name,
                    EnrollmentCode = x.EnrollmentCode,
                    Group = x.Group.Name,
                    NumberOfLearners = db.Enrollments
                        .Where(y => y.CourseEventId == x.Id).Count()
                });

            //foreach(var item in data)
            //{
            //    var numberOfLearners = db.Enrollments.Where(x => x.CourseEventId == item.CourseEventId).ToList();

            //    item.NumberOfLearners = numberOfLearners == null ? 0 : numberOfLearners.Count();
            //}

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
                            data = data.OrderBy(o => o.Name);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.Name);
                        }

                        break;

                    case "EnrollmentCode":

                        if (sortAscending)
                        {
                            data = data.OrderBy(o => o.EnrollmentCode);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.EnrollmentCode);
                        }

                        break;

                    case "NumberOfLearners":

                        if (sortAscending)
                        {
                            data = data.OrderBy(o => o.NumberOfLearners);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.NumberOfLearners);
                        }

                        break;

                    case "Group":

                        if (sortAscending)
                        {
                            data = data.OrderBy(o => o.Group);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.Group);
                        }

                        break;

                    default:
                        data = data.OrderBy(o => o.Name).OrderBy(o => o.Name);
                        break;
                }
            }
            else
            {
                data = data.OrderBy(o => o.Name).OrderBy(o => o.Name);
            }

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        /// For use in Invitation page, to list all invited users
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEvents/GetInvitationByCourseEventId")]
        [HttpPost]
        public IHttpActionResult GetInvitationByCourseEventId(FilterCourseInvitationModel request)
        {
            var query = db.CourseInvitations
                .Where(x => x.CourseEventId == request.CourseEventId);

            if (!String.IsNullOrEmpty(request.Email))
                query = query.Where(x => x.Emails.ToLower().Contains(request.Email.ToLower()));

            var totalCount = query.Count();

            query = query.OrderBy(x => x.CreatedDate);

            //quick search,  and hide course where ViewCategory.Public and Status = Private
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Emails.Contains(value));
            }

            
            var data = query.Skip(request.start).Take(request.length)
                .Select(x => new ReturnBriefCourseInvitationModel
                {
                    CourseEventId = x.Id,
                    Name = "",
                    Email = x.Emails,
                    Status = EnrollmentStatus.Invited,
                });

            var tempData = data.ToList();

            foreach (var item in tempData)
            {
                var learner = db.Enrollments.FirstOrDefault(x => x.CourseEventId == item.CourseEventId &&
                    x.Learner.User.Email == request.Email);

                if (learner != null)
                {
                    item.Name = learner.Learner.User.Name;
                    item.Status = learner.Status;
                }
                else
                {
                    item.Status = EnrollmentStatus.Invited;
                }
            }

            data = tempData.AsQueryable();          
            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Email":

                        if (sortAscending)
                        {
                            data = data.OrderBy(o => o.Email);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.Email);
                        }

                        break;

                    case "Name":

                        if (sortAscending)
                        {
                            data = data.OrderBy(o => o.Name);
                        }
                        else
                        {
                            data = data.OrderByDescending(o => o.Name);
                        }

                        break;

                    default:
                        data = data.OrderBy(o => o.Email).OrderBy(o => o.Email);
                        break;
                }
            }
            else
            {
                data = data.OrderBy(o => o.Email).OrderBy(o => o.Email);
            }

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        [Route("api/eLearning/CourseEvents/GetEventByCourseId")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventByCourseId(int? id)
        {
            var entity = await db.CourseEvents
                .Where(x => x.CourseId == id.Value)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            if (entity == null)
                return NotFound();

            var model = _mapper.Map<CourseEventModel>(entity);

            return Ok(model);
        }

        [Route("api/eLearning/CourseEvents/StartTrial")]
        public async Task<IHttpActionResult> StartTrial(int id, int createdBy)
        {
            var entity = await db.Courses
                            .Include(x => x.CourseApprovalLog)
                            .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return BadRequest();

            if (entity.Status == CourseStatus.Trial)
            {
                return BadRequest("The course is already in trial mode.");
            }

            try
            {
                if (entity.Status == CourseStatus.Draft)
                {
                    entity.Status = CourseStatus.Trial;
                    db.SetModified(entity);

                    var newEvent = new CourseEvent
                    {
                        Name = "Trial Event",
                        CourseId = entity.Id,
                        Status = CourseEventStatus.Trial,
                        Start = DateTime.Now,
                        EnrollmentCode = $"TRIAL({entity.Code}-{DateTime.Now.Ticks})",
                        ViewCategory = ViewCategory.Private,
                        CreatedBy = createdBy,
                        IsTrial = true
                    };

                    db.CourseEvents.Add(newEvent);

                    await db.SaveChangesAsync();

                    if (entity.CourseApprovalLog == null)
                        entity.CourseApprovalLog = new List<CourseApprovalLog>();

                    entity.CourseApprovalLog.Add(new CourseApprovalLog
                    {
                        CreatedByName = entity.CreatedByName,
                        ActionDate = DateTime.Now,
                        Remark = "Course " + entity.Title + " goes for Trial.",
                        ApprovalStatus = ApprovalStatus.None
                    });

                    await db.SaveChangesAsync();

                    ChangeCourseStatusModel data = new ChangeCourseStatusModel
                    {
                        CourseId = entity.Id,
                        CourseName = entity.Title,
                        CourseEventId = newEvent.Id,
                        Message = "New course event created"
                    };

                    return Ok(data);
                }
                else
                {
                    return BadRequest("Cannot go to Trial if status is not in Draft");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " " + e.InnerException.Message);
            }
        }

        [Route("api/eLearning/CourseEvents/StopTrial")]
        public async Task<IHttpActionResult> StopTrial(int id)
        {
            var entity = await db.Courses
                .Include(x => x.CourseApprovalLog)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return BadRequest();

            if (entity.Status != CourseStatus.Trial)
                return BadRequest("Cannot stop. Course is not in Trial mode.");

            entity.Status = CourseStatus.Draft;

            // insert into course event
            var courseEvent = await db.CourseEvents.Where(x => x.CourseId == entity.Id &&
               x.Status == CourseEventStatus.Trial).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();

            if (courseEvent != null)
            {
                courseEvent.End = DateTime.Now;
                courseEvent.TrialRemark = "Trial ended";
                courseEvent.Status = CourseEventStatus.TrialEnded;

                db.SetModified(entity);
                db.SetModified(courseEvent);

                await db.SaveChangesAsync();

                entity.CourseApprovalLog.Add(new CourseApprovalLog
                {
                    CreatedByName = entity.CreatedByName,
                    ActionDate = DateTime.Now,
                    Remark = "Course " + entity.Title + " stop Trial.",
                });

                await db.SaveChangesAsync();

                ChangeCourseStatusModel data = new ChangeCourseStatusModel
                {
                    CourseId = entity.Id,
                    CourseName = entity.Title,
                    Message = "Trial stop for course " + entity.Title,
                };

                return Ok(data);
            }
            else
            {
                return BadRequest("No event to stop.");
            }
        }

        // course => courseEvent => Enrollment

        [Route("api/eLearning/CourseEvents/GetEnrollment")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEnrollment([FromUri]int courseId, int? courseEventId, [FromBody] FilterUserModel request)
        {
            Course course = await db.Courses
                                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                return BadRequest();

            CourseEvent courseEvent = new CourseEvent();
            var users = new List<User>();

            if (course.Status == CourseStatus.Trial)
            {
                courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == course.Id
                    && x.Status == CourseEventStatus.Trial);
            }

            if (course.Status == CourseStatus.Published)
            {
                if (courseEventId != null)
                    courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.Id == courseEventId.Value);
                else
                    courseEvent = await db.CourseEvents.Where(x => x.CourseId == courseId).OrderByDescending(x => x.CreatedDate)
                            .FirstOrDefaultAsync();
            }

            if (courseEvent != null)
            {
                users = db.Enrollments
                    .Include(x => x.Learner)
                    .Where(x => x.CourseEventId == courseEvent.Id && x.Status == EnrollmentStatus.Enrolled)
                    .Select(x => x.Learner.User).ToList();
            }
            else
                return BadRequest();

            if (users == null)
                return NotFound();

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

        [Route("api/eLearning/CourseEvents/GetAllRoleLearner")]
        [HttpPost]
        public IHttpActionResult GetAllRoleLearner(FilterIndividualModel request)
        {
            var users = db.UserRole.Where(u => u.UserAccount.User.Display && u.Role.Name == RoleNames.eLearningLearner).Select(x => x.UserAccount.User);

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


        //Get Individual user that has learner role
        [Route("api/eLearning/CourseEvents/GetAllIndividuals")]
        [HttpPost]
        public IHttpActionResult GetAllLearners(FilterIndividualModel request)
        {
            var users = db.UserRole.Where(u => u.UserAccount.User.Display &&
                u.Role.Name == RoleNames.eLearningLearner).Select(x => x.UserAccount.User);

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

        //Get Agency/Company user that has learners role
        [Route("api/eLearning/CourseEvents/GetAllAgency")]
        [HttpPost]
        public IHttpActionResult Post(FilterCompanyModel request)
        {
            var query = db.UserRole.Where(u => u.UserAccount.User.Display && u.UserAccount.User.UserType == UserType.Company &&
                u.Role.Name == RoleNames.eLearningLearner).Select(x => x.UserAccount.User.CompanyProfile);

            //var query = db.CompanyProfile.Where(u => u.User.Display && u.User.UserType == UserType.Company);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.CompanyName == null || s.CompanyName.Contains(request.CompanyName))
               && (request.Email == null || s.User.Email.Contains(request.Email))
               && (request.Type == null || s.Type == request.Type)
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.CompanyName.Contains(value)
                || p.CompanyRegNo.Contains(value)
                || p.Sector.Name.Contains(value)
                || p.User.Email.Contains(value)
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
                    case "CompanyName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CompanyName);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CompanyName);
                        }

                        break;

                    case "TypeDesc":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Type);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Type);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.User.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Email);
                        }

                        break;

                    case "Sector":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Sector.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Sector.Name);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.CompanyName);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.CompanyName);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new CompanyModel
                {
                    Id = s.UserId,
                    CompanyName = s.CompanyName,
                    Email = s.User.Email,
                    Type = s.Type,
                    Status = s.User.UserAccount != null ? s.User.UserAccount.IsEnable : false
                }).ToList();

            data.ForEach(s => s.TypeDesc = s.Type.GetDisplayName());

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        //Get staff user that has learners role
        [Route("api/eLearning/CourseEvents/GetAllStaff")]
        [HttpPost]
        public IHttpActionResult Post(FilterStaffModel request)
        {
            var query = db.UserRole.Where(u => u.UserAccount.User.Display && u.UserAccount.User.UserType == UserType.Staff &&
               u.Role.Name == RoleNames.eLearningLearner).Select(x => x.UserAccount.User.StaffProfile);

            //var query = db.StaffProfile.Where(u => u.User.Display && u.User.UserType == UserType.Staff);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Name == null || s.User.Name.Contains(request.Name))
               && (request.BranchId == null || s.BranchId == request.BranchId)
               && (request.Email == null || s.User.Email.Contains(request.Email))
               && (request.DepartmentId == null || s.DepartmentId == request.DepartmentId)
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.User.Name.Contains(value)
                || p.User.Email.Contains(value)
                || p.Branch.Name.Contains(value)
                || p.Department.Name.Contains(value)
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
                            query = query.OrderBy(o => o.User.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Name);
                        }

                        break;

                    case "Branch":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Branch.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Branch.Name);
                        }

                        break;

                    case "Department":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Department.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Department.Name);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.User.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Email);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.User.Name);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.User.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new StaffModel
                {
                    Id = s.User.Id,
                    Name = s.User.Name,
                    Email = s.User.Email,
                    Branch = s.Branch.Name,
                    Department = s.Department.Name,
                    Status = s.User.UserAccount.IsEnable
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        [Route("api/eLearning/CourseEvents/AddLearner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLearner(UpdateLearnerEnrollmentModel model)
        {
            try
            {
                var courseEvent = await db.CourseEvents.FindAsync(model.CourseEventId);

                foreach (var userId in model.UserId)
                {
                    var learner = await db.Learners
                                        .Include(x => x.User)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (learner == null)
                    {
                        learner = new Learner
                        {
                            UserId = userId,
                            Point = 0,
                            CourseEnrolled = 1
                        };

                        db.Learners.Add(learner);
                        db.SaveChanges();
                    }

                    var enrollment = db.Enrollments.FirstOrDefault(x => x.CourseEventId == courseEvent.Id &&
                            x.LearnerId == learner.Id);

                    if (enrollment == null)
                    {
                        var newEnrollment = new Enrollment
                        {
                            CourseId = courseEvent.CourseId,
                            CourseEventId = courseEvent.Id,
                            LearnerId = learner.Id,
                            EnrolledDate = DateTime.Now,
                            Status = EnrollmentStatus.Enrolled,
                        };

                        db.Enrollments.Add(newEnrollment);

                        db.SaveChanges();

                        newEnrollment.EnrollmentHistories = new List<EnrollmentHistory>
                        {
                            new EnrollmentHistory
                            {
                                EnrollmentId = newEnrollment.Id,
                                LearnerId = learner.Id,
                                Status = EnrollmentStatus.Enrolled,
                                UserId = userId,
                                CourseId = courseEvent.CourseId,
                                CourseEventId = courseEvent.Id
                            }
                        };

                        db.SetModified(newEnrollment);
                        db.SaveChanges();
                    }
                    else
                    {
                        enrollment.Status = EnrollmentStatus.Enrolled;
                        enrollment.EnrollmentHistories.Add(new EnrollmentHistory
                        {
                            EnrollmentId = enrollment.Id,
                            LearnerId = learner.Id,
                            Status = EnrollmentStatus.Enrolled,
                            Remark = "Learner reenrolled",
                            UserId = userId,
                            CourseId = courseEvent.CourseId,
                            CourseEventId = courseEvent.Id
                        }
                        );

                        db.SetModified(enrollment);
                        db.SaveChanges();
                    }
                }

                return Ok(courseEvent.CourseId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Unenrolled learner
        [Route("api/eLearning/CourseEvents/RemoveLearner")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveLearner(UpdateLearnerEnrollmentModel model)
        {
            try
            {
                var courseEvent = await db.CourseEvents.FindAsync(model.CourseEventId);

                foreach (var userId in model.UserId)
                {
                    var learner = await db.Learners
                                        .Include(x => x.User)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (learner == null)
                    {
                        return BadRequest("Unable to unenroll learner");
                    }

                    var enrollment = db.Enrollments.FirstOrDefault(x => x.CourseEventId == courseEvent.Id &&
                            x.LearnerId == learner.Id && x.Status == EnrollmentStatus.Enrolled);

                    if (enrollment == null)
                    {
                        return BadRequest("Unable to unenroll learner");
                    }

                    enrollment.Status = EnrollmentStatus.Removed;

                    enrollment.EnrollmentHistories.Add(
                            new EnrollmentHistory
                            {
                                EnrollmentId = enrollment.Id,
                                LearnerId = learner.Id,
                                Status = EnrollmentStatus.Removed,
                                Remark = "Removed from Enrollment"
                            }
                        );

                    db.SetModified(enrollment);
                    db.SaveChanges();
                }

                return Ok(courseEvent.CourseId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Called by counter part in intranet
        // Here, the course status will be changed to Publish.
        // By default, A public event will becreated.
        // If ViewCategory is Private, the CourseEvent will be created
        // once the user create/invite a group with an enrollment code.
        // A course can only have 1 Public course event
        [Route("api/eLearning/CourseEvents/Publish")]
        [HttpPost]
        public async Task<IHttpActionResult> Publish(int id, int createdBy)
        {
            var entity = await db.Courses.FindAsync(id);
            if (entity == null)
            {
                return BadRequest();
            }

            CourseEvent courseEvent = new CourseEvent();

            // check whether a public event has been created
            var publicEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id && x.ViewCategory == ViewCategory.Public);

            if (publicEvent == null)
            {
                var newEvent = new CourseEvent
                {
                    Name = "Public Course",
                    CourseId = id,
                    AllowablePercentageBeforeWithdraw = entity.DefaultAllowablePercentageBeforeWithdraw,
                    CreatedBy = createdBy,
                    EnrollmentCode = $"PUBLIC ({entity.Id})",
                    ViewCategory = ViewCategory.Public,
                    Status = entity.ViewCategory == ViewCategory.Public ? CourseEventStatus.AvailableToPublic : CourseEventStatus.AvailableToPrivate,
                    Start = DateTime.Now,
                    IsDisplayed = entity.ViewCategory == ViewCategory.Public ? true : false
                };

                db.CourseEvents.Add(newEvent);

                await db.SaveChangesAsync();
            }

            //sum total content - added by wawar

            var totalContent = db.CourseContents.Where(x => x.CourseId == id);
            entity.TotalContents = totalContent.Count();

            var totalModule = db.CourseModules.Where(x => x.CourseId == id);
            entity.TotalModules = totalContent.Count();

            //end sum

            entity.Status = CourseStatus.Published;

            if (entity.CourseApprovalLog == null)
                entity.CourseApprovalLog = new List<CourseApprovalLog>();

            var createdByName = "";
            var user = await db.User.FindAsync(createdBy);
            if (user != null)
                createdByName = user.Name;

            entity.CourseApprovalLog.Add(new CourseApprovalLog
            {
                CreatedByName = createdByName,
                ActionDate = DateTime.Now,
                Remark = "Course " + entity.Title + " is published.",
                ApprovalStatus = ApprovalStatus.None
            });

            await db.SaveChangesAsync();

            ChangeCourseStatusModel data = new ChangeCourseStatusModel
            {
                CourseId = entity.Id,
                CourseName = entity.Title,
                Message = "Published"
            };
            return Ok(data);
        }

        /// <summary>
        /// Create course event/session.
        /// If HasGroup is true, a group will also be created for the session
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEvents/Create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(CourseEventModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await db.Courses.FindAsync(model.CourseId);
                if (entity == null)
                {
                    return BadRequest();
                }

                var courseEvent = _mapper.Map<CourseEvent>(model);

                courseEvent.Status = entity.ViewCategory == ViewCategory.Private ?
                    CourseEventStatus.AvailableToPrivate : CourseEventStatus.AvailableToPublic;

                try
                {
                    db.CourseEvents.Add(courseEvent);

                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest("Fail save event. - " + e.Message + " " + e.InnerException);
                }

                if (model.HasGroup)
                {
                    var group = new Group
                    {
                        CourseEventId = courseEvent.Id,
                        CourseId = model.CourseId,
                        EnrollmentCode = model.EnrollmentCode,
                        CreatedBy = model.CreatedBy,
                        UpdatedBy = model.CreatedBy,
                        Description = model.Description,
                        IsVisible = true,
                        Name = model.Name,
                    };

                    try
                    {
                        db.Groups.Add(group);

                        await db.SaveChangesAsync();

                        courseEvent.GroupId = group.Id;
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Session created, but group cannot be created");
                    }

                    db.SetModified(courseEvent);
                    await db.SaveChangesAsync();
                }

                TrxResult<CourseEvent> response = new TrxResult<CourseEvent>
                {
                    CourseId = model.CourseId,
                    IsSuccess = true,
                    Message = "Success",
                    ObjectId = courseEvent.Id,
                };

                return Ok(response);
            }

            return BadRequest("Invalid data");
        }

        /// <summary>
        /// Invite learners by email. Called by intranet, same name
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEvents/InviteLearners")]
        [HttpPost]
        public async Task<IHttpActionResult> InviteLearners(InviteLearnerModel model)
        {
            if (ModelState.IsValid)
            {
                var courseEvent = await db.CourseEvents.FindAsync(model.CourseEventId);

                if (courseEvent == null)
                    return BadRequest("No course event found");

                if (String.IsNullOrEmpty(model.LearnerEmails))
                    return BadRequest("No emails found.");

                var emails = model.LearnerEmails.Split(',');

                foreach (var email in emails)
                {
                    db.CourseInvitations.Add(new CourseInvitation
                    {
                        CourseId = model.CourseId,
                        CourseEventId = model.CourseEventId,
                        NotificationType = NotificationType.Course_Invitation,
                        CreatedBy = int.Parse(model.CreatedBy),
                        Emails = email,
                    });
                }
                await db.SaveChangesAsync();
            }
            return Ok();
        }
    }
}