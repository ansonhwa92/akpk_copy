using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
	public enum EventTargetGroup
	{
		[Display(Name = "Student")]
		Student,
		[Display(Name = "Goverment")]
		Goverment,
		[Display(Name = "PrivateAgency")]
		PrivateAgency,
		[Display(Name = "NGO")]
		NGO,
		[Display(Name = "Public")]
		Public
	}

	//public enum EventCategory
	//{
	//	[Display(Name = "")]
	//	Workshop,
	//	[Display(Name = "")]
	//	Seminar,
	//	[Display(Name = "")]
	//	Dialogue,
	//	[Display(Name = "")]
	//	Conference,
	//	[Display(Name = "")]
	//	Symposium,
	//	[Display(Name = "")]
	//	Convention
	//}

	public enum EventStatus
	{
		New,
		PendingToVerified,
		Approval,
		Approved, //==Published
		RequestToModify,
		RequestToCancel,
		Cancelled
	}

	public enum Ticket
	{
		Individual = 0,
		IndividualWithPaper = 1,
		Group = 3,
		Agency = 4
	}

	public enum BookingStatus
	{
		NewBooking,
		Paid,
		Withdraw
	}

	public enum ApprovalType
	{
		NewEvent,
		ModifyEvent,
		CancelEvent
	}

	public enum VerifyType
	{
		NewEvent,
		ModifyEvent,
		CancelEvent
	}

	public enum SpeakerType
	{
		FEP,
		OBS
	}
}
