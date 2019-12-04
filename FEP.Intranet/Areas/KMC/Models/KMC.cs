using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Models
{
    public class CreateKMCModel
    {
        [Display(Name = "FieldCategory", ResourceType = typeof(Language.KMC))]
        public string Category { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMC))]
        public string Title { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.KMC))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCategory", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldCategory", ResourceType = typeof(Language.KMC))]
        public int CategoryId { get; set; }

        [Display(Name = "FieldIsPublic", ResourceType = typeof(Language.KMC))]
        public bool IsPublic { get; set; }

        [Display(Name = "FieldIsShow", ResourceType = typeof(Language.KMC))]
        public bool IsShow { get; set; }

        [Display(Name = "FieldThumbnail", ResourceType = typeof(Language.KMC))]
        public string ThumbnailUrl { get; set; }

        public HttpPostedFileBase ThumbnailFile { get; set; }

        [Display(Name = "FieldIsEditor", ResourceType = typeof(Language.KMC))]
        public bool IsEditor { get; set; }
        
        [Required(ErrorMessageResourceName = "ValidRequiredType", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldType", ResourceType = typeof(Language.KMC))]
        public KMCType? Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredFile", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldFile", ResourceType = typeof(Language.KMC))]
        public HttpPostedFileBase File { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "ValidRequiredEditor", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldEditorCode", ResourceType = typeof(Language.KMC))]
        public string EditorCode { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.KMC))]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.KMC))]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public string filter_imgs { get; set; }
        public string filter_videos { get; set; }
        public string filter_audios { get; set; }
        public string filter_docs { get; set; }

    }

    public class EditKMCModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldCategory", ResourceType = typeof(Language.KMC))]
        public string Category { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldTitle", ResourceType = typeof(Language.KMC))]
        public string Title { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.KMC))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCategory", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldCategory", ResourceType = typeof(Language.KMC))]
        public int CategoryId { get; set; }

        [Display(Name = "FieldIsPublic", ResourceType = typeof(Language.KMC))]
        public bool IsPublic { get; set; }

        [Display(Name = "FieldIsShow", ResourceType = typeof(Language.KMC))]
        public bool IsShow { get; set; }

        [Display(Name = "FieldThumbnail", ResourceType = typeof(Language.KMC))]
        public string ThumbnailUrl { get; set; }

        public HttpPostedFileBase ThumbnailFile { get; set; }

        [Display(Name = "FieldIsEditor", ResourceType = typeof(Language.KMC))]
        public bool IsEditor { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredType", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldType", ResourceType = typeof(Language.KMC))]
        public KMCType? Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredFile", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldFile", ResourceType = typeof(Language.KMC))]
        public HttpPostedFileBase File { get; set; }

        public int? FileId { get; set; }
                
        public string FileName { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "ValidRequiredEditor", ErrorMessageResourceType = typeof(Language.KMC))]
        [Display(Name = "FieldEditorCode", ResourceType = typeof(Language.KMC))]
        public string EditorCode { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.KMC))]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.KMC))]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public string filter_imgs { get; set; }
        public string filter_videos { get; set; }
        public string filter_audios { get; set; }
        public string filter_docs { get; set; }
    }
}