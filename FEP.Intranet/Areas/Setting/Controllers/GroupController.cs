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
        public ActionResult Create()
        {
            var model = new CreateTargetedGroup();
            return View(model);
        }

        // Process create
        [HttpPost]
        public async Task<ActionResult> Create(CreateTargetedGroup model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Setting/Group/Create", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Create Targeted Group: " + response.Data, model);

                    TempData["SuccessMessage"] = "Targeted Group " + response.Data + " created successfully.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create Targeted Group.";

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
                        
            return View(model);
        }

        // Process edit
        [HttpPost]
        public async Task<ActionResult> Edit(EditTargetedGroup model)
        {                        
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Setting/Group/Update", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Edit Targeted Group: " + response.Data, model);

                    TempData["SuccessMessage"] = "Targeted Group " + response.Data + " updated successfully.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to update Targeted Group.";

                    return RedirectToAction("Index", "Group", new { area = "Setting" });
                }

            }

            return View(model);
        }
    }
}