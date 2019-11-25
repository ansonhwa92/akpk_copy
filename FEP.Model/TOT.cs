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
        public string Module { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Venue { get; set; }
        public int NoOfParticipant { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }  

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
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
    
}
