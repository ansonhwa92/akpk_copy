using FEP.Model;
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
		[Required(ErrorMessage = "Please Insert Media Name")]
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }

		[Required(ErrorMessage = "Please Insert Media Type")]
		[Display(Name = "Media Type")]
		public MediaType? MediaType { get; set; }

		[Required(ErrorMessage = "Please Insert Contact Person")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[Required(ErrorMessage = "Please Insert Contact Number")]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Contact Number")]
		public int? ContactNo { get; set; }

		[Required(ErrorMessage = "Please Insert Address")]
		[Display(Name = "Address")]
		public string AddressStreet1 { get; set; }
		[Required(ErrorMessage = "Please Insert Address")]
		public string AddressStreet2 { get; set; }
		[Required(ErrorMessage = "Please Insert Poscode")]
		public int? AddressPoscode { get; set; }
		[Required(ErrorMessage = "Please Insert City")]
		public string AddressCity { get; set; }
		[Required(ErrorMessage = "Please Select State")]
		public MediaState? State { get; set; }


		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? DateStart { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "Event Date")]
		public DateTime? DateEnd { get; set; }

		[Required(ErrorMessage = "Please Insert Time")]
		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "Time")]
		public DateTime? Time { get; set; }

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

		public IEnumerable<SelectListItem> RepresentativeList { get; set; }

		[Display(Name = "Supporting Document")]
		public string GetFileName { get; set; }



		//File
		[Display(Name = "Supporting Document")]
		public HttpPostedFileBase DocumentMedia { get; set; } 
		[Display(Name = "File Name")]
		public string FileName { get; set; }
		[Display(Name = "File Description")]
		public string FileDescription { get; set; }
		[Display(Name = "Uploaded Date")]
		public DateTime UploadedDate { get; set; }
	}

	public class CreateMediaInterviewModel : MediaInterviewModel
	{
		public CreateMediaInterviewModel() { }
	}

	public class EditMediaInterviewModel : MediaInterviewModel
	{
		public EditMediaInterviewModel() { }

		[Required]
		public int Id { get; set; }

		public string origin { get; set; }
	}

	public class DetailsMediaInterviewModel : MediaInterviewModel
	{
		public DetailsMediaInterviewModel() { }

		[Required]
		public int Id { get; set; }

		public bool Display { get; set; }
		public int? CreatedBy { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime CreatedDate { get; set; }
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

		[DataType(DataType.Date)]
		[Display(Name = "Start Date")]
		public DateTime? DateStart { get; set; }

		[DataType(DataType.Date)]
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