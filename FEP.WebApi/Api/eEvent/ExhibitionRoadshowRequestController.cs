using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/ExhibitionRoadshowRequest")]
	public class ExhibitionRoadshowRequestController : ApiController
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

		[Route("api/eEvent/ExhibitionRoadshowRequest/GetExhibitionList")]
		[HttpPost]
		public IHttpActionResult Post(FilterExhibitionRoadshowRequestModel request)
		{

			var query = db.EventExhibitionRequest.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.EventName == null || s.EventName.Contains(request.EventName))

			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.EventName.Contains(value)

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
					case "EventName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventName);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventName);
						}

						break;

					case "Organiser":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Organiser);
						}
						else
						{
							query = query.OrderByDescending(o => o.Organiser);
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


					case "ExhibitionStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.ExhibitionStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.ExhibitionStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.EventName);
						break;
				}

			}
			else
			{
				query = query.OrderByDescending(o => o.EventName);
			}


			var data = query.Skip(request.start).Take(request.length)
				.Select(i => new ExhibitionRoadshowRequestModel
				{
					Id = i.Id,
					EventName = i.EventName,
					Organiser = i.Organiser,
					OrganiserEmail = i.OrganiserEmail,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					StartTime = i.StartTime,
					EndTime = i.EndTime,
					ExhibitionStatus = i.ExhibitionStatus,
					ParticipationRequirement = i.ParticipationRequirement,
					ReceivedById = i.ReceivedById,
					ReceivedDate = i.ReceivedDate,
					Receive_Via = i.Receive_Via,
				}).ToList();

			data.ForEach(s => s.ExhibitionStatusDesc = s.ExhibitionStatus.GetDisplayName());


			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount,
				data = data.ToArray()
			});
		}


		public ExhibitionRoadshowApprovalModel Get(int id)
		{
			var exhibition = db.EventExhibitionRequest.Where(u => u.Id == id)
				.Select(s => new ExhibitionRoadshowRequestModel
				{
					Id = s.Id,
					EventName = s.EventName,
					Organiser = s.Organiser,
					OrganiserEmail = s.OrganiserEmail,
					AddressStreet1 = s.AddressStreet1,
					AddressStreet2 = s.AddressStreet2,
					AddressPoscode = s.AddressPoscode,
					AddressCity = s.AddressCity,
					State = s.State,
					StartDate = s.StartDate,
					EndDate = s.EndDate,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					ExhibitionStatus = s.ExhibitionStatus,
					ParticipationRequirement = s.ParticipationRequirement,
					ReceivedById = s.ReceivedById,
					ReceivedByName = s.ReceivedBy.Name,
					ReceivedDate = s.ReceivedDate,
					Receive_Via = s.Receive_Via,
					RefNo = s.RefNo,
					CreatedDate = s.CreatedDate,
					CreatedBy = s.CreatedBy,
					CreatedByName = s.CreatedByUser.Name,
				}).FirstOrDefault();


			if (exhibition.ExhibitionStatus != ExhibitionStatus.ApprovedByApprover3 && exhibition.ExhibitionStatus != ExhibitionStatus.AcceptParticipation && exhibition.ExhibitionStatus != ExhibitionStatus.DeclineParticipation && exhibition.ExhibitionStatus != ExhibitionStatus.RequireAmendment && exhibition.ExhibitionStatus != ExhibitionStatus.NomineesInvited)
			{
				var approval = db.EventExhibitionRequestApproval.Where(pa => pa.ExhibitionId == id && pa.Status == EventApprovalStatus.None).Select(s => new ApprovalModel
				{
					Id = s.Id,
					ExhibitionId = s.ExhibitionId,
					Level = s.Level,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					Remarks = "",
					RequireNext = s.RequireNext
				}).FirstOrDefault();

				var evaluation = new ExhibitionRoadshowApprovalModel
				{
					exhibitionroadshow = exhibition,
					approval = approval
				};

				evaluation.exhibitionroadshow.NomineeId = db.ExhibitionNominee.Where(u => u.ExhibitionRoadshowId == id).Select(s => s.UserId).ToArray();
				evaluation.exhibitionroadshow.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.ExhibitionRoadshow && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

				return evaluation;
			}
			else
			{
				var approval = db.EventExhibitionRequestApproval.Where(pa => pa.ExhibitionId == id).Select(s => new ApprovalModel
				{
					Id = s.Id,
					ExhibitionId = s.ExhibitionId,
					Level = s.Level,
					ApproverId = 0,
					Status = s.Status,
					Remarks = "",
					RequireNext = s.RequireNext
				}).FirstOrDefault();

				var evaluation = new ExhibitionRoadshowApprovalModel
				{
					exhibitionroadshow = exhibition,
					approval = approval
				};

				evaluation.exhibitionroadshow.NomineeId = db.ExhibitionNominee.Where(u => u.ExhibitionRoadshowId == id).Select(s => s.UserId).ToArray();
				evaluation.exhibitionroadshow.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.ExhibitionRoadshow && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

				return evaluation;
			}

		}

		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateExhibitionRoadshowRequestModel model)
		{
			var exroad = new EventExhibitionRequest
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
				Display = true,
				CreatedDate = DateTime.Now,
				CreatedBy = model.CreatedBy
			};

			db.EventExhibitionRequest.Add(exroad);
			db.SaveChanges();

			foreach (var nomineeid in model.NomineeId)
			{
				var nominee = new ExhibitionNominee
				{
					UserId = nomineeid,
					ExhibitionRequest = exroad,
				};
				db.ExhibitionNominee.Add(nominee);
			}
			db.SaveChanges();

			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.ExhibitionRoadshow,
					FileId = fileid,
					ParentId = exroad.Id
				};

				db.EventFile.Add(eventfile);
			}
			db.SaveChanges();

			if (exroad != null)
			{
				var approval = new EventExhibitionRequestApproval
				{
					ExhibitionId = exroad.Id,
					Level = EventApprovalLevel.Verifier,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					ApprovedDate = DateTime.Now,
					Remark = "",
					RequireNext = false
				};

				db.EventExhibitionRequestApproval.Add(approval);
			}

			//save refno exhibition roadshow request
			var refno = "EVT/" + DateTime.Now.ToString("yyMM");
			refno += "/" + exroad.Id.ToString("D4");
			exroad.RefNo = refno;

			db.Entry(exroad).State = EntityState.Modified;
			db.SaveChanges();

			return Ok(exroad.Id);
		}


		public IHttpActionResult Put(int id, [FromBody] EditExhibitionRoadshowRequestModel model)
		{
			var exroad = db.EventExhibitionRequest.Where(u => u.Id == id).FirstOrDefault();

			if (exroad == null)
			{
				return NotFound();
			}

			exroad.EventName = model.EventName;
			exroad.Organiser = model.Organiser;
			exroad.OrganiserEmail = model.OrganiserEmail;
			exroad.AddressStreet1 = model.AddressStreet1;
			exroad.AddressStreet2 = model.AddressStreet2;
			exroad.AddressPoscode = model.AddressPoscode;
			exroad.AddressCity = model.AddressCity;
			exroad.State = model.State;
			exroad.StartDate = model.StartDate;
			exroad.EndDate = model.EndDate;
			exroad.StartTime = model.StartTime;
			exroad.EndTime = model.EndTime;
			exroad.ExhibitionStatus = model.ExhibitionStatus;
			exroad.ParticipationRequirement = model.ParticipationRequirement;
			exroad.ReceivedById = model.ReceivedById;
			exroad.ReceivedDate = model.ReceivedDate;
			exroad.Receive_Via = model.Receive_Via;

			db.EventExhibitionRequest.Attach(exroad);
			db.Entry(exroad).Property(x => x.EventName).IsModified = true;
			db.Entry(exroad).Property(x => x.Organiser).IsModified = true;
			db.Entry(exroad).Property(x => x.OrganiserEmail).IsModified = true;
			db.Entry(exroad).Property(x => x.AddressStreet1).IsModified = true;
			db.Entry(exroad).Property(x => x.AddressStreet2).IsModified = true;
			db.Entry(exroad).Property(x => x.AddressPoscode).IsModified = true;
			db.Entry(exroad).Property(x => x.AddressCity).IsModified = true;
			db.Entry(exroad).Property(x => x.State).IsModified = true;
			db.Entry(exroad).Property(x => x.StartDate).IsModified = true;
			db.Entry(exroad).Property(x => x.EndDate).IsModified = true;
			db.Entry(exroad).Property(x => x.StartTime).IsModified = true;
			db.Entry(exroad).Property(x => x.EndTime).IsModified = true;
			db.Entry(exroad).Property(x => x.ParticipationRequirement).IsModified = true;
			db.Entry(exroad).Property(x => x.ReceivedById).IsModified = true;
			db.Entry(exroad).Property(x => x.ReceivedDate).IsModified = true;
			db.Entry(exroad).Property(x => x.Receive_Via).IsModified = true;

			db.Entry(exroad).Property(x => x.ExhibitionStatus).IsModified = false;
			db.Entry(exroad).Property(x => x.Display).IsModified = false;
			db.Entry(exroad).Property(x => x.Id).IsModified = false;

			db.ExhibitionNominee.RemoveRange(db.ExhibitionNominee.Where(u => u.ExhibitionRoadshowId == id));//remove all
			foreach (var nomineeid in model.NomineeId)
			{
				var nominee = new ExhibitionNominee
				{
					ExhibitionRoadshowId = id,
					UserId = nomineeid,
				};

				db.ExhibitionNominee.Add(nominee);
			}

			//remove file 
			var attachments = db.EventFile.Where(s => s.FileCategory == EventFileCategory.ExhibitionRoadshow && s.ParentId == model.Id).ToList();

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
					FileCategory = EventFileCategory.ExhibitionRoadshow,
					FileId = fileid,
					ParentId = exroad.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var exroad = db.EventExhibitionRequest.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (exroad == null)
			{
				return NotFound();
			}

			exroad.Display = false;

			db.Entry(exroad).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/IsEventNameExist")]
		[HttpGet]
		public IHttpActionResult IsEventNameExist(int? id, string name)
		{
			if (id == null)
			{
				if (db.EventExhibitionRequest.Any(u => u.EventName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
					return Ok(true);
			}
			else
			{
				if (db.EventExhibitionRequest.Any(u => u.EventName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
					return Ok(true);
			}
			return NotFound();
		}


		[Route("api/eEvent/ExhibitionRoadshowRequest/SubmitToVerify")]
		public IHttpActionResult SubmitToVerify(int id)
		{
			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad.ExhibitionStatus == ExhibitionStatus.RequireAmendment)
			{
				var approval = new EventExhibitionRequestApproval
				{
					ExhibitionId = exroad.Id,
					Level = EventApprovalLevel.Verifier,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					ApprovedDate = DateTime.Now,
					Remark = "",
					RequireNext = false
				};

				db.EventExhibitionRequestApproval.Add(approval);
			}
			db.SaveChanges();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.PendingVerified;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/Verified")]
		public IHttpActionResult Verified(int id)
		{
			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.Verified;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}


		[Route("api/eEvent/ExhibitionRoadshowRequest/Reject")]
		public IHttpActionResult Reject(int id)
		{
			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.RequireAmendment;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		//First Approved Public Event 
		[Route("api/eEvent/ExhibitionRoadshowRequest/FirstApproved")]
		public IHttpActionResult FirstApproved(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.ApprovedByApprover1;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		//Second Approved Public Event 
		[Route("api/eEvent/ExhibitionRoadshowRequest/SecondApproved")]
		public IHttpActionResult SecondApproved(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.ApprovedByApprover2;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		//Final Approved Public Event 
		[Route("api/eEvent/ExhibitionRoadshowRequest/FinalApproved")]
		public IHttpActionResult FinalApproved(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.ApprovedByApprover3;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/SubmitDutyRoster")]
		public IHttpActionResult SubmitDutyRoster(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.SubmitVerifyDutyRoster;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/VerifiedDutyRoster")]
		public IHttpActionResult VerifiedDutyRoster(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.VerifiedDutyRoster;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/RejectDutyRoster")]
		public IHttpActionResult RejectDutyRoster(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.RequireAmendmentDutyRoster;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/ApproveDutyRoster")]
		public IHttpActionResult ApproveDutyRoster(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.ApproveDutyRoster;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/AcceptParticipation")]
		public IHttpActionResult AcceptParticipation(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.AcceptParticipation;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/DeclineParticipation")]
		public IHttpActionResult DeclineParticipation(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.DeclineParticipation;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/SendInvitationToNominees")]
		public IHttpActionResult SendInvitationToNominees(int id)
		{

			var exroad = db.EventExhibitionRequest.Where(p => p.Id == id).FirstOrDefault();

			if (exroad != null)
			{
				exroad.ExhibitionStatus = ExhibitionStatus.NomineesInvited;
				db.EventExhibitionRequest.Attach(exroad);
				db.Entry(exroad).Property(m => m.ExhibitionStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				ExhibitionRoadshowRequestModel model = new ExhibitionRoadshowRequestModel
				{
					Id = exroad.Id,
					EventName = exroad.EventName,
					RefNo = exroad.RefNo,
					ExhibitionStatus = exroad.ExhibitionStatus,
					ExhibitionStatusDesc = exroad.ExhibitionStatus.GetDisplayName()
				};

				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/GetHistory")]
		public List<ExhibitionApprovalHistoryModel> GetHistory(int id)
		{
			var phistory = db.EventExhibitionRequestApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { pa.ExhibitionId, pa.Level, pa.ApproverId, pa.ApprovedDate, pa.Status, pa.Remark, UserName = u.Name })
				.Where(pa => pa.ExhibitionId == id && pa.Status != EventApprovalStatus.None).OrderByDescending(pa => pa.ApprovedDate).Select(s => new ExhibitionApprovalHistoryModel
			{
				Level = s.Level,
				ApproverId = s.ApproverId,
				ApprovalDate = s.ApprovedDate,
				UserName = s.UserName,
				Status = s.Status,
				Remarks = s.Remark
			}).ToList();

			return phistory;
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/UpdateApproval")]
		[HttpPost]
		[ValidationActionFilter]
		public string UpdateApproval([FromBody] ExhibitionRoadshowApprovalModel model)
		{

			if (ModelState.IsValid)
			{
				var papproval = db.EventExhibitionRequestApproval.Where(pa => pa.Id == model.approval.Id).FirstOrDefault();

				if (papproval != null)
				{
					papproval.ApproverId = model.approval.ApproverId;
					papproval.Status = model.approval.Status;
					papproval.ApprovedDate = DateTime.Now;
					papproval.Remark = model.approval.Remarks;
					papproval.RequireNext = model.approval.RequireNext;
					// requirenext is always set to true when coming from verifier approval, and always false from approver3

					db.Entry(papproval).State = EntityState.Modified;
					// HERE
					db.SaveChanges();

					var publicevent = db.EventExhibitionRequest.Where(p => p.Id == papproval.ExhibitionId).FirstOrDefault();
					if (publicevent != null)
					{

						// proceed depending on requirenext
						if (model.approval.RequireNext == true)
						{

							EventApprovalLevel nextlevel;
							switch (papproval.Level)
							{
								case EventApprovalLevel.Verifier:
									nextlevel = EventApprovalLevel.Approver1;

									break;
								case EventApprovalLevel.Approver1:
									nextlevel = EventApprovalLevel.Approver2;
									break;
								case EventApprovalLevel.Approver2:
									nextlevel = EventApprovalLevel.Approver3;
									break;
								default:
									nextlevel = EventApprovalLevel.Approver1;
									break;
							}
							if (papproval.Status != EventApprovalStatus.Rejected)
							{
								// create next approval record
								var pnewapproval = new EventExhibitionRequestApproval
								{
									ExhibitionId = papproval.ExhibitionId,
									Level = nextlevel,
									ApproverId = 0,
									Status = EventApprovalStatus.None,
									ApprovedDate = DateTime.Now,
									Remark = "",
									RequireNext = false
								};

								db.EventExhibitionRequestApproval.Add(pnewapproval);
								// HERE
								db.SaveChanges();
							}
						}



						//return publication.Title;
						return publicevent.Id + "|" + publicevent.EventName + "|" + publicevent.RefNo +  "|" + publicevent.ExhibitionStatus;
					}
				}
			}

			return "";
		}



		[HttpGet]
		[Route("api/eEvent/ExhibitionRoadshowRequest/GetEditDelete")]
		public IHttpActionResult GetEditDelete(int id)
		{
			var exhibition = db.EventExhibitionRequest.Where(u => u.Id == id)
				.Select(s => new DetailsExhibitionRoadshowRequestModel
				{
					Id = s.Id,
					EventName = s.EventName,
					Organiser = s.Organiser,
					OrganiserEmail = s.OrganiserEmail,
					AddressStreet1 = s.AddressStreet1,
					AddressStreet2 = s.AddressStreet2,
					AddressPoscode = s.AddressPoscode,
					AddressCity = s.AddressCity,
					State = s.State,
					StartDate = s.StartDate,
					EndDate = s.EndDate,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					ExhibitionStatus = s.ExhibitionStatus,
					ParticipationRequirement = s.ParticipationRequirement,
					ReceivedById = s.ReceivedById,
					ReceivedByName = s.ReceivedBy.Name,
					ReceivedDate = s.ReceivedDate,
					Receive_Via = s.Receive_Via,
					RefNo = s.RefNo,
					CreatedDate = s.CreatedDate,
					CreatedBy = s.CreatedBy,
					CreatedByName = s.CreatedByUser.Name,
				}).FirstOrDefault();

			if (exhibition == null)
			{
				return NotFound();
			}

			exhibition.NomineeId = db.ExhibitionNominee.Where(u => u.ExhibitionRoadshowId == id).Select(s => s.UserId).ToArray();
			exhibition.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.ExhibitionRoadshow && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

			return Ok(exhibition);
		}









	}
}
