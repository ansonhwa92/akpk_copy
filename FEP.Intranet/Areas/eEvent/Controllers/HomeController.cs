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
				$"&symposium={symposium}&convention={convention}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var browser = response.Data;

			if (browser == null)
			{
				return HttpNotFound();
			}

			if (browser.Sorting == "EventTitle")
			{
				ViewBag.DefaultSorting = "";
				ViewBag.TitleSorting = "selected";
				ViewBag.YearSorting = "";
				ViewBag.AddedSorting = "";
			}
			else if (browser.Sorting == "CreatedDate")
			{
				ViewBag.DefaultSorting = "";
				ViewBag.TitleSorting = "";
				ViewBag.YearSorting = "selected";
				ViewBag.AddedSorting = "";
			}
			else if (browser.Sorting == "RefNo")
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

			ViewBag.SeatAvailable = "";

			return View(browser);
		}

		

		[ChildActionOnly]
		public ActionResult _Menu()
		{
			return PartialView();
		}

		[AllowAnonymous]
		public async Task<ActionResult> PublicEventDetails(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var publicevent = response.Data;

			return View(publicevent);
		}

		[AllowAnonymous]
		public async Task<ActionResult> EventSelectTicket(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var publicevent = response.Data;

			return View(publicevent);
		}

		public async Task<ActionResult> PurchasePublicEvent(string puid, string card_i, string card_g, string card_bil, string card_a)
		{
			var id = int.Parse(puid);

			var resPub = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

			if (!resPub.isSuccess)
			{
				return HttpNotFound();
			}

			var publicevent = resPub.Data;

			if (publicevent == null)
			{
				return HttpNotFound();
			}

			ViewBag.card_i = card_i;
			ViewBag.card_g = card_g;
			ViewBag.card_bil = card_bil;
			ViewBag.card_a = card_a;

			if (card_i == "true")
			{
				ViewBag.IndividualAmt = publicevent.IndividualFee;
			}
			else
			{
				ViewBag.IndividualAmt = 0;
			}
			if (card_g == "true")
			{
				ViewBag.GroupAmt = (publicevent.IndividualFee * int.Parse(card_bil));
			}
			else
			{
				ViewBag.GroupAmt = 0;
			}
			if (card_a == "true")
			{
				ViewBag.AgencyAmt = publicevent.AgencyFee;
			}
			else
			{
				ViewBag.AgencyAmt = 0;
			}

			ViewBag.TotalAmt = ViewBag.IndividualAmt + ViewBag.GroupAmt + ViewBag.AgencyAmt;

			return View(publicevent);
		}
	}
}