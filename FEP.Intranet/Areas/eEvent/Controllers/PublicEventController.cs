using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using FEP.WebApiModel.eEvent;
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
		public ActionResult Details(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DetailsPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					EventStatus = i.EventStatus,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,
					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,
					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					origin = origin,
					RefNo = i.RefNo,
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		//GET: PublicEvent/Create
		public ActionResult Create(int? ctgryId)
		{
			CreatePublicEventModel model = new CreatePublicEventModel
			{
				EventStatus = EventStatus.New,
				EventCategoryId = ctgryId,
			};

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
		}

		//[HttpGet]
		//public async Task<ActionResult> Create(int? ctgryId)
		//{
		//	var model = new FEP.Intranet.Areas.eEvent.Models.CreatePublicEventModel()
		//	{
		//		EventStatus = EventStatus.New,
		//		EventCategoryId = ctgryId,
		//	};

		//	model.CategoryList = new SelectList(await GetCategory(), "Id", "Name", 0);
		//	model.SpeakerList = new SelectList(GetSpeaker(), "Id", "Name", 0);
		//	model.ExternalExhibitorList = new SelectList(GetExhibitor(), "Id", "Name", 0);

		//	return View(model);
		//}


		// POST: PublicEvent/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreatePublicEventModel model)
		{
			if (ModelState.IsValid)
			{
				PublicEvent x = new PublicEvent
				{
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,

					EventStatus = EventStatus.New,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					Reasons = model.Reasons,
					Remarks = model.Remarks,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,
					Display = true,
					ExternalExhibitorId = model.ExternalExhibitorId,
				};
				db.PublicEvent.Add(x);

				string path = "FileUploaded/";
				if (model.DocumentEvent != null)
				{
					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentEvent.FileName,
						FilePath = path,
						UploadedDate = DateTime.Now,
						Display = true,
						CreatedBy = CurrentUser.UserId,
						Category = FileCategory.NewFile,
						EventId = x.Id,
					};

					db.EventFile.Add(eventfile);
				};
				db.SaveChanges();

				//save refno public event
				var refno = "EVP/" + DateTime.Now.ToString("yyMM");
				refno += "/" + x.Id.ToString("D4");
				x.RefNo = refno;

				db.Entry(x).State = EntityState.Modified;
				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Public Event successfully created.";
				return RedirectToAction("List");
			}


			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
		}

		// GET: PublicEvent/Edit/5
		public ActionResult Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new EditPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,
					origin = origin,
					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,
					RefNo = i.RefNo,
					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					//GetFileName = eventfile.FileName

				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			e.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			e.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			e.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(e);
		}

		// POST: PublicEvent/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditPublicEventModel model)
		{
			if (ModelState.IsValid)
			{
				PublicEvent eEvent = new PublicEvent
				{
					Id = model.Id,
					//EventTitle = (model.EventTitle != null) ? model.EventTitle.ToUpper() : model.EventTitle,
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
					Reasons = model.Reasons,
					Remarks = model.Remarks,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
					RefNo = model.RefNo,
					
				};

				db.Entry(eEvent).State = EntityState.Modified;
				db.Entry(eEvent).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(eEvent).Property(x => x.Display).IsModified = false;
				db.Configuration.ValidateOnSaveEnabled = true;

				string path = "FileUploaded/";
				if (model.DocumentEvent != null)
				{
					var getIdFile = db.EventFile.Where(s => s.EventId == model.Id).FirstOrDefault();

					if (getIdFile != null)
					{
						db.EventFile.Remove(getIdFile);
					}

					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentEvent.FileName,
						FilePath = path,
						UploadedDate = DateTime.Now,
						Display = true,
						CreatedBy = CurrentUser.UserId,
						Category = FileCategory.NewFile,
						EventId = model.Id,
						Id = getIdFile.Id
					};

					db.EventFile.Add(eventfile);
				};
				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Public Event successfully updated.";

				if (model.origin == "fromlist")
				{
					return RedirectToAction("List");
				}
				else if (model.origin == "amendment")
				{
					return RedirectToAction("Details", new { area = "eEvent", id = model.Id , origin = model.origin});
				}
				else
				{
					return RedirectToAction("Details", new { area = "eEvent", id = model.Id });
				}
			}

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
		}

		// GET: PublicEvent/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DeletePublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ApprovalId1 = i.ApprovalId1,
					ApprovalId2 = i.ApprovalId2,
					ApprovalId3 = i.ApprovalId3,
					ApprovalId4 = i.ApprovalId4,

					ApprovalName1 = i.Approval1.User.Name,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalName4 = i.Approval4.User.Name,

					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,

					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,

					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					//GetFileName = eventfile.FileName
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		// POST: PublicEvent/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeletePublicEventModel model)
		{
			PublicEvent eEvent = new PublicEvent() { Id = model.Id };
			eEvent.Display = false;

			db.PublicEvent.Attach(eEvent);
			db.Entry(eEvent).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();

			//LogActivity();
			TempData["SuccessMessage"] = "Public Event successfully deleted.";
			return RedirectToAction("List");
		}

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

				await LogActivity(Modules.Event, "Submit Public Event Ref No: " + response.Data +" for verification.");
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
					ReceiverId = new List<int> { 2,3,4,5 }
					//ReceiverId = 
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
					NotificationType = NotificationType.Approve_Public_Event_Creation,
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
					NotificationType = NotificationType.Approve_Public_Event_Creation,
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
					NotificationType = NotificationType.Approve_Public_Event_Creation,
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

		//[NonAction]
		//private async Task<IEnumerable<EventSpeakerModel>> GetSpeaker()
		//{

		//	var roles = Enumerable.Empty<EventSpeakerModel>();

		//	var response = await WepApiMethod.SendApiAsync<List<EventSpeakerModel>>(HttpVerbs.Get, $"Administration/User");

		//	if (response.isSuccess)
		//	{
		//		roles = response.Data.OrderBy(o => o.Name);
		//	}
		//	return roles;
		//}


		//[NonAction]
		//private async Task<IEnumerable<EventExternalExhibitorModel>> GetExhibitor()
		//{
		//	var roles = Enumerable.Empty<EventExternalExhibitorModel>();

		//	var response = await WepApiMethod.SendApiAsync<List<EventExternalExhibitorModel>>(HttpVerbs.Get, $"eEvent/EventExternalExhibitor");

		//	if (response.isSuccess)
		//	{
		//		roles = response.Data.OrderBy(o => o.Name);
		//	}

		//	return roles;

		//}


	}
}