﻿using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eLearning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        // GET: eLearning/Courses/Create
        public async Task<ActionResult> Create()
        {
            CreateOrEditCourseModel model = new CreateOrEditCourseModel();

            await GetCategories();

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
        public async Task<ActionResult> Create(CreateOrEditCourseModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;
                model.CreatedByName = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseApiUrl.CreateCourse, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Course successfully created. Now you can add contents.";

                    await LogActivity(Modules.Learning, "Create Course : " + model.Title);

                    var id = response.Data;

                    if (!String.IsNullOrEmpty(id))

                        return RedirectToAction("Content", "Courses", new { id = id });
                    else
                        return RedirectToAction("Index", "Courses");
                }   
            }

            TempData["ErrorMessage"] = "Cannot add course. Please ensure all required fields are filled in.";

            await GetCategories();

            return View(model);
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
                await LogActivity(Modules.Admin, "Remove User From Role", model);
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


        //// post the new order
        //[HttpPost]
        //public async Task<ActionResult> Content(int? Id, int CreatedBy, string order)
        //{
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    if (String.IsNullOrEmpty(order))
        //    {
        //        RedirectToAction("Content", new { id = Id.Value });
        //    }

        //    var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Post,
        //        CourseApiUrl.Content + $"?Id={Id}&CreatedBy={CreatedBy}&order={order}");

        //    if (response.isSuccess)
        //    {
        //        TempData["SuccessMessage"] = "Changes saved";

        //        var model = response.Data;

        //        await LogActivity(Modules.Learning, "Update Course Content : " + model.Title);

        //        return View(model);
        //    }

        //    TempData["ErrorMessage"] = "Error in saving order..";

        //    // get Model
        //    var vm = await TryGetFrontCourse(Id.Value);

        //    if (vm == null)
        //    {
        //        TempData["ErrorMessage"] = "No such course.";

        //        return RedirectToAction("Index", "Courses");
        //    }

        //    vm.Modules = vm.Modules.OrderBy(x => x.Order).ToList();

        //    return View(vm);
        //}



        public async Task<ActionResult> View(int? id)
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

            //var vm = new CourseRuleModel
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    CompletionCriteriaType = model.CompletionCriteriaType,
            //    ModulesCompleted = model.ModulesCompleted,
            //    LearningPath = model.LearningPath,
            //    PercentageCompletion = model.PercentageCompletion,
            //    ScoreCalculation = model.ScoreCalculation,
            //    TestsPassed = model.TestsPassed,
            //    TraversalRule = model.TraversalRule
            //};

            return View(model);
        }

        [HasAccess(UserAccess.CourseCreate)]
        [HttpPost]
        public async Task<ActionResult> EditRules(CourseRuleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, CourseApiUrl.EditRulesCourse, model);

                if (response.isSuccess)
                {
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

        public async Task<CreateOrEditCourseModel> TryGetCourse(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, CourseApiUrl.GetCourse + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        public async Task<CreateOrEditCourseModel> TryGetFrontCourse(int id)
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
        [HasAccess(UserAccess.CourseCreate)]
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
                    await LogActivity(Modules.Learning, "Edit Course: " + response.Data.Title, model);

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

            return View(model);

        }


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
        public async Task<ActionResult> Start(int id)
        {

            //var module = await db.CourseModules.Where(x => x.CourseId == id).OrderBy(x => x.Order).FirstOrDefaultAsync();

            var response = await WepApiMethod.SendApiAsync<CourseContent>(HttpVerbs.Get, CourseApiUrl.Start + $"?id={id}");

            if (response.isSuccess)
            {
                return RedirectToAction("View", "CourseModules", new { area = "eLearning", @id = response.Data.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Could not start the course.";

                return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = id });
            }

        }

    }
}