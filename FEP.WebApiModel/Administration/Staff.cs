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

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }

        public IEnumerable<SelectListItem> Branchs { get; set; }

    }

    public class StaffModel
    {       
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

    }
       
    public class DetailsStaffModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

    }

   


}
