using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseEventApiUrl
    {
        public const string StartTrial = "eLearning/CourseEvents/StartTrial";
        public const string StopTrial = "eLearning/CourseEvents/StopTrial";
        public const string Get = "eLearning/CourseEvents";
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

            var course = db.Courses.FirstOrDefault(x => x.Id == courseId);

            if (course != null)
            {
                ViewBag.CourseTitle = course.Title;
            }

            return View(model);
        }

        // List of all sessions available for this course
        // From this list, can also see enrolmments
        [Authorize]
        public ActionResult Invitation(int courseEventId, string eventName, string courseTitle)
        {
            if (!CurrentUser.HasAccess(UserAccess.CourseDiscussionCreate) && !CurrentUser.HasAccess(UserAccess.CourseDiscussionGroupCreate))
            {
                TempData["Error"] = "Unauthorized Access";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var model = new ReturnListCourseInvitationModel
            {
                CourseInvitations = new ReturnBriefCourseInvitationModel(),
                Filters = new FilterCourseInvitationModel { CourseEventId = courseEventId },
            };

            ViewBag.CourseTitle = courseTitle;
            ViewBag.CourseEventName = eventName;
            ViewBag.CourseEventId = courseEventId;

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
                        await LogActivity(Modules.Learning, $"A session titled {model.Name} is Created for Course Id : {model.CourseId}");

                        TempData["SuccessMessage"] = $"A session titled {model.Name} is created. You can now invite students to the session.";

                        return RedirectToAction(nameof(InviteLearners), "CourseEvents",
                            new
                            {
                                area = "eLearning",
                                courseId = model.CourseId,
                                eventId = response.Data.ObjectId,
                                enrollmentCode = model.EnrollmentCode,
                                title = model.Name
                            });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to create a session for this Course. - {response.Data}";
                }
            }
            else
            {
                await LogError(Modules.Learning, $"Fail create session for Course Id {model.Id}");

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
        public ActionResult _AddIndividual()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> _AddCompany()
        {
            var filter = new FilterCompanyModel();

            filter.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

            return View(new ListCompanyModel { Filter = filter });
        }

        [HttpGet]
        public async Task<ActionResult> _AddStaff()
        {
            var filter = new FilterStaffModel();

            filter.Branchs = new SelectList(await GetBranches(), "Id", "Name", 0);
            filter.Departments = new SelectList(await GetDepartments(), "Id", "Name", 0);

            return View(new ListStaffModel { Filter = filter });
        }

        [NonAction]
        private async Task<IEnumerable<SectorModel>> GetSectors()
        {

            var sectors = Enumerable.Empty<SectorModel>();

            var response = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (response.isSuccess)
            {
                sectors = response.Data.OrderBy(o => o.Name);
            }

            return sectors;
        }

        [NonAction]
        private async Task<IEnumerable<BranchModel>> GetBranches()
        {

            var branches = Enumerable.Empty<BranchModel>();

            var response = await WepApiMethod.SendApiAsync<List<BranchModel>>(HttpVerbs.Get, $"Administration/Branch");

            if (response.isSuccess)
            {
                branches = response.Data.OrderBy(o => o.Name);
            }

            return branches;
        }

        [NonAction]
        private async Task<IEnumerable<DepartmentModel>> GetDepartments()
        {

            var departments = Enumerable.Empty<DepartmentModel>();

            var response = await WepApiMethod.SendApiAsync<List<DepartmentModel>>(HttpVerbs.Get, $"Administration/Department");

            if (response.isSuccess)
            {
                departments = response.Data.OrderBy(o => o.Name);
            }

            return departments;

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

                await Notifier.NotifyCoursePublish(NotificationType.Course_Publish,
                     id, CurrentUser.UserId.Value, "", createdBy.ToString(),
                     title, Url.AbsoluteAction("Content", "Course", new { id = id }));

                if (viewCategory == ViewCategory.Public)
                {
                    TempData["SuccessMessage"] = $"Course titled {title} published successfully and open for public.";
                }
                else
                {
                    TempData["SuccessMessage"] = $"Course titled {title} published successfully.You can now invite group / students to enroll to the course.";
                }
            }
            else
            {
                await LogError(Modules.Learning, $"Error publishing - Course - {id}-{title}");
                TempData["ErrorMessage"] = $"Error publishing the course {title}.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = id });
        }

        public ActionResult InviteLearners(int courseId, int eventId, string enrollmentCode = "", string title = "")
        {
            var createdBy = CurrentUser.UserId;

            var model = new InviteLearnerModel
            {
                CourseId = courseId,
                CourseEventId = eventId,
                EnrollmentCode = enrollmentCode,
                CourseTitle = title
            };

            var courseEvent = db.CourseEvents.FirstOrDefault(x => x.Id == eventId);

            if (courseEvent != null)
            {
                model.EnrollmentCode = courseEvent.EnrollmentCode;
                model.CourseTitle = courseEvent.Course.Title;
            }

            return View(model);
        }

        public async Task<ActionResult> InviteMoreLearners(int eventId, string title)
        {
            var createdBy = CurrentUser.UserId;

            var courseEvent = await TryGetCourseEvent(eventId);

            if (courseEvent != null)
            {
                var model = new InviteLearnerModel
                {
                    CourseId = courseEvent.CourseId,
                    CourseEventId = eventId,
                    EnrollmentCode = courseEvent.EnrollmentCode,
                    CourseTitle = title
                };

                return View("InviteLearners", model);
            }
            else
            {
                TempData["ErrorMessage"] = $"Cannot get the event.";

                return Redirect(Request.UrlReferrer.ToString());
            }
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
                await Notifier.SendEnrollmentInvitationByListOfEmails(NotificationType.Course_Invitation,
                    model.CourseEventId, CurrentUser.UserId.Value,
                    model.LearnerEmails, model.CreatedBy, model.CourseTitle, Url.AbsoluteAction("View", "Courses",
                    new { id = model.CourseId, enrollmentCode = model.EnrollmentCode }));
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