using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("NotificationTemplate")]
    public class NotificationTemplate
    {
        [Key]
        public int Id { get; set; }

        public NotificationType NotificationType { get; set; }

        public string TemplateName { get; set; }

        public string TemplateSubject { get; set; }

        public string TemplateRefNo { get; set; }

        public string TemplateMessage { get; set; }
        public bool enableEmail { get; set; }


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
        public string TemplateParameterType { get; set; }
    }

    [Table("ParameterGroup")]
    public class ParameterGroup
    {
        [Key]
        public int Id { get; set; }
        public SLAEventType SLAEventType { get; set; }
        public TemplateParameterType TemplateParameterType { get; set; }
    }    
    

    public enum TemplateParameterType
    {
        //1-20 for System
        [Display(Name = "Friendly Site Name")]
        FriendlySiteName = 1,
        [Display(Name = "User Full Name")]
        UserFullName = 2,
        [Display(Name = "User's Role")]
        UserRole = 3,

        //21-40 for Verify/Approval Public Event
        [Display(Name = "Event Name")]
        EventName = 21,
        [Display(Name = "Event Location")]
        EventLocation = 22,
        [Display(Name = "Event Code")]
        EventCode = 23,
        [Display(Name = "Event Approval")]
        EventApproval = 24,

        //41-60 for Payment
        [Display(Name = "GL Holder")]
        GLHolderName = 41,
        [Display(Name = "Payment Ref No")]
        PaymentRefNo = 42,
        [Display(Name = "Payment Status")]
        PaymentStatus = 43,



    }


}
