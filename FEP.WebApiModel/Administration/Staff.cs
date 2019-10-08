using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Administration
{
    public class ListStaffModel
    {
        public FilterStaffModel Filter { get; set; }
        public StaffModel List { get; set; }
    }

    public class FilterStaffModel : DataTableModel
    {

        [Display(Name = "FieldName", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Staff))]
        public int? DepartmentId { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Staff))]
        public int? BranchId { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Staff))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        public IEnumerable<SelectListItem> Branchs { get; set; }

    }

    public class StaffModel
    {       
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Staff))]
        public string Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Staff))]
        public string Branch { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Staff))]
        public bool Status { get; set; }

    }
       
    public class DetailsStaffModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldStaffId", ResourceType = typeof(Language.Staff))]
        public string StaffId { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Staff))]
        public DepartmentModel Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Staff))]
        public BranchModel Branch { get; set; }

        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Staff))]
        public DesignationModel Designation { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Staff))]
        public string ICNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Staff))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Staff))]
        public bool Status { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Individual))]
        public List<RoleModel> Roles { get; set; }

    }

    public class DepartmentModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }
    }

    public class BranchModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }
    }

    public class DesignationModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }
    }


}
