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
		[Display(Name = "Public Event - Rejected")]
		Reject_Public_Event,
		[Display(Name = "Public Event - Published")]
		Publish_Public_Event,

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

		[Display(Name = "Media Interview - Rejected")]
		Reject_Media_Interview,


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

		[Display(Name = "Exhibition RoadShow - Rejected")]
		Reject_Exhibition_RoadShow,

		//Payment
		[Display(Name = "Payment Pending GL")]
		Payment_Pending_GL,
		[Display(Name = "Payment Pending Payment")]
		Payment_Pending_Payment,
		[Display(Name = "Payment Verify GL")]
		Payment_Verify_GL,
		[Display(Name = "Payment Verify Payment")]
		Payment_Verify_Payment,
		[Display(Name = "Payment Verify Refund Request")]
		Payment_Verify_Refund_Request,
		[Display(Name = "Payment Approve Refund Request")]
		Payment_Approve_Refund_Request,
		[Display(Name = "Payment Pending Refund")]
		Payment_Pending_Refund,

		//Verify External Request

		[Display(Name = "Verify External RequestExhibition ESS")]
		Verify_External_Request_Exhibition_ESS,
		[Display(Name = "Verify External RequestDuty Roster")]
		Verify_External_Request_Duty_Roster,

		//Approve External Request

		[Display(Name = "Approve External Request Exhibition Participation")]
		Approve_External_Request_Exhibition_Participation,
		[Display(Name = "Approve External Request Duty Roster")]
		Approve_External_Request_Duty_Roster,

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
		[Display(Name = "Approve Courses Creation")]
		Approve_Courses_Creation,
		[Display(Name = "Approve Courses Published Change")]
		Approve_Courses_Published_Change,
		[Display(Name = "Approve Courses Published Withdraw")]
		Approve_Courses_Published_Withdraw,
		[Display(Name = "Approve Courses Participant Withdraw")]
		Approve_Courses_Participant_Withdraw,

		//Survey
		[Display(Name = "Submit Survey Creation")]
		Submit_Survey_Creation,
		[Display(Name = "Submit Survey Cancellation")]
		Submit_Survey_Cancellation,
		[Display(Name = "Submit Survey Publication")]
		Submit_Survey_Publication,

		//Verify Survey
		[Display(Name = "Verify Survey Creation")]
		Verify_Survey_Creation,

		//Approve Survey
		[Display(Name = "Approve Survey Creation 1")]
		Approve_Survey_Creation_1,
		[Display(Name = "Approve Survey Creation 2")]
		Approve_Survey_Creation_2,
		[Display(Name = "Approve Survey Creation 3")]
		Approve_Survey_Creation_3,
		[Display(Name = "Approve Survey Creation Final")]
		Approve_Survey_Creation_Final,

        //Survey Response
        [Display(Name = "Submit Survey Distribution")]
        Submit_Survey_Distribution,
        [Display(Name = "Submit Survey Response")]
        Submit_Survey_Response,

        //Publication
        [Display(Name = "Submit Publication Creation")]
        Submit_Publication_Creation,
        [Display(Name = "Submit Publication Cancellation")]
        Submit_Publication_Cancellation,
        [Display(Name = "Submit Publication Publication")]
        Submit_Publication_Publication,
        [Display(Name = "Submit Publication Modification")]
        Submit_Publication_Modification,
        [Display(Name = "Submit Publication Withdrawal")]
        Submit_Publication_Withdrawal,
        [Display(Name = "Submit Publication Modification Cancellation")]
        Submit_Publication_Modification_Cancellation,
        [Display(Name = "Submit Publication Withdrawal Cancellation")]
        Submit_Publication_Withdrawal_Cancellation,

		//VerifyPublication
		[Display(Name = "Verify Publication Creation")]
		Verify_Publication_Creation,
		[Display(Name = "Verify Publication Modification")]
		Verify_Publication_Modification,
		[Display(Name = "Verify Publication Withdrawal")]
		Verify_Publication_Withdrawal,

		//Approve Publication
		[Display(Name = "Approve Publication Creation 1")]
		Approve_Publication_Creation_1,
		[Display(Name = "Approve Publication Creation 2")]
		Approve_Publication_Creation_2,
		[Display(Name = "Approve Publication Creation 3")]
		Approve_Publication_Creation_3,
		[Display(Name = "Approve Publication Creation Final")]
		Approve_Publication_Creation_Final,
		[Display(Name = "Approve Publication Modification 1")]
		Approve_Publication_Modification_1,
		[Display(Name = "Approve Publication Modification 2")]
		Approve_Publication_Modification_2,
		[Display(Name = "Approve Publication Modification 3")]
		Approve_Publication_Modification_3,
		[Display(Name = "Approve Publication Modification Final")]
		Approve_Publication_Modification_Final,
		[Display(Name = "Approve Publication Withdrawal 1")]
		Approve_Publication_Withdrawal_1,
		[Display(Name = "Approve Publication Withdrawal 2")]
		Approve_Publication_Withdrawal_2,
		[Display(Name = "Approve Publication Withdrawal 3")]
		Approve_Publication_Withdrawal_3,
		[Display(Name = "Approve Publication Withdrawal Final")]
		Approve_Publication_Withdrawal_Final,


		//---------------------------------------------------
	}

	[Table("Notification")]
	public class Notification
	{
		[Key]
		public long Id { get; set; }
		public NotificationType NotificationType { get; set; }
		public int UserId { get; set; }
		public string Message { get; set; }
		public string Link { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsRead { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}

	[Table("NotificationToSend")]
	public class NotificationToSend
	{
		[Key]
		public long Id { get; set; }
		public string Message { get; set; }
		public string Link { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime SendDate { get; set; }
		public bool IsSent { get; set; }
		public DateTime? SentDate { get; set; }

		public virtual ICollection<NotificationToSendRecipient> Recipient { get; set; }
	}

	[Table("NotificationToSendRecipient")]
	public class NotificationToSendRecipient
	{
		[Key]
		public long Id { get; set; }
		public long NotificationToSendId { get; set; }
		public int UserId { get; set; }

		[ForeignKey("NotificationToSendId")]
		public virtual NotificationToSend NotificationToSend { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}

	[Table("NotificationSetting")]
	public class NotificationSetting
	{
		[Key]
		public NotificationType NotificationType { get; set; }
		public bool IsSendEmail { get; set; }
		public bool IsSendNotification { get; set; }
		public string NotificationMessage { get; set; }
		public string EmailSubject { get; set; }
		public string EmailMessage { get; set; }

	}

	[Table("TabBulkSMS")]
	public class TabBulkSMS
	{
		[Key]
		public int DatID { get; set; }
		public int DatType { get; set; }
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
		public DateTime? DTInsert { get; set; }
		public DateTime? DTSchedule { get; set; }
		public DateTime? DTExpired { get; set; }
		public string SMSTo { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool AttachmentState { get; set; }
		public string Attachment_01 { get; set; }
		public string Attachment_02 { get; set; }
		public string Attachment_03 { get; set; }
	}
}
