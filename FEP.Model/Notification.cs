using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    public enum NotificationType
    {
        [Display(Name = "Activate Account")]
        ActivateAccount,

        [Display(Name = "Reset Password")]
        ResetPassword,

        [Display(Name = "System Error")]
        SystemError,

        [Display(Name = "KMC Created")]
        KMCCreated,

        //---------------------------------------------------
        //tajul tambah for SLA

        // Public Event
        [Display(Name = "Public Event - Submit Public Event For Verification")]
        Submit_Public_Event_For_Verification,

        [Display(Name = "Public Event - Verify Public Event")]
        Verify_Public_Event_After_Submit_For_Verification,

        [Display(Name = "Public Event - Approve Public Event By Approver 1")]
        Approve_Public_Event_ByApprover_1,

        [Display(Name = "Public Event - Approve Public Event By Approver 2")]
        Approve_Public_Event_ByApprover_2,

        [Display(Name = "Public Event - Approve Public Event By Approver 3")]
        Approve_Public_Event_ByApprover_3,

        [Display(Name = "Public Event - Cancelled")]
        Cancel_Public_Event,

        [Display(Name = "Public Event - Require Amendment")]
        Reject_Public_Event,

        [Display(Name = "Public Event - Published")]
        Publish_Public_Event,

        //Public Event Cancellation/Modification Request
        [Display(Name = "Cancellation/Modification Request - Submit For Verification")]
        Submit_CancellationModification_For_Verification,

        [Display(Name = "Cancellation/Modification Request - Verify")]
        Verify_CancellationModification,

        [Display(Name = "Cancellation/Modification Request - Approve By Approver 1")]
        Approve_CancellationModification_ByApprover_1,

        [Display(Name = "Cancellation/Modification Request - ApproveBy Approver 2")]
        Approve_CancellationModification_ByApprover_2,

        [Display(Name = "Cancellation/Modification Request - Approve By Approver 3")]
        Approve_CancellationModification_ByApprover_3,

        [Display(Name = "Cancellation/Modification Request - Require Amendment")]
        RequireAmendment_CancellationModification,

        //Media Interview
        [Display(Name = "Media Interview - Submit Media Interview For Verification")]
        Submit_Media_Interview_For_Verification,

        [Display(Name = "Media Interview - Verify Media Interview")]
        Verify_Media_Interview_After_Submit_For_Verification,

        [Display(Name = "Media Interview - Approve Media Interview By Approver 1")]
        Approve_Media_Interview_ByApprover_1,

        [Display(Name = "Media Interview - Approve Media Interview By Approver 2")]
        Approve_Media_Interview_ByApprover_2,

        [Display(Name = "Media Interview - Approve Media Interview By Approver 3")]
        Approve_Media_Interview_ByApprover_3,

        [Display(Name = "Media Interview - Require Amendment")]
        Reject_Media_Interview,

		[Display(Name = "Media Interview - Representative Available")]
		RepAvailable_MediaInterview,
		[Display(Name = "Media Interview - Representative Not Available")]
		RepNotAvailable_MediaInterview,


		//Exhibition Roadshow
		[Display(Name = "Exhibition Roadshow - Submit Exhibition Roadshow For Verification")]
		Submit_Exhibition_RoadShow_For_Verification,
		[Display(Name = "Exhibition Roadshow - Verify Exhibition Roadshow")]
		Verify_Exhibition_RoadShow_After_Submit_For_Verification,

        [Display(Name = "Exhibition Roadshow - Approve Exhibition Roadshow By Approver 1")]
        Approve_Exhibition_RoadShow_ByApprover_1,

        [Display(Name = "Exhibition Roadshow - Approve Exhibition Roadshow By Approver 2")]
        Approve_Exhibition_RoadShow_ByApprover_2,

        [Display(Name = "Exhibition Roadshow - Approve Exhibition Roadshow By Approver 3")]
        Approve_Exhibition_RoadShow_ByApprover_3,

        [Display(Name = "Exhibition RoadShow - Require Amendment")]
        Reject_Exhibition_RoadShow,

        //[Display(Name = "Exhibition RoadShow - Submit Duty Roster")]
        //Submit_DutyRoster_For_Verification,

        //[Display(Name = "Exhibition RoadShow - Verify Duty Roster")]
        //Verify_DutyRoster,

        //[Display(Name = "Exhibition RoadShow - Require Amendment Duty Roster")]
        //NotVerify_DutyRoster,

        //[Display(Name = "Exhibition RoadShow - Approve Duty Roster")]
        //Approve_DutyRoster,


		[Display(Name = "Exhibition RoadShow - Accept Participation")]
		AcceptParticipation_Exhibition_RoadShow,
		[Display(Name = "Exhibition RoadShow - Decline Participation")]
		DeclineParticipation_Exhibition_RoadShow,
		//[Display(Name = "Exhibition RoadShow - Send Invitation To Nominees")]
		//SendInvitationToNominees,

        //Payment
        [Display(Name = "Payment Pending GL")]
        Payment_Pending_GL,

        [Display(Name = "Payment Verify GL")]
        Payment_Verify_GL,

        [Display(Name = "Payment Pending Payment")]
        Payment_Pending_Payment,

        [Display(Name = "Payment Verify Payment")]
        Payment_Verify_Payment,

        [Display(Name = "Payment Submit Refund Request")]
        Payment_Submit_Refund_Request,

        [Display(Name = "Payment Verify Refund Request")]
        Payment_Verify_Refund_Request,

        [Display(Name = "Payment Approve Refund Request")]
        Payment_Approve_Refund_Request,

        [Display(Name = "Payment Pending Refund")]
        Payment_Pending_Refund,

        [Display(Name = "Payment Refund Incomplete")]
        Payment_Refund_Incomplete,

        [Display(Name = "Payment Refund Complete")]
        Payment_Refund_Complete,

        ////Approve External Request
        //[Display(Name = "Approve External Request Exhibition Participation")]
        //Approve_External_Request_Exhibition_Participation,
        //[Display(Name = "Approve External Request Duty Roster")]
        //Approve_External_Request_Duty_Roster,

        //Verify Courses
        [Display(Name = "Verify Courses Creation")]
        Verify_Courses_Creation,

        [Display(Name = "Verify Courses Published Change")]
        Verify_Courses_Published_Change,

        [Display(Name = "Verify Courses Published Withdraw")]
        Verify_Courses_Published_Withdraw,

        [Display(Name = "Verify Courses Participant Withdraw")]
        Verify_Courses_Participant_Withdraw,

        //Approve Courses
        [Display(Name = "Approve Courses Creation By First Approver")]
        Approve_Courses_Creation_Approver1,

        [Display(Name = "Approve Courses Creation By Second Approver")]
        Approve_Courses_Creation_Approver2,

        [Display(Name = "Approve Courses Creation By Third Approver")]
        Approve_Courses_Creation_Approver3,

        [Display(Name = "Approve Courses Published Change")]
        Approve_Courses_Published_Change,

        [Display(Name = "Approve Courses Published Withdraw")]
        Approve_Courses_Published_Withdraw,

        [Display(Name = "Approve Courses Participant Withdraw")]
        Approve_Courses_Participant_Withdraw,

        //Other Courses Action
        [Display(Name = "Course Is Approved")]
        Course_Approved,

        [Display(Name = "Course Requires Amendment")]
        Course_Amendment,

        [Display(Name = "Invitation To Enroll To Course")]
        Course_Invitation,

        // Course - Facilitators
        [Display(Name = "Assigned To Course")]
        Course_Assigned_To_Facilitator,

        [Display(Name = "Student Enroll in the Course")]
        Course_Student_Enrolled,

        //Survey
        [Display(Name = "Survey Created and Submitted for Verification")]
        Submit_Survey_Creation,

        [Display(Name = "Survey Cancelled")]
        Submit_Survey_Cancellation,

        [Display(Name = "Survey Published on FEP")]
        Submit_Survey_Publication,

        //Verify Survey
        [Display(Name = "Survey Verified and Submitted for Approval")]
        Verify_Survey_Creation,

        //Approve Survey
        [Display(Name = "Survey Approved by 1st-Level Approver")]
        Approve_Survey_Creation_1,

        [Display(Name = "Survey Approved by 2nd-Level Approver")]
        Approve_Survey_Creation_2,

        [Display(Name = "Survey Approved by 3rd-Level Approver")]
        Approve_Survey_Creation_3,

        [Display(Name = "Approve Survey Creation Final")]
        Approve_Survey_Creation_Final,

        //Survey Response
        [Display(Name = "Survey Invitation")]
        Submit_Survey_Distribution,

        [Display(Name = "Survey Response")]
        Submit_Survey_Response,

        //Publication
        [Display(Name = "Publication Created and Submitted for Verification")]
        Submit_Publication_Creation,

        [Display(Name = "Publication Cancelled")]
        Submit_Publication_Cancellation,

        [Display(Name = "Publication Published on FEP")]
        Submit_Publication_Publication,

        [Display(Name = "Publication Modified and Submitted for Verification")]
        Submit_Publication_Modification,

        [Display(Name = "Publication Withdrawal Submitted for Verification")]
        Submit_Publication_Withdrawal,

        [Display(Name = "Publication Modification Cancelled")]
        Submit_Publication_Modification_Cancellation,

        [Display(Name = "Publication Withdrawal Cancelled")]
        Submit_Publication_Withdrawal_Cancellation,

        //VerifyPublication
        [Display(Name = "Publication Verified and Submitted for Approval")]
        Verify_Publication_Creation,

        [Display(Name = "Publication Modification Verified and Submitted for Approval")]
        Verify_Publication_Modification,

        [Display(Name = "Publication Withdrawal Verified and Submitted for Approval")]
        Verify_Publication_Withdrawal,

        //Approve Publication
        [Display(Name = "Publication Approved by 1st-Level Approver")]
        Approve_Publication_Creation_1,

        [Display(Name = "Publication Approved by 2nd-Level Approver")]
        Approve_Publication_Creation_2,

        [Display(Name = "Publication Approved by 3rd-Level Approver")]
        Approve_Publication_Creation_3,

        [Display(Name = "Approve Publication Creation Final")]
        Approve_Publication_Creation_Final,

        [Display(Name = "Publication Modification Approved by 1st-Level Approver")]
        Approve_Publication_Modification_1,

        [Display(Name = "Publication Modification Approved by 2nd-Level Approver")]
        Approve_Publication_Modification_2,

        [Display(Name = "Publication Modification Approved by 3rd-Level Approver")]
        Approve_Publication_Modification_3,

        [Display(Name = "Approve Publication Modification Final")]
        Approve_Publication_Modification_Final,

        [Display(Name = "Publication Withdrawal Approved by 1st-Level Approver")]
        Approve_Publication_Withdrawal_1,

        [Display(Name = "Publication Withdrawal Approved by 2nd-Level Approver")]
        Approve_Publication_Withdrawal_2,

        [Display(Name = "Publication Withdrawal Approved by 3rd-Level Approver")]
        Approve_Publication_Withdrawal_3,

        [Display(Name = "Publication Withdrawal Notice")]
        Approve_Publication_Withdrawal_Final,

        //Refund publication
        [Display(Name = "Publication Refund Requested")]
        Submit_Publication_Refund,

        [Display(Name = "Publication Refund Incomplete")]
        Approve_Publication_Refund_Incomplete,

        [Display(Name = "Publication Refund Complete")]
        Approve_Publication_Refund_Complete,

        //---------------------------------------------------
    }

    [Table("Notification")]
    public class Notification
    {
        [Key]
        public long Id { get; set; }

        public NotificationType? NotificationType { get; set; }
        public int UserId { get; set; }
        public NotificationCategory Category { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    //[Table("NotificationToSend")]
    //public class NotificationToSend
    //{
    //	[Key]
    //	public long Id { get; set; }
    //	public string Message { get; set; }
    //	public string Link { get; set; }
    //	public DateTime CreatedDate { get; set; }
    //	public DateTime SendDate { get; set; }
    //	public bool IsSent { get; set; }
    //	public DateTime? SentDate { get; set; }

    //	public virtual ICollection<NotificationToSendRecipient> Recipient { get; set; }
    //}

    //[Table("NotificationToSendRecipient")]
    //public class NotificationToSendRecipient
    //{
    //	[Key]
    //	public long Id { get; set; }
    //	public long NotificationToSendId { get; set; }
    //	public int UserId { get; set; }

    //	[ForeignKey("NotificationToSendId")]
    //	public virtual NotificationToSend NotificationToSend { get; set; }

    //	[ForeignKey("UserId")]
    //	public virtual User User { get; set; }
    //}

    //[Table("NotificationSetting")]
    //public class NotificationSetting
    //{
    //	[Key]
    //	public NotificationType NotificationType { get; set; }
    //	public bool IsSendEmail { get; set; }
    //	public bool IsSendNotification { get; set; }
    //	public string NotificationMessage { get; set; }
    //	public string EmailSubject { get; set; }
    //	public string EmailMessage { get; set; }

    //}

    [Table("TabBulkSMS")]
    public class TabBulkSMS
    {
        [Key]
        public int DatID { get; set; }

        public int DatType { get; set; }
        public int DatNotify { get; set; }
        public DateTime? DTInsert { get; set; }
        public DateTime? DTSchedule { get; set; }
        public DateTime? DTExpired { get; set; }
        public string SMSTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    [Table("TabBulkEmail")]
    public class TabBulkEmail
    {
        [Key]
        public int DatID { get; set; }

        public int DatType { get; set; }
        public int DatNotify { get; set; }
        public DateTime? DTInsert { get; set; }
        public DateTime? DTSchedule { get; set; }
        public DateTime? DTExpired { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool AttachmentState { get; set; }
        public string Attachment_01 { get; set; }
        public string Attachment_02 { get; set; }
        public string Attachment_03 { get; set; }
    }
}