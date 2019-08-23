using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using FEP.Model;
using FEP.WebApiModel.RnP;


namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class PublicationController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: RnP/Publication
        public async Task<ActionResult> Index()
        {
            var resPubs = await WepApiMethod.SendApiAsync<IEnumerable<ReturnPublicationModel>>(HttpVerbs.Get, $"RnP/Publication");

            if (resPubs.isSuccess)
            {
                var publications = resPubs.Data;
                if (publications == null)
                {
                    return HttpNotFound();
                }
                return View(publications);
            }
            return View();
        }

        // Show create form
        // GET: RnP/Publication/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name");
            var model = new UpdatePublicationModel();
            return View(model);
        }

        // POST: RnP/Publication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UpdatePublicationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/CreatePublication", model);

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification to others upon submission

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });

                }
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Show edit form
        // GET: RnP/Publication/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (resPub.isSuccess)
            {
                var publication = resPub.Data;
                if (publication == null)
                {
                    return HttpNotFound();
                }
                var vmpublication = new UpdatePublicationModel
                {
                    ID = publication.ID,
                    CategoryID = publication.CategoryID,
                    Author = publication.Author,
                    Coauthor = publication.Coauthor,
                    Title = publication.Title,
                    Year = publication.Year,
                    Description = publication.Description,
                    Language = publication.Language,
                    ISBN = publication.ISBN,
                    Free = publication.Free,
                    Hardcopy = publication.Hardcopy,
                    Digitalcopy = publication.Digitalcopy,
                    HDcopy = publication.HDcopy,
                    HPrice = publication.HPrice,
                    DPrice = publication.DPrice,
                    HDPrice = publication.HDPrice,
                    Pictures = publication.Pictures,
                    ProofOfApproval = publication.ProofOfApproval,
                    StockBalance = publication.StockBalance
                };
                ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", vmpublication.CategoryID);
                return View(vmpublication);
            }
            return View();
        }

        // Process edit form
        // POST: Publication/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdatePublicationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/EditPublication", model);

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification to others upon submission

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });

                }
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Show view form
        // GET: RnP/Publication/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (resPub.isSuccess)
            {
                var publication = resPub.Data;
                if (publication == null)
                {
                    return HttpNotFound();
                }
                return View(publication);
            }
            return View();
        }

        // Show delete form
        // GET: RnP/Publication/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (resPub.isSuccess)
            {
                var publication = resPub.Data;
                if (publication == null)
                {
                    return HttpNotFound();
                }
                return View(publication);
            }
            return View();
        }

        // Process delete form
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(UpdateSurveyModel model)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/DeletePublication?id={id}");

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });

                }
            }

            return View();
        }

        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}

