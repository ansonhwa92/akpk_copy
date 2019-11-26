using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class EventExternalExhibitorController : FEPController 
	{
		private DbEntities db = new DbEntities();
		// GET: eEvent/EventExternalExhibitor
		public ActionResult List()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> List(FilterEventExternalExhibitorModel filter)
		{
			var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"eEvent/EventExternalExhibitor/GetExternalExhibitorList", filter);

			return Content(JsonConvert.SerializeObject(response.Data), "application/json");
		}

		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventExternalExhibitorModel>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;


			return View(model);
		}

		[HttpGet]
		public ActionResult Create()
		{
			var model = new CreateEventExternalExhibitorModel();

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(CreateEventExternalExhibitorModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventExternalExhibitor", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "External Exhibitor successfully added";

					return RedirectToAction("List");
				}
			}
			TempData["ErrorMessage"] = "Fail to add new External Exhibitor";

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<ActionResult> Edit(int? id)
		{

			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<EditEventExternalExhibitorModel>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditEventExternalExhibitorModel model)
		{

			var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor/IsNameExist?id={model.Id}&name={model.Name}");

			if (nameResponse.isSuccess)
			{
				TempData["ErrorMessage"] = " Name already exist in the system";
				return RedirectToAction("List");
			}

			if (ModelState.IsValid)
			{

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventExternalExhibitor?id={model.Id}", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "External Exhibitor successfully updated";

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = "Fail to update EventExternalExhibitor";

			return RedirectToAction("List");

		}

		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventExternalExhibitorModel>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;


			return View(model);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{

			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventExternalExhibitor?id={id}");

			if (response.isSuccess)
			{
				TempData["SuccessMessage"] = "External Exhibitor successfully deleted";

				return RedirectToAction("List", "EventExternalExhibitor", new { area = "eEvent" });
			}

			TempData["ErrorMessage"] = "Fail to delete Exhibition/Roadshow Request";

			return RedirectToAction("List", "EventExternalExhibitor", new { area = "eEvent" });

		}
	}
}