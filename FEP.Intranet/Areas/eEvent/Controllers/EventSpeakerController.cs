using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
    public class EventSpeakerController : FEPController
    {
		private DbEntities db = new DbEntities();

        // GET: eEventSpeaker/EventSpeaker
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult List(FilterEventSpeakerModel filter)
		{
			var obj = db.EventSpeaker.Where(i => i.Display && (filter.UserName == filter.UserName))
				.Select(i => new DetailsEventSpeakerModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					SpeakerType = i.SpeakerType,
					Remark = i.Remark,
				}).ToList();

			ListEventSpeakerModel model = new ListEventSpeakerModel(obj);

			return View(model);
		}

		// GET: eEventSpeaker/EventSpeaker/Details/5
		public ActionResult Details(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var obj = db.EventSpeaker.Where(i => i.Id == id)
				.Select(i => new DetailsEventSpeakerModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					SpeakerType = i.SpeakerType,
					Remark = i.Remark,
				}).FirstOrDefault();

			if (obj == null)
			{
				return HttpNotFound();
			}

			return View("Details", obj);
		}

        // GET: eEventSpeaker/EventSpeaker/Create
        public ActionResult Create()
        {
			CreateEventSpeakerModel model = new CreateEventSpeakerModel {
				DateAssigned = DateTime.Now,
			};

			ViewBag.UserId = new SelectList(db.User.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View(model);
        }

        // POST: eEventSpeaker/EventSpeaker/Create
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateEventSpeakerModel model)
        {
			if (ModelState.IsValid)
			{
				EventSpeaker obj = new EventSpeaker
				{
					UserId = model.UserId,
					SpeakerType = model.SpeakerType,
					Remark = model.Remark,
					DateAssigned = model.DateAssigned,

					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.EventSpeaker.Add(obj);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Speaker successfully created.";
				return RedirectToAction("List");
			}
			return View(model);
		}

        // GET: eEventSpeaker/EventSpeaker/Edit/5
        public ActionResult Edit(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var obj = db.EventSpeaker.Where(i => i.Id == id)
				.Select(i => new EditEventSpeakerModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					SpeakerType = i.SpeakerType,
					Remark = i.Remark,
					DateAssigned = i.DateAssigned,
				}).FirstOrDefault();

			if (obj == null)
			{
				return HttpNotFound();
			}
			return View(obj);
		}

        // POST: eEventSpeaker/EventSpeaker/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditEventSpeakerModel model)
        {
			if (ModelState.IsValid)
			{
				EventSpeaker obj = new EventSpeaker
				{
					Id = model.Id,
					UserId = model.UserId,
					SpeakerType = model.SpeakerType,
					Remark = model.Remark,
					DateAssigned = model.DateAssigned,
				};

				db.Entry(obj).State = EntityState.Modified;
				db.Entry(obj).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(obj).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Speaker successfully updated.";
				return RedirectToAction("List");
			}
			return View(model);
		}

        // GET: eEventSpeaker/EventSpeaker/Delete/5
        public ActionResult Delete(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var obj = db.EventSpeaker.Where(i => i.Id == id)
				.Select(i => new DeleteEventSpeakerModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					SpeakerType = i.SpeakerType,
					Remark = i.Remark,
					DateAssigned = i.DateAssigned,
				}).FirstOrDefault();

			if (obj == null)
			{
				return HttpNotFound();
			}

			return View(obj);
		}

		// POST: eEventSpeaker/EventSpeaker/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteEventSpeakerModel model)
		{
			EventSpeaker obj = new EventSpeaker() { Id = model.Id };
			obj.Display = false;

			db.EventSpeaker.Attach(obj);
			db.Entry(obj).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Event Speaker successfully deleted.";
			return RedirectToAction("List");
		}
    }
}
