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
        [Display(Name = "Notification Type")]
        public IEnumerable<SelectListItem> NotificationTypeList { get; set; }
        [Display(Name = "SLA Event Type")]
        public IEnumerable<SelectListItem> SLAEventTypeList { get; set; }
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

        [Display(Name = "SLA Group")]
        public SLAEventType SLAEventType { get; set; }

    }
    public class SLAReminderModel
    {
        
        public int Id { get; set; }

        [Display(Name = "SLA Event")]
        public SLAEventType SLAEventType { get; set; }
        public string SLAEventTypeName { get; set; }

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
        public string NotificationId { get; set; }
    }
    public class CreateBulkNotificationModel : BulkNotificationModel
    {
        public CreateBulkNotificationModel() { }
    }

    public class CreateAutoReminder
    {
        public NotificationType NotificationType { get; set; }

        public NotificationCategory NotificationCategory { get; set; }
        public ParameterListToSend ParameterListToSend { get; set; }
        public DateTime StartNotificationDate { get; set; }

        public List<int> ReceiverId { get; set; }
    }

    public class ReminderResponse
    {
        public string Status { get; set; }
        public int SLAReminderStatusId { get; set; }
    }

    public class EmailClass
    {
        public string datID { get; set; }
        public int datType { get; set; }
        public int datNotify { get; set; }
        public string dtInsert { get; set; }
        public string dtSchedule { get; set; }
        public string dtExpired { get; set; }
        public string emailTo { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
    public class SMSClass
    {
        public string datID { get; set; }
        public int datType { get; set; }
        public int datNotify { get; set; }
        public string dtInsert { get; set; }
        public string dtSchedule { get; set; }
        public string dtExpired { get; set; }
        public string smsTo { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }


    public class ParameterListToSend
    {
        //1-20 for System
        public string FriendlySiteName { get; set; }
        public string UserFullName { get; set; }
        public string UserRole { get; set; }
        public string Link { get; set; }
        public string LoginDetail { get; set; }
        public string ErrorDetail { get; set; }


        //21-40 for Verify/Approval Public Event
        public string EventName { get; set; }
        public string EventLocation { get; set; }
        public string EventCode { get; set; }
        public string EventApproval { get; set; }

        //41-60 for Payment
        public string GLHolderName { get; set; }
        public string PaymentRefNo { get; set; }
        public string PaymentStatus { get; set; }

        //101-120 for Survey
        public string SurveyTitle { get; set; }
        public string SurveyType { get; set; }
        public string SurveyCode { get; set; }
        public string SurveyApproval { get; set; }

        //121-140 for Publication
        public string PublicationTitle { get; set; }
        public string PublicationAuthor { get; set; }
        public string PublicationCode { get; set; }
        public string PublicationApproval { get; set; }

    }

}
