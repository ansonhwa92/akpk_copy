﻿using FEP.Helper;
using FEP.Model;
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
		public MediaType? MediaType { get; set; }
		[Display(Name = "Media Type")]
		public string MediaTypeDesc { get; set; }
		[Display(Name = "Contact Person")]
		public string ContactPerson { get; set; }
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Contact No")]
		public int? ContactNo { get; set; }
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
		public DateTime? DateStart { get; set; }

		[Display(Name = "End Date")]
		public DateTime? DateEnd { get; set; }
		public DateTime? Time { get; set; }
		public string Location { get; set; }
		public MediaLanguage? Language { get; set; }
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

	public class ListMediaInterviewRequestApiModel
	{
		public FilterMediaInterviewRequestApiModel Filter { get; set; }
		public MediaInterviewRequestApiModel List { get; set; }
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
		public DateTime? DateStart { get; set; }

		[Display(Name = "End Date")]
		public DateTime? DateEnd { get; set; }

		[Display(Name = "Status")]
		public MediaStatus? MediaStatus { get; set; }
	}
}
