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

namespace FEP.WebApiModel.PublicEvent
{
	public class ListPublicEventModel
	{
		public FilterPublicEventModel Filter { get; set; }
		public PublicEventModel List { get; set; }
	}

	public class FilterPublicEventModel : DataTableModel
	{

		[Display(Name = "PubEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

        [Display(Name = "PubEventRefNo", ResourceType = typeof(Language.Event))]
        public string RefNo { get; set; }

        [Display(Name = "PubEventCategory", ResourceType = typeof(Language.Event))]
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category", ResourceType = typeof(Language.Event))]
		public string EventCategoryName { get; set; }

		[Display(Name = "PubEventTargetedGroup", ResourceType = typeof(Language.Event))]
		public EventTargetGroup? TargetedGroup { get; set; }

		[Display(Name = "PubEventStartDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Display(Name = "PubEventEndDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }

		[Display(Name = "PubEventStatus", ResourceType = typeof(Language.Event))]
		public EventStatus? EventStatus { get; set; }

        public bool? RequireAction { get; set; }

        public UserAccess? UserAccess { get; set; }

    }

	public class PublicEventModel
	{
		public int Id { get; set; }

		[Display(Name = "PubEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

		[Display(Name = "PubEventObjective", ResourceType = typeof(Language.Event))]
		public string EventObjective { get; set; }

		[Display(Name = "PubEventStartDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Display(Name = "PubEventEndDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }

		[Display(Name = "PubEventVenue", ResourceType = typeof(Language.Event))]
		public string Venue { get; set; }

		//-----------individual------------//
		[Display(Name = "PubEventIndividualFree", ResourceType = typeof(Language.Event))]
		public bool FreeIndividual { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualFee { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualEarlyBird { get; set; }

		//-----individual w/ paper------------//
		[Display(Name = "PubEventIndividualPaperFree", ResourceType = typeof(Language.Event))]
		public bool FreeIndividualPaper { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualPaperFee { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualPaperEarlyBird { get; set; }

		//----individual w/ paper to present----//
		[Display(Name = "PubEventIndividualPresentFree", ResourceType = typeof(Language.Event))]
		public bool FreeIndividualPresent { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualPresentFee { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? IndividualPresentEarlyBird { get; set; }

		//--------------agency------------//
		[Display(Name = "PubEventAgencyFree", ResourceType = typeof(Language.Event))]
		public bool FreeAgency { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? AgencyFee { get; set; }

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
		public float? AgencyEarlyBird { get; set; }

		//[Display(Name = "PubEventFee", ResourceType = typeof(Language.Event))]
		//public float? Fee { get; set; }

		[Display(Name = "PubEventSeatAllocated_EarlyBird", ResourceType = typeof(Language.Event))]
		public int? SeatAllocated_EarlyBird { get; set; }

		[Display(Name = "PubEventParticipantAllowed", ResourceType = typeof(Language.Event))]
		public int? ParticipantAllowed { get; set; }

		[Display(Name = "PubEventTargetedGroup", ResourceType = typeof(Language.Event))]
		public EventTargetGroup? TargetedGroup { get; set; }

		[Display(Name = "PubEventExternalExhibitorId", ResourceType = typeof(Language.Event))]
		public int[] ExternalExhibitorId { get; set; }

		[Display(Name = "PubEventExternalExhibitorName", ResourceType = typeof(Language.Event))]
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "PubEventCategoryId", ResourceType = typeof(Language.Event))]
		public int? EventCategoryId { get; set; }

		[Display(Name = "PubEventCategoryName", ResourceType = typeof(Language.Event))]
		public string EventCategoryName { get; set; }

		[Display(Name = "PubEventStatus", ResourceType = typeof(Language.Event))]
		public EventStatus? EventStatus { get; set; }

		[Display(Name = "PubEventStatusDesc", ResourceType = typeof(Language.Event))]
		public string EventStatusDesc { get; set; }

		[Display(Name = "PubEventRemarks", ResourceType = typeof(Language.Event))]
		public string Remarks { get; set; }

		[Display(Name = "PubEventSpeakerId", ResourceType = typeof(Language.Event))]
		public int[] SpeakerId { get; set; }

		[Display(Name = "PubEventSpeakerName", ResourceType = typeof(Language.Event))]
		public string SpeakerName { get; set; }
		public string origin { get; set; }
		[Display(Name = "PubEventRefNo", ResourceType = typeof(Language.Event))]
		public string RefNo { get; set; }
		public int? SLAReminderStatusId { get; set; }
		public bool? IsRequested { get; set; }

		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }


		public IEnumerable<SelectListItem> CategoryList { get; set; }
		public IEnumerable<SelectListItem> SpeakerList { get; set; }
		public IEnumerable<SelectListItem> ExternalExhibitorList { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }


		[Display(Name = "PubEventLabelParticipantType", ResourceType = typeof(Language.Event))]
		public string LabelParticipantType { get; set; }
		[Display(Name = "PubEventLabelFree", ResourceType = typeof(Language.Event))]
		public string LabelFree { get; set; }
		[Display(Name = "PubEventLabelNormalFee", ResourceType = typeof(Language.Event))]
		public string LabelNormalFee { get; set; }
		[Display(Name = "PubEventLabelEarlyBirdFee", ResourceType = typeof(Language.Event))]
		public string LabelEarlyBirdFee { get; set; }

		[Display(Name = "PubEventLabelIndividual", ResourceType = typeof(Language.Event))]
		public string LabelIndividual { get; set; }
		[Display(Name = "PubEventLabelIndividualPaper", ResourceType = typeof(Language.Event))]
		public string LabelIndividualPaper { get; set; }
		[Display(Name = "PubEventLabelIndividualPresent", ResourceType = typeof(Language.Event))]
		public string LabelIndividualPresent { get; set; }
		[Display(Name = "PubEventLabelAgency", ResourceType = typeof(Language.Event))]
		public string LabelAgency { get; set; }

        public string tentativeScript { get; set; }

		[Display(Name = "PubEventPictures", ResourceType = typeof(Language.Event))]
		public string CoverPicture { get; set; }

		[Display(Name = "PubEventSpeakerPictures", ResourceType = typeof(Language.Event))]
		public string SpeakerPicture { get; set; }

	}


	public class DetailsPublicEventModel : PublicEventModel
	{
		public DetailsPublicEventModel() { }
		//public PublicEventApprovalModel approval { get; set; }
	}


	public class CreatePublicEventModel : PublicEventModel
	{
		public CreatePublicEventModel() 
        {
            FilesId = new List<int>();
        }

        public List<int> FilesId { get; set; }

	}


	public class EditPublicEventModel : CreatePublicEventModel
	{
		public EditPublicEventModel() 
        {
            FilesId = new List<int>();
        }
    }


	public class DeletePublicEventModel : DetailsPublicEventModel
	{
		public DeletePublicEventModel() { }
	}

	public class PublicEventApprovalModel
	{
		public PublicEventModel publicevent { get; set; }
		public ApprovalModel approval { get; set; }
	}

	public class ApprovalModel //Share dengan cancel/modi
	{
		[Required]
		public int? Id { get; set; }

		[Required]
		public int? EventId { get; set; }

		[Required]
		[Display(Name = "Level")]
		public EventApprovalLevel Level { get; set; }

		[Required]
		public int? ApproverId { get; set; }

		[Required]
		[Range((int)(EventApprovalStatus.Approved), (int)(EventApprovalStatus.Rejected), ErrorMessage = "Please Select")]
		[Display(Name = "PubApprovalStatus")]
		public EventApprovalStatus Status { get; set; }

		[Required]
		[Display(Name = "Remarks")]
		public string Remarks { get; set; }

		[Display(Name = "Require Next")]
		public bool RequireNext { get; set; }
	}


	public class PublicEventApprovalHistoryModel //Share dengan cancel/modi
	{
		public EventApprovalLevel Level { get; set; }

		public int? ApproverId { get; set; }

		public string UserName { get; set; }

		public EventApprovalStatus Status { get; set; }

		public DateTime? ApprovalDate { get; set; }

		public string Remarks { get; set; }
	}

	public class EventRequestApprovalModel
	{
		public EventRequestModel eventrequest { get; set; }
		public ApprovalModel approval { get; set; }
	}

	public class EventRequestModel
	{ 
		public EventRequestModel() 
		{
			FilesId = new List<int>();
		}

		public List<int> FilesId { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }


		//Request information to display
		public int? Id { get; set; }

		[Display(Name = "ReqReason", ResourceType = typeof(Language.Event))]
		public string Reason { get; set; }

		[Display(Name = "ReqType", ResourceType = typeof(Language.Event))]
		public RequestType? RequestType { get; set; }

		[Display(Name = "ReqStatus", ResourceType = typeof(Language.Event))]
		public RequestStatus? RequestStatus { get; set; }

		//Event information to display
		public int? EventId { get; set; }

		[Display(Name = "ReqEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

		[Display(Name = "ReqEventRefNo", ResourceType = typeof(Language.Event))]
		public string EventRefNo { get; set; }

		[Display(Name = "ReqEventCategory", ResourceType = typeof(Language.Event))]
		public string EventCategory { get; set; }

		[Display(Name = "ReqEventObjective", ResourceType = typeof(Language.Event))]
		public string EventObjective { get; set; }
		public int? SLAReminderStatusId { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}
}
