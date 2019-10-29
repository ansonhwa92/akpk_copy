//using FEP.Helper;
//using FEP.Model;
//using FEP.WebApiModel.eEvent;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace FEP.WebApi.Api.eEvent
//{
//	[Route("api/eEvent/EventRefund")]
//	public class EventRefundController : ApiController
//    {
//		private DbEntities db = new DbEntities();

//		protected override void Dispose(bool disposing)
//		{
//			if (disposing)
//			{
//				db.Dispose();
//			}
//			base.Dispose(disposing);
//		}

//		[HttpGet]
//		public IHttpActionResult Get()
//		{
//			var users = db.EventRefund.Where(u => u.Display).Select(s => new EventRefundModel
//			{
//				Id = s.Id,
//				EventId = s.EventId,
//				EventName = s.PublicEvent.RefNo,
//				BankInformationId = s.BankInformationId,
//				BankInformationName = s.BankInformation.FullName,

//			}).ToList();

//			return Ok(users);
//		}




//		[Route("api/eEvent/EventRefund/GetRefundList")]
//		[HttpPost]
//		public IHttpActionResult Post(FilterEventRefundModel request)
//		{

//			var query = db.EventRefund.Where(u => u.Display);

//			var totalCount = query.Count();

//			//advance search
//			query = query.Where(s => (request.AccountNumber == null || s.AccountNumber.Contains(request.AccountNumber))
//			   && (request.EventId == null || s.EventId == request.EventId)
//			   && (request.UserId == null || s.UserId == request.UserId)
//			   //&& (request.EventStatus.GetDisplayName() == null || s.EventStatus.GetDisplayName() == request.EventStatus.GetDisplayName())
//			   );

//			//quick search 
//			if (!string.IsNullOrEmpty(request.search.value))
//			{
//				var value = request.search.value.Trim();

//				query = query.Where(p => p.AccountNumber.Contains(value)
//				//|| p.EventCategory.CategoryName.Contains(value)
//				//|| p.TargetedGroup.GetDisplayName().Contains(value)
//				//|| p.EventStatus.GetDisplayName().Contains(value)
//				);
//			}

//			var filteredCount = query.Count();

//			//order
//			if (request.order != null)
//			{
//				string sortBy = request.columns[request.order[0].column].data;
//				bool sortAscending = request.order[0].dir.ToLower() == "asc";

//				switch (sortBy)
//				{
//					case "EventTitle":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.EventTitle);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.EventTitle);
//						}

//						break;

//					case "RefNo":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.RefNo);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.RefNo);
//						}

//						break;

//					case "EventCategoryId":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.EventCategory.CategoryName);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.EventCategory.CategoryName);
//						}

//						break;

//					case "StartDate":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.StartDate);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.StartDate);
//						}

//						break;

//					case "EndDate":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.EndDate);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.EndDate);
//						}

//						break;

//					case "Venue":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.Venue);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.Venue);
//						}

//						break;

//					case "Fee":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.Fee);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.Fee);
//						}

//						break;

//					case "EventStatus":

//						if (sortAscending)
//						{
//							query = query.OrderBy(o => o.EventStatus);
//						}
//						else
//						{
//							query = query.OrderByDescending(o => o.EventStatus);
//						}

//						break;

//					default:
//						query = query.OrderByDescending(o => o.EventTitle);
//						break;
//				}

//			}
//			else
//			{
//				query = query.OrderByDescending(o => o.EventTitle);
//			}

//			var data = query.Skip(request.start).Take(request.length)
//				.Select(s => new EventRefundModel
//				{
//					Id = s.Id,
//					EventTitle = s.EventTitle,
//					EventCategoryId = s.EventCategoryId,
//					EventCategoryName = s.EventCategory.CategoryName,
//					TargetedGroup = s.TargetedGroup,
//					EventStatus = s.EventStatus,
//					//EventStatusDesc = s.EventStatus.GetDisplayName(),
//					StartDate = s.StartDate,
//					EndDate = s.EndDate,
//					EventObjective = s.EventObjective,
//					Venue = s.Venue,
//					Fee = s.Fee,
//					RefNo = s.RefNo
//				}).ToList();

//			data.ForEach(s => s.EventStatusDesc = s.EventStatus.GetDisplayName());

//			return Ok(new DataTableResponse
//			{
//				draw = request.draw,
//				recordsTotal = totalCount,
//				recordsFiltered = filteredCount,
//				data = data.ToArray()
//			});
//		}

//		public IHttpActionResult Get(int id)
//		{
//			var model = db.EventRefund.Where(i => i.Display && i.Id == id)
//				.Select(i => new DetailsEventRefundModel
//				{
//					Id = i.Id,
//					EventId = i.EventId,
//					EventName = i.PublicEvent.RefNo,
//					AccountNumber = i.AccountNumber,
//					BankInformationId = i.BankInformationId,
//					BankInformationName = i.BankInformation.FullName,
//					UserId = i.UserId,
//					UserName = i.User.Name,
//				}).FirstOrDefault();

//			if (model == null)
//			{
//				return NotFound();
//			}

//			return Ok(model);
//		}

//		[HttpPost]
//		public IHttpActionResult Post([FromBody] CreateEventRefundModel model)
//		{
//			var refund = new EventRefund
//			{
//				EventId = model.EventId,
//				BankInformationId = model.BankInformationId,
//				UserId = model.UserId,
//				AccountNumber = model.AccountNumber,
//				CreatedBy = null,
//				Display = true,
//				CreatedDate = DateTime.Now,
//			};

//			db.EventRefund.Add(refund);
//			db.SaveChanges();

//			return Ok(refund.Id);
//		}


//		public IHttpActionResult Delete(int id)
//		{
//			var refund = db.EventRefund.Where(r => r.Id == id && r.Display).FirstOrDefault();

//			if (refund == null)
//			{
//				return NotFound();
//			}

//			refund.Display = false;
//			db.Entry(refund).State = EntityState.Modified;

//			db.SaveChanges();
//			return Ok(true);
//		}

//	}
//}
