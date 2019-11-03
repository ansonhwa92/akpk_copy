using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Models
{
	public class MediaInterviewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Please Insert Media Name")]
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }

		[Required(ErrorMessage = "Please Insert Media Type")]
		[Display(Name = "Media Type")]
		public MediaType? MediaType { get; set; }

		[Required(ErrorMessage = "Please Insert Contact Person")]
		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[Required(ErrorMessage = "Please Insert Contact Number")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		[Display(Name = "Contact Number")]
		public string ContactNo { get; set; }

		[Required(ErrorMessage = "Please Insert Address")]
		[Display(Name = "Address")]
		public string AddressStreet1 { get; set; }
		[Required(ErrorMessage = "Please Insert Address")]
		public string AddressStreet2 { get; set; }
		[Required(ErrorMessage = "Please Insert Poscode")]
		public string AddressPoscode { get; set; }
		[Required(ErrorMessage = "Please Insert City")]
		public string AddressCity { get; set; }
		[Required(ErrorMessage = "Please Select State")]
		public MediaState? State { get; set; }


		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please Insert Start Date")]
		[Display(Name = "Start Date")]
		public DateTime? DateStart { get; set; }

		[Required(ErrorMessage = "Please Insert End Date")]
		[Display(Name = "End Date")]
		public DateTime? DateEnd { get; set; }

		[Required(ErrorMessage = "Please Insert Time")]
		[Display(Name = "Time")]
		[DataType(DataType.Time)]
		public DateTime? Time { get; set; }

		[Required(ErrorMessage = "Please Select Language")]
		[Display(Name = "Language")]
		public MediaLanguage? Language { get; set; }


		[Required(ErrorMessage = "Please Insert Topic")]
		[Display(Name = "Topic")]
		public string Topic { get; set; }

		[Display(Name = "Designation")]
		public string RepDesignation { get; set; }

		[Display(Name = "Email Address")]
		public string RepEmail { get; set; }

		[Display(Name = "Mobile Number")]
		public string RepMobileNumber { get; set; }

		[Required(ErrorMessage = "Please Insert Staff Representative")]
		[Display(Name = "Name")]
		public int? RepUserId { get; set; }

		[Display(Name = "Name")]
		public string RepUserName { get; set; }

		[Display(Name = "Status")]
		public MediaStatus? MediaStatus { get; set; }

		[Display(Name = "Reference No")]
		public string RefNo { get; set; }

		public IEnumerable<SelectListItem> RepresentativeList { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }
	}

	public class CreateMediaInterviewModel : MediaInterviewModel
	{
		public CreateMediaInterviewModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
		}

		[Required]
		[Display(Name = "Proof of Approval")]
		public IEnumerable<Attachment> Attachments { get; set; }

		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }
	}

	public class EditMediaInterviewModel : MediaInterviewModel
	{
		public EditMediaInterviewModel()
		{
			Attachments = new List<Attachment>();
			AttachmentFiles = new List<HttpPostedFileBase>();
		}

		[Required]
		[Display(Name = "Proof of Approval")]
		public IEnumerable<Attachment> Attachments { get; set; }

		public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }
	}

	public class DetailsMediaInterviewModel : MediaInterviewModel
	{
		public DetailsMediaInterviewModel() { }

		[Display(Name = "Proof of Approval")]
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class DeleteMediaInterviewModel : DetailsMediaInterviewModel
	{
		public DeleteMediaInterviewModel() { }
	}

	public class FilterMediaInterviewModel
	{
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }

		[Display(Name = "Media Type")]
		public MediaType? MediaType { get; set; }

		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[Display(Name = "Start Date")]
		public DateTime? DateStart { get; set; }

		[Display(Name = "End Date")]
		public DateTime? DateEnd { get; set; }

		[Display(Name = "Status")]
		public MediaStatus? MediaStatus { get; set; }
	}

	public class ListMediaInterviewModel 
	{
		public ListMediaInterviewModel() { }
		public FilterMediaInterviewModel filter { get; set; }

		public List<DetailsMediaInterviewModel> MediaInterviewList { get; set; }

		public ListMediaInterviewModel(List<DetailsMediaInterviewModel> ListMediaInterview)
		{
			this.MediaInterviewList = ListMediaInterview;
		}
	}
}