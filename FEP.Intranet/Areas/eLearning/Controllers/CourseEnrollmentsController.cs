using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseEnrollmentApiUrl
    {
        public const string EnrollAsync = "eLearning/CourseEnrollments/EnrollAsync";
        public const string UserDetails = "eLearning/CourseEnrollments/GetUserDetails";

    }

    public class CourseEnrollmentsController : FEPController
    {
        private readonly DbEntities db = new DbEntities();
        private readonly IMapper _mapper;

        public CourseEnrollmentsController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserCourseEnrollmentModel, Enrollment>();

            });

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// View the course Event and list of enrolled users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(int courseId, int courseEventId)
        {
            var model = new ReturnListCourseEnrollmentModel
            {
                CourseEnrollment = new ReturnBriefCourseEnrollmentModel(),
                Filters = new FilterCourseEnrollmentModel
                {
                    CourseId = courseId,
                    CourseEventId = courseEventId
                },
            };

            return View(model);
        }

        [Authorize]
        public ActionResult UsersProgress(int? id)
        {
            var model = new ReturnListCourseEnrollmentModel
            {
                CourseEnrollment = new ReturnBriefCourseEnrollmentModel(),
                Filters = new FilterCourseEnrollmentModel
                {
                    CourseId = id.Value,
                },
            };

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> UserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await TryGetCourseUserDetails(id.Value);

            if (user == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            var model = _mapper.Map<UserCourseEnrollmentModel>(user);

            return View(model);
        }

        //[HasAccess(UserAccess.CourseCreate)]
        public static async Task<UserCourseEnrollmentModel> TryGetCourseUserDetails(int id)
        {
            var response = await WepApiMethod.SendApiAsync<UserCourseEnrollmentModel>(HttpVerbs.Get, CourseEnrollmentApiUrl.UserDetails + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        [Authorize]
        public async Task<ActionResult> EnrollAsync(int courseId, string enrollmentCode = "")
        {
            var currentUserId = CurrentUser.UserId.Value;

            WebApiResponse<bool> response = null;

            if (String.IsNullOrEmpty(enrollmentCode))
            {
                response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                    CourseApiUrl.IsUserEnrolled + $"?id={courseId}&userId={currentUserId}");
            }
            else
            {
                response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                    CourseApiUrl.IsUserEnrolled + $"?id={courseId}&userId={currentUserId}&enrollmentCode={enrollmentCode}");
            }

            if (response.isSuccess)
            {
                if (response.Data)
                {
                    TempData["ErrorMessage"] = "You are already enrolled to this course.";

                    return RedirectToAction("View", "Courses", new { area = "eLearning", id = courseId, enrollmentCode = enrollmentCode });
                }
            }

            var enrollResponse = new WebApiResponse<TrxResult<Enrollment>>();

            if (String.IsNullOrEmpty(enrollmentCode))
            {
                enrollResponse = await WepApiMethod.SendApiAsync<TrxResult<Enrollment>>(HttpVerbs.Get,
                CourseEnrollmentApiUrl.EnrollAsync + $"?id={courseId}&userId={currentUserId}");
            }
            else
            {
                enrollResponse = await WepApiMethod.SendApiAsync<TrxResult<Enrollment>>(HttpVerbs.Get,
                CourseEnrollmentApiUrl.EnrollAsync + $"?id={courseId}&userId={currentUserId}&enrollmentCode={enrollmentCode}");
            }

            if (enrollResponse.isSuccess)
            {
                var result = enrollResponse.Data;

                if (result.IsSuccess)
                {
                    await LogActivity(Modules.Learning, Gamification.UserEnrolled.ToString(), $"User {currentUserId} enrolled to the course {courseId} with" +
                        $" enrollment code - {enrollmentCode} ");

                    TempData["SuccessMessage"] = "You are now enrolled to this course.";

                    return RedirectToAction("View", "Courses", new { area = "eLearning", id = courseId, enrollmentCode = enrollmentCode });
                }
            }

            await LogError(Modules.Learning, "User Enrolled Failed ", $"User {currentUserId} failed to enroll to the course {courseId} with" +
                    $" enrollment code - {enrollmentCode}. Error - {enrollResponse.Data.Message}");

            TempData["ErrorMessage"] = "Error enrolling to the course." + enrollResponse.Data.Message;

            return RedirectToAction("View", "Courses", new { area = "eLearning", id = courseId, enrollmentCode = enrollmentCode });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}