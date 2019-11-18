using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eEvent
{
	public class EventExternalExhibitorModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Please Insert Exhibitor Name")]
		[Display(Name = "ExhibitorName", ResourceType = typeof(Language.Event))]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "ExhibitorEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please Insert Phone No")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "ExhibitorPhoneNo", ResourceType = typeof(Language.Event))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public string PhoneNo { get; set; }

		[Required(ErrorMessage = "Please Insert Company Name")]
		[Display(Name = "ExhibitorCompanyName", ResourceType = typeof(Language.Event))]
		public string CompanyName { get; set; }

		[Display(Name = "ExhibitorRemark", ResourceType = typeof(Language.Event))]
		public string Remark { get; set; }
	}


	public class FilterEventExternalExhibitorModel : DataTableModel
	{
		[Required(ErrorMessage = "Please Insert Exhibitor Name")]
		[Display(Name = "ExhibitorName", ResourceType = typeof(Language.Event))]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "ExhibitorEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please Insert Phone No")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "ExhibitorPhoneNo", ResourceType = typeof(Language.Event))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public string PhoneNo { get; set; }

		[Display(Name = "ExhibitorRemark", ResourceType = typeof(Language.Event))]
		public string Remark { get; set; }
	}

	public class ListEventExternalExhibitorModel
	{
		public FilterEventExternalExhibitorModel Filter { get; set; }

		public EventExternalExhibitorModel List { get; set; }
	}

	public class DetailsEventExternalExhibitorModel : EventExternalExhibitorModel
	{
		public DetailsEventExternalExhibitorModel() { }
	}

	public class CreateEventExternalExhibitorModel
	{
		[Required(ErrorMessage = "Please Insert Exhibitor Name")]
		[Display(Name = "ExhibitorName", ResourceType = typeof(Language.Event))]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "ExhibitorEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please Insert Phone No")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "ExhibitorPhoneNo", ResourceType = typeof(Language.Event))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public string PhoneNo { get; set; }

		[Display(Name = "ExhibitorRemark", ResourceType = typeof(Language.Event))]
		public string Remark { get; set; }

		[Required(ErrorMessage = "Please Insert Company Name")]
		[Display(Name = "ExhibitorCompanyName", ResourceType = typeof(Language.Event))]
		public string CompanyName { get; set; }
	}

	public class EditEventExternalExhibitorModel : CreateEventExternalExhibitorModel
	{
		public EditEventExternalExhibitorModel() { }
		public int Id { get; set; }
	}

	public class DeleteEventExternalExhibitorModel : EventExternalExhibitorModel
	{
		public DeleteEventExternalExhibitorModel() { }
	}

}
