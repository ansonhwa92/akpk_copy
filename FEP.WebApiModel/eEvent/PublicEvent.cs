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

		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

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
		public string Venue { get; set; }
		public float? Fee { get; set; }
		public int? ParticipantAllowed { get; set; }
		public EventTargetGroup? TargetedGroup { get; set; }
		public int? ExternalExhibitorId { get; set; }
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "Event Category")]
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category")]
		public string EventCategoryName { get; set; }

		[Display(Name = "Status")]
		public EventStatus? EventStatus { get; set; }

		[Display(Name = "Status")]
		public string EventStatusDesc { get; set; } 

		public string Reasons { get; set; }
		public string Remarks { get; set; }
		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
