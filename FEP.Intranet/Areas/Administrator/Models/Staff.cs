using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Models
{
    public class StaffModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.User))]
        public string Branch { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.User))]
        public string Department { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }        
    }

    public class StaffListModel
    {
        public StaffFilterModel Filter { get; set; }
        public StaffModel List { get; set; }        
    }

    public class StaffFilterModel
    {
        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.User))]
        public int BranchId { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.User))]
        public int DepartmentId { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Branches { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
    }

}