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
    [Table("TargetedGroups")]
    public class TargetedGroups
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public MemberGender Gender { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public MemberStatus? Status { get; set; }
        public MemberPaymentStatus? PaymentStatus { get; set; }
        public MemberDelinquent? Delinquent { get; set; }
        public MemberEmploymentType? EmploymentType { get; set; }
        public int? State { get; set; }
        public string City { get; set; }
        // auto-filled in data...............................................................................................
        public DateTime DateCreated { get; set; }
        public int CreatorId { get; set; }
        public bool Active { get; set; }
    }

    [Table("TargetedGroupMembers")]
    public class TargetedGroupMembers
    {
        [Key]
        public int ID { get; set; }
        public int TargetedGroupID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public SourceSystem Source { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("TargetedGroupID")]
        public virtual TargetedGroups Group { get; set; }
    }

    public enum MemberGender
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female
    }

    public enum MemberStatus
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }

    public enum MemberPaymentStatus
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Incomplete")]
        Incomplete,
        [Display(Name = "Complete")]
        Complete
    }

    public enum MemberDelinquent
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Yes")]
        Yes,
        [Display(Name = "No")]
        No
    }

    public enum MemberEmploymentType
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Unemployed")]
        Unemployed,
        [Display(Name = "Employed")]
        Employed,
        [Display(Name = "Self Employed")]
        SelfEmployed,
    }

    public enum SourceSystem
    {
        [Display(Name = "HRMS")]
        HRMS,
        [Display(Name = "OBS")]
        OBS,
        [Display(Name = "CBS")]
        CBS
    }

}
