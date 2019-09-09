using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("SLAReminder")]
    public class SLAReminder
    {
        [Key]
        public int Id { get; set; }
        public SLAEventType SLAEventType { get; set; }
        public NotificationType NotificationType { get; set; }
        public string ETCode { get; set; }
        public int SLAResolutionTime { get; set; }
        public int IntervalDuration { get; set; }
        public SLADurationType? SLADurationType { get; set; }
    }

    public class SLAReminderStatus
    {
        [Key]
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }
    }

    public enum NotificationReminderStatusType
    {
        Open,
        Suspended,
        Closed
    }

    public enum SLADurationType
    {
        [Display(Name = "Hours")]
        Hours,
        [Display(Name = "Days")]
        Days,
    }

    public enum SLAEventType
    {
        [Display(Name = "Verify Public Event")]
        VerifyPublicEvent,
        [Display(Name = "Approve Public Event")]
        ApprovePublicEvent,
        [Display(Name = "Payment")]
        Payment,
        [Display(Name = "Verify External Request")]
        VerifyExternalRequest,
        [Display(Name = "Approve External Request")]
        ApproveExternalRequest,
        [Display(Name = "Verify Courses")]
        VerifyCourses,
        [Display(Name = "Approve Courses")]
        ApproveCourses,
        [Display(Name = "Verify Survey")]
        VerifySurvey,
        [Display(Name = "Approve Survey")]
        ApproveSurvey,
        [Display(Name = "Verify Publication")]
        VerifyPublication,
        [Display(Name = "Approve Publication")]
        ApprovePublication
    }
}
