using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.KMC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Controllers
{
    [LogError(Modules.KMC)]
    public class ManageController : FEPController
    {
        public const string filter_imgs = ".png,.gif,.ico,.jpg,.jpeg,.png,.svg,.tiff,.webp";
        public const string filter_videos = ".mp4,.webm,.ogg";
        public const string filter_audios = ".mp3,.ogg,.wav";
        public const string filter_docs = ".doc,.docx,.xls,.xlsx,.ppt,.pptx,.pdf";

        // GET: KMC/Manage
        public async Task<ActionResult> Index()
        {
            var model = new List<CategoryModel>();

            var response = await WepApiMethod.SendApiAsync<List<CategoryModel>>(HttpVerbs.Get, $"KMC/Category");

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return View(model);
        }

        public async Task<ActionResult> List(int? Id)
        {
            if (Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }

            var model = new ListKMCModel();

            var response = await WepApiMethod.SendApiAsync<CategoryModel>(HttpVerbs.Get, $"KMC/Category?id={Id}");

            if (response.isSuccess)
            {
                model.Category = response.Data;
            }

            ViewBag.Categories = new List<CategoryModel>();

            var responseCategory = await WepApiMethod.SendApiAsync<List<CategoryModel>>(HttpVerbs.Get, $"KMC/Category");

            if (responseCategory.isSuccess)
            {
                ViewBag.Categories = responseCategory.Data;
            }

            var responseKMC = await WepApiMethod.SendApiAsync<List<KMCModel>>(HttpVerbs.Get, $"KMC/Manage/GetAll?categoryId={Id}");

            if (responseKMC.isSuccess)
            {
                model.List = responseKMC.Data;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> _List(int? Id, FilterKMCModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<List<KMCModel>>(HttpVerbs.Post, $"KMC/Manage/GetAll?categoryId={Id}", filter);

            var model = new List<KMCModel>();

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return PartialView(model);

        }

        [HttpGet]
        public async Task<ActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }

            var model = new DetailsKMCModel();

            var response = await WepApiMethod.SendApiAsync<DetailsKMCModel>(HttpVerbs.Get, $"KMC/Manage?id={Id}");

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> _Content(int Id)
        {
            var response = await WepApiMethod.SendApiAsync<DetailsKMCModel>(HttpVerbs.Get, $"KMC/Manage?id={Id}");

            var model = new DetailsKMCModel();

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> Create(int? Id)
        {
            if (Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }

            var model = new Models.CreateKMCModel();

            var response = await WepApiMethod.SendApiAsync<CategoryModel>(HttpVerbs.Get, $"KMC/Category?id={Id}");

            if (response.isSuccess)
            {
                model.Category = response.Data.Title;
                model.CategoryId = response.Data.Id;
            }

            model.filter_imgs = filter_imgs;
            model.filter_videos = filter_videos;
            model.filter_audios = filter_audios;
            model.filter_docs = filter_docs;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.CreateKMCModel model)
        {

            if (model.IsEditor)
            {
                ModelState.Remove("File");
                ModelState.Remove("Type");
            }
            else
            {
                ModelState.Remove("EditorCode");

                //validation of file type
                if (model.File != null)
                {
                    var isValid = true;

                    switch (model.Type)
                    {
                        case KMCType.Image:
                            isValid = FileMethod.IsValidType(model.File, filter_imgs);
                            break;

                        case KMCType.Video:
                            isValid = FileMethod.IsValidType(model.File, filter_videos);
                            break;

                        case KMCType.Audio:
                            isValid = FileMethod.IsValidType(model.File, filter_audios);
                            break;

                        case KMCType.Document:
                            isValid = FileMethod.IsValidType(model.File, filter_docs);
                            break;

                        case KMCType.Others:

                            break;

                        default:
                            break;

                    }

                    if (!isValid)
                    {
                        ModelState.AddModelError("File", Language.KMC.ValidIsValidTypeFile);
                    }

                }
                else
                {
                    ModelState.AddModelError("FileName", Language.KMC.ValidRequiredFile);
                }

            }

            if (ModelState.IsValid)
            {

                var modelapi = new CreateKMCModel
                {
                    KMCCategoryId = model.CategoryId,
                    Title = model.Title,
                    Description = model.Description,
                    Type = model.Type,
                    IsPublic = model.IsPublic,
                    IsShow = model.IsShow,
                    IsEditor = model.IsEditor,
                    EditorCode = model.EditorCode,
                    CreatedBy = CurrentUser.UserId.Value
                };

                if (model.ThumbnailFile != null)
                {
                    var filename = FileMethod.SaveFile(model.ThumbnailFile, Server.MapPath("~/img/kmc-thumbnail"));
                    modelapi.ThumbnailUrl = filename;
                }

                if (model.File != null)
                {
                    var responseFile = await FileMethod.UploadFile(new List<HttpPostedFileBase> { model.File }, CurrentUser.UserId, "KMC/", model.File.ContentType);

                    if (responseFile != null)
                    {
                        modelapi.FileId = responseFile.Select(f => f.Id).FirstOrDefault();
                        modelapi.FileType = model.File.ContentType;
                    }
                }

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"KMC/Manage", modelapi);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.KMC, "Create KMC", model);

                    TempData["SuccessMessage"] = Language.KMC.AlertSuccessCreate;

                    return RedirectToAction("List", "Manage", new { area = "KMC", @id = model.CategoryId });
                }
                else
                {
                    TempData["ErrorMessage"] = Language.KMC.AlertFailCreate;
                }

            }

            model.filter_imgs = filter_imgs;
            model.filter_videos = filter_videos;
            model.filter_audios = filter_audios;
            model.filter_docs = filter_docs;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsKMCModel>(HttpVerbs.Get, $"KMC/Manage?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var data = response.Data;

            var model = new Models.EditKMCModel
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                ThumbnailUrl = data.ThumbnailUrl,
                IsEditor = data.IsEditor,
                Type = data.Type,
                FileId = data.FileId,
                FileName = data.FileName,
                CategoryId = data.Category.Id,
                Category = data.Category.Title,
                EditorCode = data.EditorCode,
                IsPublic = data.IsPublic,
                IsShow = data.IsShow
            };

            model.filter_imgs = filter_imgs;
            model.filter_videos = filter_videos;
            model.filter_audios = filter_audios;
            model.filter_docs = filter_docs;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.EditKMCModel model)
        {

            if (model.IsEditor)
            {
                ModelState.Remove("File");
                ModelState.Remove("Type");
            }
            else
            {
                ModelState.Remove("EditorCode");

                //validation of file type
                if (model.File != null)
                {
                    var isValid = true;

                    switch (model.Type)
                    {
                        case KMCType.Image:
                            isValid = FileMethod.IsValidType(model.File, filter_imgs);
                            break;

                        case KMCType.Video:
                            isValid = FileMethod.IsValidType(model.File, filter_videos);
                            break;

                        case KMCType.Audio:
                            isValid = FileMethod.IsValidType(model.File, filter_audios);
                            break;

                        case KMCType.Document:
                            isValid = FileMethod.IsValidType(model.File, filter_docs);
                            break;

                        case KMCType.Others:

                            break;

                        default:
                            break;

                    }

                    if (!isValid)
                    {
                        ModelState.AddModelError("File", Language.KMC.ValidIsValidTypeFile);
                    }

                }
                else
                {
                    if (model.FileId != null)
                    {
                        ModelState.Remove("File");
                    }
                    
                }

            }

            if (ModelState.IsValid)
            {

                var modelapi = new EditKMCModel
                {
                    KMCCategoryId = model.CategoryId,
                    Title = model.Title,
                    Description = model.Description,
                    Type = model.Type,
                    IsPublic = model.IsPublic,
                    IsShow = model.IsShow,
                    IsEditor = model.IsEditor,
                    EditorCode = model.EditorCode,
                    FileId = model.IsEditor ? null : model.FileId,
                    ThumbnailUrl = model.ThumbnailUrl
                };

                if (model.ThumbnailFile != null)
                {
                    var filename = FileMethod.SaveFile(model.ThumbnailFile, Server.MapPath("~/img/kmc-thumbnail"), model.ThumbnailUrl);
                    modelapi.ThumbnailUrl = filename;
                }

                if (model.File != null)
                {
                    var responseFile = await FileMethod.UploadFile(new List<HttpPostedFileBase> { model.File }, CurrentUser.UserId, "KMC/", model.File.ContentType);

                    if (responseFile != null)
                    {
                        modelapi.FileId = responseFile.Select(f => f.Id).FirstOrDefault();
                        modelapi.FileType = model.File.ContentType;
                    }
                }

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"KMC/Manage?id={model.Id}", modelapi);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.KMC, "Edit KMC", model);

                    TempData["SuccessMessage"] = Language.KMC.AlertSuccessUpdate;

                    return RedirectToAction("Details", "Manage", new { area = "KMC", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = Language.KMC.AlertFailDelete;
                }

            }

            model.filter_imgs = filter_imgs;
            model.filter_videos = filter_videos;
            model.filter_audios = filter_audios;
            model.filter_docs = filter_docs;

            return View(model);

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }

            var model = new DetailsKMCModel();

            var response = await WepApiMethod.SendApiAsync<DetailsKMCModel>(HttpVerbs.Get, $"KMC/Manage?id={Id}");

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int id, CategoryModel Category)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"KMC/Manage?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.KMC, "Delete KMC");

                TempData["SuccessMessage"] = Language.KMC.AlertSuccessDelete;

                return RedirectToAction("List", "Manage", new { area = "KMC", @id = Category.Id});
            }

            TempData["ErrorMessage"] = Language.KMC.AlertFailDelete;

            return RedirectToAction("Delete", new { id = id });
        }

        [HttpPost]
        public ActionResult LoadThumbnail()
        {
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];

                var image64 = FileMethod.ConvertImageToBase64(fileContent);

                if (image64 != null)
                {
                    return Content(JsonConvert.SerializeObject(new { image64 = image64 }), "application/json");
                }
            }

            return Content(JsonConvert.SerializeObject(new { image64 = "" }), "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> GetContent(int Id)
        {
            return await FileMethod.DownloadFile(Id);
        }
    }
}