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

		//---------------------------------------------------
		//tajul tambah for SLA

		//Verify Public Event 
		[Display(Name = "Verify Public Event Creation")]
		Verify_Public_Event_Creation,
		[Display(Name = "Verify Public Event Published Changed")]
		Verify_Public_Event_Published_Changed,
		[Display(Name = "Verify Public Event Published Cancelled")]
		Verify_Public_Event_Published_Cancelled,

		//Approve Public Event
		[Display(Name = "Approve Public Event Creation 1")]
		Approve_Public_Event_Creation1,
		[Display(Name = "Approve Public Event Creation 2")]
		Approve_Public_Event_Creation2,
		[Display(Name = "Approve Public Event Creation 3")]
		Approve_Public_Event_Creation3,
		[Display(Name = "Approve Public Event Creation 4")]
		Approve_Public_Event_Creation4,
		[Display(Name = "Approve Public Event Published Changed")]
		Approve_Public_Event_Published_Changed,
		[Display(Name = "Approve Public Event Published Cancelled")]
		Approve_Public_Event_Published_Cancelled,

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
		[Display(Name = "Verify External Request Media Interview")]
		Verify_External_Request_Media_Interview,
		[Display(Name = "Verify External RequestExhibition ESS")]
		Verify_External_Request_Exhibition_ESS,
		[Display(Name = "Verify External RequestDuty Roster")]
		Verify_External_Request_Duty_Roster,

		//Approve External Request
		[Display(Name = "Approve External Request Media Interview")]
		Approve_External_Request_Media_Interview,
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

		//Verify Survey
		[Display(Name = "Verify Survey Creation")]
		Verify_Survey_Creation,
		[Display(Name = "Verify Survey Published Cancelled")]
		Verify_Survey_Published_Cancelled,

		//Approve Survey
		[Display(Name = "Approve Survey Creation")]
		Approve_Survey_Creation,
		[Display(Name = "Approve Survey Published Cancelled")]
		Approve_Survey_Published_Cancelled,

		//VerifyPublication
		[Display(Name = "Verify Publication Creation")]
		Verify_Publication_Creation,
		[Display(Name = "Verify Publication Published Change")]
		Verify_Publication_Published_Change,
		[Display(Name = "Verify Publication Published Withdraw")]
		Verify_Publication_Published_Withdraw,

		//Approve Publication
		[Display(Name = "Approve Publication Creation")]
		Approve_Publication_Creation,
		[Display(Name = "Approve Publication Published Change")]
		Approve_Publication_Published_Change,
		[Display(Name = "Approve Publication Published Withdraw")]
		Approve_Publication_Published_Withdraw,

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
