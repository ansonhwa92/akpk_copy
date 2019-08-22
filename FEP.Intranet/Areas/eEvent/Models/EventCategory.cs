using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class EventCategoryModel
	{
		public EventCategoryModel() { }

		[Required(ErrorMessage = "Please Select Category")]
		[Display(Name = "Category")]
		public string CategoryName { get; set; }
	}

	public class CreateEventCategoryModel : EventCategoryModel
	{
		public CreateEventCategoryModel() { }
	}

	public class EditEventCategoryModel : EventCategoryModel
	{
		public EditEventCategoryModel() { }

		[Required]
		public int Id { get; set; }
	}

	public class DetailsEventCategoryModel : EventCategoryModel
	{
		public DetailsEventCategoryModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
	}

	public class DeleteEventCategoryModel : DetailsEventCategoryModel
	{
		public DeleteEventCategoryModel() { }
	}

	public class ListEventCategoryModel
	{
		public List<DetailsEventCategoryModel> CategoryList { get; set; }

		public FilterEventCategoryModel filter { get; set; }
		public ListEventCategoryModel() { }
		public ListEventCategoryModel(List<DetailsEventCategoryModel> ListCategory)
		{
			this.CategoryList = ListCategory;
		}
	}

	public class FilterEventCategoryModel
	{
		[Display(Name = "Category")]
		public string CategoryName { get; set; }
	}
}