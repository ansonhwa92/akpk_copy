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

        public string WebMessage { get; set; }
        public string WebNotifyLink { get; set; }
        public bool enableWebMessage { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public NotificationCategory NotificationCategory { get; set; }

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

        [Display(Name = "Link")]
        Link = 4,

        [Display(Name = "Login Detail")]
        LoginDetail = 5,

        [Display(Name = "Error Details")]
        ErrorDetail = 6,

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

        //101-120 for Survey
        [Display(Name = "Survey Title")]
        SurveyTitle = 101,

        [Display(Name = "Survey Type")]
        SurveyType = 102,

        [Display(Name = "Survey Code")]
        SurveyCode = 103,

        [Display(Name = "Survey Approval")]
        SurveyApproval = 104,

        [Display(Name = "Survey Link")]
        SurveyLink = 105,

        [Display(Name = "Respondent Email")]
        SurveyRespondentEmail = 106,

        //121-140 for Publication
        [Display(Name = "Publication Title")]
        PublicationTitle = 121,

        [Display(Name = "Publication Author")]
        PublicationAuthor = 122,

        [Display(Name = "Publication Code")]
        PublicationCode = 123,

        [Display(Name = "Publication Approval")]
        PublicationApproval = 124,

        [Display(Name = "Refund Type")]
        RefundType = 131,

        [Display(Name = "Customer Name")]
        RefundFullName = 132,

        [Display(Name = "Reference/Receipt No.")]
        RefundReferenceNo = 133,

        [Display(Name = "Remarks")]
        RefundRemarks = 134,

        //141-160 for eLearning
        [Display(Name = "Course Title")]
        CourseTitle = 141,

        [Display(Name = "Course Author")]
        CourseAuthor = 142,

        [Display(Name = "Course Code")]
        CourseCode = 143,

        [Display(Name = "Course Approval")]
        CourseApproval = 144,

        [Display(Name = "Enrollment Code")]
        EnrollmentCode = 145,

        [Display(Name = "Student Name")]
        StudentName = 146,
    }
}