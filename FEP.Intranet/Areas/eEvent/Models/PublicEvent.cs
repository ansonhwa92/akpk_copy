using FEP.Model;
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
		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Required(ErrorMessage = "Please Insert Event Objective")]
		[Display(Name = "Event Objective")]
		public string EventObjective { get; set; }

		[Required(ErrorMessage = "Please Insert Start Date")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "Start Date")]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please Insert End Date")]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
		[Display(Name = "End Date")]
		public DateTime? EndDate { get; set; }

		[Required(ErrorMessage = "Please Insert Event Venue")]
		[Display(Name = "Event Venue")]
		public string Venue { get; set; }

		[Required(ErrorMessage = "Please Insert Event Fee")]
		[Display(Name = "Event Fee (RM) per Person")]
		public float? Fee { get; set; }

		[Display(Name = "No. of Participant")]
		[RegularExpression("([1-9][0-9]*)")]
		public int? ParticipantAllowed { get; set; }

		[Display(Name = "Targeted Group")]
		public EventTargetGroup? TargetedGroup { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId1 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName1 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId2 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName2 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId3 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName3 { get; set; }

		[Display(Name = "Approver Name")]
		public int? ApprovalId4 { get; set; }

		[Display(Name = "Approver Name")]
		public string ApprovalName4 { get; set; }

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

		[Display(Name = "Speaker")]
		public int[] SpeakerId { get; set; }

		[Display(Name = "Speaker")]
		public string SpeakerName { get; set; }

		[Display(Name = "Speaker")]
		public IEnumerable<SelectListItem> SpeakerList { get; set; }

		[Display(Name = "External Exhibitor")] 
		public int[] ExternalExhibitorId { get; set; }

		[Display(Name = "External Exhibitor")]
		public string ExternalExhibitorName { get; set; }

		[Display(Name = "External Exhibitor")]
		public IEnumerable<SelectListItem> ExternalExhibitorList { get; set; }

		[Display(Name = "Proof of Approval")]
		public string GetFileName { get; set; }

		public string origin { get; set; }
		 
		public string RefNo { get; set; }

		[Display(Name = "Total Days")]
		public string TotalDay { get; set; }


		//File
		
		[Display(Name = "File Name")]
		public string FileName { get; set; }
		[Display(Name = "File Description")]
		public string FileDescription { get; set; }
		[Display(Name = "Uploaded Date")]
		public DateTime UploadedDate { get; set; }
	}

	public class CreatePublicEventModel : PublicEventModel
	{
		public CreatePublicEventModel() { }

		[Required(ErrorMessage = "Please attach file")]
		[Display(Name = "Proof of Approval")]
		public HttpPostedFileBase DocumentEvent { get; set; }
	}

	public class EditPublicEventModel : PublicEventModel
	{
		[Required]
		public int Id { get; set; }

		public string origin { get; set; }

		//[Required(ErrorMessage = "Please attach file")]
		[Display(Name = "Proof of Approval")]
		public HttpPostedFileBase DocumentEvent { get; set; }
	}

	public class DetailsPublicEventModel : PublicEventModel
	{
		public DetailsPublicEventModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
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

		[Display(Name = "Objective")]
		public string EventObjective { get; set; }


		[DataType(DataType.Date)]
		[Display(Name = "Start Date")]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "End Date")]
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
}