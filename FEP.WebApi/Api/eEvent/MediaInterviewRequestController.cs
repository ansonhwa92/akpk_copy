using FEP.Model;
using FEP.WebApiModel;
using System;
using System.Collections.Generic;
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
				Designation = i.Designation,
				UserId = i.UserId,
				UserName = i.User.Name,
				ContactPerson = i.ContactPerson,
			}).ToList();

			return model;
		}

		public MediaInterviewRequestApiModel Get(int id)
		{
			var model = db.EventMediaInterviewRequest.Where(i => i.Display && i.Id == id).Select(i => new MediaInterviewRequestApiModel
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
				Designation = i.Designation,
				UserId = i.UserId,
				UserName = i.User.Name,
				ContactPerson = i.ContactPerson,
				CreatedBy = i.CreatedBy,
				CreatedDate = i.CreatedDate
			}).FirstOrDefault();

			return model;
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

	}
}

