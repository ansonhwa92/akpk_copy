using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DiscountCode { get; set; }
        public string ProformaInvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public PaymentModes PaymentMode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float TotalPrice { get; set; }
        public CheckoutStatus Status { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }

    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Description { get; set; }
        public PurchaseType PurchaseType { get; set; }
        public int ItemId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }

    [Table("PromotionCode")]
    public class PromotionCode
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public int MoneyValue { get; set; }
        public int PercentageValue { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        // future: applicable to which module, max per receipt, etc.
    }

    [Table("BankInformation")]
    public class BankInformation
    {
        [Key]
        public int ID { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        // future:
        // indicators of availability for online, fpx, etc
        // url etc for integration
    }

    [Table("Refund")]
    public class Refund
    {
        [Key]
        public int ID { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int BankID { get; set; }
        public string BankAccountNo { get; set; }
        public string ReferenceNo { get; set; }
        public RefundStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        // guarantee letter? (per agency)

        [ForeignKey("ItemId")]
        public virtual PurchaseOrderItem Item { get; set; }

        [ForeignKey("BankID")]
        public virtual BankInformation Bank { get; set; }
    }

    public enum PurchaseType
    {
        [Display(Name = "PurchaseTypeEvent", ResourceType = typeof(Language.Cart))]
        Event,
        [Display(Name = "PurchaseTypeCourse", ResourceType = typeof(Language.Cart))]
        Course,
        [Display(Name = "PurchaseTypePublication", ResourceType = typeof(Language.Cart))]
        Publication
    }

    public enum PaymentModes
    {
        [Display(Name = "PaymentModeOnline", ResourceType = typeof(Language.Cart))]
        Online,
        [Display(Name = "PaymentModeOffline", ResourceType = typeof(Language.Cart))]
        Offline
    }

    public enum CheckoutStatus
    {
        [Display(Name = "CheckoutStatusShopping", ResourceType = typeof(Language.Cart))]
        Shopping,
        [Display(Name = "CheckoutStatusCheckedOut", ResourceType = typeof(Language.Cart))]
        CheckedOut,
        [Display(Name = "CheckoutStatusPaid", ResourceType = typeof(Language.Cart))]
        Paid
    }

    public enum DeliveryStatus
    {
        [Display(Name = "DeliveryStatusNone", ResourceType = typeof(Language.Cart))]
        None,
        [Display(Name = "DeliveryStatusPreparing", ResourceType = typeof(Language.Cart))]
        Preparing,
        [Display(Name = "DeliveryStatusShipped", ResourceType = typeof(Language.Cart))]
        Shipped,
        [Display(Name = "DeliveryStatusDelivered", ResourceType = typeof(Language.Cart))]
        Delivered
    }

    public enum RefundStatus
    {
        [Display(Name = "RefundStatusRequested", ResourceType = typeof(Language.Cart))]
        Requested,
        [Display(Name = "RefundStatusIncomplete", ResourceType = typeof(Language.Cart))]
        Incomplete,
        [Display(Name = "RefundStatusComplete", ResourceType = typeof(Language.Cart))]
        Complete
    }
}
