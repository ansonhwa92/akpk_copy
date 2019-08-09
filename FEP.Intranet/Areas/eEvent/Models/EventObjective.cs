using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class EventObjectiveModel
	{
		public EventObjectiveModel() { }
		public string ObjectiveTitle { get; set; }
	}

	public class CreateEventObjectiveModel : EventObjectiveModel
	{
		public CreateEventObjectiveModel() { }
	}

	public class EditEventObjectiveModel : EventObjectiveModel
	{
		public EditEventObjectiveModel() { }

		[Required]
		public int Id { get; set; }
	}

	public class DetailsEventObjectiveModel : EventObjectiveModel
	{
		public DetailsEventObjectiveModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
	}

	public class DeleteEventObjectiveModel : DetailsEventObjectiveModel
	{
		public DeleteEventObjectiveModel() { }
	}

	public class ListEventObjectiveModel
	{
		public ListEventObjectiveModel() { }
		public FilterEventObjectiveModel filter{ get; set; }
		public List<DetailsEventObjectiveModel> ObjectiveList { get; set; }
		public ListEventObjectiveModel(List<DetailsEventObjectiveModel> ListObejective)
		{
			this.ObjectiveList = ListObejective;
		}
	}

	public class FilterEventObjectiveModel
	{
		public string ObjectiveTitle { get; set; }
	}
}