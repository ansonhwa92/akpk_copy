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
    public class ListNotificationTemplateModel
    {
        public FilterNotificationTemplateModel Filter { get; set; }
        public NotificationTemplateModel List { get; set; }
    }

    public class FilterNotificationTemplateModel : DataTableModel
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
    public class NotificationTemplateModel
    {
        public int Id { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "ID")]
        public string NotificationTypeName { get; set; }

        [Display(Name = "Description")]
        public string TemplateName { get; set; }

        [Display(Name = "Email Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Email Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "Send Email")]
        public bool enableEmail { get; set; }

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
    
    public class DetailsNotificationTemplateModel : NotificationTemplateModel
    {
        public DetailsNotificationTemplateModel() { }
    }

    public class DeleteNotificationTemplateModel : NotificationTemplateModel
    {
        public DeleteNotificationTemplateModel() { }
    }

    public class EditNotificationTemplateModel
    {
        public int Id { get; set; }

        public List<String> ParameterList { get; set; }

        [Display(Name = "Notification Type")]
        public IEnumerable<SelectListItem> NotificationTypeList { get; set; }

        [Display(Name = "Parameter Available")]
        public List<ParameterList> TemplateParameterTypeList { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Description")]
        public string TemplateName { get; set; }

        [Display(Name = "Email Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Email Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "Send Email")]
        public bool enableEmail { get; set; }

        [Display(Name = "SMS Message")]
        public string SMSMessage { get; set; }
        [Display(Name = "Send SMS")]
        public bool enableSMSMessage { get; set; }

        [Display(Name = "Web Message")]
        public string WebMessage { get; set; }
        [Display(Name = "Send Web Message")]
        public bool enableWebMessage { get; set; }

    }

    public class ParameterList
    {
        public TemplateParameterType TemplateParameterType { get; set; }
        public string parameterDisplayName { get; set; }
    }

    public class ParameterToSend
    {
        public TemplateParameterType TemplateParameterType { get; set; }
        public string ParamTypeText { get; set; }
        public string Value { get; set; }
    }

    


    public class CreateNotificationTemplateModel
    {
        [Display(Name = "Notification Type")]
        public IEnumerable<SelectListItem> NotificationTypeList { get; set; }

        [Display(Name = "Parameter Available")]
        public List<ParameterList> TemplateParameterTypeList { get; set; }

        public List<String> ParameterList { get; set; }

        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Description")]
        public string TemplateName { get; set; }

        [Display(Name = "Send Email")]
        public bool enableEmail { get; set; }

        [Display(Name = "Email Subject")]
        public string TemplateSubject { get; set; }

        [Display(Name = "Template Reference Number")]
        public string TemplateRefNo { get; set; }

        [Display(Name = "Email Message")]
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
