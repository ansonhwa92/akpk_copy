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
using System.Threading.Tasks;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class HomeController : FEPController
    {
        // Landing page - redirects to dashboard or browse elearning based on login state
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            if (CurrentUser.IsAuthenticated())
            {
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

            return RedirectToAction("BrowseElearnings", "Home", new { area = "eLearning" });
        }

        // Browse Elearning
        // TODO: Handle search/filtering, include star rating
        // GET: Elearning/Home/BrowseElearning
        [AllowAnonymous]
        public async Task<ActionResult> BrowseElearnings(string keyword, string sorting, bool? cashflow, bool? car, bool? house, bool? investment, bool? protection, /*bool? beginner, bool? intermediate, bool? advanced,*/ bool? english, bool? malay, bool? chinese, bool? tamil, bool? multiLanguage)
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


            // get anonymous surveys
            //&beginner={beginner}&intermediate={intermediate}&advanced={advanced}
            var resPubs = await WepApiMethod.SendApiAsync<BrowseElearningModel>(HttpVerbs.Get, $"eLearning/Courses/GetCourses?keyword={keyword}&sorting={sorting}&cashflow={cashflow}&car={car}&house={house}&investment={investment}&protection={protection}&english={english}&malay={malay}&chinese={chinese}&tamil={tamil}&multiLanguage={multiLanguage}");

            if (!resPubs.isSuccess)
            {
                return HttpNotFound();
            }

            var browser = resPubs.Data;

            if (browser == null)
            {
                return HttpNotFound();
            }

            if (browser.Sorting == "title")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.TitleSorting = "selected";
                //ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "";
            }
            //else if (browser.Sorting == "year")
            //{
            //    ViewBag.DefaultSorting = "";
            //    ViewBag.TitleSorting = "";
            //    ViewBag.YearSorting = "selected";
            //    ViewBag.AddedSorting = "";
            //}
            else if (browser.Sorting == "added")
            {
                ViewBag.DefaultSorting = "";
                ViewBag.TitleSorting = "";
                //ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "selected";
            }
            else
            {
                ViewBag.DefaultSorting = "selected";
                ViewBag.TitleSorting = "";
                //ViewBag.YearSorting = "";
                ViewBag.AddedSorting = "";
            }

            ViewBag.TypeCashflow = "";
            ViewBag.TypeCar = "";
            ViewBag.TypeHouse = "";
            ViewBag.TypeInvestment = "";
            ViewBag.TypeProtection = "";

            //ViewBag.LevelBeginner = "";
            //ViewBag.LevelIntermediate = "";
            //ViewBag.LevelAdvanced = "";

            ViewBag.LanguageEnglish = "";
            ViewBag.LanguageMalay = "";
            ViewBag.LanguageChinese = "";
            ViewBag.LanguageTamil = "";
            ViewBag.LanguageMultiLanguage = "";


            if ((bool)cashflow) { ViewBag.TypeCashflow = "checked"; }
            if ((bool)car) { ViewBag.TypeCar = "checked"; }
            if ((bool)house) { ViewBag.TypeHouse = "checked"; }
            if ((bool)investment) { ViewBag.TypeInvestment = "checked"; }
            if ((bool)protection) { ViewBag.TypeProtection = "checked"; }

            //if ((bool)beginner) { ViewBag.LevelBeginner = "checked"; }
            //if ((bool)intermediate) { ViewBag.LevelIntermediate = "checked"; }
            //if ((bool)advanced) { ViewBag.LevelAdvanced = "checked"; }

            if ((bool)english) { ViewBag.LanguageEnglish = "checked"; }
            if ((bool)malay) { ViewBag.LanguageMalay = "checked"; }
            if ((bool)chinese) { ViewBag.LanguageChinese = "checked"; }
            if ((bool)tamil) { ViewBag.LanguageTamil = "checked"; }
            if ((bool)multiLanguage) { ViewBag.LanguageMultiLanguage = "checked"; }


            return View(browser);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {

            return PartialView("_Menu");
        }
    }
}