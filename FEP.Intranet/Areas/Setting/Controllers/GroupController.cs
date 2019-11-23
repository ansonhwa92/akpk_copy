using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Setting.Controllers
{
    public class GroupController : FEPController
    {
        private DbEntities db = new DbEntities();

        // List
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var resCities = await WepApiMethod.SendApiAsync<List<ReturnTargetedGroupCities>>(HttpVerbs.Get, $"Setting/Group/GetCities");

            if (!resCities.isSuccess)
            {
                return HttpNotFound();
            }

            var cities = resCities.Data;

            ViewBag.CityId = new SelectList(cities, "Code", "Name");

            var model = new CreateTargetedGroup();
            return View(model);
        }

        // Process create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTargetedGroup model)
        {
            var dupName = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Setting/Group/NameExists?id={null}&name={model.Name}");

            if (dupName.Data)
            {
                ModelState.AddModelError("Name", "A Group with the same Name already exists in the system");
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Setting/Group/Create", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Create Target Group: " + response.Data, model);

                    TempData["SuccessMessage"] = "Target Group " + response.Data + " created successfully.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create Target Group.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }

            }

            return View(model);
        }

        // Edit
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var response = await WepApiMethod.SendApiAsync<EditTargetedGroup>(HttpVerbs.Get, $"Setting/Group/GetSingle?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            var resCities = await WepApiMethod.SendApiAsync<List<ReturnTargetedGroupCities>>(HttpVerbs.Get, $"Setting/Group/GetCities");

            if (!resCities.isSuccess)
            {
                return HttpNotFound();
            }

            var cities = resCities.Data;

            ViewBag.CityId = new SelectList(cities, "Code", "Name", model.CityCode);

            return View(model);
        }

        // Process edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditTargetedGroup model)
        {
            var dupName = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Setting/Group/NameExists?id={model.ID}&name={model.Name}");

            if (dupName.Data)
            {
                ModelState.AddModelError("Name", "A Group with the same Name already exists in the system");
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Setting/Group/Update", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Edit Target Group: " + response.Data, model);

                    TempData["SuccessMessage"] = "Target Group " + response.Data + " updated successfully.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to update Target Group.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }

            }

            return View(model);
        }

        // Delete
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DeleteTargetedGroup>(HttpVerbs.Get, $"Setting/Group/GetSingle?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        // Process delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"Setting/Group/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Setting, "Delete Target Group: " + response.Data);

                TempData["SuccessMessage"] = "Target Group " + response.Data + " successfully deleted.";

                return RedirectToAction("Index", "Group", new { area = "Setting" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Target Group.";

                return RedirectToAction("Details", "Group", new { area = "Setting", @id = id });
            }
        }

        // Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsTargetedGroup>(HttpVerbs.Get, $"Setting/Group/GetSingle?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

    }
}