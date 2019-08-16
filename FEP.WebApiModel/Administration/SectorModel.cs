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
        [Display(Name = "FieldSectorName", ResourceType = typeof(Language.Administration))]
        public string Name { get; set; }
    }

    public class ListSectorModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldSectorName", ResourceType = typeof(Language.Administration))]
        public string Name { get; set; }
    }

    public class CreateSectorModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredSectorName", ErrorMessageResourceType = typeof(Language.Administration))]
        [Display(Name = "FieldSectorName", ResourceType = typeof(Language.Administration))]
        public string Name { get; set; }
    }

    public class EditSectorModel
    {        
        [Required(ErrorMessageResourceName = "ValidRequiredSectorName", ErrorMessageResourceType = typeof(Language.Administration))]
        [Display(Name = "FieldSectorName", ResourceType = typeof(Language.Administration))]
        public string Name { get; set; }
    }
    
}
