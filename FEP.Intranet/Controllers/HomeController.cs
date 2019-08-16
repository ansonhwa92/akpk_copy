using FEP.Helper;
using FEP.Intranet.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Controllers
{
    public class HomeController : FEPController
    {
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPagePublic.cshtml";
           
            if (CurrentUser.IsAuthenticated())
            {
                view.MasterName = "~/Views/Shared/_Layout1.cshtml";
            }

            return view;
        }
        
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Profiles()
        {
            var userid = CurrentUser.UserId;

            return View();
        }
        
    }
}