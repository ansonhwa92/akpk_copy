using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: eEvent/ExternalExhibitor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: eEvent/ExternalExhibitor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: eEvent/ExternalExhibitor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: eEvent/ExternalExhibitor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
