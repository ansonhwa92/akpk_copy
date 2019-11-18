using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class StateModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.State))]
        public string Name { get; set; }
    }

    public class CreateStateModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.State))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.State))]
        public string Name { get; set; }
    }

    public class EditStateModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.State))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.State))]
        public string Name { get; set; }
    }

    public class DeleteStateModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.State))]
        public string Name { get; set; }
    }
}
