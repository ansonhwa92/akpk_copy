using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FEP.Intranet.Areas.SLAReminder.Controllers
{
    public class SLARemindersController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: SLAReminder/SLAReminders
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<SLAReminderModel>>(HttpVerbs.Get, $"Reminder/SLA/");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            ListSLAReminderModel model = new ListSLAReminderModel(response.Data);
            model.SLADurationTypeList = (Enum.GetValues(typeof(SLADurationType)).Cast<int>()
                .Select(e => new SelectListItem()
                {
                    Text = ((DisplayAttribute)
                    typeof(SLADurationType)
                    .GetMember(Enum.GetName(typeof(SLADurationType), e).ToString())
                    .First()
                    .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name,
                    Value = ((int)e).ToString()
                })).ToList();

            return View(model);
        }

        // GET: SLAReminder/SLAReminders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // GET: SLAReminder/SLAReminders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SLAReminder/SLAReminders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SLAEventType,NotificationType,ETCode,SLAResolutionTime,IntervalDuration,SLADurationType")] SLAReminderModel sLAReminder)
        {
            if (ModelState.IsValid)
            {
                //db.SLAReminder.Add(sLAReminder);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sLAReminder);
        }

        // GET: SLAReminder/SLAReminders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // POST: SLAReminder/SLAReminders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit()
        {
            EditSLAReminderModel model = new EditSLAReminderModel
            {
                Id = Convert.ToInt32(Request["item.Id"]),
                ETCode = Request["item.ETCode"],
                SLAResolutionTime = Convert.ToInt32(Request["SLAResolutionTime"]),
                IntervalDuration = Convert.ToInt32(Request["IntervalDuration"]),
                SLADurationType = (SLADurationType)Convert.ToInt32(Request["item.SLADurationType"])
            };

            if (model != null)
            {
                var response = await WepApiMethod.SendApiAsync<EditSLAReminderModel>(HttpVerbs.Put, $"Reminder/SLA/?id={model.Id}", model);
                if(response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Update SLA Reminder Settings");
					TempData["SuccessMessage"] = "SLA Reminder updated successfully";

                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update SLA Reminder Settings";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update SLA Reminder Settings";
                return RedirectToAction("List");
            }

        }

        // GET: SLAReminder/SLAReminders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View();
        }

        // POST: SLAReminder/SLAReminders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //SLAReminderModel sLAReminder = db.SLAReminder.Find(id);
            //db.SLAReminder.Remove(sLAReminder);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
