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
	[Route("api/eEvent/EventSpeaker")]
	public class EventSpeakerController : ApiController
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

		[Route("api/eEvent/EventSpeaker/GetSpeakerList")]
		[HttpPost]
		public IHttpActionResult Post(FilterEventSpeakerModel request)
		{

			var query = db.EventSpeaker.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s =>
				(request.UserId == null || s.UserId == request.UserId)
			 && (request.SpeakerType == null || s.SpeakerType == request.SpeakerType)
			 //&& (request.DateAssigned == null || DbFunctions.TruncateTime(request.DateAssigned) == DbFunctions.TruncateTime(DateTime.Now))
			 && (request.Email == null || s.User.Email.Contains(request.Email))
			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.User.Name.Contains(value)
				|| p.SpeakerType.GetDisplayName().Contains(value)
				|| p.User.Email.Contains(value)
				//|| p.DateAssigned.ToString().Contains(value)
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
					case "UserId":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.User.Name);
						}
						else
						{
							query = query.OrderByDescending(o => o.User.Name);
						}

						break;

					case "SpeakerType":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.SpeakerType);
						}
						else
						{
							query = query.OrderByDescending(o => o.SpeakerType);
						}

						break;

					case "SpeakerStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.SpeakerStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.SpeakerStatus);
						}

						break;


					default:
						query = query.OrderByDescending(o => o.UserId);
						break;
				}
			}
			else
			{
				query = query.OrderByDescending(o => o.UserId);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(i => new EventSpeakerModel
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					SpeakerStatus = i.SpeakerStatus,
					SpeakerType = i.SpeakerType,
				}).ToList();

			data.ForEach(s => s.SpeakerTypeDesc = s.SpeakerType.GetDisplayName());
			data.ForEach(s => s.SpeakerStatusDesc = s.SpeakerStatus.GetDisplayName());

			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount,
				data = data.ToArray()
			});
		}


		public IHttpActionResult Get(int id)
		{
			var speaker = db.EventSpeaker.Where(u => u.Id == id)
				.Select(s => new DetailsEventSpeakerModel
				{
					Id = s.Id,
					UserId = s.UserId,
					SpeakerType = s.SpeakerType,
					PhoneNo = s.User.MobileNo,
					Email = s.User.Email,
					Experience = s.Experience,
					SpeakerStatus = s.SpeakerStatus,				
				}).FirstOrDefault();

			if (speaker == null)
			{
				return NotFound();
			}

			speaker.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.EventSpeaker && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

			return Ok(speaker);
		}

		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateEventSpeakerModel model)
		{
			var speaker = new EventSpeaker
			{
				UserId = model.UserId,
				SpeakerType = model.SpeakerType,
				Experience = model.Experience,
				SpeakerStatus = model.SpeakerStatus,
				CreatedBy = null,
				Display = true,
				CreatedDate = DateTime.Now,
			};

			db.EventSpeaker.Add(speaker);

            db.SaveChanges();

            //files
            foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.EventSpeaker,
					FileId = fileid,
					ParentId = speaker.Id
				};
				db.EventFile.Add(eventfile);
			}

            db.SaveChanges();

            return Ok(speaker.Id);
		}

		public IHttpActionResult Put(int id, [FromBody] EditEventSpeakerModel model)
		{
			var speaker = db.EventSpeaker.Where(u => u.Id == id).FirstOrDefault();

			if (speaker == null)
			{
				return NotFound();
			}

			speaker.UserId = model.UserId;
			speaker.SpeakerType = model.SpeakerType;
			speaker.Experience = model.Experience;
			speaker.SpeakerStatus = model.SpeakerStatus;

			db.EventSpeaker.Attach(speaker);
			db.Entry(speaker).Property(x => x.UserId).IsModified = true;
			db.Entry(speaker).Property(x => x.SpeakerType).IsModified = true;
			db.Entry(speaker).Property(x => x.Experience).IsModified = true;
			db.Entry(speaker).Property(x => x.SpeakerStatus).IsModified = true;

			db.Entry(speaker).Property(x => x.Display).IsModified = false;
			db.Entry(speaker).Property(x => x.Id).IsModified = false;


			//remove file 
			var attachments = db.EventFile.Where(s => s.FileCategory == EventFileCategory.EventSpeaker && s.ParentId == model.Id).ToList();

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

			//add files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.EventSpeaker,
					FileId = fileid,
					ParentId = speaker.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var speaker = db.EventSpeaker.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (speaker == null)
			{
				return NotFound();
			}

			speaker.Display = false;

			db.Entry(speaker).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

		[HttpGet]
		public IHttpActionResult Get()
		{
			var speakers = db.EventSpeaker.Where(u => u.Display && (u.SpeakerType == SpeakerType.Internal || u.SpeakerType == SpeakerType.External)).Select(s => new EventSpeakerModel
			{
				Id = s.Id,
				UserName = s.User.Name,
				Email = s.User.Email,
			}).ToList();

			return Ok(speakers);
		}
	}
}
