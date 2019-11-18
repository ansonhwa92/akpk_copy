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
    public class BranchController : FEPController
    {
        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<BranchModel>>(HttpVerbs.Get, $"Administration/Branch");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<BranchModel>());
        }

        public async Task<ActionResult> _Create()
        {
            var model = new CreateBranchModel();

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateBranchModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Branch/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Branch.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Administration/Branch", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Branch.AlertSuccessCreate;

                    await LogActivity(Modules.Setting, "Create Parameter Branch", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Branch.AlertFailCreate;

            return RedirectToAction("List");

        }

        public async Task<ActionResult> _Edit(int id, string No, int StateId, string Name)
        {

            var model = new EditBranchModel
            {
                Id = id,
                No = No,
                StateId = StateId,
                Name = Name
            };

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditBranchModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Branch/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.Branch.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Branch?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Branch.AlertSuccessUpdate;

                    await LogActivity(Modules.Setting, "Update Parameter Branch", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.Branch.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string State, string Name)
        {

            var model = new DeleteBranchModel
            {
                Id = id,
                No = No,
                Name = Name,
                State = State
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Delete(int id)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Administration/Branch?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.Branch.AlertSuccessDelete;

                await LogActivity(Modules.Setting, "Delete Parameter Branch", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.Branch.AlertFailDelete;

            return RedirectToAction("List");

        }

        [NonAction]
        private async Task<IEnumerable<StateModel>> GetStates()
        {
            var states = Enumerable.Empty<StateModel>();

            var response = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (response.isSuccess)
            {
                states = response.Data.OrderBy(o => o.Name);
            }

            return states;

        }
    }
}