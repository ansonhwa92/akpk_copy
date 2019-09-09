using FEP.Model;
using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.SLAReminder
{

    public class ListSLAReminderModel
    {
        [Display(Name = "Duration Type")]
        public IEnumerable<SelectListItem> SLADurationTypeList { get; set; }
        public List<SLAReminderModel> SLAReminderList { get; set; }

        public FilterSLAReminderModel filter { get; set; }
        public ListSLAReminderModel() { }
        public ListSLAReminderModel(List<SLAReminderModel> ListSLAReminder)
        {
            this.SLAReminderList = ListSLAReminder;
        }
    }
    public class FilterSLAReminderModel : DataTableModel
    {
        [Display(Name = "Stages")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "ET Code")]
        public string ETCode { get; set; }

        [Display(Name = "SLA")]
        public int SLAResolutionTime { get; set; }

        [Display(Name = "Reminder Notice")]
        public int IntervalDuration { get; set; }

    }
    public class SLAReminderModel
    {
        
        public int Id { get; set; }

        [Display(Name = "SLA Event")]
        public SLAEventType SLAEventType { get; set; }

        [Display(Name = "Stages")]
        public NotificationType NotificationType { get; set; }
        public string StagesName { get; set; }

        [Display(Name = "ET Code")]
        public string ETCode { get; set; }

        [Display(Name = "SLA")]
        public int SLAResolutionTime { get; set; }

        [Display(Name = "Reminder Notice")]
        public int IntervalDuration { get; set; }

        public SLADurationType? SLADurationType { get; set; }
    }

    public class EditSLAReminderModel
    {
        public int Id { get; set; }

        [Display(Name = "ET Code")]
        public string ETCode { get; set; }

        [Display(Name = "SLA")]
        public int SLAResolutionTime { get; set; }

        [Display(Name = "Reminder Notice")]
        public int IntervalDuration { get; set; }

        public SLADurationType? SLADurationType { get; set; }
    }

    public class SLAReminderStatusModel
    {
        [Display(Name = "Notification ID")]
        public int NotificationId { get; set; }
        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }
        [Display(Name = "Notification Status")]
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }
    }

    public class CreateSLAReminderStatusModel : SLAReminderStatusModel
    {
        CreateSLAReminderStatusModel() { }
    }
    public class EditSLAReminderStatusModel 
    {
        public int NotificationId { get; set; }
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }
    }
}
