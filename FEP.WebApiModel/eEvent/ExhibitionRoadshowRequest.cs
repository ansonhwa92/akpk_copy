﻿using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.eEvent
{
	public class ExhibitionRoadshowRequestModel
	{
		public int Id { get; set; }
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }
		[Display(Name = "ExRoadOrganiserEmail", ResourceType = typeof(Language.Event))]
		public string OrganiserEmail { get; set; }

		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }
		public string AddressStreet2 { get; set; }
		public string AddressPoscode { get; set; }
		public string AddressCity { get; set; }
		public MediaState? State { get; set; }

		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }

		[Display(Name = "ExRoadStartTime", ResourceType = typeof(Language.Event))]
		public DateTime? StartTime { get; set; }

		[Display(Name = "ExRoadEndTime", ResourceType = typeof(Language.Event))]
		public DateTime? EndTime { get; set; }

		[Display(Name = "ExRoadParticipantRequirement", ResourceType = typeof(Language.Event))]
		public int? ParticipationRequirement { get; set; }

		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }
		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }

		[Display(Name = "ExRoadReceivedById", ResourceType = typeof(Language.Event))]
		public int? ReceivedById { get; set; }

		[Display(Name = "ExRoadReceivedByName", ResourceType = typeof(Language.Event))]
		public string ReceivedByName { get; set; }

		[Display(Name = "ExRoadReceivedDate", ResourceType = typeof(Language.Event))]
		[UIHint("Date")]
		public DateTime? ReceivedDate { get; set; }

		[Display(Name = "ExRoadReceive_Via", ResourceType = typeof(Language.Event))]
		public string Receive_Via { get; set; }
		public IEnumerable<SelectListItem> ReceivedBys { get; set; }

		[Display(Name = "ExRoadNomineeId", ResourceType = typeof(Language.Event))]
		public int[] NomineeId { get; set; }

		[Display(Name = "ExRoadNomineeName", ResourceType = typeof(Language.Event))]
		public string NomineeName { get; set; }
		public IEnumerable<SelectListItem> Nominees { get; set; }

		[Display(Name = "ExRoadRefNo", ResourceType = typeof(Language.Event))]
		public string RefNo { get; set; }
		
		[Display(Name = "ExRoadBranch", ResourceType = typeof(Language.Event))]
		public int? BranchId { get; set; }

		[Display(Name = "ExRoadBranch", ResourceType = typeof(Language.Event))]
		public string BranchName { get; set; }

		[Display(Name = "ExRoadContactNo", ResourceType = typeof(Language.Event))]
		public string ContactNo { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }

		public int? SLAReminderStatusId { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }

		public DutyRosterTempModel DutyRoster { get; set; }
        public string RecommendationsJSON { get; set; }

		public bool HasDetail { get; set; }
		public bool HasEdit { get; set; }
		public bool HasDelete { get; set; }
	}

	public class FilterExhibitionRoadshowRequestModel : DataTableModel
	{
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }
		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string Location { get; set; }
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }
		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }
		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }
        public bool? RequireAction { get; set; }
        public UserAccess? UserAccess { get; set; }

		public int? UserId { get; set; }

		public bool? HasAccessCCD { get; set; }
	}

	public class ListExhibitionRoadshowRequestModel
	{
		public FilterExhibitionRoadshowRequestModel Filter { get; set; }

		public ExhibitionRoadshowRequestModel List { get; set; }
	}

    //duty roster
    public class DutyPIC
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class DutyDetails
    {
        public int id { get; set; }
        public string date { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public List<DutyPIC> pic { get; set; }
    }
    public class DutyRosterTempModel
    {
        public int exhibitionId { get; set; }
        public List<DutyDetails> dutyRoster { get; set; }
    }

    public class RecommendationDetailModel
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    public class RecommendationModel
    {
        public int exhibitionId { get; set; }
        public List<RecommendationDetailModel> recommendations { get; set; }
    }
    public class DetailsExhibitionRoadshowRequestModel : ExhibitionRoadshowRequestModel
	{
		public DetailsExhibitionRoadshowRequestModel() { }

		[Display(Name = "ExRoadExhibitionSupDoc", ResourceType = typeof(Language.Event))]
		public IEnumerable<Attachment> Attachments { get; set; }

        public string DutyRosterJSON { get; set; }
    }

	public class CreateExhibitionRoadshowRequestModel
	{
		public CreateExhibitionRoadshowRequestModel()
		{
			FilesId = new List<int>();
		}
		public List<int> FilesId { get; set; }

        //tajul add
        public string DutyRosterJSON { get; set; }
        public string RecommendationsJSON { get; set; }

		[Required(ErrorMessage = "Please insert Event Name")]
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }

		[Required(ErrorMessage = "Please insert Organiser")]
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }

		[Required(ErrorMessage = "Please Insert Email")]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "ExRoadOrganiserEmail", ResourceType = typeof(Language.Event))]
		public string OrganiserEmail { get; set; }

		[Required(ErrorMessage = "Please Insert Contact Number")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^[0-9]{11,}$", ErrorMessage = "Not a valid phone number")]
		[Display(Name = "ExRoadContactNo", ResourceType = typeof(Language.Event))]
		public string ContactNo { get; set; }

		[Required(ErrorMessage = "Please insert Location")]
		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string AddressStreet1 { get; set; }
		[Required(ErrorMessage = "Please Insert Address")]
		public string AddressStreet2 { get; set; }
		[Required(ErrorMessage = "Please Insert Poscode")]
		public string AddressPoscode { get; set; }
		[Required(ErrorMessage = "Please Insert City")]
		public string AddressCity { get; set; }
		[Required(ErrorMessage = "Please Select State")]
		public MediaState? State { get; set; }

		[Required(ErrorMessage = "Please insert Start Date")]
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please insert End Date")]
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }

		[Required(ErrorMessage = "Please insert Start Time")]
		[DataType(DataType.Time)]
		[Display(Name = "ExRoadStartTime", ResourceType = typeof(Language.Event))]
		public DateTime? StartTime { get; set; }

		[Required(ErrorMessage = "Please insert End Time")]
		[DataType(DataType.Time)]
		[Display(Name = "ExRoadEndTime", ResourceType = typeof(Language.Event))]
		public DateTime? EndTime { get; set; }

		[Required(ErrorMessage = "Please insert Participant Requirement")]
		[Display(Name = "ExRoadParticipantRequirement", ResourceType = typeof(Language.Event))]
		public int? ParticipationRequirement { get; set; }

		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }

		[Display(Name = "ExRoadExhibitionStatusDesc", ResourceType = typeof(Language.Event))]
		public string ExhibitionStatusDesc { get; set; }

		[Required(ErrorMessage = "Please insert Receive By")]
		[Display(Name = "ExRoadReceivedById", ResourceType = typeof(Language.Event))]
		public int? ReceivedById { get; set; }

		[Display(Name = "ExRoadReceivedByName", ResourceType = typeof(Language.Event))]
		public string ReceivedByName { get; set; }

		[Required(ErrorMessage = "Please insert Receive Date")]
		[Display(Name = "ExRoadReceivedDate", ResourceType = typeof(Language.Event))]
		[DataType(DataType.Date)]
		[UIHint("Date")]
		public DateTime? ReceivedDate { get; set; }

		[Required(ErrorMessage = "Please insert Receive Via")]
		[Display(Name = "ExRoadReceive_Via", ResourceType = typeof(Language.Event))]
		public string Receive_Via { get; set; }

		[Display(Name = "ExRoadReceivedBys", ResourceType = typeof(Language.Event))]
		public IEnumerable<SelectListItem> ReceivedBys { get; set; }

		[Required(ErrorMessage = "Please select Nominees")]
		[Display(Name = "ExRoadNomineeId", ResourceType = typeof(Language.Event))]
		public int[] NomineeId { get; set; }

		[Display(Name = "ExRoadNomineeName", ResourceType = typeof(Language.Event))]
		public string NomineeName { get; set; }

		public int? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }

		[Display(Name = "ExRoadBranch", ResourceType = typeof(Language.Event))]
		public int? BranchId { get; set; }

		[Display(Name = "ExRoadBranch", ResourceType = typeof(Language.Event))]
		public string BranchName { get; set; }

		public IEnumerable<SelectListItem> Nominees { get; set; }
		public IEnumerable<SelectListItem> BranchList { get; set; }

		public bool HasDetail { get; set; }
		public bool HasEdit { get; set; }
		public bool HasDelete { get; set; }
	}

	public class EditExhibitionRoadshowRequestModel : CreateExhibitionRoadshowRequestModel
	{
		public EditExhibitionRoadshowRequestModel()
		{
			FilesId = new List<int>();
		}

		public int Id { get; set; }
		public string RefNo { get; set; }
		public IEnumerable<Attachment> Attachments { get; set; }
	}

	public class DeleteExhibitionRoadshowRequestModel : ExhibitionRoadshowRequestModel
	{
		public DeleteExhibitionRoadshowRequestModel() { }
	}

	public class ExhibitionRoadshowApprovalModel
	{
		public ExhibitionRoadshowRequestModel exhibitionroadshow { get; set; }
		public ApprovalModel approval { get; set; }
	}

	public class ApprovalModel
	{
		[Required]
		public int? Id { get; set; }

		[Required]
		public int? ExhibitionId { get; set; } 

		[Required]
		[Display(Name = "Level")]
		public EventApprovalLevel Level { get; set; }

		[Required]
		public int? ApproverId { get; set; }

		[Required]
		[Range((int)(EventApprovalStatus.Approved), (int)(EventApprovalStatus.Rejected), ErrorMessage = "Please Select")]
		[Display(Name = "PubApprovalStatus")]
		public EventApprovalStatus Status { get; set; }

		[Required]
		[Display(Name = "Remarks")]
		public string Remarks { get; set; }

		[Display(Name = "Require Next")]
		public bool RequireNext { get; set; }
	}


	public class ExhibitionApprovalHistoryModel
	{
		public EventApprovalLevel Level { get; set; }

		public int? ApproverId { get; set; }

		public string UserName { get; set; }

		public EventApprovalStatus Status { get; set; }

		public DateTime? ApprovalDate { get; set; }

		public string Remarks { get; set; }
	}

}


