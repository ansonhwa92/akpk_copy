using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    public class CourseCategoryModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.eLearning.CourseCategory))]
        public string Name { get; set; }
    }

    public class CreateCourseCategoryModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.eLearning.CourseCategory))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.eLearning.CourseCategory))]
        public string Name { get; set; }
    }

    public class EditCourseCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.eLearning.CourseCategory))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.eLearning.CourseCategory))]
        public string Name { get; set; }
    }

    public class DeleteCourseCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.eLearning.CourseCategory))]
        public string Name { get; set; }
    }
}
