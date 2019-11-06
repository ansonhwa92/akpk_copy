using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.MediaInterview;
using FEP.WebApiModel.Administration;
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
		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsMediaInterviewRequestApiModel>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest?id={id}");
			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsMediaInterviewModel()
			{
				Id = response.Data.Id,
				MediaName = response.Data.MediaName,
				MediaType = response.Data.MediaType,
				ContactPerson = response.Data.ContactPerson,
				ContactNo = response.Data.ContactNo,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				Email = response.Data.Email,
				DateStart = response.Data.DateStart,
				DateEnd = response.Data.DateEnd,
				Time = response.Data.Time,
				Language = response.Data.Language,
				Topic = response.Data.Topic,
				RepUserId = response.Data.UserId,
				RepUserName = response.Data.RepUserName,
				MediaStatus = response.Data.MediaStatus,
				RefNo = response.Data.RefNo,
				RepEmail = response.Data.RepEmail,
				RepMobileNumber = response.Data.RepMobileNumber,
				RepDesignation = response.Data.RepDesignation,
				CreatedByName = response.Data.CreatedByName,
				CreatedDate = response.Data.CreatedDate,
				Attachments = response.Data.Attachments,
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name", 0);

			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Create
		[HttpGet]
		public async Task<ActionResult> Create()
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.CreateMediaInterviewModel() { };

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name", 0);

			return View(model);
		}

		// POST: eEventMediaInterview/MediaInterview/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateMediaInterviewModel model, string Submittype)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (model.DateStart > model.DateEnd)
			{
				ModelState.AddModelError("DateEnd", "End Date must greater or equal than Start Date");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new FEP.WebApiModel.MediaInterview.CreateMediaInterviewRequestApiModel
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

				//attachment
				if (model.AttachmentFiles.Count() > 0)
				{
					var files = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

					if (files != null)
					{
						modelapi.FilesId = files.Select(f => f.Id).ToList();
					}
				}

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Media Interview", model);
					if (Submittype == "Save")
					{
						TempData["SuccessMessage"] = "Media Interview successfully created";
						return RedirectToAction("List");
					}
					else if (Submittype == "Submit")
					{
						return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", id = response.Data });
					}

				}

			}

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name");

			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Edit/5
		public async Task<ActionResult> Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var response = await WepApiMethod.SendApiAsync<EditMediaInterviewRequestApiModel>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EditMediaInterviewModel()
			{
				MediaName = response.Data.MediaName,
				MediaType = response.Data.MediaType,
				ContactPerson = response.Data.ContactPerson,
				ContactNo = response.Data.ContactNo,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				Email = response.Data.Email,
				DateStart = response.Data.DateStart,
				DateEnd = response.Data.DateEnd,
				Time = response.Data.Time,
				Language = response.Data.Language,
				Topic = response.Data.Topic,
				RepUserId = response.Data.UserId,
				RepUserName = response.Data.RepUserName,
				MediaStatus = response.Data.MediaStatus,
				RefNo = response.Data.RefNo,
				RepEmail = response.Data.RepEmail,
				RepMobileNumber = response.Data.RepMobileNumber,
				Attachments = response.Data.Attachments
			};

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name");

			return View(model);
		}

		// POST: eEventMediaInterview/MediaInterview/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(FEP.Intranet.Areas.eEvent.Models.EditMediaInterviewModel model)
		{
			if (model.DateStart > model.DateEnd)
			{
				ModelState.AddModelError("DateEnd", "End Date must greater or equal than Start Date");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EditMediaInterviewRequestApiModel
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
					MediaStatus = model.MediaStatus,
					RefNo = model.RefNo,
					Attachments = model.Attachments
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

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/MediaInterviewRequest?id={model.Id}", modelapi);

				if (response.isSuccess)
				{

					await LogActivity(Modules.Event, "Edit Media Interview", model);
					TempData["SuccessMessage"] = "Media Interview successfully updated";

					return RedirectToAction("List");
				}
			}

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name");

			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<FEP.WebApiModel.MediaInterview.DetailsMediaInterviewRequestApiModel>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.DetailsMediaInterviewModel()
			{
				MediaName = response.Data.MediaName,
				MediaType = response.Data.MediaType,
				ContactPerson = response.Data.ContactPerson,
				ContactNo = response.Data.ContactNo,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				Email = response.Data.Email,
				DateStart = response.Data.DateStart,
				DateEnd = response.Data.DateEnd,
				Time = response.Data.Time,
				Language = response.Data.Language,
				Topic = response.Data.Topic,
				RepUserId = response.Data.UserId,
				RepUserName = response.Data.RepUserName,
				RepEmail = response.Data.RepEmail,
				RepMobileNumber = response.Data.RepMobileNumber,
				Attachments = response.Data.Attachments,
				MediaStatus = response.Data.MediaStatus,
				RefNo = response.Data.RefNo,
				RepDesignation = response.Data.RepDesignation,

			};

			if (model == null)
			{
				return HttpNotFound();
			}

			model.RepresentativeList = new SelectList(await GetUser(), "Id", "Name");

			return View(model);
		}

		// POST: eEventMediaInterview/MediaInterview/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirm(int id)
		{
			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/MediaInterviewRequest?id={id}");

			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, "Delete Media Interview");
				TempData["SuccessMessage"] = "Media Interview successfully deleted";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			TempData["ErrorMessage"] = "Fail to delete Media Interview";
			return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
		}

		[NonAction]
		private async Task<IEnumerable<UserModel>> GetUser()
		{
			var speaker = Enumerable.Empty<UserModel>();

			var response = await WepApiMethod.SendApiAsync<List<UserModel>>(HttpVerbs.Get, $"Administration/User");

			if (response.isSuccess)
			{
				speaker = response.Data.OrderBy(o => o.Name);
			}
			return speaker;
		}


		// Submit for Verification
		public async Task<ActionResult> SubmitToVerify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/SubmitToVerify?id={id}");
			if (response.isSuccess)
			{

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();
				paramToSend.EventLocation = response.Data.Location;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Submit_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_Media_Interview_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};

					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Submit Media Interview Ref No: " + response.Data.RefNo + " for verification.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully submitted for verification.";
				}
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to submit Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> Verified(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/Verified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Verify_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verify_Media_Interview_After_Submit_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + " is verified.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully verified and submitted for approval.";
				}
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
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
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/FirstApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver1_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Media_Interview_ByApprover_1,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + " is approved on first level.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}

		public async Task<ActionResult> SecondApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/SecondApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver2_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Media_Interview_ByApprover_2,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + " is approved on second level.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}
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
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/FinalApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver3_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Media_Interview_ByApprover_3,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + " for approved.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully approved.";
				}
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
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/RejectVerified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/MediaInterviewRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.MediaName;
				paramToSend.EventApproval = response.Data.MediaStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_MediaInterview}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Reject_Media_Interview,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/MediaInterviewRequestRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + " is rejected and require amendment.");
					TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully rejected and require amendment.";
				}
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> RepAvailable(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/RepAvailable?id={id}");
			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + ", Representative is available.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully updated.";
				return RedirectToAction("List", "MediaInterview", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Media Interview.";
				return RedirectToAction("Details", "MediaInterview", new { area = "eEvent", @id = id });
			}
		}




		public async Task<ActionResult> RepNotAvailable(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<MediaInterviewRequestApiModel>(HttpVerbs.Post, $"eEvent/MediaInterviewRequest/RepNotAvailable?id={id}");
			if (response.isSuccess)
			{
				await LogActivity(Modules.Event, " Media Interview Ref No: " + response.Data.RefNo + ", Representative is not available.");
				TempData["SuccessMessage"] = "Media Interview Ref No: " + response.Data.RefNo + ", successfully updated.";
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
