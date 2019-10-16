using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/EventAgenda")]
	public class EventAgendaController : ApiController
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

		[HttpGet]
		public IHttpActionResult Get()
		{
			var users = db.EventAgenda.Where(u => u.Display).Select(s => new EventAgendaModel
			{
				Id = s.Id,
				AgendaTitle = s.AgendaTitle,
				AgendaDescription = s.AgendaDescription,
				Tentative = s.Tentative,
				Time = s.Time,
				EventId = s.EventId,
				EventName = s.Event.RefNo,
				PersonInChargeId = s.PersonInChargeId,
				PersonInChargeName = s.User.Name,
			}).ToList();

			return Ok(users);
		}


		[Route("api/eEvent/EventAgenda/GetEventAgendaList")]
		[HttpPost]
		public IHttpActionResult Post(FilterEventAgendaModel request)
		{

			var query = db.EventAgenda.Where(u => u.Display);

			var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.AgendaTitle == null || s.AgendaTitle.Contains(request.AgendaTitle))
			   && (request.AgendaDescription == null || s.AgendaDescription.Contains(request.AgendaDescription))
			   && (request.Tentative == null || s.Tentative.Contains(request.Tentative))
			   && (request.EventId == null || s.EventId == request.EventId)
			   );

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.AgendaTitle.Contains(value)
				|| p.AgendaDescription.Contains(value)
				|| p.Tentative.Contains(value)
				|| p.Event.RefNo.Contains(value)
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
					case "EventId":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.EventId);
						}
						else
						{
							query = query.OrderByDescending(o => o.EventId);
						}

						break;

					case "Time":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Time);
						}
						else
						{
							query = query.OrderByDescending(o => o.Time);
						}

						break;

					case "Tentative":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Tentative);
						}
						else
						{
							query = query.OrderByDescending(o => o.Tentative);
						}

						break;

					case "AgendaTitle":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.AgendaTitle);
						}
						else
						{
							query = query.OrderByDescending(o => o.AgendaTitle);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.Event.RefNo);
						break;
				}
			}
			else
			{
				query = query.OrderByDescending(o => o.Event.RefNo);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(s => new EventAgendaModel
				{
					Id = s.Id,
					AgendaTitle = s.AgendaTitle,
					AgendaDescription = s.AgendaDescription,
					EventId = s.EventId,
					EventName = s.Event.RefNo,
					Tentative = s.Tentative,
					Time = s.Time,
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
			var model = db.EventAgenda.Where(i => i.Display && i.Id == id)
				.Select(i => new DeleteEventAgendaModel
				{
					Id = i.Id,
					AgendaTitle = i.AgendaTitle,
					AgendaDescription = i.AgendaDescription,
					Tentative = i.Tentative,
					Time = i.Time,
					EventId = i.EventId,
					EventName = i.Event.RefNo,
					PersonInChargeId = i.PersonInChargeId,
					PersonInChargeName = i.User.Name,
					
				}).FirstOrDefault();

			if (model == null)
			{
				return NotFound();
			}

			model.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.EventAgenda && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

			return Ok(model);
		}



		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateEventAgendaModel model)
		{
			var agenda = new EventAgenda
			{
				AgendaTitle = model.AgendaTitle,
				AgendaDescription = model.AgendaDescription,
				Tentative = model.Tentative,
				Time = model.Time,
				EventId = model.EventId,
				PersonInChargeId = model.PersonInChargeId,
				CreatedBy = null,
				Display = true,
				CreatedDate = DateTime.Now,
			};

			db.EventAgenda.Add(agenda);
			db.SaveChanges();

			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.EventAgenda,
					FileId = fileid,
					ParentId = agenda.Id
				};

				db.EventFile.Add(eventfile);
			}

			return Ok(agenda.Id);
		}


		//Edit
		public IHttpActionResult Put(int id, [FromBody] EditEventAgendaModel model)
		{
			var agenda = db.EventAgenda.Where(u => u.Id == id).FirstOrDefault();

			if (agenda == null)
			{
				return NotFound();
			}

			agenda.AgendaTitle = model.AgendaTitle;
			agenda.AgendaDescription = model.AgendaDescription;
			agenda.Tentative = model.Tentative;
			agenda.Time = model.Time;
			agenda.EventId = model.EventId;
			agenda.PersonInChargeId = model.PersonInChargeId;
			
			db.Entry(agenda).State = EntityState.Modified;
			db.Entry(agenda).Property(x => x.Display).IsModified = false;

			//remove file 
			var attachments = db.EventFile.Where(s => s.FileCategory == EventFileCategory.EventAgenda && s.ParentId == model.Id).ToList();

			if (attachments != null)
			{
				//delete all
				if (model.Attachments == null)
				{
					foreach (var attachment in attachments)
					{
						attachment.FileDocument.Display = false;
						db.FileDocument.Attach(attachment.FileDocument);
						db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

						db.EventFile.Remove(attachment);
					}
				}
				else
				{
					foreach (var attachment in attachments)
					{
						if (!model.Attachments.Any(u => u.Id == attachment.FileDocument.Id))//delete if not exist anymore
						{
							attachment.FileDocument.Display = false;
							db.FileDocument.Attach(attachment.FileDocument);
							db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

							db.EventFile.Remove(attachment);
						}
					}
				}
			}

			//add new file
			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.EventAgenda,
					FileId = fileid,
					ParentId = agenda.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();

			return Ok(true);
		}

		public IHttpActionResult Delete(int id)
		{
			var agenda = db.EventAgenda.Where(r => r.Id == id && r.Display).FirstOrDefault();

			if (agenda == null)
			{
				return NotFound();
			}

			agenda.Display = false;
			db.Entry(agenda).State = EntityState.Modified;

			db.SaveChanges();
			return Ok(true);
		}

	}
}
