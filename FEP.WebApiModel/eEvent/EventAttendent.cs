using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.eEvent
{
	public class EventAttendentModel
	{
		public int Id { get; set; }
		[Display(Name = "AttName", ResourceType = typeof(Language.Event))]
		public string AttendeeName { get; set; }

		[Display(Name = "AttUserType", ResourceType = typeof(Language.Event))]
		public ParticipantType? UserType { get; set; }

		[Display(Name = "AttUserType", ResourceType = typeof(Language.Event))]
		public string UserTypeDesc { get; set; }

		[Display(Name = "AttCompanyName", ResourceType = typeof(Language.Event))]
		public string CompanyName { get; set; }
		[Display(Name = "AttBookingNumber", ResourceType = typeof(Language.Event))]
		public string BookingNumber { get; set; }

		[Display(Name = "AttICNo", ResourceType = typeof(Language.Event))]
		public string ICNo { get; set; }

		[Display(Name = "AttCheckInStatus", ResourceType = typeof(Language.Event))]
		public CheckInStatus? CheckInStatus { get; set; }

		[Display(Name = "AttCheckInStatus", ResourceType = typeof(Language.Event))]
		public string CheckInStatusDesc { get; set; }

		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
		public int? UserId { get; set; }
		public string EventName { get; set; }
		public int? EventId { get; set; }
		public int? CreatedBy { get; set; }
	}

	public class CreateEventAttendentModel
	{
		[Display(Name = "AttName", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Insert Attendee Name")]
		public string AttendeeName { get; set; }

		[Display(Name = "AttUserType", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Select Attendee Type")]
		public ParticipantType? UserType { get; set; }

		[Display(Name = "AttCompanyName", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Insert Company Name")]
		public string CompanyName { get; set; }

		[Display(Name = "AttBookingNumber", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Insert Booking Number")]
		public string BookingNumber { get; set; }

		[Display(Name = "AttICNo", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Insert MyCard Number")]
		[RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
		public string ICNo { get; set; }

		[Display(Name = "AttCheckInStatus", ResourceType = typeof(Language.Event))]
		[Required(ErrorMessage = "Please Select Checked-in Status")]
		public CheckInStatus? CheckInStatus { get; set; }

		[Display(Name = "AttUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "AttEventId", ResourceType = typeof(Language.Event))]
		public int? EventId { get; set; }

		public IEnumerable<SelectListItem> EventList { get; set; }

		[Display(Name = "AttEventId", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }



		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}

	public class EditEventAttendentModel : CreateEventAttendentModel
	{
		public EditEventAttendentModel() { }
	}

	public class DetailsEventAttendentModel : EventAttendentModel
	{
		public DetailsEventAttendentModel() { }
	}

	public class DeleteEventAttendentModel : DetailsEventAttendentModel
	{
		public DeleteEventAttendentModel() { }
	}

	public class ListEventAttendentModel
	{
		public FilterEventAttendentModel Filter { get; set; }
		public EventAttendentModel List { get; set; }
	}

	public class FilterEventAttendentModel : DataTableModel 
	{
		[Display(Name = "AttName", ResourceType = typeof(Language.Event))]
		public string AttendeeName { get; set; }

		[Display(Name = "AttUserType", ResourceType = typeof(Language.Event))]
		public ParticipantType? UserType { get; set; }

		[Display(Name = "AttCompanyName", ResourceType = typeof(Language.Event))]
		public string CompanyName { get; set; }

		[Display(Name = "AttBookingNumber", ResourceType = typeof(Language.Event))]
		public string BookingNumber { get; set; }

		[Display(Name = "AttICNo", ResourceType = typeof(Language.Event))]
		public string ICNo { get; set; }

		[Display(Name = "AttCheckInStatus", ResourceType = typeof(Language.Event))]
		public CheckInStatus? CheckInStatus { get; set; }

		[Display(Name = "AttUserId", ResourceType = typeof(Language.Event))]
		public int? UserId { get; set; }

		[Display(Name = "AttUserId", ResourceType = typeof(Language.Event))]
		public string UserName { get; set; }

		[Display(Name = "AttEventId", ResourceType = typeof(Language.Event))]
		public int? EventId { get; set; }

		[Display(Name = "AttEventId", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
	}

}
