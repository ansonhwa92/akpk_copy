using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.PublicEvent;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class EventAttendentController : FEPController
	{
		// GET: eEvent/EventAttendent
		public ActionResult Index()
		{
			return View();
		}

		//public async Task<ActionResult> List(int? id)
		public ActionResult List(int? id)
		{
			//var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

			//if (!response.isSuccess)
			//{
			//	return HttpNotFound();
			//}

			//model.List.EventName = response.Data.EventTitle;

			ViewBag.EventId = id;


			return View();
		}

		// GET: eEvent/EventAttendent/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: eEvent/EventAttendent/Create
		public async Task<ActionResult> Create()
		{
			var model = new CreateEventAttendentModel() { };

			model.EventList = new SelectList(await GetEvent(), "Id", "RefNo");

			return View(model);
		}

		// POST: eEvent/EventAttendent/Create
		[HttpPost]
		public async Task<ActionResult> Create(CreateEventAttendentModel model)
		{

			if (ModelState.IsValid)
			{
				model.CreatedBy = CurrentUser.UserId;
				model.CreatedDate = DateTime.Now;
				model.Display = true;


				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventAttendent", model);
				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Add Participant", model);
					TempData["SuccessMessage"] = "Participant successfully added";
					return RedirectToAction("List");
				}
			}
			model.EventList = new SelectList(await GetEvent(), "Id", "RefNo");

			return View(model);
		}

		// GET: eEvent/EventAttendent/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: eEvent/EventAttendent/Edit/5
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

		// GET: eEvent/EventAttendent/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: eEvent/EventAttendent/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		[NonAction]
		private async Task<IEnumerable<PublicEventModel>> GetEvent()
		{
			var geteventlist = Enumerable.Empty<PublicEventModel>();

			var response = await WepApiMethod.SendApiAsync<List<PublicEventModel>>(HttpVerbs.Get, $"eEvent/PublicEvent");

			if (response.isSuccess)
			{
				geteventlist = response.Data.OrderBy(o => o.RefNo);
			}

			return geteventlist;

		}
	}
}
