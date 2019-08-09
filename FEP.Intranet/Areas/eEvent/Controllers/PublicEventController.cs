﻿using FEP.Helper;
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
	public class PublicEventController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEvent/PublicEvent
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(FilterPublicEventModel filter)
		{
			var e = db.PublicEvent.Where(i => i.Display && (filter.EventTitle == filter.EventTitle))
				.Select(i => new DetailsPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjectiveId = i.EventObjectiveId,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee
				}).ToList();

			ListPublicEventModel model = new ListPublicEventModel(e);

			return View(model);
		}

		// GET: PublicEvent/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}


			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DetailsPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjectiveId = i.EventObjectiveId,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ExternalExhibitorId = i.ExternalExhibitorId,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					EventStatus = i.EventStatus,
					EventCategory = i.EventCategory,
					Reasons = i.Reasons,
					Remarks = i.Remarks
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View("Details", e);
		}

		// GET: PublicEvent/Create
		public ActionResult Create()
		{
			CreatePublicEventModel model = new CreatePublicEventModel
			{
				EventStatus = EventStatus.New,
			};

			ViewBag.EventObjectiveId = new SelectList(db.EventObjective.Where(p => p.Display).OrderBy(o => o.ObjectiveTitle), "Id", "ObjectiveTitle");
			ViewBag.ExternalExhibitorId = new SelectList(db.EventExternalExhibitor.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View("Create", model);
		}

		// POST: PublicEvent/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreatePublicEventModel model)
		{
			if (ModelState.IsValid)
			{
				PublicEvent x = new PublicEvent
				{
					EventTitle = model.EventTitle,
					EventObjectiveId = model.EventObjectiveId,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					ExternalExhibitorId = model.ExternalExhibitorId,
					ApprovalId1 = model.ApprovalId1,
					ApprovalId2 = model.ApprovalId2,
					ApprovalId3 = model.ApprovalId3,
					ApprovalId4 = model.ApprovalId4,
					EventStatus = model.EventStatus,
					EventCategory = model.EventCategory,
					Reasons = model.Reasons,
					Remarks = model.Remarks,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.PublicEvent.Add(x);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Public Event successfully created.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: PublicEvent/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new EditPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjectiveId = i.EventObjectiveId,
					EventObjectiveTitle = i.EventObjective.ObjectiveTitle,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,

					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.EventExternalExhibitor.Name,

					ApprovalId1 = i.ApprovalId1,
					ApprovalId2 = i.ApprovalId2,
					ApprovalId3 = i.ApprovalId3,
					ApprovalId4 = i.ApprovalId4,

					ApprovalName1 = i.Approval1.User.Name,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalName4 = i.Approval4.User.Name,

					EventStatus = i.EventStatus,

					EventCategory = i.EventCategory,
					Reasons = i.Reasons,
					Remarks = i.Remarks
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			ViewBag.AgendaId = new SelectList(db.EventAgenda.Where(p => p.Display).OrderBy(o => o.AgendaTitle), "Id", "AgendaTitle");

			return View(e);
		}

		// POST: PublicEvent/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditPublicEventModel model)
		{
			if (ModelState.IsValid)
			{
				PublicEvent eEvent = new PublicEvent
				{
					Id = model.Id,
					EventTitle = (model.EventTitle != null) ? model.EventTitle.ToUpper() : model.EventTitle,
					EventObjectiveId = model.EventObjectiveId,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					ExternalExhibitorId = model.ExternalExhibitorId,
					ApprovalId1 = model.ApprovalId1,
					ApprovalId2 = model.ApprovalId2,
					ApprovalId3 = model.ApprovalId3,
					ApprovalId4 = model.ApprovalId4,
					EventStatus = model.EventStatus,
					EventCategory = model.EventCategory,
					Reasons = model.Reasons,
					Remarks = model.Remarks
				};

				db.Entry(eEvent).State = EntityState.Modified;
				db.Entry(eEvent).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(eEvent).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Public Event successfully updated.";
				return RedirectToAction("List");
			}
			return View(model);
		}

		// GET: PublicEvent/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DeletePublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,

					EventObjectiveId = i.EventObjectiveId,
					EventObjectiveTitle = i.EventExternalExhibitor.Name,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.EventExternalExhibitor.Name,

					ApprovalId1 = i.ApprovalId1,
					ApprovalId2 = i.ApprovalId2,
					ApprovalId3 = i.ApprovalId3,
					ApprovalId4 = i.ApprovalId4,

					ApprovalName1 = i.Approval1.User.Name,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalName4 = i.Approval4.User.Name,

					EventStatus = i.EventStatus,
					EventCategory = i.EventCategory,
					Reasons = i.Reasons,
					Remarks = i.Remarks
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		// POST: PublicEvent/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeletePublicEventModel model)
		{
			PublicEvent eEvent = new PublicEvent() { Id = model.Id };
			eEvent.Display = false;

			db.PublicEvent.Attach(eEvent);
			db.Entry(eEvent).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Public Event successfully deleted.";
			return RedirectToAction("List");
		}
	}
}