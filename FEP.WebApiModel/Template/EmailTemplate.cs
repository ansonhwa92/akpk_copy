using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Template
{
    public class ListEmailTemplateModel
    {
        public FilterEmailTemplateModel Filter { get; set; }
        public EmailTemplateModel List { get; set; }
    }

    public class FilterEmailTemplateModel : DataTableModel
    {
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
    }
    public class EmailTemplateModel
    {
        public int Id { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "ID")]
        public string NotificationTypeName { get; set; }

        [Display(Name = "Description")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Template Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "SMS Message")]
        public string SMSMessage { get; set; }
        [Display(Name = "Send SMS")]
        public bool enableSMSMessage { get; set; }

        [Display(Name = "Web Message")]
        public string WebMessage { get; set; }
        [Display(Name = "Send Web Message")]
        public bool enableWebMessage { get; set; }

       

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }

        

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        public bool Display { get; set; }
    }

    public class TemplateParametersModel
    {
        [Display(Name = "SLA")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Parameters")]
        public TemplateParameterType TemplateParameterType { get; set; }
    }
    
    public class DetailsEmailTemplateModel : EmailTemplateModel
    {
        public DetailsEmailTemplateModel() { }
    }

    public class DeleteEmailTemplateModel : EmailTemplateModel
    {
        public DeleteEmailTemplateModel() { }
    }

    public class EditEmailTemplateModel
    {
        public int Id { get; set; }

        [Display(Name = "Notification Type")]
        public IEnumerable<SelectListItem> NotificationTypeList { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Template Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "SMS Message")]
        public string SMSMessage { get; set; }
        [Display(Name = "Send SMS")]
        public bool enableSMSMessage { get; set; }

        [Display(Name = "Web Message")]
        public string WebMessage { get; set; }
        [Display(Name = "Send Web Message")]
        public bool enableWebMessage { get; set; }

    }

    public class CreateEmailTemplateModel
    {
        [Display(Name = "Notification Type")]
        public IEnumerable<SelectListItem> NotificationTypeList { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Template Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "SMS Message")]
        public string SMSMessage { get; set; }
        [Display(Name = "Send SMS")]
        public bool enableSMSMessage { get; set; }

        [Display(Name = "Web Message")]
        public string WebMessage { get; set; }
        [Display(Name = "Send Web Message")]
        public bool enableWebMessage { get; set; }

    }
}
