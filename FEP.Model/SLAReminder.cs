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

        public NotificationCategory NotificationCategory { get; set; }
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
        public string NotificationId { get; set; }

 
    }

    public enum NotificationCategory
    {
        [Display(Name = "Event")]
        Event = 1,
        [Display(Name = "Learning")]
        Learning = 2,
        [Display(Name = "Research And Publication")]
        ResearchAndPublication = 3,
        [Display(Name = "System")]
        System = 4,
        [Display(Name = "Payment")]
        Payment = 5
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

        [Display(Name = "System Error")]
        SystemError,
        [Display(Name = "Account Activation")]
        ActivateAccount,
        [Display(Name = "Reset Password")]
        ResetPassword,

		[Display(Name = "Submit Public Event")]
		SubmitPublicEvent,
		[Display(Name = "Verify Public Event")]
        VerifyPublicEvent,
        [Display(Name = "Approve Public Event")]
        ApprovePublicEvent,
		[Display(Name = "Publish Public Event")]
		PublishPublicEvent,
		[Display(Name = "Reject Public Event")]
		RejectPublicEvent,
		[Display(Name = "Cancel Public Event")]
		CancelPublicEvent,

		[Display(Name = "Submit Exhibition Roadshow")]
		SubmitExhibitionRoadshow,
		[Display(Name = "Verify Exhibition Roadshow")]
		VerifyExhibitionRoadshow,
		[Display(Name = "Approve Exhibition Roadshow")]
		ApproveExhibitionRoadshow,
		[Display(Name = "Reject Exhibition Roadshow")]
		RejectExhibitionRoadshow,

		[Display(Name = "Submit Exhibition Roadshow - Duty Roster")]
		SubmitExhibitionRoadshowDutyRoster,
		[Display(Name = "Verify Exhibition Roadshow - Duty Roster")]
		VerifyExhibitionRoadshowDutyRoster,
		[Display(Name = "Approve Exhibition Roadshow - Duty Roster")]
		ApproveExhibitionRoadshowDutyRoster,
		[Display(Name = "Reject Exhibition Roadshow - Duty Roster")]
		RejectExhibitionRoadshowDutyRoster,

		[Display(Name = "Submit Media Interview")]
		SubmitMediaInterview,
		[Display(Name = "Verify Media Interview")]
		VerifyMediaInterview,
		[Display(Name = "Approve Media Interview")]
		ApproveMediaInterview,
		[Display(Name = "Reject Media Interview")]
		RejectMediaInterview,


		[Display(Name = "Payment")]
        Payment,

        // ELearning
        [Display(Name = "Verify Courses")]
        VerifyCourses,
        [Display(Name = "Approve Courses")]
        ApproveCourses,
        [Display(Name = "Invite To Enroll to Course")]
        InviteToCourse,


        // payment
        // guarantee letter - not verified (reg user), not yet uploaded (admin), upload/reupload (admin)
        // payment - order confirmation (reg user), 
        // refund (publication) - submit refund request (admin/finance), completed (admin), not completed (admin/finance)

        // survey
        [Display(Name = "Submit Survey")]
        SubmitSurvey,
        [Display(Name = "Cancel Survey")]
        CancelSurvey,
        [Display(Name = "Publish Survey")]
        PublishSurvey,

        [Display(Name = "Verify Survey")]
        VerifySurvey,
        [Display(Name = "Approve Survey")]
        ApproveSurvey,

        [Display(Name = "Distribute Survey")]           // to respondent
        DistributeSurvey,
        [Display(Name = "Submit Survey Response")]      // to admin and respondent
        AnswerSurvey,

        // publication
        [Display(Name = "Submit Publication")]
        SubmitPublication,
        [Display(Name = "Cancel Publication")]
        CancelPublication,
        [Display(Name = "Publish Publication")]
        PublishPublication,
        [Display(Name = "Modify Publication")]
        ModifyPublication,
        [Display(Name = "Withdraw Publication")]
        WithdrawPublication,
        [Display(Name = "Cancel Modify Publication")]
        CancelModifyPublication,
        [Display(Name = "Cancel Withdraw Publication")]
        CancelWithdrawPublication,

        [Display(Name = "Verify Publication")]
        VerifyPublication,
        [Display(Name = "Verify Publication Modification")]
        VerifyPublicationModification,
        [Display(Name = "Verify Publication Withdrawal")]
        VerifyPublicationWithdrawal,

        [Display(Name = "Approve Publication")]
        ApprovePublication,
        [Display(Name = "Approve Publication Modification")]
        ApprovePublicationModification,
        [Display(Name = "Approve Publication Withdrawal")]
        ApprovePublicationWithdrawal,

        // temp only
        [Display(Name = "Refund Publication")]
        RefundPublication,

        // other refunds can add here (in case wanna use different params)
        [Display(Name = "Refund")]
        Refund
    }
}
