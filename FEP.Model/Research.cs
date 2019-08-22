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
        public SurveyType? Type { get; set; }
        public SurveyCategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetGroup { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Contents { get; set; }
        public string Template { get; set; }
        public bool Active { get; set; }
        public string Pictures { get; set; }
        public string ProofOfApproval { get; set; }         // uploaded proof of approval
        // non-key in data
        public DateTime DateAdded { get; set; }
        public SurveyStatus Status { get; set; }
        // nav
        public virtual ICollection<SurveyApproval> Approvals { get; set; }
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
        public DateTime ResponseDate { get; set; }
        public string Contents { get; set; }

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
        [Display(Name = "SurveyStatusNew", ResourceType = typeof(Language.RnPEnum))]
        New,
        [Display(Name = "SurveyStatusSubmitted", ResourceType = typeof(Language.RnPEnum))]
        Submitted,
        [Display(Name = "SurveyStatusApproved", ResourceType = typeof(Language.RnPEnum))]
        Approved,
        [Display(Name = "SurveyStatusPublished", ResourceType = typeof(Language.RnPEnum))]
        Published,
        [Display(Name = "SurveyStatusUnpublished", ResourceType = typeof(Language.RnPEnum))]
        Unpublished,
        [Display(Name = "SurveyStatusTrashed", ResourceType = typeof(Language.RnPEnum))]
        Trashed
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
