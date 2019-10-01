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

		//public ActionResult List(FilterPublicEventModel filter)
		//{
		//	var e = db.PublicEvent.Where(i => i.Display && (filter.EventTitle == filter.EventTitle))
		//		.Select(i => new DetailsPublicEventModel()
		//		{
		//			Id = i.Id,
		//			EventTitle = i.EventTitle,
		//			EventObjective = i.EventObjective,
		//			EventCategoryId = i.EventCategoryId,
		//			EventCategoryName = i.EventCategory.CategoryName,
		//			StartDate = i.StartDate,
		//			EndDate = i.EndDate,
		//			Venue = i.Venue,
		//			Fee = i.Fee,
		//			EventStatus = i.EventStatus
		//		}).ToList();
		//	ListPublicEventModel model = new ListPublicEventModel(e);
		//	return View(model);
		//}

		public ActionResult List()
		{
			return View();
		}

		// GET: PublicEvent/Details/5
		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "Name", 0);
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
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreatePublicEventModel model)
		{
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
					Remarks = model.Remarks,
				};

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/PublicEvent", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Public Event", model);

					TempData["SuccessMessage"] = "Public Event successfully created";

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = "Fail to add new Public Event";
			return RedirectToAction("List");
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
				//GetFileName = response.Data.GetFileName
			};

			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}

		// POST: PublicEvent/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditPublicEventModel model)
		{
			if (ModelState.IsValid)
			{

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventSpeaker?id={model.Id}", model);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Edit Public Event", model);
					TempData["SuccessMessage"] = "Event Speaker successfully updated";

					return RedirectToAction("List");
				}
			}
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			TempData["ErrorMessage"] = "Fail to update Event Speaker";

			return RedirectToAction("List");
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
				//GetFileName = eventfile.FileName
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{
			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventSpeaker?id={id}");

			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, "Delete Public Event");
				TempData["SuccessMessage"] = "Public Event successfully deleted";
				return RedirectToAction("List", "EventSpeaker", new { area = "eEvent" });
			}
			TempData["ErrorMessage"] = "Fail to delete Public Event";
			return RedirectToAction("List", "EventSpeaker", new { area = "eEvent" });
		}

		//// POST: PublicEvent/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(DeletePublicEventModel model)
		//{
		//	PublicEvent eEvent = new PublicEvent() { Id = model.Id };
		//	eEvent.Display = false;
		//	db.PublicEvent.Attach(eEvent);
		//	db.Entry(eEvent).Property(m => m.Display).IsModified = true;
		//	db.Configuration.ValidateOnSaveEnabled = false;
		//	db.SaveChanges();
		//	TempData["SuccessMessage"] = "Public Event successfully deleted.";
		//	return RedirectToAction("List");
		//}


		// Submit Public Event for Verification
		public async Task<ActionResult> SubmitToVerify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SubmitToVerify?id={id}");
			if (response.isSuccess)
			{
				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				await LogActivity(Modules.Event, "Submit Public Event Ref No: " + response.Data + " for verification.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully submitted for verification.";

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Pending Approval";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Verify_Public_Event_Creation,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 },
				};

				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else

			{
				TempData["ErrorMessage"] = "Failed to submit Public Event.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/FirstApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Pending Approval";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Approve_Public_Event_Creation1,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 }
				};
				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}

				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is approved on first level.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully approved and submitted to next approval.";
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Public Event.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SecondApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Pending Approval";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Approve_Public_Event_Creation2,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 }
				};
				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}

				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is approved on first level.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully approved and submitted to next approval.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/FinalApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Approved";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Approve_Public_Event_Creation3,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 }
				};
				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}

				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is approved");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully approved.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/RejectPublicEvent?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Require Amendment";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Approve_Public_Event_Published_Changed,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 }
				};
				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}


				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is rejected and require amendment.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully rejected and require amendment.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/CancelPublicEvent?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getevent = db.PublicEvent.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getevent.EventTitle;
				paramToSend.EventApproval = "Cancelled";

				CreateAutoReminder reminder = new CreateAutoReminder
				{
					NotificationType = NotificationType.Approve_Public_Event_Published_Cancelled,
					NotificationCategory = NotificationCategory.Event,
					ParameterListToSend = paramToSend,
					StartNotificationDate = DateTime.Now,
					ReceiverId = new List<int> { 2, 3, 4, 5 }
				};
				var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
				int saveThisID = response2.Data.SLAReminderStatusId;

				//save saveThisID dalam table public event

				if (getevent != null)
				{
					getevent.SLAReminderStatusId = saveThisID;
					db.PublicEvent.Attach(getevent);
					db.Entry(getevent).Property(m => m.SLAReminderStatusId).IsModified = true;
					db.Configuration.ValidateOnSaveEnabled = false;
					db.SaveChanges();
				}


				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is cancelled.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully cancelled.";
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
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/PublishedPublicEvent?id={id}");
			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, "Public Event Ref No: " + response.Data + " is Published.");
				TempData["SuccessMessage"] = "Public Event Ref No: " + response.Data + ", successfully Published.";
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to publish Public Event.";
				return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		//[NonAction]
		//private async Task<IEnumerable<UserModel>> GetUsers()
		//{

		//	var roles = Enumerable.Empty<UserModel>();

		//	var response = await WepApiMethod.SendApiAsync<List<UserModel>>(HttpVerbs.Get, $"Administration/User");

		//	if (response.isSuccess)
		//	{
		//		roles = response.Data.OrderBy(o => o.Name);
		//	}

		//	return roles;

		//}

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


	}
}