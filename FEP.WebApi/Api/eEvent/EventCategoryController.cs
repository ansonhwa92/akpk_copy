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
	[Route("api/eEvent/EventCategory")]
	public class EventCategoryController : ApiController
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

		public List<EventCategoryApiModel> Get()
		{
			var model = db.EventCategory.Where(i => i.Display).Select(i => new EventCategoryApiModel
			{
				Id = i.Id,
				CategoryName = i.CategoryName
			}).ToList();

			return model;
		}

		public EventCategoryApiModel Get(int id)
		{
			var model = db.EventCategory.Where(i => i.Display && i.Id == id).Select(i => new EventCategoryApiModel
			{
				Id = i.Id,
				CategoryName = i.CategoryName
			}).FirstOrDefault();

			return model;
		}

		public HttpResponseMessage Post([FromBody]string value)
		{
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
			return response;
		}

		public HttpResponseMessage Put(int id, [FromBody]string value)
		{
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });

			return response;
		}

		public bool Delete(int id)
		{
			var model = db.EventCategory.Where(u => u.Id == id).FirstOrDefault();

			if (model != null)
			{
				model.Display = false;

				db.EventCategory.Attach(model);
				db.Entry(model).Property(m => m.Display).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;

				db.SaveChanges();

				return true;
			}

			return false;

		}
	}
}