using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseEventApiUrl
    {
        public const string StartTrial = "eLearning/CourseEvents/StartTrial";
        public const string StopTrial = "eLearning/CourseEvents/StopTrial";
        public const string Get = "eLearning/CourseEvents/";
        public const string GetEventByCourseId = "eLearning/CourseEvents/GetEventByCourseId";

        public const string AddLearner = "eLearning/CourseEvents/AddLearner";
        public const string RemoveLearner = "eLearning/CourseEvents/RemoveLearner";

        public const string Publish = "eLearning/CourseEvents/Publish";
    }

    public class CourseEventsController : FEPController
    {
        private DbEntities db = new DbEntities();

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> StartTrial(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<ChangeCourseStatusModel>(HttpVerbs.Post, CourseEventApiUrl.StartTrial + $"?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Course Start Trial: " + response.Data.CourseName);

                TempData["SuccessMessage"] = "Course " + response.Data.CourseName + " now in Trial Mode. Please assign learners for the trial.";

            }
            else
            {
                await LogActivity(Modules.Learning, "Fail : Course Start Trial: " + response.Data);
                TempData["ErrorMessage"] = "Failed to Start Trial for this Course.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
        }


        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> StopTrial(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<ChangeCourseStatusModel>(HttpVerbs.Post, CourseEventApiUrl.StopTrial + $"?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Course Stop Trial: " + response.Data.CourseName);

                TempData["SuccessMessage"] = "Course " + response.Data.CourseName + " has stopped Trial.";

            }
            else
            {
                await LogActivity(Modules.Learning, "Fail : Course Stop Trial: " + response.Data.CourseName);
                TempData["ErrorMessage"] = "Failed to Stop Trial for this Course.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
        }


        // get all the learners for this course or courseevent
        [HttpGet]
        public async Task<ActionResult> Learners(int? id, int courseEventId = -1)
        {
            if (id == null)
            {
                return HttpNotFound();
            }


            var data = new CourseEventModel();

            if (courseEventId < 0)
            {
                data = await TryGetEventByCourseId(id.Value);

            }
            else
            {
                data = await TryGetCourseEvent(courseEventId);

            }


            if (data == null)
            {
                TempData["ErrorMessage"] = "Cannot get course/event info";

                return RedirectToAction("Content", "Courses", new { @area = "eLearning", @id = id.Value });
            }

            LearnerEnrollmentModel model = new LearnerEnrollmentModel
            {
                CourseId = id.Value,
                CourseEventId = data.Id
            };

            ViewBag.Role = RoleNames.eLearningLearner;

            return View(model);
        }

        [HttpGet]
        public ActionResult AddLearner()
        {
            return View("_addLearner");
        }

        [HttpPost]
        public async Task<ActionResult> AddLearner(int CourseId, int CourseEventId, int[] Ids)
        {
            var model = new UpdateLearnerEnrollmentModel
            {
                CourseEventId = CourseEventId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseEventApiUrl.AddLearner, model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully assigned as learner for this course.";
                await LogActivity(Modules.Learning, "Assign learner to this course.", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to assign learner to this course.";
            }


            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = CourseId });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveLearner(int CourseId, int CourseEventId, int[] Ids)
        {
            var model = new UpdateLearnerEnrollmentModel
            {
                CourseEventId = CourseEventId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseEventApiUrl.RemoveLearner, model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully removed from this course.";
                await LogActivity(Modules.Learning, "Remove learner from this course.", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to remove learner to this course.";
            }


            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = CourseId });
        }

        public async Task<CourseEventModel> TryGetCourseEvent(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CourseEventModel>(HttpVerbs.Get, CourseEventApiUrl.Get + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }
        public async Task<CourseEventModel> TryGetEventByCourseId(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CourseEventModel>(HttpVerbs.Get, CourseEventApiUrl.GetEventByCourseId + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }


        /// <summary>
        /// Published course. Course must be in approved state before can be published.
        /// When published, course can now be offerd to public or by inviting a group.
        /// </summary>
        /// <param name="id">Course Id</param>
        /// <returns></returns>
        //[HasAccess(UserAccess.CoursePublish)]
        //public async Task<ActionResult> Publish(int id, string title)
        //{
        //    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, CourseApiUrl.Publish + $"?id={id}");

        //    if (response.isSuccess)
        //    {
        //        if (response.Data == true)
        //        {
        //            await LogActivity(Modules.Learning, $"Course published. Course {id} - {title}");
        //            TempData["SuccessMessage"] = "Course is now published. You can start offering this course to public or invite students to enroll.";
        //        }
        //        else
        //        {
        //            await LogError(Modules.Learning, $"Error publishing Course {id} - {title}");
        //            TempData["ErrorMessage"] = "Error publishing course. Perhaps the course is not approved yet?";
        //        }
        //    }
        //    else
        //    {
        //        await LogError(Modules.Learning, $"API failed - Error publishing course Course {id} - {title}");
        //        TempData["ErrorMessage"] = "Could not published the course.";
        //    }
        //    return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
        //}


        /// <summary>
        /// Open the course to public. Check whether there are existing public course event and its status
        /// If so, decide to reuse or not
        /// Enrollment code is 
        /// </summary>
        /// <param name="id">CourseId</param>
        /// <returns></returns>
        public async Task<ActionResult> Publish(int id, string title, ViewCategory viewCategory)
        {
            var createdBy = CurrentUser.UserId;

            var response = await WepApiMethod.SendApiAsync<ChangeCourseStatusModel>(HttpVerbs.Post,
                CourseEventApiUrl.Publish + $"?id={id}&createdBy={createdBy}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, $"Success publishing - Course - {id}-{title}");

                if (viewCategory == ViewCategory.Public)
                {
                    TempData["SuccessMessage"] = "Published successful. The course is now available for the Public.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Published successful. You can now invite group/students to enroll to the course.";
                }
            }
            else
            {
                await LogError(Modules.Learning, $"Error publishing - Course - {id}-{title}");
                TempData["ErrorMessage"] = "Error publishing the course.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = id });

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