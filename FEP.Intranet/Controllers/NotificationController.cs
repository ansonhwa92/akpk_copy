using FEP.Helper;
using FEP.WebApiModel.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Controllers
{
    public class NotificationController : FEPController
    {
        // GET: Notification
        public async Task<ActionResult> RefreshNotification()
        {
            var model = new List<NotificationModel>();
                        
            var response = await WepApiMethod.SendApiAsync<List<NotificationModel>>(HttpVerbs.Get, $"System/Notification?userId={CurrentUser.UserId}");

            if (response.isSuccess)
            {
                model = response.Data;

                model.ForEach(s =>
                {
                    s.Message = s.Message.Length > 60 ? s.Message.Substring(0, 57) + "..." : s.Message;
                });// truncate message
            }

            return PartialView("_Notification", model);
        }

        public ActionResult List()
        {
            return View();
        }

        public async Task<ActionResult> ClearNotification()
        {
            var response = await WepApiMethod.SendApiAsync<List<NotificationModel>>(HttpVerbs.Put, $"System/Notification/MarkReadAll?userId={CurrentUser.UserId}");

            return PartialView("_Notification", new List<NotificationModel>());
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            await WepApiMethod.SendApiAsync<List<NotificationModel>>(HttpVerbs.Put, $"System/Notification/MarkRead?id={id}");

            var response = await WepApiMethod.SendApiAsync<NotificationModel>(HttpVerbs.Get, $"System/Notification?id={id}");

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

            await WepApiMethod.SendApiAsync<List<NotificationModel>>(HttpVerbs.Put, $"System/Notification/MarkRead?id={id}");

            var response = await WepApiMethod.SendApiAsync<NotificationModel>(HttpVerbs.Get, $"System/Notification?id={id}");

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

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"System/Notification?id={id}");

            if (response.isSuccess)
            {

                TempData["SuccessMessage"] = Language.Notification.AlertSuccessDelete;

                return RedirectToAction("List", "Notification", new { area = "" });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Notification.AlertFailDelete;

                return RedirectToAction("Details", "Notification", new { area = "", @id = id });
            }

        }
    }
}