using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;
using FEP.Model.eLearning;

namespace FEP.WebApiModel.eLearning
{

    public class CourseApprovalLogModel : BaseModel
    {
        public int CourseId { get; set; }
        public string CreatedByName { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? ApproverId { get; set; }
        public virtual User Approver { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
        public bool IsNextLevelRequired { get; set; }

        public string Remark { get; set; }
    }


    //public enum ApprovalLevel
    //{
    //    [Display(Name = "ApprovalLevelNone", ResourceType = typeof(Language.eLearning.Enum))]
    //    None,
    //    [Display(Name = "ApprovalLevelVerifier", ResourceType = typeof(Language.eLearning.Enum))]
    //    Verifier,
    //    [Display(Name = "ApprovalLevelApprover1", ResourceType = typeof(Language.eLearning.Enum))]
    //    Approver1,
    //    [Display(Name = "ApprovalLevelApprover2", ResourceType = typeof(Language.eLearning.Enum))]
    //    Approver2,
    //    [Display(Name = "ApprovalLevelApprover3", ResourceType = typeof(Language.eLearning.Enum))]
    //    Approver3
    //}

    //public enum ApprovalStatus
    //{
    //    [Display(Name = "ApprovalStatusNone", ResourceType = typeof(Language.eLearning.Enum))]
    //    None,
    //    [Display(Name = "ApprovalStatusApproved", ResourceType = typeof(Language.eLearning.Enum))]
    //    Approved,
    //    [Display(Name = "ApprovalStatusRejected", ResourceType = typeof(Language.eLearning.Enum))]
    //    Rejected,
    //    [Display(Name = "ApprovalStatusSubmitted", ResourceType = typeof(Language.eLearning.Enum))]
    //    Submitted,
    //}
}
