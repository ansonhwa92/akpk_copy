using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class CountryModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.Country))]
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }

    public class CreateCountryModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Country))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Country))]
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }

    public class EditCountryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Country))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Country))]
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }

    public class DeleteCountryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Country))]
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
