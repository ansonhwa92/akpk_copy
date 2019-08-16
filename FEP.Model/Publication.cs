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
    [Table("Publication")]
    public class Publication
    {
        [Key]
        public int ID { get; set; }
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
        public int Year { get; set; }
        [Required(ErrorMessageResourceName = "ValidRequiredDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = "ValidRequiredLanguage", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubLanguage", ResourceType = typeof(Language.RnPForm))]
        public string Language { get; set; }
        [Required(ErrorMessageResourceName = "ValidRequiredISBN", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubISBN", ResourceType = typeof(Language.RnPForm))]
        public string ISBN { get; set; }
        [Display(Name = "PubFree", ResourceType = typeof(Language.RnPForm))]
        public bool Free { get; set; }              // Free publication if True
        [Display(Name = "PubHardcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Hardcopy { get; set; }
        [Display(Name = "PubDigitalcopy", ResourceType = typeof(Language.RnPForm))]
        public bool Digitalcopy { get; set; }
        [Display(Name = "PubHDcopy", ResourceType = typeof(Language.RnPForm))]
        public bool HDcopy { get; set; }
        [Display(Name = "PubHPrice", ResourceType = typeof(Language.RnPForm))]
        public float HPrice { get; set; }
        [Display(Name = "PubDPrice", ResourceType = typeof(Language.RnPForm))]
        public float DPrice { get; set; }
        [Display(Name = "PubHDPrice", ResourceType = typeof(Language.RnPForm))]
        public float HDPrice { get; set; }
        [Display(Name = "PubProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }         // uploaded proof of approval
        [Display(Name = "PubStockBalance", ResourceType = typeof(Language.RnPForm))]
        public int StockBalance { get; set; }
        // withdrawal use
        [Display(Name = "PubWithdrawalReason", ResourceType = typeof(Language.RnPForm))]
        public string WithdrawalReason { get; set; }
        [Display(Name = "PubProofOfWithdrawal", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfWithdrawal { get; set; }       // uploaded proof of approval of withdrawal
        // non-key in data
        [Display(Name = "PubDateAdded", ResourceType = typeof(Language.RnPForm))]
        public DateTime DateAdded { get; set; }
        [Display(Name = "PubStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationStatus Status { get; set; }
        [Display(Name = "PubWStatus", ResourceType = typeof(Language.RnPForm))]
        public PublicationWithdrawalStatus WStatus { get; set; }
        [Display(Name = "PubViewCount", ResourceType = typeof(Language.RnPForm))]
        public int ViewCount { get; set; }
        [Display(Name = "PubPurchaseCount", ResourceType = typeof(Language.RnPForm))]
        public int PurchaseCount { get; set; }
        // DMS integration (TODO)
        // nav
        [ForeignKey("CategoryID")]
        [Display(Name = "PubCategoryName", ResourceType = typeof(Language.RnPForm))]
        public virtual PublicationCategory Category { get; set; }
        public virtual ICollection<PublicationApproval> Approvals { get; set; }
        public virtual ICollection<PublicationWithdrawal> Withdrawals { get; set; }
    }

    [Table("PublicationCategory")]
    public class PublicationCategory
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "PubCategoryNameList", ResourceType = typeof(Language.RnPForm))]
        public string Name { get; set; }
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
        [Required(ErrorMessageResourceName = "ValidRequiredRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }
        public bool RequireNext { get; set; }

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
        [Required(ErrorMessageResourceName = "ValidRequiredRemarks", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubWithdrawalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }
        public bool RequireNext { get; set; }

        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
    }

    [Table("PublicationPurchase")]
    public class PublicationPurchase
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }     // also determines payment from individual/agency
        [Required(ErrorMessageResourceName = "ValidRequiredDeliveryAddress", ErrorMessageResourceType = typeof(Language.RnPForm))]
        [Display(Name = "PubPurchaseDeliveryAddress", ResourceType = typeof(Language.RnPForm))]
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
        New,
        Submitted,
        Published,
        Trashed
    }

    public enum PublicationWithdrawalStatus
    {
        None,
        Submitted,
        Withdrawn
    }

    public enum PublicationApprovalLevels
    {
        Verifier,
        Approver1,
        Approver2,
        Approver3
    }

    public enum PublicationApprovalStatus
    {
        None,
        Approved,
        Rejected
    }

    public enum PublicationPaymentModes
    {
        Online,
        Offline
    }

    public enum PublicationFormats
    {
        Hardcopy,
        Digital,
        Both
    }

    public enum PublicationRefundStatus
    {
        Incomplete,
        Complete
    }
}
