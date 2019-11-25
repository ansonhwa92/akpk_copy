using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Controllers
{
    public class CategoryController : FEPController
    {
        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<CategoryModel>>(HttpVerbs.Get, $"KMC/Category");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<CategoryModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"KMC/Category/IsNameExist?id={null}&title={model.Title}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.KMCCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"KMC/Category", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.KMCCategory.AlertSuccessCreate;

                    await LogActivity(Modules.KMC, "Create KMC Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Ministry.AlertFailCreate;

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Title)
        {

            var model = new EditCategoryModel
            {
                Id = id,
                No = No,
                Title = Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"KMC/Category/IsNameExist?id={model.Id}&title={model.Title}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.KMCCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"KMC/Category?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.KMCCategory.AlertSuccessUpdate;

                    await LogActivity(Modules.KMC, "Update KMC Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.KMCCategory.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Title)
        {

            var model = new DeleteCategoryModel
            {
                Id = id,
                No = No,
                Title = Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Delete(int id)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"KMC/Category?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.KMCCategory.AlertSuccessDelete;

                await LogActivity(Modules.KMC, "Delete KMC Category", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.KMCCategory.AlertFailDelete;

            return RedirectToAction("List");

        }
    }
}