using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.Helper;
using System.Threading.Tasks;
using FEP.WebApiModel.eLearning;
using AutoMapper;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class ApiUrl
    {
        public const string GetCategory = "eLearning/CourseCategory/";
        public const string CreateCourse = "eLearning/Courses/Create";
        public const string GetCourse = "eLearning/Courses";
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
            var response = await WepApiMethod.SendApiAsync<IEnumerable<CourseCategoryModel>>(HttpVerbs.Get, ApiUrl.GetCategory);

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

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, ApiUrl.CreateCourse, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Course successfully created. Now you can add contents and rules.";

                    await LogActivity(Modules.Learning, "Create Course : " + model.Title);

                    //return RedirectToAction("Manage", new { area = "eLearning", id = model.Id });


                    var id = response.Data;

                    if (!String.IsNullOrEmpty(id))

                        return RedirectToAction("Manage", "Courses", new { id = id });
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



        public async Task<ActionResult> Manage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get Model
            var model = await TryGetCourse(id.Value);

            if(model == null)
            {
                TempData["ErrorMessage"] = "No such course.";

                return RedirectToAction("Index", "Courses");
            }

            return View(model);

        }


        public async Task<CreateOrEditCourseModel> TryGetCourse(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditCourseModel>(HttpVerbs.Get, ApiUrl.GetCourse + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }


        // GET: eLearning/Courses/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name", course.CategoryId);
            ViewBag.FirstApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.FirstApprovalId);
            ViewBag.SecondApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.SecondApprovalId);
            ViewBag.ThirdApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.ThirdApprovalId);
            ViewBag.VerifierApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.VerifierApprovalId);
            return View(course);
        }

        // POST: eLearning/Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Code,Objectives,Medium,ScheduleType,Duration,DurationType,Language,CategoryId,IsFree,Price,Status,IntroVideoPath,TraversalRule,ScoreCalculation,VerifierApprovalId,FirstApprovalId,SecondApprovalId,ThirdApprovalId,TotalModules,CertificateId,DefaultAllowablePercentageBeforeWithdraw,ViewCategory,CompletionCriteriaType,ModulesCompleted,PercentageCompletion,TestsPassed,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.RefCourseCategories, "Id", "Name", course.CategoryId);
            ViewBag.FirstApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.FirstApprovalId);
            ViewBag.SecondApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.SecondApprovalId);
            ViewBag.ThirdApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.ThirdApprovalId);
            ViewBag.VerifierApprovalId = new SelectList(db.CourseApprovals, "Id", "Remark", course.VerifierApprovalId);
            return View(course);
        }

        // GET: eLearning/Courses/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: eLearning/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
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
