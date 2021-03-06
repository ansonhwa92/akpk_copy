﻿using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.PublicEvent;
using FEP.WebApiModel.SLAReminder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApprovalModel = FEP.WebApiModel.PublicEvent.ApprovalModel;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class PublicEventController : FEPController
	{
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

		[HttpPost]
		public async Task<ActionResult> List(FilterPublicEventModel filter)
		{
			var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"eEvent/PublicEvent/GetEventList", filter);

			return Content(JsonConvert.SerializeObject(response.Data), "application/json");
		}

		// GET: PublicEvent/Details/Id
		[HttpGet]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<PublicEventApprovalModel>(HttpVerbs.Get, $"eEvent/PublicEvent?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var pubapproval = response.Data;

			var publicevent = new PublicEventModel()
			{
				Id = pubapproval.publicevent.Id,
				EventTitle = pubapproval.publicevent.EventTitle,
				EventObjective = pubapproval.publicevent.EventObjective,
				StartDate = pubapproval.publicevent.StartDate,
				EndDate = pubapproval.publicevent.EndDate,
				Venue = pubapproval.publicevent.Venue,

				FreeIndividual = pubapproval.publicevent.FreeIndividual,
				FreeIndividualPaper = pubapproval.publicevent.FreeIndividualPaper,
				FreeIndividualPresent = pubapproval.publicevent.FreeIndividualPresent,
				FreeAgency = pubapproval.publicevent.FreeAgency,

				IndividualFee = pubapproval.publicevent.IndividualFee,
				IndividualPaperFee = pubapproval.publicevent.IndividualPaperFee,
				IndividualPresentFee = pubapproval.publicevent.IndividualPresentFee,
				AgencyFee = pubapproval.publicevent.AgencyFee,

				IndividualEarlyBird = pubapproval.publicevent.IndividualEarlyBird,
				IndividualPaperEarlyBird = pubapproval.publicevent.IndividualPaperEarlyBird,
				IndividualPresentEarlyBird = pubapproval.publicevent.IndividualPresentEarlyBird,
				AgencyEarlyBird = pubapproval.publicevent.AgencyEarlyBird,

				//Fee = pubapproval.publicevent.Fee,
				SeatAllocated_EarlyBird = pubapproval.publicevent.SeatAllocated_EarlyBird,
				ParticipantAllowed = pubapproval.publicevent.ParticipantAllowed,
				TargetedGroup = pubapproval.publicevent.TargetedGroup,
				EventStatus = pubapproval.publicevent.EventStatus,
				EventCategoryId = pubapproval.publicevent.EventCategoryId,
				EventCategoryName = pubapproval.publicevent.EventCategoryName,
				Remarks = pubapproval.publicevent.Remarks,
				origin = pubapproval.publicevent.origin,
				RefNo = pubapproval.publicevent.RefNo,
				Attachments = pubapproval.publicevent.Attachments,
				SpeakerId = pubapproval.publicevent.SpeakerId,
				ExternalExhibitorId = pubapproval.publicevent.ExternalExhibitorId,
				CreatedByName = pubapproval.publicevent.CreatedByName,
				CreatedDate = pubapproval.publicevent.CreatedDate,

                tentativeScript = pubapproval.publicevent.tentativeScript
            };

			var approval = new ApprovalModel
			{
				Id = pubapproval.approval.Id,
				EventId = pubapproval.approval.EventId,
				Level = pubapproval.approval.Level,
				ApproverId = pubapproval.approval.ApproverId,
				Status = pubapproval.approval.Status,
				Remarks = pubapproval.approval.Remarks,
				RequireNext = pubapproval.approval.RequireNext
			};

			var pevaluation = new PublicEventApprovalModel
			{
				publicevent = publicevent,
				approval = approval
			};

			var responseHistory = await WepApiMethod.SendApiAsync<IEnumerable<PublicEventApprovalHistoryModel>>(HttpVerbs.Get, $"eEvent/PublicEvent/GetHistory?id={id}");

			if (responseHistory.isSuccess)
			{
				ViewBag.History = responseHistory.Data;
			}

			pevaluation.publicevent.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			pevaluation.publicevent.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName");
			pevaluation.publicevent.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name");

			return View(pevaluation);
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
		public async Task<ActionResult> Create(FEP.Intranet.Areas.eEvent.Models.CreatePublicEventModel model, string SubmittypeX)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (model.StartDate > model.EndDate)
			{
				ModelState.AddModelError("EndDate", "End Date must greater or equal than Start Date");
			}

			if (model.SeatAllocated_EarlyBird > model.ParticipantAllowed) {

				ModelState.AddModelError("SeatAllocated_EarlyBird", "Seat Allocated must be lower than Participant Allowed");
			}

			if (model.FreeIndividual) { }

			if (ModelState.IsValid)
			{
				var modelapi = new CreatePublicEventModel
				{
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,

					FreeIndividual = model.FreeIndividual,
					FreeIndividualPaper = model.FreeIndividualPaper,
					FreeIndividualPresent = model.FreeIndividualPresent,
					FreeAgency = model.FreeAgency,

					IndividualFee = model.IndividualFee,
					IndividualPaperFee = model.IndividualPaperFee,
					IndividualPresentFee = model.IndividualPresentFee,
					AgencyFee = model.AgencyFee,

					IndividualEarlyBird = model.IndividualEarlyBird,
					IndividualPaperEarlyBird = model.IndividualPaperEarlyBird,
					IndividualPresentEarlyBird = model.IndividualPresentEarlyBird,
					AgencyEarlyBird = model.AgencyEarlyBird,
					//Fee = model.Fee,
					SeatAllocated_EarlyBird = model.SeatAllocated_EarlyBird,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					EventStatus = model.EventStatus,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
					Remarks = model.Remarks,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,

                    tentativeScript = model.TentativeScript
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
					if (SubmittypeX == "Save")
					{
						TempData["SuccessMessage"] = "Public Event successfully created";
						return RedirectToAction("List");
					}
					else if (SubmittypeX == "Submit")
					{
						return RedirectToAction("Details", "PublicEvent", new { area = "eEvent", id = response.Data });
					}
					else
					{
						TempData["SuccessMessage"] = "Public Event successfully created";
						return RedirectToAction("List");
					}

				}

			}

			model.CategoryList = new SelectList(await GetCategory(), "Id", "Name");
			model.SpeakerList = new SelectList(await GetSpeaker(), "Id", "UserName", 0);
			model.ExternalExhibitorList = new SelectList(await GetExternalExhibitor(), "Id", "Name", 0);

			return View(model);
		}

		[HasAccess(UserAccess.EventAdministratorFED) ]
		// GET: PublicEvent/Edit/5
		public async Task<ActionResult> Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var response = await WepApiMethod.SendApiAsync<EditPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

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

				FreeIndividual = response.Data.FreeIndividual,
				FreeIndividualPaper = response.Data.FreeIndividualPaper,
				FreeIndividualPresent = response.Data.FreeIndividualPresent,
				FreeAgency = response.Data.FreeAgency,

				IndividualFee = response.Data.IndividualFee,
				IndividualPaperFee = response.Data.IndividualPaperFee,
				IndividualPresentFee = response.Data.IndividualPresentFee,
				AgencyFee = response.Data.AgencyFee,

				IndividualEarlyBird = response.Data.IndividualEarlyBird,
				IndividualPaperEarlyBird = response.Data.IndividualPaperEarlyBird,
				IndividualPresentEarlyBird = response.Data.IndividualPresentEarlyBird,
				AgencyEarlyBird = response.Data.AgencyEarlyBird,

				//Fee = response.Data.Fee,
				SeatAllocated_EarlyBird = response.Data.SeatAllocated_EarlyBird,
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
				Attachments = response.Data.Attachments,

                TentativeScript = response.Data.tentativeScript
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

			if (model.SeatAllocated_EarlyBird > model.ParticipantAllowed)
			{
				ModelState.AddModelError("SeatAllocated_EarlyBird", "Seat Allocated must be lower than Participant Allowed");
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

					FreeIndividual = model.FreeIndividual,
					FreeIndividualPaper = model.FreeIndividualPaper,
					FreeIndividualPresent = model.FreeIndividualPresent,
					FreeAgency = model.FreeAgency,

					IndividualFee = model.IndividualFee,
					IndividualPaperFee = model.IndividualPaperFee,
					IndividualPresentFee = model.IndividualPresentFee,
					AgencyFee = model.AgencyFee,

					IndividualEarlyBird = model.IndividualEarlyBird,
					IndividualPaperEarlyBird = model.IndividualPaperEarlyBird,
					IndividualPresentEarlyBird = model.IndividualPresentEarlyBird,
					AgencyEarlyBird = model.AgencyEarlyBird,

					//Fee = model.Fee,
					SeatAllocated_EarlyBird = model.SeatAllocated_EarlyBird,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					EventStatus = model.EventStatus,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
					Remarks = model.Remarks,
					Attachments = model.Attachments,

                    tentativeScript = model.TentativeScript
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

		[HasAccess(UserAccess.EventAdministratorFED)]
		// GET: PublicEvent/Delete/5
		[HttpGet]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<DetailsPublicEventModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetDelete?id={id}");

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

				FreeIndividual = response.Data.FreeIndividual,
				FreeIndividualPaper = response.Data.FreeIndividualPaper,
				FreeIndividualPresent = response.Data.FreeIndividualPresent,
				FreeAgency = response.Data.FreeAgency,

				IndividualFee = response.Data.IndividualFee,
				IndividualPaperFee = response.Data.IndividualPaperFee,
				IndividualPresentFee = response.Data.IndividualPresentFee,
				AgencyFee = response.Data.AgencyFee,

				IndividualEarlyBird = response.Data.IndividualEarlyBird,
				IndividualPaperEarlyBird = response.Data.IndividualPaperEarlyBird,
				IndividualPresentEarlyBird = response.Data.IndividualPresentEarlyBird,
				AgencyEarlyBird = response.Data.AgencyEarlyBird,

				//Fee = response.Data.Fee,
				SeatAllocated_EarlyBird = response.Data.SeatAllocated_EarlyBird,
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
                TentativeScript = response.Data.tentativeScript
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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Submit_PublicEvent}");
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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Verify_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver1_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver2_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver3_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.RequireAmendment_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}


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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Cancelled_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}


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

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Published_PublicEvent}");
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
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}


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


		[HttpGet]
		public ActionResult Create_RequestCancelModify(int? id)
		{
			var model = new FEP.Intranet.Areas.eEvent.Models.EventRequestModel()
			{
				EventId = id
			};

			return View(model);
		}


		// POST: PublicEvent/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create_RequestCancelModify(FEP.Intranet.Areas.eEvent.Models.EventRequestModel model)
		{
			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EventRequestModel
				{
					RequestStatus = RequestStatus.New,
					Reason = model.Reason,
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					EventId = model.EventId,
					RequestType = model.RequestType,
					Display = true,
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

				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/PublicEvent/Create", modelapi);

				if (response.isSuccess)
				{
					await LogActivity(Modules.Event, "Create Cancellation/Modification Request", model);
					TempData["SuccessMessage"] = "Cancellation/Modification Request successfully created and need to submit for verification";
					return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
				}

			}

			return View(model);
		}



		[HttpGet]
		public async Task<ActionResult> Details_RequestCancelModify(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			var response = await WepApiMethod.SendApiAsync<EventRequestApprovalModel>(HttpVerbs.Get, $"eEvent/PublicEvent/EventRequestDetails?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var reqapproval = response.Data;

			var eventreq = new EventRequestModel()
			{
				Id = reqapproval.eventrequest.Id,
				EventTitle = reqapproval.eventrequest.EventTitle,
				EventObjective = reqapproval.eventrequest.EventObjective,
				EventCategory = reqapproval.eventrequest.EventCategory,
				EventId = reqapproval.eventrequest.EventId,
				RequestStatus = reqapproval.eventrequest.RequestStatus,
				RequestType = reqapproval.eventrequest.RequestType,
				CreatedBy = reqapproval.eventrequest.CreatedBy,
				CreatedByName = reqapproval.eventrequest.CreatedByName,
				Display = reqapproval.eventrequest.Display,
				EventRefNo = reqapproval.eventrequest.EventRefNo,
				Reason = reqapproval.eventrequest.Reason,
				Attachments = reqapproval.eventrequest.Attachments,
				CreatedDate = reqapproval.eventrequest.CreatedDate,
			};

			var approval = new ApprovalModel
			{
				Id = reqapproval.approval.Id,
				EventId = reqapproval.approval.EventId,
				Level = reqapproval.approval.Level,
				ApproverId = reqapproval.approval.ApproverId,
				Status = reqapproval.approval.Status,
				Remarks = reqapproval.approval.Remarks,
				RequireNext = reqapproval.approval.RequireNext
			};

			var pevaluation = new EventRequestApprovalModel
			{
				eventrequest = eventreq,
				approval = approval
			};

			var responseHistory = await WepApiMethod.SendApiAsync<IEnumerable<PublicEventApprovalHistoryModel>>(HttpVerbs.Get, $"eEvent/PublicEvent/GetHistoryRequest?id={reqapproval.eventrequest.Id}");

			if (responseHistory.isSuccess)
			{
				ViewBag.History = responseHistory.Data;
			}

			return View(pevaluation);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Details_RequestCancelModify(EventRequestApprovalModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/UpdateApprovalEventRequest", model);

				if (response.isSuccess)
				{
					if (model.approval.Status == EventApprovalStatus.Approved)
					{
						if (model.approval.Level == EventApprovalLevel.Verifier)
						{
							await VerifiedRequest(model.approval.EventId);
							await LogActivity(Modules.Event, "Verify Cancellation/Modification Request: ", model);
							TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully verified.";
						}
						else
						{
							if (model.approval.Level == EventApprovalLevel.Approver1)
							{
								if (model.approval.RequireNext)
								{
									TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully approved and require next approval.";
									await FirstApprovedRequest(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Cancellation/Modification Request: ", model);
								}
								else
								{
									TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully approved.";
									await FinalApprovedRequest(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Cancellation/Modification Request by Approver 1 ", model);
								}
							}
							else if (model.approval.Level == EventApprovalLevel.Approver2)
							{
								if (model.approval.RequireNext)
								{
									TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully approved and require next approval.";
									await SecondApprovedRequest(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Cancellation/Modification Request: ", model);
								}
								else
								{
									TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully approved.";
									await FinalApprovedRequest(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Cancellation/Modification Request by Approver 2 ", model);
								}
							}
							else if (model.approval.Level == EventApprovalLevel.Approver3)
							{
								TempData["SuccessMessage"] = "Cancellation/Modification Request is successfully approved.";
								await FinalApprovedRequest(model.approval.EventId);
								await LogActivity(Modules.Event, "Cancellation/Modification Request Event by Approver 3  ", model);
							}
						}
					}
					else
					{
						if (model.approval.Level == EventApprovalLevel.Verifier)
						{
							await RequireAmendmentRequest(model.approval.EventId);
							await LogActivity(Modules.Event, "Cancellation/Modification Request requires amendment.", model);
							TempData["SuccessMessage"] = "Cancellation/Modification Request requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver1)
						{
							await RequireAmendmentRequest(model.approval.EventId);
							await LogActivity(Modules.Event, "Cancellation/Modification Request requires amendment.", model);
							TempData["SuccessMessage"] = "Cancellation/Modification Request requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver2)
						{
							await RequireAmendmentRequest(model.approval.EventId);
							await LogActivity(Modules.Event, "Cancellation/Modification Request requires amendment.", model);
							TempData["SuccessMessage"] = "Cancellation/Modification Request requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver3)
						{
							await RequireAmendmentRequest(model.approval.EventId);
							await LogActivity(Modules.Event, "Cancellation/Modification Request requires amendment.", model);
							TempData["SuccessMessage"] = "Cancellation/Modification Request requires amendment.";
						}
					}
					return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
				}
				else
				{
					return RedirectToAction("List", "PublicEvent", new { area = "eEvent", @id = model.approval.EventId });
				}
			}

			return View(model);
		}


		public async Task<ActionResult> Edit_RequestCancelModify(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Get, $"eEvent/PublicEvent/GetEditDeleteRequest?id={id}");

			if (!response.isSuccess)
			{
				return HttpNotFound();
			}

			var model = new FEP.Intranet.Areas.eEvent.Models.EventRequestModel()
			{
				Id = response.Data.Id,
				EventTitle = response.Data.EventTitle,
				EventObjective = response.Data.EventObjective,
				EventCategory = response.Data.EventCategory,
				EventId = response.Data.EventId,
				RequestStatus = response.Data.RequestStatus,
				RequestType = response.Data.RequestType,
				CreatedBy = response.Data.CreatedBy,
				CreatedByName = response.Data.CreatedByName,
				Display = response.Data.Display,
				EventRefNo = response.Data.EventRefNo,
				Reason = response.Data.Reason,
				Attachments = response.Data.Attachments,
				CreatedDate = response.Data.CreatedDate,
			};

			if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		// POST: PublicEvent/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit_RequestCancelModify(FEP.Intranet.Areas.eEvent.Models.EventRequestModel model)
		{

			if (model.Attachments.Count() == 0 && model.AttachmentFiles.Count() == 0)
			{
				ModelState.AddModelError("Attachments", "Please upload file");
			}

			if (ModelState.IsValid)
			{
				var modelapi = new EventRequestModel
				{
					Id = model.Id,
					RequestStatus = model.RequestStatus,
					Reason = model.Reason,
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					EventId = model.EventId,
					RequestType = model.RequestType,
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

				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/PublicEvent/Edit?id={model.Id}", modelapi);

				if (response.isSuccess)
				{

					await LogActivity(Modules.Event, "Update Cancellation/Modification Request", model);
					TempData["SuccessMessage"] = "Cancellation/Modification Request successfully updated";

					return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { Id = model.Id });
				}
			}

			return View(model);
		}






		public async Task<ActionResult> SubmitToVerifyRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/SubmitToVerifyRequest?id={id}");
			if (response.isSuccess)
			{
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Submit_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Submit_CancellationModification_For_Verification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};

					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;
						response.Data.SLAReminderStatusId = saveThisID;
						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is submit for verification.");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully submitted for verification.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to submit Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", for verification.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}



		public async Task<ActionResult> VerifiedRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/VerifiedRequest?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAIdRequest?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Verifier_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)


				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Verify_CancellationModification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is verified.");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully verified and submitted for approval.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Cancellation/Modification Request.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> FirstApprovedRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/FirstApprovedRequest?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAIdRequest?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//
				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver1_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_CancellationModification_ByApprover_1,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is approved on first level.");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully approved and submitted to next approval.";
				}
				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Fail to approve Cancellation/Modification Request.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> SecondApprovedRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/SecondApprovedRequest?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAIdRequest?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver2_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)
				{
					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_CancellationModification_ByApprover_2,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is approved on second level.");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully approved and submitted to next approval.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Cancellation/Modification Request.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}


		public async Task<ActionResult> FinalApprovedRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/FinalApprovedRequest?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAIdRequest?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Approver3_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.Approve_CancellationModification_ByApprover_3,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is approved");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully approved.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to approve Cancellation/Modification Request.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}

		public async Task<ActionResult> RequireAmendmentRequest(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var response = await WepApiMethod.SendApiAsync<EventRequestModel>(HttpVerbs.Post, $"eEvent/PublicEvent/RequireAmendmentRequest?id={id}");
			if (response.isSuccess)
			{
				//--------------------------------------------------Stop Previous Email---------------------------------------------//
				var responseGetSLAId = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"eEvent/PublicEvent/GetSLAIdRequest?id={id}");
				if (responseGetSLAId.isSuccess)
				{
					int SLAReminderStatusId = responseGetSLAId.Data;
					var responseSLA = await WepApiMethod.SendApiAsync<List<BulkNotificationModel>>
						(HttpVerbs.Get, $"Reminder/SLA/StopNotification/?SLAReminderStatusId={SLAReminderStatusId}");
					List<BulkNotificationModel> myNotification = responseSLA.Data;
				}

				//--------------------------------------------------Send Email---------------------------------------------//

				ParameterListToSend paramToSend = new ParameterListToSend();
				paramToSend.EventCode = response.Data.EventRefNo;
				paramToSend.EventName = response.Data.EventTitle;
				paramToSend.RequestType = response.Data.RequestType.GetDisplayName();
				paramToSend.RequestStatus = response.Data.RequestStatus.GetDisplayName();
				paramToSend.Reason = response.Data.Reason;

				var receiveresponse = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"Administration/Access/GetUser?access={UserAccess.Amendment_CancellationModificationRequest}");
				if (receiveresponse.isSuccess)
				{

					CreateAutoReminder reminder = new CreateAutoReminder
					{
						NotificationType = NotificationType.RequireAmendment_CancellationModification,
						NotificationCategory = NotificationCategory.Event,
						ParameterListToSend = paramToSend,
						StartNotificationDate = DateTime.Now,
						ReceiverId = receiveresponse.Data,
					};
					var response2 = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
					if (response2.isSuccess)
					{
						int saveThisID = response2.Data.SLAReminderStatusId;

						response.Data.SLAReminderStatusId = saveThisID;

						var response3 = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/SaveSLAStatusId?id={response.Data.Id}&saveThisID={saveThisID}");
						if (response3.isSuccess) { }
					}

					await LogActivity(Modules.Event, "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + " is required amendment.");
					TempData["SuccessMessage"] = "Cancellation/Modification Request for Public Event Ref No: " + response.Data.EventRefNo + ", successfully required amendment.";
				}

				return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to amendment Cancellation/Modification Request.";
				return RedirectToAction("Details_RequestCancelModify", "PublicEvent", new { area = "eEvent", @id = id });
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Details(PublicEventApprovalModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eEvent/PublicEvent/UpdateApproval", model);

				if (response.isSuccess)
				{
					if (model.approval.Status == EventApprovalStatus.Approved)
					{
						if (model.approval.Level == EventApprovalLevel.Verifier)
						{
							await Verified(model.approval.EventId);
							await LogActivity(Modules.Event, "Verify Public Event: ", model);
							TempData["SuccessMessage"] = "Public Event is successfully verified.";
						}
						else
						{
							if (model.approval.Level == EventApprovalLevel.Approver1)
							{
								if (model.approval.RequireNext)
								{
									TempData["SuccessMessage"] = "Public Event is successfully approved and require next approval.";
									await FirstApproved(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Public Event: ", model);
								}
								else
								{
									TempData["SuccessMessage"] = "Public Event is successfully approved.";
									await FinalApproved(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Public Event by Approver 1 ", model);
								}
							}
							else if (model.approval.Level == EventApprovalLevel.Approver2)
							{
								if (model.approval.RequireNext)
								{
									TempData["SuccessMessage"] = "Public Event is successfully approved and require next approval.";
									await SecondApproved(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Public Event: ", model);
								}
								else
								{
									TempData["SuccessMessage"] = "Public Event is successfully approved.";
									await FinalApproved(model.approval.EventId);
									await LogActivity(Modules.Event, "Approve Public Event by Approver 2 ", model);
								}
							}
							else if (model.approval.Level == EventApprovalLevel.Approver3)
							{
								TempData["SuccessMessage"] = "Public Event is successfully approved.";
								await FinalApproved(model.approval.EventId);
								await LogActivity(Modules.Event, "Approve Public Event by Approver 3  ", model);
							}
						}
					}
					else
					{
						if (model.approval.Level == EventApprovalLevel.Verifier)
						{
							await RejectPublicEvent(model.approval.EventId);
							await LogActivity(Modules.Event, "Public Event requires amendment.", model);
							TempData["SuccessMessage"] = "Public Event requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver1)
						{
							await RejectPublicEvent(model.approval.EventId);
							await LogActivity(Modules.Event, "Public Event requires amendment.", model);
							TempData["SuccessMessage"] = "Public Event requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver2)
						{
							await RejectPublicEvent(model.approval.EventId);
							await LogActivity(Modules.Event, "Public Event requires amendment.", model);
							TempData["SuccessMessage"] = "Public Event requires amendment.";
						}
						else if (model.approval.Level == EventApprovalLevel.Approver3)
						{
							await RejectPublicEvent(model.approval.EventId);
							await LogActivity(Modules.Event, "Public Event requires amendment.", model);
							TempData["SuccessMessage"] = "Public Event requires amendment.";
						}
					}
					return RedirectToAction("List", "PublicEvent", new { area = "eEvent" });
				}
				else
				{
					return RedirectToAction("List", "PublicEvent", new { area = "eEvent", @id = model.approval.EventId });
				}
			}

			return View(model);
		}


	}
}