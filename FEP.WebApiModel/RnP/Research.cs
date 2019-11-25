using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System.Web;


namespace FEP.WebApiModel.RnP
{
    // class for returning survey local images
    public class SurveyImagesModel
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public string CoverPicture { get; set; }
        public string AuthorPicture { get; set; }
    }

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

        [Display(Name = "SurveyRequireLogin", ResourceType = typeof(Language.RnPForm))]
        public bool RequireLogin { get; set; }

        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }

        [Display(Name = "SurveyTemplateName", ResourceType = typeof(Language.RnPForm))]
        public string TemplateName { get; set; }

        [Display(Name = "SurveyTemplateDescription", ResourceType = typeof(Language.RnPForm))]
        public string TemplateDescription { get; set; }

        [Display(Name = "SurveyActive", ResourceType = typeof(Language.RnPForm))]
        public bool Active { get; set; }

        //[Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        //public string Pictures { get; set; }

        //[Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        //public string ProofOfApproval { get; set; }

        [Display(Name = "SurveyCancelRemark", ResourceType = typeof(Language.RnPForm))]
        public string CancelRemark { get; set; }

        [Display(Name = "SurveyDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }

        [Display(Name = "SurveyCreatorId", ResourceType = typeof(Language.RnPForm))]
        public int CreatorId { get; set; }

        [Display(Name = "SurveyCreatorId", ResourceType = typeof(Language.RnPForm))]
        public string CreatorName { get; set; }

        [Display(Name = "SurveyRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "SurveyStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyStatus Status { get; set; }

        [Display(Name = "SurveyDateCancelled", ResourceType = typeof(Language.RnPForm))]
        public DateTime? DateCancelled { get; set; }

        [Display(Name = "SurveyInviteCount", ResourceType = typeof(Language.RnPForm))]
        public int InviteCount { get; set; }

        [Display(Name = "SurveySubmitCount", ResourceType = typeof(Language.RnPForm))]
        public int SubmitCount { get; set; }

        [Display(Name = "SurveyDmsPath", ResourceType = typeof(Language.RnPForm))]
        public string DmsPath { get; set; }

        //public List<SurveyApproval> Approvals { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> CoverPictures { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> AuthorPictures { get; set; }

        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public string CoverPicture { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public string AuthorPicture { get; set; }
    }

    // Class for returning surveys for user browsing
    public class BrowseSurveyModel
    {
        public string Keyword { get; set; }

        public string Sorting { get; set; }

        public int LastIndex { get; set; }

        public int ItemCount { get; set; }

        public List<ReturnSurveyModel> Surveys { get; set; }
    }

    // class for returning just the survey design contents to client app
    // returned whenever the client requests for contents information on a single survey
    public class ReturnSurveyContentsModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

    // class for returning survey information as well as deisgn contents to client app
    public class ReturnFullSurveyModel
    {
        public ReturnSurveyModel Survey { get; set; }
        public ReturnSurveyContentsModel Contents { get; set; }
    }

    // class for returning just the auto-fields of survey information to client app
    // returned whenever the client requests for auto-field information on a single survey
    public class ReturnSurveyAutofieldsModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }

        [Display(Name = "SurveyCreatorId", ResourceType = typeof(Language.RnPForm))]
        public int CreatorId { get; set; }

        [Display(Name = "SurveyRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "SurveyStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyStatus Status { get; set; }

        [Display(Name = "SurveyDateCancelled", ResourceType = typeof(Language.RnPForm))]
        public DateTime? DateCancelled { get; set; }

        [Display(Name = "SurveyInviteCount", ResourceType = typeof(Language.RnPForm))]
        public int InviteCount { get; set; }

        [Display(Name = "SurveySubmitCount", ResourceType = typeof(Language.RnPForm))]
        public int SubmitCount { get; set; }

        [Display(Name = "SurveyDmsPath", ResourceType = typeof(Language.RnPForm))]
        public string DmsPath { get; set; }
    }

    // class for returning minimal survey information to client app for listing purposes
    // returned whenever the client requests for information on a list of surveys
    public class ReturnBriefSurveyModel
    {
        public int ID { get; set; }

        [Display(Name = "PubRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        public SurveyType? Type { get; set; }

        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime EndDate { get; set; }

        //[Display(Name = "SurveyDuration", ResourceType = typeof(Language.RnPForm))]
        public string Duration { get; set; }

        [Display(Name = "SurveyInviteCount", ResourceType = typeof(Language.RnPForm))]
        public int InviteCount { get; set; }

        [Display(Name = "SurveySubmitCount", ResourceType = typeof(Language.RnPForm))]
        public int SubmitCount { get; set; }

        //[Display(Name = "SurveyProgress", ResourceType = typeof(Language.RnPForm))]
        public string Progress { get; set; }

        [Display(Name = "SurveyStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyStatus Status { get; set; }

        public SurveyApprovalLevels? ApprovalLevel { get; set; }
    }

    // class for setting and returning filters for the datatable list of surveys
    public class FilterSurveyModel : DataTableModel
    {
        //[Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        //public SurveyType? Type { get; set; }

        //[Display(Name = "SurveyCategory", ResourceType = typeof(Language.RnPForm))]
        //public SurveyCategory Category { get; set; }

        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        //[Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        //public DateTime StartDate { get; set; }

        //[Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        //public DateTime EndDate { get; set; }

        // status filter(TODO)
    }

    // class for returning list of filtered survey information to client app (datatable)
    public class ReturnListSurveyModel
    {
        public FilterSurveyModel Filters { get; set; }
        public ReturnBriefSurveyModel Surveys { get; set; }
    }

    // class for returning survey approval history
    public class SurveyApprovalHistoryModel
    {
        //public IEnumerable<SurveyApproval> Event { get; set; }
        public SurveyApprovalLevels Level { get; set; }

        public int ApproverId { get; set; }

        public string UserName { get; set; }

        public SurveyApprovalStatus Status { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string Remarks { get; set; }
    }

    // class for returning unfilled survey approval information to client app
    //public class ReturnApprovalModel
    public class ReturnUpdateSurveyApprovalModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int SurveyID { get; set; }

        [Required]
        [Display(Name = "SurveyApprovalLevel", ResourceType = typeof(Language.RnPForm))]
        public SurveyApprovalLevels Level { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Range((int)(SurveyApprovalStatus.Approved), (int)(SurveyApprovalStatus.Rejected), ErrorMessageResourceName = "ValidInvalidSurveyApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyApprovalStatus Status { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "SurveyApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }
    }

    // class for returning survey information as well as approval form to client app
    // used to create form for reviewing (approving/rejecting) survey
    public class ReturnSurveyApprovalModel
    {
        public ReturnSurveyModel Survey { get; set; }
        public ReturnUpdateSurveyApprovalModel Approval { get; set; }
    }

    // class for updating of survey information by client app
    // used to create and edit survey information
    public class SurveyModel
    {
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
        //[DataType(DataType.Date, ErrorMessageResourceName = "ValidInvalidSurveyStartDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[UIHint("Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyEndDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        //[DataType(DataType.Date, ErrorMessageResourceName = "ValidInvalidSurveyEndDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyRequireLogin", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyRequireLogin", ResourceType = typeof(Language.RnPForm))]
        public bool RequireLogin { get; set; }

        ////[Required(ErrorMessageResourceName = "ValidRequiredSurveyPictures", ErrorMessageResourceType = typeof(Language.RnPForm))]
        //[Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        //public string Pictures { get; set; }

        ////[Required(ErrorMessageResourceName = "ValidRequiredSurveyProofOfApproval", ErrorMessageResourceType = typeof(Language.RnPForm))]
        //[Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        //public string ProofOfApproval { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public string CreatorName { get; set; }
    }

    // for creating
    public class CreateSurveyModel : SurveyModel
    {
        public CreateSurveyModel()
        {
            CoverPictures = new List<Attachment>();
            AuthorPictures = new List<Attachment>();
            ProofOfApproval = new List<Attachment>();
            CoverPictureFiles = new List<HttpPostedFileBase>();
            AuthorPictureFiles = new List<HttpPostedFileBase>();
            ProofOfApprovalFiles = new List<HttpPostedFileBase>();
            CoverFilesId = new List<int>();
            AuthorFilesId = new List<int>();
            ProofFilesId = new List<int>();
        }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> CoverPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> CoverPictureFiles { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> AuthorPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> AuthorPictureFiles { get; set; }

        [Required]
        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }
        public IEnumerable<HttpPostedFileBase> ProofOfApprovalFiles { get; set; }

        public List<int> CoverFilesId { get; set; }
        public List<int> AuthorFilesId { get; set; }
        public List<int> ProofFilesId { get; set; }
    }

    // for editing
    public class EditSurveyModel : SurveyModel
    {
        public EditSurveyModel()
        {
            CoverPictures = new List<Attachment>();
            AuthorPictures = new List<Attachment>();
            ProofOfApproval = new List<Attachment>();
            CoverPictureFiles = new List<HttpPostedFileBase>();
            AuthorPictureFiles = new List<HttpPostedFileBase>();
            ProofOfApprovalFiles = new List<HttpPostedFileBase>();
            CoverFilesId = new List<int>();
            AuthorFilesId = new List<int>();
            ProofFilesId = new List<int>();
        }

        [Required]
        public int ID { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> CoverPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> CoverPictureFiles { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> AuthorPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> AuthorPictureFiles { get; set; }

        [Required]
        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }
        public IEnumerable<HttpPostedFileBase> ProofOfApprovalFiles { get; set; }

        public List<int> CoverFilesId { get; set; }
        public List<int> AuthorFilesId { get; set; }
        public List<int> ProofFilesId { get; set; }
    }

    // for details
    public class DetailsSurveyModel : SurveyModel
    {
        public DetailsSurveyModel() { }

        [Required]
        public int ID { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> CoverPictures { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> AuthorPictures { get; set; }

        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public string CoverPicture { get; set; }

        [Display(Name = "SurveyAuthorPictures", ResourceType = typeof(Language.RnPForm))]
        public string AuthorPicture { get; set; }
    }

    // class for updating of survey information by client app
    // used to create and edit survey information
    public class SurveyModelNoFile
    {
        [Required(ErrorMessageResourceName = "ValidRequiredSurveyType", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public SurveyType? Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyCategory", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public SurveyCategory Category { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyTitle", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        public string TargetGroup { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyStartDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyEndDate", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyRequireLogin", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public bool RequireLogin { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public string CreatorName { get; set; }
    }

    // for creating
    public class CreateSurveyModelNoFile : SurveyModelNoFile
    {
        public CreateSurveyModelNoFile()
        {
            CoverPictures = new List<Attachment>();
            AuthorPictures = new List<Attachment>();
            ProofOfApproval = new List<Attachment>();
            CoverFilesId = new List<int>();
            AuthorFilesId = new List<int>();
            ProofFilesId = new List<int>();
        }

        public IEnumerable<Attachment> CoverPictures { get; set; }

        public IEnumerable<Attachment> AuthorPictures { get; set; }

        [Required]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }

        public List<int> CoverFilesId { get; set; }
        public List<int> AuthorFilesId { get; set; }
        public List<int> ProofFilesId { get; set; }
    }

    // for editing
    public class EditSurveyModelNoFile : SurveyModelNoFile
    {
        public EditSurveyModelNoFile()
        {
            CoverPictures = new List<Attachment>();
            AuthorPictures = new List<Attachment>();
            ProofOfApproval = new List<Attachment>();
            CoverFilesId = new List<int>();
            AuthorFilesId = new List<int>();
            ProofFilesId = new List<int>();
        }

        [Required]
        public int ID { get; set; }

        public IEnumerable<Attachment> CoverPictures { get; set; }

        public IEnumerable<Attachment> AuthorPictures { get; set; }

        [Required]
        public IEnumerable<Attachment> ProofOfApproval { get; set; }

        public List<int> CoverFilesId { get; set; }
        public List<int> AuthorFilesId { get; set; }
        public List<int> ProofFilesId { get; set; }
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

    // class for returning survey information for review and submission to client app
    // used to create form for reviewing survey before submission (admin only)
    public class UpdateSurveyReviewModel
    {
        public DetailsSurveyModel Survey { get; set; }
        public UpdateSurveyContentsModel Contents { get; set; }
    }

    // class for updating of survey template by client app
    // used to save (create/edit) survey question set as a template
    // templates are stored as normal surveys with the difference that the Template field stores the
    // template name and description
    public class UpdateSurveyTemplateModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyTemplateName", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyTemplateName", ResourceType = typeof(Language.RnPForm))]
        public string TemplateName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyTemplateDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyTemplateDescription", ResourceType = typeof(Language.RnPForm))]
        public string TemplateDescription { get; set; }
    }

    // class for updating of survey cancellation information by client app
    // used to edit survey information with cancellation remarks
    public class UpdateSurveyCancellationModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyCancellationRemark", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyCancelRemark", ResourceType = typeof(Language.RnPForm))]
        public string CancelRemark { get; set; }
    }

    // class for updating of survey start/end date information by client app
    // used to edit survey information with new start/end dates
    public class UpdateSurveyExtensionModel
    {
        [Required]
        public int ID { get; set; }

        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime NewStartDate { get; set; }

        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime NewEndDate { get; set; }
    }

    // class for returning survey information for submission as well as cancellation form to client app
    // used to create form for viewing survey (admin only)
    public class UpdateSurveyViewModel
    {
        public DetailsSurveyModel Survey { get; set; }
        public UpdateSurveyContentsModel Contents { get; set; }
        public ReturnSurveyAutofieldsModel Auto { get; set; }
        public UpdateSurveyCancellationModel Cancellation { get; set; }
        public UpdateSurveyExtensionModel Extension { get; set; }
    }

    /*
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
    */

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

        // has value if UserId = 0
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyResponseContents", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "SurveyResponseContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }

        public string Answers { get; set; }
    }

    // class for returning survey response preparation to client app
    // used to create form for answering survey (test & actual)
    public class ReturnSurveyResponseModel
    {
        public ReturnSurveyModel Survey { get; set; }
        public UpdateSurveyResponseModel Response { get; set; }
    }

    // class for returning survey response to client app
    public class SurveyResponseModel
    {
        public int ID { get; set; }

        public int SurveyID { get; set; }

        public SurveyResponseTypes Type { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        public string Contents { get; set; }

        public string Answers { get; set; }
    }

    // class for survey questions
    public class SurveyQuestion
    {
        public string Question;
    }

    public class SurveyQuestions
    {
        public List<SurveyQuestions> Questions { get; set; }
    }

    // class for survey answers

    // classes for eLearning

    // class for updating of quiz/assessment information by client app
    // used to create and edit quiz/assessment information
    public class AssessmentSurveyModel
    {
        [Required]
        public int AssessmentID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyContents", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "AssessmentContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

    // for creating
    public class CreateAssessmentSurveyModel : AssessmentSurveyModel
    {
        public CreateAssessmentSurveyModel() { }
    }

    // for editing
    public class EditAssessmentSurveyModel : AssessmentSurveyModel
    {
        public EditAssessmentSurveyModel() { }

        [Required]
        public int ID { get; set; }
    }

    // for details
    public class DetailsAssessmentSurveyModel : AssessmentSurveyModel
    {
        public DetailsAssessmentSurveyModel() { }

        public int ID { get; set; }
    }

    // responses
    public class AssessmentResponseModel
    {
        [Required]
        public int SurveyID { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "AssessmentResponseContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }

        public string Answers { get; set; }
    }

    // for creating
    public class CreateAssessmentResponseModel : AssessmentResponseModel
    {
        public CreateAssessmentResponseModel() { }
    }

    // for view
    public class ViewAssessmentResponseModel : AssessmentResponseModel
    {
        public ViewAssessmentResponseModel() { }

        public int ID { get; set; }
    }

    // classes for eLearning/eEvent feedbacks

    // class for updating of feedback information by client app
    // used to create and edit feedback information
    public class FeedbackSurveyModel
    {
        [Required]
        public int FeedbackID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSurveyContents", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "FeedbackContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

    // for creating
    public class CreateFeedbackSurveyModel : FeedbackSurveyModel
    {
        public CreateFeedbackSurveyModel() { }
    }

    // for editing
    public class EditFeedbackSurveyModel : FeedbackSurveyModel
    {
        public EditFeedbackSurveyModel() { }

        [Required]
        public int ID { get; set; }
    }

    // for details
    public class DetailsFeedbackSurveyModel : FeedbackSurveyModel
    {
        public DetailsFeedbackSurveyModel() { }

        [Required]
        public int ID { get; set; }
    }

    // responses
    public class FeedbackResponseModel
    {
        [Required]
        public int SurveyID { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "FeedbackResponseContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }

        public string Answers { get; set; }
    }

    // for creating
    public class CreateFeedbackResponseModel : FeedbackResponseModel
    {
        public CreateFeedbackResponseModel() { }
    }

    // for view
    public class ViewFeedbackResponseModel : FeedbackResponseModel
    {
        public ViewFeedbackResponseModel() { }

        public int ID { get; set; }
    }

    // Class for returning selected survey info
    public class ReturnSurveyModelNoFile
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

        [Display(Name = "SurveyRequireLogin", ResourceType = typeof(Language.RnPForm))]
        public bool RequireLogin { get; set; }

        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }

        [Display(Name = "SurveyActive", ResourceType = typeof(Language.RnPForm))]
        public bool Active { get; set; }

        [Display(Name = "SurveyDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }

        [Display(Name = "SurveyCreatorId", ResourceType = typeof(Language.RnPForm))]
        public int CreatorId { get; set; }

        [Display(Name = "SurveyCreatorId", ResourceType = typeof(Language.RnPForm))]
        public string CreatorName { get; set; }

        [Display(Name = "SurveyRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "SurveyStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyStatus Status { get; set; }

        [Display(Name = "SurveyInviteCount", ResourceType = typeof(Language.RnPForm))]
        public int InviteCount { get; set; }

        [Display(Name = "SurveySubmitCount", ResourceType = typeof(Language.RnPForm))]
        public int SubmitCount { get; set; }
    }

    // Class for processing/viewing survey results
    public class SurveyResultsModel : ReturnSurveyModelNoFile
    {
        public SurveyResultsModel()
        {
            Questions = new List<string>();
            Answers = new List<List<string>>();
            SingleChoices = new List<SurveySingleChoiceAnswersModel>();
            MultipleChoices = new List<SurveyMultipleChoiceAnswersModel>();
        }

        public string RawQuestions { get; set; }

        public List<string> Questions { get; set; }

        public int ParticipantCount { get; set; }

        public string RawResults { get; set; }

        public string RawAnswers { get; set; }

        public List<List<string>> Answers { get; set; }

        public List<SurveySingleChoiceAnswersModel> SingleChoices { get; set; }

        public List<SurveyMultipleChoiceAnswersModel> MultipleChoices { get; set; }

        public string RawOutput { get; set; }

        public string CSVOutput { get; set; }

        public string XLSOutput { get; set; }

        public string PDFOutput { get; set; }
    }

    // Answers
    public class SurveyAnswersModel
    {
        public string surveyId { get; set; }

        public string question { get; set; }

        public dynamic answer { get; set; }

        public string questionType { get; set; }
    }

    // Single-choice answers
    public class SurveySingleChoiceAnswersModel
    {
        public SurveySingleChoiceAnswersModel()
        {
            maxrating = 0;
            answers = new List<string>();
            values = new List<string>();
            counts = new List<int>();
        }

        public string question { get; set; }

        public string questionname { get; set; }

        public string questiontype { get; set; }

        public int maxrating { get; set; }

        public int mean { get; set; }

        public float median { get; set; }

        public int mode { get; set; }

        public List<string> answers { get; set; }

        public List<string> values { get; set; }

        public List<int> counts { get; set; }
    }

    // Multiple-choice answers
    public class SurveyMultipleChoiceAnswersModel
    {
        public SurveyMultipleChoiceAnswersModel()
        {
            maxrating = 0;
            answers = new List<string>();
            values = new List<string>();
            counts = new List<int>();
        }

        public string question { get; set; }

        public string questionname { get; set; }

        public string questiontype { get; set; }

        public int maxrating { get; set; }

        public int mean { get; set; }

        public float median { get; set; }

        public int mode { get; set; }

        public List<string> answers { get; set; }

        public List<string> values { get; set; }

        public List<int> counts { get; set; }
    }

    // Questions
    public class SurveyElementsModel
    {
        public string type { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public dynamic choices { get; set; }

        public dynamic columns { get; set; }

        public dynamic rows { get; set; }

        public dynamic items { get; set; }
    }

    // Choices
    public class SurveyElementsChoicesModel
    {
        public string value { get; set; }

        public string text { get; set; }
    }

    // Image Choices
    public class SurveyElementsImageChoicesModel
    {
        public string value { get; set; }

        public string imageLink { get; set; }
    }

    // Pages
    public class SurveyPagesModel
    {
        public string name { get; set; }

        public List<SurveyElementsModel> elements { get; set; }
    }

    // Certain question types
    public class SurveyCheckboxModel
    {
        public SurveyCheckboxModel()
        {
            items = new List<string>();
        }

        public List<string> items { get; set; }
    }

    //public class SurveyMatrixDropdownColModel
    //{
    //    public dynamic column { get; set; }
    //}

    //public class SurveyMatrixDropdownRowModel
    //{
    //    public dynamic List<SurveyMatrixDropdownColModel>
    //}
}
