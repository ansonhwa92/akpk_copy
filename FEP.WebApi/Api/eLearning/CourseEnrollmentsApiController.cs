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
                .Where(x => x.CourseEventId == request.CourseEventId);

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

            var data = new List<ReturnBriefCourseEnrollmentModel>().AsQueryable();

            if (filteredCount > 0)
            {
                // TODO : Add also course progress, PercentageCompleted,
                 data = query.Skip(request.start).Take(request.length)
                .Select(x => new ReturnBriefCourseEnrollmentModel
                {
                    CourseEventId = x.CourseEventId,
                    StudentName = String.IsNullOrEmpty(x.Learner.User.Name) ? "" : x.Learner.User.Name,
                    DateEnrolled = x.EnrolledDate,
                    Status = x.Status
                });

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
                                data = data.OrderBy(o => o.StudentName);
                            }
                            else
                            {
                                data = data.OrderByDescending(o => o.StudentName);
                            }

                            break;

                        case "DateEnrolled":

                            if (sortAscending)
                            {
                                data = data.OrderBy(o => o.DateEnrolled);
                            }
                            else
                            {
                                data = data.OrderByDescending(o => o.DateEnrolled);
                            }

                            break;

                        case "Status":

                            if (sortAscending)
                            {
                                data = data.OrderBy(o => o.Status);
                            }
                            else
                            {
                                data = data.OrderByDescending(o => o.Status);
                            }

                            break;

                        case "PercentageCompleted":

                            if (sortAscending)
                            {
                                data = data.OrderBy(o => o.PercentageCompleted);
                            }
                            else
                            {
                                data = data.OrderByDescending(o => o.PercentageCompleted);
                            }

                            break;

                        default:
                            data = data.OrderBy(o => o.StudentName).OrderBy(o => o.StudentName);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(o => o.StudentName).OrderBy(o => o.StudentName);
                }
            }
            else
            {
                data = query.Select(x => new ReturnBriefCourseEnrollmentModel
                {
                    CourseEventId = x.CourseEventId,
                    StudentName = String.IsNullOrEmpty(x.Learner.User.Name) ? "" : x.Learner.User.Name,
                    DateEnrolled = x.EnrolledDate,
                    Status = x.Status
                });
            }

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        /// For use in view content, when user wants to ernoll
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseEnrollments/EnrollAsync")]
        [HttpGet]
        public async Task<IHttpActionResult> EnrollAsync(int id, int userId, string enrollmentCode = "")
        {
            // enroll as public
            if (String.IsNullOrEmpty(enrollmentCode))
            {
                var courseEvent = await db.CourseEvents.FirstOrDefaultAsync(x => x.CourseId == id && x.ViewCategory == ViewCategory.Public);

                var user = db.User.Find(userId);

                if (courseEvent != null && user != null)
                {
                    var enrollment = new Enrollment
                    {
                        LearnerId = userId,
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
                        EnrolmmentId = enrollment.Id,
                        LearnerId = userId,
                        Remark = "User Enrolled",
                        Status = EnrollmentStatus.Enrolled,
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

                    return Ok(false);
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
                        LearnerId = userId,
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
                        EnrolmmentId = enrollment.Id,
                        LearnerId = userId,
                        Remark = "User Enrolled",
                        Status = EnrollmentStatus.Enrolled,
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

                    return Ok(false);
                }
            }
            return Ok(true);
        }
    }
}