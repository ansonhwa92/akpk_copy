using FEP.Model;
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

        public bool IsPublic { get; set; }

        public bool IsShow { get; set; }

        [Display(Name = "FieldThumbnail", ResourceType = typeof(Language.KMC))]
        public string ThumbnailUrl { get; set; }

        public HttpPostedFileBase ThumbnailFile { get; set; }

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

        public bool IsPublic { get; set; }

        public bool IsShow { get; set; }

        [Display(Name = "FieldThumbnail", ResourceType = typeof(Language.KMC))]
        public string ThumbnailUrl { get; set; }

        public HttpPostedFileBase ThumbnailFile { get; set; }

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
    }
}