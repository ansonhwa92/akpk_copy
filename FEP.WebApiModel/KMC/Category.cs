using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.KMC
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMCCategory))]
        public string Title { get; set; }
    }

    public class CreateCategoryModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.KMCCategory))]
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMCCategory))]
        public string Title { get; set; }
    }

    public class EditCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.KMCCategory))]
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMCCategory))]
        public string Title { get; set; }
    }

    public class DeleteCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMCCategory))]
        public string Title { get; set; }
    }

}
