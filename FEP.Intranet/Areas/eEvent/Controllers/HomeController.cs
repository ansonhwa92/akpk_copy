using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.WebApiModel.LandingPage;
using FEP.WebApiModel.PublicEvent;

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
		public async Task<ActionResult> BrowseEvent(string keyword, string sorting, bool? workshops, bool? seminars, bool? dialogues,
			bool? conferences, bool? symposium, bool? convention)
		{
			if (keyword == null) keyword = "";
			if (sorting == null) sorting = "default";
			if (workshops == null) workshops = true;
			if (seminars == null) seminars = true;
			if (dialogues == null) dialogues = true;
			if (conferences == null) conferences = true;
			if (symposium == null) symposium = true;
			if (convention == null) convention = true;

			var response = await WepApiMethod.SendApiAsync<BrowseEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetPublishedPublicEvent" +
				$"?keyword={keyword}&sorting={sorting}&workshops={workshops}&seminars={seminars}&dialogues={dialogues}&conferences={conferences}" +
				$"symposium={symposium}&convention={convention}");

			//if (!response.isSuccess)
			//{
			//	return HttpNotFound();
			//}

			var browser = response.Data;

			//if (browser == null)
			//{
			//	return HttpNotFound();
			//}

			if (browser.Sorting == "title")
			{
				ViewBag.DefaultSorting = "";
				ViewBag.TitleSorting = "selected";
				ViewBag.YearSorting = "";
				ViewBag.AddedSorting = "";
			}
			else if (browser.Sorting == "year")
			{
				ViewBag.DefaultSorting = "";
				ViewBag.TitleSorting = "";
				ViewBag.YearSorting = "selected";
				ViewBag.AddedSorting = "";
			}
			else if (browser.Sorting == "added")
			{
				ViewBag.DefaultSorting = "";
				ViewBag.TitleSorting = "";
				ViewBag.YearSorting = "";
				ViewBag.AddedSorting = "selected";
			}
			else
			{
				ViewBag.DefaultSorting = "selected";
				ViewBag.TitleSorting = "";
				ViewBag.YearSorting = "";
				ViewBag.AddedSorting = "";
			}

			ViewBag.Typeworkshops = "";
			ViewBag.Typeseminars = "";
			ViewBag.Typedialogues = "";
			ViewBag.Typeconferences = "";
			ViewBag.Typesymposium = "";
			ViewBag.Typeconvention = "";

			if ((bool)workshops) { ViewBag.Typeworkshops = "checked"; }
			if ((bool)seminars) { ViewBag.Typeseminars = "checked"; }
			if ((bool)dialogues) { ViewBag.Typedialogues = "checked"; }
			if ((bool)conferences) { ViewBag.Typeconferences = "checked"; }
			if ((bool)symposium) { ViewBag.Typesymposium = "checked"; }
			if ((bool)convention) { ViewBag.Typeconvention = "checked"; }


			return View(browser);
		}

		[AllowAnonymous]
		public async Task<ActionResult> PublicEventDetails(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var resPub = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/GetDelete?id={id}");

			if (!resPub.isSuccess)
			{
				return HttpNotFound();
			}

			var publicevent = resPub.Data;

			if (publicevent == null)
			{
				return HttpNotFound();
			}

			return View(publicevent);
		}

		[ChildActionOnly]
		public ActionResult _Menu()
		{
			return PartialView();
		}
	}

}