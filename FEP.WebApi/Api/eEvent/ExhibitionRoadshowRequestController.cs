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
	[Route("api/eEvent/ExhibitionRoadshowRequest")]
	public class ExhibitionRoadshowRequestController : ApiController
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

		[Route("api/eEvent/ExhibitionRoadshowRequest/GetExhibitionList")]
		[HttpPost]
		public IHttpActionResult Post(FilterExhibitionRoadshowRequestModel request)
		{

			var query = db.EventExhibitionRequest.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.EventName == null || s.EventName.Contains(request.EventName))

			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.EventName.Contains(value)

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
					case "EventName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventName);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventName);
						}

						break;

					case "Organiser":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Organiser);
						}
						else
						{
							query = query.OrderByDescending(o => o.Organiser);
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


					case "ExhibitionStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.ExhibitionStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.ExhibitionStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.EventName);
						break;
				}

			}
			else
			{
				query = query.OrderByDescending(o => o.EventName);
			}


			var data = query.Skip(request.start).Take(request.length)
				.Select(i => new ExhibitionRoadshowRequestModel
				{
					Id = i.Id,
					EventName = i.EventName,
					Organiser = i.Organiser,
					Location = i.Location,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					StartTime = i.StartTime,
					EndTime = i.EndTime,
					ExhibitionStatus = i.ExhibitionStatus,
					ParticipationRequirement = i.ParticipationRequirement,
					ReceivedById = i.ReceivedById,
					ReceivedDate = i.ReceivedDate,
					Receive_Via = i.Receive_Via,
					NomineeId = i.NomineeId
				}).ToList();

			data.ForEach(s => s.ExhibitionStatusDesc = s.ExhibitionStatus.GetDisplayName());


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
			var exhibition = db.EventExhibitionRequest.Where(u => u.Id == id)
				.Select(s => new DetailsExhibitionRoadshowRequestModel
				{
					Id = s.Id,
					EventName = s.EventName,
					Organiser = s.Organiser,
					Location = s.Location,
					StartDate = s.StartDate,
					EndDate = s.EndDate,
					StartTime = s.StartTime,
					EndTime = s.EndTime,
					ExhibitionStatus = s.ExhibitionStatus,
					ParticipationRequirement = s.ParticipationRequirement,
					ReceivedById = s.ReceivedById,
					ReceivedByName = s.ReceivedBy.Name,
					ReceivedDate = s.ReceivedDate,
					Receive_Via = s.Receive_Via,
					NomineeId = s.NomineeId,
					NomineeName = s.Nominee.Name
				}).FirstOrDefault();

			if (exhibition == null)
			{
				return NotFound();
			}

			return Ok(exhibition);
		}

		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateExhibitionRoadshowRequestModel model)
		{
			var exroad = new EventExhibitionRequest
			{
				EventName = model.EventName,
				Organiser = model.Organiser,
				Location = model.Location,
				StartDate = model.StartDate,
				EndDate = model.EndDate,
				StartTime = model.StartTime,
				EndTime = model.EndTime,
				ExhibitionStatus = ExhibitionStatus.New,
				ParticipationRequirement = model.ParticipationRequirement,
				ReceivedById = model.ReceivedById,
				ReceivedDate = model.ReceivedDate,
				Receive_Via = model.Receive_Via,
				Display = true,
				CreatedDate = DateTime.Now,
				NomineeId = model.NomineeId
			};
			db.EventExhibitionRequest.Add(exroad);
			db.SaveChanges();

			//save refno exhibition roadshow request
			var refno = "EVT/" + DateTime.Now.ToString("yyMM");
			refno += "/" + exroad.Id.ToString("D4");
			exroad.RefNo = refno;

			db.Entry(exroad).State = EntityState.Modified;
			db.SaveChanges();

			return Ok(exroad.Id);
		}


		public IHttpActionResult Put(int id, [FromBody] EditExhibitionRoadshowRequestModel model)
		{
			var exroad = db.EventExhibitionRequest.Where(u => u.Id == id).FirstOrDefault();

			if (exroad == null)
			{
				return NotFound();
			}

			exroad.EventName = model.EventName;
			exroad.Organiser = model.Organiser;
			exroad.Location = model.Location;
			exroad.StartDate = model.StartDate;
			exroad.EndDate = model.EndDate;
			exroad.StartTime = model.StartTime;
			exroad.EndTime = model.EndTime;
			exroad.ExhibitionStatus = model.ExhibitionStatus;
			exroad.ParticipationRequirement = model.ParticipationRequirement;
			exroad.ReceivedById = model.ReceivedById;
			exroad.ReceivedDate = model.ReceivedDate;
			exroad.Receive_Via = model.Receive_Via;
			exroad.NomineeId = model.NomineeId;

			db.EventExhibitionRequest.Attach(exroad);
			db.Entry(exroad).Property(x => x.EventName).IsModified = true;
			db.Entry(exroad).Property(x => x.Organiser).IsModified = true;
			db.Entry(exroad).Property(x => x.Location).IsModified = true;
			db.Entry(exroad).Property(x => x.StartDate).IsModified = true;
			db.Entry(exroad).Property(x => x.EndDate).IsModified = true;
			db.Entry(exroad).Property(x => x.StartTime).IsModified = true;
			db.Entry(exroad).Property(x => x.EndTime).IsModified = true;
			db.Entry(exroad).Property(x => x.ParticipationRequirement).IsModified = true;
			db.Entry(exroad).Property(x => x.ReceivedById).IsModified = true;
			db.Entry(exroad).Property(x => x.ReceivedDate).IsModified = true;
			db.Entry(exroad).Property(x => x.Receive_Via).IsModified = true;
			db.Entry(exroad).Property(x => x.NomineeId).IsModified = true;

			db.Entry(exroad).Property(x => x.ExhibitionStatus).IsModified = false;
			db.Entry(exroad).Property(x => x.Display).IsModified = false;
			db.Entry(exroad).Property(x => x.Id).IsModified = false;

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var exroad = db.EventExhibitionRequest.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (exroad == null)
			{
				return NotFound();
			}

			exroad.Display = false;

			db.Entry(exroad).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

		[Route("api/eEvent/ExhibitionRoadshowRequest/IsEventNameExist")]
		[HttpGet]
		public IHttpActionResult IsEventNameExist(int? id, string name)
		{
			if (id == null)
			{
				if (db.EventExhibitionRequest.Any(u => u.EventName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
					return Ok(true);
			}
			else
			{
				if (db.EventExhibitionRequest.Any(u => u.EventName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
					return Ok(true);
			}

			return NotFound();
		}



	}
}
