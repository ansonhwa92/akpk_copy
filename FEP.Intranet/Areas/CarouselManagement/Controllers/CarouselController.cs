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
using FEP.WebApiModel.Carousel;
using Newtonsoft.Json;

namespace FEP.Intranet.Areas.CarouselManagement.Controllers
{
    public class CarouselController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: CarouselManagement/Carousel
        [HasAccess(UserAccess.CarouselManage)]
        public ActionResult Index()
        {
            return View();
        }

        [HasAccess(UserAccess.CarouselView)]
        // GET: CarouselManagement/Carousel/SequenceList
        [Route("CarouselManagement/Carousel/SequenceList")]
        [HttpGet]
        public ActionResult SequenceList()
        {
            return View();
        }

        [HasAccess(UserAccess.CarouselMenu)]
        //menu
        [ChildActionOnly]
        public ActionResult _Menu()
        {
            return PartialView();
        }

        [HasAccess(UserAccess.CarouselView)]
        [HttpPost]
        public async Task<ActionResult> List(FilterCarouselModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"Carousels/Carousel/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        [HasAccess(UserAccess.CarouselView)]
        // GET: CarouselManagement/Carousel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var resPub = await WepApiMethod.SendApiAsync<CreateCarouselModel>(HttpVerbs.Get, $"Carousels/Carousel?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            return View(resPub.Data);
        }

        [HasAccess(UserAccess.CarouselAdd)]
        // GET: CarouselManagement/Carousel/Create
        public ActionResult Create()
        {
            var model = new CreateCarouselModel();
            return View(model);
        }

        // POST: CarouselManagement/Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HasAccess(UserAccess.CarouselAdd)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCarouselModel model, string Submittype)
        {
            if (ModelState.IsValid)
            {
                var apimodel = new CreateCarouselModelNoFile
                {
                    Title = model.Title,
                    Description = model.Description,
                    Display = model.Display,
                    DisplayDate = model.DisplayDate,
                    TextLocation = model.TextLocation,
                    FreeTextArea = model.FreeTextArea,
                    CoverPictures = model.CoverPictures,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = model.CreatedDate
                };

                if (model.CoverPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.CoverPictureFiles.ToList(), CurrentUser.UserId, "carousel");
                    if (files != null)
                    {
                        apimodel.CoverFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Carousels/Carousel/Create", apimodel);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string newid = resparray[0];
                    string title = resparray[1];

                    if ((model.CoverPictureFiles.Count() > 0))
                    {
                        await UploadImageFiles(int.Parse(newid), model.CoverPictureFiles.First());
                    }

                    await LogActivity(Modules.CarouselManagement, "Create New Carousel: " + title);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "New Carousel titled " + title + " created successfully and saved as draft.";

                        return RedirectToAction("Index", "Carousel", new { area = "CarouselManagement" });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Carousel", new { area = "CarouselManagement", @id = newid });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Carousel.";

                    return RedirectToAction("Index", "Carousel", new { area = "CarouselManagement" });
                }
            }
            
            return View(model);
        }

