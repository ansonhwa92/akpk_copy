using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateExhibitionRoadshowRequestModel model)
		{

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreateExhibitionRoadshowRequestModel model, string Submittype)
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
				var modelapi = new CreateExhibitionRoadshowRequestModel
				{
					EventName = model.EventName,
					Organiser = model.Organiser,
					OrganiserEmail = model.OrganiserEmail,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					StartTime = model.StartTime,
					EndTime = model.EndTime,
					ExhibitionStatus = ExhibitionStatus.New,
					ParticipationRequirement = model.ParticipationRequirement,
					ReceivedById = model.ReceivedById,
					ReceivedDate = model.ReceivedDate,
					Receive_Via = model.Receive_Via,
					NomineeId = model.NomineeId,
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

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Exhibition/Roadshow Request", model);
					if (Submittype == "Save")
					{
						TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully added";
						return RedirectToAction("List");
					}
					else if (Submittype == "Submit")
					{
						TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully added";
						return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", id = response.Data });
					}
				}
			}

			TempData["ErrorMessage"] = "Fail to add new Exhibition/Roadshow Request";

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name");
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name");

			return RedirectToAction("List");
		}



		[HttpGet]
		public async Task<ActionResult> Edit(int? id)
		{

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var response = await WepApiMethod.SendApiAsync<EditExhibitionRoadshowRequestModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EditExhibitionRoadshowRequestModel()
			{
				EventName = response.Data.EventName,
				Organiser = response.Data.Organiser,
				OrganiserEmail = response.Data.OrganiserEmail,
				AddressStreet1 = response.Data.AddressStreet1,
				AddressStreet2 = response.Data.AddressStreet2,
				AddressPoscode = response.Data.AddressPoscode,
				AddressCity = response.Data.AddressCity,
				State = response.Data.State,
				StartDate = response.Data.StartDate,
				EndDate = response.Data.EndDate,
				StartTime = response.Data.StartTime,
				EndTime = response.Data.EndTime,
				ExhibitionStatus = response.Data.ExhibitionStatus,
				ParticipationRequirement = response.Data.ParticipationRequirement,
				ReceivedById = response.Data.ReceivedById,
				ReceivedDate = response.Data.ReceivedDate,
				Receive_Via = response.Data.Receive_Via,
				Attachments = response.Data.Attachments,
				
			};

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(FEP.Intranet.Areas.eEvent.Models.EditExhibitionRoadshowRequestModel model)
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
				var modelapi = new EditExhibitionRoadshowRequestModel
				{
					Id = model.Id,
					EventName = model.EventName,
					Organiser = model.Organiser,
					OrganiserEmail = model.OrganiserEmail,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					StartTime = model.StartTime,
					EndTime = model.EndTime,
					ExhibitionStatus = model.ExhibitionStatus,
					ParticipationRequirement = model.ParticipationRequirement,
					ReceivedById = model.ReceivedById,
					ReceivedDate = model.ReceivedDate,
					Receive_Via = model.Receive_Via,
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

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/ExhibitionRoadshowRequest?id={model.Id}", modelapi);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully updated";

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = "Fail to update Exhibition/Roadshow Request";

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);

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

		public async Task<ActionResult> SubmitToVerify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SubmitToVerify?id={id}");
			if (response.isSuccess)
			{
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.RefNo;
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Submit_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_Exhibition_RoadShow_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};

					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Submit Exhibition Roadshow Ref No: " + response.Data.RefNo + " for verification.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully submitted for verification.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to submit Exhibition Roadshow for verification.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}



		public async Task<ActionResult> Verified(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/Verified?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Verify_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verify_Exhibition_RoadShow_After_Submit_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;

					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is verified.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully verified and submitted for approval.";
				}
				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Exhibition Roadshow.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> FirstApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/FirstApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver1_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_1,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is approved on first level.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}
				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Exhibition Roadshow.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> SecondApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SecondApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver2_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_2,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is approved on first level.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully approved and submitted to next approval.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Exhibition Roadshow.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> FinalApproved(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/FinalApproved?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver3_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_3,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is approved");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully approved.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Exhibition Roadshow.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}

		public async Task<ActionResult> Reject(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/Reject?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Reject_Exhibition_RoadShow,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is rejected and require amendment.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully rejected and require amendment.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Exhibition Roadshow.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}

		//===============================================================================================

		public async Task<ActionResult> SubmitDutyRoster(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SubmitDutyRoster?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Submit_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_DutyRoster_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is submitted Duty Roster for verification.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully submitted Duty Roster for verification.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to submit Duty Roster for verification.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}



		public async Task<ActionResult> VerifiedDutyRoster(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/VerifiedDutyRoster?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Verify_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verify_DutyRoster,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is verified Duty Roster.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully verified Duty Roster.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to verify Duty Roster.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> RejectDutyRoster(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/RejectDutyRoster?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Reject_Exhibition_RoadShow,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is rejected Duty Roster.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully rejected Duty Roster.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to reject Duty Roster.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}



		public async Task<ActionResult> ApproveDutyRoster(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/ApproveDutyRoster?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Approver3_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_DutyRoster,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is approved Duty Roster.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully approved Duty Roster.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Duty Roster.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}

		//=======================================================================================================================

		public async Task<ActionResult> AcceptParticipation(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/AcceptParticipation?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.AcceptParticipation_Exhibition_RoadShow,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is accepted participation.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully accepted participation.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to accept participation.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}



		public async Task<ActionResult> DeclineParticipation(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<ExhibitionRoadshowRequestModel>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/Reject?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/GetSLAId?id={id}");
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
				paramToSend.EventName = response.Data.EventName;
				paramToSend.EventApproval = response.Data.ExhibitionStatus.GetDisplayName();

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Recipient_Reject_ExhibitionRoadShow}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.DeclineParticipation_Exhibition_RoadShow,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					int saveThisID = response2.Data.SLAReminderStatusId;

					//save saveThisID dalam table 
					response.Data.SLAReminderStatusId = saveThisID;
					var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
					if (response3.isSuccess) { }

					await LogActivity(Modules.Event, "Exhibition Roadshow Ref No: " + response.Data.RefNo + " is declined participation.");
					TempData["SuccessMessage"] = "Exhibition Roadshow Ref No: " + response.Data.RefNo + ", successfully declined participation.";
				}

				return RedirectToAction("List", "ExhibitionRoadshowRequest", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to decline participation.";
				return RedirectToAction("Details", "ExhibitionRoadshowRequest", new { area = "eEvent", @id = id });
			}
		}
	}
}