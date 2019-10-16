using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using FEP.WebApiModel;
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
				UserName = i.User.Name,
				//RepDesignation = i.User.Designation,
				RepEmail = i.User.Email,
				RepMobileNumber = i.User.MobileNo,
				ContactPerson = i.ContactPerson,
			}).ToList();

			return model;
		}

		//public MediaInterviewRequestApiModel Get(int id)
		//{
		//	var model = db.EventMediaInterviewRequest.Where(i => i.Display && i.Id == id).Select(i => new MediaInterviewRequestApiModel
		//	{
		//		Id = i.Id,
		//		MediaName = i.MediaName,
		//		MediaType = i.MediaType,
		//		ContactNo = i.ContactNo,
		//		AddressStreet1 = i.AddressStreet1,
		//		AddressStreet2 = i.AddressStreet2,
		//		AddressPoscode = i.AddressPoscode,
		//		AddressCity = i.AddressCity,
		//		State = i.State,
		//		Email = i.Email,
		//		DateStart = i.DateStart,
		//		DateEnd = i.DateEnd,
		//		Time = i.Time,
		//		Location = i.Location,
		//		Language = i.Language,
		//		Topic = i.Topic,
		//		UserId = i.UserId,
		//		UserName = i.User.Name,
		//		//RepDesignation = i.User.Designation,
		//		RepEmail = i.User.Email,
		//		RepMobileNumber = i.User.MobileNo,
		//		ContactPerson = i.ContactPerson,
		//		CreatedBy = i.CreatedBy,
		//		CreatedDate = i.CreatedDate
		//	}).FirstOrDefault();

		//	return model;
		//}



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
					UserName = i.User.Name,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					ContactPerson = i.ContactPerson,
					CreatedBy = i.CreatedBy,
					CreatedDate = i.CreatedDate
				}).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}

			return Ok(media);
		}




		public HttpResponseMessage Post([FromBody]string value)
		{
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
			return response;
		}

		//Edit
		public HttpResponseMessage Put(int id, [FromBody]string value)
		{
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });

			return response;
		}

		//Delete
		public bool Delete(int id)
		{
			var model = db.EventMediaInterviewRequest.Where(u => u.Id == id).FirstOrDefault();

			if (model != null)
			{
				model.Display = false;

				db.EventMediaInterviewRequest.Attach(model);
				db.Entry(model).Property(m => m.Display).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;

				db.SaveChanges();

				return true;
			}

			return false;
		}


		//Submit Public Event for Verification
		[Route("api/eEvent/MediaInterviewRequest/SubmitToVerify")]
		public string SubmitToVerify(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.PendingVerified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}

		[Route("api/eEvent/MediaInterviewRequest/Verified")]
		public string Verified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.Verified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}


		[Route("api/eEvent/MediaInterviewRequest/RejectVerified")]
		public string RejectVerified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.NotVerified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}

		//First Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FirstApproved")]
		public string FirstApproved(int id)
		{

			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover1;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}

		//Second Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/SecondApproved")]
		public string SecondApproved(int id)
		{

			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover2;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}

		//Final Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FinalApproved")]
		public string FinalApproved(int id)
		{

			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover3;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				//return model.Title;
				return media.RefNo;
			}
			return "";
		}


		[Route("api/eEvent/MediaInterviewRequest/Evaluate")]
		[HttpPost]
		[ValidationActionFilter]
		public string Evaluate([FromBody] MediaInterviewApprovalModel model, int id)
		{
			if (ModelState.IsValid)
			{
				var mediaapproval = db.EventMediaInterviewApproval.Where(x => x.Id == id).FirstOrDefault();

				if (mediaapproval != null)
				{
					mediaapproval.ApproverId = model.ApproverId;
					mediaapproval.Status = model.Status;
					mediaapproval.ApprovedDate = DateTime.Now;
					mediaapproval.Remark = model.Remarks;
					mediaapproval.RequireNext = model.RequireNext;
					mediaapproval.MediaId = id;
					mediaapproval.Level = model.Level;

					db.Entry(mediaapproval).State = EntityState.Modified;
					db.SaveChanges();

				}
			}
			return "";
		}
	}
}

