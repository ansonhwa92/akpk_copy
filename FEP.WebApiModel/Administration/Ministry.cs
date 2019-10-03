using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class MinistryModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.Ministry))]
        public string Name { get; set; }
    }
       
    public class CreateMinistryModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Ministry))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Ministry))]
        public string Name { get; set; }
    }

    public class EditMinistryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Ministry))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Ministry))]
        public string Name { get; set; }
    }

    public class DeleteMinistryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Ministry))]
        public string Name { get; set; }
    }

  

}
