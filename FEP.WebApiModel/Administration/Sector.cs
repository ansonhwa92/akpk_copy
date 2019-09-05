using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class SectorModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.Sector))]
        public string Name { get; set; }
    }
       
    public class CreateSectorModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Sector))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Sector))]
        public string Name { get; set; }
    }

    public class EditSectorModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Sector))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Sector))]
        public string Name { get; set; }
    }

    public class DeleteSectorModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Sector))]
        public string Name { get; set; }
    }

  

}
