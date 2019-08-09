using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEP.Model;

namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class SurveyController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: RnP/Survey
        public ActionResult Index()
        {
            //return View();
            var surveys = db.Survey;    //.Include(s => s.Category);
            return View(surveys.ToList());
        }

        // GET: RnP/Survey/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Survey.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: RnP/Survey/Create
        public ActionResult Create()
        {
            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

