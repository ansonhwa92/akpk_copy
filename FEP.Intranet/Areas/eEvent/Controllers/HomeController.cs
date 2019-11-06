﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

			return RedirectToAction("BrowseEvent", "Home", new { area = "eEvent" });
		}

		[AllowAnonymous]
		public async Task<ActionResult> BrowseEvent()
		{


			return View();
		}

		[ChildActionOnly]
        public ActionResult _Menu()
        {
            return PartialView();
        }
    }

}