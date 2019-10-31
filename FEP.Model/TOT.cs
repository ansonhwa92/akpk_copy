using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("TOTReport")]
    public class TOTReport
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }

        public int ModuleId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Venue { get; set; }
        public int NoOfParticipant { get; set; }
        public int NoOfMale { get; set; }
        public int NoOfFemale { get; set; }
        public TOTAgeRange AgeRange { get; set; }
        public TOTSalaryRange SalaryRange { get; set; }                       
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }   
        
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } 

        [ForeignKey("ModuleId")]
        public virtual CourseModule Module { get; set; }
    }

    [Table("TOTReportFile")]
    public class TOTReportFile
    {
        [Key]
        public int Id { get; set; }

        public int TOTReportId { get; set; }

        public int FileId { get; set; }
        
        [ForeignKey("TOTReportId")]
        public virtual TOTReport Report { get; set; }

        [ForeignKey("FileId")]
        public virtual FileDocument FileDocument { get; set; }

    }


    public enum TOTAgeRange
    {
        [Display(Name = "20 - 30")]
        R1,
        [Display(Name = "31 - 40")]
        R2,
        [Display(Name = "41 - 50")]
        R3,
        [Display(Name = "51 - 60")]
        R4,
        [Display(Name = "60 and above")]
        R5
    }

    public enum TOTSalaryRange
    {
        [Display(Name = "1000 - 2000")]
        R1,
        [Display(Name = "2001 - 4000")]
        R2,
        [Display(Name = "4001 - 6000")]
        R3,
        [Display(Name = "6001 - 8000")]
        R4,
        [Display(Name = "8001 - 10000")]
        R5,
        [Display(Name = "10001 and above")]
        R6
    }
}
