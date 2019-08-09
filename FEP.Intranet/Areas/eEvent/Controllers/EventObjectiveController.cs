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
    public class EventObjectiveController : FEPController
    {
		private DbEntities db = new DbEntities();

		// GET: eEventObjective/EventObjective
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult List(FilterEventObjectiveModel filter)
		{
			var obj = db.EventObjective.Where(i => i.Display && (filter.ObjectiveTitle == filter.ObjectiveTitle))
				.Select(i => new DetailsEventObjectiveModel()
				{
					Id = i.Id,
					ObjectiveTitle = i.ObjectiveTitle,
				}).ToList();

			ListEventObjectiveModel model = new ListEventObjectiveModel(obj);

			return View(model);
		}

		// GET: eEventSpeaker/EventSpeaker/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}


			var obj = db.EventObjective.Where(i => i.Id == id)
				.Select(i => new DetailsEventObjectiveModel()
				{
					Id = i.Id,
					ObjectiveTitle = i.ObjectiveTitle,
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
			CreateEventObjectiveModel model = new CreateEventObjectiveModel { };

			return View(model);
		}

		// POST: eEventSpeaker/EventSpeaker/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateEventObjectiveModel model)
		{
			if (ModelState.IsValid)
			{
				EventObjective obj = new EventObjective
				{
					ObjectiveTitle = model.ObjectiveTitle,

					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.EventObjective.Add(obj);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Objective successfully created.";
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
			var obj = db.EventObjective.Where(i => i.Id == id)
				.Select(i => new EditEventObjectiveModel()
				{
					Id = i.Id,
					ObjectiveTitle = i.ObjectiveTitle,
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
		public ActionResult Edit(EditEventObjectiveModel model)
		{
			if (ModelState.IsValid)
			{
				EventObjective obj = new EventObjective
				{
					Id = model.Id,
					ObjectiveTitle = (model.ObjectiveTitle != null) ? model.ObjectiveTitle.ToUpper() : model.ObjectiveTitle,
				};

				db.Entry(obj).State = EntityState.Modified;
				db.Entry(obj).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(obj).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Objective successfully updated.";
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

			var e = db.EventObjective.Where(i => i.Id == id)
				.Select(i => new DeleteEventObjectiveModel()
				{
					Id = i.Id,
					ObjectiveTitle = i.ObjectiveTitle,
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		// POST: eEventSpeaker/EventSpeaker/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteEventObjectiveModel model)
		{
			EventObjective obj = new EventObjective() { Id = model.Id };
			obj.Display = false;

			db.EventObjective.Attach(obj);
			db.Entry(obj).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Event Objective successfully deleted.";
			return RedirectToAction("List");
		}
	}
}
