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
    public class PublicationController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: RnP/Publication
        public ActionResult Index()
        {
            //return View();
            var publications = db.Publication.Include(p => p.Category);
            //publications = db.Publication.Where()
            return View(publications.ToList());
        }

        // GET: RnP/Publication/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publication.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // GET: RnP/Publication/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name");
            return View();
        }

        // POST: RnP/Publication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Author,Coauthor,Title,Year,Description,Language,ISBN,Free,Hardcopy,Digitalcopy,HDcopy,HPrice,DPrice,HDPrice,ProofOfApproval,StockBalance,WithdrawalReason,ProofOfWithdrawal,DateAdded,Status,Withdrawal,ViewCount,PurchaseCount")] Publication publication, string Submittype)
        {
            if (ModelState.IsValid)
            {
                ModelState.Remove("WithdrawalReason");
                ModelState.Remove("ProofOfWithdrawal");
                ModelState.Remove("DateAdded");
                ModelState.Remove("Status");
                ModelState.Remove("WStatus");
                ModelState.Remove("ViewCount");
                ModelState.Remove("PurchaseCount");
                publication.WithdrawalReason = "";
                publication.ProofOfWithdrawal = "";
                publication.DateAdded = DateTime.Today;
                publication.WStatus = PublicationWithdrawalStatus.None;
                publication.ViewCount = 0;
                publication.PurchaseCount = 0;
                switch (Submittype)
                {
                    case "Save":
                        publication.Status = PublicationStatus.New;
                        break;
                    case "Submit":
                        publication.Status = PublicationStatus.Submitted;
                        break;
                }
                //publication.Status = PublicationStatus.New;
                db.Publication.Add(publication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", publication.CategoryID);
            return View(publication);
        }

        // GET: Publication/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publication.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", publication.CategoryID);
            return View(publication);
        }

        // POST: Publication/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Author,Coauthor,Title,Year,Description,Language,ISBN,Free,Hardcopy,Digitalcopy,HDcopy,HPrice,DPrice,HDPrice,ProofOfApproval,StockBalance,WithdrawalReason,ProofOfWithdrawal,DateAdded,Status,Withdrawal,ViewCount,PurchaseCount")] Publication publication, string Submittype)
        {
            // ,WithdrawalReason,ProofOfWithdrawal,DateAdded,Status,Withdrawal,ViewCount,PurchaseCount
            if (ModelState.IsValid)
            {
                ModelState.Remove("WithdrawalReason");
                ModelState.Remove("ProofOfWithdrawal");
                ModelState.Remove("DateAdded");
                ModelState.Remove("Status");
                ModelState.Remove("WStatus");
                ModelState.Remove("ViewCount");
                ModelState.Remove("PurchaseCount");
                publication.WithdrawalReason = "";
                publication.ProofOfWithdrawal = "";
                publication.DateAdded = DateTime.Today;
                publication.WStatus = PublicationWithdrawalStatus.None;
                publication.ViewCount = 0;
                publication.PurchaseCount = 0;
                switch (Submittype)
                {
                    case "Save":
                        publication.Status = PublicationStatus.New;
                        break;
                    case "Submit":
                        publication.Status = PublicationStatus.Submitted;
                        break;
                }
                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", publication.CategoryID);
            return View(publication);
        }

        // GET: Publication/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publication.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publication publication = db.Publication.Find(id);
            db.Publication.Remove(publication);
            db.SaveChanges();
            return RedirectToAction("Index");
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

