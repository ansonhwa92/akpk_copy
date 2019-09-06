using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel;
using FEP.WebApiModel.PublicEvent;
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

		[Route("api/eEvent/PublicEvent/GetEventList")]
		[HttpPost]
		public IHttpActionResult Post(FilterPublicEventModel request)
		{

			var query = db.PublicEvent.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.EventTitle == null || s.EventTitle.Contains(request.EventTitle))
			   //&& (request.EventCategoryId == null || s.EventCategoryId == request.EventCategoryId)
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

					case "EventObjective":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventObjective);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventObjective);
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
		public PublicEventModel Get(int id)
		{
			var model = db.PublicEvent.Where(i => i.Display && i.Id == id).Select(i => new PublicEventModel
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
