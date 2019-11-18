using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.PublicEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FEP.Model;
using FEP.Helper;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class EventAgendaController : FEPController
	{
		// GET: eEvent/EventAgenda
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List()
		{
			return View();
		}

		// GET: eEvent/EventAgenda/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventAgendaModel>(HttpVerbs.Get, $"eEvent/EventAgenda?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// GET: eEvent/EventAgenda/Create
		public async Task<ActionResult> Create()
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.CreateEventAgendaModel() { };

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// POST: eEvent/EventAgenda/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateEventAgendaModel model)
		{

			if (ModelState.IsValid)
			{
				var modelapi = new CreateEventAgendaModel
				{
					AgendaTitle = model.AgendaTitle,
					AgendaDescription = model.AgendaDescription,
					Time = model.Time,
					Tentative = model.Tentative,
					EventId = model.EventId,
					PersonInChargeId = model.PersonInChargeId
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var responseFile = await WepApiMethod.SendApiAsync<List<FileDocument>>($"File?userId={CurrentUser.UserId}", model.AttachmentFiles.ToList());

					if (responseFile.isSuccess)
					{
						modelapi.FilesId = responseFile.Data.Select(f => f.Id).ToList();
					}
				}

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventAgenda", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Event Agenda", model);
					TempData["SuccessMessage"] = "Event Agenda successfully created";

					return RedirectToAction("List");
				}
			}

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// GET: eEvent/EventAgenda/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var response = await WepApiMethod.SendApiAsync<EditEventAgendaModel>(HttpVerbs.Get, $"eEvent/EventAgenda?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EditEventAgendaModel()
			{
				AgendaTitle = response.Data.AgendaTitle,
				AgendaDescription = response.Data.AgendaDescription,
				Time = response.Data.Time,
				Tentative = response.Data.Tentative,
				PersonInChargeId = response.Data.PersonInChargeId,
				EventId = response.Data.EventId,
				Attachments = response.Data.Attachments
			};

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// POST: eEvent/EventAgenda/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(FEP.Intranet.Areas.eEvent.Models.EditEventAgendaModel model)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EditEventAgendaModel
				{
					Id = model.Id,
					AgendaTitle = model.AgendaTitle,
					AgendaDescription = model.AgendaDescription,
					Time = model.Time,
					Tentative = model.Tentative,
					EventId = model.EventId,
					PersonInChargeId = model.PersonInChargeId,
					Attachments = model.Attachments,
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var responseFile = await WepApiMethod.SendApiAsync<List<FileDocument>>($"File?userId={CurrentUser.UserId}", model.AttachmentFiles.ToList());

					if (responseFile.isSuccess)
					{
						modelapi.FilesId = responseFile.Data.Select(f => f.Id).ToList();
					}

				}

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventAgenda?id={model.Id}", modelapi);

				if (response.isSuccess)
				{

					await LogActivity(Modules.Event, "Edit Event Agenda", model);
					TempData["SuccessMessage"] = "Event Agenda successfully updated";

					return RedirectToAction("List");
				}
			}

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// GET: eEvent/EventAgenda/Delete/5
		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventAgendaModel>(HttpVerbs.Get, $"eEvent/EventAgenda?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsEventAgendaModel()
			{
				AgendaTitle = response.Data.AgendaTitle,
				AgendaDescription = response.Data.AgendaDescription,
				Time = response.Data.Time,
				Tentative = response.Data.Tentative,
				PersonInChargeId = response.Data.PersonInChargeId,
				EventId = response.Data.EventId,
				EventName = response.Data.EventName,
				Attachments = response.Data.Attachments
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.EventIds = new SelectList(await GetEvent(), "Id", "RefNo");
			model.PersonInchargeIds = new SelectList(await GetUsers(), "Id", "Name");

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

	}

}
