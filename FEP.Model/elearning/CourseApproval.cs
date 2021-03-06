﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model.eLearning
{
    public class CourseApprovalLog : BaseEntity
    {
        public int CourseId { get; set; }
        public string CreatedByName { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.None;
        public int? ApproverId { get; set; }
        public virtual User Approver { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
        public bool IsNextLevelRequired { get; set; }

        public string Remark { get; set; }
    }

    public enum ApprovalLevel
    {
        [Display(Name = "ApprovalLevelNone", ResourceType = typeof(Language.eLearning.Enum))]
        None,

        [Display(Name = "ApprovalLevelVerifier", ResourceType = typeof(Language.eLearning.Enum))]
        Verifier,

        [Display(Name = "ApprovalLevelApprover1", ResourceType = typeof(Language.eLearning.Enum))]
        Approver1,

        [Display(Name = "ApprovalLevelApprover2", ResourceType = typeof(Language.eLearning.Enum))]
        Approver2,

        [Display(Name = "ApprovalLevelApprover3", ResourceType = typeof(Language.eLearning.Enum))]
        Approver3
    }

    public enum ApprovalStatus
    {
        [Display(Name = "ApprovalStatusNone", ResourceType = typeof(Language.eLearning.Enum))]
        None,

        [Display(Name = "ApprovalStatusApproved", ResourceType = typeof(Language.eLearning.Enum))]
        Approved,

        [Display(Name = "ApprovalStatusRejected", ResourceType = typeof(Language.eLearning.Enum))]
        Rejected,

        [Display(Name = "ApprovalStatusSubmitted", ResourceType = typeof(Language.eLearning.Enum))]
        Submitted,
    }
}