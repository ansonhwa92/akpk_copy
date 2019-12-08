using AutoMapper;
using FEP.Helper;
using FEP.Intranet.Areas.Administrator.Controllers;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
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
        public const string CancelCourse = "eLearning/Courses/CancelCourse";
        public const string Start = "eLearning/Courses/Start";
        public const string Continue = "eLearning/Courses/Continue";

        //public const string Publish = "eLearning/Courses/Publish";
        public const string IsUserEnrolled = "eLearning/Courses/IsUserEnrolled";

        public const string IsUserCompleted = "eLearning/Courses/IsUserCompleted";

        // firus
        public const string GetAdditionalInput = "eLearning/Courses/GetAdditionalInput";
        public const string SaveAdditionalInput = "eLearning/Courses/SaveAdditionalInput";
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
            if (CurrentUser.HasAccess(UserAccess.CourseNonLearnerView))
                return View();
            else
                return RedirectToAction(nameof(HomeController.BrowseElearnings), "Home", new { area = "eLearning" });
        }

        [AllowAnonymous]
        public ActionResult SelectCategory()
        {
            // need to change to call api
            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name");

            ViewBag.Categories = new List<RefCourseCategory>(db.RefCourseCategories);

            var cat = db.RefCourseCategories.ToList();

            cat.Count();

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

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> Create(int? catid)
        {
            await GetCategories(catid.Value);

            var model = new CreateOrEditCourseModel();
            model.CategoryId = catid.Value;

            return View(model);
        }

        private async Task GetCategories(int catId = 0)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<CourseCategoryModel>>(HttpVerbs.Get, CourseApiUrl.GetCategory);

            if (response.isSuccess)
            {
                if (catId > 0)
                {
                    ViewBag.Categories = new SelectList(response.Data, "Id", "Name", catId);
                }
                else
                {
                    ViewBag.Categories = new SelectList(response.Data, "Id", "Name");
                }
            }
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

                //check coursecode
                var course = db.Courses.FirstOrDefault(x => x.Code.Equals(model.Code, StringComparison.OrdinalIgnoreCase));

                if (course != null) // change == to != by wawar
                {
                    TempData["ErrorMessage"] = $"There is already a course with the Course Code {course.Code}. Please select a new code.";

                    await GetCategories();

                    return View(model);
                }

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
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Course.";

                    return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                }
            }

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
        public async Task<ActionResult> _Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

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

            // allow to see if not published for creator, verifier, etc
            // this should be a filter but....
            if (model.Status != CourseStatus.Published)
            {
                if (CurrentUser.HasAccess(UserAccess.CourseCreate) ||
                    CurrentUser.HasAccess(UserAccess.CourseVerify) ||
                    CurrentUser.HasAccess(UserAccess.CourseApproval1) ||
                    CurrentUser.HasAccess(UserAccess.CourseApproval2) ||
                    CurrentUser.HasAccess(UserAccess.CourseApproval3))
                {
                    model.Modules = model.Modules.OrderBy(x => x.Order).ToList();

                    return View(model);
                }
            }
            else
            {
                if (CurrentUser.HasAccess(UserAccess.CourseEnroll) ||
                    CurrentUser.HasAccess(UserAccess.CourseDiscussionCreate) ||
                    CurrentUser.HasAccess(UserAccess.CourseDiscussionGroupCreate))
                {
                    model.Modules = model.Modules.OrderBy(x => x.Order).ToList();
                }
            }

            return View(model);
        }

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

            // Get enrollment Status
            if (model.Status == CourseStatus.Published)
            {
                if (CurrentUser.UserId != null)
                {
                    var currentUserId = CurrentUser.UserId.Value;

                    if (String.IsNullOrEmpty(enrollmentCode))
                    {
                        var response = await WepApiMethod.SendApiAsync<UserCourseEnrollmentModel>(HttpVerbs.Get, CourseEnrollmentApiUrl.GetEnrollment +
                            $"?id={id}&userId={currentUserId}");

                        if (response.isSuccess)
                        {
                            if (response.Data.Status == EnrollmentStatus.Enrolled || response.Data.Status == EnrollmentStatus.Completed)
                                model.IsUserEnrolled = true;

                            //to continue course
                            if (response.Data.CourseProgressCount != 0)
                                ViewBag.InProgress = true;
                            else
                                ViewBag.InProgress = false;

                            ViewBag.EnrollmentStatus = response.Data.Status;
                            ViewBag.EnrollmentId = response.Data.Id;

                            //to continue course
                            ViewBag.ProgressCourseContentId = response.Data.ProgressCourseContentId;
                        }
                    }
                    else
                    {
                        var response = await WepApiMethod.SendApiAsync<UserCourseEnrollmentModel>(HttpVerbs.Get, CourseApiUrl.IsUserCompleted + $"?id={id}&userId={currentUserId}&enrollmentCode={enrollmentCode}");

                        if (response.isSuccess)
                        {
                            if (response.Data.Status == EnrollmentStatus.Enrolled || response.Data.Status == EnrollmentStatus.Completed)
                                model.IsUserEnrolled = true;

                            //to continue course
                            if (response.Data.CourseProgressCount != 0)
                                ViewBag.InProgress = true;
                            else
                                ViewBag.InProgress = false;

                            ViewBag.EnrollmentStatus = response.Data.Status;
                            ViewBag.EnrollmentId = response.Data.Id;

                            //to continue course
                            ViewBag.ProgressCourseContentId = response.Data.ProgressCourseContentId;
                        }
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

        // firus
        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> AdditionalInput(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = await TryGetCourse(id.Value);

            if (course == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            var model = await GetAdditionalInput(id.Value, course.Title);
            //ViewBag.Id = 0;
            //ViewBag.InputId = model.Id;
            //ViewBag.UserId = CurrentUser.UserId;
            //ViewBag.InputContents = model.Contents;

            return View(model);
        }

        // firus
        [HasAccess(UserAccess.CourseEdit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> AdditionalInput(CourseAdditionalInputModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, CourseApiUrl.SaveAdditionalInput, model);

                if (response.isSuccess)
                {
                    return "success";
                }
                else
                {
                    return "error";
                }
            }
            return "invalid";
        }

        // firus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> SubmitResponse(CourseAdditionalInputResponseModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eLearning/Courses/SubmitResponse", model);

                if (response.isSuccess)
                {
                    return "success";
                }
                else
                {
                    return "error";
                }
            }
            return "invalid";
        }

        // firus
        private async Task<CourseAdditionalInputModel> GetAdditionalInput(int id, string coursetitle)
        {
            var responseInput = await WepApiMethod.SendApiAsync<CourseAdditionalInputModel>(HttpVerbs.Get, CourseApiUrl.GetAdditionalInput + $"?id={id}&coursetitle={coursetitle}");

            if (responseInput.isSuccess)
                return responseInput.Data;
            else
            {
                var newinput = new CourseAdditionalInputModel
                {
                    CourseId = id,
                    Contents = "",
                    PageTitle = coursetitle,
                };

                return newinput;
            }
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

                //check coursecode
                var course = db.Courses.FirstOrDefault(x => x.Code.Equals(model.Code, StringComparison.OrdinalIgnoreCase));

                if (course != null && course.Id != model.Id)
                {
                    TempData["ErrorMessage"] = $"There is already a course with the Course Code {course.Code}. Please select a new code.";

                    await GetCategories();

                    return View(model);
                }

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

        [HttpPost]
        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> CancelCourse(int? id, string title = "")
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find such course.";

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseApiUrl.CancelCourse + $"?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, $"Cancel Course titled {title} Success: " + response.Data, CurrentUser.UserId.Value);

                TempData["SuccessMessage"] = $"Creation of course titled {title} is successfully cancelled.";

                await Notifier.NotifyCourseCancelled(NotificationType.Course_Cancelled,
                    id.Value, CurrentUser.UserId.Value, "", "",
                    title, Url.AbsoluteAction("Index", "Courses", new { area = "eLearning" }));
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to Cancel Course {title}.";
            }

            return RedirectToAction("Index", "Courses", new { area = "eLearning" });
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
        public async Task<ActionResult> SaveCertificate(ReviewCertificateModel model)
        {
            if (ModelState.IsValid)
            {
                //model.CreatedBy = CurrentUser.UserId.Value;
                //model.CreatedByName = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Courses/SaveCertificate", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Certificate successfully assigned.";
                    await LogActivity(Modules.Learning, "Assign certificate to course", model);

                    return RedirectToAction("AssignCertificate", "Courses", new { area = "eLearning", @id = model.CourseId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Fail to assign certificate.";
                }
            }

            return View(model);
        }

        [HttpPost]
        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> ReviewCertificate(CertificatesModel model)
        {
            var response = await WepApiMethod.SendApiAsync<ReviewCertificateModel>(HttpVerbs.Post, $"eLearning/Courses/ReviewCertificate", model);

            if (response.isSuccess)
            {
                return View(response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to assign certificate.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ViewCertificate(int enrollID)
        {
            var response = await WepApiMethod.SendApiAsync<ViewCertificateModel>(HttpVerbs.Get, $"eLearning/Courses/ViewCertificate?id={enrollID}");

            if (response.isSuccess)
            {
                var certView = new ViewCertificateModel();

                certView = response.Data;

                return View(certView);
            }
            return View();
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
                TempData["ErrorMessage"] = "Could not load next content.";

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

        //wawa - for my courses list
        [HasAccess(UserAccess.CourseView)]
        [HttpGet]
        public async Task<ActionResult> MyCourses(string keyword, string sorting, bool? cashflow, bool? car, bool? house, bool? investment, bool? protection, /*bool? beginner, bool? intermediate, bool? advanced,*/ bool? english, bool? malay, bool? chinese, bool? tamil, bool? multiLanguage)
        {
            if (keyword == null) keyword = "";
            if (sorting == null) sorting = "default";
            if (cashflow == null) cashflow = true;
            if (car == null) car = true;
            if (house == null) house = true;
            if (investment == null) investment = true;
            if (protection == null) protection = true;
            //if (beginner == null) beginner = true;
            //if (intermediate == null) intermediate = true;
            //if (advanced == null) advanced = false;
            if (english == null) english = false;
            if (malay == null) malay = false;
            if (chinese == null) chinese = false;
            if (tamil == null) tamil = false;
            if (multiLanguage == null) multiLanguage = false;

            var currentUserId = CurrentUser.UserId.Value;

            var response = await WepApiMethod.SendApiAsync<ReturnMyCoursesModel>(HttpVerbs.Get, $"eLearning/Courses/GetMyCoursesList?id={currentUserId}&keyword={keyword}&sorting={sorting}&cashflow={cashflow}&car={car}&house={house}&investment={investment}&protection={protection}&english={english}&malay={malay}&chinese={chinese}&tamil={tamil}&multiLanguage={multiLanguage}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
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