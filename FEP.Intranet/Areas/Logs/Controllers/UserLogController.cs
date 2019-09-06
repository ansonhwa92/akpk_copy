using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Logs.Controllers
{
    public class UserLogController : FEPController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<UserLogModel>(HttpVerbs.Get, $"Logs/UserLog?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<UserLogModel>(HttpVerbs.Get, $"Logs/UserLog?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Logs/UserLog?id={id}");

            if (response.isSuccess)
            {
                
                TempData["SuccessMessage"] = Language.UserLog.AlertSuccessDelete;

                return RedirectToAction("List", "UserLog", new { area = "Logs" });
            }
            else
            {

                TempData["ErrorMessage"] = Language.UserLog.AlertFailDelete;

                return RedirectToAction("Details", "UserLog", new { area = "Logs", @id = id });
            }

        }
    }
}