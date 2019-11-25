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
		//-----------individual------------//
		public bool FreeIndividual { get; set; }
		public float? IndividualFee { get; set; }
		public float? IndividualEarlyBird { get; set; }
		//-----individual w/ paper------------//
		public bool FreeIndividualPaper { get; set; }
		public float? IndividualPaperFee { get; set; }
		public float? IndividualPaperEarlyBird { get; set; } 

		//----individual w/ paper to present----//
		public bool FreeIndividualPresent { get; set; }
		public float? IndividualPresentFee { get; set; }
		public float? IndividualPresentEarlyBird { get; set; }

		//--------------agency------------//
		public bool FreeAgency { get; set; }
		public float? AgencyFee { get; set; }
		public float? AgencyEarlyBird { get; set; } 


		public int? ParticipantAllowed { get; set; }
		public EventTargetGroup? TargetedGroup { get; set; }
		public string Remarks { get; set; }
		public EventStatus? EventStatus { get; set; }
		public int? EventCategoryId { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
		public int? SLAReminderStatusId { get; set; }
		public bool? IsRequested { get; set; }

		//----ForeignKey----
		[ForeignKey("EventCategoryId")]
		public virtual EventCategory EventCategory { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }

		public virtual ICollection<EventAgenda> EventAgendas { get; set; }
	}

	[Table("EventExternalExhibitor")]
	public class EventExternalExhibitor
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public string Email { get; set; }

		public string CompanyName { get; set; }

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

	[Table("AgendaScript")]
	public class AgendaScript
	{
		[Key]
		public int Id { get; set; }
		public string TentativeScript { get; set; }
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
		public string AgendaDescription { get; set; }
		public string Tentative { get; set; }
		public DateTime? Time { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		public int? PersonInChargeId { get; set; }
		[ForeignKey("PersonInChargeId")]
		public virtual User User { get; set; }

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
		public SpeakerStatus? SpeakerStatus { get; set; }
		public string Experience { get; set; }
		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
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
		public string ContactNo { get; set; }
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

		public int? BranchId { get; set; }
		[ForeignKey("BranchId")]
		public virtual Branch Branch { get; set; }

		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
		public int? SLAReminderStatusId { get; set; }
	}


	[Table("EventAttendance")]
	public class EventAttendance
	{
		[Key]
		public int Id { get; set; }
		public string AttendeeName { get; set; }
		public ParticipantType? UserType { get; set; }
		public string CompanyName { get; set; }
		public string BookingNumber { get; set; }
		public string ICNo { get; set; }
		public CheckInStatus? CheckInStatus { get; set; }



		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }



		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }


		[ForeignKey("CreatedBy")]
		public virtual User CreatedByUser { get; set; }
		public int? CreatedBy { get; set; }
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

	[Table("PublicEventApproval")]
	public class PublicEventApproval
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string Remark { get; set; }
		public EventApprovalLevel ApprovalLevel { get; set; }
		public bool RequireNext { get; set; }
		public EventApprovalStatus Status { get; set; }
		public int? ApproverId { get; set; }
		

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	[Table("EventMediaInterviewApproval")]
	public class EventMediaInterviewApproval
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string Remark { get; set; }
		public EventApprovalLevel Level { get; set; }
		public EventApprovalStatus Status { get; set; }
		public bool RequireNext { get; set; }
		public int? ApproverId { get; set; }
	
		public int? MediaId { get; set; }
		[ForeignKey("MediaId")]
		public virtual EventMediaInterviewRequest MediaInterview { get; set; }
	}


	[Table("EventExhibitionRequestApproval")]
	public class EventExhibitionRequestApproval
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string Remark { get; set; }
		public EventApprovalLevel Level { get; set; }
		public EventApprovalStatus Status { get; set; }
		public bool RequireNext { get; set; }
		public int? ApproverId { get; set; }

		public int? ExhibitionId { get; set; }
		[ForeignKey("ExhibitionId")]
		public virtual EventExhibitionRequest EventExhibitionRequest { get; set; }
	}


	[Table("EventExhibitionRequest")]
	public class EventExhibitionRequest
	{
		[Key]
		public int Id { get; set; }
		public string RefNo { get; set; }
		public string EventName { get; set; }
		public string Organiser { get; set; }
		public string OrganiserEmail { get; set; }
		public string OrganiserPhoneNo { get; set; }
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public int? ParticipationRequirement { get; set; }
		public ExhibitionStatus? ExhibitionStatus { get; set; }

		public int? BranchId { get; set; }
		[ForeignKey("BranchId")]
		public virtual Branch Branch { get; set; }

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
		public int? SLAReminderStatusId { get; set; }

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

	[Table("DutyRoster")]
	public class DutyRoster
	{
		[Key]
		public int Id { get; set; }
        public int ExhibitionRoadshowId { get; set; }

        [ForeignKey("ExhibitionRoadshowId")]
        public virtual EventExhibitionRequest ExhibitionRequest { get; set; }


        public DateTime? Date { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("DutyRosterOfficer")]
	public class DutyRosterOfficer
	{
		[Key]
		public int Id { get; set; }
		public int DutyRosterId { get; set; }
		public int? UserId { get; set; }

		[ForeignKey("DutyRosterId")]
		public virtual DutyRoster DutyRoster { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}


	[Table("EventRequest")]
	public class EventRequest
	{
		[Key]
		public int Id { get; set; }

		public string Reason { get; set; } 

		public RequestStatus? RequestStatus { get; set; }
		public RequestType? RequestType { get; set; }
		public int? SLAReminderStatusId { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	[Table("EventRequestApproval")]
	public class EventRequestApproval
	{
		[Key]
		public int Id { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string Remark { get; set; }
		public EventApprovalLevel Level { get; set; }
		public EventApprovalStatus Status { get; set; }
		public bool RequireNext { get; set; }
		public int? ApproverId { get; set; }
		 
		public int? EventRequestId { get; set; }
		[ForeignKey("EventRequestId")]
		public virtual EventRequest EventRequest { get; set; }
	}


}