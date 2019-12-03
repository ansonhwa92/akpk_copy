using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.eEvent
{
	public class EventSpeakerModel
	{
		public int Id { get; set; }
		[Display(Name = "SpType", ResourceType = typeof(Language.Event))]
		public SpeakerType? SpeakerType { get; set; }

		[Display(Name = "SpTypeDesc", ResourceType = typeof(Language.Event))]
		public string SpeakerTypeDesc { get; set; }

		[Display(Name = "SpStatus", ResourceType = typeof(Language.Event))]
		public SpeakerStatus? SpeakerStatus { get; set; }

		[Display(Name = "SpStatusDesc", ResourceType = typeof(Language.Event))]
		public string SpeakerStatusDesc { get; set; }

		[Display(Name = "SpExperience", ResourceType = typeof(Language.Event))]
		public string Experience { get; set; }

		[Display(Name = "SpFieldThumbnail", ResourceType = typeof(Language.Event))]
		public string ThumbnailUrl { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "SpEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[DataType(DataType.PhoneNumber)]
		[Display(Name = "SpPhoneNo", ResourceType = typeof(Language.Event))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public string PhoneNo { get; set; }

		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		public IEnumerable<SelectListItem> UserIds { get; set; }

	}

	public class FilterEventSpeakerModel : DataTableModel
	{
		[Display(Name = "SpType", ResourceType = typeof(Language.Event))]
		public SpeakerType? SpeakerType { get; set; }

		[Display(Name = "SpStatus", ResourceType = typeof(Language.Event))]
		public SpeakerStatus? SpeakerStatus { get; set; }

		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		public IEnumerable<SelectListItem> UserIds { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string ExternalUserName { get; set; }

		[Display(Name = "SpEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Display(Name = "SpPhoneNo", ResourceType = typeof(Language.Event))]
		public string PhoneNo { get; set; }
	}

	public class ListEventSpeakerModel
	{
		public FilterEventSpeakerModel Filter { get; set; }

		public EventSpeakerModel List { get; set; }
	}

	public class DetailsEventSpeakerModel : EventSpeakerModel
	{
		public DetailsEventSpeakerModel() { }	
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class CreateEventSpeakerModel
	{
		public CreateEventSpeakerModel()
		{
			FilesId = new List<int>();
		}

		public List<int> FilesId { get; set; }

		[Display(Name = "SpType", ResourceType = typeof(Language.Event))]
		public SpeakerType? SpeakerType { get; set; }

		[Display(Name = "SpExperience", ResourceType = typeof(Language.Event))]
		public string Experience { get; set; }

		[Display(Name = "SpStatus", ResourceType = typeof(Language.Event))]
		public SpeakerStatus? SpeakerStatus { get; set; }

		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		public IEnumerable<SelectListItem> UserIds { get; set; }

		public string ThumbnailUrl { get; set; }
	}

	public class EditEventSpeakerModel : CreateEventSpeakerModel
	{
		public EditEventSpeakerModel()
		{
			FilesId = new List<int>();
		}

		public int Id { get; set; }

		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class DeleteEventSpeakerModel : DetailsEventSpeakerModel
	{
		public DeleteEventSpeakerModel() { }

	}
}
