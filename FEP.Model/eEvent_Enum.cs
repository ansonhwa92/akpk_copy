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
		[Display(Name = "Broadcast")]
		Broadcast,

		[Display(Name = "Internet")]
		Internet,

		[Display(Name = "Magazine")]
		Magazine,

		[Display(Name = "Newspaper")]
		Newspaper,

		[Display(Name = "Pres Agency")]
		PressAgency
	}

	public enum EventStatus
	{
		[Display(Name = "New")]
		New,
		[Display(Name = "Pending")]
		PendingToVerified,
		[Display(Name = "Pending")]
		Approval,
		[Display(Name = "Approved")]
		Approved, //==Published
		[Display(Name = "Modify Request")]
		RequestToModify,
		[Display(Name = "Cancel Request")]
		RequestToCancel,
		[Display(Name = "Cancelled")]
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
		[Display(Name = "Internal")]
		FEP,
		[Display(Name = "External")]
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

	public enum MaritialStatus
	{
		Married,
		Single,
		Divorced,
		Widowed
	}

	public enum Religion
	{
		Islam,
		Hindu,
		Buddha, 
		Christian
	}

	public enum MediaLanguage
	{
		[Display(Name = "Bahasa Malaysia")]
		BahasaMalaysia,
		English,
		Tamil,
		Mandarin,
		Cantonese,
	}
}
