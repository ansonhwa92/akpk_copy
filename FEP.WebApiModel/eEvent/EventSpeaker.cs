using FEP.Helper;
using FEP.Model;
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

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "SpDateAssigned", ResourceType = typeof(Language.Event))]
		public DateTime? DateAssigned { get; set; }

		[Display(Name = "SpExperience", ResourceType = typeof(Language.Event))]
		public string Experience { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "SpEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Display(Name = "SpRemark", ResourceType = typeof(Language.Event))]
		public string Remark { get; set; }

		[DataType(DataType.PhoneNumber)]
		[Display(Name = "SpPhoneNo", ResourceType = typeof(Language.Event))]
		public int? PhoneNo { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "SpDateOfBirth", ResourceType = typeof(Language.Event))]
		public DateTime? DateOfBirth { get; set; }

		[Display(Name = "SpAddress1", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }

		[Display(Name = "SpAddress2", ResourceType = typeof(Language.Event))]
		public string AddressStreet2 { get; set; }

		[Display(Name = "SpPoscode", ResourceType = typeof(Language.Event))]
		public string AddressPoscode { get; set; }

		[Display(Name = "SpCity", ResourceType = typeof(Language.Event))]
		public string AddressCity { get; set; }

		[Display(Name = "SpState", ResourceType = typeof(Language.Event))]
		public MediaState? State { get; set; }

		[Display(Name = "SpMaritialStatus", ResourceType = typeof(Language.Event))]
		public MaritialStatus? MaritialStatus { get; set; }

		[Display(Name = "SpReligion", ResourceType = typeof(Language.Event))]
		public Religion? Religion { get; set; }

		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }
		public IEnumerable<SelectListItem> UserIds { get; set; }

		//[Display(Name = "SpPicture", ResourceType = typeof(Language.Event))]
		//public HttpPostedFileBase SpeakerPicture { get; set; }

		//[Display(Name = "SpPicture", ResourceType = typeof(Language.Event))]
		//public string SpeakerPictureName { get; set; }

		//[Display(Name = "SpAttachment", ResourceType = typeof(Language.Event))]
		//public HttpPostedFileBase SpeakerAttachment { get; set; }

		//[Display(Name = "SpAttachment", ResourceType = typeof(Language.Event))]
		//public string SpeakerAttachmentName { get; set; }

	}

	public class FilterEventSpeakerModel : DataTableModel
	{
		[Display(Name = "SpType", ResourceType = typeof(Language.Event))]
		public SpeakerType? SpeakerType { get; set; }

		[Display(Name = "SpDateAssigned", ResourceType = typeof(Language.Event))]
		public DateTime? DateAssigned { get; set; }

		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		public IEnumerable<SelectListItem> UserIds { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		[Display(Name = "SpEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }
	}

	public class ListEventSpeakerModel
	{
		public FilterEventSpeakerModel Filter { get; set; }

		public EventSpeakerModel List { get; set; }
	}

	public class DetailsEventSpeakerModel : EventSpeakerModel
	{
		public DetailsEventSpeakerModel() { }

		public int Id { get; set; }
	}

	public class CreateEventSpeakerModel
	{
		[Required(ErrorMessage = "Please Select Speaker Type")]
		[Display(Name = "SpType", ResourceType = typeof(Language.Event))]
		public SpeakerType? SpeakerType { get; set; }

		[Required(ErrorMessage = "Please Insert Date Assigned")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "SpDateAssigned", ResourceType = typeof(Language.Event))]
		public DateTime? DateAssigned { get; set; }

		[Display(Name = "SpExperience", ResourceType = typeof(Language.Event))]
		public string Experience { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "SpEmail", ResourceType = typeof(Language.Event))]
		public string Email { get; set; }

		[Display(Name = "SpRemark", ResourceType = typeof(Language.Event))]
		public string Remark { get; set; }

		[DataType(DataType.PhoneNumber)]
		[Display(Name = "SpPhoneNo", ResourceType = typeof(Language.Event))]
		public int? PhoneNo { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "SpDateOfBirth", ResourceType = typeof(Language.Event))]
		public DateTime? DateOfBirth { get; set; }

		[Display(Name = "SpAddress1", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }

		[Display(Name = "SpAddress2", ResourceType = typeof(Language.Event))]
		public string AddressStreet2 { get; set; }

		[Display(Name = "SpPoscode", ResourceType = typeof(Language.Event))]
		public string AddressPoscode { get; set; }

		[Display(Name = "SpCity", ResourceType = typeof(Language.Event))]
		public string AddressCity { get; set; }

		[Display(Name = "SpState", ResourceType = typeof(Language.Event))]
		public MediaState? State { get; set; }

		[Display(Name = "SpMaritialStatus", ResourceType = typeof(Language.Event))]
		public MaritialStatus? MaritialStatus { get; set; }

		[Display(Name = "SpReligion", ResourceType = typeof(Language.Event))]
		public Religion? Religion { get; set; }

		[Required(ErrorMessage = "Please Select User")]
		[Display(Name = "SpUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "SpUserName", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		public IEnumerable<SelectListItem> UserIds { get; set; }


		//[Display(Name = "SpPictureName", ResourceType = typeof(Language.Event))]
		//public string SpeakerPictureName { get; set; }

		//[Display(Name = "SpAttachmentName", ResourceType = typeof(Language.Event))]
		//public string SpeakerAttachmentName { get; set; }
	}

	public class EditEventSpeakerModel : CreateEventSpeakerModel
	{
		public EditEventSpeakerModel() { }

		public int Id { get; set; }
	}

	public class DeleteEventSpeakerModel : EventSpeakerModel
	{
		public DeleteEventSpeakerModel() { }

	}
}
