using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.eLearning.Models
{
    public class CreateTOTReportModel
    {
        public CreateTOTReportModel()
        {       
            Attachments = new List<Attachment>();
            AttachmentFiles = new List<HttpPostedFileBase>();
        }

      
        [Required(ErrorMessageResourceName = "ValidRequiredModule", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldModule", ResourceType = typeof(Language.TOT))]
        public string Module { get; set; }
                
        [Required(ErrorMessageResourceName = "ValidRequiredStartDate", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldStartDate", ResourceType = typeof(Language.TOT))]
        public DateTime? StartDate { get; set; }
                
        [Required(ErrorMessageResourceName = "ValidRequiredEndDate", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldEndDate", ResourceType = typeof(Language.TOT))]
        public DateTime? EndDate { get; set; }
        
        [Required(ErrorMessageResourceName = "ValidRequiredStartTime", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldStartTime", ResourceType = typeof(Language.TOT))]
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }               
        
        [Required(ErrorMessageResourceName = "ValidRequiredEndTime", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldEndTime", ResourceType = typeof(Language.TOT))]
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredVenue", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldVenue", ResourceType = typeof(Language.TOT))]
        public string Venue { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfMale", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldNoOfMale", ResourceType = typeof(Language.TOT))]
        [Range(1, Int32.MaxValue)]
        public int NoOfMale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfFemale", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldNoOfFemale", ResourceType = typeof(Language.TOT))]
        [Range(1, Int32.MaxValue)]
        public int NoOfFemale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgeRange", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldAgeRange", ResourceType = typeof(Language.TOT))]
        public TOTAgeRange? AgeRange { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSalaryRange", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldSalaryRange", ResourceType = typeof(Language.TOT))]
        public TOTSalaryRange? SalaryRange { get; set; }

        [Display(Name = "FieldAttachment", ResourceType = typeof(Language.TOT))]
        public IEnumerable<Attachment> Attachments { get; set; }

        public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

        
    }


    public class EditTOTReportModel
    {
        public int Id { get; set; }

        public EditTOTReportModel()
        {           
            Attachments = new List<Attachment>();
            AttachmentFiles = new List<HttpPostedFileBase>();
        }
           
        [Required(ErrorMessageResourceName = "ValidRequiredModule", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldModule", ResourceType = typeof(Language.TOT))]
        public string Module { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStartDate", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldStartDate", ResourceType = typeof(Language.TOT))]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEndDate", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldEndDate", ResourceType = typeof(Language.TOT))]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStartTime", ErrorMessageResourceType = typeof(Language.TOT))]
        [DataType(DataType.Time)]
        [Display(Name = "FieldStartTime", ResourceType = typeof(Language.TOT))]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEndTime", ErrorMessageResourceType = typeof(Language.TOT))]
        [DataType(DataType.Time)]
        [Display(Name = "FieldEndTime", ResourceType = typeof(Language.TOT))]
        public DateTime? EndTime { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredVenue", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldVenue", ResourceType = typeof(Language.TOT))]
        public string Venue { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfMale", ErrorMessageResourceType = typeof(Language.TOT))]
        [Range(0, Int32.MaxValue)]
        [Display(Name = "FieldNoOfMale", ResourceType = typeof(Language.TOT))]
        public int NoOfMale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfFemale", ErrorMessageResourceType = typeof(Language.TOT))]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "FieldNoOfFemale", ResourceType = typeof(Language.TOT))]
        public int NoOfFemale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgeRange", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldAgeRange", ResourceType = typeof(Language.TOT))]
        public TOTAgeRange? AgeRange { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSalaryRange", ErrorMessageResourceType = typeof(Language.TOT))]
        [Display(Name = "FieldSalaryRange", ResourceType = typeof(Language.TOT))]
        public TOTSalaryRange? SalaryRange { get; set; }

        [Display(Name = "FieldAttachment", ResourceType = typeof(Language.TOT))]
        public IEnumerable<Attachment> Attachments { get; set; }

        public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

        
    }
}