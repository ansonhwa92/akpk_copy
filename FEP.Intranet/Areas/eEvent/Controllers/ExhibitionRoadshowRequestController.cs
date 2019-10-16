using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task<ActionResult> Create()
		{
			var model = new CreateExhibitionRoadshowRequestModel();

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(CreateExhibitionRoadshowRequestModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/ExhibitionRoadshowRequest", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully added";

					return RedirectToAction("List");
				}
			}
			TempData["ErrorMessage"] = "Fail to add new Exhibition/Roadshow Request";

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<ActionResult> Edit(int? id)
		{

			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<EditExhibitionRoadshowRequestModel>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = response.Data;

			model.ReceivedBys = new SelectList(await GetUsers(), "Id", "Name", 0);
			model.Nominees = new SelectList(await GetUsers(), "Id", "Name", 0);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditExhibitionRoadshowRequestModel model)
		{

			//var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest/IsEventNameExist?id={model.Id}&name={model.EventName}");

			//if (nameResponse.isSuccess)
			//{
			//	TempData["ErrorMessage"] = "Event Name already exist in the system";
			//	return RedirectToAction("List");
			//}

			if (ModelState.IsValid)
			{

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/ExhibitionRoadshowRequest?id={model.Id}", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = "Exhibition/Roadshow Request successfully updated";

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = "Fail to update Exhibition/Roadshow Request";

			return RedirectToAction("List");

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

















	}
}