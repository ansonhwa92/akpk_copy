using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eEvent
{
    public class EventCategoryModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldName", ResourceType = typeof(Language.EventCategory))]
        public string Name { get; set; }
    }

    public class CreateEventCategoryModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.EventCategory))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.EventCategory))]
        public string Name { get; set; }
    }

    public class EditEventCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.EventCategory))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.EventCategory))]
        public string Name { get; set; }
    }

    public class DeleteEventCategoryModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.EventCategory))]
        public string Name { get; set; }
    }
}
