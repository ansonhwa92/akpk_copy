using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.Model;
using AutoMapper;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class HomeController : FEPController
    {
        private DbEntities db = new DbEntities();
        private readonly IMapper _mapper;

        public HomeController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditCourseModel, Course>();

                cfg.CreateMap<Course, CreateOrEditCourseModel>();

            });

            _mapper = config.CreateMapper();
        }

        // GET: eLearning/Home
        //[AllowAnonymous]
        //public ActionResult Index()
        //{
        //    var view = View();
        //    view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

        //    if (CurrentUser.IsAuthenticated())
        //    {
        //        return RedirectToAction("Dashboard", "Home", new { area = "" });
        //    }

        //    return view;
        //}

        [AllowAnonymous]
        public ActionResult Index()
        {
            var category = db.RefCourseCategories.Where(x => x.IsDisplayed == true).ToList();
            var listCourses = new ReturnDashboardCourseModel()
            {
                CourseCategory = category
            };

            return View(listCourses);
        }

        [AllowAnonymous]
        public ActionResult GetPublicCourses()
        {
            var entity = db.Courses.Where(x => x.IsDeleted == false).ToList();

            //if (entity == null)
            //    return HttpNotFound();

            var courses = new List<DashboardCourseModel>();
            foreach (var item in entity)
            {
                var enrolls = db.Enrollments.Where(x => x.CourseId == item.Id).Count();
                var instructor = db.TrainerCourses.FirstOrDefault(x => x.CourseId == item.Id);

                courses.Add(new DashboardCourseModel
                {
                    Title = item.Title,
                    Description = item.Description,
                    Code = item.Code,
                    CategoryId = item.CategoryId,
                    Price = item.Price.ToString(),
                    Status = item.Status,
                    IntroImageFileName = item.IntroImageFileName,
                    TotalModules = item.Modules.Count(),
                    TotalStudent = enrolls,
                    InstructorBy = instructor?.Trainer.User.Name
                });
            }

            var listCourses = new ReturnDashboardCourseModel()
            {
                Courses = courses
            };

            //if (CurrentUser.IsAuthenticated())
            //{
            //    return RedirectToAction("Dashboard", "Home", new { area = "" });
            //}

            //return Json(new { result = listCourses }, JsonRequestBehavior.AllowGet);

            return PartialView("_Course", listCourses );
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {         

            return PartialView("_Menu");
        }
    }
}