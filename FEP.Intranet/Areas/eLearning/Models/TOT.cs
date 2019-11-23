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

        public int AgeR1NoOfMale { get; set; }
        public int AgeR1NoOfFemale { get; set; }

        public int AgeR2NoOfMale { get; set; }
        public int AgeR2NoOfFemale { get; set; }

        public int AgeR3NoOfMale { get; set; }
        public int AgeR3NoOfFemale { get; set; }

        public int AgeR4NoOfMale { get; set; }
        public int AgeR4NoOfFemale { get; set; }

        public int AgeR5NoOfMale { get; set; }
        public int AgeR5NoOfFemale { get; set; }

        public int SalaryR1NoOfMale { get; set; }
        public int SalaryR1NoOfFemale { get; set; }

        public int SalaryR2NoOfMale { get; set; }
        public int SalaryR2NoOfFemale { get; set; }

        public int SalaryR3NoOfMale { get; set; }
        public int SalaryR3NoOfFemale { get; set; }

        public int SalaryR4NoOfMale { get; set; }
        public int SalaryR4NoOfFemale { get; set; }

        public int SalaryR5NoOfMale { get; set; }
        public int SalaryR5NoOfFemale { get; set; }

        public int SalaryR6NoOfMale { get; set; }
        public int SalaryR6NoOfFemale { get; set; }

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

        public int AgeR1NoOfMale { get; set; }
        public int AgeR1NoOfFemale { get; set; }

        public int AgeR2NoOfMale { get; set; }
        public int AgeR2NoOfFemale { get; set; }

        public int AgeR3NoOfMale { get; set; }
        public int AgeR3NoOfFemale { get; set; }

        public int AgeR4NoOfMale { get; set; }
        public int AgeR4NoOfFemale { get; set; }

        public int AgeR5NoOfMale { get; set; }
        public int AgeR5NoOfFemale { get; set; }

        public int SalaryR1NoOfMale { get; set; }
        public int SalaryR1NoOfFemale { get; set; }

        public int SalaryR2NoOfMale { get; set; }
        public int SalaryR2NoOfFemale { get; set; }

        public int SalaryR3NoOfMale { get; set; }
        public int SalaryR3NoOfFemale { get; set; }

        public int SalaryR4NoOfMale { get; set; }
        public int SalaryR4NoOfFemale { get; set; }

        public int SalaryR5NoOfMale { get; set; }
        public int SalaryR5NoOfFemale { get; set; }

        public int SalaryR6NoOfMale { get; set; }
        public int SalaryR6NoOfFemale { get; set; }

        [Display(Name = "FieldAttachment", ResourceType = typeof(Language.TOT))]
        public IEnumerable<Attachment> Attachments { get; set; }

        public IEnumerable<HttpPostedFileBase> AttachmentFiles { get; set; }

        
    }
}