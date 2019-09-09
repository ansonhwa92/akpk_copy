using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        public int Id { get; set; }

        public NotificationType NotificationType { get; set; }

        public string TemplateName { get; set; }

        public string TemplateSubject { get; set; }

        public string TemplateRefNo { get; set; }

        public string TemplateMessage { get; set; }

        public string SMSMessage { get; set; }
        public bool enableSMSMessage { get; set; }

        public string WebMessage { get;set; }
        public bool enableWebMessage { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        

        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }  
        public bool Display { get; set; }
                
    }
    
    [Table("TemplateParameters")]
    public class TemplateParameters
    {
        [Key]
        public int Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public TemplateParameterType TemplateParameterType { get; set; }
    }

    
    
    

    public enum TemplateParameterType
    {
        //1-100 for system
        [Display(Name = "Friendly Site Name")]
        FriendlySiteName = 1,

        //101-200 for User
        [Display(Name = "User Full Name")]
        UserFullName = 101,
        [Display(Name = "User's Role")]
        UserRole = 102,

        //201-300 for eLearning
        [Display(Name = "Application Number")]
        ApplicationNo = 201,
        [Display(Name = "Course Name")]
        CourseName = 202,
    }


}
