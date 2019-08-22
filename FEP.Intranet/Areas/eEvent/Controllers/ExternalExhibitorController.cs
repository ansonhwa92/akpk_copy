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
	public class ExternalExhibitorController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEvent/ExternalExhibitor
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(FilterExternalExhibitorModel filter)
		{
			var local = db.EventExternalExhibitor.Where(i => i.Display && (filter.Name == filter.Name))
				.Select(i => new DetailsExternalExhibitorModel()
				{
					Id = i.Id,
					Name = i.Name
				}).ToList();

			ListExternalExhibitorModel model = new ListExternalExhibitorModel(local);

			return View(model);
		}

		// GET: eEvent/ExternalExhibitor/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var local = db.EventExternalExhibitor.Where(i => i.Id == id)
				.Select(i => new DetailsExternalExhibitorModel()
				{
					Id = i.Id,
					Name = i.Name
				}).FirstOrDefault();

			if (local == null)
			{
				return HttpNotFound();
			}

			return View(local);
		}

		// GET: eEvent/ExternalExhibitor/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: eEvent/ExternalExhibitor/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateExternalExhibitorModel model)
		{
			if (ModelState.IsValid)
			{
				EventExternalExhibitor x = new EventExternalExhibitor
				{
					Name = model.Name,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,
					Display = true
				};
				db.EventExternalExhibitor.Add(x);
				db.SaveChanges();

				TempData["SuccessMessage"] = "External Exhibitor successfully created.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: eEvent/ExternalExhibitor/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var e = db.EventExternalExhibitor.Where(i => i.Id == id)
				.Select(i => new EditExternalExhibitorModel()
				{
					Id = i.Id,
					Name = i.Name,
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}
			return View(e);
		}

		// POST: eEvent/ExternalExhibitor/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditExternalExhibitorModel model)
		{
			if (ModelState.IsValid)
			{
				EventExternalExhibitor ex = new EventExternalExhibitor
				{
					Id = model.Id,
					Name = model.Name
				};

				db.Entry(ex).State = EntityState.Modified;
				db.Entry(ex).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(ex).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "External Exhibitor successfully updated.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: eEvent/ExternalExhibitor/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var e = db.EventExternalExhibitor.Where(i => i.Id == id)
				.Select(i => new DeleteExternalExhibitorModel()
				{
					Id = i.Id,
					Name = i.Name,
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		// POST: eEvent/ExternalExhibitor/Delete/5
		[HttpPost]
		public ActionResult Delete(DeleteExternalExhibitorModel model)
		{
			EventExternalExhibitor ex = new EventExternalExhibitor() { Id = model.Id };
			ex.Display = false;

			db.EventExternalExhibitor.Attach(ex);
			db.Entry(ex).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "External Exhibitor successfully deleted.";
			return RedirectToAction("List");
		}
	}
}
