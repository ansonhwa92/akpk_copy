using FEP.Intranet.Areas.eEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAS.Model;

namespace FEP.Portal.Controllers
{

	//[LogError]
    public class PublicEventController : Controller
    {
		private DbEntities db = new DbEntities();

        // GET: PublicEvent
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult List(FilterPublicEventModel filter)
		{
			var x = db.eEvent.Where(i => i.Display && (filter.EventTitle == filter.EventTitle))
				.Select(i => new DetailsPublicEventModel()
			{
				Id = i.Id,
				EventTitle = i.EventTitle,
				EventObjective = i.EventObjective,
				Date = i.Date,
				Venue = i.Venue,
				Fee = i.Fee
			}).ToList();

			ListPublicEventModel model = new ListPublicEventModel(x);

			return View(model);
		}

		// GET: PublicEvent/Details/5
		public ActionResult Details(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var x = db.eEvent.Where(i => i.Id == id)
				.Select(i => new DetailsPublicEventModel()
			{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					Date = i.Date,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ExternalExhibitor = i.ExternalExhibitor,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					EventStatus = i.EventStatus,
					AgendaId = i.AgendaId,
					AgendaName = i.Agenda.AgendaTitle,
					EventCategory = i.EventCategory,
					Remarks = i.Remarks
				}).FirstOrDefault();
            return View("Details", x);
        }

        // GET: PublicEvent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicEvent/Create
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

        // GET: PublicEvent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PublicEvent/Edit/5
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

        // GET: PublicEvent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PublicEvent/Delete/5
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
