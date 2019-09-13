using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
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
	public class EventSpeakerController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEventSpeaker/EventSpeaker
		public async Task<ActionResult> List()
		{
			var filter = new FilterEventSpeakerModel();

			filter.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(new ListEventSpeakerModel { Filter = filter });
		}

		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventSpeakerModel>(HttpVerbs.Get, $"eEvent/EventSpeaker?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.CreateEventSpeakerModel()
			{
				DateAssigned = DateTime.Now
			};

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateEventSpeakerModel model)
		{
			if (ModelState.IsValid)
			{
				var modelapi = new CreateEventSpeakerModel()
				{
					//webapi = intranet
					UserId = model.UserId,
					UserName = model.UserName,
					SpeakerType = model.SpeakerType,
					DateAssigned = model.DateAssigned,
					Experience = model.Experience,
					Email = model.Email,
					Remark = model.Remark,
					Religion = model.Religion,
					PhoneNo = model.PhoneNo,
					DateOfBirth = model.DateOfBirth,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					MaritialStatus = model.MaritialStatus,
					SpeakerPictureName = model.SpeakerPicture.FileName,
					SpeakerAttachmentName = model.SpeakerAttachment.FileName,
				};

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventSpeaker", modelapi);

				if (response.isSuccess)
				{
					//LogActivity("Create Event Speaker");

					TempData["SuccessMessage"] = "Event Speaker successfully added";

					return RedirectToAction("List");
				}
			}
			TempData["ErrorMessage"] = "Fail to add new Event Speaker";

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<EditEventSpeakerModel>(HttpVerbs.Get, $"eEvent/EventSpeaker?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EditEventSpeakerModel()
			{
				UserId = response.Data.UserId,
				UserName = response.Data.UserName,
				SpeakerType = response.Data.SpeakerType,
				DateAssigned = response.Data.DateAssigned,
				Experience = response.Data.Experience,
				Email = response.Data.Email,
				Remark = response.Data.Remark,
				Religion = response.Data.Religion,
				PhoneNo = response.Data.PhoneNo,
				DateOfBirth = response.Data.DateOfBirth,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				MaritialStatus = response.Data.MaritialStatus,
				SpeakerPictureName = response.Data.SpeakerPictureName,
				SpeakerAttachmentName = response.Data.SpeakerAttachmentName,
			};

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditEventSpeakerModel model)
		{
			if (ModelState.IsValid)
			{
				//var modelapi = new EditEventSpeakerModel()
				//{
				//	UserId = model.UserId,
				//	UserName = model.UserName,
				//	SpeakerType = model.SpeakerType,
				//	DateAssigned = model.DateAssigned,
				//	Experience = model.Experience,
				//	Email = model.Email,
				//	Remark = model.Remark,
				//	Religion = model.Religion,
				//	PhoneNo = model.PhoneNo,
				//	DateOfBirth = model.DateOfBirth,
				//	AddressStreet1 = model.AddressStreet1,
				//	AddressStreet2 = model.AddressStreet2,
				//	AddressPoscode = model.AddressPoscode,
				//	AddressCity = model.AddressCity,
				//	State = model.State,
				//	MaritialStatus = model.MaritialStatus,
				//	SpeakerPictureName = model.SpeakerPicture.FileName,
				//	SpeakerAttachmentName = model.SpeakerAttachment.FileName,
				//};

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventSpeaker?id={model.Id}", model);

				if (response.isSuccess)
				{
					//LogActivity("Update Event Speaker", model);

					TempData["SuccessMessage"] = "Event Speaker successfully updated";

					return RedirectToAction("List");
				}
			}
			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			TempData["ErrorMessage"] = "Fail to update Event Speaker";

			return RedirectToAction("List");

		}

		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsEventSpeakerModel>(HttpVerbs.Get, $"eEvent/EventSpeaker?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsEventSpeakerModel()
			{
				UserId = response.Data.UserId,
				UserName = response.Data.UserName,
				SpeakerType = response.Data.SpeakerType,
				DateAssigned = response.Data.DateAssigned,
				Experience = response.Data.Experience,
				Email = response.Data.Email,
				Remark = response.Data.Remark,
				Religion = response.Data.Religion,
				PhoneNo = response.Data.PhoneNo,
				DateOfBirth = response.Data.DateOfBirth,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				MaritialStatus = response.Data.MaritialStatus,
				SpeakerPictureName = response.Data.SpeakerPictureName,
				SpeakerAttachmentName = response.Data.SpeakerAttachmentName,
			};

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{

			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventSpeaker?id={id}");

			if (response.isSuccess)
			{
				//LogActivity("Delete Event Speaker");

				TempData["SuccessMessage"] = "Event Speaker successfully deleted";

				return RedirectToAction("List", "EventSpeaker", new { area = "eEvent" });
			}

			TempData["ErrorMessage"] = "Fail to delete Event Speaker";

			return RedirectToAction("List", "EventSpeaker", new { area = "eEvent" });

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
