using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.WebApiModel.LandingPage;

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
		public async Task<ActionResult> BrowseEvent(string keyword, string sorting, bool? Workshops, bool? seminars, bool? dialogues, bool? conferences, bool? symposium, bool? convention)
		{
			var response = await WepApiMethod.SendApiAsync<BrowseEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetPublishedPublicEvent");
				 if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			return View();
		}

		[ChildActionOnly]
		public ActionResult _Menu()
		{
			return PartialView();
		}
	}

}