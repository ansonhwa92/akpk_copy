using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace FEP.Model
{
    [Table("PublicationCategory")]
    public class PublicationCategory
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Table("Publication")]
    public class Publication
    {
        [Key]
        public int ID { get; set; }
        // publication info..................................................................................................
        public int CategoryID { get; set; }
        public string Author { get; set; }
        public string Coauthor { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public bool Hardcopy { get; set; }
        public bool Digitalcopy { get; set; }
        public bool HDcopy { get; set; }
        public bool FreeHCopy { get; set; }
        public bool FreeDCopy { get; set; }
        public bool FreeHDCopy { get; set; }
        public float HPrice { get; set; }
        public float DPrice { get; set; }
        public float HDPrice { get; set; }
        public string Pictures { get; set; }
        public string ProofOfApproval { get; set; }
        public int? StockBalance { get; set; }
        public string CancelRemark { get; set; }
        // withdrawal info...................................................................................................
        public string WithdrawalReason { get; set; }
        public string ProofOfWithdrawal { get; set; }
        // auto-filled in data...............................................................................................
        public DateTime DateAdded { get; set; }
        public int CreatorId { get; set; }
        public string RefNo { get; set; }
        public PublicationStatus Status { get; set; }
        public DateTime? DateCancelled { get; set; }
        //public PublicationWithdrawalStatus WStatus { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        // DMS integration (TODO)............................................................................................
        public string DmsPath { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("CategoryID")]
        public virtual PublicationCategory Category { get; set; }
        // sub-tables........................................................................................................
        public virtual ICollection<PublicationApproval> Approvals { get; set; }
        public virtual ICollection<PublicationWithdrawal> Withdrawals { get; set; }
    }

    [Table("PublicationApproval")]
    public class PublicationApproval
    {
        [Key]
        public int ID { get; set; }
        public int PublicationID { get; set; }
        public PublicationApprovalLevels Level { get; set; }
        public int ApproverId { get; set; }
        public PublicationApprovalStatus Status { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Remarks { get; set; }
        public bool RequireNext { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
    }

    [Table("PublicationWithdrawal")]
    public class PublicationWithdrawal
    {
        [Key]
        public int ID { get; set; }
        public int PublicationID { get; set; }
        public PublicationApprovalLevels Level { get; set; }
        public int ApproverId { get; set; }
        public PublicationApprovalStatus Status { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Remarks { get; set; }
        public bool RequireNext { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
    }

    [Table("PublicationPurchase")]
    public class PublicationPurchase
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }     // also determines payment from individual/agency
        //[Required(ErrorMessageResourceName = "ValidRequiredDeliveryAddress", ErrorMessageResourceType = typeof(Language.RnPForm))]
        //[Display(Name = "PubPurchaseDeliveryAddress", ResourceType = typeof(Language.RnPForm))]
        public string DeliveryAddress { get; set; }
        public string DiscountCode { get; set; }
        public string InvoiceNo { get; set; }
        public PublicationPaymentModes PaymentMode { get; set; }
        public DateTime CreatedDate { get; set; }
        // guarantee letter? (per agency)
    }

    [Table("PublicationPurchaseItem")]
    public class PublicationPurchaseItem
    {
        [Key]
        public int ID { get; set; }
        public int PublicationPurchaseID { get; set; }
        public int PublicationID { get; set; }
        public PublicationFormats Format { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("PublicationPurchaseID")]
        public virtual PublicationPurchase PublicationPurchase { get; set; }

        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
    }

    [Table("PublicationRefund")]
    public class PublicationRefund
    {
        [Key]
        public int ID { get; set; }
        public int PublicationID { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string BankAccountNo { get; set; }
        public string ReferenceNo { get; set; }
        public PublicationRefundStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        // guarantee letter? (per agency)

        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
    }

    public enum PublicationCategories
    {
        [Display(Name = "PubCategoryArticles", ResourceType = typeof(Language.RnPEnum))]
        Articles = 1,
        [Display(Name = "PubCategoryBooks", ResourceType = typeof(Language.RnPEnum))]
        Books = 2,
        [Display(Name = "PubCategoryFactsSheet", ResourceType = typeof(Language.RnPEnum))]
        FactsSheet = 3,
        [Display(Name = "PubCategoryJournals", ResourceType = typeof(Language.RnPEnum))]
        Journals = 4,
        [Display(Name = "PubCategoryLiteratureReviews", ResourceType = typeof(Language.RnPEnum))]
        LiteratureReviews = 5,
        [Display(Name = "PubCategoryReports", ResourceType = typeof(Language.RnPEnum))]
        Reports = 6,
        [Display(Name = "PubCategoryResearchPapers", ResourceType = typeof(Language.RnPEnum))]
        ResearchPapers = 7
    }

    public enum PublicationStatus
    {
        [Display(Name = "PubStatusNew", ResourceType = typeof(Language.RnPEnum))]                           // draft
        New,
        [Display(Name = "PubStatusSubmitted", ResourceType = typeof(Language.RnPEnum))]                     // pending verification
        Submitted,
        [Display(Name = "PubStatusVerifierRejected", ResourceType = typeof(Language.RnPEnum))]              // pending amendment
        VerifierRejected,
        [Display(Name = "PubStatusVerified", ResourceType = typeof(Language.RnPEnum))]                      // pending approval
        Verified,
        [Display(Name = "PubStatusApproverRejected", ResourceType = typeof(Language.RnPEnum))]              // pending amendment
        ApproverRejected,
        [Display(Name = "PubStatusApproved", ResourceType = typeof(Language.RnPEnum))]                      // approved
        Approved,
        [Display(Name = "PubStatusPublished", ResourceType = typeof(Language.RnPEnum))]                     // published
        Published,
        [Display(Name = "PubStatusUnpublished", ResourceType = typeof(Language.RnPEnum))]                   // unpublished
        Unpublished,
        [Display(Name = "PubStatusTrashed", ResourceType = typeof(Language.RnPEnum))]                       // cancelled
        Trashed,
        [Display(Name = "PubStatusWithdrawalNew", ResourceType = typeof(Language.RnPEnum))]                 // draft
        NewWithdrawal,
        [Display(Name = "PubStatusWithdrawalSubmitted", ResourceType = typeof(Language.RnPEnum))]           // pending verification
        WithdrawalSubmitted,
        [Display(Name = "PubStatusWithdrawalVerifierRejected", ResourceType = typeof(Language.RnPEnum))]    // pending amendment
        WithdrawalVerifierRejected,
        [Display(Name = "PubStatusWithdrawalVerified", ResourceType = typeof(Language.RnPEnum))]            // pending approval
        WithdrawalVerified,
        [Display(Name = "PubStatusWithdrawalApproverRejected", ResourceType = typeof(Language.RnPEnum))]    // pending amendment
        WithdrawalApproverRejected,
        [Display(Name = "PubStatusWithdrawalApproved", ResourceType = typeof(Language.RnPEnum))]            // approved
        WithdrawalApproved,
        [Display(Name = "PubStatusWithdrawalComplete", ResourceType = typeof(Language.RnPEnum))]            // published
        Withdrawn,
        [Display(Name = "PubStatusWithdrawalTrashed", ResourceType = typeof(Language.RnPEnum))]             // cancelled
        WithdrawalTrashed
    }

    /*
    public enum PublicationWithdrawalStatus
    {
        [Display(Name = "PubWithdrawalStatusNone", ResourceType = typeof(Language.RnPEnum))]
        None,
        [Display(Name = "PubWithdrawalStatusSubmitted", ResourceType = typeof(Language.RnPEnum))]
        Submitted,
        [Display(Name = "PubWithdrawalStatusRejected", ResourceType = typeof(Language.RnPEnum))]
        Rejected,
        [Display(Name = "PubWithdrawalStatusWithdrawn", ResourceType = typeof(Language.RnPEnum))]
        Withdrawn
    }
    */

    public enum PublicationApprovalLevels
    {
        [Display(Name = "PubApprovalLevelVerifier", ResourceType = typeof(Language.RnPEnum))]
        Verifier,
        [Display(Name = "PubApprovalLevelApprover1", ResourceType = typeof(Language.RnPEnum))]
        Approver1,
        [Display(Name = "PubApprovalLevelApprover2", ResourceType = typeof(Language.RnPEnum))]
        Approver2,
        [Display(Name = "PubApprovalLevelApprover3", ResourceType = typeof(Language.RnPEnum))]
        Approver3
    }

    public enum PublicationApprovalStatus
    {
        [Display(Name = "PubApprovalStatusNone", ResourceType = typeof(Language.RnPEnum))]
        None,
        [Display(Name = "PubApprovalStatusApproved", ResourceType = typeof(Language.RnPEnum))]
        Approved,
        [Display(Name = "PubApprovalStatusRejected", ResourceType = typeof(Language.RnPEnum))]
        Rejected
    }

    public enum PublicationPaymentModes
    {
        [Display(Name = "PubPaymentModeOnline", ResourceType = typeof(Language.RnPEnum))]
        Online,
        [Display(Name = "PubPaymentModeOffline", ResourceType = typeof(Language.RnPEnum))]
        Offline
    }

    public enum PublicationFormats
    {
        [Display(Name = "PubFormatHardcopy", ResourceType = typeof(Language.RnPEnum))]
        Hardcopy,
        [Display(Name = "PubFormatDigital", ResourceType = typeof(Language.RnPEnum))]
        Digital,
        [Display(Name = "PubFormatBoth", ResourceType = typeof(Language.RnPEnum))]
        Both
    }

    public enum PublicationRefundStatus
    {
        [Display(Name = "PubRefundStatusIncomplete", ResourceType = typeof(Language.RnPEnum))]
        Incomplete,
        [Display(Name = "PubRefundStatusComplete", ResourceType = typeof(Language.RnPEnum))]
        Complete
    }
}
