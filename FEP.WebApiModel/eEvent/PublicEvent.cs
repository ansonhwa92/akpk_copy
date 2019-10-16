using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "PubEventStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "PubEventEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }

		[Display(Name = "PubEventStatus", ResourceType = typeof(Language.Event))]
		public EventStatus? EventStatus { get; set; }

	}

	public class PublicEventModel
	{
		public int Id { get; set; }

		[Display(Name = "PubEventTitle", ResourceType = typeof(Language.Event))]
		public string EventTitle { get; set; }

		[Display(Name = "PubEventObjective", ResourceType = typeof(Language.Event))]
		public string EventObjective { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "PubEventStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "PubEventEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }

		[Display(Name = "PubEventVenue", ResourceType = typeof(Language.Event))]
		public string Venue { get; set; }

		[Display(Name = "PubEventFee", ResourceType = typeof(Language.Event))]
		public float? Fee { get; set; }

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





		public IEnumerable<SelectListItem> CategoryList { get; set; }
		public IEnumerable<SelectListItem> SpeakerList { get; set; }
		public IEnumerable<SelectListItem> ExternalExhibitorList { get; set; }



	}


	public class DetailsPublicEventModel : PublicEventModel
	{
		public DetailsPublicEventModel() { }

        public IEnumerable<Attachment> Attachments { get; set; }
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
        
        public IEnumerable<Attachment> Attachments { get; set; }

    }


	public class DeletePublicEventModel : DetailsPublicEventModel
	{
		public DeletePublicEventModel() { }

	}
}
