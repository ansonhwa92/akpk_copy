using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEventCancellationRequest.Models
{
	public class CancellationRequestModel
	{
		public CancellationRequestModel() { }

		[Display(Name = "Requester Name")]
		public int? UserId { get; set; }

		[Display(Name = "Requester Name")]
		public string UserName { get; set; }

		[Display(Name = "Event Title")]
		public int? EventId { get; set; }

		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Display(Name = "Reasons")]
		public string Reasons { get; set; }

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

		public int? VerifyId { get; set; }

		public string VerifierName { get; set; }
	}

	public class CreateCancellationRequestModel : CancellationRequestModel
	{
		public CreateCancellationRequestModel() { }
	}

	public class EditCancellationRequestModel : CancellationRequestModel
	{
		[Required]
		public int Id { get; set; }
	}

	public class DetailsCancellationRequestModel : CancellationRequestModel
	{
		public DetailsCancellationRequestModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
	}

	public class DeleteCancellationRequestModel : DetailsCancellationRequestModel
	{
		public DeleteCancellationRequestModel() { }
	}

	public class ListCancellationRequestModel
	{
		public ListCancellationRequestModel() { }
		public List<DetailsCancellationRequestModel> CancelRequestList { get; set; }
		public FilterCancellationRequestModel filter { get; set; }

		public ListCancellationRequestModel(List<DetailsCancellationRequestModel> ListCancelRequest)
		{
			this.CancelRequestList = ListCancelRequest;
		}
	}

	public class FilterCancellationRequestModel {
		[Display(Name = "Requester Name")]
		public int? UserId { get; set; }

		[Display(Name = "Requester Name")]
		public string UserName { get; set; }

		[Display(Name = "Event Title")]
		public int? EventId { get; set; }

		[Display(Name = "Event Title")]
		public string EventTitle { get; set; }

		[Display(Name = "Reasons")]
		public string Reasons { get; set; }
	}

}