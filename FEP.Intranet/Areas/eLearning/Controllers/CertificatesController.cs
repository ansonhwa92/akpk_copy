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
            var response = await WepApiMethod.SendApiAsync<List<CertificatesModel>>(HttpVerbs.Get, $"eLearning/Certificate");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<CertificatesModel>());
        }

        // GET: eLearning/CourseContents/Create
        public ActionResult Create_Background()
        {
            CreateBackgroundModel model = new CreateBackgroundModel();
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

            TempData["ErrorMessage"] = "Cannot add content.";

            return View(model);
        }

        public async Task<ActionResult> Edit_Background(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = await WepApiMethod.SendApiAsync<CertificatesModel>(HttpVerbs.Get, $"eLearning/Certificate?id={id}");

            if (!response.isSuccess)
                return HttpNotFound();

            var vm = response.Data;

            if (vm == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        [HttpPost, ActionName("Delete_Background")]
        [ValidateAntiForgeryToken]
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

    }
}