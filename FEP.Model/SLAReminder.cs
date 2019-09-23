﻿using System;
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

    [Table("SLAReminderStatus")]
    public class SLAReminderStatus
    {
        [Key]
        public int Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationReminderStatusType NotificationReminderStatusType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? closeDate { get; set; }
    }

    [Table("BulkNotification")]
    public class BulkNotification
    {
        [Key]
        public int Id { get; set; }

        public NotificationMedium NotificationMedium { get; set; }

        public int SLAReminderStatusId { get; set; }
        public int NotificationId { get; set; }

 
    }

    public enum NotificationMedium
    {
        Email,
        SMS,
        Web
    }

    public enum NotificationReminderStatusType
    {
        [Display(Name = "Open")]
        Open,
        [Display(Name = "Closed")]
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
        [Display(Name = "System")]
        System,
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