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

namespace FEP.WebApiModel.MediaInterview
{
	public class ListMediaInterviewRequestApi
	{
		public FilterMediaInterviewRequestApiModel Filter { get; set; }
		public MediaInterviewRequestApiModel List { get; set; }
	}

	public class MediaInterviewRequestApiModel
	{
		public int Id { get; set; }
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }
		[Display(Name = "Media Type")]
		public MediaType? MediaType { get; set; }
		[Display(Name = "Media Type")]
		public string MediaTypeDesc { get; set; }
		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		[Display(Name = "Contact No")]
		public string ContactNo { get; set; }
		[Display(Name = "Address")]
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }

		[Display(Name = "Status")]
		public MediaStatus? MediaStatus { get; set; }

		[Display(Name = "Status")]
		public string MediaStatusDesc { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime? DateStart { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime? DateEnd { get; set; }

		[DataType(DataType.Time)]
		public DateTime? Time { get; set; }
		public string Location { get; set; }
		public MediaLanguage? Language { get; set; }
		public string Topic { get; set; }

		[Display(Name = "Reference No")]
		public string RefNo { get; set; }

	
		[Display(Name = "MediaBranch", ResourceType = typeof(Language.Event))]
		public int? BranchId { get; set; }

		[Display(Name = "MediaBranch", ResourceType = typeof(Language.Event))]
		public string BranchName { get; set; }

		[Display(Name = "MediaRepUserName", ResourceType = typeof(Language.Event))]
		public string RepUserName { get; set; }
		[Display(Name = "MediaRepDesignation", ResourceType = typeof(Language.Event))]
		public string RepDesignation { get; set; }
		[Display(Name = "MediaRepEmail", ResourceType = typeof(Language.Event))]
		public string RepEmail { get; set; }
		[Display(Name = "MediaRepMobileNumber", ResourceType = typeof(Language.Event))]
		public string RepMobileNumber { get; set; }
		[Display(Name = "Name")]
		public int[] RepUserId { get; set; }

		public int? EventId { get; set; }
		public string EventTitle { get; set; }
		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? SLAReminderStatusId { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }
		public IEnumerable<SelectListItem> RepresentativeList { get; set; }
	}

	public class FilterMediaInterviewRequestApiModel : DataTableModel
	{
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }

		[Display(Name = "Media Type")]
		public MediaType? MediaType { get; set; }

		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime? DateStart { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		public DateTime? DateEnd { get; set; }

		[Display(Name = "Status")]
		public MediaStatus? MediaStatus { get; set; }

		[Display(Name = "Reference No")]
		public string RefNo { get; set; }
        public bool? RequireAction { get; set; }
        public UserAccess UserAccess { get; set; }
    }

    public class DetailsMediaInterviewRequestApiModel : MediaInterviewRequestApiModel
	{
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class CreateMediaInterviewRequestApiModel : MediaInterviewRequestApiModel
	{
		public CreateMediaInterviewRequestApiModel()
		{
			FilesId = new List<int>();
		}

		public List<int> FilesId { get; set; }
	}

	public class EditMediaInterviewRequestApiModel : CreateMediaInterviewRequestApiModel
	{
		public EditMediaInterviewRequestApiModel()
		{
			FilesId = new List<int>();
		}

		public IEnumerable<Attachment> Attachments { get; set; }
	}


	public class DeleteMediaInterviewRequestApiModel : MediaInterviewRequestApiModel
	{
		public DeleteMediaInterviewRequestApiModel() { }
	}

	public class MediaInterviewApprovalModel
	{
		public MediaInterviewRequestApiModel mediainterview { get; set; }
		public ApprovalModel approval { get; set; }
	}

	public class ApprovalModel
	{
		[Required]
		public int? Id { get; set; }

		[Required]
		public int? MediaId { get; set; }

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

	public class MediaInterviewApprovalHistoryModel
	{
		public EventApprovalLevel Level { get; set; }

		public int? ApproverId { get; set; }

		public string UserName { get; set; }

		public EventApprovalStatus Status { get; set; }

		public DateTime? ApprovalDate { get; set; }

		public string Remarks { get; set; }
	}
}
