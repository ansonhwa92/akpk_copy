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
using FEP.WebApiModel.SLAReminder;
using Newtonsoft.Json;


namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class PublicationController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: RnP/Publication
        [HasAccess(UserAccess.RnPPublicationView)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: RnP/Publication
        [HasAccess(UserAccess.RnPPublicationView)]
        [HttpPost]
        public async Task<ActionResult> Index(FilterPublicationModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"RnP/Publication/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        //menu
        [ChildActionOnly]
        public ActionResult _Menu()
        {
            return PartialView();
        }

        // Show select publication category form
        // After category selection, user automatically redirected to creation page
        // GET: RnP/Publication/SelectCategory
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> SelectCategory()
        {
            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.Categories = categories;
            return View();
        }

        // Show create form (blank form so no api call needed)
        // GET: RnP/Publication/Create
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> Create(int? catid)
        {
            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            if (catid != null)
            {
                ViewBag.CategoryId = new SelectList(categories, "Id", "Name", catid);
            }
            else
            {
                ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            }

            var resYear = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/GetSettingsMinimumPublishedYear");

            
            if (!resYear.isSuccess)
            {
                ViewBag.MinimumYear = 1900;
            }
            else
            {
                ViewBag.MinimumYear = resYear.Data;
            }
            

            //ViewBag.MinimumYear = GetMinimumPublishedYear();

            var model = new CreatePublicationModel();
            return View(model);
        }

        // Process creation form
        // After creation, user automatically redirected to review page if successful, and list page if failed.
        // POST: RnP/Publication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePublicationModel model, string Submittype)
        {
            if (model.ProofOfApproval.Count() == 0 && model.ProofOfApprovalFiles.Count() == 0)
            {
                ModelState.AddModelError("ProofOfApproval", "Please upload at least one (1) Proof Of Approval");
            }

            if (model.DigitalPublications.Count() == 0 && model.DigitalPublicationFiles.Count() == 0 && (model.Digitalcopy == true || model.HDcopy == true))
            {
                ModelState.AddModelError("DigitalPublications", "Please upload the Digital copy of the Publication");
            }

            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/TitleExists?id={null}&title={model.Title}&author={model.Author}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title", "A Publication with the same Title and Author already exists in the system");
            }

            var dupISBNResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/ISBNExists?id={null}&isbn={model.ISBN}");

            if (dupISBNResponse.Data)
            {
                ModelState.AddModelError("ISBN", "A Publication with the same ISBN/ISSN/DOI already exists in the system");
            }

            if (ModelState.IsValid)
            {
                var apimodel = new CreatePublicationModelNoFile
                {
                    CategoryID = model.CategoryID,
                    Author = model.Author,
                    Coauthor = model.Coauthor,
                    Title = model.Title,
                    Year = model.Year,
                    Description = model.Description,
                    Language = model.Language,
                    ISBN = model.ISBN,
                    Hardcopy = model.Hardcopy,
                    Digitalcopy = model.Digitalcopy,
                    HDcopy = model.HDcopy,
                    FreeHCopy = model.FreeHCopy,
                    FreeDCopy = model.FreeDCopy,
                    FreeHDCopy = model.FreeHDCopy,
                    HPrice = model.HPrice,
                    DPrice = model.DPrice,
                    HDPrice = model.HDPrice,
                    StockBalance = model.StockBalance,
                    CreatorId = model.CreatorId,
                    CoverPictures = model.CoverPictures,
                    AuthorPictures = model.AuthorPictures,
                    ProofOfApproval = model.ProofOfApproval,
                    DigitalPublications = model.DigitalPublications
                };

                //attachment 1: cover pics
                if (model.CoverPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.CoverPictureFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.CoverFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 2: author pics
                if (model.AuthorPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.AuthorPictureFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.AuthorFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 3: proof pics
                if (model.ProofOfApprovalFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.ProofOfApprovalFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.ProofFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 4: digital publication
                if (model.DigitalPublicationFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.DigitalPublicationFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.DigitalFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Create", apimodel);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string newid = resparray[0];
                    string title = resparray[1];

                    await UploadImageFiles(int.Parse(newid), model.CoverPictureFiles, model.AuthorPictureFiles);

                    await LogActivity(Modules.RnP, "Create New Publication: " + title);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "New Publication titled " + title + " created successfully and saved as draft.";

                        return RedirectToAction("Index", "Publication", new { area = "RnP" });
                    }
                    else
                    {
                        return RedirectToAction("Review", "Publication", new { area = "RnP", @id = newid });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Publication.";

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Show edit form
        // GET: RnP/Publication/Edit/5
        [HasAccess(UserAccess.RnPPublicationEdit)]
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

            var vmpublication = new EditPublicationModel
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
                StockBalance = publication.StockBalance,
                CreatorId = publication.CreatorId,
                CoverPictures = publication.CoverPictures,
                AuthorPictures = publication.AuthorPictures,
                ProofOfApproval = publication.ProofOfApproval,
                DigitalPublications = publication.DigitalPublications
            };

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            
            var resYear = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/GetSettingsMinimumPublishedYear");

            if (!resYear.isSuccess)
            {
                ViewBag.MinimumYear = 1900;
            }
            else
            {
                ViewBag.MinimumYear = resYear.Data;
            }
            

            //ViewBag.MinimumYear = GetMinimumPublishedYear();

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", vmpublication.CategoryID);

            return View(vmpublication);
        }

        // Process edit form
        // After editing, user automatically redirected to review page if successful, and details page if failed.
        // POST: Publication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPublicationModel model, string Submittype)
        {
            if (model.ProofOfApproval.Count() == 0 && model.ProofOfApprovalFiles.Count() == 0)
            {
                ModelState.AddModelError("ProofOfApproval", "Please upload at least one (1) Proof Of Approval");
            }

            if (model.DigitalPublications.Count() == 0 && model.DigitalPublicationFiles.Count() == 0 && (model.Digitalcopy == true || model.HDcopy == true))
            {
                ModelState.AddModelError("DigitalPublications", "Please upload the Digital copy of the Publication");
            }

            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/TitleExists?id={model.ID}&title={model.Title}&author={model.Author}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title", "A Publication with the same Title and Author already exists in the system");
            }

            var dupISBNResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/ISBNExists?id={model.ID}&isbn={model.ISBN}");

            if (dupISBNResponse.Data)
            {
                ModelState.AddModelError("ISBN", "A Publication with the same ISBN/ISSN/DOI already exists in the system");
            }

            if (ModelState.IsValid)
            {
                var apimodel = new EditPublicationModelNoFile
                {
                    ID = model.ID,
                    CategoryID = model.CategoryID,
                    Author = model.Author,
                    Coauthor = model.Coauthor,
                    Title = model.Title,
                    Year = model.Year,
                    Description = model.Description,
                    Language = model.Language,
                    ISBN = model.ISBN,
                    Hardcopy = model.Hardcopy,
                    Digitalcopy = model.Digitalcopy,
                    HDcopy = model.HDcopy,
                    FreeHCopy = model.FreeHCopy,
                    FreeDCopy = model.FreeDCopy,
                    FreeHDCopy = model.FreeHDCopy,
                    HPrice = model.HPrice,
                    DPrice = model.DPrice,
                    HDPrice = model.HDPrice,
                    StockBalance = model.StockBalance,
                    CreatorId = model.CreatorId,
                    CoverPictures = model.CoverPictures,
                    AuthorPictures = model.AuthorPictures,
                    ProofOfApproval = model.ProofOfApproval,
                    DigitalPublications = model.DigitalPublications
                };

                //attachment 1: cover pics
                if (model.CoverPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.CoverPictureFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.CoverFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 2: author pics
                if (model.AuthorPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.AuthorPictureFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.AuthorFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 3: proof pics
                if (model.ProofOfApprovalFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.ProofOfApprovalFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.ProofFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                //attachment 4: digital publication
                if (model.DigitalPublicationFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.DigitalPublicationFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.DigitalFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Edit", apimodel);

                if (response.isSuccess)
                {
                    await UpdateImageFileCover(model.ID, model.CoverPictureFiles, model.CoverPictures);

                    await UpdateImageFileAuthor(model.ID, model.AuthorPictureFiles, model.AuthorPictures);

                    await LogActivity(Modules.RnP, "Edit Publication: " + response.Data, model);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Publication titled " + response.Data + " updated successfully and saved as draft.";

                        return RedirectToAction("Index", "Publication", new { area = "RnP" });
                    }
                    else
                    {
                       return RedirectToAction("Review", "Publication", new { area = "RnP", @id = model.ID });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to edit Publication.";

                    return RedirectToAction("Details", "Publication", new { area = "RnP", @id = model.ID });
                }
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", model.CategoryID);
            return View(model);
        }

        // Show review form
        // User is redirected here after saving as draft at creation or editing page
        // GET: RnP/Publication/Review/5
        [HasAccess(UserAccess.RnPPublicationEdit)]
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

            var vmpublication = new DetailsPublicationModel
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
                StockBalance = publication.StockBalance,
                CreatorId = publication.CreatorId,
                CoverPictures = publication.CoverPictures,
                AuthorPictures = publication.AuthorPictures,
                ProofOfApproval = publication.ProofOfApproval,
                DigitalPublications = publication.DigitalPublications
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", vmpublication.CategoryID);
            return View(vmpublication);
        }

        // Process review form (i.e. submit)
        // After submitting, user automatically redirected to list page if successful, and back to review page if failed.
        // POST: Publication/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Review(DetailsPublicationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Submit", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string refno = resparray[1];

                    await LogActivity(Modules.RnP, "Submit Publication: " + title, model);

                    TempData["SuccessMessage"] = "Publication titled " + title + " submitted successfully for verification.";

                    await SendNotification(model.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Creation, model.Title, model.Author, refno, "Publication Submitted", PublicationApprovalStatus.None, false);

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit Publication.";

                    return RedirectToAction("Review", "Publication", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Process submission from details page
        // Called for direct submission via id
        // GET: RnP/Publication/SubmitByID/5
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> SubmitByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/SubmitByID?id={id}");

            if (response.isSuccess)
            {
                string[] resparray = response.Data.Split('|');
                string title = resparray[0];
                string author = resparray[1];
                string refno = resparray[2];

                await LogActivity(Modules.RnP, "Submit Publication: " + title);

                TempData["SuccessMessage"] = "Publication titled " + title + " submitted successfully for verification.";

                await SendNotification(id.Value, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Creation, title, author, refno, "Publication Submitted", PublicationApprovalStatus.None, false);

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to submit Publication.";

                return RedirectToAction("Details", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Process publishing from details page
        // Called for direct publishing via id
        // GET: RnP/Publication/PublishByID/5
        [HasAccess(UserAccess.RnPPublicationPublish)]
        public async Task<ActionResult> PublishByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/PublishByID?id={id}");

            if (response.isSuccess)
            {
                string[] resparray = response.Data.Split('|');
                string title = resparray[0];
                string author = resparray[1];
                string refno = resparray[2];

                await LogActivity(Modules.RnP, "Publish Publication: " + title);

                TempData["SuccessMessage"] = "Publication titled " + title + " published successfully.";

                await SendNotification(id.Value, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Publication, title, author, refno, "Publication Published", PublicationApprovalStatus.None, false);

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to publish Publication.";

                return RedirectToAction("Details", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Process withdraw form (modal dialog from details)
        // Called via ajax and returns empty string or "error"
        // POST: Publication/Withdraw/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAccess(UserAccess.RnPPublicationWithdraw)]
        public async Task<string> Withdraw(UpdatePublicationWithdrawalModel model)
        {
            if (model.ProofOfWithdrawal.Count() == 0 && model.ProofOfWithdrawalFiles.Count() == 0)
            {
                ModelState.AddModelError("ProofOfWithdrawal", "Please upload at least one (1) Proof Of Withdrawal");
            }

            if (ModelState.IsValid)
            {
                var apimodel = new UpdatePublicationWithdrawalModelNoFile
                {
                    ID = model.ID,
                    WithdrawalReason = model.WithdrawalReason,
                    ProofOfWithdrawal = model.ProofOfWithdrawal
                };

                //attachment 1: proofs
                if (model.ProofOfWithdrawalFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.ProofOfWithdrawalFiles.ToList(), CurrentUser.UserId, "publication");
                    if (files != null)
                    {
                        apimodel.ProofFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Withdraw", apimodel);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string author = resparray[1];
                    string refno = resparray[2];

                    await LogActivity(Modules.RnP, "Withdraw Publication: " + title, model);

                    // no notification because it's handled by the ajax caller
                    //TempData["SuccessMessage"] = "Publication titled " + response.Data + " requested to be withdrawn.";

                    await SendNotification(model.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Withdrawal, title, author, refno, "Publication Withdrawal Submitted", PublicationApprovalStatus.None, false);

                    return "";
                }
                else
                {
                    //TempData["SuccessMessage"] = "Failed to submit Publication.";

                    return "error";
                }
            }

            return "error";
        }

        // Process cancel form (modal dialog from details)
        // Called via ajax and returns empty string or "error"
        // POST: Publication/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<string> Cancel(UpdatePublicationCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/Cancel", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string author = resparray[1];
                    string refno = resparray[2];

                    await LogActivity(Modules.RnP, "Cancel Publication: " + title, model);

                    // no notification because it's handled by the ajax caller
                    //TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully cancelled.";

                    await SendNotification(model.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Cancellation, title, author, refno, "Publication Cancelled", PublicationApprovalStatus.None, false);

                    // dashboard

                    return "";
                }
                else
                {
                    //TempData["SuccessMessage"] = "Failed to cancel Publication.";

                    return "error";
                }
            }

            return "error";
        }

        // Process cancel withdrawal form (modal dialog from details)
        // Called via ajax and returns empty string or "error"
        // POST: Publication/CancelWithdrawal/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAccess(UserAccess.RnPPublicationWithdraw)]
        public async Task<string> CancelWithdrawal(UpdatePublicationCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/CancelWithdrawal", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string author = resparray[1];
                    string refno = resparray[2];

                    await LogActivity(Modules.RnP, "Cancel Publication Withdrawal: " + title, model);

                    // no notification because it's handled by the ajax caller
                    //TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully cancelled.";

                    await SendNotification(model.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Publication_Withdrawal_Cancellation, title, author, refno, "Publication Withdrawal Cancelled", PublicationApprovalStatus.None, false);

                    // dashboard

                    return "";
                }
                else
                {
                    //TempData["SuccessMessage"] = "Failed to cancel Publication.";

                    return "error";
                }
            }

            return "error";
        }

        // Show view form
        // From here user can do one of the following:
        // 1. Admin can edit the publication if it's not submitted yet (redirection button)
        // 2. Admin can delete the application if it's not submitted yet (direct prompt) (KIV)
        // 3. Admin can submit the application if it's not been submitted yet
        // 4. Admin can cancel the application if the status is Pending Amendment
        // GET: RnP/Publication/Details/5
        [HasAccess(UserAccess.RnPPublicationView)]
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

            // redirect for eavluation if applicable

            if ((CurrentUser.HasAccess(UserAccess.RnPPublicationVerify)) || (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove1)) || (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove2)) || (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove3)))
            {
                // if approvers, check pending approval

                var resApp = await WepApiMethod.SendApiAsync<PublicationApprovalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextApproval?id={id}");

                if (resApp.isSuccess)
                {
                    var nextapp = resApp.Data;

                    if (nextapp != null)
                    {
                        if (((nextapp.Level == PublicationApprovalLevels.Verifier) && (CurrentUser.HasAccess(UserAccess.RnPPublicationVerify))) || ((nextapp.Level == PublicationApprovalLevels.Approver1) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove1))) || ((nextapp.Level == PublicationApprovalLevels.Approver2) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove2))) || ((nextapp.Level == PublicationApprovalLevels.Approver3) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove3))))
                        {
                            if ((publication.Status == PublicationStatus.Submitted) || (publication.Status == PublicationStatus.Verified))
                            {
                                return RedirectToAction("Evaluate", "Publication", new { area = "RnP", @id = id });
                            }
                        }
                    }
                }
                else
                {
                    await LogError(Modules.RnP, "Web API Failure: Call to GetNextApproval failed");
                }

                // if approvers, also check pending withdrawal approval

                var resWithApp = await WepApiMethod.SendApiAsync<PublicationWithdrawalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextWithdrawalApproval?id={id}");

                if (resWithApp.isSuccess)
                {
                    var nextapp = resWithApp.Data;

                    if (nextapp != null)
                    {
                        if (((nextapp.Level == PublicationApprovalLevels.Verifier) && (CurrentUser.HasAccess(UserAccess.RnPPublicationVerify))) || ((nextapp.Level == PublicationApprovalLevels.Approver1) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove1))) || ((nextapp.Level == PublicationApprovalLevels.Approver2) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove2))) || ((nextapp.Level == PublicationApprovalLevels.Approver3) && (CurrentUser.HasAccess(UserAccess.RnPPublicationApprove3))))
                        {
                            if ((publication.Status == PublicationStatus.WithdrawalSubmitted) || (publication.Status == PublicationStatus.WithdrawalVerified))
                            {
                                return RedirectToAction("EvaluateWithdrawal", "Publication", new { area = "RnP", @id = id });
                            }
                        }
                    }
                }
                else
                {
                    await LogError(Modules.RnP, "Web API Failure: Call to GetNextApproval failed");
                }
            }

            var vmpublication = new DetailsPublicationModel
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
                StockBalance = publication.StockBalance,
                CreatorId = publication.CreatorId,
                CreatorName = publication.CreatorName,
                CoverPictures = publication.CoverPictures,
                AuthorPictures = publication.AuthorPictures,
                ProofOfApproval = publication.ProofOfApproval,
                DigitalPublications = publication.DigitalPublications
            };

            var vmautofields = new ReturnPublicationAutofieldsModel
            {
                ID = publication.ID,
                WithdrawalDate = publication.WithdrawalDate,
                DateAdded = publication.DateAdded,
                CreatorId = publication.CreatorId,
                RefNo = publication.RefNo,
                Status = publication.Status,
                DateCancelled = publication.DateCancelled,
                ViewCount = publication.ViewCount,
                PurchaseCount = publication.PurchaseCount,
                DownloadCount = publication.DownloadCount,
                DmsPath = publication.DmsPath
            };

            var vmcancellation = new UpdatePublicationCancellationModel
            {
                ID = publication.ID,
                CancelRemark = publication.CancelRemark
            };

            var vmwithdrawal = new UpdatePublicationWithdrawalModel
            {
                ID = publication.ID,
                WithdrawalReason = publication.WithdrawalReason,
                ProofOfWithdrawal = publication.ProofOfWithdrawal,
            };

            var vmview = new UpdatePublicationViewModel
            {
                Pub = vmpublication,
                Auto = vmautofields,
                Withdrawal = vmwithdrawal,
                Cancellation = vmcancellation
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            var resWith = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetWithdrawalHistory?id={id}");

            if (resWith.isSuccess)
            {
                ViewBag.Withdrawal = resWith.Data;
            }

            ViewBag.ApprovalStage = "";

            var resNext = await WepApiMethod.SendApiAsync<PublicationApprovalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextApproval?id={id}");

            if (resNext.isSuccess)
            {
                if (resNext.Data != null)
                {
                    if (resNext.Data.Level == PublicationApprovalLevels.Approver1)
                    {
                        ViewBag.ApprovalStage = "1";
                    }
                    else if (resNext.Data.Level == PublicationApprovalLevels.Approver2)
                    {
                        ViewBag.ApprovalStage = "2";
                    }
                    else if (resNext.Data.Level == PublicationApprovalLevels.Approver3)
                    {
                        ViewBag.ApprovalStage = "3";
                    }
                }
            }

            ViewBag.WithdrawalStage = "";

            var resNextW = await WepApiMethod.SendApiAsync<PublicationWithdrawalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextWithdrawalApproval?id={id}");

            if (resNextW.isSuccess)
            {
                if (resNextW.Data != null)
                {
                    if (resNextW.Data.Level == PublicationApprovalLevels.Approver1)
                    {
                        ViewBag.WithdrawalStage = "1";
                    }
                    else if (resNextW.Data.Level == PublicationApprovalLevels.Approver2)
                    {
                        ViewBag.WithdrawalStage = "2";
                    }
                    else if (resNextW.Data.Level == PublicationApprovalLevels.Approver3)
                    {
                        ViewBag.WithdrawalStage = "3";
                    }
                }
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            //ViewBag.CategoryId = new SelectList(db.PublicationCategory, "Id", "Name", vmpublication.CategoryID);
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", vmpublication.CategoryID);
            return View(vmview);
        }

        // Show delete form (only from list page)
        // GET: RnP/Publication/Delete/5
        [HasAccess(UserAccess.RnPPublicationEdit)]
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
                await LogActivity(Modules.RnP, "Delete Publication: " + response.Data);

                TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully deleted.";

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
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> Discard(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"RnP/Publication/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.RnP, "Delete Publication: " + response.Data);

                TempData["SuccessMessage"] = "Publication titled " + response.Data + " successfully deleted.";

                return RedirectToAction("Index", "Publication", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Publication.";

                return RedirectToAction("Review", "Publication", new { area = "RnP", @id = id });
            }
        }

        // Show verifier/approver evaluation form
        // From here user can do the following:
        // 1. Verifier/Approver can approve and submit for next approval (if applicable) if status is applicable
        // 2. Verifier/Approver can reject and require amendment if status is applicable
        // GET: RnP/Publication/Evaluate/5
        //[HasAccess(UserAccess.RnPPublicationVerify)]
        //[HasAccess(UserAccess.RnPPublicationApprove1)]
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
                StockBalance = pubapproval.Pub.StockBalance,
                DateAdded = pubapproval.Pub.DateAdded,
                CreatorId = pubapproval.Pub.CreatorId,
                RefNo = pubapproval.Pub.RefNo,
                Status = pubapproval.Pub.Status,
                DmsPath = pubapproval.Pub.DmsPath,
                CreatorName = pubapproval.Pub.CreatorName,
                CoverPictures = pubapproval.Pub.CoverPictures,
                AuthorPictures = pubapproval.Pub.AuthorPictures,
                ProofOfApproval = pubapproval.Pub.ProofOfApproval,
                DigitalPublications = pubapproval.Pub.DigitalPublications
            };

            var papproval = new ReturnUpdatePublicationApprovalModel
            {
                ID = pubapproval.Approval.ID,
                PublicationID = pubapproval.Approval.PublicationID,
                Level = pubapproval.Approval.Level,
                ApproverId = pubapproval.Approval.ApproverId,
                Status = pubapproval.Approval.Status,
                Remarks = pubapproval.Approval.Remarks,
                RequireNext = pubapproval.Approval.RequireNext
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

            var resWith = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetWithdrawalHistory?id={id}");

            if (resWith.isSuccess)
            {
                ViewBag.Withdrawal = resWith.Data;
            }

            ViewBag.ApprovalStage = "";

            var resNext = await WepApiMethod.SendApiAsync<PublicationApprovalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextApproval?id={id}");

            if (resNext.isSuccess)
            {
                if (resNext.Data.Level == PublicationApprovalLevels.Approver1)
                {
                    ViewBag.ApprovalStage = "1";
                }
                else if (resNext.Data.Level == PublicationApprovalLevels.Approver2)
                {
                    ViewBag.ApprovalStage = "2";
                }
                else if (resNext.Data.Level == PublicationApprovalLevels.Approver3)
                {
                    ViewBag.ApprovalStage = "3";
                }
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", publication.CategoryID);
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
                    string[] resparray = response.Data.Split('|');
                    int pid = int.Parse(resparray[0]);
                    string title = resparray[1];
                    string author = resparray[2];
                    string refno = resparray[3];

                    if (model.Approval.Status == PublicationApprovalStatus.Approved)
                    {
                        if (model.Approval.Level == PublicationApprovalLevels.Verifier)
                        {
                            await LogActivity(Modules.RnP, "Verify Publication: " + title, model);
                            TempData["SuccessMessage"] = "Publication titled " + title + " updated as Pending Approval 1.";

                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Publication_Creation, title, author, refno, "Verified and Pending Approval", model.Approval.Status, model.Approval.RequireNext);
                        }
                        else
                        {
                            await LogActivity(Modules.RnP, "Approve Publication: " + title, model);

                            if (model.Approval.Level == PublicationApprovalLevels.Approver1)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    TempData["SuccessMessage"] = "Publication titled " + title + " updated as Pending Approval 2.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_1, title, author, refno, "Approved by 1st-Level Approver and Pending 2nd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    TempData["SuccessMessage"] = "Publication titled " + title + " updated as Approved.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_1, title, author, refno, "Approved by 1st-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == PublicationApprovalLevels.Approver2)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    TempData["SuccessMessage"] = "Publication titled " + title + " updated as Pending Approval 3.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_2, title, author, refno, "Approved by 2nd-Level Approver and Pending 3rd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    TempData["SuccessMessage"] = "Publication titled " + title + " updated as Approved.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_2, title, author, refno, "Approved by 2nd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == PublicationApprovalLevels.Approver3)
                            {
                                TempData["SuccessMessage"] = "Publication titled " + title + " updated as Approved.";
                                await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_3, title, author, refno, "Approved by 3rd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                            }
                        }
                    }
                    else
                    {
                        await LogActivity(Modules.RnP, "Publication Requires Amendment: " + title, model);
                        TempData["SuccessMessage"] = "Publication titled " + title + " updated as Requires Amendment.";

                        if (model.Approval.Level == PublicationApprovalLevels.Verifier)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Publication_Creation, title, author, refno, "Amendment Requested by Verifier", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver1)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_1, title, author, refno, "Amendment Requested by 1st-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver2)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_2, title, author, refno, "Amendment Requested by 2nd-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver3)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Creation_3, title, author, refno, "Amendment Requested by 3rd-Level Approver", model.Approval.Status, false);
                        }
                    }

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to process Publication.";

                    return RedirectToAction("Evaluate", "Publication", new { area = "RnP", @id = model.Approval.PublicationID });
                }
            }

            return View(model);
        }

        // Show verifier/approver withdrawal evaluation form
        // From here user can do the following:
        // 1. Verifier/Approver can approve and submit for next approval (if applicable) if status is applicable
        // 2. Verifier/Approver can reject and require amendment if status is applicable
        // GET: RnP/Publication/EvaluateWithdrawal/5
        //[HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> EvaluateWithdrawal(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationWithdrawalModel>(HttpVerbs.Get, $"RnP/Publication/GetForWithdrawalEvaluation?id={id}");

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
                StockBalance = pubapproval.Pub.StockBalance,
                WithdrawalDate = pubapproval.Pub.WithdrawalDate,
                DateAdded = pubapproval.Pub.DateAdded,
                CreatorId = pubapproval.Pub.CreatorId,
                RefNo = pubapproval.Pub.RefNo,
                Status = pubapproval.Pub.Status,
                DmsPath = pubapproval.Pub.DmsPath,
                CoverPictures = pubapproval.Pub.CoverPictures,
                AuthorPictures = pubapproval.Pub.AuthorPictures,
                ProofOfApproval = pubapproval.Pub.ProofOfApproval,
                ProofOfWithdrawal = pubapproval.Pub.ProofOfWithdrawal,
                DigitalPublications = pubapproval.Pub.DigitalPublications
            };

            var pwithdrawal = new UpdatePublicationWithdrawalModel
            {
                ID = pubapproval.Pub.ID,
                WithdrawalReason = pubapproval.Pub.WithdrawalReason,
                ProofOfWithdrawal = pubapproval.Pub.ProofOfWithdrawal
            };

            var papproval = new ReturnUpdatePublicationApprovalModel
            {
                ID = pubapproval.Approval.ID,
                PublicationID = pubapproval.Approval.PublicationID,
                Level = pubapproval.Approval.Level,
                ApproverId = pubapproval.Approval.ApproverId,
                Status = pubapproval.Approval.Status,
                Remarks = pubapproval.Approval.Remarks,
                RequireNext = pubapproval.Approval.RequireNext
            };

            var pevaluation = new ReturnPublicationWithdrawalModel
            {
                Pub = publication,
                Withdrawal = pwithdrawal,
                Approval = papproval
            };

            // TODO: show approval history for publication only or withdrawal only or both??

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<PublicationApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            var resWith = await WepApiMethod.SendApiAsync<IEnumerable<PublicationWithdrawalHistoryModel>>(HttpVerbs.Get, $"RnP/Publication/GetWithdrawalHistory?id={id}");

            if (resWith.isSuccess)
            {
                ViewBag.Withdrawal = resWith.Data;
            }

            ViewBag.WithdrawalStage = "";

            var resNextW = await WepApiMethod.SendApiAsync<PublicationWithdrawalHistoryModel>(HttpVerbs.Get, $"RnP/Publication/GetNextWithdrawalApproval?id={id}");

            if (resNextW.isSuccess)
            {
                if (resNextW.Data.Level == PublicationApprovalLevels.Approver1)
                {
                    ViewBag.WithdrawalStage = "1";
                }
                else if (resNextW.Data.Level == PublicationApprovalLevels.Approver2)
                {
                    ViewBag.WithdrawalStage = "2";
                }
                else if (resNextW.Data.Level == PublicationApprovalLevels.Approver3)
                {
                    ViewBag.WithdrawalStage = "3";
                }
            }

            var resCat = await WepApiMethod.SendApiAsync<List<ReturnPublicationCategory>>(HttpVerbs.Get, $"RnP/Publication/GetCategories");

            if (!resCat.isSuccess)
            {
                return HttpNotFound();
            }

            var categories = resCat.Data;

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", publication.CategoryID);
            return View(pevaluation);
        }

        // Process withdrawal evaluation form
        // User (verifier/approver) is redirected to Index afterwards because their "work is done"
        // POST: Publication/EvaluateWithdrawal/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EvaluateWithdrawal(ReturnPublicationWithdrawalModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Publication/EvaluateWithdrawal", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    int pid = int.Parse(resparray[0]);
                    string title = resparray[1];
                    string author = resparray[2];
                    string refno = resparray[3];

                    if (model.Approval.Status == PublicationApprovalStatus.Approved)
                    {
                        if (model.Approval.Level == PublicationApprovalLevels.Verifier)
                        {
                            await LogActivity(Modules.RnP, "Verify Publication Withdrawal: " + title, model);
                            TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Pending Approval 1.";

                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Publication_Withdrawal, title, author, refno, "Withdrawal Verified and Pending Approval", model.Approval.Status, model.Approval.RequireNext);
                        }
                        else
                        {
                            await LogActivity(Modules.RnP, "Approve Publication Withdrawal: " + title, model);

                            if (model.Approval.Level == PublicationApprovalLevels.Approver1)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Pending Approval 2.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_1, title, author, refno, "Withdrawal Approved by 1st-Level Approver and Pending 2nd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Approved.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_1, title, author, refno, "Withdrawal Approved by 1st-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == PublicationApprovalLevels.Approver2)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Pending Approval 3.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_2, title, author, refno, "Withdrawal Approved by 2nd-Level Approver and Pending 3rd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Approved.";
                                    await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_2, title, author, refno, "Withdrawal Approved by 2nd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == PublicationApprovalLevels.Approver3)
                            {
                                TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Approved.";
                                await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_3, title, author, refno, "Withdrawal Approved by 3rd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                            }
                        }
                    }
                    else
                    {
                        await LogActivity(Modules.RnP, "Publication Withdrawal Requires Amendment: " + title, model);
                        TempData["SuccessMessage"] = "Withdrawal of Publication titled " + title + " updated as Requires Amendment.";

                        if (model.Approval.Level == PublicationApprovalLevels.Verifier)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Publication_Withdrawal, title, author, refno, "Withdrawal Amendment Requested by Verifier", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver1)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_1, title, author, refno, "Withdrawal Amendment Requested by 1st-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver2)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_2, title, author, refno, "Withdrawal Amendment Requested by 2nd-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == PublicationApprovalLevels.Approver3)
                        {
                            await SendNotification(pid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Publication_Withdrawal_3, title, author, refno, "Withdrawal Amendment Requested by 3rd-Level Approver", model.Approval.Status, false);
                        }
                    }

                    return RedirectToAction("Index", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to process Publication Withdrawal.";

                    return RedirectToAction("EvaluateWithdrawal", "Publication", new { area = "RnP", @id = model.Approval.PublicationID });
                }
            }

            return View(model);
        }

        // Increments (api calls)

        // Function to increment download count.
        // GET: api/RnP/Publication/IncrementDownload
        [Route("api/RnP/Publication/IncrementDownload")]
        [HttpGet]
        public async Task<string> IncrementDownload(int? id, int? userid)
        {
            if (id == null)
            {
                return "invalid";
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Publication/IncrementDownload?id={id}&userid={userid}");

            if (response.isSuccess)
            {
                return "success";
            }
            else
            {
                return "error";
            }
        }

        // Visitor functions (registered)
        // currently all unused?

        // Browse publications
        // TODO: Handle search/filtering, include star rating
        // GET: RnP/Publication/BrowsePublications
        [AllowAnonymous]
        public async Task<ActionResult> BrowsePublications()
        {
            var resPubs = await WepApiMethod.SendApiAsync<IEnumerable<ReturnPublicationModel>>(HttpVerbs.Get, $"RnP/Publication");

            if (!resPubs.isSuccess)
            {
                return HttpNotFound();
            }

            var publications = resPubs.Data;

            if (publications == null)
            {
                return HttpNotFound();
            }

            return View(publications);
        }

        // Publication details
        // TODO: include ratings and reviews info
        // GET: RnP/Publication/PublicationDetails
        [AllowAnonymous]
        public async Task<ActionResult> PublicationDetails(int? id)
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

        // Select format to purchase
        // GET: RnP/Publication/SelectFormat
        public async Task<ActionResult> SelectFormat(int? id)
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

        // Purchase publication
        // GET: RnP/Publication/PurchasePublication
        [AllowAnonymous]
        public async Task<ActionResult> PurchasePublication(int? id)
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

        // Refunds

        // GET: RnP/Publication/RefundRequest
        [HttpGet]
        [HasAccess(UserAccess.Refunds)]
        public async Task<ActionResult> RefundRequest()
        {
            var resBank = await WepApiMethod.SendApiAsync<List<BankInformationModel>>(HttpVerbs.Get, $"Commerce/Cart/GetBanks");

            if (!resBank.isSuccess)
            {
                return HttpNotFound();
            }

            ViewBag.Banks = resBank.Data;

            return View();
        }

        // GET: RnP/Publication/RefundRequest
        [HttpPost]
        [HasAccess(UserAccess.Refunds)]
        public async Task<ActionResult> RefundRequest(FilterRefundRequestModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"Commerce/Cart/ListRefund", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        // Update refund status to approved, then refresh list
        // POST: RnP/Publication/ApproveRefund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> ApproveRefund(UpdateRefundStatusModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Commerce/Cart/ApproveRefund", model);

                if (!response.isSuccess)
                {
                    return "error";
                }

                if (response.Data == true)
                {
                    return "success";
                }
                else
                {
                    return "error";
                }
            }

            return "error";
        }

        // Download

        // File download
        [HttpGet]
        public async Task<ActionResult> Download(int id)
        {
            return await FileMethod.DownloadFile(id);
        }

        // Settings

        // Settings
        // GET: RnP/Publication/Settings
        [HasAccess(UserAccess.RnPPublicationEdit)]
        public async Task<ActionResult> Settings()
        {
            var resSettings = await WepApiMethod.SendApiAsync<PublicationSettingsModel>(HttpVerbs.Get, $"RnP/Publication/LoadSettings");

            if (!resSettings.isSuccess)
            {
                return HttpNotFound();
            }

            var settings = resSettings.Data;

            if (settings == null)
            {
                return HttpNotFound();
            }

            return View(settings);
        }

        // Settings
        // GET: RnP/Publication/Settings
        [HasAccess(UserAccess.RnPPublicationEdit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Settings(PublicationSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/SaveSettings", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.RnP, "Save Publication Settings");

                    TempData["SuccessMessage"] = "Publication Settings saved successfully.";

                    return RedirectToAction("Settings", "Publication", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to save Publication Settings.";

                    return RedirectToAction("Settings", "Publication", new { area = "RnP" });
                }
            }

            return View();
        }

        // Private functions

        // Actual upload of cover file
        private string UploadCoverFile(HttpPostedFileBase coverfile)
        {
            //string UploadPath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

            if (coverfile != null)
            {
                string UploadPath = HttpContext.Server.MapPath("~/Data/images/publication");

                string FileName = System.IO.Path.GetFileNameWithoutExtension(coverfile.FileName);

                string FileExtension = System.IO.Path.GetExtension(coverfile.FileName);

                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.Trim() + FileExtension;

                //string ServerPath = UploadPath + FileName;

                string ServerPath = System.IO.Path.Combine(UploadPath, FileName);

                coverfile.SaveAs(ServerPath);

                return ServerPath;
            }
            return "";
        }

        // Actual upload of author file
        private string UploadAuthorFile(HttpPostedFileBase authorfile)
        {
            //string UploadPath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

            if (authorfile != null)
            {
                string UploadPath = HttpContext.Server.MapPath("~/Data/images/publication");

                string FileName = System.IO.Path.GetFileNameWithoutExtension(authorfile.FileName);

                string FileExtension = System.IO.Path.GetExtension(authorfile.FileName);

                FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.Trim() + FileExtension;

                //string ServerPath = UploadPath + FileName;

                string ServerPath = System.IO.Path.Combine(UploadPath, FileName);

                authorfile.SaveAs(ServerPath);

                return ServerPath;
            }
            return "";
        }

        // Upload picture files
        private async Task<int> UploadImageFiles(int pubid, IEnumerable<HttpPostedFileBase> coverfiles, IEnumerable<HttpPostedFileBase> authorfiles)
        {
            string coverpath = "";
            string authorpath = "";

            if (coverfiles.Count() > 0)
            {
                HttpPostedFileBase coverfile = coverfiles.First();
                coverpath = UploadCoverFile(coverfile);
            }

            if (authorfiles.Count() > 0)
            {
                HttpPostedFileBase authorfile = authorfiles.First();
                authorpath = UploadAuthorFile(authorfile);
            }

            var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/UploadImages?pubid={pubid}&coverpic={coverpath}&authorpic={authorpath}");

            if (response.isSuccess)
            {
                var newid = response.Data;
                return newid;
            }

            return 0;
        }

        // Update cover file
        private async Task<int> UpdateImageFileCover(int pubid, IEnumerable<HttpPostedFileBase> coverfiles, IEnumerable<WebApiModel.FileDocuments.Attachment> covers)
        {
            if (covers.Count() <= 0)
            {
                string coverpath = "";

                if (coverfiles.Count() > 0)
                {
                    HttpPostedFileBase coverfile = coverfiles.First();
                    coverpath = UploadCoverFile(coverfile);
                }

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/UpdateImagesCover?pubid={pubid}&coverpic={coverpath}");

                if (response.isSuccess)
                {
                    var oldid = response.Data;
                    return oldid;
                }

            }

            return 0;
        }

        // Update author file
        private async Task<int> UpdateImageFileAuthor(int pubid, IEnumerable<HttpPostedFileBase> authorfiles, IEnumerable<WebApiModel.FileDocuments.Attachment> authors)
        {
            if (authors.Count() <= 0)
            {
                string authorpath = "";

                if (authorfiles.Count() > 0)
                {
                    HttpPostedFileBase authorfile = authorfiles.First();
                    authorpath = UploadAuthorFile(authorfile);
                }

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/UpdateImagesAuthor?pubid={pubid}&authorpic={authorpath}");

                if (response.isSuccess)
                {
                    var oldid = response.Data;
                    return oldid;
                }

            }

            return 0;
        }

        // Get minimum publication year from API
        private async Task<int> GetMinimumPublishedYear()
        {
            var resYear = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/GetSettingsMinimumPublishedYear");

            if (resYear.isSuccess)
            {
                return resYear.Data;
            }
            return 1900;
        }

        // Get hardcopy return period from API
        private async Task<int> GetHardcopyReturnPeriod()
        {
            var resDays = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"RnP/Publication/GetSettingsHardcopyReturnPeriod");

            if (resDays.isSuccess)
            {
                return resDays.Data;
            }
            return 30;
        }

        // get notification receiver IDs
        // called by SendNotification
        private async Task<List<int>> GetNotificationReceivers(NotificationCategory ncat, NotificationType ntype, PublicationApprovalStatus status, bool forward)
        {
            List<int> result = new List<int> { };
            var response = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"RnP/Publication/GetNotificationReceivers/?cat={ncat}&type={ntype}&status={status}&forward={forward}");
            if (response.isSuccess)
            {
                result = response.Data;
            }
            else
            {
                await LogError(Modules.RnP, "Failed to get Auto Notification receivers");
            }
            return result;
        }

        // save notification ID
        // called by SendNotification
        private async Task<bool> SaveNotificationID(int id, int notification_id)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Publication/SaveNotificationID?id={id}&notificationid={notification_id}");
            if (!response.isSuccess)
            {
                await LogError(Modules.RnP, "Failed to save Auto Notification ID (API Error)");
            }
            else
            {
                if (response.Data == false)
                {
                    await LogError(Modules.RnP, "Failed to save Auto Notification ID (Publication Error)");
                }
            }
            return response.isSuccess;
        }

        // Send notifications
        private async Task<bool> SendNotification(int id, NotificationCategory ncat, NotificationType ntype, string title, string author, string code, string approvalmessage, PublicationApprovalStatus appstatus, bool forward)
        {
            try
            {
                var receivers = await GetNotificationReceivers(ncat, ntype, appstatus, forward);
                if (receivers.Count > 0)
                {
                    ParameterListToSend paramToSend = new ParameterListToSend();
                    paramToSend.PublicationTitle = title;
                    paramToSend.PublicationAuthor = author;
                    paramToSend.PublicationCode = code;
                    paramToSend.PublicationApproval = approvalmessage;

                    CreateAutoReminder reminder = new CreateAutoReminder
                    {
                        NotificationType = ntype,
                        NotificationCategory = ncat,
                        ParameterListToSend = paramToSend,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = receivers
                        // new List<int> { 2, 3, 4, 5 }
                    };
                    try
                    {
                        await LogActivity(Modules.RnP, "Sending email notification for Publication " + code + " (" + approvalmessage + ") to users " + string.Join(",", receivers));

                        var response = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
                        if (response.isSuccess)
                        {
                            int saveThisID = response.Data.SLAReminderStatusId;
                            //save saveThisID back into survey table
                            var ressave = await SaveNotificationID(id, saveThisID);

                            await LogActivity(Modules.RnP, "Email notification sent for Publication " + code + " - " + approvalmessage);

                            return true;
                        }
                        else
                        {
                            await LogError(Modules.RnP, "Failed to generate Auto Notification (API Call Returned Failure)");
                            return false;
                        }
                    }
                    catch
                    {
                        await LogError(Modules.RnP, "Failed to generate Auto Notification (API Call Failed)");
                        return false;
                    }
                }
                else
                {
                    await LogError(Modules.RnP, "Failed to generate Auto Notification (No Receivers Found)");
                    return false;
                }
            }
            catch
            {
                await LogError(Modules.RnP, "Failed to generate Auto Notification");
                return false;
            }
        }

        /*
         * Statics 
         */

        public static List<SelectListItem> GetDropDownListForYears()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            for (int i = 1960; i <= 2200; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
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

