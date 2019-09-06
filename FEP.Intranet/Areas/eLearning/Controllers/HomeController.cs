using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class HomeController : FEPController
    {
        // GET: eLearning/Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            if (CurrentUser.IsAuthenticated())
            {
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

            return view;
        }
    }
}