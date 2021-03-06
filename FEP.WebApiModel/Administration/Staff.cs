﻿using FEP.Helper;
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

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Administrator.Staff))]
        public int? DepartmentId { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Administrator.Staff))]
        public int? BranchId { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Staff))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        public IEnumerable<SelectListItem> Branchs { get; set; }

    }

    public class StaffModel
    {       
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Administrator.Staff))]
        public string Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Administrator.Staff))]
        public string Branch { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Staff))]
        public bool Status { get; set; }

    }
       
    public class DetailsStaffModel
    {
        public int Id { get; set; }
               
        [Display(Name = "FieldStaffId", ResourceType = typeof(Language.Administrator.Staff))]
        public string StaffId { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Administrator.Staff))]
        public DepartmentModel Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Administrator.Staff))]
        public BranchModel Branch { get; set; }

        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Administrator.Staff))]
        public DesignationModel Designation { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Staff))]
        public string ICNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Staff))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Staff))]
        public bool Status { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Staff))]
        public List<RoleModel> Roles { get; set; }

    }

    public class EditStaffModel
    {

        public int Id { get; set; }

        [Display(Name = "FieldStaffId", ResourceType = typeof(Language.Administrator.Staff))]
        public string StaffId { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Administrator.Staff))]
        public DepartmentModel Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Administrator.Staff))]
        public int? BranchId { get; set; }

        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Administrator.Staff))]
        public DesignationModel Designation { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Staff))]
        public string ICNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Staff))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Staff))]
        public bool Status { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Staff))]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Administrator.Staff))]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Branches { get; set; }

    }

    public class DepartmentModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }
    }
      
    public class DesignationModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Administrator.Staff))]
        public string Name { get; set; }
    }


}
