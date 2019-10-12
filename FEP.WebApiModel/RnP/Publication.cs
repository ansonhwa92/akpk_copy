using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;

namespace FEP.WebApiModel.RnP
{
    // class for returning publication category
    public class ReturnPublicationCategory
    {
        public int ID { get; set; }
        [Display(Name = "PubCategoryNameList", ResourceType = typeof(Language.RnPForm))]
        public string Name { get; set; }
    }

    // class for returning publication information to client app
    // returned whenever the client requests for information on a single publication
    public class ReturnPublicationModel
    {
        public int ID { get; set; }

        [Display(Name = "PubCategoryID", ResourceType = typeof(Language.RnPForm))]
        public int CategoryID { get; set; }

        [Display(Name = "PubAuthor", ResourceType = typeof(Language.RnPForm))]
        public string Author { get; set; }

        [Display(Name = "PubCoauthor", ResourceType = typeof(Language.RnPForm))]
        public string Coauthor { get; set; }

        [Display(Name = "PubTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Display(Name = "PubYear", ResourceType = typeof(Language.RnPForm))]
        public int? Year { get; set; }

        [Display(Name = "PubDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        [Display(Name = "PubLanguage", ResourceType = typeof(Language.RnPForm))]
        public string Language { get; set; }

        [Display(Name = "PubISBN", ResourceType = typeof(Language.RnPForm))]
        public string ISBN { get; set; }

        [Display(Name = "PubHardcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Hardcopy { get; set; }

        [Display(Name = "PubDigitalcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Digitalcopy { get; set; }

        [Display(Name = "PubHDcopy", ResourceType = typeof(Language.RnPForm))]
        public bool HDcopy { get; set; }

        [Display(Name = "PubFreeHCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeHCopy { get; set; }

        [Display(Name = "PubFreeDCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeDCopy { get; set; }

        [Display(Name = "PubFreeHDCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeHDCopy { get; set; }

        [Display(Name = "PubHPrice", ResourceType = typeof(Language.RnPForm))]
        public float HPrice { get; set; }

        [Display(Name = "PubDPrice", ResourceType = typeof(Language.RnPForm))]
        public float DPrice { get; set; }

        [Display(Name = "PubHDPrice", ResourceType = typeof(Language.RnPForm))]
        public float HDPrice { get; set; }

        [Display(Name = "PubPictures", ResourceType = typeof(Language.RnPForm))]
        public string Pictures { get; set; }

        [Display(Name = "PubProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }

        [Display(Name = "PubStockBalance", ResourceType = typeof(Language.RnPForm))]
        public int? StockBalance { get; set; }

        [Display(Name = "PubCancelRemark", ResourceType = typeof(Language.RnPForm))]
        public string CancelRemark { get; set; }

        [Display(Name = "PubWithdrawalReason", ResourceType = typeof(Language.RnPForm))]
        public string WithdrawalReason { get; set; }

        [Display(Name = "PubWithdrawalDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime? WithdrawalDate { get; set; }

        [Display(Name = "PubProofOfWithdrawal", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfWithdrawal { get; set; }

        [Display(Name = "PubDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }

        [Display(Name = "PubCreatorId", ResourceType = typeof(Language.RnPForm))]
        public int CreatorId { get; set; }

        [Display(Name = "PubCreatorId", ResourceType = typeof(Language.RnPForm))]
        public string CreatorName { get; set; }

        [Display(Name = "PubRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "PubStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationStatus Status { get; set; }

        [Display(Name = "PubDateCancelled", ResourceType = typeof(Language.RnPForm))]
        public DateTime? DateCancelled { get; set; }

        [Display(Name = "PubViewCount", ResourceType = typeof(Language.RnPForm))]
        public int ViewCount { get; set; }

        [Display(Name = "PubPurchaseCount", ResourceType = typeof(Language.RnPForm))]
        public int PurchaseCount { get; set; }

        [Display(Name = "PubDmsPath", ResourceType = typeof(Language.RnPForm))]
        public string DmsPath { get; set; }

        //public List<PublicationApproval> Approvals { get; set; }
        //public List<PublicationWithdrawal> Withdrawals { get; set; }

        [Display(Name = "PubCategoryNameList", ResourceType = typeof(Language.RnPForm))]
        public string Category { get; set; }
    }

    // class for returning just the auto-fields of publication information to client app
    // returned whenever the client requests for auto-field information on a single publication
    public class ReturnPublicationAutofieldsModel
    {
        public int ID { get; set; }

        [Display(Name = "PubWithdrawalDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime? WithdrawalDate { get; set; }

        [Display(Name = "PubDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }

        [Display(Name = "PubCreatorId", ResourceType = typeof(Language.RnPForm))]
        public int CreatorId { get; set; }

        [Display(Name = "PubRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "PubStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationStatus Status { get; set; }

        [Display(Name = "PubDateCancelled", ResourceType = typeof(Language.RnPForm))]
        public DateTime? DateCancelled { get; set; }

        [Display(Name = "PubViewCount", ResourceType = typeof(Language.RnPForm))]
        public int ViewCount { get; set; }

        [Display(Name = "PubPurchaseCount", ResourceType = typeof(Language.RnPForm))]
        public int PurchaseCount { get; set; }

        [Display(Name = "PubDmsPath", ResourceType = typeof(Language.RnPForm))]
        public string DmsPath { get; set; }
    }

    // class for returning minimal publication information to client app for listing purposes
    // returned whenever the client requests for information on a list of publications
    public class ReturnBriefPublicationModel
    {
        public int ID { get; set; }

        //[Display(Name = "PubCategoryID", ResourceType = typeof(Language.RnPForm))]
        //public int CategoryID { get; set; }

        [Display(Name = "PubRefNo", ResourceType = typeof(Language.RnPForm))]
        public string RefNo { get; set; }

        [Display(Name = "PubAuthor", ResourceType = typeof(Language.RnPForm))]
        public string Author { get; set; }

        [Display(Name = "PubTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        //[Display(Name = "PubYear", ResourceType = typeof(Language.RnPForm))]
        //public int? Year { get; set; }

        [Display(Name = "PubISBN", ResourceType = typeof(Language.RnPForm))]
        public string ISBN { get; set; }

        [Display(Name = "PubStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationStatus Status { get; set; }

        [Display(Name = "PubCategoryNameList", ResourceType = typeof(Language.RnPForm))]
        public string Category { get; set; }
    }

    // class for setting and returning filters for the datatable list of publications
    public class FilterPublicationModel : DataTableModel
    {
        //[Display(Name = "PubCategoryNameList", ResourceType = typeof(Language.RnPForm))]
        //public string Type { get; set; }

        [Display(Name = "PubAuthor", ResourceType = typeof(Language.RnPForm))]
        public string Author { get; set; }

        [Display(Name = "PubTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Display(Name = "PubISBN", ResourceType = typeof(Language.RnPForm))]
        public string ISBN { get; set; }

        // status filter(TODO)
    }

    // class for returning list of filtered publication information to client app (datatable)
    public class ReturnListPublicationModel
    {
        public FilterPublicationModel Filters { get; set; }
        public ReturnBriefPublicationModel Pubs { get; set; }
    }

    // class for returning publication approval history
    public class PublicationApprovalHistoryModel
    {
        //public IEnumerable<PublicationApproval> Event { get; set; }
        public PublicationApprovalLevels Level { get; set; }

        public int ApproverId { get; set; }

        public string UserName { get; set; }

        public PublicationApprovalStatus Status { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string Remarks { get; set; }
    }

    // class for returning publication withdrawal approval history
    public class PublicationWithdrawalHistoryModel
    {
        //public IEnumerable<PublicationApproval> Event { get; set; }
        public PublicationApprovalLevels Level { get; set; }

        public int ApproverId { get; set; }

        public string UserName { get; set; }

        public PublicationApprovalStatus Status { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string Remarks { get; set; }
    }

    // class for returning unfilled publication approval information to client app
    //public class ReturnApprovalModel
    public class ReturnUpdatePublicationApprovalModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int PublicationID { get; set; }

        [Required]
        [Display(Name = "PubApprovalLevel", ResourceType = typeof(Language.RnPForm))]
        public PublicationApprovalLevels Level { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Range((int)(PublicationApprovalStatus.Approved), (int)(PublicationApprovalStatus.Rejected), ErrorMessageResourceName = "ValidInvalidApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationApprovalStatus Status { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "PubApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }
    }

    // class for returning publication information as well as approval form to client app
    // used to create form for reviewing (approving/rejecting) publication
    public class ReturnPublicationApprovalModel
    {
        public ReturnPublicationModel Pub { get; set; }
        public ReturnUpdatePublicationApprovalModel Approval { get; set; }
        //public ReturnApprovalModel Approval { get; set; }
        //public UpdatePublicationApprovalModel Review { get; set; }
    }

    // class for updating of publication information by client app
    // used to create and edit publication information
    public class UpdatePublicationModel
    {
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCategory", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubCategoryID", ResourceType = typeof(Language.RnPForm))]
        public int CategoryID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAuthor", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubAuthor", ResourceType = typeof(Language.RnPForm))]
        public string Author { get; set; }

        [Display(Name = "PubCoauthor", ResourceType = typeof(Language.RnPForm))]
        public string Coauthor { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredYear", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubYear", ResourceType = typeof(Language.RnPForm))]
        //[MaxLength(4)]
        //[MinLength(4)]
        //[Range(1900, 3000, ErrorMessage = "Please enter a valid year")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public int? Year { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredLanguage", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubLanguage", ResourceType = typeof(Language.RnPForm))]
        public string Language { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredISBN", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubISBN", ResourceType = typeof(Language.RnPForm))]
        public string ISBN { get; set; }

        //[Display(Name = "PubFree", ResourceType = typeof(Language.RnPForm))]
        //public bool Free { get; set; }

        [Display(Name = "PubHardcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Hardcopy { get; set; }

        [Display(Name = "PubDigitalcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Digitalcopy { get; set; }

        [Display(Name = "PubHDcopy", ResourceType = typeof(Language.RnPForm))]
        public bool HDcopy { get; set; }

        [Display(Name = "PubFreeHCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeHCopy { get; set; }

        [Display(Name = "PubFreeDCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeDCopy { get; set; }

        [Display(Name = "PubFreeHDCopy", ResourceType = typeof(Language.RnPForm))]
        public bool FreeHDCopy { get; set; }

        [Display(Name = "PubHPrice", ResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float HPrice { get; set; }

        [Display(Name = "PubDPrice", ResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float DPrice { get; set; }

        [Display(Name = "PubHDPrice", ResourceType = typeof(Language.RnPForm))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float HDPrice { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredPictures", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubPictures", ResourceType = typeof(Language.RnPForm))]
        public string Pictures { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredProofOfApproval", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }         // uploaded proof of approval

        [Required(ErrorMessageResourceName = "ValidRequiredStockBalance", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubStockBalance", ResourceType = typeof(Language.RnPForm))]
        //[MaxLength(4)]
        //[MinLength(1)]
        //[Range(0, 9999, ErrorMessage = "Please enter a valid year")]
        public int? StockBalance { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public string CreatorName { get; set; }
    }

    // class for updating of publication cancellation information by client app
    // used to edit publication information with cancellation remarks
    public class UpdatePublicationCancellationModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCancellationRemark", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubCancelRemark", ResourceType = typeof(Language.RnPForm))]
        public string CancelRemark { get; set; }
    }

    // class for updating of publication withdrawal information by client app
    // used to create and edit publication withdrawal information
    public class UpdatePublicationWithdrawalModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredWithdrawalReason", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubWithdrawalReason", ResourceType = typeof(Language.RnPForm))]
        public string WithdrawalReason { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredProofOfWithdrawal", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubProofOfWithdrawal", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfWithdrawal { get; set; }
    }

    // class for returning publication information for submission as well as cancellation form to client app
    // used to create form for viewing publication (admin only)
    public class UpdatePublicationViewModel
    {
        public UpdatePublicationModel Pub { get; set; }
        public ReturnPublicationAutofieldsModel Auto { get; set; }
        public UpdatePublicationWithdrawalModel Withdrawal { get; set; }
        public UpdatePublicationCancellationModel Cancellation { get; set; }
    }

    // class for returning publication information, withdrawal information, as well as withdrawal approval form to client app
    // used to create form for reviewing (approving/rejecting) publication withdrawal
    public class ReturnPublicationWithdrawalModel
    {
        public ReturnPublicationModel Pub { get; set; }
        public UpdatePublicationWithdrawalModel Withdrawal { get; set; }
        public ReturnUpdatePublicationApprovalModel Approval { get; set; }
    }

    // class for returning publication featured/rank information
    public class ReturnPublicationRank
    {
        public int ID { get; set; }

        public int PublicationID { get; set; }

        [Display(Name = "PubRankPosition", ResourceType = typeof(Language.RnPForm))]
        public int? Position { get; set; }
    }

    // class for updating publication featured/rank information
    public class UpdatePublicationRank
    {
        public int ID { get; set; }

        [Required]
        public int PublicationID { get; set; }

        [Display(Name = "PubRankPosition", ResourceType = typeof(Language.RnPForm))]
        public int? Position { get; set; }
    }

    // class for returning publication review information
    public class ReturnPublicationReview
    {
        public int ID { get; set; }

        public int PublicationID { get; set; }

        [Display(Name = "PubReviewRating", ResourceType = typeof(Language.RnPForm))]
        public int Rating { get; set; }

        [Display(Name = "PubReviewReviewerId", ResourceType = typeof(Language.RnPForm))]
        public int? ReviewerId { get; set; }

        [Display(Name = "PubReviewReviewerName", ResourceType = typeof(Language.RnPForm))]
        public string ReviewerName { get; set; }

        [Display(Name = "PubReviewReviewDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime? ReviewDate { get; set; }

        [Display(Name = "PubReviewRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }
    }

    // class for updating publication review information
    public class UpdatePublicationReview
    {
        public int ID { get; set; }

        [Required]
        public int PublicationID { get; set; }

        [Required]
        [Display(Name = "PubReviewRating", ResourceType = typeof(Language.RnPForm))]
        public int Rating { get; set; }

        [Display(Name = "PubReviewReviewerId", ResourceType = typeof(Language.RnPForm))]
        public int? ReviewerId { get; set; }

        [Required]
        [Display(Name = "PubReviewReviewerName", ResourceType = typeof(Language.RnPForm))]
        public string ReviewerName { get; set; }

        [Display(Name = "PubReviewReviewDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime? ReviewDate { get; set; }

        [Display(Name = "PubReviewRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }
    }

    /*
    // class for creation of publication approval record by client app
    // used to create approval record for later filling in by approvers
    // this function is called appropriately depending on approval stage, and is not apparent to user so
    // no user-accessible error messages are necessary
    public class CreatePublicationApprovalModel
    {
        [Required]
        public int PublicationID { get; set; }

        [Required]
        public PublicationApprovalLevels Level { get; set; }

        // always None at this point
        [Required]
        public PublicationApprovalStatus Status { get; set; }
    }
    */

    /*
    // class for updating of publication approval by client app
    // used to update approval record with approver's remarks
    // approver ID is automatic based on who's doing the review
    public class UpdatePublicationApprovalModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationApprovalStatus Status { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "PubApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }
    }
    */

    /*
    // class for creation of publication withdrawal approval record by client app
    // used to create approval record for later filling in by approvers
    // this function is called appropriately depending on approval stage, and is not apparent to user so
    // no user-accessible error messages are necessary
    public class CreatePublicationWithdrawalApprovalModel
    {
        [Required]
        public int SurveyID { get; set; }

        [Required]
        public PublicationApprovalLevels Level { get; set; }

        // always None at this point
        [Required]
        public PublicationWithdrawalStatus Status { get; set; }
    }
    */

    // class for updating of publication withdrawal approval by client app
    // used to update withdrawal approval record with approver's remarks
    // approver ID is automatic based on who's doing the review
    public class UpdatePublicationWithdrawalApprovalModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredWithdrawalApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubWithdrawalApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        [Range((int)(PublicationApprovalStatus.Approved), (int)(PublicationApprovalStatus.Rejected), ErrorMessageResourceName = "ValidInvalidApprovalStatus", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public PublicationApprovalStatus Status { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "PubApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }
    }

    // class for returning/updating delivery address for publication ordering
    public class PublicationDeliveryModel
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Street Address 1")]
        [Required]
        public string Address1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string Postcode { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        public DeliveryStates State { get; set; }

        [Display(Name = "Contact No.")]
        [Required]
        public string PhoneNumber { get; set; }
    }

    // class for purchase publication page - contains delivery address and a single publication
    // in up to 3 formats
    public class PurchasePublicationModel
    {
        public int PublicationID { get; set; }

        public bool FormatDigital { get; set; }

        public bool FormatHardcopy { get; set; }

        public bool FormatPromotion { get; set; }

        public int HardcopyQuantity { get; set; }

        public int DeliveryID { get; set; }

        public int UserId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Street Address 1")]
        [Required]
        public string Address1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string Postcode { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        public DeliveryStates State { get; set; }

        [Display(Name = "Contact No.")]
        [Required]
        public string PhoneNumber { get; set; }
    }

    // class for updating delivery address only
    public class EditDeliveryAddressModel
    {
        public int DeliveryID { get; set; }

        public int UserId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Street Address 1")]
        [Required]
        public string Address1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string Postcode { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        public DeliveryStates State { get; set; }

        [Display(Name = "Contact No.")]
        [Required]
        public string PhoneNumber { get; set; }
    }

    // class for updating delivery information + adding item to order - NOT USED?
    public class UpdatePublicationDeliveryModel
    {
        public List<PublicationPurchaseItemModel> Items { get; set; }
        public PublicationDeliveryModel Delivery { get; set; }
    }

    // class for returning/updating publication purchase item
    // filled in by system
    public class PublicationPurchaseItemModel
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public int? PurchaseOrderId { get; set; }

        [Display(Name = "Publication Title")]
        [Required]
        public int PublicationID { get; set; }

        [Display(Name = "Format")]
        [Required]
        public PublicationFormats Format { get; set; }

        [Display(Name = "Unit Price")]
        [Required]
        public float Price { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public int Quantity { get; set; }
    }

    // class for returning/updating promotion code
    public class PromotionCodeModel
    {
        public int ID { get; set; }

        [Display(Name = "Code")]
        [Required]
        public string Code { get; set; }

        [Display(Name = "Monetary Value (RM)")]
        [Required]
        public int MoneyValue { get; set; }

        [Display(Name = "Percentage Value")]
        [Required]
        public int PercentageValue { get; set; }

        [Display(Name = "Expiry Date")]
        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; }
    }

}
