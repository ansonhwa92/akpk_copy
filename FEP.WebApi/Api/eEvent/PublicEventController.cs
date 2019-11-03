using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel;
using FEP.WebApiModel.FileDocuments;
using FEP.WebApiModel.PublicEvent;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/PublicEvent")]
	public class PublicEventController : ApiController
	{
		private DbEntities db = new DbEntities();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		[HttpGet]
		public IHttpActionResult Get()
		{
			var users = db.PublicEvent.Where(u => u.Display).Select(s => new PublicEventModel
			{
				Id = s.Id,
				RefNo = s.RefNo,
				EventTitle = s.EventTitle
			}).ToList();

			return Ok(users);
		}

		[Route("api/eEvent/PublicEvent/GetEventList")]
		[HttpPost]
		public IHttpActionResult Post(FilterPublicEventModel request)
		{

			var query = db.PublicEvent.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.EventTitle == null || s.EventTitle.Contains(request.EventTitle))
			   && (request.RefNo == null || s.RefNo.Contains(request.RefNo))
			   //&& (request.TargetedGroup == null || s.TargetedGroup == request.TargetedGroup)
			   //&& (request.EventStatus.GetDisplayName() == null || s.EventStatus.GetDisplayName() == request.EventStatus.GetDisplayName())
			   );

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.EventTitle.Contains(value)
				//|| p.EventCategory.CategoryName.Contains(value)
				//|| p.TargetedGroup.GetDisplayName().Contains(value)
				//|| p.EventStatus.GetDisplayName().Contains(value)
				);
			}

			var filteredCount = query.Count();

			//order
			if (request.order != null)
			{
				string sortBy = request.columns[request.order[0].column].data;
				bool sortAscending = request.order[0].dir.ToLower() == "asc";

				switch (sortBy)
				{
					case "EventTitle":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventTitle);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventTitle);
						}

						break;

					case "RefNo":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.RefNo);
						}
						else
						{
							query = query.OrderByDescending(o => o.RefNo);
						}

						break;

					case "EventCategoryId":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventCategory.CategoryName);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventCategory.CategoryName);
						}

						break;

					case "StartDate":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.StartDate);
						}
						else
						{
							query = query.OrderByDescending(o => o.StartDate);
						}

						break;

					case "EndDate":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EndDate);
						}
						else
						{
							query = query.OrderByDescending(o => o.EndDate);
						}

						break;

					case "Venue":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Venue);
						}
						else
						{
							query = query.OrderByDescending(o => o.Venue);
						}

						break;

					case "Fee":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Fee);
						}
						else
						{
							query = query.OrderByDescending(o => o.Fee);
						}

						break;

					case "EventStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.EventTitle);
						break;
				}

			}
			else
			{
				query = query.OrderByDescending(o => o.EventTitle);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(s => new PublicEventModel
				{
					Id = s.Id,
					EventTitle = s.EventTitle,
					EventCategoryId = s.EventCategoryId,
					EventCategoryName = s.EventCategory.CategoryName,
					TargetedGroup = s.TargetedGroup,
					EventStatus = s.EventStatus,
					//EventStatusDesc = s.EventStatus.GetDisplayName(),
					StartDate = s.StartDate,
					EndDate = s.EndDate,
					EventObjective = s.EventObjective,
					Venue = s.Venue,
					Fee = s.Fee,
					RefNo = s.RefNo
				}).ToList();

			data.ForEach(s => s.EventStatusDesc = s.EventStatus.GetDisplayName());

			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount,
				data = data.ToArray()
			});
		}

		public IHttpActionResult Get(int id)
		//public GlobalPublicEventApprovalModel Get(int id)
		{
			var model = db.PublicEvent.Where(i => i.Display && i.Id == id)
				.Select(i => new DetailsPublicEventModel
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					TargetedGroup = i.TargetedGroup,
					ParticipantAllowed = i.ParticipantAllowed,
					Remarks = i.Remarks,
					RefNo = i.RefNo,
					CreatedDate = i.CreatedDate,
					CreatedByName = i.CreatedByUser.Name,
					
				}).FirstOrDefault();

			//var approval = db.PublicEventApproval.Where(pa => pa.EventId == id && pa.Status == EventApprovalStatus.None).Select(s => new PublicEventApprovalModel
			//{
			//	Id = s.Id,
			//	EventId = s.EventId,
			//	Level = s.ApprovalLevel,
			//	ApproverId = 0,
			//	Status = EventApprovalStatus.None,
			//	Remarks = "",
			//	RequireNext = s.RequireNext
			//}).FirstOrDefault();

			//var evaluation = new DetailsPublicEventModel
			//{
			//	//publicevent = model,
			//	approval = approval
			//};

			if (model == null)
			{
				return NotFound();
			}

			model.SpeakerId = db.AssignedSpeaker.Where(u => u.PublicEventId == id).Select(s => s.EventSpeakerId).ToArray();
			model.ExternalExhibitorId = db.AssignedExternalExhibitor.Where(u => u.PublicEventId == id).Select(s => s.ExternalExhibitorId).ToArray();
			model.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.PublicEvent && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

			return Ok(model);
		}

		//Create
		[HttpPost]
		public IHttpActionResult Post([FromBody] CreatePublicEventModel model)
		{
			var publicevent = new PublicEvent
			{
				EventTitle = model.EventTitle,
				EventObjective = model.EventObjective,
				StartDate = model.StartDate,
				EndDate = model.EndDate,
				Venue = model.Venue,
				Fee = model.Fee,
				EventStatus = model.EventStatus,
				EventCategoryId = model.EventCategoryId,
				TargetedGroup = model.TargetedGroup,
				ParticipantAllowed = model.ParticipantAllowed,
				Remarks = model.Remarks,
				CreatedBy = null,
				Display = true,
				CreatedDate = DateTime.Now,
			};

			foreach (var speakerid in model.SpeakerId)
			{
				var assignedsp = new AssignedSpeaker
				{
					EventSpeakerId = speakerid,
					PublicEvent = publicevent,
				};

				db.AssignedSpeaker.Add(assignedsp);
			}

			db.PublicEvent.Add(publicevent);

			foreach (var externalexhibitorid in model.ExternalExhibitorId)
			{
				var assignedex = new AssignedExternalExhibitor
				{
					ExternalExhibitorId = externalexhibitorid,
					PublicEvent = publicevent,
				};

				db.AssignedExternalExhibitor.Add(assignedex);
			}

			db.PublicEvent.Add(publicevent);

			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.PublicEvent,
					FileId = fileid,
					ParentId = publicevent.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.SaveChanges();

			//if (publicevent != null )
			//{
			//	var approval = new PublicEventApproval
			//	{
			//		EventId = publicevent.Id,
			//		ApprovalLevel = EventApprovalLevel.Verifier,
			//		ApproverId = 0,
			//		Status = EventApprovalStatus.None,
			//		ApprovedDate = DateTime.Now,
			//		Remark = "",
			//		RequireNext = false
			//	};

			//	db.PublicEventApproval.Add(approval);
			//}

			//---running number----//
			var refno = "EVP/" + DateTime.Now.ToString("yyMM");
			refno += "/" + publicevent.Id.ToString("D4");
			publicevent.RefNo = refno;
			db.Entry(publicevent).State = EntityState.Modified;

			db.SaveChanges();

			return Ok(publicevent.Id);
		}


		//Edit
		public IHttpActionResult Put(int id, [FromBody] EditPublicEventModel model)
		{
			var publicevent = db.PublicEvent.Where(u => u.Id == id).FirstOrDefault();

			if (publicevent == null)
			{
				return NotFound();
			}

			publicevent.EventTitle = model.EventTitle;
			publicevent.EventObjective = model.EventObjective;
			publicevent.StartDate = model.StartDate;
			publicevent.EndDate = model.EndDate;
			publicevent.Venue = model.Venue;
			publicevent.Fee = model.Fee;
			publicevent.EventStatus = model.EventStatus;
			publicevent.EventCategoryId = model.EventCategoryId;
			publicevent.TargetedGroup = model.TargetedGroup;
			publicevent.ParticipantAllowed = model.ParticipantAllowed;
			publicevent.Remarks = model.Remarks;
			publicevent.RefNo = model.RefNo;

			db.Entry(publicevent).State = EntityState.Modified;

			db.Entry(publicevent).Property(x => x.Display).IsModified = false;
			db.Entry(publicevent).Property(x => x.RefNo).IsModified = false;

			db.AssignedSpeaker.RemoveRange(db.AssignedSpeaker.Where(u => u.PublicEventId == id));//remove all
			foreach (var assignedspeakerid in model.SpeakerId)
			{
				var assignedsp = new AssignedSpeaker
				{
					EventSpeakerId = assignedspeakerid,
					PublicEventId = id,
				};

				db.AssignedSpeaker.Add(assignedsp);
			}

			db.AssignedExternalExhibitor.RemoveRange(db.AssignedExternalExhibitor.Where(u => u.PublicEventId == id));//remove all
			foreach (var externalexhibitorid in model.ExternalExhibitorId)
			{
				var assignedsp = new AssignedExternalExhibitor
				{
					ExternalExhibitorId = externalexhibitorid,
					PublicEventId = id,
				};

				db.AssignedExternalExhibitor.Add(assignedsp);
			}

			//remove file 
			var attachments = db.EventFile.Where(s => s.FileCategory == EventFileCategory.PublicEvent && s.ParentId == model.Id).ToList();

			if (attachments != null)
			{
				//delete all
				if (model.Attachments == null)
				{
					foreach (var attachment in attachments)
					{
						attachment.FileDocument.Display = false;
						db.FileDocument.Attach(attachment.FileDocument);
						db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

						db.EventFile.Remove(attachment);
					}
				}
				else
				{
					foreach (var attachment in attachments)
					{
						if (!model.Attachments.Any(u => u.Id == attachment.FileDocument.Id))//delete if not exist anymore
						{
							attachment.FileDocument.Display = false;
							db.FileDocument.Attach(attachment.FileDocument);
							db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

							db.EventFile.Remove(attachment);
						}
					}
				}
			}

			//add new file
			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.PublicEvent,
					FileId = fileid,
					ParentId = publicevent.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var publicEvent = db.PublicEvent.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (publicEvent == null)
			{
				return NotFound();
			}

			publicEvent.Display = false;
			db.Entry(publicEvent).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

		//Submit Public Event for Verification
		[Route("api/eEvent/PublicEvent/SubmitToVerify")]
		public IHttpActionResult SubmitToVerify(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.PendingforVerification;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;

				// create first approval record (using existing ID)
				var approval = new PublicEventApproval
				{
					EventId = publicevent.Id,
					ApprovalLevel = EventApprovalLevel.Verifier,
					ApproverId = null,
					Status = EventApprovalStatus.None,
					ApprovedDate = DateTime.Now,
					Remark = "",
					RequireNext = false
				};

				db.PublicEventApproval.Add(approval);
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
					EventStatusDesc = publicevent.EventStatus.GetDisplayName()
				};
				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/PublicEvent/Verified")]
		public IHttpActionResult Verified(int id)
		{
			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();
			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Verified;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
					EventStatusDesc = publicevent.EventStatus.GetDisplayName()
				};
				return Ok(model);
			}
			return Ok();
		}

		//First Approved Public Event 
		[Route("api/eEvent/PublicEvent/FirstApproved")]
		public IHttpActionResult FirstApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.VerifiedbyFirstApprover;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}

		//Second Approved Public Event 
		[Route("api/eEvent/PublicEvent/SecondApproved")]
		public IHttpActionResult SecondApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.VerifiedbySecondApprover;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}

		//Final Approved Public Event 
		[Route("api/eEvent/PublicEvent/FinalApproved")]
		public IHttpActionResult FinalApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Approved;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}

		//Reject Approved Public Event 
		[Route("api/eEvent/PublicEvent/RejectPublicEvent")]
		public IHttpActionResult RejectPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.RejectNeedToEdit;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}

		//Cancel Approved Public Event 
		[Route("api/eEvent/PublicEvent/CancelPublicEvent")]
		public IHttpActionResult CancelPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Cancelled;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}

		//Published Public Event 
		[Route("api/eEvent/PublicEvent/PublishedPublicEvent")]
		public IHttpActionResult PublishedPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Published;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				PublicEventModel model = new PublicEventModel
				{
					Id = publicevent.Id,
					EventStatus = publicevent.EventStatus,
					RefNo = publicevent.RefNo,
					EventTitle = publicevent.EventTitle,
				};
				return Ok(model);
			}
			return Ok();
		}


		[Route("api/eEvent/PublicEvent/SaveSLAStatusId")]
		public string SaveSLAStatusId(int id, int SaveThisID)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.SLAReminderStatusId = SaveThisID;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.SLAReminderStatusId).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return "";
			}
			return "";
		}


		[Route("api/eEvent/PublicEvent/GetSLAId")]
		public IHttpActionResult GetSLAId(int id)
		{
			var publicevent = db.PublicEvent.Where(p => p.Id == id).Select(i => i.SLAReminderStatusId).FirstOrDefault();

			return Ok(publicevent);
		}


		[Route("api/eEvent/PublicEvent/GetHistory")]
		public List<PublicEventApprovalHistoryModel> GetHistory(int id)
		{
			var phistory = db.PublicEventApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { pa.EventId, pa.ApprovalLevel, pa.ApproverId, pa.ApprovedDate, pa.Status, pa.Remark, UserName = u.Name }).Where(pa => pa.EventId == id && pa.Status != EventApprovalStatus.None).OrderByDescending(pa => pa.ApprovedDate).Select(s => new PublicEventApprovalHistoryModel
			{
				Level = s.ApprovalLevel,
				ApproverId = s.ApproverId,
				ApprovalDate = s.ApprovedDate,
				UserName = s.UserName,
				Status = s.Status,
				Remarks = s.Remark
			}).ToList();

			return phistory;
		}

		//[Route("api/eEvent/PublicEvent/Evaluate")]
		//[HttpPost]
		//[ValidationActionFilter]
		//public string Evaluate([FromBody] GlobalPublicEventApprovalModel model)
		//{

		//	if (ModelState.IsValid)
		//	{
		//		var papproval = db.PublicEventApproval.Where(pa => pa.Id == model.approval.Id).FirstOrDefault();

		//		if (papproval != null)
		//		{
		//			papproval.ApproverId = model.approval.ApproverId;
		//			papproval.Status = model.approval.Status;
		//			papproval.ApprovedDate = DateTime.Now;
		//			papproval.Remark = model.approval.Remarks;
		//			papproval.RequireNext = model.approval.RequireNext;
		//			// requirenext is always set to true when coming from verifier approval, and always false from approver3

		//			db.Entry(papproval).State = EntityState.Modified;
		//			// HERE
		//			db.SaveChanges();

		//			var publicevent = db.PublicEvent.Where(p => p.Id == papproval.EventId).FirstOrDefault();
		//			if (publicevent != null)
		//			{
		//				// proceed depending on status (assuming user can only pick approve and reject)
		//				if (model.approval.Status == EventApprovalStatus.Rejected)
		//				{
		//					if (publicevent.EventStatus == EventStatus.PendingforVerification)
		//					{
		//						publicevent.EventStatus = EventStatus.RejectNeedToEdit;
		//						db.Entry(publicevent).State = EntityState.Modified;
		//						db.SaveChanges();
		//					}
		//					else if (publicevent.EventStatus == EventStatus.Verified)
		//					{
		//						publicevent.EventStatus = EventStatus.RejectNeedToEdit;
		//						db.Entry(publicevent).State = EntityState.Modified;
		//						db.SaveChanges();
		//					}
		//				}
		//				else
		//				{
		//					// proceed depending on requirenext
		//					if (model.approval.RequireNext == false)
		//					{
		//						// no more approvals necessary (assumes verifier will never get here)
		//						publicevent.EventStatus = EventStatus.Approved;
		//						db.Entry(publicevent).State = EntityState.Modified;
		//						db.SaveChanges();
		//					}
		//					else
		//					{
		//						EventApprovalLevel nextlevel;
		//						switch (papproval.ApprovalLevel)
		//						{
		//							case EventApprovalLevel.Verifier:
		//								nextlevel = EventApprovalLevel.Approver1;
		//								publicevent.EventStatus = EventStatus.Verified;
		//								db.Entry(publicevent).State = EntityState.Modified;
		//								break;
		//							case EventApprovalLevel.Approver1:
		//								nextlevel = EventApprovalLevel.Approver2;
		//								break;
		//							case EventApprovalLevel.Approver2:
		//								nextlevel = EventApprovalLevel.Approver3;
		//								break;
		//							default:
		//								nextlevel = EventApprovalLevel.Approver1;
		//								break;
		//						}

		//						// create next approval record
		//						var pnewapproval = new PublicEventApproval
		//						{
		//							EventId = papproval.EventId,
		//							ApprovalLevel = nextlevel,
		//							ApproverId = 0,
		//							Status = EventApprovalStatus.None,
		//							ApprovedDate = DateTime.Now,
		//							Remark = "",
		//							RequireNext = false
		//						};

		//						db.PublicEventApproval.Add(pnewapproval);
		//						// HERE
		//						db.SaveChanges();
		//					}

		//				}

		//				//return publication.Title;
		//				return publicevent.Id + "|" + publicevent.EventTitle + "|" + publicevent.RefNo + "|" + publicevent.Venue + "|" + publicevent.EventStatus;
		//			}
		//		}
		//	}

		//	return "";
		//}


	}
}
