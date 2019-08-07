using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eEventMediaInterview.Models
{
	public class MediaInterviewModel
	{
		[Display(Name = "Media Name")]
		public string MediaName { get; set; }

		[Display(Name = "Media Type")]
		public string MediaType { get; set; }

		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Contact No")]
		public int ContactNo { get; set; }

		[Display(Name = "Address")]
		public string Address { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		public DateTime Date { get; set; }

		public DateTime Time { get; set; }

		public string Location { get; set; }

		public string Language { get; set; }

		public string Topic { get; set; }

		public string Designation { get; set; }

		public int? UserId { get; set; }

		public string UserName { get; set; }

		public int? EventId { get; set; }
	
		public string EventTitle { get; set; }
		
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
		public string MediaType { get; set; }

		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Date")]
		public DateTime Date { get; set; }
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