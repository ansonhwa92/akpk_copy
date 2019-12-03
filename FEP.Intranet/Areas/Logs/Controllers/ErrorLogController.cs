using FEP.Helper;
using FEP.WebApiModel.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Logs.Controllers
{
    public class ErrorLogController : FEPController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> List(FilterErrorLogModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"Logs/ErrorLog/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<ErrorLogModel>(HttpVerbs.Get, $"Logs/ErrorLog?id={id}");

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

            var response = await WepApiMethod.SendApiAsync<ErrorLogModel>(HttpVerbs.Get, $"Logs/ErrorLog?id={id}");

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

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Logs/ErrorLog?id={id}");

            if (response.isSuccess)
            {

                TempData["SuccessMessage"] = Language.ErrorLog.AlertSuccessDelete;

                return RedirectToAction("List", "ErrorLog", new { area = "Logs" });
            }
            else
            {

                TempData["ErrorMessage"] = Language.ErrorLog.AlertFailDelete;

                return RedirectToAction("Details", "ErrorLog", new { area = "Logs", @id = id });
            }

        }
    }
}