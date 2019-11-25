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
	[Route("api/eEvent/EventAttendent")]
	public class EventAttendentController : ApiController
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


		[Route("api/eEvent/EventAttendent/GetAttendentList")]
		[HttpPost]
		public IHttpActionResult Post(FilterEventAttendentModel request,int? id)
		{
			var query = db.EventAttendance.Where(i => i.Display && i.EventId == id);
			var totalcount = query.Count();

			//advance search
			query = query.Where(e =>
			   (request.AttendeeName == null || e.AttendeeName.Contains(request.AttendeeName))
			//&& (request.UserType.GetDisplayName() == null || e.UserType.GetDisplayName().Contains(request.UserType.GetDisplayName()))
			&& (request.CompanyName == null || e.User.CompanyProfile.CompanyName.Contains(request.CompanyName))
			&& (request.BookingNumber == null || e.BookingNumber.Contains(request.BookingNumber))
			&& (request.ICNo == null || e.ICNo.Contains(request.ICNo))
			//&& (request.CheckInStatus.GetDisplayName() == null || e.CheckInStatus.GetDisplayName().Contains(request.CheckInStatus.GetDisplayName()))
			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.AttendeeName.Contains(value)
				//|| p.UserType.GetDisplayName().Contains(value)
				|| p.CompanyName.Contains(value)
				|| p.BookingNumber.Contains(value)
				|| p.ICNo.Contains(value)
				//|| p.CheckInStatus.GetDisplayName().Contains(value)
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
					case "AttendeeName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.AttendeeName);
						}
						else
						{
							query = query.OrderByDescending(o => o.AttendeeName);
						}

						break;

					case "UserType":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.UserType);
						}
						else
						{
							query = query.OrderByDescending(o => o.UserType);
						}

						break;

					case "CompanyName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.CompanyName);
						}
						else
						{
							query = query.OrderByDescending(o => o.CompanyName);
						}

						break;

					case "BookingNumber":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.BookingNumber);
						}
						else
						{
							query = query.OrderByDescending(o => o.BookingNumber);
						}

						break;

					case "ICNo":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.ICNo);
						}
						else
						{
							query = query.OrderByDescending(o => o.ICNo);
						}

						break;

					case "CheckInStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.CheckInStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.CheckInStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.AttendeeName);
						break;
				}

			}
			else
			{
				query = query.OrderByDescending(o => o.User.Name);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(s => new EventAttendentModel
				{
					Id = s.Id,
					AttendeeName = s.AttendeeName,
					CompanyName = s.CompanyName,
					BookingNumber = s.BookingNumber,
					ICNo = s.ICNo,
					CheckInStatus = s.CheckInStatus,
					UserType = s.UserType,
					EventId = s.EventId,
					EventName = s.Event.EventTitle,
				}).ToList();

			data.ForEach(s =>
			{
				s.UserTypeDesc = s.UserType.GetDisplayName();
				s.CheckInStatusDesc = s.CheckInStatus.GetDisplayName();
			});

			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalcount,
				recordsFiltered = filteredCount,
				data = data.ToArray(),
				
			});
		}


		public IHttpActionResult Get(int id)
		{
			var model = db.EventAttendance.Where(i => i.Display && i.Id == id)
				.Select(i => new DetailsEventAttendentModel
				{
					Id = i.Id,
					UserId = i.UserId,
					AttendeeName = i.AttendeeName,
					UserType = i.UserType,
					CompanyName = i.CompanyName,
					BookingNumber = i.BookingNumber,
					ICNo = i.ICNo,
					CheckInStatus = i.CheckInStatus,
					EventId = i.EventId,
					EventName = i.Event.RefNo,
					CreatedDate = i.CreatedDate,
					CreatedBy = i.CreatedBy,
					Display = i.Display,
				}).FirstOrDefault();

			if (model == null)
			{
				return NotFound();
			}

			return Ok(model);
		}


		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateEventAttendentModel model)
		{

			var attendance = new EventAttendance
			{
				AttendeeName = model.AttendeeName,
				UserId = model.UserId,
				UserType = model.UserType,
				CompanyName = model.CompanyName,
				BookingNumber = model.BookingNumber,
				ICNo = model.ICNo,
				CheckInStatus = model.CheckInStatus,
				EventId = model.EventId,
				CreatedBy = model.CreatedBy,
				CreatedDate = model.CreatedDate,
				Display = model.Display,
			};
			db.EventAttendance.Add(attendance);
			db.SaveChanges();

			return Ok(attendance.Id);
		}

		public IHttpActionResult Put(int id, [FromBody] EditEventAttendentModel model)
		{
			var attendance = db.EventAttendance.Where(u => u.Id == id).FirstOrDefault();

			if (attendance == null)
			{
				return NotFound();
			}

			attendance.AttendeeName = model.AttendeeName;
			attendance.UserType = model.UserType;
			attendance.CompanyName = model.CompanyName;
			attendance.BookingNumber = model.BookingNumber;
			attendance.ICNo = model.ICNo;
			attendance.CheckInStatus = model.CheckInStatus;
			attendance.EventId = model.EventId;

			db.Entry(attendance).State = EntityState.Modified;
			db.Entry(attendance).Property(x => x.CreatedDate).IsModified = false;
			db.Entry(attendance).Property(x => x.CreatedBy).IsModified = false;
			db.Entry(attendance).Property(x => x.Display).IsModified = false;

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var attendance = db.EventAttendance.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (attendance == null)
			{
				return NotFound();
			}

			attendance.Display = false;
			db.Entry(attendance).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}




	}
}
