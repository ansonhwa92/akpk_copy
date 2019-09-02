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
using FEP.WebApiModel.Template;
using System.Threading.Tasks;

namespace FEP.Intranet.Areas.Template.Controllers
{
    public class EmailTemplatesController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: Template/EmailTemplates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            //ViewBag.WebApiURL = WebApiURL;
            //var filter = new FilterEmailTemplateModel();
            return View(new ListEmailTemplateModel {});
        }

        // GET: Template/EmailTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // GET: Template/EmailTemplates/Create
        public ActionResult Create()
        {
            CreateEmailTemplateModel model = new CreateEmailTemplateModel();
            model.CreatedBy = CurrentUser.UserId.Value;
            return View(model);
        }

        // POST: Template/EmailTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmailTemplateModel model)//([Bind(Include = "Id,TemplateName,TemplateMessage,CreatedDate,CreatedBy,Display")] EmailTemplate emailTemplate)

        {
            if (ModelState.IsValid)
            {
                EmailTemplateModel obj = new EmailTemplateModel
                {
                    TemplateName = model.TemplateName,
                    TemplateMessage = model.TemplateMessage,
                    CreatedBy = CurrentUser.UserId.Value,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    Display = true
                };

                var response = await WepApiMethod.SendApiAsync<EmailTemplateModel>(HttpVerbs.Post, $"Template/Email/", obj);
                if (response.isSuccess)
                {
                    LogActivity("Create Email Template");
                    TempData["SuccessMessage"] = "Email Template created successfully";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create Email Template";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Email Template";
                return RedirectToAction("List");
            }

        }

        // GET: Template/EmailTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Name", emailTemplate.CreatedBy);
            return View(emailTemplate);
        }

        // POST: Template/EmailTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TemplateName,TemplateMessage,CreatedDate,CreatedBy,Display")] EmailTemplate emailTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emailTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Name", emailTemplate.CreatedBy);
            return View(emailTemplate);
        }

        // GET: Template/EmailTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // POST: Template/EmailTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            db.EmailTemplates.Remove(emailTemplate);
            db.SaveChanges();
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
