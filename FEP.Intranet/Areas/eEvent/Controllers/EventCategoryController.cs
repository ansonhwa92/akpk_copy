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
	public class EventCategoryController : FEPController
	{

		private DbEntities db = new DbEntities();

		// GET: eEvent/EventCategory
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(FilterEventCategoryModel filter)
		{
			var e = db.EventCategory.Where(i => i.Display && (filter.CategoryName == filter.CategoryName))
				.Select(i => new DetailsEventCategoryModel()
				{
					Id = i.Id,
					CategoryName = i.CategoryName,
				}).ToList();

			ListEventCategoryModel model = new ListEventCategoryModel(e);

			return View(model);
		}

		// GET: eEvent/EventCategory/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}


			var model = db.EventCategory.Where(i => i.Id == id)
				.Select(i => new DetailsEventCategoryModel()
				{
					Id = i.Id,
					CategoryName = i.CategoryName
				}).FirstOrDefault();

			if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		// GET: eEvent/EventCategory/Create
		public ActionResult Create()
		{
			CreateEventCategoryModel model = new CreateEventCategoryModel { };

			return View(model);
		}

		// POST: eEvent/EventCategory/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateEventCategoryModel model)
		{
			if (ModelState.IsValid)
			{
				EventCategory category = new EventCategory
				{
					CategoryName = model.CategoryName,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.EventCategory.Add(category);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Category successfully created.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: eEvent/EventCategory/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var model = db.EventCategory.Where(i => i.Id == id)
				.Select(i => new EditEventCategoryModel()
				{
					Id = i.Id,
					CategoryName = i.CategoryName,

				}).FirstOrDefault();

			if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		// POST: eEvent/EventCategory/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditEventCategoryModel model)
		{
			if (ModelState.IsValid)
			{
				EventCategory category = new EventCategory
				{
					Id = model.Id,
					CategoryName = (model.CategoryName != null) ? model.CategoryName.ToUpper() : model.CategoryName,
				};

				db.Entry(category).State = EntityState.Modified;
				db.Entry(category).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(category).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Event Category successfully updated.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: eEvent/EventCategory/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var model = db.EventCategory.Where(i => i.Id == id)
				.Select(i => new DeleteEventCategoryModel()
				{
					Id = i.Id,
					CategoryName = i.CategoryName,
				}).FirstOrDefault();

			if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		// POST: eEvent/EventCategory/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteEventCategoryModel model)
		{
			EventCategory category = new EventCategory() { Id = model.Id };
			category.Display = false;

			db.EventCategory.Attach(category);
			db.Entry(category).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Event Category successfully deleted.";
			return RedirectToAction("List");
		}
	}
}
