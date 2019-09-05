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

        /*
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
        */

        // GET: RnP/Publication
        public ActionResult Index()
        {
            return View();
        }

        // Show select publication category form
        // After category selection, user automatically redirected to creation page
        // GET: RnP/Publication/SelectCategory
        public ActionResult SelectCategory()
        {
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name");
            ViewBag.Categories = new List<PublicationCategory>(db.PublicationCategory);
            return View();
        }

        // Show create form (blank form so no api call needed)
        // GET: RnP/Publication/Create
        public ActionResult Create(int? catid)
        {
            if (catid != null)
            {
                ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", catid);
            }
            else
            {
                ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name");
            }
            var model = new UpdatePublicationModel();
            return View(model);
        }

        // Process creation form
        // After creation, user automatically redirected to review page if successful, and list page if failed.
        // POST: RnP/Publication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UpdatePublicationModel model, string Submittype)
        {
            /*
            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/TitleExists?id={null}&title={model.Title}&author={model.Author}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title/Author", "A Publication with the same Title and Author already exists in the system");
            }

            var dupISBNResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/ISBNExists?id={null}&isbn={model.ISBN}");

            if (dupISBNResponse.Data)
            {
                ModelState.AddModelError("ISBN/ISS/DOI", "A Publication with the same ISBN/ISS/DOI already exists in the system");
            }
            */

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Create", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string newid = resparray[0];
                    string title = resparray[1];

                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Create New Publication: " + title);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "New Publication titled " + title + " created successfully and saved as draft.";

                        // dashboard

                        //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                        // sms

                        return RedirectToAction("Index", "Publication", new { area = "RnP" });
                    }
                    else
                    {
                        // dashboard

                        //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                        // sms

                        return RedirectToAction("Review", "Publication", new { area = "RnP", @id = newid });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Publication.";

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
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

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
                Hardcopy = publication.Hardcopy,
                Digitalcopy = publication.Digitalcopy,
                HDcopy = publication.HDcopy,
                FreeHCopy = publication.FreeHCopy,
                FreeDCopy = publication.FreeDCopy,
                FreeHDCopy = publication.FreeHDCopy,
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

        // Process edit form
        // After editing, user automatically redirected to review page if successful, and details page if failed.
        // POST: Publication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdatePublicationModel model, string Submittype)
        {
            /*
            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/TitleExists?id={model.ID}&title={model.Title}&author={model.Author}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title/Author", "A Publication with the same Title and Author already exists in the system");
            }

            var dupISBNResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/ISBNExists?id={model.ID}&isbn={model.ISBN}");

            if (dupISBNResponse.Data)
            {
                ModelState.AddModelError("ISBN/ISS/DOI", "A Publication with the same ISBN/ISS/DOI already exists in the system");
            }
            */

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Edit", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Edit Publication: " + response.Data, model);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Publication titled " + response.Data + " updated successfully and saved as draft.";

                        // dashboard

                        //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                        // sms

                        return RedirectToAction("Index", "Publication", new { area = "RnP" });
                    }
                    else
                    {
                        // dashboard

                        //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                        // sms

                        return RedirectToAction("Review", "Publication", new { area = "RnP", @id = model.ID });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to edit Publication.";

                    return RedirectToAction("Details", "Publication", new { area = "RnP", @id = model.ID });
                }
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Show review form
        // User is redirected here after saving as draft at creation or editing page
        // GET: RnP/Publication/Review/5
        public async Task<ActionResult> Review(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication/GetForReview?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

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
                Hardcopy = publication.Hardcopy,
                Digitalcopy = publication.Digitalcopy,
                HDcopy = publication.HDcopy,
                FreeHCopy = publication.FreeHCopy,
                FreeDCopy = publication.FreeDCopy,
                FreeHDCopy = publication.FreeHDCopy,
                HPrice = publication.HPrice,
                DPrice = publication.DPrice,
                HDPrice = publication.HDPrice,
                Pictures = publication.Pictures,
                ProofOfApproval = publication.ProofOfApproval,
                StockBalance = publication.StockBalance,
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", vmpublication.CategoryID);
            return View(vmpublication);
        }

        // Process review form (i.e. submit)
        // After submitting, user automatically redirected to list page if successful, and back to review page if failed.
        // POST: Publication/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Review(UpdatePublicationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Submit", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Submit Publication: " + response.Data, model);

                    TempData["SuccessMessage"] = "Publication titled " + response.Data + " submitted successfully for verification.";

                    // dashboard

                    //SendEmail("New Publication Submitted", "A new Publication has been submitted for verification." + "\n" + "Please login to AKPK-FEP and review the submission.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit Publication.";

                    return RedirectToAction("Review", "Publication", new { area = "RnP", @id = model.ID });
                }
            }

            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Process submission from details page
        // Called for direct submission via id
        // GET: RnP/Publication/SubmitByID/5
        public async Task<ActionResult> SubmitByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/SubmitByID?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Submit Publication: " + response.Data);

                TempData["SuccessMessage"] = "Publication titled " + response.Data + " submitted successfully for verification.";

                // dashboard

                //SendEmail("New Publication Submitted", "A new Publication has been submitted for verification." + "\n" + "Please login to AKPK-FEP and review the submission.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to submit Publication.";

                return RedirectToAction("Details", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Show view form
        // From here user can do one of the following:
        // 1. Admin can edit the publication if it's not submitted yet (redirection button)
        // 2. Admin can delete the application if it's not submitted yet (direct prompt) (KIV)
        // 3. Admin can submit the application if it's not been submitted yet
        // 4. Admin can cancel the application if the status is Pending Amendment
        // GET: RnP/Publication/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication/GetForView?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

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
                Hardcopy = publication.Hardcopy,
                Digitalcopy = publication.Digitalcopy,
                HDcopy = publication.HDcopy,
                FreeHCopy = publication.FreeHCopy,
                FreeDCopy = publication.FreeDCopy,
                FreeHDCopy = publication.FreeHDCopy,
                HPrice = publication.HPrice,
                DPrice = publication.DPrice,
                HDPrice = publication.HDPrice,
                Pictures = publication.Pictures,
                ProofOfApproval = publication.ProofOfApproval,
                StockBalance = publication.StockBalance
            };

            var vmautofields = new ReturnPublicationAutofieldsModel
            {
                ID = publication.ID,
                DateAdded = publication.DateAdded,
                CreatorId = publication.CreatorId,
                RefNo = publication.RefNo,
                Status = publication.Status,
                DateCancelled = publication.DateCancelled,
                ViewCount = publication.ViewCount,
                PurchaseCount = publication.PurchaseCount,
                DmsPath = publication.DmsPath
            };

            var vmcancellation = new UpdatePublicationCancellationModel
            {
                ID = publication.ID,
                CancelRemark = publication.CancelRemark
            };

            var vmview = new UpdatePublicationViewModel
            {
                Pub = vmpublication,
                Auto = vmautofields,
                Cancellation = vmcancellation
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", vmpublication.CategoryID);
            return View(vmview);
        }

        // Show delete form (only from list page)
        // GET: RnP/Publication/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }

        // Process delete form
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"RnP/Publication/Delete?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Delete Publication: " + response.Data);

                TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully deleted.";

                // dashboard

                //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Publication.";

                return RedirectToAction("Details", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Process deletion from review page (confirmation by prompt)
        // GET: RnP/Publication/Discard/5
        public async Task<ActionResult> Discard(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"RnP/Publication/Delete?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Delete Publication: " + response.Data);

                TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully deleted.";

                // dashboard

                //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Publication.";

                return RedirectToAction("Review", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Process cancel form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(UpdatePublicationCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Cancel", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Cancel Publication: " + response.Data, model);

                    TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully cancelled.";

                    // dashboard

                    //SendEmail("New Publication Created", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to cancel Publication.";

                    return RedirectToAction("Details", "Publication", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Show verifier/approver evaluation form
        // From here user can do the following:
        // 1. Verifier/Approver can approve and submit for next approval (if applicable) if status is applicable
        // 2. Verifier/Approver can reject and require amendment if status is applicable
        // GET: RnP/Publication/Evaluate/5
        public async Task<ActionResult> Evaluate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationApprovalModel>(HttpVerbs.Get, $"RnP/Publication/GetForEvaluation?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var pubapproval = resPub.Data;

            if (pubapproval == null)
            {
                return HttpNotFound();
            }

            var publication = new ReturnPublicationModel
            {
                ID = pubapproval.Pub.ID,
                CategoryID = pubapproval.Pub.CategoryID,
                Category = pubapproval.Pub.Category,
                Author = pubapproval.Pub.Author,
                Coauthor = pubapproval.Pub.Coauthor,
                Title = pubapproval.Pub.Title,
                Year = pubapproval.Pub.Year,
                Description = pubapproval.Pub.Description,
                Language = pubapproval.Pub.Language,
                ISBN = pubapproval.Pub.ISBN,
                Hardcopy = pubapproval.Pub.Hardcopy,
                Digitalcopy = pubapproval.Pub.Digitalcopy,
                HDcopy = pubapproval.Pub.HDcopy,
                FreeHCopy = pubapproval.Pub.FreeHCopy,
                FreeDCopy = pubapproval.Pub.FreeDCopy,
                FreeHDCopy = pubapproval.Pub.FreeHDCopy,
                HPrice = pubapproval.Pub.HPrice,
                DPrice = pubapproval.Pub.DPrice,
                HDPrice = pubapproval.Pub.HDPrice,
                Pictures = pubapproval.Pub.Pictures,
                ProofOfApproval = pubapproval.Pub.ProofOfApproval,
                StockBalance = pubapproval.Pub.StockBalance,
                DateAdded = pubapproval.Pub.DateAdded,
                CreatorId = pubapproval.Pub.CreatorId,
                RefNo = pubapproval.Pub.RefNo,
                Status = pubapproval.Pub.Status,
                DmsPath = pubapproval.Pub.DmsPath
            };

            var papproval = new ReturnUpdatePublicationApprovalModel
            {
                ID = pubapproval.Approval.ID,
                PublicationID = pubapproval.Approval.PublicationID,
                Level = pubapproval.Approval.Level,
                ApproverId = pubapproval.Approval.ApproverId,
                Status  = pubapproval.Approval.Status,
                Remarks = pubapproval.Approval.Remarks,
                RequireNext  = pubapproval.Approval.RequireNext
            };

            var pevaluation = new ReturnPublicationApprovalModel
            {
                Pub = publication,
                Approval = papproval
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", publication.CategoryID);
            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", vmpublication.CategoryID);
            //ViewBag.TestItem = pubapproval.Pub.Category;
            ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", publication.CategoryID);
            return View(pevaluation);
        }

        // Process evaluation form
        // User (verifier/approver) is redirected to Index afterwards because their "work is done"
        // POST: Publication/Evaluate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Evaluate(ReturnPublicationApprovalModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Evaluate", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    if (model.Approval.Status == PublicationApprovalStatus.Approved)
                    {
                        if (model.Approval.Level == PublicationApprovalLevels.Verifier)
                        {
                            LogActivity("Verify Publication: " + response.Data, model);
                            TempData["SuccessMessage"] = "Publication titled " + response.Data + " updated as Verified.";
                            //SendEmail("Publication Approved", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                            // sms
                        }
                        else
                        {
                            LogActivity("Approve Publication: " + response.Data, model);
                            TempData["SuccessMessage"] = "Publication titled " + response.Data + " updated as Approved.";
                            //SendEmail("Publication Approved", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                            // sms
                        }
                    }
                    else
                    {
                        LogActivity("Publication Requires Amendment: " + response.Data, model);
                        TempData["SuccessMessage"] = "Publication titled " + response.Data + " updated as Requires Amendment.";
                        //SendEmail("Publication Approved", "A new Publication has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                        // sms
                    }

                    // dashboard

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to process Publication.";

                    return RedirectToAction("Evaluate", "Publication", new { area = "RnP", @id = model.Pub.ID });
                }
            }

            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", model.CategoryID);
            return View(model);
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

