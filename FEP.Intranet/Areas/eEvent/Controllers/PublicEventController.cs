using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.PublicEvent;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class PublicEventController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEvent/PublicEvent
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Event_Setting()
		{
			return View();
		}

		public async Task<ActionResult> Create_SelectCategory()
		{
			var response = await WepApiMethod.SendApiAsync<List<EventCategoryModel>>(HttpVerbs.Get, $"eEvent/EventCategory");

			if (response.isSuccess)
				return View(response.Data);

			return View(new List<EventCategoryModel>());

		}

		public ActionResult List()
		{
			return View();
		}

		// GET: PublicEvent/Details/Id
		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsPublicEventModel()
			{
				Id = response.Data.Id,
				EventTitle = response.Data.EventTitle,
				EventObjective = response.Data.EventObjective,
				StartDate = response.Data.StartDate,
				EndDate = response.Data.EndDate,
				Venue = response.Data.Venue,
				Fee = response.Data.Fee,
				ParticipantAllowed = response.Data.ParticipantAllowed,
				TargetedGroup = response.Data.TargetedGroup,
				EventStatus = response.Data.EventStatus,
				EventCategoryId = response.Data.EventCategoryId,
				EventCategoryName = response.Data.EventCategoryName,
				Remarks = response.Data.Remarks,
				origin = response.Data.origin,
				RefNo = response.Data.RefNo,
				Attachments = response.Data.Attachments,
				SpeakerId = response.Data.SpeakerId,
				ExternalExhibitorId = response.Data.ExternalExhibitorId,
				CreatedByName = response.Data.CreatedByName,
				CreatedDate = response.Data.CreatedDate,

			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name", 0);
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}


		[HttpGet]
		public async Task<ActionResult> Create(int? ctgryId)
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.CreatePublicEventModel()
			{
				EventStatus = EventStatus.New,
				EventCategoryId = ctgryId,
			};

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}


		// POST: PublicEvent/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreatePublicEventModel model, string Submittype)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (model.StartDate > model.EndDate)
			{
				ModelState.AddModelError("EndDate", "End Date must greater or equal than Start Date");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new CreatePublicEventModel
				{
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					EventStatus = model.EventStatus,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
					Remarks = model.Remarks,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var files = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

					if (files != null)
					{
						modelapi.FilesId = files.Select(f => f.Id).ToList();
					}
				}

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/PublicEvent", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Public Event", model);
					if (Submittype == "Save")
					{
						TempData["SuccessMessage"] = "Public Event successfully created";
						return RedirectToAction("List");
					}
					else if (Submittype == "Submit")
					{
						return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", id = response.Data });
					}

				}

			}

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}


		// GET: PublicEvent/Edit/5
		public async Task<ActionResult> Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var response = await WepApiMethod.SendApiAsync<EditPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EditPublicEventModel()
			{
				Id = response.Data.Id,
				EventTitle = response.Data.EventTitle,
				EventObjective = response.Data.EventObjective,
				StartDate = response.Data.StartDate,
				EndDate = response.Data.EndDate,
				Venue = response.Data.Venue,
				Fee = response.Data.Fee,
				ParticipantAllowed = response.Data.ParticipantAllowed,
				TargetedGroup = response.Data.TargetedGroup,
				EventStatus = response.Data.EventStatus,
				EventCategoryId = response.Data.EventCategoryId,
				EventCategoryName = response.Data.EventCategoryName,
				Remarks = response.Data.Remarks,
				origin = origin,
				RefNo = response.Data.RefNo,
				SpeakerId = response.Data.SpeakerId,
				ExternalExhibitorId = response.Data.ExternalExhibitorId,
				Attachments = response.Data.Attachments
			};

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}

		// POST: PublicEvent/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(FEP.Intranet.Areas.eEvent.Models.EditPublicEventModel model)
		{

			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (model.StartDate > model.EndDate)
			{
				ModelState.AddModelError("EndDate", "End Date must greater or equal than Start Date");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EditPublicEventModel
				{
					Id = model.Id,
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					EventStatus = model.EventStatus,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
					Remarks = model.Remarks,
					Attachments = model.Attachments,
				};

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var files = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

					if (files != null)
					{
						modelapi.FilesId = files.Select(f => f.Id).ToList();
					}

				}

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/PublicEvent?id={model.Id}", modelapi);

				if (response.isSuccess)
				{

					await LogActivity(Modules.Event, "Edit Public Event", model);
					TempData["SuccessMessage"] = "Public Event successfully updated";

					return RedirectToAction("List");
				}
			}

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}


		// GET: PublicEvent/Delete/5
		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsPublicEventModel()
			{
				Id = response.Data.Id,
				EventTitle = response.Data.EventTitle,
				EventObjective = response.Data.EventObjective,
				StartDate = response.Data.StartDate,
				EndDate = response.Data.EndDate,
				Venue = response.Data.Venue,
				Fee = response.Data.Fee,
				ParticipantAllowed = response.Data.ParticipantAllowed,
				TargetedGroup = response.Data.TargetedGroup,
				EventStatus = response.Data.EventStatus,
				EventCategoryId = response.Data.EventCategoryId,
				EventCategoryName = response.Data.EventCategoryName,
				Remarks = response.Data.Remarks,
				origin = response.Data.origin,
				RefNo = response.Data.RefNo,
				Attachments = response.Data.Attachments,
				SpeakerId = response.Data.SpeakerId,
				ExternalExhibitorId = response.Data.ExternalExhibitorId
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{
			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/PublicEvent?id={id}");

			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, "Delete Public Event");
				TempData["SuccessMessage"] = "Public Event successfully deleted";
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			TempData["ErrorMessage"] = "Fail to delete Public Event";
			return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
		}

		// Submit Public Event for Verification
		public async Task<ActionResult> SubmitToVerify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/SubmitToVerify?id={id}");
			if (response.isSuccess)
			{
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Submit_PublicEvent}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_Public_Event_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};

					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						//save saveThisID dalam table
						response.Data.SLAReminderStatusId = saveThisID;


						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}


					await LogActivity(Modules.Event, "Submit Public Event Ref No: " + response.Data.RefNo + " for verification.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully submitted for verification.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else

			{
				TempData["ErrorMessage"] = "Fail to submit Public Event for verification.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		public async Task<ActionResult> Verified(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/Verified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();
				paramToSend.EventLocation = response.Data.Venue;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Verify_PublicEvent}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verify_Public_Event_After_Submit_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;

					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is verified.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully verified and submitted for approval.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//First Approved Public Event 
		public async Task<ActionResult> FirstApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/FirstApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver1_PublicEvent}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Public_Event_ByApprover_1,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is approved on first level.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//Second Approved Public Event 
		public async Task<ActionResult> SecondApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/SecondApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}
				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver2_PublicEvent}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Public_Event_ByApprover_2,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is approved on second level.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//Final Approved Public Event 
		public async Task<ActionResult> FinalApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/FinalApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}
				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver3_PublicEvent}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Public_Event_ByApprover_3,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is approved");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully approved.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//Reject Approved Public Event 
		public async Task<ActionResult> RejectPublicEvent(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/RejectPublicEvent?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}
				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_PublicEvent}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Reject_Public_Event,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }


					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is rejected and require amendment.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully rejected and require amendment.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//Cancel Approved Public Event
		public async Task<ActionResult> CancelPublicEvent(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/CancelPublicEvent?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAId?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}
				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Cancel_PublicEvent}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Cancel_Public_Event,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }


					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is cancelled.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully cancelled.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to cancel Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//Publised Public Event
		public async Task<ActionResult> PublishedPublicEvent(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<PublicEventModel>(HttpVerbs.Post, $"eEvent/PublicEvent/PublishedPublicEvent?id={id}");
			if (response.isSuccess)
			{

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.EventApproval = response.Data.EventStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Published_PublicEvent}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Publish_Public_Event,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }


					await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data.RefNo + " is Published.");
					TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data.RefNo + ", successfully Published.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to cancel Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		[NonAction]
		private async Task<IEnumerable<EventSpeakerModel>> GetSpeaker()
		{
			var speaker = Enumerable.Empty<EventSpeakerModel>();

			var response = await WepApiMethod.SendApiAsync<List<EventSpeakerModel>>(HttpVerbs.Get, $"eEvent/EventSpeaker");

			if (response.isSuccess)
			{
				speaker = response.Data.OrderBy(o => o.UserName);
			}
			return speaker;
		}


		[NonAction]
		private async Task<IEnumerable<EventExternalExhibitorModel>> GetExternalExhibitor()
		{
			var exhibitor = Enumerable.Empty<EventExternalExhibitorModel>();

			var response = await WepApiMethod.SendApiAsync<List<EventExternalExhibitorModel>>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor");

			if (response.isSuccess)
			{
				exhibitor = response.Data.OrderBy(o => o.Name);
			}

			return exhibitor;

		}

		[NonAction]
		private async Task<IEnumerable<EventCategoryModel>> GetCategory()
		{
			var category = Enumerable.Empty<EventCategoryModel>();

			var response = await WepApiMethod.SendApiAsync<List<EventCategoryModel>>(HttpVerbs.Get, $"eEvent/EventCategory");

			if (response.isSuccess)
			{
				category = response.Data.OrderBy(o => o.Name);
			}

			return category;

		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Details(GlobalPublicEventApprovalModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/Evaluate", model);

		//		if (response.isSuccess)
		//		{
		//			string[] resparray = response.Data.Split('|');
		//			int pid = int.Parse(resparray[0]);
		//			string title = resparray[1];
		//			string refno = resparray[2];
		//			string venue = resparray[3];
		//			// log trail/system success notification/dashboard notification/email/sms upon submission
		//			// log trail/system success/dashboard notification upon saving as draft

		//			if (model.approval.Status == EventApprovalStatus.Approved)
		//			{
		//				if (model.approval.Level == EventApprovalLevel.Verifier)
		//				{
		//					await LogActivity(Modules.Event, "Verify Public Event: " + title, model);
		//					TempData["SuccessMessage"] = "Public Event titled " + title + " updated as Verified.";

		//					await SendNotification(pid, NotificationCategory.Event, NotificationType.Verify_Public_Event_After_Submit_For_Verification, title, refno, venue, "Verified and Pending Approval", model.approval.Status, model.approval.RequireNext);
		//					// dashboard
		//				}
		//				else
		//				{
		//					await LogActivity(Modules.Event, "Approve Public Event: " + title, model);
		//					TempData["SuccessMessage"] = "Public Event titled " + title + " updated as Approved.";

		//					if (model.approval.Level == EventApprovalLevel.Approver1)
		//					{
		//						if (model.approval.RequireNext)
		//						{
		//							await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Publication_Creation_1, title, refno, venue, "Approved by 1st-Level Approver and Pending 2nd-Level Approval", model.approval.Status, model.approval.RequireNext);
		//						}
		//						else
		//						{
		//							await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Publication_Creation_1, title, refno, venue, "Approved by 1st-Level Approver", model.approval.Status, model.approval.RequireNext);
		//						}
		//					}
		//					else if (model.approval.Level == EventApprovalLevel.Approver2)
		//					{
		//						if (model.approval.RequireNext)
		//						{
		//							await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Publication_Creation_2, title, refno, venue, "Approved by 2nd-Level Approver and Pending 3rd-Level Approval", model.approval.Status, model.approval.RequireNext);
		//						}
		//						else
		//						{
		//							await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Publication_Creation_2, title, refno, venue, "Approved by 2nd-Level Approver", model.approval.Status, model.approval.RequireNext);
		//						}
		//					}
		//					else if (model.approval.Level == EventApprovalLevel.Approver3)
		//					{
		//						await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Publication_Creation_3, title, refno, venue, "Approved by 3rd-Level Approver", model.approval.Status, model.approval.RequireNext);
		//					}
		//					// dashboard
		//				}
		//			}
		//			else
		//			{
		//				await LogActivity(Modules.Event, "Public Event Requires Amendment: " + title, model);
		//				TempData["SuccessMessage"] = "Public Event titled " + title + " updated as Requires Amendment.";


		//				if (model.approval.Level == EventApprovalLevel.Verifier)
		//				{
		//					await SendNotification(pid, NotificationCategory.Event, NotificationType.Verify_Public_Event_After_Submit_For_Verification, title, refno, venue, "Amendment Requested by Verifier", model.approval.Status, false);
		//				}
		//				else if (model.approval.Level == EventApprovalLevel.Approver1)
		//				{
		//					await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Public_Event_ByApprover_1, title, refno, venue, "Amendment Requested by 1st-Level Approver", model.approval.Status, false);
		//				}
		//				else if (model.approval.Level == EventApprovalLevel.Approver2)
		//				{
		//					await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Public_Event_ByApprover_2, title, refno, venue, "Amendment Requested by 2nd-Level Approver", model.approval.Status, false);
		//				}
		//				else if (model.approval.Level == EventApprovalLevel.Approver3)
		//				{
		//					await SendNotification(pid, NotificationCategory.Event, NotificationType.Approve_Public_Event_ByApprover_3, title, refno, venue, "Amendment Requested by 3rd-Level Approver", model.approval.Status, false);
		//				}
		//				// dashboard
		//			}

		//			return RedirectToAction("Index", "PublicEvent", new { area = "RnP" });
		//		}
		//		else
		//		{

		//			return RedirectToAction("List", "PublicEvent", new { area = "eEvent", @id = model.approval.EventId });
		//		}
		//	}

		//	//ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
		//	return View(model);
		//}

		//	private async Task<List<int>> GetNotificationReceivers(NotificationCategory ncat, NotificationType ntype, EventApprovalStatus status, bool forward)
		//	{
		//		List<int> result = new List<int> { };
		//		var response = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"eEvent/PublicEvent/GetNotificationReceivers/?cat={ncat}&type={ntype}&status={status}&forward={forward}");
		//		if (response.isSuccess)
		//		{
		//			result = response.Data;
		//		}
		//		else
		//		{
		//			await LogError(Modules.RnP, "Failed to get Auto Notification receivers");
		//		}
		//		return result;
		//	}

		//	private async Task<bool> SaveNotificationID(int id, int notification_id)
		//	{
		//		var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveNotificationID?id={id}&notificationid={notification_id}");
		//		if (!response.isSuccess)
		//		{
		//			await LogError(Modules.Event, "Failed to save Auto Notification ID (API Error)");
		//		}
		//		else
		//		{
		//			if (response.Data == false)
		//			{
		//				await LogError(Modules.Event, "Failed to save Auto Notification ID (Publication Error)");
		//			}
		//		}
		//		return response.isSuccess;
		//	}

		//	private async Task<bool> SendNotification(int id, NotificationCategory ncat, NotificationType ntype, string title, string code, string venue, string status, EventApprovalStatus appstatus, bool forward)
		//	{
		//		try
		//		{
		//			var receivers = await GetNotificationReceivers(ncat, ntype, appstatus, forward);
		//			if (receivers.Count > 0)
		//			{
		//				ParameterListToSend paramToSend = new ParameterListToSend();
		//				paramToSend.EventName = title;
		//				paramToSend.EventCode = code;
		//				paramToSend.EventLocation = venue;
		//				paramToSend.EventApproval = status;

		//				CreateAutoReminder reminder = new CreateAutoReminder
		//				{
		//					NotificationType = ntype,
		//					NotificationCategory = ncat,
		//					ParameterListToSend = paramToSend,
		//					StartNotificationDate = DateTime.Now,
		//					ReceiverId = receivers
		//					// new List<int> { 2, 3, 4, 5 }
		//				};
		//				try
		//				{
		//					var response = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
		//					if (response.isSuccess)
		//					{
		//						int saveThisID = response.Data.SLAReminderStatusId;
		//						//save saveThisID back into survey table
		//						var ressave = await SaveNotificationID(id, saveThisID);
		//						return true;
		//					}
		//					else
		//					{
		//						await LogError(Modules.Event, "Failed to generate Auto Notification (API Call Returned Failure)");
		//						return false;
		//					}
		//				}
		//				catch
		//				{
		//					await LogError(Modules.Event, "Failed to generate Auto Notification (API Call Failed)");
		//					return false;
		//				}
		//			}
		//			else
		//			{
		//				await LogError(Modules.Event, "Failed to generate Auto Notification (No Receivers Found)");
		//				return false;
		//			}
		//		}
		//		catch
		//		{
		//			await LogError(Modules.Event, "Failed to generate Auto Notification");
		//			return false;
		//		}
		//	}

		}
	}