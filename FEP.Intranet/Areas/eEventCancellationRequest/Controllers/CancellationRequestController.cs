using FEP.Intranet.Areas.eEventCancellationRequest.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEventCancellationRequest.Controllers
{
	public class CancellationRequestController : Controller
	{
		private DbEntities db = new DbEntities();

		// GET: eEventCancellationRequest/CancellationRequest
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(FilterCancellationRequestModel filter)
		{
			var cancel = db.EventCancellation.Where(i => i.Display)
				.Select(i => new DetailsCancellationRequestModel()
			{
				Id = i.Id,
				UserId = i.UserId,
				UserName = i.User.Name,
				EventId = i.EventId,
				EventTitle = i.Event.EventTitle,
				Reasons = i.Reasons,
				ApprovalId1 = i.ApprovalId1,
				ApprovalName1 = i.Approval1.User.Name,
				ApprovalId2 = i.ApprovalId2,
				ApprovalName2 = i.Approval2.User.Name,
				ApprovalId3 = i.ApprovalId3,
				ApprovalName3 = i.Approval3.User.Name,
				ApprovalId4 = i.ApprovalId4,
				ApprovalName4 = i.Approval4.User.Name,
				VerifyId = i.VerifyId,
				VerifierName = i.Verifier.User.Name,
			}).ToList();

			ListCancellationRequestModel model = new ListCancellationRequestModel(cancel);

			return View(model);
		}

		public ActionResult Details(int? id)
		{
			var cancel = db.EventCancellation.Where(i => i.Id == id).Select(i => new DetailsCancellationRequestModel()
			{
				Id = i.Id,
				UserId = i.UserId,
				UserName = i.User.Name,
				EventId = i.EventId,
				EventTitle = i.Event.EventTitle,
				Reasons = i.Reasons,
				ApprovalId1 = i.ApprovalId1,
				ApprovalName1 = i.Approval1.User.Name,
				ApprovalId2 = i.ApprovalId2,
				ApprovalName2 = i.Approval2.User.Name,
				ApprovalId3 = i.ApprovalId3,
				ApprovalName3 = i.Approval3.User.Name,
				ApprovalId4 = i.ApprovalId4,
				ApprovalName4 = i.Approval4.User.Name,
				VerifyId = i.VerifyId,
				VerifierName = i.Verifier.User.Name,
			}).FirstOrDefault();

			if (cancel == null)
			{
				return HttpNotFound();
			}

			return View("Details", cancel);
		}

		public ActionResult Create()
		{
			ViewBag.EventId = new SelectList(db.PublicEvent.Where(p => p.Display).OrderBy(o => o.EventTitle), "Id", "EventTitle");
			ViewBag.UserId = new SelectList(db.User.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View("Create");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateCancellationRequestModel model)
		{
			if (ModelState.IsValid)
			{
				EventCancellation cancellation = new EventCancellation
				{
					UserId = model.UserId,
					EventId = model.EventId,
					Reasons = model.Reasons,
					ApprovalId1 = model.ApprovalId1,
					ApprovalId2 = model.ApprovalId2,
					ApprovalId3 = model.ApprovalId3,
					ApprovalId4 = model.ApprovalId4,
					VerifyId = model.VerifyId,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.EventCancellation.Add(cancellation);
				db.SaveChanges();

				TempData["SuccessMessage"] = "Cancellation Request successfully created.";
				return RedirectToAction("List");
			}
			return View("Create", model);
		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var cancel = db.EventCancellation.Where(i => i.Id == id)
				.Select(i => new EditCancellationRequestModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle,
					Reasons = i.Reasons,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					VerifyId = i.VerifyId,
					VerifierName = i.Verifier.User.Name,
				}).FirstOrDefault();

			if (cancel == null)
			{
				return HttpNotFound();
			}

			ViewBag.EventId = new SelectList(db.PublicEvent.Where(p => p.Display).OrderBy(o => o.EventTitle), "Id", "EventTitle");
			ViewBag.UserId = new SelectList(db.User.Where(p => p.Display).OrderBy(o => o.Name), "Id", "Name");

			return View("Edit", cancel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditCancellationRequestModel model)
		{
			if (ModelState.IsValid)
			{
				EventCancellation cancellation = new EventCancellation
				{
					Id = model.Id,
					UserId = model.UserId,
					EventId = model.EventId,
					Reasons = model.Reasons,
					ApprovalId1 = model.ApprovalId1,
					ApprovalId2 = model.ApprovalId2,
					ApprovalId3 = model.ApprovalId3,
					ApprovalId4 = model.ApprovalId4,
					VerifyId = model.VerifyId,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true
				};

				db.Entry(cancellation).State = EntityState.Modified;
				db.Entry(cancellation).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(cancellation).Property(x => x.Display).IsModified = false;

				db.Configuration.ValidateOnSaveEnabled = true;
				db.SaveChanges();

				TempData["SuccessMessage"] = "Cancellation Request successfully updated.";
				return RedirectToAction("List");
			}
			return View("Edit", model);
		}

		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var cancel = db.EventCancellation.Where(i => i.Id == id)
				.Select(i => new DeleteCancellationRequestModel()
				{
					Id = i.Id,
					UserId = i.UserId,
					UserName = i.User.Name,
					EventId = i.EventId,
					EventTitle = i.Event.EventTitle,
					Reasons = i.Reasons,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					VerifyId = i.VerifyId,
					VerifierName = i.Verifier.User.Name,
				}).FirstOrDefault();

			if (cancel == null)
			{
				return HttpNotFound();
			}

			return View("Delete", cancel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteCancellationRequestModel model)
		{
			EventCancellation cancellation = new EventCancellation() { Id = model.Id };
			cancellation.Display = false;

			db.EventCancellation.Attach(cancellation);
			db.Entry(cancellation).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();


			TempData["SuccessMessage"] = "Public Event successfully deleted.";
			return RedirectToAction("List");
		}
	}
}