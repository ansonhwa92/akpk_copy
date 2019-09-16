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
        public int Id { get; set; }
        [Display(Name = "Notification Type")]
        public NotificationType NotificationType { get; set; }
        [Display(Name = "Notification Status")]
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Closed Date")]
        public DateTime? closeDate { get; set; }
    }

    public class CreateSLAReminderStatusModel : SLAReminderStatusModel
    {
        public CreateSLAReminderStatusModel() { }
    }
    public class EditSLAReminderStatusModel 
    {
        public int NotificationId { get; set; }
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }
    }

    public class BulkNotificationModel
    {

        [Display(Name = "SLA Reminder Status")]
        public int SLAReminderStatusId { get; set; }

        [Display(Name = "Notification Medium")]
        public NotificationMedium NotificationMedium { get; set; }

        [Display(Name = "Notification ID")]
        public int NotificationId { get; set; }
    }
    public class CreateBulkNotificationModel : BulkNotificationModel
    {
        public CreateBulkNotificationModel() { }
    }

    public class CreateAutoReminder
    {
        public NotificationType NotificationType { get; set; }
        public ParameterListToSend ParameterListToSend { get; set; }
        public DateTime StartNotificationDate { get; set; }
    }

    public class ParameterListToSend
    {
        //1-20 for System
        public string FriendlySiteName { get; set; }
        public string UserFullName { get; set; }
        public string UserRole { get; set; }

        //21-40 for Verify/Approval Public Event
        public string EventName { get; set; }
        public string EventLocation { get; set; }
        public string EventCode { get; set; }
        public string EventApproval { get; set; }

        //41-60 for Payment
        public string GLHolderName { get; set; }
        public string PaymentRefNo { get; set; }
        public string PaymentStatus { get; set; }


    }
}
