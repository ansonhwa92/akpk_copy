using FEP.Helper;
using FEP.WebApiModel.FileDocument;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.eEvent
{
	public class EventAgendaModel
	{
		public int Id { get; set; }

		[Display(Name = "AgendaTitle", ResourceType = typeof(Language.Event))]
		public string AgendaTitle { get; set; }

		[Display(Name = "AgendaDescription", ResourceType = typeof(Language.Event))]
		public string AgendaDescription { get; set; }

		[Display(Name = "AgendaTentative", ResourceType = typeof(Language.Event))]
		public string Tentative { get; set; }

		[UIHint("Time")]
		[DataType(DataType.Time)]
		[Display(Name = "AgendaTime", ResourceType = typeof(Language.Event))]
		public DateTime? Time { get; set; }

		[Display(Name = "AgendaEventRefNo", ResourceType = typeof(Language.Event))]
		public int? EventId { get; set; }
		[Display(Name = "AgendaEventRefNo", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		public IEnumerable<SelectListItem> EventIds { get; set; }

		[Display(Name = "AgendaPersonInCharge", ResourceType = typeof(Language.Event))]
		public int? PersonInChargeId { get; set; }
		[Display(Name = "AgendaPersonInCharge", ResourceType = typeof(Language.Event))]
		public string PersonInChargeName { get; set; }
		public IEnumerable<SelectListItem> PersonInchargeIds { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	public class FilterEventAgendaModel : DataTableModel
	{
		[Display(Name = "AgendaTitle", ResourceType = typeof(Language.Event))]
		public string AgendaTitle { get; set; }

		[Display(Name = "AgendaDescription", ResourceType = typeof(Language.Event))]
		public string AgendaDescription { get; set; }

		[Display(Name = "AgendaTentative", ResourceType = typeof(Language.Event))]
		public string Tentative { get; set; }

		[UIHint("Time")]
		[DataType(DataType.Time)]
		[Display(Name = "AgendaTime", ResourceType = typeof(Language.Event))]
		public DateTime? Time { get; set; }

		[Display(Name = "AgendaEventRefNo", ResourceType = typeof(Language.Event))]
		public int? EventId { get; set; }
		public IEnumerable<SelectListItem> EventIds { get; set; }

		[Display(Name = "AgendaPersonInCharge", ResourceType = typeof(Language.Event))]
		public int? PersonInChargeId { get; set; }
		public IEnumerable<SelectListItem> PersonInchargeIds { get; set; }
	}

	public class ListEventAgendaModel
	{
		public FilterEventAgendaModel Filter { get; set; }
		public EventAgendaModel List { get; set; }
	}

	public class DetailsEventAgendaModel : EventAgendaModel
	{
		public DetailsEventAgendaModel() { }

		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class CreateEventAgendaModel : EventAgendaModel
	{
		public CreateEventAgendaModel()
		{
			FilesId = new List<int>();
		}

		public List<int> FilesId { get; set; }
	}

	public class EditEventAgendaModel : CreateEventAgendaModel
	{
		public EditEventAgendaModel()
		{
			FilesId = new List<int>();
		}

		public IEnumerable<Attachment> Attachments { get; set; }

	}

	public class DeleteEventAgendaModel : DetailsEventAgendaModel
	{
		public DeleteEventAgendaModel() { }

	}

}
