using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        [Display(Name = "Department")]
        public string Name { get; set; }
    }

    public class BranchModel
    {
        public int Id { get; set; }
        [Display(Name = "Branch")]
        public string Name { get; set; }
    }
}
