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

		//List
		public List<PublicEventApiModel> Get()
		{
			var model = db.PublicEvent.Where(i => i.Display).Select(i => new PublicEventApiModel
			{
				Id = i.Id,
				EventTitle = i.EventTitle,
				EventObjective = i.EventObjective,
				StartDate = i.StartDate,
				EndDate = i.EndDate,
				Venue = i.Venue,
				Fee = i.Fee,
				EventStatus = i.EventStatus,
				EventCategoryId = i.CategoryId,
				EventCategoryName = i.EventCategory.CategoryName,
				TargetedGroup = i.TargetedGroup,
				ParticipantAllowed = i.ParticipantAllowed,
				Reasons = i.Reasons,
				Remarks = i.Remarks
			}).ToList();

			return model;
		}

		//Details
		public PublicEventApiModel Get(int id)
		{
			var model = db.PublicEvent.Where(i => i.Display && i.Id == id).Select(i => new PublicEventApiModel
			{
				Id = i.Id,
				EventTitle = i.EventTitle,
				EventObjective = i.EventObjective,
				StartDate = i.StartDate,
				EndDate = i.EndDate,
				Venue = i.Venue,
				Fee = i.Fee,
				EventStatus = i.EventStatus,
				EventCategoryId = i.CategoryId,
				EventCategoryName = i.EventCategory.CategoryName,
				TargetedGroup = i.TargetedGroup,
				ParticipantAllowed = i.ParticipantAllowed,
				Reasons = i.Reasons,
				Remarks = i.Remarks,
				Display = i.Display,
				CreatedBy = i.CreatedBy,
				CreatedDate = i.CreatedDate
			}).FirstOrDefault();

			return model;
		}

		//Create
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
			var model = db.PublicEvent.Where(u => u.Id == id).FirstOrDefault();

			if (model != null)
			{
				model.Display = false;

				db.PublicEvent.Attach(model);
				db.Entry(model).Property(m => m.Display).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;

				db.SaveChanges();

				return true;
			}

			return false;

		}
	}
}
