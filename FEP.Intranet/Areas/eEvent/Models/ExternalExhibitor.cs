using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class ExternalExhibitorModel
	{
		public string Name { get; set; }
	}

	public class CreateExternalExhibitorModel : ExternalExhibitorModel
	{
		public CreateExternalExhibitorModel() { }
	}

	public class EditExternalExhibitorModel : ExternalExhibitorModel
	{
		public EditExternalExhibitorModel() { }
		[Key]
		public int Id { get; set; }
		
	}

	public class DetailsExternalExhibitorModel : ExternalExhibitorModel
	{
		public DetailsExternalExhibitorModel() { }
		[Key]
		public int Id { get; set; }
		
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	public class DeleteExternalExhibitorModel : DetailsExternalExhibitorModel
	{
		public DeleteExternalExhibitorModel() { }
	}

	public class ListExternalExhibitorModel
	{
		public ListExternalExhibitorModel() { }
		public FilterExternalExhibitorModel filter { get; set; }
		public List<DetailsExternalExhibitorModel> ExternalExhibitorList { get; set; }
		public ListExternalExhibitorModel(List<DetailsExternalExhibitorModel> ListExternalExhibitor)
		{
			this.ExternalExhibitorList = ListExternalExhibitor;
		}
	}

	public class FilterExternalExhibitorModel
	{
		public string Name { get; set; }
	}
}