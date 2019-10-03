using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
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
			 && (request.DateAssigned == null || DbFunctions.TruncateTime(request.DateAssigned) == DbFunctions.TruncateTime(DateTime.Now))
			 && (request.Email == null || s.Email.Contains(request.Email))
			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.User.Name.Contains(value)
				|| p.SpeakerType.GetDisplayName().Contains(value)
				|| p.Email.Contains(value)
				|| p.DateAssigned.ToString().Contains(value)
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

					case "DateAssigned":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.DateAssigned);
						}
						else
						{
							query = query.OrderByDescending(o => o.DateAssigned);
						}

						break;

					case "Email":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Email);
						}
						else
						{
							query = query.OrderByDescending(o => o.Email);
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
					DateAssigned = i.DateAssigned,
					Email = i.Email,
					DateOfBirth = i.DateOfBirth,
					PhoneNo = i.PhoneNo,
					SpeakerType = i.SpeakerType,

				}).ToList();

			data.ForEach(s => s.SpeakerTypeDesc = s.SpeakerType.GetDisplayName());

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
					Religion = s.Religion,
					DateAssigned = s.DateAssigned,
					DateOfBirth = s.DateOfBirth,
					PhoneNo = s.PhoneNo,
					Email = s.Email,
					Experience = s.Experience,
					MaritialStatus = s.MaritialStatus,
					AddressStreet1 = s.AddressStreet1,
					AddressStreet2 = s.AddressStreet2,
					AddressPoscode = s.AddressPoscode,
					AddressCity = s.AddressCity,
					State = s.State,
					Remark = s.Remark,
					//SpeakerPictureName = db.SpeakerFile.Where(f => f.EventSpeakerId == s.Id && f.SpeakerFileType == SpeakerFileType.Picture).Select(i => i.FileName).FirstOrDefault(),
					//SpeakerAttachmentName = db.SpeakerFile.Where(f => f.EventSpeakerId == s.Id && f.SpeakerFileType == SpeakerFileType.Attachment).Select(i => i.FileName).FirstOrDefault()
				}).FirstOrDefault();

			if (speaker == null)
			{
				return NotFound();
			}

			return Ok(speaker);
		}

		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateEventSpeakerModel model)
		{
			var speaker = new EventSpeaker
			{
				UserId = model.UserId,
				SpeakerType = model.SpeakerType,
				Religion = model.Religion,
				DateAssigned = model.DateAssigned,
				DateOfBirth = model.DateOfBirth,
				PhoneNo = model.PhoneNo,
				Email = model.Email,
				Experience = model.Experience,
				MaritialStatus = model.MaritialStatus,
				AddressStreet1 = model.AddressStreet1,
				AddressStreet2 = model.AddressStreet2,
				AddressPoscode = model.AddressPoscode,
				AddressCity = model.AddressCity,
				State = model.State,
				Remark = model.Remark,
				CreatedBy = null,
				Display = true,
				CreatedDate = DateTime.Now,
			};
			db.EventSpeaker.Add(speaker);

			//SpeakerFile speakerFile = new SpeakerFile
			//{
			//	FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.SpeakerPictureName,
			//	UploadedDate = DateTime.Now,
			//	CreatedBy = model.UserId,
			//	EventSpeakerId = model.UserId,
			//	Display = true,
			//	SpeakerFileType = SpeakerFileType.Picture
			//};
			//db.SpeakerFile.Add(speakerFile);

			//SpeakerFile speakerAttachFile = new SpeakerFile
			//{
			//	FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.SpeakerAttachmentName,
			//	UploadedDate = DateTime.Now,
			//	CreatedBy = model.UserId,
			//	EventSpeakerId = model.UserId,
			//	Display = true,
			//	SpeakerFileType = SpeakerFileType.Attachment
			//};
			//db.SpeakerFile.Add(speakerAttachFile);


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
			speaker.Religion = model.Religion;
			speaker.DateAssigned = model.DateAssigned;
			speaker.DateOfBirth = model.DateOfBirth;
			speaker.PhoneNo = model.PhoneNo;
			speaker.Email = model.Email;
			speaker.Experience = model.Experience;
			speaker.MaritialStatus = model.MaritialStatus;
			speaker.AddressStreet1 = model.AddressStreet1;
			speaker.AddressStreet2 = model.AddressStreet2;
			speaker.AddressPoscode = model.AddressPoscode;
			speaker.AddressCity = model.AddressCity;
			speaker.State = model.State;
			speaker.Remark = model.Remark;

			db.EventSpeaker.Attach(speaker);
			db.Entry(speaker).Property(x => x.UserId).IsModified = true;
			db.Entry(speaker).Property(x => x.SpeakerType).IsModified = true;
			db.Entry(speaker).Property(x => x.Religion).IsModified = true;
			db.Entry(speaker).Property(x => x.DateAssigned).IsModified = true;
			db.Entry(speaker).Property(x => x.DateOfBirth).IsModified = true;
			db.Entry(speaker).Property(x => x.PhoneNo).IsModified = true;
			db.Entry(speaker).Property(x => x.Email).IsModified = true;
			db.Entry(speaker).Property(x => x.Experience).IsModified = true;
			db.Entry(speaker).Property(x => x.MaritialStatus).IsModified = true;
			db.Entry(speaker).Property(x => x.AddressStreet1).IsModified = true;
			db.Entry(speaker).Property(x => x.AddressStreet2).IsModified = true;
			db.Entry(speaker).Property(x => x.AddressPoscode).IsModified = true;
			db.Entry(speaker).Property(x => x.AddressCity).IsModified = true;
			db.Entry(speaker).Property(x => x.State).IsModified = true;
			db.Entry(speaker).Property(x => x.Remark).IsModified = true;


			db.Entry(speaker).Property(x => x.Display).IsModified = false;
			db.Entry(speaker).Property(x => x.Id).IsModified = false;
			db.Configuration.ValidateOnSaveEnabled = true;

			//SpeakerFile speakerFile = new SpeakerFile
			//{
			//	FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.SpeakerPictureName,
			//	UploadedDate = DateTime.Now,
			//	CreatedBy = model.UserId,
			//	EventSpeakerId = model.UserId,
			//	Display = true,
			//	SpeakerFileType = SpeakerFileType.Picture
			//};
			//db.SpeakerFile.Add(speakerFile);

			//SpeakerFile speakerAttachFile = new SpeakerFile
			//{
			//	FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.SpeakerAttachmentName,
			//	UploadedDate = DateTime.Now,
			//	CreatedBy = model.UserId,
			//	EventSpeakerId = model.UserId,
			//	Display = true,
			//	SpeakerFileType = SpeakerFileType.Attachment
			//};
			//db.SpeakerFile.Add(speakerAttachFile);

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
				Email = s.Email,
			}).ToList();

			return Ok(speakers);
		}
	}
}
