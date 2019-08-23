using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using FEP.Model;

namespace FEP.WebApiModel.RnP
{
    // class for returning survey information to client app
    // returned whenever the client requests for information on a single survey
    public class ReturnSurveyModel
    {
        public int ID { get; set; }
        [Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        public SurveyType? Type { get; set; }
        [Display(Name = "SurveyCategory", ResourceType = typeof(Language.RnPForm))]
        public SurveyCategory Category { get; set; }
        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }
        [Display(Name = "SurveyDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }
        [Display(Name = "SurveyTargetGroup", ResourceType = typeof(Language.RnPForm))]
        public string TargetGroup { get; set; }
        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime EndDate { get; set; }
        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
        [Display(Name = "SurveyTemplate", ResourceType = typeof(Language.RnPForm))]
        public string Template { get; set; }
        [Display(Name = "SurveyActive", ResourceType = typeof(Language.RnPForm))]
        public bool Active { get; set; }
        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public string Pictures { get; set; }
        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }
        public DateTime DateAdded { get; set; }
        [Display(Name = "SurveyStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyStatus Status { get; set; }
        //public List<SurveyApproval> Approvals { get; set; }
    }

    /*
    // class for returning survey list filter information to client app
    public class ReturnSurveyFilterModel
    {
        public SurveyType Type { get; set; }
        public SurveyCategory Category { get; set; }
        public string Title { get; set; }
        public string TargetGroup { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
    }

    // class for returning list of survey information to client app
    public class ReturnListSurveyModel
    {
        List<ReturnSurveyFilterModel> Filters { get; set; }
        List<ReturnSurveyModel> Surveys { get; set; }
    }
    */

    // class for updating of survey information by client app
    // used to create and edit survey information
    public class UpdateSurveyModel
    {
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyType", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        public SurveyType? Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyCategory", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyCategory", ResourceType = typeof(Language.RnPForm))]
        public SurveyCategory Category { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyTitle", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        [Display(Name = "SurveyTargetGroup", ResourceType = typeof(Language.RnPForm))]
        public string TargetGroup { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyStartDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date, ErrorMessageResourceName = "ValidInvalidSurveyStartDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]    //, ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyEndDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date, ErrorMessageResourceName = "ValidInvalidSurveyEndDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]    //, ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredSurveyPictures", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public string Pictures { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredSurveyProofOfApproval", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }
    }

    // class for updating of survey questions by client app
    // used to create and update survey questions
    public class UpdateSurveyContentsModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyContents", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

    // class for updating of survey template by client app
    // used to save (create/edit) survey question set as a template
    // templates are stored as normal surveys with the difference that the Template field stores the
    // template name
    public class UpdateSurveyTemplateModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyTemplate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyTemplate", ResourceType = typeof(Language.RnPForm))]
        public string Template { get; set; }
    }

    // class for creation of survey approval record by client app
    // used to create approval record for later filling in by approvers
    // this function is called appropriately depending on approval stage, and is not apparent to user so
    // no user-accessible error messages are necessary
    public class CreateSurveyApprovalModel
    {
        [Required]
        public int SurveyID { get; set; }

        [Required]
        public SurveyApprovalLevels Level { get; set; }

        // always None at this point
        [Required]
        public SurveyApprovalStatus Status { get; set; }
    }

    // class for updating of survey approval by client app
    // used to update approval record with approver's remarks
    // approver ID is automatic based on who's doing the review
    public class UpdateSurveyApprovalModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyApprovalStatus Status { get; set; }

        // might be automatic (right now comes from a disabled date field anyway)
        //[Required(ErrorMessageResourceName = "ValidRequiredSurveyApprovalApprovalDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        //[Display(Name = "SurveyApprovalApprovalDate", ResourceType = typeof(Language.RnPForm))]
        //public DateTime ApprovalDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyApprovalRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "SurveyApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }
    }

    // class for updating of survey response by client app
    // used to create and edit survey response for both actual response and for test-run
    public class UpdateSurveyResponseModel
    {
        public int ID { get; set; }

        [Required]
        public int SurveyID { get; set; }

        [Required]
        public SurveyResponseTypes Type { get; set; }

        // can have the value 0 for public surveys
        [Required]
        public int UserId { get; set; }

        // automatic date
        //[Required(ErrorMessageResourceName = "ValidRequiredSurveyResponseResponseDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        //[Display(Name = "SurveyResponseResponseDate", ResourceType = typeof(Language.RnPForm))]
        //public DateTime ResponseDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyResponseContents", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyResponseContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }
}
