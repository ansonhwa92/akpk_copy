﻿using FEP.Helper;
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
	public class ExhibitionRoadshowRequestModel
	{
		public int Id { get; set; }
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }
		[Display(Name = "ExRoadOrganiserEmail", ResourceType = typeof(Language.Event))]
		public string OrganiserEmail { get; set; }
		//[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		//public string Location { get; set; }


		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }


		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "ExRoadStartTime", ResourceType = typeof(Language.Event))]
		public DateTime? StartTime { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "ExRoadEndTime", ResourceType = typeof(Language.Event))]
		public DateTime? EndTime { get; set; }
		[Display(Name = "ExRoadParticipantRequirement", ResourceType = typeof(Language.Event))]
		public int? ParticipationRequirement { get; set; }
		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }
		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }
		[Display(Name = "ExRoadReceivedById", ResourceType = typeof(Language.Event))]
		public int? ReceivedById { get; set; }
		[Display(Name = "ExRoadReceivedByName", ResourceType = typeof(Language.Event))]
		public string ReceivedByName { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadReceivedDate", ResourceType = typeof(Language.Event))]
		public DateTime? ReceivedDate { get; set; }
		[Display(Name = "ExRoadReceive_Via", ResourceType = typeof(Language.Event))]
		public string Receive_Via { get; set; }
		public IEnumerable<SelectListItem> ReceivedBys { get; set; }

		[Display(Name = "ExRoadNomineeId", ResourceType = typeof(Language.Event))]
		public int[] NomineeId { get; set; }
		[Display(Name = "ExRoadNomineeName", ResourceType = typeof(Language.Event))]
		public string NomineeName { get; set; }
		public IEnumerable<SelectListItem> Nominees { get; set; }

		[Display(Name = "ExRoadRefNo", ResourceType = typeof(Language.Event))]
		public string RefNo { get; set; }
		public int? SLAReminderStatusId { get; set; }
		[Display(Name = "ExRoadExhibitionSupDoc", ResourceType = typeof(Language.Event))]
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class FilterExhibitionRoadshowRequestModel : DataTableModel
	{
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }
		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string Location { get; set; }
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }
		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }
		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }

	}

	public class ListExhibitionRoadshowRequestModel
	{
		public FilterExhibitionRoadshowRequestModel Filter { get; set; }

		public ExhibitionRoadshowRequestModel List { get; set; }
	}

	public class DetailsExhibitionRoadshowRequestModel : ExhibitionRoadshowRequestModel
	{
		public DetailsExhibitionRoadshowRequestModel() { }

		public int Id { get; set; }
	}

	public class CreateExhibitionRoadshowRequestModel
	{
		public CreateExhibitionRoadshowRequestModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
			FilesId = new List<int>();
		}
		public List<int> FilesId { get; set; }

		[Required]
		[Display(Name = "ExRoadExhibitionSupDoc", ResourceType = typeof(Language.Event))]
		public IEnumerable<Attachment> Attachments { get; set; }

		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

		[Required(ErrorMessage = "Please insert Event Name")]
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }

		[Required(ErrorMessage = "Please insert Organiser")]
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "ExRoadOrganiserEmail", ResourceType = typeof(Language.Event))]
		public string OrganiserEmail { get; set; }

		//[Required(ErrorMessage = "Please insert Location")]
		//[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		//public string Location { get; set; }

		[Required(ErrorMessage = "Please insert Location")]
		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }
		[Required(ErrorMessage = "Please Insert Address")]
		public string AddressStreet2 { get; set; }
		[Required(ErrorMessage = "Please Insert Poscode")]
		public string AddressPoscode { get; set; }
		[Required(ErrorMessage = "Please Insert City")]
		public string AddressCity { get; set; }
		[Required(ErrorMessage = "Please Select State")]
		public MediaState? State { get; set; }

		[Required(ErrorMessage = "Please insert Start Date")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please insert End Date")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }

		[Required(ErrorMessage = "Please insert Start Time")]
		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "ExRoadStartTime", ResourceType = typeof(Language.Event))]
		public DateTime? StartTime { get; set; }

		[Required(ErrorMessage = "Please insert End Time")]
		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "ExRoadEndTime", ResourceType = typeof(Language.Event))]
		public DateTime? EndTime { get; set; }

		[Required(ErrorMessage = "Please insert Participant Requirement")]
		[Display(Name = "ExRoadParticipantRequirement", ResourceType = typeof(Language.Event))]
		public int? ParticipationRequirement { get; set; }

		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }

		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }

		[Required(ErrorMessage = "Please insert Receive By")]
		[Display(Name = "ExRoadReceivedById", ResourceType = typeof(Language.Event))]
		public int? ReceivedById { get; set; }

		[Display(Name = "ExRoadReceivedByName", ResourceType = typeof(Language.Event))]
		public string ReceivedByName { get; set; }

		[Required(ErrorMessage = "Please insert Receive Date")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "ExRoadReceivedDate", ResourceType = typeof(Language.Event))]
		public DateTime? ReceivedDate { get; set; }

		[Required(ErrorMessage = "Please insert Receive Via")]
		[Display(Name = "ExRoadReceive_Via", ResourceType = typeof(Language.Event))]
		public string Receive_Via { get; set; }

		[Display(Name = "ExRoadReceivedBys", ResourceType = typeof(Language.Event))]
		public IEnumerable<SelectListItem> ReceivedBys { get; set; }

		[Required(ErrorMessage = "Please select Nominees")]
		[Display(Name = "ExRoadNomineeId", ResourceType = typeof(Language.Event))]
		public int[] NomineeId { get; set; }

		[Display(Name = "ExRoadNomineeName", ResourceType = typeof(Language.Event))]
		public string NomineeName { get; set; }

		public IEnumerable<SelectListItem> Nominees { get; set; }
	}

	public class EditExhibitionRoadshowRequestModel : CreateExhibitionRoadshowRequestModel
	{
		public EditExhibitionRoadshowRequestModel() { }

		public int Id { get; set; }
	}

	public class DeleteExhibitionRoadshowRequestModel : ExhibitionRoadshowRequestModel
	{
		public DeleteExhibitionRoadshowRequestModel() { }
	}

}


