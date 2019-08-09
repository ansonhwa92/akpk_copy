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
        public SurveyType Type { get; set; }
        public SurveyCategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetGroup { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Contents { get; set; }
        public bool Active { get; set; }
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
        Public,
        Targeted
    }

    public enum SurveyCategory
    {
        Research,
        eLearning,
        Event
    }

    public enum SurveyStatus
    {
        New,
        Submitted,
        Approved,
        Published,
        Unpublished,
        Trashed
    }

    public enum SurveyApprovalLevels
    {
        Verifier,
        Approver1,
        Approver2,
        Approver3
    }

    public enum SurveyApprovalStatus
    {
        None,
        Approved,
        Rejected
    }

    public enum SurveyResponseTypes
    {
        Actual,
        Testing
    }

}
