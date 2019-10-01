using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class MinistryController : FEPController
    {
        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<MinistryModel>>(HttpVerbs.Get, $"Administration/Ministry");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<MinistryModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateMinistryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Ministry/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Ministry.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Administration/Ministry", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Sector.AlertSuccessCreate;

                    await LogActivity(Modules.Setting, "Create Parameter Ministry", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Ministry.AlertFailCreate;

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name)
        {

            var model = new EditMinistryModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditMinistryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Ministry/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Ministry.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Ministry?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Ministry.AlertSuccessUpdate;

                    await LogActivity(Modules.Setting, "Update Parameter Ministry", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Ministry.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name)
        {

            var model = new DeleteMinistryModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Delete(int id)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Administration/Ministry?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.Ministry.AlertSuccessDelete;

                await LogActivity(Modules.Setting, "Delete Parameter Ministry", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.Ministry.AlertFailDelete;

            return RedirectToAction("List");

        }
    }
}