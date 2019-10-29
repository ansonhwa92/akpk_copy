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
	[Route("api/eEvent/DutyRoster")]
	public class DutyRosterController : ApiController
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

		public IHttpActionResult Get()
		{
			var duty = db.DutyRoster.Where(u => u.Display).Select(s => new DutyRosterModel
			{
				Id = s.Id,
				Date = s.Date,
				StartTime = s.StartTime,
				EndTime = s.EndTime
			}).ToList();

			return Ok(duty);
		}


		public IHttpActionResult Get(int id)
		{
			var category = db.DutyRoster.Where(u => u.Display && u.Id == id).Select(s => new DutyRosterModel
			{
				Id = s.Id,
				Date = s.Date,
				StartTime = s.StartTime,
				EndTime = s.EndTime
			}).FirstOrDefault();

			if (category != null)
			{
				return Ok(category);
			}

			return NotFound();
		}


		[ValidationActionFilter]
		public IHttpActionResult Post([FromBody]CreateDutyRosterModel model)
		{

			var duty = new DutyRoster
			{
				Date = model.Date,
				StartTime = model.StartTime,
				EndTime = model.EndTime,
				Display = true
			};

			db.DutyRoster.Add(duty);
			db.SaveChanges();

			return Ok(duty.Id);

		}


		[ValidationActionFilter]
		public IHttpActionResult Put(int id, [FromBody]EditDutyRosterModel model)
		{

			var duty = db.DutyRoster.Where(s => s.Id == id).FirstOrDefault();

			if (duty != null)
			{
				duty.Date = model.Date;
				duty.StartTime = model.StartTime;
				duty.EndTime = model.EndTime;

				db.Entry(duty).State = EntityState.Modified;
				db.Entry(duty).Property(x => x.Display).IsModified = false;

				db.SaveChanges();

				return Ok(true);
			}
			else
			{
				return NotFound();
			}

		}


		public IHttpActionResult Delete(int id)
		{
			var category = db.DutyRoster.Where(u => u.Id == id).FirstOrDefault();

			if (category != null)
			{
				category.Display = false;

				db.DutyRoster.Attach(category);
				db.Entry(category).Property(m => m.Display).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;

				db.SaveChanges();

				return Ok(true);
			}
			else
			{
				return NotFound();
			}

		}
	}
}
