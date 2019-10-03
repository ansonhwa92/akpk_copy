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
		[Display(Name = "Televisyen")]
		Televisyen,

		[Display(Name = "Radio")]
		Radio,

		[Display(Name = "Online")]
		Online,

		[Display(Name = "Magazine / Newspaper")]
		MagazineNewspaper,

	}

	public enum EventStatus
	{
		[Display(Name = "Draft")]
		New, //If Saved Public Event

		[Display(Name = "Pending Approval")]
		PendingforVerification, //If Admin Public Event Submit

		[Display(Name = "Pending Approval")]
		VerifiedbyFirstApprover, //If Approver 1 Submit

		[Display(Name = "Pending Approval")]
		VerifiedbySecondApprover, //If Approver 1 Submit

		[Display(Name = "Approved")]
		Approved, //If Approver 3 Approved

		[Display(Name = "Require Amendment")]
		RejectNeedToEdit, //If Approver 3 Rejected

		[Display(Name = "Rejected")]
		Cancelled, //If Admin Cancel Public Event 

		[Display(Name = "Published")]
		Published //If Admin Event who creates Public Event 
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

	public enum ApprovalLevel
	{
		[Display(Name = "Verifier")]
		Verifier,
		[Display(Name = "Approver 1")]
		Approver1,
		[Display(Name = "Approver 2")]
		Approver2,
		[Display(Name = "Approver 3")]
		Approver3
	}

	public enum ApprovalStatus
	{
		[Display(Name = "None")]
		None,
		[Display(Name = "Approved")]
		Approved,
		[Display(Name = "Rejected")]
		Rejected
	}

	public enum SpeakerType
	{
		[Display(Name = "Internal")]
		Internal, //FEP STAFF
		[Display(Name = "External")]
		External //OBS SPEAKER
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
		[Display(Name = "Draft")]
		New,
		[Display(Name = "Representative Available")]
		RepAvailable,
		[Display(Name = "Representative Not Available")]
		RepNotAvailable,
		[Display(Name = "Pending Verification")]
		PendingVerified,
		[Display(Name = "Pending Approval 1")]
		Verified,
		[Display(Name = "Require Amendment")]
		NotVerified,
		[Display(Name = "Pending Approval 2")]
		ApprovedByApprover1,
		[Display(Name = "Pending Approval 3")]
		ApprovedByApprover2,
		[Display(Name = "Approved")]
		ApprovedByApprover3,
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
