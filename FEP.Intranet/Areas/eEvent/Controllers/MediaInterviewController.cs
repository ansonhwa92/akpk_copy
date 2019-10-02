using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
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
	//[LogError(Modules.   )]
	public class MediaInterviewController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEventMediaInterview/MediaInterview
		public ActionResult Index()
		{
			return View();
		}

		//public ActionResult List(FilterMediaInterviewModel filter)
		//{
		//	var media = db.EventMediaInterviewRequest.Where(i => i.Display)
		//		.Select(i => new DetailsMediaInterviewModel()
		//		{
		//			Id = i.Id,
		//			MediaName = i.MediaName,
		//			MediaType = i.MediaType,
		//			ContactPerson = i.ContactPerson,
		//			ContactNo = i.ContactNo,
		//			AddressStreet1 = i.AddressStreet1,
		//			AddressStreet2 = i.AddressStreet2,
		//			AddressPoscode = i.AddressPoscode,
		//			AddressCity = i.AddressCity,
		//			State = i.State,
		//			Email = i.Email,
		//			DateStart = i.DateStart,
		//			DateEnd = i.DateEnd,
		//			Time = i.Time,
		//			Language = i.Language,
		//			Topic = i.Topic,
		//			RepUserId = i.UserId,
		//			RepUserName = i.User.Name,
		//			RepDesignation = i.Designation,
		//			MediaStatus = i.MediaStatus,
		//		}).ToList();

		//	ListMediaInterviewModel model = new ListMediaInterviewModel(media);

		//	return View("List", model);
		//}

		public ActionResult List()
		{
			return View();
		}

		// GET: eEventMediaInterview/MediaInterview/Details/5
		public ActionResult Details(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DetailsMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Details", media);
		}

		// GET: eEventMediaInterview/MediaInterview/Create
		public ActionResult Create()
		{
			CreateMediaInterviewModel model = new CreateMediaInterviewModel() { };

			//var getuser = db.User.Where(c => c.Display && c.UserType == UserType.Staff)
			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(model);
		}

		// POST: eEventMediaInterview/MediaInterview/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateMediaInterviewModel model)
		{
            if (model.DateStart > model.DateEnd)
            {
                ModelState.AddModelError("DateEnd", "Start Date must less or equal to End Date");
            }

			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					Email = model.Email,
					DateStart = model.DateStart,
					DateEnd = model.DateEnd,
					Time = model.Time,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.RepUserId,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true,
					MediaStatus = MediaStatus.New
				};
				db.EventMediaInterviewRequest.Add(media);
				db.SaveChanges();

				//save refno public event
				var refno = "EVT/" + DateTime.Now.ToString("yyMM");
				refno += "/" + media.Id.ToString("D4");
				media.RefNo = refno;

				db.Entry(media).State = EntityState.Modified;
				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Media Interview Request successfully created.";
				return RedirectToAction("List");
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Edit/5
		public ActionResult Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new EditMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					origin = origin,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			media.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(media);
		}

		// POST: eEventMediaInterview/MediaInterview/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditMediaInterviewModel model)
		{
			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					Id = model.Id,
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					Email = model.Email,
					DateStart = model.DateStart,
					DateEnd = model.DateEnd,
					Time = model.Time,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.RepUserId,
				};

				db.Entry(media).State = EntityState.Modified;
				db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(media).Property(x => x.Display).IsModified = false;
				db.Configuration.ValidateOnSaveEnabled = true;


				string path = "FileUploaded/";
				if (model.DocumentMedia != null)
				{
					var getIdFile = db.MediaFile.Where(s => s.EventId == model.Id).FirstOrDefault();

					if (getIdFile != null)
					{
						db.MediaFile.Remove(getIdFile);
					}

					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentMedia.FileName,
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
				TempData["SuccessMessage"] = "Media Interview Request successfully updated.";
				if (model.origin == "fromlist")
				{
					return RedirectToAction("List");
				}
				else
				{
					return RedirectToAction("Details", new { area = "eEvent", id = model.Id });
				}
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
			.Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);
			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DeleteMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Delete", media);
		}

		// POST: eEventMediaInterview/MediaInterview/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteMediaInterviewModel model)
		{
			EventMediaInterviewRequest media = new EventMediaInterviewRequest() { Id = model.Id };
			MediaFile file = new MediaFile() { EventId = model.Id };
			media.Display = false;
			file.Display = false;

			db.EventMediaInterviewRequest.Attach(media);
			db.Entry(media).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();

			//LogActivity();
			TempData["SuccessMessage"] = "Media Interview Request successfully deleted.";
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









	}
}
