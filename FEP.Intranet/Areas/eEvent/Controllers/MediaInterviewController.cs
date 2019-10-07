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
					RefNo = i.RefNo,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					MediaStatus = i.MediaStatus,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					CreatedBy = i.CreatedBy,
					CreatedByName = i.User.Name,
					CreatedDate = i.CreatedDate,
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

			var getuser = db.User.Where(c => c.Display && c.UserType == UserType.Staff)
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
				ModelState.AddModelError("DateEnd", "End Date must greater or equal than Start Date");
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
					CreatedBy = CurrentUser.UserId,
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
					MediaStatus = i.MediaStatus,
					RefNo = i.RefNo,
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
			if (model.DateStart > model.DateEnd)
			{
				ModelState.AddModelError("DateEnd", "End Date must greater or equal than Start Date");
			}

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
					MediaStatus = model.MediaStatus,
					RefNo = model.RefNo
				};

				db.Entry(media).State = EntityState.Modified;
				db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(media).Property(x => x.Display).IsModified = false;
				db.Configuration.ValidateOnSaveEnabled = true;

				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Media Interview Request successfully updated.";
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


		// Submit for Verification
		public async Task<ActionResult> SubmitToVerify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/SubmitToVerify?id={id}");
			if (response.isSuccess)
			{
				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault(); // will change webapi

				//var getmediaresponse = await WepApiMethod.SendApiAsync<DetailsMediaInterviewModel>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest?id={id}");

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Pending Verification";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Verifier}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_Verify_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};

					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						//save saveThisID dalam table public event
						if (getmedia != null)
						{
							getmedia.SLAReminderStatusId = saveThisID;
							db.EventMediaInterviewRequest.Attach(getmedia);
							db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
							db.Configuration.ValidateOnSaveEnabled = false;
							db.SaveChanges();
						}
					}
				}

				await LogActivity(Modules.Event, "Submit Media Interview Ref No: " + response.Data + " for verification.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully submitted for verification.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else

			{
				TempData["ErrorMessage"] = "Failed to submit Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		//Verified
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> Verified(int? id, MediaInterviewApprovalModel model)
		public async Task<ActionResult> Verified(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			//var actionlogresponse = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/Evaluate", model);
			//if (actionlogresponse.isSuccess)
			//{

			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/Verified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Verified";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Approver1}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verified_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					if (getmedia != null)
					{
						getmedia.SLAReminderStatusId = saveThisID;
						db.EventMediaInterviewRequest.Attach(getmedia);
						db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
						db.Configuration.ValidateOnSaveEnabled = false;
						db.SaveChanges();
					}
				}

				await LogActivity(Modules.Event, "Media Interview Ref No: " + response.Data + " is verified.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully verified.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}

			//	TempData["ErrorMessage"] = "Failed to verified Media Interview.";
			//	return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			//}

			else
			{
				TempData["ErrorMessage"] = "Failed to verified Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		//First Approved 
		public async Task<ActionResult> FirstApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/FirstApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Pending Approval";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Approver2}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve1_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					if (getmedia != null)
					{
						getmedia.SLAReminderStatusId = saveThisID;
						db.EventMediaInterviewRequest.Attach(getmedia);
						db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
						db.Configuration.ValidateOnSaveEnabled = false;
						db.SaveChanges();
					}
				}

				await LogActivity(Modules.Event, "Media Interview Ref No: " + response.Data + " is approved on first level.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully approved and submitted to next approval.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		//Second Approved Public Event 
		public async Task<ActionResult> SecondApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/SecondApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Pending Approval";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Approver3}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve2_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					if (getmedia != null)
					{
						getmedia.SLAReminderStatusId = saveThisID;
						db.EventMediaInterviewRequest.Attach(getmedia);
						db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
						db.Configuration.ValidateOnSaveEnabled = false;
						db.SaveChanges();
					}
				}

				await LogActivity(Modules.Event, "Media Interview Ref No: " + response.Data + " is approved on first level.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully approved and submitted to next approval.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		//Final Approved 
		public async Task<ActionResult> FinalApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/FinalApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Approved";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Approver3}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve3_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					if (getmedia != null)
					{
						getmedia.SLAReminderStatusId = saveThisID;
						db.EventMediaInterviewRequest.Attach(getmedia);
						db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
						db.Configuration.ValidateOnSaveEnabled = false;
						db.SaveChanges();
					}
				}

				await LogActivity(Modules.Event, "Media Interview Ref No: " + response.Data + " is approved on first level.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully approved and submitted to next approval.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		//Reject and Require amendment
		public async Task<ActionResult> Reject(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/RejectVerified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var getSLAId = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				int SLAReminderStatusId = getSLAId.SLAReminderStatusId.Value;
				var response3 = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
					(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");

				List<BulkNotificationModel> myNotification = response3.Data;
				//myNotification[0].NotificationId;

				//--------------------------------------------------Send Email---------------------------------------------//

				var getmedia = db.EventMediaInterviewRequest.Where(e => e.Id == id).FirstOrDefault();

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data;
				paramToSend.EventName = getmedia.MediaName;
				paramToSend.EventApproval = "Approved";

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.MediaInterview_Verifier}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Reject_Verify_External_Request_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table public event

					if (getmedia != null)
					{
						getmedia.SLAReminderStatusId = saveThisID;
						db.EventMediaInterviewRequest.Attach(getmedia);
						db.Entry(getmedia).Property(m => m.SLAReminderStatusId).IsModified = true;
						db.Configuration.ValidateOnSaveEnabled = false;
						db.SaveChanges();
					}
				}

				await LogActivity(Modules.Event, "Media Interview Ref No: " + response.Data + " is rejected.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data + ", successfully rejected and require amendment.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

	}
}
