using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Administration
{
    public class BranchModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.Branch))]
        public string Name { get; set; }

        public int StateId { get; set; }

        [Display(Name = "FieldState", ResourceType = typeof(Language.Branch))]
        public string State { get; set; }
    }
       
    public class CreateBranchModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Branch))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Branch))]
        public string Name { get; set; }

        [Display(Name = "FieldState", ResourceType = typeof(Language.Branch))]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }
    }

    public class EditBranchModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Branch))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Branch))]
        public string Name { get; set; }

        [Display(Name = "FieldState", ResourceType = typeof(Language.Branch))]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }
    }

    public class DeleteBranchModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Branch))]
        public string Name { get; set; }

        [Display(Name = "FieldState", ResourceType = typeof(Language.Branch))]
        public string State { get; set; }
    }

  

}
