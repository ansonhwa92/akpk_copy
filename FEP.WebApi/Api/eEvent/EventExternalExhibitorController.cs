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
	[Route("api/eEvent/EventExternalExhibitor")]
	public class EventExternalExhibitionController : ApiController
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

		[Route("api/eEvent/EventExternalExhibitor/GetExternalExhibitorList")]
		[HttpPost]
		public IHttpActionResult Post(FilterEventExternalExhibitorModel request)
		{

			var query = db.EventExternalExhibitor.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.Name == null || s.Name.Contains(request.Name))
			&& (request.PhoneNo == null || s.PhoneNo == (request.PhoneNo))
			&& (request.Email == null || s.Email.Contains(request.Email))
			);

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.Name.Contains(value)

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
					case "Name":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Name);
						}
						else
						{
							query = query.OrderByDescending(o => o.Name);
						}

						break;

					case "PhoneNo":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.PhoneNo);
						}
						else
						{
							query = query.OrderByDescending(o => o.PhoneNo);
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
						query = query.OrderByDescending(o => o.Name);
						break;
				}

			}
			else
			{
				query = query.OrderByDescending(o => o.Name);
			}


			var data = query.Skip(request.start).Take(request.length)
				.Select(i => new EventExternalExhibitorModel
				{
					Id = i.Id,
					Name = i.Name,
					Email = i.Email,
					PhoneNo = i.PhoneNo,
					Remark = i.Remark,
					
				}).ToList();

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
			var exhibitor = db.EventExternalExhibitor.Where(u => u.Id == id)
				.Select(i => new DetailsEventExternalExhibitorModel
				{
					Id = i.Id,
					Name = i.Name,
					Email = i.Email,
					PhoneNo = i.PhoneNo,
					Remark = i.Remark,
				}).FirstOrDefault();

			if (exhibitor == null)
			{
				return NotFound();
			}

			return Ok(exhibitor);
		}

		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateEventExternalExhibitorModel model)
		{
			var exhibitor = new EventExternalExhibitor
			{
				Name = model.Name,
				Email = model.Email,
				PhoneNo = model.PhoneNo,
				Remark = model.Remark,
				
				Display = true,
				CreatedDate = DateTime.Now,
			};
			db.EventExternalExhibitor.Add(exhibitor);
			db.SaveChanges();

			return Ok(exhibitor.Id);
		}

		public IHttpActionResult Put(int id, [FromBody] EditEventExternalExhibitorModel model)
		{
			var exhibitor = db.EventExternalExhibitor.Where(u => u.Id == id).FirstOrDefault();

			if (exhibitor == null)
			{
				return NotFound();
			}

			exhibitor.Name = model.Name;
			exhibitor.PhoneNo = model.PhoneNo;
			exhibitor.Email = model.Email;
			exhibitor.Remark = model.Remark;
			

			db.EventExternalExhibitor.Attach(exhibitor);
			db.Entry(exhibitor).Property(x => x.Name).IsModified = true;
			db.Entry(exhibitor).Property(x => x.PhoneNo).IsModified = true;
			db.Entry(exhibitor).Property(x => x.Email).IsModified = true;
			db.Entry(exhibitor).Property(x => x.Remark).IsModified = true;

			db.Entry(exhibitor).Property(x => x.Display).IsModified = false;
			db.Entry(exhibitor).Property(x => x.Id).IsModified = false;

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var exhibitor = db.EventExternalExhibitor.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (exhibitor == null)
			{
				return NotFound();
			}

			exhibitor.Display = false;

			db.Entry(exhibitor).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

		[Route("api/eEvent/EventExternalExhibitor/IsNameExist")]
		[HttpGet]
		public IHttpActionResult IsNameExist(int? id, string name)
		{
			if (id == null)
			{
				if (db.EventExternalExhibitor.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
					return Ok(true);
			}
			else
			{
				if (db.EventExternalExhibitor.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
					return Ok(true);
			}

			return NotFound();
		}


		[HttpGet]
		public IHttpActionResult Get()
		{
			var exhibitor = db.EventExternalExhibitor.Where(u => u.Display).Select(s => new EventExternalExhibitorModel
			{
				Id = s.Id,
				Name = s.Name,
				Email = s.Email,
				PhoneNo = s.PhoneNo
			}).ToList();

			return Ok(exhibitor);
		}

	}
}