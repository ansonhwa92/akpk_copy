using FEP.Helper;
using FEP.Intranet.Areas.eEvent.Models;
using FEP.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	//[LogError(Modules.   )]
	public class PublicEventController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEvent/PublicEvent
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create_SelectCategory(FilterEventCategoryModel filter)
		{
			var selectCategory = db.EventCategory.Where(i => i.Display).Select(i => new DetailsEventCategoryModel()
			{
				Id = i.Id,
				CategoryName = i.CategoryName
			}).ToList();

			ListEventCategoryModel model = new ListEventCategoryModel(selectCategory);

			return View(model);
		}

		//public ActionResult List(FilterPublicEventModel filter)
		//{
		//	var e = db.PublicEvent.Where(i => i.Display && (filter.EventTitle == filter.EventTitle))
		//		.Select(i => new DetailsPublicEventModel()
		//		{
		//			Id = i.Id,
		//			EventTitle = i.EventTitle,
		//			EventObjective = i.EventObjective,
		//			EventCategoryId = i.EventCategoryId,
		//			EventCategoryName = i.EventCategory.CategoryName,
		//			StartDate = i.StartDate,
		//			EndDate = i.EndDate,
		//			Venue = i.Venue,
		//			Fee = i.Fee,
		//			EventStatus = i.EventStatus
		//		}).ToList();
		//	ListPublicEventModel model = new ListPublicEventModel(e);
		//	return View(model);
		//}

		public ActionResult List()
		{
			return View();
		}

		// GET: PublicEvent/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DetailsPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ApprovalId1 = i.ApprovalId1,
					ApprovalName1 = i.Approval1.User.Name,
					ApprovalId2 = i.ApprovalId2,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalId3 = i.ApprovalId3,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalId4 = i.ApprovalId4,
					ApprovalName4 = i.Approval4.User.Name,
					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,
					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,
					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.User.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			return View(e);
		}

		// GET: PublicEvent/Create
		public ActionResult Create(int? ctgryId)
		{
			CreatePublicEventModel model = new CreatePublicEventModel
			{
				EventStatus = EventStatus.New,
				EventCategoryId = ctgryId,
			};

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
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
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,

					EventStatus = EventStatus.New,
					EventCategoryId = model.EventCategoryId,
					SpeakerId = model.SpeakerId,
					Reasons = model.Reasons,
					Remarks = model.Remarks,
					CreatedBy = CurrentUser.UserId,
					CreatedDate = DateTime.Now,
					Display = true,
					ExternalExhibitorId = model.ExternalExhibitorId,
				};
				db.PublicEvent.Add(x);

				string path = "FileUploaded/";
				if (model.DocumentEvent != null)
				{
					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentEvent.FileName,
						FilePath = path,
						UploadedDate = DateTime.Now,
						Display = true,
						CreatedBy = CurrentUser.UserId,
						Category = FileCategory.NewFile,
						EventId = x.Id,

					};

					db.EventFile.Add(eventfile);
				};

				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Public Event successfully created.";
				return RedirectToAction("List");
			}

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
		}

		// GET: PublicEvent/Edit/5
		public ActionResult Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new EditPublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,
					origin = origin,
					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,
					
					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.User.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					//GetFileName = eventfile.FileName

				}).FirstOrDefault();

			if (e == null)
			{
				return HttpNotFound();
			}

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			e.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			e.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			e.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

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
					//EventTitle = (model.EventTitle != null) ? model.EventTitle.ToUpper() : model.EventTitle,
					EventTitle = model.EventTitle,
					EventObjective = model.EventObjective,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Venue = model.Venue,
					Fee = model.Fee,
					ParticipantAllowed = model.ParticipantAllowed,
					TargetedGroup = model.TargetedGroup,
					EventStatus = model.EventStatus,
					EventCategoryId = model.EventCategoryId,
					Reasons = model.Reasons,
					Remarks = model.Remarks,
					SpeakerId = model.SpeakerId,
					ExternalExhibitorId = model.ExternalExhibitorId,
				};

				db.Entry(eEvent).State = EntityState.Modified;
				db.Entry(eEvent).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(eEvent).Property(x => x.Display).IsModified = false;
				db.Configuration.ValidateOnSaveEnabled = true;

				string path = "FileUploaded/";
				if (model.DocumentEvent != null)
				{
					var getIdFile = db.EventFile.Where(s => s.EventId == model.Id).FirstOrDefault();

					if (getIdFile != null)
					{
						db.EventFile.Remove(getIdFile);
					}

					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentEvent.FileName,
						FilePath = path,
						UploadedDate = DateTime.Now,
						Display = true,
						CreatedBy = CurrentUser.UserId,
						Category = FileCategory.NewFile,
						EventId = model.Id,
						Id = getIdFile.Id
					};

					db.EventFile.Add(eventfile);
				};
				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Public Event successfully updated.";

				if (model.origin == "fromlist")
				{
					return RedirectToAction("List");
				}
				else
				{
					return RedirectToAction("Details", new { area = "eEvent", id = model.Id });
				}
			}

			var getcategory = db.EventCategory.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.CategoryName
			});

			var getspeaker = db.EventSpeaker.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.User.Name
			});

			var getexhibitor = db.EventExternalExhibitor.Where(c => c.Display).Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.CategoryList = new SelectList(getcategory, "Id", "Name", 0);
			model.SpeakerList = new SelectList(getspeaker, "Id", "Name", 0);
			model.ExternalExhibitorList = new SelectList(getexhibitor, "Id", "Name", 0);

			return View(model);
		}

		// GET: PublicEvent/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var eventfile = db.EventFile.Where(w => w.EventId == id).FirstOrDefault();

			var e = db.PublicEvent.Where(i => i.Id == id)
				.Select(i => new DeletePublicEventModel()
				{
					Id = i.Id,
					EventTitle = i.EventTitle,
					EventObjective = i.EventObjective,
					StartDate = i.StartDate,
					EndDate = i.EndDate,
					Venue = i.Venue,
					Fee = i.Fee,
					ParticipantAllowed = i.ParticipantAllowed,
					TargetedGroup = i.TargetedGroup,
					ApprovalId1 = i.ApprovalId1,
					ApprovalId2 = i.ApprovalId2,
					ApprovalId3 = i.ApprovalId3,
					ApprovalId4 = i.ApprovalId4,

					ApprovalName1 = i.Approval1.User.Name,
					ApprovalName2 = i.Approval2.User.Name,
					ApprovalName3 = i.Approval3.User.Name,
					ApprovalName4 = i.Approval4.User.Name,

					EventStatus = i.EventStatus,
					EventCategoryId = i.EventCategoryId,
					EventCategoryName = i.EventCategory.CategoryName,
					Reasons = i.Reasons,
					Remarks = i.Remarks,

					SpeakerId = i.SpeakerId,
					SpeakerName = i.EventSpeaker.User.Name,

					ExternalExhibitorId = i.ExternalExhibitorId,
					ExternalExhibitorName = i.ExternalExhibitor.User.Name,
					GetFileName = i.EventFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
					//GetFileName = eventfile.FileName
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

			//LogActivity();
			TempData["SuccessMessage"] = "Public Event successfully deleted.";
			return RedirectToAction("List");
		}
	}
}