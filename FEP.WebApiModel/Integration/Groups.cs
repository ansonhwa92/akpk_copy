using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;

namespace FEP.WebApiModel.Integration
{
    public class TargetedGroup
    {
        public int ID { get; set; }

        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Display(Name = "Group Description")]
        public string Description { get; set; }

        [Display(Name = "Minimum Age")]
        public int? MinAge { get; set; }

        [Display(Name = "Maximum Age")]
        public int? MaxAge { get; set; }

        [Display(Name = "Gender")]
        public MemberGender Gender { get; set; }

        [Display(Name = "Minimum Salary")]
        public int? MinSalary { get; set; }

        [Display(Name = "Maximum Salary")]
        public int? MaxSalary { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        [Display(Name = "Payment Status")]
        public int? PaymentStatus { get; set; }

        [Display(Name = "Delinquent")]
        public int? Delinquent { get; set; }

        [Display(Name = "Employment Type")]
        public int? EmploymentType { get; set; }

        [Display(Name = "State")]
        public int? State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }
    }

    public class TargetedGroupDropdown
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class TargetedGroupMember
    {
        public int ID { get; set; }

        [Display(Name = "Group Name")]
        public int TargetedGroupID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [Display(Name = "Information Source")]
        public SourceSystem Source { get; set; }
    }

}