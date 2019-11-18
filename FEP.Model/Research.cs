using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace FEP.Model
{
    [Table("Survey")]
    public class Survey
    {
        [Key]
        public int ID { get; set; }
        // survey info.......................................................................................................
        public SurveyType? Type { get; set; }
        public SurveyCategory Category { get; set; }        // auto based on situation
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetGroup { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool RequireLogin { get; set; }
        public string Contents { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public bool Active { get; set; }
        //public string Pictures { get; set; }
        //public string ProofOfApproval { get; set; }
        public string CancelRemark { get; set; }
        // aut-filled in data................................................................................................
        public DateTime DateAdded { get; set; }
        public int CreatorId { get; set; }
        public string RefNo { get; set; }
        public SurveyStatus Status { get; set; }
        public DateTime? DateCancelled { get; set; }
        public int InviteCount { get; set; }
        public int SubmitCount { get; set; }
        // DMS integration (TODO)............................................................................................
        public string DmsPath { get; set; }
        public int NotificationID { get; set; }
        // sub-tables
        public virtual ICollection<SurveyApproval> Approvals { get; set; }
    }

    [Table("SurveyFile")]
    public class SurveyFile
    {
        [Key]
        public int ID { get; set; }
        public SurveyFileCategory FileCategory { get; set; }
        public int FileId { get; set; }
        public int ParentId { get; set; }
        [ForeignKey("FileId")]
        public virtual FileDocument FileDocument { get; set; }
    }

    [Table("SurveyApproval")]
    public class SurveyApproval
    {
        [Key]
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public SurveyApprovalLevels Level { get; set; }
        public int ApproverId { get; set; }
        public SurveyApprovalStatus Status { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Remarks { get; set; }
        public bool RequireNext { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("SurveyID")]
        public virtual Survey Survey { get; set; }
    }

    [Table("SurveyResponse")]
    public class SurveyResponse
    {
        [Key]
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public SurveyResponseTypes Type { get; set; }
        public int UserId { get; set; }     // if 0/Null = anonymous/public respondent
        public string Email { get; set; }
        public DateTime ResponseDate { get; set; }
        public string Contents { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("SurveyID")]
        public virtual Survey Survey { get; set; }
    }

    public enum SurveyType
    {
        [Display(Name = "SurveyTypePublic", ResourceType = typeof(Language.RnPEnum))]
        Public,
        [Display(Name = "SurveyTypeTargeted", ResourceType = typeof(Language.RnPEnum))]
        Targeted
    }

    public enum SurveyCategory
    {
        [Display(Name = "SurveyCategoryResearch", ResourceType = typeof(Language.RnPEnum))]
        Research,
        [Display(Name = "SurveyCategoryeLearning", ResourceType = typeof(Language.RnPEnum))]
        eLearning,
        [Display(Name = "SurveyCategoryEvent", ResourceType = typeof(Language.RnPEnum))]
        Event
    }

    public enum SurveyStatus
    {
        [Display(Name = "SurveyStatusNew", ResourceType = typeof(Language.RnPEnum))]                        // draft
        New,
        [Display(Name = "SurveyStatusSubmitted", ResourceType = typeof(Language.RnPEnum))]                  // pending verification
        Submitted,
        [Display(Name = "SurveyStatusVerifierRejected", ResourceType = typeof(Language.RnPEnum))]           // pending amendment
        VerifierRejected,
        [Display(Name = "SurveyStatusVerified", ResourceType = typeof(Language.RnPEnum))]                   // pending approval
        Verified,
        [Display(Name = "SurveyStatusApproverRejected", ResourceType = typeof(Language.RnPEnum))]           // pending amendment
        ApproverRejected,
        [Display(Name = "SurveyStatusApproved", ResourceType = typeof(Language.RnPEnum))]                   // approved
        Approved,
        [Display(Name = "SurveyStatusPublished", ResourceType = typeof(Language.RnPEnum))]                  // published
        Published,
        [Display(Name = "SurveyStatusUnpublished", ResourceType = typeof(Language.RnPEnum))]                // unpublished
        Unpublished,
        [Display(Name = "SurveyStatusTrashed", ResourceType = typeof(Language.RnPEnum))]                    // cancelled
        Trashed
    }

    public enum SurveyFileCategory
    {
        [Display(Name = "Cover Image")]
        CoverImage,
        [Display(Name = "Author Image")]
        AuthorImage,
        [Display(Name = "Proof of Approval")]
        ProofOfApproval
    }

    public enum SurveyApprovalLevels
    {
        [Display(Name = "SurveyApprovalLevelVerifier", ResourceType = typeof(Language.RnPEnum))]
        Verifier,
        [Display(Name = "SurveyApprovalLevelApprover1", ResourceType = typeof(Language.RnPEnum))]
        Approver1,
        [Display(Name = "SurveyApprovalLevelApprover2", ResourceType = typeof(Language.RnPEnum))]
        Approver2,
        [Display(Name = "SurveyApprovalLevelApprover3", ResourceType = typeof(Language.RnPEnum))]
        Approver3
    }

    public enum SurveyApprovalStatus
    {
        [Display(Name = "SurveyApprovalStatusNone", ResourceType = typeof(Language.RnPEnum))]
        None,
        [Display(Name = "SurveyApprovalStatusApproved", ResourceType = typeof(Language.RnPEnum))]
        Approved,
        [Display(Name = "SurveyApprovalStatusRejected", ResourceType = typeof(Language.RnPEnum))]
        Rejected
    }

    public enum SurveyResponseTypes
    {
        [Display(Name = "SurveyResponseTypeActual", ResourceType = typeof(Language.RnPEnum))]
        Actual,
        [Display(Name = "SurveyResponseTypeTesting", ResourceType = typeof(Language.RnPEnum))]
        Testing
    }

}
