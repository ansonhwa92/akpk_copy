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
        public PaymentModes PaymentMode { get; set; }
        public DateTime CreatedDate { get; set; }
        public float TotalPrice { get; set; }
        public CheckoutStatus Status { get; set; }
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
}
