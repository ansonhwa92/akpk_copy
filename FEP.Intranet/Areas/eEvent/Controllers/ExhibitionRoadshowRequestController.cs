using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class ExhibitionRoadshowRequestController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEvent/ExhibitionRoadshowRequest
		public ActionResult List()
		{
			return View();
		}


		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsExhibitionRoadshowRequestModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			var model = new CreateExhibitionRoadshowRequestModel();

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(CreateExhibitionRoadshowRequestModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully added";

					return RedirectToAction("List");
				}
			}
			TempData["ErrorMessage"] = "Fail to add new Exhibition/Roadshow Request";

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<ActionResult> Edit(int? id)
		{

			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<EditExhibitionRoadshowRequestModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditExhibitionRoadshowRequestModel model)
		{

			//var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/IsEventNameExist?id={model.Id}&name={model.EventName}");

			//if (nameResponse.isSuccess)
			//{
			//	TempData["ErrorMessage"] = "Event Name already exist in the system";
			//	return RedirectToAction("List");
			//}

			if (ModelState.IsValid)
			{

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/ExhibitionRoadshowRequest?id={model.Id}", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully updated";

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = "Fail to update Exhibition/Roadshow Request";

			return RedirectToAction("List");

		}

		[HttpGet]
		public async Task<ActionResult> Delete(int? id) 
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsExhibitionRoadshowRequestModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{

			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (response.isSuccess)
			{
				TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully deleted";

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}

			TempData["ErrorMessage"] = "Fail to delete Exhibition/Roadshow Request";

			return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });

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


	}
}