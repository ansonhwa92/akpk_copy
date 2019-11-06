using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.PublicEvent;
using FEP.WebApiModel.RnP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
    public class EventRefundController : FEPController
    {
		private DbEntities db = new DbEntities();

		public async Task<ActionResult> List()
		{
			var filter = new FilterEventRefundModel();

			filter.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(new ListEventRefundModel { Filter = filter });
		}

		// GET: eEvent/EventRefund
		public ActionResult Index()
        {
            return View();
        }

        // GET: eEvent/EventRefund/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.CreateEventRefundModel();

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name");
			model.BankInformationIds = new SelectList(await GetBank(), "Id", "Name");
			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateEventRefundModel model)
		{
		
			if (ModelState.IsValid)
			{
				var modelapi = new CreateEventRefundModel()
				{
					EventId = model.EventId,
					EventName = model.EventName,
					UserId = model.UserId,
					UserName = model.UserName,
					BankInformationId = model.BankInformationId,
					BankInformationName = model.BankInformationName,
					AccountNumber = model.AccountNumber,
				};

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventRefund", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Update Event Refund", model);

					TempData["SuccessMessage"] = "Event Refund successfully updated";

					return RedirectToAction("List");
				}
			}
			TempData["ErrorMessage"] = "Fail to update Event Refund";

			return RedirectToAction("List");
		}

		// GET: eEvent/EventRefund/Edit/5
		public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: eEvent/EventRefund/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		// GET: eEvent/EventRefund/Delete/5
		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventRefundModel>(HttpVerbs.Get, $"eEvent/EventRefund?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsEventRefundModel()
			{
				EventId = response.Data.EventId,
				EventName = response.Data.EventName,
				BankInformationId = response.Data.BankInformationId,
				BankInformationName = response.Data.BankInformationName,
				AccountNumber = response.Data.AccountNumber,
				UserId = response.Data.UserId,
				UserName = response.Data.UserName,
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.UserIds = new SelectList(await GetUsers(), "Id", "Refono");
			model.BankInformationIds = new SelectList(await GetBank(), "Id", "Name");
			model.EventIds = new SelectList(await GetEvent(), "Id", "Name");

			return View(model);
		}

		// POST: eEvent/EventAgenda/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{
			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventAgenda?id={id}");

			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, "Delete Event Agenda");
				TempData["SuccessMessage"] = "Event Agenda successfully deleted";
				return RedirectToAction("List", "EventAgenda", new { area = "eEvent" });
			}
			TempData["ErrorMessage"] = "Fail to delete Event Agenda";
			return RedirectToAction("List", "EventAgenda", new { area = "eEvent" });
		}


		[NonAction]
		private async Task<IEnumerable<UserModel>> GetUsers()
		{

			var roles = Enumerable.Empty<UserModel>();

			var response = await WepApiMethod.SendApiAsync<List<UserModel>>(HttpVerbs.Get, $"Administration/User");

			if (response.isSuccess)
			{
				roles = response.Data.OrderBy(o => o.Name);
			}

			return roles;

		}

		[NonAction]
		private async Task<IEnumerable<PublicEventModel>> GetEvent()
		{

			var roles = Enumerable.Empty<PublicEventModel>();

			var response = await WepApiMethod.SendApiAsync<List<PublicEventModel>>(HttpVerbs.Get, $"eEvent/PublicEvent");

			if (response.isSuccess)
			{
				roles = response.Data.OrderBy(o => o.RefNo);
			}

			return roles;
		}

		[NonAction]
		private async Task<IEnumerable<BankInformationModel>> GetBank() 
		{

			var roles = Enumerable.Empty<BankInformationModel>();

			var response = await WepApiMethod.SendApiAsync<List<BankInformationModel>>(HttpVerbs.Get, $"RnP/Cart/GetBanks");

			if (response.isSuccess)
			{
				roles = response.Data.OrderBy(o => o.FullName);
			}

			return roles;
		}
	}
}
