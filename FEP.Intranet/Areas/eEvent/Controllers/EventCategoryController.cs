using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
	public class EventCategoryController : FEPController
	{

        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<EventCategoryModel>>(HttpVerbs.Get, $"eEvent/EventCategory");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<EventCategoryModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateEventCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eEvent/EventCategory/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.EventCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/EventCategory", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.EventCategory.AlertSuccessCreate;

                    await LogActivity(Modules.Setting, "Create Parameter Event Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.EventCategory.AlertFailCreate;

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name)
        {

            var model = new EditEventCategoryModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditEventCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eEvent/EventCategory/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.EventCategory.ValidExistName;

                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/EventCategory?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.EventCategory.AlertSuccessUpdate;

                    await LogActivity(Modules.Setting, "Update Parameter Event Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.EventCategory.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name)
        {

            var model = new DeleteEventCategoryModel
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

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventCategory?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.EventCategory.AlertSuccessDelete;

                await LogActivity(Modules.Setting, "Delete Parameter Event Category", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.EventCategory.AlertFailDelete;

            return RedirectToAction("List");

        }
    }
}
