using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class PublicEventModel
	{
		public PublicEventModel() { }
		[Required(ErrorMessage = "Please Insert Event Title")]
		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Required(ErrorMessage = "")]
		[Display(Name = "Objective")]
		public int? EventObjectiveId { get; set; }

		[Display(Name = "Objective")]
		public string EventObjectiveTitle { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Event Date")]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Event Date")]
		public DateTime EndDate { get; set; }

		[Display(Name = "Event Venue")]
		public string Venue { get; set; }

		[Display(Name = "Event Fee")]
		public float? Fee { get; set; }

		[Display(Name = "Participant")]
		public int? ParticipantAllowed { get; set; }

		[Display(Name = "")]
		public EventTargetGroup TargetedGroup { get; set; }

		[Display(Name = "External Exhibitor")]
		public int? ExternalExhibitorId { get; set; }

		[Display(Name = "External Exhibitor")]
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId1 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName1 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId2 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName2 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId3 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName3 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId4 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName4 { get; set; }

		[Display(Name = "Status")]
		public EventStatus EventStatus { get; set; }

		[Display(Name = "Agenda Name")]
		public string AgendaName { get; set; }

		[Display(Name = "Category")]
		public EventCategory EventCategory { get; set; }

		[Display(Name = "Remarks")]
		public string Remarks { get; set; }
	}

	public class CreatePublicEventModel : PublicEventModel
	{
		public CreatePublicEventModel() { }
	}

	public class EditPublicEventModel : PublicEventModel
	{
		[Required]
		public int Id { get; set; }
	}

	public class DetailsPublicEventModel : PublicEventModel
	{
		public DetailsPublicEventModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
	}

	public class DeletePublicEventModel : DetailsPublicEventModel
	{
		public DeletePublicEventModel() { }
	}

	public class ListPublicEventModel
	{
		public List<DetailsPublicEventModel> PublicEventList { get; set; }

		public FilterPublicEventModel filter { get; set; }
		public ListPublicEventModel() { }
		public ListPublicEventModel(List<DetailsPublicEventModel> ListPublicEvent)
		{
			this.PublicEventList = ListPublicEvent;
		}
	}

	public class FilterPublicEventModel
	{
		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Display(Name = "Objective")]
		public string EventObjective { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Event Date")]
		public DateTime Date { get; set; }

		[Display(Name = "Event Venue")]
		public string Venue { get; set; }

		[Display(Name = "Event Fee")]
		public decimal? Fee { get; set; }
	}
}