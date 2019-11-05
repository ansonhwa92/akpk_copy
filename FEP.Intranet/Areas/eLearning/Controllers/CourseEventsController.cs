using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
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

        public const string Create = "eLearning/CourseEvents/Create";
        public const string InviteLearners = "eLearning/CourseEvents/InviteLearners";
    }

    public class CourseEventsController : FEPController
    {
        private readonly DbEntities db = new DbEntities();

        // List of all sessions available for this course
        // From this list, can also see enrolmments
        [Authorize]
        public ActionResult Index(int courseId)
        {
            var model = new ReturnListCourseEventModel
            {
                CourseEvents = new ReturnBriefCourseEventModel(),
                Filters = new FilterCourseEventModel { CourseId = courseId },
            };

            return View(model);
        }


        [Authorize]
        [HasAccess(UserAccess.CoursePublish)]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find course to Create the session.";
                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            CourseEventModel model = new CourseEventModel();
            model.CourseId = id.Value;

            return View(model);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(CourseEventModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<TrxResult<CourseEvent>>(HttpVerbs.Post, CourseEventApiUrl.Create, model);

                if (response.isSuccess)
                {
                    if (response.Data.IsSuccess)
                    {
                        await LogActivity(Modules.Learning, $"A session -{model.Name} is Created for Course Id : {model.CourseId}");

                        TempData["SuccessMessage"] = $"Successfully created  {model.Name}. You can now invite students to the session.";

                        return RedirectToAction(nameof(InviteLearners), "CourseEvents", 
                            new { area = "eLearning", courseId = model.CourseId, eventId = response.Data.ObjectId,
                                    enrollmentCode = model.EnrollmentCode, title = model.Name});
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to create a session for this Course. - {response.Data}";
                }
            }
            else
            {
                await LogError(Modules.Learning, $"Fail create session for Course Id {model.Id} ");

                TempData["ErrorMessage"] = $"Failed to create a session for this Course.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = model.CourseId });
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> StartTrial(int? id)
        {
            var createdBy = CurrentUser.UserId.Value;
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find course to start Trial.";
                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var response = await WepApiMethod.SendApiAsync<ChangeCourseStatusModel>(HttpVerbs.Post, CourseEventApiUrl.StartTrial + $"?id={id}&createdBy={createdBy}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Course Start Trial: " + response.Data.CourseName);

                TempData["SuccessMessage"] = "Course " + response.Data.CourseName + " now in Trial Mode. Please assign learners for the trial.";
            }
            else
            {
                await LogError(Modules.Learning, "Fail : Course Start Trial: " + response.Data);
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
                TempData["SuccessMessage"] = "Users successfully assigned as learner for this course.";
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
        /// Open the course to public. Check whether there are existing public course event and its status
        /// If so, decide to reuse or not
        /// Enrollment code is
        /// </summary>
        /// <param name="id">CourseId</param>
        /// <returns></returns>
        ///
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


        public ActionResult InviteLearners(int courseId, int eventId, string enrollmentCode, string title)
        {
            var createdBy = CurrentUser.UserId;

            var model = new InviteLearnerModel
            {
                CourseId = courseId,
                CourseEventId = eventId,
                EnrollmentCode = enrollmentCode,
                CourseTitle = title
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> InviteLearners(InviteLearnerModel model)
        {

            var response = await WepApiMethod.SendApiAsync<InviteLearnerModel>(HttpVerbs.Post,
                CourseEventApiUrl.InviteLearners, model);

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, $"Success invite learner - Course - {model.CourseTitle}, Enrollment Code - {model.EnrollmentCode}");

                TempData["SuccessMessage"] = "An invitation will be sent to the listed emails.";

                // -- send email
                var notifyModel = new NotificationModel
                {
                    Id = model.CourseEventId,
                    Type = typeof(CourseEvent),
                    NotificationType = NotificationType.Course_Invitation,
                    NotificationCategory = NotificationCategory.Learning,
                    StartNotificationDate = DateTime.Now,
                    ParameterListToSend = new ParameterListToSend
                    {
                        CourseAuthor = model.CreatedBy,
                        CourseTitle = model.CourseTitle,
                        CourseApproval = "Course Verification",
                        Link = this.Url.AbsoluteAction("View", "Courses", new { id = model.CourseId, enrollmentCode = model.EnrollmentCode })
                    },
                    Emails = model.LearnerEmails,
                    IsNeedRemainder = false,
                    ReceiverType = ReceiverType.Emails,
                };

                var emailResponse = await EmaiHelper.SendNotification(notifyModel);

                if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                    !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
                {
                    await LogError(Modules.Learning, $"Error Sending Email For Course Invitation. Course Title (Id) : {model.CourseTitle} - {model.CourseId}");
                }
            }
            else
            {
                await LogError(Modules.Learning, $"Error inviting learners - Course - {model.CourseId}");
                TempData["ErrorMessage"] = "Error inviting learners to the course.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = model.CourseId });
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