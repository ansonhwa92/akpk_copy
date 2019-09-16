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
using System.Reflection;
using FEP.Intranet.Areas.Template.Models;
using FEP.WebApiModel.SLAReminder;
using FEP.Intranet.Areas.Template.Method;

namespace FEP.Intranet.Areas.Template.Controllers
{
    public class EmailTemplatesController : FEPController
    {
        private DbEntities db = new DbEntities();

        TemplateFunction TemplateFunction = new TemplateFunction();

        // GET: Template/EmailTemplates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            //ViewBag.WebApiURL = WebApiURL;
            //var filter = new FilterEmailTemplateModel();
            return View(new ListNotificationTemplateModel { });
        }

        [HttpGet]
        // GET: Template/EmailTemplates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsNotificationTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        // GET: Template/EmailTemplates/Create
        public ActionResult Create()
        {

            CreateNotificationTemplateModel model = new CreateNotificationTemplateModel();
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

            model.TemplateParameterTypeList = new List<ParameterList>();
            foreach (TemplateParameterType param in Enum.GetValues(typeof(TemplateParameterType)))
            {
                ParameterList paramList = new ParameterList
                {
                    TemplateParameterType = param,
                    parameterDisplayName = param.GetDisplayName()
                };
                model.TemplateParameterTypeList.Add(paramList);
            }

            /*model.enableEmail = true;
            model.enableSMSMessage = true;
            model.enableWebMessage = true;*/
            
            return View(model);
        }

        // POST: Template/EmailTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(CreateNotificationTemplateModel model)//([Bind(Include = "Id,TemplateName,TemplateMessage,CreatedDate,CreatedBy,Display")] EmailTemplate emailTemplate)

        {
            if (ModelState.IsValid)
            {
                CreateNotificationTemplateModel obj = new CreateNotificationTemplateModel
                {
                    NotificationType = model.NotificationType,
                    TemplateName = model.TemplateName,
                    TemplateSubject = model.TemplateSubject,
                    TemplateRefNo = model.TemplateRefNo,
                    TemplateMessage = Server.HtmlEncode(model.TemplateMessage),
                    enableEmail = model.enableEmail,
                    CreatedBy = CurrentUser.UserId.Value,
                    CreatedDate = DateTime.Now,
                    //LastModified = DateTime.Now,
                    enableSMSMessage = model.enableSMSMessage,
                    SMSMessage = model.SMSMessage,
                    enableWebMessage = model.enableWebMessage,
                    WebMessage = model.WebMessage,
                    //Display = true
                };

                List<string> ListA, ParamList;// = new List<string>();
                ListA = new List<string>();
                ParamList = new List<string>();
                if (obj.enableEmail)
                {
                    ParamList = ParamList.Union(ListA).ToList();
                    if(obj.TemplateSubject != null)
                        ParamList = ParamList.Union(ParameterListing(obj.TemplateSubject)).ToList();
                    if (obj.TemplateMessage != null)
                        ParamList = ParamList.Union(ParameterListing(obj.TemplateMessage)).ToList();
                }
                if (obj.enableSMSMessage)
                {
                    if (obj.SMSMessage != null)
                        ParamList = ParamList.Union(ParameterListing(obj.SMSMessage)).ToList();
                }
                if (obj.enableWebMessage)
                {
                    if (obj.WebMessage != null)
                        ParamList = ParamList.Union(ParameterListing(obj.WebMessage)).ToList();
                }

                obj.ParameterList = ParamList;

                //var response = await WepApiMethod.SendApiAsync<CreateNotificationTemplateModel>(HttpVerbs.Post, $"Template/Email/", obj);

                //test generate email
                //1 create ParamListToSend
                //2 generate body message
                //3 generate subject message
                //4 generate schedule to send email
                //5 call email API
                ParameterListToSend paramToSend = new ParameterListToSend();
                paramToSend.EventName = "Hari Terbuka AKPK";
                paramToSend.EventCode = "HTAKPK2019";
                paramToSend.EventLocation = "Dewan Terbuka AKPK";

                CreateAutoReminder reminder = new CreateAutoReminder
                {
                    NotificationType = NotificationType.Verify_Public_Event_Creation,
                    ParameterListToSend = paramToSend,
                    StartNotificationDate = DateTime.Now

                };

                var response = await WepApiMethod.SendApiAsync<string>
                    (HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);

                //TemplateFunction.GenerateAutoNotificationReminder(response.Data.NotificationType, paramToSend, DateTime.Now);

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

       

        //proses parameter function
        public List<string> ParameterListing(string message)
        {
            bool FoundParam = false;
            string tempParam = "";
            List<string> ParamList = new List<string>();
            for (int i = 0; i < message.Length; i++)
            {

                if (message[i] == '[' && message[i + 1] == '#')
                {
                    i += 2;
                    FoundParam = true;
                }
                if (message[i] == ']')
                {
                    FoundParam = false;
                    ParamList.Add(tempParam);
                    tempParam = "";
                }

                if (FoundParam)
                {
                    tempParam += message[i];
                }
            }

            return ParamList;
        }

        [HttpGet]
        // GET: Template/EmailTemplates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsNotificationTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            EditNotificationTemplateModel model = new EditNotificationTemplateModel
            {
                Id = response.Data.Id,
                NotificationType = response.Data.NotificationType,
                TemplateName = response.Data.TemplateName,
                TemplateSubject = response.Data.TemplateSubject,
                TemplateRefNo = response.Data.TemplateRefNo,
                TemplateMessage = HttpUtility.HtmlDecode(response.Data.TemplateMessage),
                enableEmail = response.Data.enableEmail,
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

            /*
            foreach (TemplateParameterType param in Enum.GetValues(typeof(TemplateParameterType)))
            {
                ParameterList paramList = new ParameterList
                {
                    TemplateParameterType = param,
                    parameterDisplayName = param.GetDisplayName()
                };
                model.TemplateParameterTypeList.Add(paramList);
            }*/
            model.TemplateParameterTypeList = new List<ParameterList>();
            var response2 = await WepApiMethod.SendApiAsync<List<TemplateParameterType>>
                (HttpVerbs.Get, $"Reminder/SLA/GetParameterList?id={(int)model.NotificationType}");
            if (response2.isSuccess)
            {
                foreach (var item in response2.Data)
                {
                    ParameterList paramList = new ParameterList
                    {
                        TemplateParameterType = item,
                        parameterDisplayName = item.GetDisplayName()
                    };
                    model.TemplateParameterTypeList.Add(paramList);
                }
            }
            return View(model);
        }

        

        // POST: Template/EmailTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(EditNotificationTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                model.TemplateMessage = Server.HtmlEncode(model.TemplateMessage);
                var response = await WepApiMethod.SendApiAsync<EditNotificationTemplateModel>(HttpVerbs.Put, $"Template/Email?id={model.Id}", model);
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

            var response = await WepApiMethod.SendApiAsync<DetailsNotificationTemplateModel>(HttpVerbs.Get, $"Template/Email?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            DeleteNotificationTemplateModel model = new DeleteNotificationTemplateModel
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
