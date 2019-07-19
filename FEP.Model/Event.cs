using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
	[Table("Event")]
	public class Event
	{
		[Key]

		public int Id { get; set; }

		public string EventTitle { get; set; }

		public string EventObjective { get; set; }

		public DateTime Date { get; set; }

		public string Venue { get; set; }

		public decimal? Fee { get; set; }

		public int? ParticipantAllowed { get; set; }

		public int? TargetedGroup { get; set; }

		public string ExternalExhibitor { get; set; }

		public int? SpeakerId { get; set; }

		[ForeignKey("SpeakerId")]
		public virtual Speaker Speaker { get; set; }

		public int? ApprovalId1 { get; set; }

		public int? ApprovalId2 { get; set; }

		public int? ApprovalId3 { get; set; }

		public int? ApprovalId4 { get; set; }

		[ForeignKey("ApprovalId1")]
		public virtual CourseApproval Approval1 { get; set; }

		[ForeignKey("ApprovalId2")]
		public virtual CourseApproval Approval2 { get; set; }

		[ForeignKey("ApprovalId3")]
		public virtual CourseApproval Approval3 { get; set; }

		[ForeignKey("ApprovalId4")]
		public virtual CourseApproval Approval4 { get; set; }

		public EventStatus EventStatus { get; set; }

		public int? AgendaId { get; set; }

		[ForeignKey("AgendaId")]
		public virtual Agenda Agenda { get; set; }

		public EventCategory EventCategory { get; set; }

		public string Remarks { get; set; }
	}

	public enum EventCategory
	{
		Workshop, Seminar, Dialogue, Conference, Symposium, Convention
	}

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
		public virtual Event Event { get; set; }
	}

	public class Agenda
	{
		[Key]
		public int Id { get; set; }

		public string AgendaTitle { get; set; }

		public DateTime Time { get; set; }

		public string PersonInCharge { get; set; }

		[ForeignKey("PersonInCharge")]
		public virtual User User { get; set; }

		public string Remark { get; set; }
	}

	public enum EventStatus
	{
		New,
		Pending,
		Approval,
		Approved,
		Cancelled
	}

	[Table("Speaker")]
	public class Speaker
	{
		[Key]
		public int Id { get; set; }

		public string SpeakerName { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public string Remark { get; set; }
	}

	[Table("EventBooking")]
	public class EventBooking
	{
		[Key]
		public int Id { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }

		public decimal Price { get; set; }

		public int? SeatAvailable { get; set; }

		public decimal Total { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public Ticket Tiket { get; set; }
	}

	public enum Ticket
	{
		Individual = 0,
		IndividualWithPaper = 1,
		Group = 3,
		Agency = 4
	}

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
		public virtual Event Event { get; set; }
	}

	public class EventInterviewRequest
	{
		[Key]
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public DateTime Time { get; set; }

		public string Location { get; set; }

		public string Language { get; set; }

		public string Topic { get; set; }

		public string Name { get; set; }

		public string Designation { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }
	}

	public class EventAttendance {
		[Key]
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }

		public string Remark { get; set; }
	}

	public class ManuscriptSubmission {
		[Key]
		public int Id { get; set; }

		public string FileName { get; set; }

		public DateTime DateUploaded { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }
	}

	public class ParticipantFeedback {
		[Key]
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public string FeedbackDescription { get; set; }

		public int? UserId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Event Event { get; set; }
	}
}
