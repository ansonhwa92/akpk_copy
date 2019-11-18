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
    [Route("api/eLearning/CourseEnrollments")]
    public class CourseEnrollmentsController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEnrollments/Get")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseEnrollmentModel request)
        {
            var query = db.Enrollments
                .Include(x => x.Group)
                .Include(x => x.Learner.User)
                .Where(x => x.CourseId == request.CourseId);

            if (request.CourseEventId > 0)
            {
                query = query.Where(x => x.CourseEventId == request.CourseEventId);
            }
            else
            {
                var publicEvent = db.CourseEvents.FirstOrDefault(x => x.CourseId == request.CourseId && x.EnrollmentCode.ToUpper().Contains("PUBLIC"));

                if (publicEvent != null)
                    query = query.Where(x => x.CourseEventId == publicEvent.Id);
                else
                    query = query.Where(x => x.CourseEventId < 0);
            }

            if (!String.IsNullOrEmpty(request.StudentName))
                query = query.Where(x => x.Learner.User.Name.ToLower().Contains(request.StudentName.ToLower()));

            var totalCount = query.Count();

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(x => x.Learner.User.Name.ToLower().Contains(request.StudentName.ToLower()));
            }

            var filteredCount = query.Count();

            if (filteredCount > 0)
            {
                // TODO : Add also course progress, PercentageCompleted,

                //order
                if (request.order != null)
                {
                    string sortBy = request.columns[request.order[0].column].data;
                    bool sortAscending = request.order[0].dir.ToLower() == "asc";

                    switch (sortBy)
                    {
                        case "StudentName":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.Learner.User.Name);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.Learner.User.Name);
                            }

                            break;

                        case "DateEnrolled":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.EnrolledDate);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.EnrolledDate);
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

                        case "PercentageCompleted":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.PercentageCompleted);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.PercentageCompleted);
                            }

                            break;

                        default:
                            query = query.OrderBy(o => o.Learner.User.Name);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(o => o.Learner.User.Name);
                }
            }

            var finalResult = query.ToList();

            var data = finalResult.Skip(request.start).Take(request.length)
               .Select(x => new ReturnBriefCourseEnrollmentModel
               {
                   CourseEventId = x.CourseEventId,
                   StudentName = String.IsNullOrEmpty(x.Learner.User.Name) ? "" : x.Learner.User.Name,
                   DateEnrolled = x.EnrolledDate.ToString(),
                   Status = x.Status,
                   PercentageCompleted = x.PercentageCompleted.ToString()
               }).ToArray();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data
            });
        }

        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEnrollments/GetUsers")]
        [HttpPost]
        public IHttpActionResult GetUsers(FilterCourseEnrollmentModel request)
        {
            var query = db.Enrollments
                .Include(x => x.Group)
                .Include(x => x.Learner.User)
                .Include(x => x.CourseEvent)
                .Where(x => x.CourseId == request.CourseId);

            if (!String.IsNullOrEmpty(request.StudentName))
                query = query.Where(x => x.Learner.User.Name.ToLower().Contains(request.StudentName.ToLower()));

            if (!String.IsNullOrEmpty(request.SessionName))
                query = query.Where(x =>x.CourseEvent.Name.ToLower().Contains(request.SessionName.ToLower()));

            var totalCount = query.Count();

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(x => x.Learner.User.Name.ToLower().Contains(value)
               || x.CourseEvent.Name.Contains(value)
               || x.PercentageCompleted.ToString().Contains(value)
               //|| x.Status.ToString().Contains(value)
               );
            }

            var filteredCount = query.Count();

            if (filteredCount > 0)
            {
                // TODO : Add also course progress, PercentageCompleted,

                //order
                if (request.order != null)
                {
                    string sortBy = request.columns[request.order[0].column].data;
                    bool sortAscending = request.order[0].dir.ToLower() == "asc";

                    switch (sortBy)
                    {
                        case "StudentName":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.Learner.User.Name);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.Learner.User.Name);
                            }

                            break;

                        case "DateEnrolled":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.EnrolledDate);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.EnrolledDate);
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

                        case "PercentageCompleted":

                            if (sortAscending)
                            {
                                query = query.OrderBy(o => o.PercentageCompleted);
                            }
                            else
                            {
                                query = query.OrderByDescending(o => o.PercentageCompleted);
                            }

                            break;

                        default:
                            query = query.OrderBy(o => o.Learner.User.Name);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(o => o.Learner.User.Name);
                }
            }

            var finalResult = query.ToList();

            var data = finalResult.Skip(request.start).Take(request.length)
               .Select(x => new ReturnBriefCourseEnrollmentModel
               {
                   Id = x.Id,
                   CourseEventId = x.CourseEventId,
                   SessionName = db.CourseEvents.Find(x.CourseEventId).Name,
                   StudentName = String.IsNullOrEmpty(x.Learner.User.Name) ? "" : x.Learner.User.Name,
                   DateEnrolled = x.EnrolledDate.ToString(),
                   Status = x.Status,
                   PercentageCompleted = x.PercentageCompleted.ToString(),
                   CompletionDate = x.CompletionDate.ToString()
               }).ToArray();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data
            });
        }

        [Route("api/eLearning/CourseEnrollments/GetUserDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserDetails (int id)
        {
            //var entity = db.Enrollments
            //    .Include(x => x.Learner)
            //    .Include(x => x.Course)
            //    .Include(x => x.CourseEvent)
            //    .Include(x => x.CourseProgress)
            //    .Include(x => x.EnrollmentHistories)
            //    .FirstOrDefault(x => x.Id == id);

            //if (entity == null)
            //{
            //    return NotFound();
            //}

        var user = await db.Enrollments
            .Include(x => x.Learner)
            .Include(x => x.Course)
            .Include(x => x.CourseEvent)
            .Include(x => x.CourseProgress)
            .Include(x => x.EnrollmentHistories)
            .FirstOrDefaultAsync(x => x.Id == id);

            var entity = new UserCourseEnrollmentModel
            {
                Id = user.Id,
                StudentName = String.IsNullOrEmpty(user.Learner.User.Name) ? "" : user.Learner.User.Name,
                SessionName = db.CourseEvents.Find(user.CourseEventId).Name,
                CourseEventId = user.CourseEventId,
                CourseTitle = user.Course.Title,
                DateEnrolled = user.EnrolledDate.ToString(),
                Status = user.Status,
                CoursePercentageCompleted = user.PercentageCompleted.ToString(),
                CompletionDate = user.CompletionDate.ToString(),
                //CourseProgress = user.CourseProgress,
                EnrollmentHistory = user.EnrollmentHistories
            };

            var courseProgress = db.CourseProgress.Where(x => x.EnrollmentId == entity.Id).ToList();

            var progress = new List<ReturnCourseProgressModel>();

            foreach (var item in courseProgress)
            {
                var module = db.CourseModules.Find(item.ModuleId);

                progress.Add(new ReturnCourseProgressModel
                {
                    EnrollmentId = item.EnrollmentId,
                    ModuleName = module.Title,
                    IsCompleted = item.IsCompleted,
                    Score = item.Score,
                });
            }
            entity.CourseProgress = progress;

            return Ok(entity);

        }




        /// For use in view content, when user wants to ernoll
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEnrollments/EnrollAsync")]
        [HttpGet]
        public async Task<IHttpActionResult> EnrollAsync(int id, int userId, string enrollmentCode = "")
        {
            var learner = await db.Learners.FirstOrDefaultAsync(x => x.UserId == userId);

            if (learner == null) // create new learner
            {
                var user = await db.User.FindAsync(userId);

                if (user == null)
                    return Ok(new TrxResult<Enrollment>
                    {
                        CourseId = id,
                        IsSuccess = false,
                        Message = "User does not exist",
                    });

                learner = new Learner
                {
                    UserId = userId,
                };

                db.Learners.Add(learner);

                await db.SaveChangesAsync();
            }

            // enroll as public
            if (String.IsNullOrEmpty(enrollmentCode))
            {
                var courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id && x.ViewCategory == ViewCategory.Public);

                if (courseEvent != null)
                {
                    var course = await db.Courses.FirstOrDefaultAsync(x => x.Id == courseEvent.CourseId);

                    if (course == null || course?.ViewCategory == ViewCategory.Private)
                    {
                        return Ok(new TrxResult<Enrollment>
                        {
                            CourseId = id,
                            IsSuccess = false,
                            Message = "Course is not opened for public.",
                        });
                    }

                    var enrollment = new Enrollment
                    {
                        LearnerId = learner.Id,
                        CourseEventId = courseEvent.Id,
                        CourseId = id,
                        CreatedDate = DateTime.Now,
                        EnrolledDate = DateTime.Now,
                        CreatedBy = userId,
                        Status = EnrollmentStatus.Enrolled,
                    };

                    db.Enrollments.Add(enrollment);

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        var log = new ErrorLog
                        {
                            CreatedDate = DateTime.Now,
                            UserId = userId,
                            Module = Modules.Learning,
                            Source = " Controller: eLearning/CourseEnrollmentApi Action: EnrollAsync",
                            ErrorDescription = "Error enrolling student - User does not exist. Userid - " + userId,
                            ErrorDetails = $"CourseEvent - id = {courseEvent.Id}, exception - {e.Message}",
                            IPAddress = "",
                        };

                        db.ErrorLog.Add(log);
                        await db.SaveChangesAsync();
                    }

                    // create a history
                    db.EnrollmentHistories.Add(new EnrollmentHistory
                    {
                        CreatedDate = DateTime.Now,
                        EnrollmentId = enrollment.Id,
                        LearnerId = learner.Id,
                        Remark = "User Enrolled",
                        Status = EnrollmentStatus.Enrolled,
                        CourseEventId = courseEvent.Id,
                        CourseId = courseEvent.CourseId,
                        UserId = userId,
                    });

                    await db.SaveChangesAsync();
                }
                else
                {
                    var log = new ErrorLog
                    {
                        CreatedDate = DateTime.Now,
                        UserId = userId,
                        Module = Modules.Learning,
                        Source = " Controller: eLearning/CourseEnrollmentApi Action: EnrollAsync",
                        ErrorDescription = "Error enrolling student - either PUBLIC Course Event Does not exist or user does not exist.",
                        ErrorDetails = $"CourseEvent - id = {courseEvent.Id}",
                        IPAddress = "",
                    };

                    db.ErrorLog.Add(log);

                    db.SaveChanges();

                    return Ok(new TrxResult<Enrollment>
                    {
                        CourseId = id,
                        IsSuccess = false,
                        Message = "Public Course Event does not exist.",
                    });
                }
            }
            else
            {
                var courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id &&
                     x.EnrollmentCode.Equals(enrollmentCode, StringComparison.OrdinalIgnoreCase));

                if (courseEvent != null)
                {
                    var enrollment = new Enrollment
                    {
                        LearnerId = learner.Id,
                        CourseEventId = courseEvent.Id,
                        CourseId = id,
                        CreatedDate = DateTime.Now,
                        EnrolledDate = DateTime.Now,
                        CreatedBy = userId,
                        Status = EnrollmentStatus.Enrolled,
                    };

                    db.Enrollments.Add(enrollment);

                    await db.SaveChangesAsync();

                    // create a history
                    db.EnrollmentHistories.Add(new EnrollmentHistory
                    {
                        CreatedDate = DateTime.Now,
                        EnrollmentId = enrollment.Id,
                        LearnerId = learner.Id,
                        Remark = "User Enrolled",
                        Status = EnrollmentStatus.Enrolled,
                        CourseEventId = courseEvent.Id,
                        CourseId = courseEvent.CourseId,
                        UserId = userId,
                    });

                    await db.SaveChangesAsync();
                }
                else
                {
                    var log = new ErrorLog
                    {
                        CreatedDate = DateTime.Now,
                        UserId = userId,
                        Module = Modules.Learning,
                        Source = " Controller: eLearning/CourseEnrollmentApi Action: EnrollAsync",
                        ErrorDescription = "Error enrolling student - PRIVATE Course Event Does not exist",
                        ErrorDetails = $"CourseEvent - id = {courseEvent.Id}, EnrollmentCode = {enrollmentCode}",
                        IPAddress = "",
                    };

                    db.ErrorLog.Add(log);

                    db.SaveChanges();

                    return Ok(new TrxResult<Enrollment>
                    {
                        CourseId = id,
                        IsSuccess = false,
                        Message = "Private Course Event does not exist",
                    });
                }
            }
            return Ok(new TrxResult<Enrollment>
            {
                CourseId = id,
                IsSuccess = true,
                Message = "Success",
            });
        }

        [Route("api/eLearning/CourseEnrollments/GetEnrollment")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEnrollment(int id, int userId, string enrollmentCode = "")
        {
            if (!String.IsNullOrEmpty(enrollmentCode))
            {
                var courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id &&
                    x.EnrollmentCode.Equals(enrollmentCode, StringComparison.OrdinalIgnoreCase));

                if (courseEvent != null)
                {
                    var enrollment = await db.Enrollments.FirstOrDefaultAsync(x => x.CourseId == id &&
                        x.CourseEventId == courseEvent.Id && x.Learner.User.Id == userId);

                    if (enrollment != null)
                        return Ok(enrollment);
                }
            }
            else
            {
                var enrollment = db.Enrollments.Where(x => x.Learner.User.Id == userId &&
                            x.CourseId == id).OrderBy(x => x.CreatedDate).FirstOrDefault();

                if (enrollment != null)
                    return Ok(enrollment);
            }

            return BadRequest();
        }

        [Route("api/eLearning/CourseEnrollments/GetEnrollmentHistoryByCourse")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEnrollmentHistoryByCourse(int userId, int courseId)
        {
            var learner = await db.Learners.FirstOrDefaultAsync(x => x.UserId == userId);

            if (learner == null)
                return BadRequest("User Not Found");


            var history = db.EnrollmentHistories.Where(x => x.LearnerId == learner.Id &&
                    x.CourseId == courseId);

            if (history != null)
                return Ok(history);

            return BadRequest();

        }
    }             
}