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
    public class SectorController : FEPController
    {

        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<SectorModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateSectorModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Sector/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Sector.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Administration/Sector", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Sector.AlertSuccessCreate;

                    LogActivity(Modules.Setting, "Create Parameter Sector", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Sector.AlertFailCreate;

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name)
        {

            var model = new EditSectorModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditSectorModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Sector/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Sector.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Sector?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Sector.AlertSuccessUpdate;

                    LogActivity(Modules.Setting, "Update Parameter Sector", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Sector.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name)
        {

            var model = new DeleteSectorModel
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

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Administration/Sector?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.Sector.AlertSuccessDelete;

                LogActivity(Modules.Setting, "Delete Parameter Sector", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.Sector.AlertFailDelete;

            return RedirectToAction("List");

        }
    }
}