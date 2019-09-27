﻿using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
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
        public const string GetFrontContent = "eLearning/Courses/GetFrontContent";
        public const string GetFrontCourse = "eLearning/Courses/GetFrontCourse";
        public const string EditRulesCourse = "eLearning/Courses/EditRules";
        public const string DeleteCourse = "eLearning/Courses/Delete";

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
                    TempData["SuccessMessage"] = "Course successfully created. Now you can add contents and rules.";

                    await LogActivity(Modules.Learning, "Create Course : " + model.Title);

                    //return RedirectToAction("Manage", new { area = "eLearning", id = model.Id });

                    var id = response.Data;

                    if (!String.IsNullOrEmpty(id))

                        return RedirectToAction("Content", "Courses", new { id = id });
                    else
                        return RedirectToAction("Index", "Courses");
                }

                //// --- FOR TESTING ONLY ----
                //var course = _mapper.Map<Course>(model);
                //course.CreatedBy = CurrentUser.Email;
                //course.CreatedDate = DateTime.Now;

                //db.Courses.Add(course);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Manage", "Courses", new { id = model.Id });
            }

            TempData["ErrorMessage"] = "Cannot add course. Please ensure all required fields are filled in.";

            await GetCategories();

            return View(model);
        }

        public async Task<ActionResult> Content(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get Model
            var model = await TryGetFrontCourse(id.Value);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            //var frontContent = await TryGetFrontContent(id.Value);

            //if(frontContent != null)
            //{
            //    model.FrontPageContents = frontContent.ToList() ;
            //}

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

        public async Task<IEnumerable<CourseContent>> TryGetFrontContent(int id)
        {
            var response = await WepApiMethod.SendApiAsync<IEnumerable<CourseContent>>(HttpVerbs.Get, CourseApiUrl.GetFrontContent + $"?id={id}");

            if (response.isSuccess)
            {   
                return response.Data;
            }

            return null;
        }


        // GET: eLearning/Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
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


            CreateOrEditCourseModel model = _mapper.Map<CreateOrEditCourseModel>(course);


            await GetCategories();

            return View(model);            
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,Description,Code,Objectives,Medium,ScheduleType,Duration,DurationType,Language,CategoryId,IsFree,Price,Status,IntroVideoPath,TraversalRule,ScoreCalculation,VerifierApprovalId,FirstApprovalId,SecondApprovalId,ThirdApprovalId,TotalModules,CertificateId,DefaultAllowablePercentageBeforeWithdraw,ViewCategory,CompletionCriteriaType,ModulesCompleted,PercentageCompletion,TestsPassed,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Course course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(course).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(course);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateOrEditCourseModel model, string Submittype)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Courses/Edit", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Edit Course: " + response.Data, model);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Course titled " + response.Data + " updated successfully and saved as draft.";

                        return RedirectToAction("Index", "Courses", new { area = "eLearning" });
                    }
                    else
                    {
                       
                        return RedirectToAction("Review", "Courses", new { area = "eLearning", @id = model.Id });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to edit Course.";

                    return RedirectToAction("Details", "Courses", new { area = "eLearning", @id = model.Id });
                }
            }

            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);

        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var course = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, $"eLearning/Courses?id={id}");

            if (!course.isSuccess)
            {
                return HttpNotFound();
            }

            var vm = course.Data;

            if (vm == null)
            {
                return HttpNotFound();
            }

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
               
                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Course.";

                return RedirectToAction("Details", "Courses", new { area = "eLearning", @id = id });
            }

            //Course course = db.Courses.Find(id);

            //course.Display = false;

            //db.Courses.Attach(course);
            //db.Entry(course).Property(m => m.Display).IsModified = true;

            //db.Configuration.ValidateOnSaveEnabled = false;
            //db.SaveChanges();

            //TempData["SuccessMessage"] = "Public Event successfully deleted.";
            //return RedirectToAction("Index");
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