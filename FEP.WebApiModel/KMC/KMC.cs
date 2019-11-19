using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.KMC
{
    public class KMCModel
    {
        public int Id { get; set; }        
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }  
        public string AvatarUrl { get; set; }
    }

    public class ListKMCModel
    {
        public ListKMCModel()
        {
            List = new List<KMCModel>();
        }

        public CategoryModel Category { get; set; }       
        public FilterKMCModel Filter { get; set; }
        public List<KMCModel> List { get; set; }
    }

    public class FilterKMCModel
    {
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMC))]
        public string Title { get; set; }

        [Display(Name = "FieldCreatedBy", ResourceType = typeof(Language.KMC))]
        public string CreatedBy { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateFrom", ResourceType = typeof(Language.General))]
        public DateTime? DateFrom { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateTo", ResourceType = typeof(Language.General))]
        public DateTime? DateTo { get; set; }
    }

    public class CreateKMCModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredCategory", ErrorMessageResourceType = typeof(Language.KMC))]
        public int KMCCategoryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.KMC))]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredType", ErrorMessageResourceType = typeof(Language.KMC))]
        public KMCType? Type { get; set; }
        public int? FileId { get; set; }
        public string EditorCode { get; set; }
        public bool IsPublic { get; set; }
        public bool IsShow { get; set; }
        public bool IsEditor { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCreatedBy", ErrorMessageResourceType = typeof(Language.KMC))]
        public int CreatedBy { get; set; }

    }

    public class EditKMCModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMC))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCategory", ErrorMessageResourceType = typeof(Language.KMC))]
        public int KMCCategoryId { get; set; }

        public string ThumbnailUrl { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredKMCType", ErrorMessageResourceType = typeof(Language.KMC))]
        public KMCType KMCType { get; set; }

        public int? FileId { get; set; }

        public string EditorCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCreatedBy", ErrorMessageResourceType = typeof(Language.KMC))]
        public int CreatedBy { get; set; }
    }

    public class DetailsKMCModel
    {
        public int Id { get; set; }
        public CategoryModel Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }                
        public KMCType? Type { get; set; }
        public int? FileId { get; set; }
        public string EditorCode { get; set; }
        public bool IsPublic { get; set; }
        public bool IsShow { get; set; }
        public bool IsEditor { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }


}
