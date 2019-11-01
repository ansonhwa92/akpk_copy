using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEP.WebApiModel.eLearning
{

    public class TOTCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TOTModuleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ListTOTReportModel
    {
        public FilterTOTReportModel Filter { get; set; }
        public TOTReportModel List { get; set; }
    }

    public class FilterTOTReportModel : DataTableModel
    {
        public FilterTOTReportModel()
        {
            Courses = Enumerable.Empty<System.Web.Mvc.SelectListItem>();
            Modules = Enumerable.Empty<System.Web.Mvc.SelectListItem>();
        }


        [Display(Name = "FieldDate", ResourceType = typeof(Language.TOT))]
        public DateTime? Date { get; set; }

        [Display(Name = "FieldVenue", ResourceType = typeof(Language.TOT))]
        public string Venue { get; set; }

        [Display(Name = "FieldCourse", ResourceType = typeof(Language.TOT))]
        public int? CourseId { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.TOT))]
        public int? ModuleId { get; set; }

        [Display(Name = "FieldAgeRange", ResourceType = typeof(Language.TOT))]
        public TOTAgeRange? AgeRange { get; set; }

        [Display(Name = "FieldSalaryRange", ResourceType = typeof(Language.TOT))]
        public TOTSalaryRange? SalaryRange { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Courses { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Modules { get; set; }
    }

    public class TOTReportModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldCourse", ResourceType = typeof(Language.TOT))]
        public string Course { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.TOT))]
        public string Module { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDate", ResourceType = typeof(Language.TOT))]
        public DateTime Date { get; set; }

        [Display(Name = "FieldVenue", ResourceType = typeof(Language.TOT))]
        public string Venue { get; set; }

    }

    public class CreateTOTReportModel
    {
       
        [Required(ErrorMessageResourceName = "ValidRequiredCourse", ErrorMessageResourceType = typeof(Language.TOT))]       
        public int CourseId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredModule", ErrorMessageResourceType = typeof(Language.TOT))]       
        public int ModuleId { get; set; }
              
        [Required(ErrorMessageResourceName = "ValidRequiredStartDate", ErrorMessageResourceType = typeof(Language.TOT))]       
        public DateTime? StartDate { get; set; }
               
        [Required(ErrorMessageResourceName = "ValidRequiredEndDate", ErrorMessageResourceType = typeof(Language.TOT))]        
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredVenue", ErrorMessageResourceType = typeof(Language.TOT))]        
        public string Venue { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfMale", ErrorMessageResourceType = typeof(Language.TOT))]        
        public int NoOfMale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfFemale", ErrorMessageResourceType = typeof(Language.TOT))]        
        public int NoOfFemale { get; set; }
        
        [Required(ErrorMessageResourceName = "ValidRequiredAgeRange", ErrorMessageResourceType = typeof(Language.TOT))]        
        public TOTAgeRange AgeRange { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSalaryRange", ErrorMessageResourceType = typeof(Language.TOT))]        
        public TOTSalaryRange SalaryRange { get; set; }
        
        public List<int> FilesId { get; set; }

        public int? CreatedBy { get; set; }

    }

  
    public class EditTOTReportModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessageResourceName = "ValidRequiredCourse", ErrorMessageResourceType = typeof(Language.TOT))]
        public int CourseId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredModule", ErrorMessageResourceType = typeof(Language.TOT))]        
        public int ModuleId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStartDate", ErrorMessageResourceType = typeof(Language.TOT))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEndDate", ErrorMessageResourceType = typeof(Language.TOT))]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredVenue", ErrorMessageResourceType = typeof(Language.TOT))]        
        public string Venue { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfMale", ErrorMessageResourceType = typeof(Language.TOT))]
        public int NoOfMale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNoOfFemale", ErrorMessageResourceType = typeof(Language.TOT))]
        public int NoOfFemale { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgeRange", ErrorMessageResourceType = typeof(Language.TOT))]
        public TOTAgeRange AgeRange { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSalaryRange", ErrorMessageResourceType = typeof(Language.TOT))]
        public TOTSalaryRange SalaryRange { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }
        public List<int> FilesId { get; set; }

    }

    public class DetailsTOTReportModel
    {
        public int Id { get; set; }
                
        [Display(Name = "FieldCourse", ResourceType = typeof(Language.TOT))]
        public string Course { get; set; }
               
        [Display(Name = "FieldModule", ResourceType = typeof(Language.TOT))]
        public string Module { get; set; }
                
        [Display(Name = "FieldStartDate", ResourceType = typeof(Language.TOT))]
        public DateTime? StartDate { get; set; }
                
        [Display(Name = "FieldEndDate", ResourceType = typeof(Language.TOT))]
        public DateTime? EndDate { get; set; }
                
        [Display(Name = "FieldVenue", ResourceType = typeof(Language.TOT))]
        public string Venue { get; set; }
                
        [Display(Name = "FieldNoOfMale", ResourceType = typeof(Language.TOT))]
        public int NoOfMale { get; set; }
                
        [Display(Name = "FieldNoOfMale", ResourceType = typeof(Language.TOT))]
        public int NoOfFemale { get; set; }
                
        [Display(Name = "FieldAgeRange", ResourceType = typeof(Language.TOT))]
        public TOTAgeRange AgeRange { get; set; }
               
        [Display(Name = "FieldSalaryRange", ResourceType = typeof(Language.TOT))]
        public TOTSalaryRange SalaryRange { get; set; }

        [Display(Name = "FieldAttachment", ResourceType = typeof(Language.TOT))]
        public IEnumerable<Attachment> Attachments { get; set; }

    }
}
