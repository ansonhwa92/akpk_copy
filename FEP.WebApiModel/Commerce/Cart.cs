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

    // class for creating purchase order (cart)
    public class CreatePurchaseOrderModel
    {
        public int UserId { get; set; }
    }

    // class for updating purchase order (cart) discount code
    public class EditPurchaseOrderDiscountCodeModel
    {
        public int Id { get; set; }

        public string DiscountCode { get; set; }
    }

    // class for updating purchase order (cart) payment info
    public class EditPurchaseOrderPaymentInfoModel
    {
        public int Id { get; set; }

        public PaymentModes PaymentMode { get; set; }

        public float TotalPrice { get; set; }
    }

    // class for updating purchase order (cart) invoice info
    public class EditPurchaseOrderInvoiceNoModel
    {
        public int Id { get; set; }

        public string ProformaInvoiceNo { get; set; }
    }

    // class for updating purchase order (cart) status
    public class EditPurchaseOrderStatusModel
    {
        public int Id { get; set; }

        public CheckoutStatus Status { get; set; }
    }

    // class for returning/updating cart header
    public class PurchaseOrderModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string DiscountCode { get; set; }

        // updated by finance?
        public string ProformaInvoiceNo { get; set; }

        public PaymentModes PaymentMode { get; set; }

        public DateTime CreatedDate { get; set; }

        public float TotalPrice { get; set; }

        public CheckoutStatus Status { get; set; }
    }

    // class for returning/updating publication purchase item
    public class PurchaseOrderItemModel
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        public PurchaseType PurchaseType { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Display(Name = "Unit Price")]
        [Required]
        public float Price { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public int Quantity { get; set; }
    }

    // class for returning complete cart
    public class PurchaseOrderFullModel
    {
        public PurchaseOrderModel Cart { get; set; }
        public List<PurchaseOrderItemModel> Items { get; set; }
        public List<PurchaseOrderItemModel> Publications { get; set; }
        public List<PurchaseOrderItemModel> Events { get; set; }
        public List<PurchaseOrderItemModel> Courses { get; set; }
    }

}
