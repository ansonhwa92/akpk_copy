using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class EventSpeakerModel
	{
		public EventSpeakerModel() { }

		[Display(Name = "Remarks")]
		public string Remark { get; set; }

		[Display(Name = "Staff Name")]
		public int? UserId { get; set; }

		[Display(Name = "Staff Name")]
		public string UserName { get; set; }

		[Display(Name = "Speaker Type")]
		public SpeakerType? SpeakerType { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Assigned Date")]
		public DateTime DateAssigned { get; set; }
	}

	public class CreateEventSpeakerModel : EventSpeakerModel
	{
		public CreateEventSpeakerModel() { }
	}

	public class EditEventSpeakerModel : EventSpeakerModel
	{
		public EditEventSpeakerModel() { }

		[Required]
		public int Id { get; set; }
	}

	public class DetailsEventSpeakerModel : EventSpeakerModel
	{
		public DetailsEventSpeakerModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public class DeleteEventSpeakerModel : DetailsEventSpeakerModel
	{
		public DeleteEventSpeakerModel() { }
	}

	public class ListEventSpeakerModel
	{
		public ListEventSpeakerModel() { }
		public FilterEventSpeakerModel filter { get; set; }
		public List<DetailsEventSpeakerModel> SpeakerList { get; set; }
		public ListEventSpeakerModel(List<DetailsEventSpeakerModel> ListSpeaker)
		{
			this.SpeakerList = ListSpeaker;
		}
	}

	public class FilterEventSpeakerModel
	{
		[Display(Name = "Remarks")]
		public string Remark { get; set; }

		[Display(Name = "Staff Name")]
		public int? UserId { get; set; }

		[Display(Name = "Staff Name")]
		public string UserName { get; set; }

		[Display(Name = "Speaker Type")]
		public SpeakerType? SpeakerType { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Assigned Date")]
		public DateTime DateAssigned { get; set; }
	}
}
