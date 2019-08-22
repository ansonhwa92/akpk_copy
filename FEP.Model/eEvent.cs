using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
	[Table("PublicEvent")]
	public class PublicEvent
	{
		[Key]
		public int Id { get; set; }
		public string EventTitle { get; set; }
		public string EventObjective { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Venue { get; set; }
		public float? Fee { get; set; }
		public int? ParticipantAllowed { get; set; }
		public EventTargetGroup? TargetedGroup { get; set; }

		public int? ApprovalId1 { get; set; }
		public int? ApprovalId2 { get; set; }
		public int? ApprovalId3 { get; set; }
		public int? ApprovalId4 { get; set; }

		public EventStatus? EventStatus { get; set; }
		public string Reasons { get; set; } //Modification and Cancellation

		public int? EventCategoryId { get; set; }
		[ForeignKey("EventCategoryId")]
		public virtual EventCategory EventCategory { get; set; }


		[ForeignKey("ApprovalId1")]
		public virtual EventApproval Approval1 { get; set; }
		[ForeignKey("ApprovalId2")]
		public virtual EventApproval Approval2 { get; set; }
		[ForeignKey("ApprovalId3")]
		public virtual EventApproval Approval3 { get; set; }
		[ForeignKey("ApprovalId4")]
		public virtual EventApproval Approval4 { get; set; }
		public string Remarks { get; set; }
		
		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }

		public virtual ICollection<EventVerifier> EventVerifier { get; set; }
		public virtual ICollection<EventFile> EventFiles { get; set; }
		public virtual ICollection<EventAgenda> EventAgendas { get; set; }
		public virtual ICollection<EventSpeaker> EventSpeakers { get; set; }
		public virtual ICollection<EventExternalExhibitor> EventExternalExhibitors { get; set; }
		public virtual ICollection<EventObjective> EventObjectives { get; set; } 
	}

	[Table("EventExternalExhibitor")]
	public class 
		EventExternalExhibitor
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventObjective")]
	public class EventObjective
	{
		[Key]
		public int Id { get; set; }
		public string ObjectiveTitle { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventCalendar")]
	public class EventCalendar
	{
		[Key]
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int? TotalDay { get; set; }
		public string Remark { get; set; }
		public int? EventBookingId { get; set; }
		[ForeignKey("EventBookingId")]
		public virtual EventBooking EventBooking { get; set; }
		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	[Table("EventAgenda")]
	public class EventAgenda
	{
		[Key]
		public int Id { get; set; }
		public string AgendaTitle { get; set; }
		public DateTime Time { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		public int? PersonInCharge { get; set; }
		[ForeignKey("PersonInCharge")]
		public virtual User User { get; set; }

		public string Remark { get; set; }
		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventSpeaker")]
	public class EventSpeaker
	{
		[Key]
		public int Id { get; set; }
		public string Remark { get; set; }
		public SpeakerType? SpeakerType { get; set; }
		public DateTime DateAssigned { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventBooking")]
	public class EventBooking
	{
		[Key]
		public int Id { get; set; }
		public float Price { get; set; }
		public int? SeatAvailable { get; set; }
		public float Total { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		public Ticket Tiket { get; set; }
		public BookingStatus BookingStatus { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("InvitationEvent")]
	public class InvitationEvent
	{
		[Key]
		public int Id { get; set; }
		public string MediaName { get; set; }
		public string MediaType { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventMediaInterviewRequest")]
	public class EventMediaInterviewRequest
	{
		[Key]
		public int Id { get; set; }
		public string MediaName { get; set; }
		public string MediaType { get; set; }
		public string ContactPerson { get; set; }
		public int ContactNo { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public DateTime Date { get; set; }
		public DateTime Time { get; set; }
		public string Location { get; set; }
		public string Language { get; set; }
		public string Topic { get; set; }
		public string Designation { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventAttendance")]
	public class EventAttendance
	{
		[Key]
		public int Id { get; set; }
		public DateTime Date { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		public string Remark { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("ManuscriptSubmission")]
	public class ManuscriptSubmission
	{
		[Key]
		public int Id { get; set; }
		public string FileName { get; set; }
		public DateTime DateUploaded { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("ParticipantFeedback")]
	public class ParticipantFeedback
	{
		[Key]
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string FeedbackDescription { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventMember")]
	public class EventMember
	{
		[Key]
		public int Id { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventApproval")]
	public class EventApproval
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string Remark { get; set; }
		public ApprovalType ApprovalType { get; set; }

		public int ApproverId { get; set; }
		[ForeignKey("ApproverId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	[Table("EventVerifier")]
	public class EventVerifier
	{
		[Key]
		public int Id { get; set; }
		public DateTime? VerifiedDate { get; set; }
		public string Remark { get; set; }
		public VerifyType VerifyType { get; set; }

		public int VerifierId { get; set; }
		[ForeignKey("VerifierId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	//[Table("EventCancellation")]
	//public class EventCancellation
	//{
	//	[Key]
	//	public int Id { get; set; }
	//	public int? UserId { get; set; }
	//	[ForeignKey("UserId")]
	//	public virtual User User { get; set; }

	//	public int? EventId { get; set; }
	//	[ForeignKey("EventId")]
	//	public virtual PublicEvent Event { get; set; }

	//	public string Reasons { get; set; }
	//	public int? ApprovalId1 { get; set; }
	//	public int? ApprovalId2 { get; set; }
	//	public int? ApprovalId3 { get; set; }
	//	public int? ApprovalId4 { get; set; }

	//	[ForeignKey("ApprovalId1")]
	//	public virtual EventApproval Approval1 { get; set; }
	//	[ForeignKey("ApprovalId2")]
	//	public virtual EventApproval Approval2 { get; set; }
	//	[ForeignKey("ApprovalId3")]
	//	public virtual EventApproval Approval3 { get; set; }
	//	[ForeignKey("ApprovalId4")]
	//	public virtual EventApproval Approval4 { get; set; }

	//	public int? VerifyId { get; set; }
	//	[ForeignKey("VerifyId")]
	//	public virtual EventVerifier Verifier { get; set; }

	//	[ForeignKey("CreatedBy")]
	//	public virtual User CreatedByUser { get; set; }
	//	public int? CreatedBy { get; set; }
	//	public DateTime? CreatedDate { get; set; }
	//	public bool Display { get; set; }
	//}

	[Table("EventExhibitionRequest")]
	public class EventExhibitionRequest
	{
		[Key]
		public int Id { get; set; }
		public int? ReceivedBy { get; set; }
		public DateTime ReceivedDate { get; set; }
		public string Receive_Via { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventCategory")]
	public class EventCategory
	{
		[Key]
		public int Id { get; set; }
		public string CategoryName { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}
}
