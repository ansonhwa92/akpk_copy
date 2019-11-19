using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.KMC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Controllers
{
    [LogError(Modules.KMC)]
    public class ManageController : FEPController
    {
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

                if (model.File != null)
                {
                    var extension = Path.GetExtension(model.File.FileName);
                    
                    
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

                var filename = FileMethod.SaveFile(model.ThumbnailFile, Server.MapPath("~/img/kmc-thumbnail"));

                modelapi.ThumbnailUrl = filename;

                var responseFile = await FileMethod.UploadFile(new List<HttpPostedFileBase> { model.File }, CurrentUser.UserId);

                if (responseFile != null)
                {
                    modelapi.FileId = responseFile.Select(f => f.Id).FirstOrDefault();
                }
                
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"KMC/Manage", modelapi);

                if (response.isSuccess)
                {                    
                    await LogActivity(Modules.KMC, "Create KMC", model);

                    TempData["SuccessMessage"] = Language.KMC.AlertSuccessCreate;

                    return RedirectToAction("List", "Manage", new { area = "KMC", @id = model.CategoryId });
                }

            }

            return View(model);
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
    }
}