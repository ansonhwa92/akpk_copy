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
	//[LogError(Modules.   )]
	public class MediaInterviewController : FEPController
	{
		private DbEntities db = new DbEntities();

		// GET: eEventMediaInterview/MediaInterview
		public ActionResult Index()
		{
			return View();
		}

		//public ActionResult List(FilterMediaInterviewModel filter)
		//{
		//	var media = db.EventMediaInterviewRequest.Where(i => i.Display)
		//		.Select(i => new DetailsMediaInterviewModel()
		//		{
		//			Id = i.Id,
		//			MediaName = i.MediaName,
		//			MediaType = i.MediaType,
		//			ContactPerson = i.ContactPerson,
		//			ContactNo = i.ContactNo,
		//			AddressStreet1 = i.AddressStreet1,
		//			AddressStreet2 = i.AddressStreet2,
		//			AddressPoscode = i.AddressPoscode,
		//			AddressCity = i.AddressCity,
		//			State = i.State,
		//			Email = i.Email,
		//			DateStart = i.DateStart,
		//			DateEnd = i.DateEnd,
		//			Time = i.Time,
		//			Language = i.Language,
		//			Topic = i.Topic,
		//			RepUserId = i.UserId,
		//			RepUserName = i.User.Name,
		//			RepDesignation = i.Designation,
		//			MediaStatus = i.MediaStatus,
		//		}).ToList();

		//	ListMediaInterviewModel model = new ListMediaInterviewModel(media);

		//	return View("List", model);
		//}

		public ActionResult List()
		{
			return View();
		}

		// GET: eEventMediaInterview/MediaInterview/Details/5
		public ActionResult Details(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DetailsMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Details", media);
		}

		// GET: eEventMediaInterview/MediaInterview/Create
		public ActionResult Create()
		{
			CreateMediaInterviewModel model = new CreateMediaInterviewModel() { };

			//var getuser = db.User.Where(c => c.Display && c.UserType == UserType.Staff)
			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(model);
		}

		// POST: eEventMediaInterview/MediaInterview/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateMediaInterviewModel model)
		{
			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					Email = model.Email,
					DateStart = model.DateStart,
					DateEnd = model.DateEnd,
					Time = model.Time,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.RepUserId,
					CreatedBy = null,
					CreatedDate = DateTime.Now,
					Display = true,
					MediaStatus = MediaStatus.New
				};
				db.EventMediaInterviewRequest.Add(media);
				db.SaveChanges();

				//save refno public event
				var refno = "EVT/" + DateTime.Now.ToString("yyMM");
				refno += "/" + media.Id.ToString("D4");
				media.RefNo = refno;

				db.Entry(media).State = EntityState.Modified;
				db.SaveChanges();

				//LogActivity();
				TempData["SuccessMessage"] = "Media Interview Request successfully created.";
				return RedirectToAction("List");
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Edit/5
		public ActionResult Edit(int? id, string origin)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new EditMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					origin = origin,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
				.Select(i => new
				{
					Id = i.Id,
					Name = i.Name
				});

			media.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);

			return View(media);
		}

		// POST: eEventMediaInterview/MediaInterview/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditMediaInterviewModel model)
		{
			if (ModelState.IsValid)
			{
				EventMediaInterviewRequest media = new EventMediaInterviewRequest
				{
					Id = model.Id,
					MediaName = model.MediaName,
					MediaType = model.MediaType,
					ContactPerson = model.ContactPerson,
					ContactNo = model.ContactNo,
					AddressStreet1 = model.AddressStreet1,
					AddressStreet2 = model.AddressStreet2,
					AddressPoscode = model.AddressPoscode,
					AddressCity = model.AddressCity,
					State = model.State,
					Email = model.Email,
					DateStart = model.DateStart,
					DateEnd = model.DateEnd,
					Time = model.Time,
					Language = model.Language,
					Topic = model.Topic,
					UserId = model.RepUserId,
				};

				db.Entry(media).State = EntityState.Modified;
				db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
				db.Entry(media).Property(x => x.Display).IsModified = false;
				db.Configuration.ValidateOnSaveEnabled = true;


				string path = "FileUploaded/";
				if (model.DocumentMedia != null)
				{
					var getIdFile = db.MediaFile.Where(s => s.EventId == model.Id).FirstOrDefault();

					if (getIdFile != null)
					{
						db.MediaFile.Remove(getIdFile);
					}

					EventFile eventfile = new EventFile
					{
						FileDescription = model.FileDescription,
						FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.DocumentMedia.FileName,
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
				TempData["SuccessMessage"] = "Media Interview Request successfully updated.";
				if (model.origin == "fromlist")
				{
					return RedirectToAction("List");
				}
				else
				{
					return RedirectToAction("Details", new { area = "eEvent", id = model.Id });
				}
			}

			var getuser = db.User.Where(c => c.Display) //temporary boleh select admin
			.Select(i => new
			{
				Id = i.Id,
				Name = i.Name
			});

			model.RepresentativeList = new SelectList(getuser, "Id", "Name", 0);
			return View(model);
		}

		// GET: eEventMediaInterview/MediaInterview/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var media = db.EventMediaInterviewRequest.Where(i => i.Id == id)
				.Select(i => new DeleteMediaInterviewModel()
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactPerson = i.ContactPerson,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Language = i.Language,
					Topic = i.Topic,
					RepUserId = i.UserId,
					RepUserName = i.User.Name,
					//RepDesignation = i.User.Designation,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					GetFileName = i.EventMediaFiles.Where(w => w.EventId == i.Id).Select(s => s.FileName).FirstOrDefault(),
				}).FirstOrDefault();

			if (media == null)
			{
				return HttpNotFound();
			}

			return View("Delete", media);
		}

		// POST: eEventMediaInterview/MediaInterview/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(DeleteMediaInterviewModel model)
		{
			EventMediaInterviewRequest media = new EventMediaInterviewRequest() { Id = model.Id };
			MediaFile file = new MediaFile() { EventId = model.Id };
			media.Display = false;
			file.Display = false;

			db.EventMediaInterviewRequest.Attach(media);
			db.Entry(media).Property(m => m.Display).IsModified = true;

			db.Configuration.ValidateOnSaveEnabled = false;
			db.SaveChanges();

			//LogActivity();
			TempData["SuccessMessage"] = "Media Interview Request successfully deleted.";
			return RedirectToAction("List");
		}
	}
}
