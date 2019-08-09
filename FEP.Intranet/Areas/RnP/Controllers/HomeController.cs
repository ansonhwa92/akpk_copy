using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class HomeController : FEPController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            if (CurrentUser.IsAuthenticated())
            {
                view.MasterName = "~/Views/Shared/_Layout.cshtml";
            }

            return view;
        }
    }
}