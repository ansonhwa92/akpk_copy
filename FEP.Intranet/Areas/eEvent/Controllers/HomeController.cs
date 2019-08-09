using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
    public class HomeController : FEPController
    {
        [AllowAnonymous]
        // GET: eEvent/Home
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