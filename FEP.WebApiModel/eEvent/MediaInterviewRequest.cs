using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel
{
	public class MediaInterviewRequestApiModel
	{
		public int Id { get; set; }
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
		public DateTime? Date { get; set; }
		public DateTime? Time { get; set; }
		public string Location { get; set; }
		public string Language { get; set; }
		public string Topic { get; set; }
		public string Designation { get; set; }
		public int? UserId { get; set; }
		public string UserName { get; set; }
		public int? EventId { get; set; }
		public string EventTitle { get; set; }
		public bool Display { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
