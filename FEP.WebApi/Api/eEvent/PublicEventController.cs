using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel;
using FEP.WebApiModel.FileDocument;
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

		////List
		//public List<PublicEventModel> Get()
		//{
		//	var model = db.PublicEvent.Where(i => i.Display).Select(i => new PublicEventModel
		//	{
		//		Id = i.Id,
		//		EventTitle = i.EventTitle,
		//		EventObjective = i.EventObjective,
		//		StartDate = i.StartDate,
		//		EndDate = i.EndDate,
		//		Venue = i.Venue,
		//		Fee = i.Fee,
		//		EventStatus = i.EventStatus,
		//		EventCategoryId = i.EventCategoryId,
		//		EventCategoryName = i.EventCategory.CategoryName,
		//		TargetedGroup = i.TargetedGroup,
		//		ParticipantAllowed = i.ParticipantAllowed,
		//		Reasons = i.Reasons,
		//		Remarks = i.Remarks
		//	}).ToList();

		//	return model;
		//}

		//Details
		public IHttpActionResult Get(int id)
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
				}).FirstOrDefault();

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
            db.SaveChanges();

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
		public string SubmitToVerify(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.PendingforVerification;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//First Approved Public Event 
		[Route("api/eEvent/PublicEvent/FirstApproved")]
		public string FirstApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.VerifiedbyFirstApprover;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//Second Approved Public Event 
		[Route("api/eEvent/PublicEvent/SecondApproved")]
		public string SecondApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.VerifiedbySecondApprover;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//Final Approved Public Event 
		[Route("api/eEvent/PublicEvent/FinalApproved")]
		public string FinalApproved(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Approved;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//Reject Approved Public Event 
		[Route("api/eEvent/PublicEvent/RejectPublicEvent")]
		public string RejectPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.RejectNeedToEdit;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//Cancel Approved Public Event 
		[Route("api/eEvent/PublicEvent/CancelPublicEvent")]
		public string CancelPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Cancelled;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}

		//Published Public Event 
		[Route("api/eEvent/PublicEvent/PublishedPublicEvent")]
		public string PublishedPublicEvent(int id)
		{

			var publicevent = db.PublicEvent.Where(p => p.Id == id).FirstOrDefault();

			if (publicevent != null)
			{
				publicevent.EventStatus = EventStatus.Published;
				db.PublicEvent.Attach(publicevent);
				db.Entry(publicevent).Property(m => m.EventStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return publicevent.RefNo;
			}
			return "";
		}
	}
}
