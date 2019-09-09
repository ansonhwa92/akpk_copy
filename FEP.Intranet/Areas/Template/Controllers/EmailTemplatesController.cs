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
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

namespace FEP.Intranet.Areas.Template.Controllers
{
    public class EmailTemplatesController : FEPController
    {
        //private DbEntities db = new DbEntities();

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

        [HttpGet]
        // GET: Template/EmailTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsEmailTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        // GET: Template/EmailTemplates/Create
        public ActionResult Create()
        {
            
            CreateEmailTemplateModel model = new CreateEmailTemplateModel();
            model.CreatedBy = CurrentUser.UserId.Value;
            model.NotificationTypeList = (Enum.GetValues(typeof(NotificationType)).Cast<int>()
                .Select(e => new SelectListItem()
                {
                    Text = ((DisplayAttribute)
                    typeof(NotificationType)
                    .GetMember(Enum.GetName(typeof(NotificationType), e).ToString())
                    .First()
                    .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name,
                    //Enum.GetName(typeof(NotificationType), e),
                    Value = e.ToString()
                })).ToList();
        
            
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
                    NotificationType = model.NotificationType,
                    TemplateName = model.TemplateName,
                    TemplateSubject = model.TemplateSubject,
                    TemplateRefNo = model.TemplateRefNo,
                    TemplateMessage = Server.HtmlEncode(model.TemplateMessage),
                    CreatedBy = CurrentUser.UserId.Value,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    enableSMSMessage = model.enableSMSMessage,
                    SMSMessage = model.SMSMessage,
                    enableWebMessage = model.enableWebMessage,
                    WebMessage = model.WebMessage,
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

        [HttpGet]
        // GET: Template/EmailTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsEmailTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            EditEmailTemplateModel model = new EditEmailTemplateModel
            {
                Id = response.Data.Id,
                NotificationType = response.Data.NotificationType,
                TemplateName = response.Data.TemplateName,
                TemplateSubject = response.Data.TemplateSubject,
                TemplateRefNo = response.Data.TemplateRefNo,
                TemplateMessage = HttpUtility.HtmlDecode(response.Data.TemplateMessage),
                enableSMSMessage = response.Data.enableSMSMessage,
                SMSMessage = response.Data.SMSMessage,
                enableWebMessage = response.Data.enableWebMessage,
                WebMessage = response.Data.WebMessage,
            };
            model.NotificationTypeList = (Enum.GetValues(typeof(NotificationType)).Cast<int>()
                .Select(e => new SelectListItem()
                {
                    Text = ((DisplayAttribute)
                    typeof(NotificationType)
                    .GetMember(Enum.GetName(typeof(NotificationType), e).ToString())
                    .First()
                    .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name,
                    //Enum.GetName(typeof(NotificationType), e),
                    Value = e.ToString()
                })).ToList();

            return View(model);
        }

        // POST: Template/EmailTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(EditEmailTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                model.TemplateMessage = Server.HtmlEncode(model.TemplateMessage);
                var response = await WepApiMethod.SendApiAsync<EditEmailTemplateModel>(HttpVerbs.Put, $"Template/Email?id={model.Id}", model);
                if (response.isSuccess)
                {
                    LogActivity("Update Email Template");
                    TempData["SuccessMessage"] = "Email Template updated successfully";

                    return RedirectToAction("Details", "EmailTemplates", new { area = "Template", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update Email Template";
                    return RedirectToAction("Details", "EmailTemplates", new { area = "Template", @id = model.Id });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Email Template";
                return RedirectToAction("Details", "EmailTemplates", new { area = "Template", @id = model.Id });
            }
        }

        // GET: Template/EmailTemplates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsEmailTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            DeleteEmailTemplateModel model = new DeleteEmailTemplateModel
            {
                Id = response.Data.Id,
                NotificationType = response.Data.NotificationType,
                TemplateName = response.Data.TemplateName,
                TemplateMessage = response.Data.TemplateMessage,
                CreatedByName = response.Data.CreatedByName,
                CreatedDate = response.Data.CreatedDate,
                LastModified = response.Data.LastModified,
                enableSMSMessage = response.Data.enableSMSMessage,
                SMSMessage = response.Data.SMSMessage,
                enableWebMessage = response.Data.enableWebMessage,
                WebMessage = response.Data.WebMessage,

            };

            return View(model);
        }

        // POST: Template/EmailTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Delete, $"Template/Email?id={id}");
            if (response.isSuccess)
            {
                LogActivity("Delete Email Template");

                TempData["SuccessMessage"] = "Email Template successfully deleted.";
                return RedirectToAction("List");
            }
            else
            {

                TempData["ErrorMessage"] = "Failed to delete Email Template.";
                return RedirectToAction("List");
            }

        }

        /*protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
