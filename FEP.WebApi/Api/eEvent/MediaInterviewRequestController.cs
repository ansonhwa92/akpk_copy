using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using FEP.WebApiModel.MediaInterview;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/MediaInterviewRequest")]
	public class MediaInterviewRequestController : ApiController
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

		[Route("api/eEvent/MediaInterviewRequest/GetMediaList")]
		[HttpPost]
		public IHttpActionResult Post(FilterMediaInterviewRequestApiModel request)
		{
			var query = db.EventMediaInterviewRequest.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.MediaName == null || s.MediaName.Contains(request.MediaName))

			   );

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.MediaName.Contains(value)
				//|| p.ICNo.Contains(value)
				//|| p.Email.Contains(value)
				//|| p.MobileNo.Contains(value)
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

					case "MediaName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaName);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaName);
						}

						break;

					case "MediaType":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaType);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaType);
						}

						break;

					case "ContactPerson":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.ContactPerson);
						}
						else
						{
							query = query.OrderByDescending(o => o.ContactPerson);
						}

						break;

					case "DateStart":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.DateStart);
						}
						else
						{
							query = query.OrderByDescending(o => o.DateStart);
						}

						break;

					case "DateEnd":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.DateEnd);
						}
						else
						{
							query = query.OrderByDescending(o => o.DateEnd);
						}

						break;

					case "MediaStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.MediaName);
						break;
				}
			}
			else
			{
				query = query.OrderByDescending(o => o.MediaName);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(s => new MediaInterviewRequestApiModel
				{
					Id = s.Id,
					RefNo = s.RefNo,
					MediaName = s.MediaName,
					MediaType = s.MediaType,
					ContactPerson = s.ContactPerson,
					DateStart = s.DateStart,
					DateEnd = s.DateEnd,
					MediaStatus = s.MediaStatus
				}).ToList();

			data.ForEach(s =>
			{
				s.MediaTypeDesc = s.MediaType.GetDisplayName();
				s.MediaStatusDesc = s.MediaStatus.GetDisplayName();
			});

			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount,
				data = data.ToArray()
			});
		}

		public List<MediaInterviewRequestApiModel> Get()
		{
			var model = db.EventMediaInterviewRequest.Where(i => i.Display).Select(i => new MediaInterviewRequestApiModel
			{
				Id = i.Id,
				MediaName = i.MediaName,
				MediaType = i.MediaType,
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
				Location = i.Location,
				Language = i.Language,
				Topic = i.Topic,
				UserId = i.UserId,
				RepUserName = i.User.Name,
				RepEmail = i.User.Email,
				RepMobileNumber = i.User.MobileNo,
				ContactPerson = i.ContactPerson,
			}).ToList();

			return model;
		}

		public IHttpActionResult Get(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id)
				.Select(i => new DetailsMediaInterviewRequestApiModel
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
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
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					UserId = i.UserId,
					RepUserName = i.User.Name,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					ContactPerson = i.ContactPerson,
					CreatedBy = i.CreatedBy,
					CreatedDate = i.CreatedDate,
					RefNo = i.RefNo,
					MediaStatus = i.MediaStatus,
					
				}).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}

			media.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.ExhibitionRoadshow && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

			return Ok(media);
		}


		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateMediaInterviewRequestApiModel model)
		{
			var media = new EventMediaInterviewRequest
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
				UserId = model.UserId,
				CreatedBy = null,
				CreatedDate = DateTime.Now,
				Display = true,
				MediaStatus = MediaStatus.New
			};

			db.EventMediaInterviewRequest.Add(media);
			db.SaveChanges();

			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.PublicEvent,
					FileId = fileid,
					ParentId = media.Id
				};

				db.EventFile.Add(eventfile);
			}
			db.SaveChanges();

			var refno = "EVT/" + DateTime.Now.ToString("yyMM");
			refno += "/" + media.Id.ToString("D4");
			media.RefNo = refno;

			db.Entry(media).State = EntityState.Modified;
			db.SaveChanges();

			return Ok(media.Id);
		}

		//Edit
		public IHttpActionResult Put(int id, [FromBody] EditMediaInterviewRequestApiModel model)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}

			media.MediaName = model.MediaName;
			media.MediaType = model.MediaType;
			media.ContactPerson = model.ContactPerson;
			media.ContactNo = model.ContactNo;
			media.AddressStreet1 = model.AddressStreet1;
			media.AddressStreet2 = model.AddressStreet2;
			media.AddressPoscode = model.AddressPoscode;
			media.AddressCity = model.AddressCity;
			media.State = model.State;
			media.Email = model.Email;
			media.DateStart = model.DateStart;
			media.DateEnd = model.DateEnd;
			media.Time = model.Time;
			media.Language = model.Language;
			media.Topic = model.Topic;
			media.UserId = model.UserId;
			media.MediaStatus = model.MediaStatus;
			media.RefNo = model.RefNo;

			db.Entry(media).State = EntityState.Modified;
			db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
			db.Entry(media).Property(x => x.Display).IsModified = false;

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
					ParentId = media.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();
			return Ok(true);
		}

		//Delete
		public IHttpActionResult Delete(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}
			media.Display = false;
			db.Entry(media).State = EntityState.Modified;
			db.SaveChanges();
			return Ok(true);
		}


		//Submit Public Event for Verification
		[Route("api/eEvent/MediaInterviewRequest/SubmitToVerify")]
		public IHttpActionResult SubmitToVerify(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();
			if (media != null)
			{
				media.MediaStatus = MediaStatus.PendingVerified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/MediaInterviewRequest/Verified")]
		public IHttpActionResult Verified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();
			if (media != null)
			{
				media.MediaStatus = MediaStatus.Verified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}


		[Route("api/eEvent/MediaInterviewRequest/RejectVerified")]
		public IHttpActionResult RejectVerified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.NotVerified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//First Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FirstApproved")]
		public IHttpActionResult FirstApproved(int id)
		{

			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover1;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//Second Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/SecondApproved")]
		public IHttpActionResult SecondApproved(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover2;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//Final Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FinalApproved")]
		public IHttpActionResult FinalApproved(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover3;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/MediaInterviewRequest/GetSLAId")]
		public IHttpActionResult GetSLAId(int id)
		{
			var publicevent = db.EventMediaInterviewRequest.Where(p => p.Id == id).Select(i => i.SLAReminderStatusId).FirstOrDefault();

			return Ok(publicevent);
		}

		//[Route("api/eEvent/MediaInterviewRequest/Evaluate")]
		//[HttpPost]
		//[ValidationActionFilter]
		//public string Evaluate([FromBody] MediaInterviewApprovalModel model, int id)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var mediaapproval = db.EventMediaInterviewApproval.Where(x => x.Id == id).FirstOrDefault();

		//		if (mediaapproval != null)
		//		{
		//			mediaapproval.ApproverId = model.ApproverId;
		//			mediaapproval.Status = model.Status;
		//			mediaapproval.ApprovedDate = DateTime.Now;
		//			mediaapproval.Remark = model.Remarks;
		//			mediaapproval.RequireNext = model.RequireNext;
		//			mediaapproval.MediaId = id;
		//			mediaapproval.Level = model.Level;

		//			db.Entry(mediaapproval).State = EntityState.Modified;
		//			db.SaveChanges();

		//		}
		//	}
		//	return "";
		//}
	}
}

