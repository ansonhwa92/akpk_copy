using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.PublicEvent
{
	public class ListPublicEventModel
	{
		public FilterPublicEventModel Filter { get; set; }
		public PublicEventModel List { get; set; }
	}

	public class FilterPublicEventModel : DataTableModel
	{

		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Display(Name = "Event Category")] 
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category")]
		public string EventCategoryName { get; set; }

		[Display(Name = "Targeted Group")]
		public EventTargetGroup? TargetedGroup { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? EndDate { get; set; }

		[Display(Name = "Status")]
		public EventStatus? EventStatus { get; set; }

		[Display(Name = "Objective")]
		public string EventObjective { get; set; }
	}

	public class PublicEventModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please Insert Event Title")]
		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Required(ErrorMessage = "Please Insert Objective")]
		[Display(Name = "Objective")]
		public string EventObjective { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? EndDate { get; set; }

		[Display(Name = "Event Venue")]
		public string Venue { get; set; }

		[Display(Name = "Event Fee")]
		public float? Fee { get; set; }

		[Display(Name = "Participant")]
		public int? ParticipantAllowed { get; set; }

		[Display(Name = "Targeted Group")]
		public EventTargetGroup? TargetedGroup { get; set; }

		[Display(Name = "External Exhibitor")]
		public int? ExternalExhibitorId { get; set; }

		[Display(Name = "External Exhibitor")]
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "Event Category")]
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category")] 
		public string EventCategoryName { get; set; }

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
		public EventStatus? EventStatus { get; set; }

		[Display(Name = "Reasons")]
		public string Reasons { get; set; }

		[Display(Name = "Remarks")]
		public string Remarks { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
