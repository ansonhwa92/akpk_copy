﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
	public enum EventTargetGroup
	{
		[Display(Name = "Learner")]
		Learner,
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

		[Display(Name = "Pending Verification")]
		PendingforVerification, //If Admin Public Event Submit

		[Display(Name = "Pending Approval 2")]
		VerifiedbyFirstApprover, //If Approver 1 Submit

		[Display(Name = "Pending Approval 3")]
		VerifiedbySecondApprover, //If Approver 1 Submit

		[Display(Name = "Approved")]
		Approved, //If Approver 3 Approved

		[Display(Name = "Require Amendment")]
		RequireAmendment, //If Approver 3 Rejected

		[Display(Name = "Cancelled")]
		Cancelled, //If Admin Cancel Public Event 

		[Display(Name = "Published")]
		Published, //If Admin Event who creates Public Event 

		[Display(Name = "Pending Approval 1")]
		Verified
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

	public enum EventApprovalLevel
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

	public enum EventApprovalStatus
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

	public enum SpeakerStatus
	{
		[Display(Name = "Active")]
		Active,
		[Display(Name = "Inactive")]
		Inactive
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
		RequireAmendment,
		[Display(Name = "Pending Approval 2")]
		ApprovedByApprover1,
		[Display(Name = "Pending Approval 3")]
		ApprovedByApprover2,
		[Display(Name = "Approved")]
		ApprovedByApprover3,
	}

	public enum ExhibitionStatus
	{
		[Display(Name = "Draft")]
		New,
		[Display(Name = "Pending Verification")]
		PendingVerified,
		[Display(Name = "Pending Approval 1")]
		Verified,
		[Display(Name = "Require Amendment")]
		RequireAmendment,
		[Display(Name = "Pending Approval 2")]
		ApprovedByApprover1,
		[Display(Name = "Pending Approval 3")]
		ApprovedByApprover2,
		[Display(Name = "Approved")]
		ApprovedByApprover3,


		[Display(Name = "Pending Verification")]
		SubmitVerifyDutyRoster,
		[Display(Name = "Pending Approval")]
		VerifiedDutyRoster,
		[Display(Name = "Require Amendment")]
		RequireAmendmentDutyRoster,
		[Display(Name = "Approved")]
		ApproveDutyRoster,
		[Display(Name = "Participation Accepted")]
		AcceptParticipation,
		[Display(Name = "Participation Declined")]
		DeclineParticipation,
		[Display(Name = "Nominees Invited")]
		NomineesInvited,
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

	public enum RequestType
	{
		[Display(Name = "Cancellation Required")]
		CancelRequired,
		[Display(Name = "Modification Required")]
		ModifyRequired
	}

	public enum RequestStatus
	{
		[Display(Name = "Draft")]
		New,
		[Display(Name = "Pending Verification")]
		PendingVerified,
		[Display(Name = "Pending Approval 1")]
		Verified,
		[Display(Name = "Require Amendment")]
		AmendmentRequired,
		[Display(Name = "Pending Approval 2")]
		ApprovedByApprover1,
		[Display(Name = "Pending Approval 3")]
		ApprovedByApprover2,
		[Display(Name = "Approved")]
		ApprovedByApprover3,
	}

	public enum CheckInStatus
	{
		[Display(Name = "Not Yet")]
		Not_Yet,
		[Display(Name = "Checked-in")]
		Check_In
	}

	public enum ParticipantType
	{
		Individual,
		Group,
		Agency,
	}
}
