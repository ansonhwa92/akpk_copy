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
		[Display(Name = "Private Agency")]
		PrivateAgency,
		[Display(Name = "NGO")]
		NGO,
		[Display(Name = "Public")]
		Public
	}

	public enum MediaType
	{
		[Display(Name = "Panel Interview")]
		PanelInterview,
		[Display(Name = "Group Interview")]
		GroupInterview,
		[Display(Name = "Sequential Interview")]
		SequentialInterview,
		[Display(Name = "Formal / Informal Interview")]
		FormalInformalInterview,
		
	}

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

	public enum MediaState
	{
		Selangor,
		[Display(Name = "Wilayah Persekutuan Kuala Lumpur")]
		WilayahPersekutuanKualaLumpur,
		[Display(Name = "Negeri Sembilan")]
		NegeriSembilan,
		Melaka,
		Pahang,
		Terengganu,
		Kelantan,
		Perak,
		[Display(Name = "Pulau Pinang")]
		PulauPinang,
		Johor,
		Sabah,
		Sarawak,
		Perlis,
		Kedah
	}

	public enum MediaStatus
	{
		New,
		[Display(Name = "Representative Available")]
		RepAvailable,
		[Display(Name = "Representative Not Available")]
		RepNotAvailable,

	}

	public enum ExhibitionStatus
	{
		New,
		Cancelled,
		Approved
	}
}
