using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;


namespace FEP.WebApiModel.Setting
{
    public class TargetedGroup
    {
        [Required]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Required]
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
        public MemberStatus? Status { get; set; }

        [Display(Name = "Payment Status")]
        public MemberPaymentStatus? PaymentStatus { get; set; }

        [Display(Name = "Delinquent")]
        public MemberDelinquent? Delinquent { get; set; }

        [Display(Name = "Employment Type")]
        public MemberEmploymentType? EmploymentType { get; set; }

        [Display(Name = "State")]
        public int? State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }

    public class FilterTargetedGroup : DataTableModel
    {

        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Display(Name = "Group Description")]
        public string Description { get; set; }
    }

    public class ListTargetedGroup
    {
        public FilterTargetedGroup Filter { get; set; }
        public ViewTargetedGroup List { get; set; }
    }

    public class ViewTargetedGroup : TargetedGroup
    {
        public ViewTargetedGroup() { }

        public int ID { get; set; }
    }

    public class CreateTargetedGroup : TargetedGroup
    {
        public CreateTargetedGroup() { }

        public DateTime DateCreated { get; set; }

        public int CreatorId { get; set; }
    }

    public class EditTargetedGroup : TargetedGroup
    {
        public EditTargetedGroup() { }

        public int ID { get; set; }
    }

    public class DetailsTargetedGroup : TargetedGroup
    {
        public DetailsTargetedGroup() { }

        public int ID { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }
    }

    public class DeleteTargetedGroup : DetailsTargetedGroup
    {
        public DeleteTargetedGroup() { }
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