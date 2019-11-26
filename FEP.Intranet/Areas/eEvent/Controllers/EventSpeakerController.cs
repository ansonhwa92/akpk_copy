using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
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

		[HttpPost]
		public async Task<ActionResult> List(FilterEventSpeakerModel filter)
		{
			var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"eEvent/EventSpeaker/GetSpeakerList", filter);

			return Content(JsonConvert.SerializeObject(response.Data), "application/json");
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
				SpeakerType = SpeakerType.Internal,
				SpeakerStatus = SpeakerStatus.Active
			};

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateEventSpeakerModel model)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new CreateEventSpeakerModel()
				{
					//webapi = intranet
					UserId = model.UserId,
					UserName = model.UserName,
					SpeakerType = model.SpeakerType,
					Experience = model.Experience,
					SpeakerStatus = model.SpeakerStatus,
					//SpeakerPictureName = model.SpeakerPicture.FileName,
					//SpeakerAttachmentName = model.SpeakerAttachment.FileName,
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var responseFile = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

                    if (responseFile != null)
					{
						modelapi.FilesId = responseFile.Select(f => f.Id).ToList();
					}
				}

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventSpeaker", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Event Speaker", model);

					TempData["SuccessMessage"] = "Event Speaker successfully created";

					return RedirectToAction("List");
				}
			}
			//TempData["ErrorMessage"] = "Fail to add new Event Speaker";

            return View(model);
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
				Experience = response.Data.Experience,
				SpeakerStatus = response.Data.SpeakerStatus,
                Attachments = response.Data.Attachments
            };

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(FEP.Intranet.Areas.eEvent.Models.EditEventSpeakerModel model)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EditEventSpeakerModel()
				{
					UserId = model.UserId,
					UserName = model.UserName,
					SpeakerType = model.SpeakerType,
                    SpeakerStatus = model.SpeakerStatus,
					Experience = model.Experience,
					Attachments = model.Attachments,
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var responseFile = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

                    if (responseFile != null)
					{
						modelapi.FilesId = responseFile.Select(f => f.Id).ToList();
					}
				}

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventSpeaker?id={model.Id}", modelapi);

				if (response.isSuccess)
				{
					//LogActivity("Update Event Speaker", model);

					TempData["SuccessMessage"] = "Event Speaker successfully updated";

					return RedirectToAction("List");
				}
			}

			model.UserIds = new SelectList(await GetUsers(), "Id", "Name", 0);

			//TempData["ErrorMessage"] = "Fail to update Event Speaker";

            return View(model);

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
				Experience = response.Data.Experience,
				Email = response.Data.Email,
				PhoneNo = response.Data.PhoneNo,
				SpeakerStatus = response.Data.SpeakerStatus,
				Attachments = response.Data.Attachments,
				//SpeakerPictureName = response.Data.SpeakerPictureName,
				//SpeakerAttachmentName = response.Data.SpeakerAttachmentName,
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
