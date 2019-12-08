using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.LandingPage;
using FEP.WebApiModel.PublicEvent;
using FEP.WebApiModel.RnP;

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

			ViewBag.EventId = publicevent.Id;
			ViewBag.RefNo = publicevent.RefNo;
			ViewBag.EventTitle = publicevent.EventTitle;
			ViewBag.EventCategoryName = publicevent.EventCategoryName;
			ViewBag.StartDate = publicevent.StartDate.Value.ToString("dd/MM/yyyy");
			ViewBag.EndDate = publicevent.EndDate.Value.ToString("dd/MM/yyyy");
			ViewBag.Venue = publicevent.Venue;
			ViewBag.ParticipantAllowed = publicevent.ParticipantAllowed;

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

			var purchase = new PurchasePublicEventModel
			{
				EventId = publicevent.Id,
				IndividualTicket = bool.Parse(card_i),
				GroupTicket = bool.Parse(card_g),
				AgencyTicket = bool.Parse(card_a),
				AgencyTicketQuantity = int.Parse(card_bil),
				GroupTicketQuantity = int.Parse(card_bil),

				UserId = CurrentUser.UserId.Value,
			};

			return View(purchase);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<string> AddToCart(PurchasePublicEventModel model)
		{
			if (ModelState.IsValid)
			{
				var resPub = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={model.EventId}");

				if (!resPub.isSuccess)
				{
					return "notfound";
				}

				var publicevent = resPub.Data;

				if (publicevent == null)
				{
					return "notfound";
				}

				var order = new PurchaseOrderModel
				{
					UserId = CurrentUser.UserId.Value,
					DiscountCode = "",
					ProformaInvoiceNo = "",
					PaymentMode = PaymentModes.Online,
					CreatedDate = DateTime.Now,
					TotalPrice = 0,
					Status = CheckoutStatus.Shopping
				};

				var response_cart = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Commerce/Cart/Create", order);

				if (!response_cart.isSuccess)
				{
					return "notfound";
				}

				var cartid = response_cart.Data;

				var addsuccess = true;

				if (model.IndividualTicket)
				{
					var item_i = new PublicEventPurchaseItemModel
					{
						PurchaseOrderId = cartid,
						EventId = publicevent.Id,
						UserId = CurrentUser.UserId.Value,
						Ticket = ParticipantType.Individual,
						Price = publicevent.IndividualFee.Value,
						Quantity = 1
					};
					var response_i = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eEvent/PublicEvent/AddOrderItem", item_i);

					var citem1 = new PurchaseOrderItemModel
					{
						PurchaseOrderId = cartid,
						ItemId = publicevent.Id,
						Description = publicevent.EventTitle + " (Individual)",
						PurchaseType = PurchaseType.Publication,
						Price = publicevent.IndividualFee.Value,
						Quantity = 1
					};
					var cart_digital = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem1);
				}
				if (model.AgencyTicket)
				{
					var item_a = new PublicEventPurchaseItemModel
					{
						PurchaseOrderId = cartid,
						EventId = publicevent.Id,
						UserId = CurrentUser.UserId.Value,
						Ticket = ParticipantType.Agency,
						Price = publicevent.IndividualFee.Value,
						Quantity = model.AgencyTicketQuantity
					};
					var response_a = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eEvent/PublicEvent/AddOrderItem", item_a);

					var citem1 = new PurchaseOrderItemModel
					{
						PurchaseOrderId = cartid,
						ItemId = publicevent.Id,
						Description = publicevent.EventTitle + " (Agency)",
						PurchaseType = PurchaseType.Publication,
						Price = publicevent.AgencyFee.Value,
						Quantity = model.AgencyTicketQuantity
					};
					var cart_digital = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem1);
				}
				if (model.GroupTicket)
				{
					var item_g = new PublicEventPurchaseItemModel
					{
						PurchaseOrderId = cartid,
						EventId = publicevent.Id,
						UserId = CurrentUser.UserId.Value,
						Ticket = ParticipantType.Individual,
						Price = publicevent.IndividualFee.Value,
						Quantity = model.GroupTicketQuantity
					};
					var response_g = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eEvent/PublicEvent/AddOrderItem", item_g);

					var citem1 = new PurchaseOrderItemModel
					{
						PurchaseOrderId = cartid,
						ItemId = publicevent.Id,
						Description = publicevent.EventTitle + " (Group)",
						PurchaseType = PurchaseType.Publication,
						Price = publicevent.IndividualFee.Value,
						Quantity = model.GroupTicketQuantity
					};
					var cart_digital = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/AddItem", citem1);
				}

				if (addsuccess)
				{
					await LogActivity(Model.Modules.Event, "Purchase Public Event", publicevent);

					return "success";
				}
				else
				{
					return "failure";
				}
			}
			return "invalid";
		}



	}
}