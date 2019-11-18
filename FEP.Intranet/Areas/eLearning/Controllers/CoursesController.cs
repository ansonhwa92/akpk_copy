using AutoMapper;
using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class CourseApiUrl
    {
        public const string GetCategory = "eLearning/CourseCategory/";
        public const string CreateCourse = "eLearning/Courses/Create";
        public const string GetCourse = "eLearning/Courses";
        public const string EditCourse = "eLearning/Courses/Edit";
        public const string GetFrontContent = "eLearning/Courses/GetFrontContent";
        public const string GetFrontCourse = "eLearning/Courses/GetFrontCourse";
        public const string GetTrainerCourse = "eLearning/Courses/GetTrainerCourse";
        public const string EditRulesCourse = "eLearning/Courses/EditRules";
        public const string Content = "eLearning/Courses/Content";
        public const string DeleteCourse = "eLearning/Courses/Delete";
        public const string Start = "eLearning/Courses/Start";
        public const string Publish = "eLearning/Courses/Publish";
        public const string IsUserEnrolled = "eLearning/Courses/IsUserEnrolled";
    }

    public class CoursesController : FEPController
    {
        private DbEntities db = new DbEntities();
        private readonly IMapper _mapper;

        public CoursesController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditCourseModel, Course>();

                cfg.CreateMap<Course, CreateOrEditCourseModel>();

                cfg.CreateMap<CreateOrEditCourseModel, CourseRuleModel>();
            });

            _mapper = config.CreateMapper();
        }

        // GET: eLearning/Courses
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //[HasAccess(UserAccess.RnPPublicationEdit)]
        public ActionResult SelectCategory()
        {
            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name");
            ViewBag.Categories = new List<RefCourseCategory>(db.RefCourseCategories);
            return View();
        }


        // GET: eLearning/Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //[HasAccess(UserAccess.CourseCreate)]
        //// GET: eLearning/Courses/Create
        //public async Task<ActionResult> Create()
        //{
        //    CreateOrEditCourseModel model = new CreateOrEditCourseModel();

        //    await GetCategories();

        //    return View(model);
        //}

        [HasAccess(UserAccess.CourseCreate)]
        public ActionResult Create(int? catid)
        {
            if (catid != null)
            {
                ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name", catid);
            }
            else
            {
                ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name");
            }
            var model = new CreateOrEditCourseModel();
            return View(model);
        }

        private async Task GetCategories()
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<CourseCategoryModel>>(HttpVerbs.Get, CourseApiUrl.GetCategory);

            if (response.isSuccess)
                ViewBag.Categories = new SelectList(response.Data, "Id", "Name");
            else
            {
                ViewBag.Categories = new SelectList(new List<CourseCategoryModel>
                {
                    new CourseCategoryModel{ Id = 999, Name = "Error"}
                }, "Id", "Name");

                TempData["Error"] = "Cannot find any categories to display.";
            }
        }

        // POST: eLearning/Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrEditCourseModel model, string Submittype)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;
                model.CreatedByName = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseApiUrl.CreateCourse, model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string newid = resparray[0];
                    //string title = resparray[1];

                    //TempData["SuccessMessage"] = "Course successfully created. Now you can add contents.";

                    await LogActivity(Modules.Learning, "Create Course : " + model.Title);

                    var id = response.Data;

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "New Course titled " + model.Title + " created successfully and saved as draft.";

                        return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                    }
                    else
                    {
                        return RedirectToAction("Review", "Courses", new { area = "eLearning", @id = newid });
                    }

                    //if (!String.IsNullOrEmpty(id))

                    //    return RedirectToAction("Content", "Courses", new { id = id });
                    //else
                    //    return RedirectToAction("Index", "Courses");
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Course.";

                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }
            }

            //TempData["ErrorMessage"] = "Cannot add course. Please ensure all required fields are filled in correctly.";

            //await GetCategories();
            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name", model.CategoryId);


            return View(model);
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> Review(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var resPub = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, $"eLearning/Courses/GetForReview?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var course = resPub.Data;

            if (course == null)
            {
                return HttpNotFound();
            }

            var model = _mapper.Map<CreateOrEditCourseModel>(course);

            //model.Description = HttpUtility.HtmlDecode(model.Description);
            //model.Objectives = HttpUtility.HtmlDecode(model.Objectives);

            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name", course.CategoryId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(CreateOrEditCourseModel model)
        {
            if (model != null)
            {
                return RedirectToAction("Content", "Courses", new { id = model.Id });
            }
            else
            {
                return RedirectToAction("Index", "Courses");
            }
        }

        [HttpGet]
        public ActionResult Trainers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = db.Courses.FirstOrDefault(x => x.Id == id.Value);

            var model = new TrainerCourseModel
            {
                CourseId = id.Value,
                Course = entity
            };

            ViewBag.Trainer = RoleNames.eLearningTrainer;

            return View(model);
        }

        [HttpGet]
        public ActionResult _Add()
        {
            return View();
        }

        /// <summary>
        /// Misleading function Name. This is actually for adding trainer to the course.
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddUser(int CourseId, int[] Ids)
        {
            var model = new UpdateTrainerCourseModel
            {
                CourseId = CourseId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eLearning/Courses/AddUser", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully assigned to role trainer/instructor.";
                await LogActivity(Modules.Learning, "Assign user to trainer", model);

                // Notification
                var notifyModel = new NotificationModel
                {
                    Id = CourseId,
                    Type = typeof(Course),
                    NotificationType = NotificationType.Course_Assigned_To_Facilitator,
                    NotificationCategory = NotificationCategory.Learning,
                    StartNotificationDate = DateTime.Now,
                    ParameterListToSend = new ParameterListToSend
                    {
                        Link = this.Url.AbsoluteAction("View", "Courses", new { id = CourseId }),
                    },
                    ReceiverType = ReceiverType.UserIds,
                    Receivers = Ids.ToList(),
                    IsNeedRemainder = false,
                };

                var emailResponse = await EmaiHelper.SendNotification(notifyModel);

                if (emailResponse == null || String.IsNullOrEmpty(emailResponse.Status) ||
                    !emailResponse.Status.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
                {
                    await LogError(Modules.Learning, $"Error Sending Email For Facilitator When Assigned to A Course. Course Id : {CourseId}");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to assign role.";
            }

            return RedirectToAction("Trainers", "Courses", new { area = "eLearning", id = CourseId });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int CourseId, int[] Ids)
        {
            var model = new UpdateTrainerCourseModel
            {
                CourseId = CourseId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eLearning/Courses/DeleteUser", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully remove from role.";
                await LogActivity(Modules.Setting, "Remove User From Role", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to remove user.";
            }

            return RedirectToAction("Trainers", "Courses", new { area = "eLearning", id = CourseId });
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> Content(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await TryGetFrontCourse(id.Value);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            model.Modules = model.Modules.OrderBy(x => x.Order).ToList();

            return View(model);
        }

        //[HasAccess(UserAccess.CourseView)]
        [AllowAnonymous]
        public async Task<ActionResult> View(int? id, string enrollmentCode = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await TryGetFrontCourse(id.Value);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            // check if user enrolled
            if (CurrentUser.UserId != null)
            {
                var currentUserId = CurrentUser.UserId.Value;

                if (String.IsNullOrEmpty(enrollmentCode))
                {
                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                        CourseApiUrl.IsUserEnrolled + $"?id={id}&userId={currentUserId}");
                    if (response.isSuccess)
                    {
                        model.IsUserEnrolled = response.Data;
                    }
                }
                else
                {
                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get,
                        CourseApiUrl.IsUserEnrolled + $"?id={id}&userId={currentUserId}&enrollmentCode={enrollmentCode}");

                    if (response.isSuccess)
                    {
                        model.IsUserEnrolled = response.Data;
                    }
                }
            }

            model.Modules = model.Modules.OrderBy(x => x.Order).ToList();
            model.EnrollmentCode = enrollmentCode;

            return View(model);
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> EditRules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get Model
            var course = await TryGetCourse(id.Value);

            if (course == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }
            var model = _mapper.Map<CourseRuleModel>(course);

            ViewBag.CoursesList = new SelectList(await GetCoursesList(id), "Id", "Name");

            return View(model);
        }

        [HasAccess(UserAccess.CourseEdit)]
        [HttpPost]
        public async Task<ActionResult> EditRules(CourseRuleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseApiUrl.EditRulesCourse, model);

                if (response.isSuccess)
                {
                    ViewBag.CoursesList = new SelectList(await GetCoursesList(model.Id), "Id", "Name");

                    TempData["SuccessMessage"] = "Course Rules successfully updated.";

                    await LogActivity(Modules.Learning, "Update Course Rule : " + model.Title);

                    var id = response.Data;

                    if (!String.IsNullOrEmpty(id))

                        return RedirectToAction("Content", "Courses", new { id = id });
                    else
                        return RedirectToAction("Index", "Courses");
                }
            }

            TempData["ErrorMessage"] = "Cannot update course's rule.";

            return View(model);
        }

        public static async Task<CreateOrEditCourseModel> TryGetCourse(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, CourseApiUrl.GetCourse + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        public static async Task<CreateOrEditCourseModel> TryGetFrontCourse(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, CourseApiUrl.GetFrontCourse + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        public async Task<TrainerCourseModel> TryGetTrainerCourse(int id)
        {
            var response = await WepApiMethod.SendApiAsync<TrainerCourseModel>(HttpVerbs.Get, CourseApiUrl.GetTrainerCourse + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        // GET: eLearning/Courses/Edit/5
        [HasAccess(UserAccess.CourseEdit)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find such course.";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var course = await TryGetCourse(id.Value);

            if (course == null)
            {
                TempData["ErrorMessage"] = "Cannot find such course.";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            CreateOrEditCourseModel model = _mapper.Map<CreateOrEditCourseModel>(course);

            await GetCategories();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateOrEditCourseModel model, string Submittype)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = CurrentUser.UserId.Value;
                model.UpdatedByName = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Post, CourseApiUrl.EditCourse, model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, $"Edit Course Successfull - {response.Data.Title} - {model.Id}");

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Course titled " + response.Data.Title + " updated successfully.";

                        return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = model.Id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to edit course.";

                    return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = model.Id });
                }
            }

            await GetCategories();

            return View(model);
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find such course.";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var response = await TryGetCourse(id.Value);

            if (response == null)
            {
                TempData["ErrorMessage"] = "Cannot find such course.";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var vm = response;

            return View(vm);
        }

        // POST: eLearning/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"eLearning/Courses/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Course: " + response.Data);

                TempData["SuccessMessage"] = "Course titled " + response.Data + " successfully deleted.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Course.";
            }

            return RedirectToAction("Index", "Courses", new { area = "eLearning" });
        }

        [HasAccess(UserAccess.CourseEdit)]
        public async Task<ActionResult> AssignCertificate(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CertificatesModel>(HttpVerbs.Get, $"eLearning/Certificates/GetCertificate?id={id}");

            if (response.isSuccess)
            {
                response.Data.courseId = id;
                return View(response.Data);
            }

            return View(new CertificatesModel());
        }

        [HttpPost]
        public async Task<ActionResult> SaveCertificate(CertificatesModel model)
        {
            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Courses/SaveCertificate", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "Certificate successfully assigned.";
                await LogActivity(Modules.Learning, "Assign certificate to course", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to assign certificate.";
            }

            return RedirectToAction("Content", "Courses", new { area = "eLearning", id = model.courseId });
        }

        // Start the course
        [HasAccess(UserAccess.CourseView)]
        public async Task<ActionResult> Start(int id)
        {
            var currentUserId = CurrentUser.UserId.Value;

            var response = await WepApiMethod.SendApiAsync<CourseContent>(HttpVerbs.Get, CourseApiUrl.Start + $"?id={id}&userId={currentUserId}");

            if (response.isSuccess)
            {
                return RedirectToAction("View", "CourseModules", new { area = "eLearning", @id = response.Data.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Could not start the course. Are you enrolled?";

                return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
            }
        }

        [NonAction]
        private async Task<IEnumerable<CourseListModel>> GetCoursesList(int? id)
        {
            var courses = Enumerable.Empty<CourseListModel>();

            var response = await WepApiMethod.SendApiAsync<IEnumerable<CourseListModel>>(HttpVerbs.Get, $"eLearning/Courses/GetCoursesList?id={id}");

            if (response.isSuccess)
            {
                courses = response.Data.OrderBy(o => o.Name);
            }

            return courses;
        }

        public ActionResult Users(int courseId)
        {
            var model = new ReturnListCourseEnrollmentModel
            {
                CourseEnrollment = new ReturnBriefCourseEnrollmentModel(),
                Filters = new FilterCourseEnrollmentModel
                {
                    CourseId = courseId
                },
            };

            return View(model);
        }

    }
}