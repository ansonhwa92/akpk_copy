using FEP.WebApiModel.FileDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class EventAgendaModel
	{
		public EventAgendaModel() { }
		public int Id { get; set; }

		[Required(ErrorMessage ="Please insert Session")]
		[Display(Name = "AgendaTitle", ResourceType = typeof(Language.Event))]
		public string AgendaTitle { get; set; }

		[Required(ErrorMessage = "Please insert Description")]
		[Display(Name = "AgendaDescription", ResourceType = typeof(Language.Event))]
		public string AgendaDescription { get; set; }

		[Required(ErrorMessage = "Please insert Tentative")]
		[Display(Name = "AgendaTentative", ResourceType = typeof(Language.Event))]
		public string Tentative { get; set; }

		[Required(ErrorMessage = "Please insert Time")]
		[UIHint("Time")]
		[DataType(DataType.Time)]
		[Display(Name = "AgendaTime", ResourceType = typeof(Language.Event))]
		public DateTime? Time { get; set; }

		[Required(ErrorMessage = "Please select Public Event")]
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

	public class CreateEventAgendaModel : EventAgendaModel
	{
		public CreateEventAgendaModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
		}

		public IEnumerable<Attachment> Attachments { get; set; }
		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }
	}

	public class EditEventAgendaModel : EventAgendaModel
	{
		public EditEventAgendaModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
		}

		[Required(ErrorMessage = "")]
		[Display(Name = "AgendaSupportDocument", ResourceType = typeof(Language.Event))]
		public IEnumerable<Attachment> Attachments { get; set; }
		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }
	}

	public class DetailsEventAgendaModel : EventAgendaModel
	{
		public DetailsEventAgendaModel() { }

		[Display(Name = "AgendaSupportDocument", ResourceType = typeof(Language.Event))]
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class DeleteEventAgendaModel : DetailsEventAgendaModel
	{
		public DeleteEventAgendaModel() { }
	}
}