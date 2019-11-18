using FEP.Intranet.Models;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class PublicEventModel
	{
		public PublicEventModel() { }
		[Required(ErrorMessage = "Please Insert Event Title")]
		[Display(Name = "PubEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

		[Required(ErrorMessage = "Please Insert Event Objective")]
		[Display(Name = "PubEventObjective", ResourceType = typeof(Language.Event))]
		public string EventObjective { get; set; }

		[Required(ErrorMessage = "Please Insert Event Venue")]
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


		//[Required(ErrorMessage = "Please Insert Event Fee")]
		//[Display(Name = "Event Fee (RM) per Person")]
		//public float? Fee { get; set; }

		[Required(ErrorMessage = "Please Insert No of Participant")]
		[RegularExpression("([1-9][0-9]*)")]
		[Display(Name = "PubEventParticipantAllowed", ResourceType = typeof(Language.Event))]
		public int? ParticipantAllowed { get; set; }

		[Required(ErrorMessage = "Please Select Targeted Group")]
		[Display(Name = "Targeted Group")]
		public EventTargetGroup? TargetedGroup { get; set; }

		[Required(ErrorMessage = "Please Select Event Status")]
		[Display(Name = "Status")]
		public EventStatus? EventStatus { get; set; }

		[Required(ErrorMessage = "Please Select Event Category")]
		[Display(Name = "Event Category")]
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category")]
		public string EventCategoryName { get; set; }

		[Display(Name = "Event Category")]
		public IEnumerable<SelectListItem> CategoryList { get; set; }

		[Display(Name = "Reasons")]
		public string Reasons { get; set; }

		[Display(Name = "Remarks")]
		public string Remarks { get; set; }

		[Required(ErrorMessage = "Please Select Event Speaker")]
		[Display(Name = "Speakers")]
		public int[] SpeakerId { get; set; }

		[Display(Name = "Speaker")]
		public string SpeakerName { get; set; }

		[Display(Name = "Speaker")]
		public IEnumerable<SelectListItem> SpeakerList { get; set; }

		[Required(ErrorMessage = "Please Select Event External Exhibitor")]
		[Display(Name = "External Exhibitors")] 
		public int[] ExternalExhibitorId { get; set; }

		[Display(Name = "External Exhibitor")]
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "External Exhibitor")]
		public IEnumerable<SelectListItem> ExternalExhibitorList { get; set; }

		[Display(Name = "Proof of Approval")]
		public string GetFileName { get; set; }

		public string origin { get; set; }
		public string RefNo { get; set; }



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
	}

	public class CreatePublicEventModel : PublicEventModel
	{
		public CreatePublicEventModel() 
        {
            Attachments = new List<Attachment>();
            AttachmentFiles = new List<HttpPostedFileBase>();
        }
        		
        [Required]
        [Display(Name = "Proof of Approval")]
        public IEnumerable<Attachment> Attachments { get; set; }

        public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

		[Required(ErrorMessage = "Please Insert Start Date")]
		[Display(Name = "Start Date")]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please Insert End Date")]
		[Display(Name = "End Date")]
		public DateTime? EndDate { get; set; }
	}

	public class EditPublicEventModel : PublicEventModel
	{
        public EditPublicEventModel()
        {
            Attachments = new List<Attachment>();
            AttachmentFiles = new List<HttpPostedFileBase>();
        }

        [Required]
		public int Id { get; set; }

        [Required]
        [Display(Name = "Proof of Approval")]
        public IEnumerable<Attachment> Attachments { get; set; }

        public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

		[Required(ErrorMessage = "Please Insert Start Date")]
		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please Insert End Date")]
		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }
	}

	public class DetailsPublicEventModel : PublicEventModel
	{
		public DetailsPublicEventModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }

		[Display(Name = "Created Date")]
		public DateTime? CreatedDate { get; set; }

        [Display(Name = "Proof of Approval")]
        public IEnumerable<Attachment> Attachments { get; set; }

		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }
	}

	public class DeletePublicEventModel : DetailsPublicEventModel
	{
		public DeletePublicEventModel() { }
	}

	public class ListPublicEventModel
	{
		public List<DetailsPublicEventModel> PublicEventList { get; set; }

		public FilterPublicEventModel filter { get; set; }
		public ListPublicEventModel() { }
		public ListPublicEventModel(List<DetailsPublicEventModel> ListPublicEvent)
		{
			this.PublicEventList = ListPublicEvent;
		}
	}

	public class FilterPublicEventModel
	{
		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Display(Name = "RefNo")]
		public string RefNo { get; set; }


		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; }

		[Display(Name = "Event Venue")]
		public string Venue { get; set; }

		[Display(Name = "Event Fee")]
		public decimal? Fee { get; set; }

		[Display(Name = "Event Category")]
		public int? EventCategoryId { get; set; }

		[Display(Name = "Event Category")]
		public string EventCategoryName { get; set; }

		[Display(Name = "Status")]
		public EventStatus? EventStatus { get; set; }
	}

	public class EventRequestModel
	{
		public EventRequestModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
		}

		[Required]
		[Display(Name = "Proof of Approval")]
		public IEnumerable<Attachment> Attachments { get; set; } 

		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }



		public int? Id { get; set; }

		[Required]
		[Display(Name = "ReqReason", ResourceType = typeof(Language.Event))]
		public string Reason { get; set; }

		[Required]
		[Display(Name = "ReqType", ResourceType = typeof(Language.Event))]
		public RequestType? RequestType { get; set; }

		[Display(Name = "ReqStatus", ResourceType = typeof(Language.Event))]
		public RequestStatus? RequestStatus { get; set; }

		public int? EventId { get; set; }

		[Display(Name = "ReqEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

		[Display(Name = "ReqEventRefNo", ResourceType = typeof(Language.Event))]
		public string EventRefNo { get; set; }

		[Display(Name = "ReqEventCategory", ResourceType = typeof(Language.Event))]
		public string EventCategory { get; set; }

		[Display(Name = "ReqEventObjective", ResourceType = typeof(Language.Event))]
		public string EventObjective { get; set; }

		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}
}