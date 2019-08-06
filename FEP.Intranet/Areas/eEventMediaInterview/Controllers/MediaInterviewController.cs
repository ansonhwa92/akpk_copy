using FEP.Intranet.Areas.eEventMediaInterview.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEventMediaInterview.Controllers
{
	public class MediaInterviewController : Controller
	{
		private DbEntities db = new DbEntities();

		// GET: eEventMediaInterview/MediaInterview
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(FilterMediaInterviewModel filter)
		{
			var media = db.EventMediaInterviewRequest.Where(i => i.Display)
				.Select(i => new DetailsMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					Address = i.Address,
					Email = i.Email,
					Date = i.Date,
					Time = i.Time,
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					UserId = i.UserId,
					UserName = i.User.Name,
					Designation = i.Designation,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle
				}).ToList();

			ListMediaInterviewModel model = new ListMediaInterviewModel(media);

			return View("List", model);
		}

		// GET: eEventMediaInterview/MediaInterview/Details/5
		public ActionResult Details(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DetailsMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					Address = i.Address,
					Email = i.Email,
					Date = i.Date,
					Time = i.Time,
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					UserId = i.UserId,
					UserName = i.User.Name,
					Designation = i.Designation,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Details", media);
		}

		// GET: eEventMediaInterview/MediaInterview/Create
		public ActionResult Create()
		{
			ViewBag.EventId = new SelectList(db.PublicEvent.Where(p => p.Display).OrderBy(o => o.EventTitle), "Id", "EventTitle");
			ViewBag.UserId = new SelectList(db.User.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View("Create");
		}

		// POST: eEventMediaInterview/MediaInterview/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateMediaInterviewModel model)
		{
			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					Address = model.Address,
					Email = model.Email,
					Date = model.Date,
					Time = model.Time,
					Location = model.Location,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.UserId,
					Designation = model.Designation,
					EventId = model.EventId,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};
				db.EventMediaInterviewRequest.Add(media);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Media Interview Request successfully created.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new EditMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					Address = i.Address,
					Email = i.Email,
					Date = i.Date,
					Time = i.Time,
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					UserId = i.UserId,
					UserName = i.User.Name,
					Designation = i.Designation,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			ViewBag.EventId = new SelectList(db.PublicEvent.Where(p => p.Display).OrderBy(o => o.EventTitle), "Id", "EventTitle");
			ViewBag.UserId = new SelectList(db.User.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View("Edit", media);
		}

		// POST: eEventMediaInterview/MediaInterview/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditMediaInterviewModel model)
		{
			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					Address = model.Address,
					Email = model.Email,
					Date = model.Date,
					Time = model.Time,
					Location = model.Location,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.UserId,
					Designation = model.Designation,
					EventId = model.EventId,
				};
				db.Entry(media).State = EntityState.Modified;
				db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(media).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Media Interview Request successfully updated.";
				return RedirectToAction("List");
			}
			return View("Edit", model);
		}

		// GET: eEventMediaInterview/MediaInterview/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DeleteMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					Address = i.Address,
					Email = i.Email,
					Date = i.Date,
					Time = i.Time,
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					UserId = i.UserId,
					UserName = i.User.Name,
					Designation = i.Designation,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Delete", media);
		}

		// POST: eEventMediaInterview/MediaInterview/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteMediaInterviewModel model)
		{
			EventMediaInterviewRequest media = new EventMediaInterviewRequest() { Id = model.Id };
			media.Display = false;

			db.EventMediaInterviewRequest.Attach(media);
			db.Entry(media).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Media Interview Request successfully deleted.";
			return RedirectToAction("List");
		}
	}
}
