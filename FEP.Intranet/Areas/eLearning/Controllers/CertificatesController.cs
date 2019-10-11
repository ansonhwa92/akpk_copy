using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CertificatesController : FEPController
	{
		private DbEntities db = new DbEntities();

		public async Task<ActionResult> Index()
		{
            var response = await WepApiMethod.SendApiAsync<CertificatesModel>(HttpVerbs.Get, $"eLearning/Certificate");

            if (response.isSuccess)
                return View(response.Data);

            return View(new CertificatesModel());
        }

        // CREATE CERTIFICATE BACKGROUND
        public ActionResult Create_Background()
        {
            CreateBackgroundModel model = new CreateBackgroundModel();
            return View(model);
        }

        // CREATE CERTIFICATE TEMPLATE
        public ActionResult Create_Template()
        {
            CreateTemplateModel model = new CreateTemplateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Background(CreateBackgroundModel model)
        {
            if (ModelState.IsValid)
            {
                var currentFileName = model.File;
                model.FileName = model.File.FileName;

                // Check if this creation include fileupload, which will require us to save the file
                model.File = currentFileName;
                if (model.File != null)
                {
                    // upload the file

                    var result = await new FileUploadController().FileUploadToApi<List<FileUploadModel>>(model.File);

                    if (result.isSuccess)
                    {
                        var data = result.Data;

                        var fileDocument = data[0];

                        fileDocument.CreatedBy = CurrentUser.UserId.Value;

                        var resultUpload = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, FileUploadApiUrl.FileUploadInfo, fileDocument);

                        if (resultUpload.isSuccess)
                        {
                            model.File = null;
                            model.FileUploadId = int.Parse(resultUpload.Data);

                            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Certificate/Create_Background", model);

                            if (response.isSuccess)
                            {
                                TempData["SuccessMessage"] = "Image background successfully added.";

                                await LogActivity(Modules.Learning, "Create certificate background : " + model.Description);
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Cannot upload file";
                        }
                    }
                }

                    return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
            }

            TempData["ErrorMessage"] = "Cannot add background.";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Template(CreateTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Certificate/Create_Template", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Template successfully added.";

                    await LogActivity(Modules.Learning, "Create certificate template : " + model.Description);
                }
                else
                {
                    TempData["ErrorMessage"] = "Fail add new template";
                }
                    
                return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
            }

            TempData["ErrorMessage"] = "Cannot add template.";

            return View(model);
        }

        public async Task<ActionResult> Edit_Background(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = await WepApiMethod.SendApiAsync<CreateBackgroundModel>(HttpVerbs.Get, $"eLearning/Certificate/GetBackground?id={id}");

            if (!response.isSuccess)
                return HttpNotFound();

            var vm = response.Data;

            if (vm == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Update_Background(CreateBackgroundModel model)
        {
            if (model.Description != null)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Certificates/Update_Background", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Delete Background: " + response.Data);

                    TempData["SuccessMessage"] = "Background Name " + response.Data + " successfully updated.";

                    return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to delete certificate background.";

                    return RedirectToAction("Edit_Background", "Certificates", new { area = "eLearning", @id = model.Id });
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit_Template(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = await WepApiMethod.SendApiAsync<CreateTemplateModel>(HttpVerbs.Get, $"eLearning/Certificate/GetTemplate?id={id}");

            if (!response.isSuccess)
                return HttpNotFound();

            var vm = response.Data;

            if (vm == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update_Template(CreateTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/Certificates/Update_Template", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Delete Template: " + response.Data);

                    TempData["SuccessMessage"] = "Template Name " + response.Data + " successfully updated.";

                    return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to delete certificate background.";

                    return RedirectToAction("Edit_Template", "Certificates", new { area = "eLearning", @id = model.Id });
                }
            }
            return View(model);
        }

        //[HttpPost, ActionName("Delete_Background")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_Background(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var module = db.CourseCertificates.Where(p => p.Id == id).FirstOrDefault();

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"eLearning/Certificates/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Background: " + response.Data);

                TempData["SuccessMessage"] = "Background Name " + response.Data + " successfully deleted.";

                return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete certificate background.";

                return RedirectToAction("Edit_Background", "Certificates", new { area = "eLearning", @id = id });
            }
        }


        //[HttpPost, ActionName("Delete_Template")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_Template(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var module = db.CourseCertificateTemplates.Where(p => p.Id == id).FirstOrDefault();

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"eLearning/Certificates/Delete_Template?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Template: " + response.Data);

                TempData["SuccessMessage"] = "Template Name " + response.Data + " successfully deleted.";

                return RedirectToAction("Index", "Certificates", new { area = "eLearning" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete certificate template.";

                return RedirectToAction("Edit_Template", "Certificates", new { area = "eLearning", @id = id });
            }
        }
    }
}