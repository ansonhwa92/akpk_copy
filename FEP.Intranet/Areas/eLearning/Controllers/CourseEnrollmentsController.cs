﻿using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseEnrollmentApiUrl
    {
        public const string EnrollAsync = "eLearning/CourseEnrollments/EnrollAsync";
    }

    public class CourseEnrollmentsController : FEPController
    {
        private readonly DbEntities db = new DbEntities();

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

            if (String.IsNullOrEmpty(enrollmentCode))
            {
                response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                CourseEnrollmentApiUrl.EnrollAsync + $"?id={courseId}&userId={currentUserId}");
            }
            else
            {
                response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                CourseEnrollmentApiUrl.EnrollAsync + $"?id={courseId}&userId={currentUserId}&enrollmentCode={enrollmentCode}");
            }

            if (response.isSuccess)
            {
                if (response.Data)
                {
                    TempData["SuccessMessage"] = "You are now enrolled to this course.";

                    return RedirectToAction("View", "Courses", new { area = "eLearning", id = courseId, enrollmentCode = enrollmentCode });
                }
            }

            TempData["ErrorMessage"] = "Error enrolling to the course.";

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