        [HasAccess(UserAccess.CarouselEdit)]
        // GET: CarouselManagement/Carousel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var resPub = await WepApiMethod.SendApiAsync<CreateCarouselModel>(HttpVerbs.Get, $"Carousels/Carousel?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            return View(resPub.Data);
        }

        // POST: CarouselManagement/Carousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HasAccess(UserAccess.CarouselEdit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateCarouselModel model, string Submittype)
        {
            if (model.CoverPictures.Count() == 0 && model.CoverPictureFiles.Count() == 0)
            {
                ModelState.AddModelError("CoverPictures", "Please upload at least one image");
            }

            if (ModelState.IsValid)
            {
                var apimodel = new CreateCarouselModel
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Display = model.Display,
                    DisplayDate = model.DisplayDate,
                    TextLocation = model.TextLocation,
                    FreeTextArea = model.FreeTextArea,
                    LastModifiedBy = CurrentUser.UserId,
                    CoverPictures = model.CoverPictures
                };

                //attachment 1: cover pics
                if (model.CoverPictureFiles.Count() > 0)
                {
                    var files = await FileMethod.UploadFile(model.CoverPictureFiles.ToList(), CurrentUser.UserId, "carousel");
                    if (files != null)
                    {
                        apimodel.CoverFilesId = files.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Carousels/Carousel/Edit", apimodel);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.CarouselManagement, "Edit Carousel: " + response.Data, model);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Carousel titled " + response.Data + " updated successfully and saved as draft.";

                        return RedirectToAction("Index", "Carousel", new { area = "CarouselManagement" });
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Carousel titled " + response.Data + " updated successfully.";
                        return RedirectToAction("Details", "Carousel", new { area = "CarouselManagement", @id = model.Id });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to edit Publication.";

                    return RedirectToAction("Edit", "Carousel", new { area = "CarouselManagement", @id = model.Id });
                }
            }

            return View(model);
        }

        [HasAccess(UserAccess.CarouselEdit)]
        [Route("CarouselManagement/Carousel/UpdateSequence")]
        [HttpPost]
        public async Task<bool> UpdateSequence(CarouselModel model)
        {
            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Carousels/Carousel/UpdateSequence", model);

            if (response.isSuccess)
            {
                await LogActivity(Modules.CarouselManagement, "Update Carousel Sequence: " + response.Data, model);

                return true;
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit Publication.";

                return false;
            }
        }

        // Show delete form (only from list page)
        // GET: CarouselManagement/Carousel/Delete/5
        [HasAccess(UserAccess.CarouselDelete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resCrs = await WepApiMethod.SendApiAsync<CreateCarouselModel>(HttpVerbs.Get, $"Carousels/Carousel?id={id}");

            if (!resCrs.isSuccess)
            {
                return HttpNotFound();
            }

            var carousel = resCrs.Data;

            if (carousel == null)
            {
                return HttpNotFound();
            }

            return View(carousel);
        }

        [HasAccess(UserAccess.CarouselDelete)]
        // POST: CarouselManagement/Carousel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"Carousels/Carousel/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.CarouselManagement, "Delete Carousel: " + response.Data);

                TempData["SuccessMessage"] = "Carousel titled " + response.Data + " successfully deleted.";

                return RedirectToAction("Index", "Carousel", new { area = "CarouselManagement" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Publication.";

                return RedirectToAction("Details", "Carousel", new { area = "CarouselManagement", @id = id });
            }
        }

        // Upload picture files
        private async Task<int> UploadImageFiles(int pubid, HttpPostedFileBase file)
        {
            string imagePath = UploadFile(file);

            var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"Carousels/Carousel/UploadImages?pubid={pubid}&coverpic={imagePath}");

            if (response.isSuccess)
            {
                var newid = response.Data;
                return newid;
            }

            return 0;
        }

        // Update picture files
        private async Task<int> UpdateImageFiles(int pubid, HttpPostedFileBase file)
        {
            string imagePath = UploadFile(file);

            var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Get, $"Carousels/Carousel/UploadFiles?pubid={pubid}&image={imagePath}");

            if (response.isSuccess)
            {
                var oldid = response.Data;
                return oldid;
            }

            return 0;
        }

        private string UploadFile(HttpPostedFileBase coverfile)
        {
            if (coverfile != null)
            {
                try
                {
                    string UploadPath = HttpContext.Server.MapPath("~/Data/images/carousel");

                    string FileName = System.IO.Path.GetFileNameWithoutExtension(coverfile.FileName);

                    string FileExtension = System.IO.Path.GetExtension(coverfile.FileName);

                    FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.Trim() + FileExtension;

                    string ServerPath = System.IO.Path.Combine(UploadPath, FileName);

                    coverfile.SaveAs(ServerPath);

                    return ServerPath;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
            return "";
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
