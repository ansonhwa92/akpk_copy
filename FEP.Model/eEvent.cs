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
		public string RefNo { get; set; }
		public string EventTitle { get; set; }
		public string EventObjective { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Venue { get; set; }
		public float? Fee { get; set; }
		public int? ParticipantAllowed { get; set; }
		public EventTargetGroup? TargetedGroup { get; set; }
		public string Remarks { get; set; }
		public EventStatus? EventStatus { get; set; }
		public int? EventCategoryId { get; set; }
		//public int? SpeakerId { get; set; }
		//public int? ExternalExhibitorId { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
		public int? SLAReminderStatusId { get; set; }

		//----ForeignKey----
		[ForeignKey("EventCategoryId")]
		public virtual EventCategory EventCategory { get; set; }
		//[ForeignKey("SpeakerId")]
		//public virtual EventSpeaker EventSpeaker { get; set; }
		//[ForeignKey("ExternalExhibitorId")]
		//public virtual EventExternalExhibitor ExternalExhibitor { get; set; }
		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
	

		public virtual ICollection<EventFile> EventFiles { get; set; }
		public virtual ICollection<EventAgenda> EventAgendas { get; set; }
	}

	[Table("EventExternalExhibitor")]
	public class EventExternalExhibitor
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public string Email { get; set; }

		public string PhoneNo { get; set; }

		public string Remark { get; set; }

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
		public SpeakerType? SpeakerType { get; set; }
		public DateTime? DateAssigned { get; set; }
		public string Experience { get; set; }
		public string Email { get; set; }
		public string Remark { get; set; }
		public int? PhoneNo { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }
		public MaritialStatus? MaritialStatus { get; set; }
		public Religion? Religion { get; set; }
		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }

		public virtual ICollection<SpeakerFile> SpeakerFiles { get; set; }
	}

	[Table("AssignedSpeaker")]
	public class AssignedSpeaker
	{
		[Key]
		public int Id { get; set; }
		public int PublicEventId { get; set; }
		[ForeignKey("PublicEventId")] 
		public virtual PublicEvent PublicEvent { get; set; }

		public int EventSpeakerId { get; set; }
		[ForeignKey("EventSpeakerId")] 
		public virtual EventSpeaker EventSpeaker { get; set; }
	}

	[Table("AssignedExternalExhibitor")]
	public class AssignedExternalExhibitor
	{
		[Key]
		public int Id { get; set; }
		public int ExternalExhibitorId { get; set; }
		[ForeignKey("ExternalExhibitorId")] 
		public virtual EventExternalExhibitor EventExternalExhibitor { get; set; }

		public int PublicEventId { get; set; }
		[ForeignKey("PublicEventId")]
		public virtual PublicEvent PublicEvent { get; set; }
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
		public string RefNo { get; set; }
		public string MediaName { get; set; }
		public MediaType? MediaType { get; set; }
		public string ContactPerson { get; set; }
		public int? ContactNo { get; set; }
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }
		public string Email { get; set; }
		public DateTime? DateStart { get; set; }
		public DateTime? DateEnd { get; set; }
		public DateTime? Time { get; set; }
		public string Location { get; set; }
		public MediaLanguage? Language { get; set; }
		public string Topic { get; set; }
		//public string Designation { get; set; }
		public MediaStatus? MediaStatus { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }

		public virtual ICollection<MediaFile> EventMediaFiles { get; set; }
		//public virtual ICollection<User> Representatives { get; set; }
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
		public bool RequireNext { get; set; }

		public int ApproverId { get; set; }
		[ForeignKey("ApproverId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	
	[Table("EventExhibitionRequest")]
	public class EventExhibitionRequest
	{
		[Key]
		public int Id { get; set; }
		public string RefNo { get; set; }
		public string EventName { get; set; }
		public string Organiser { get; set; }
		public string Location { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int? ParticipationRequirement { get; set; }
		public ExhibitionStatus? ExhibitionStatus { get; set; }

		public int? NomineeId { get; set; }
		[ForeignKey("NomineeId")]
		public virtual User Nominee { get; set; }

		public int? ReceivedById { get; set; }
		[ForeignKey("ReceivedById")]
		public virtual User ReceivedBy { get; set; }
		public DateTime? ReceivedDate { get; set; }
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

	[Table("ExhibitionNominee")]
	public class ExhibitionNominee
	{
		[Key]
		public int Id { get; set; }
		public int ExhibitionRoadshowId { get; set; }

		[ForeignKey("ExhibitionRoadshowId")]
		public virtual EventExhibitionRequest ExhibitionRequest { get; set; }

		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

	}
}
