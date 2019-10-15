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

    // class for updating purchase order (cart) receipt info
    public class EditPurchaseOrderReceiptNoModel
    {
        public int Id { get; set; }

        public string ReceiptNo { get; set; }
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

        // updated by finance?
        public string ReceiptNo { get; set; }

        public PaymentModes PaymentMode { get; set; }

        public DateTime CreatedDate { get; set; }

        public float TotalPrice { get; set; }

        public CheckoutStatus Status { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }
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

    // class for returning purchase history information
    public class PurchaseHistoryModel
    {
        public int PurchaseOrderId { get; set; }

        public int OrderItemId { get; set; }

        public int UserId { get; set; }

        public string ReceiptNo { get; set; }

        public PurchaseType PurchaseType { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public float Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public CheckoutStatus Status { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }

        public RefundStatus? RefundStatus { get; set; }
    }

    // class for setting and returning filters for the datatable list of purchase history
    public class FilterPurchaseHistoryModel : DataTableModel
    {
        [Display(Name = "FilterDescription", ResourceType = typeof(Language.Cart))]
        public string Description { get; set; }

        [Display(Name = "FilterReceiptNo", ResourceType = typeof(Language.Cart))]
        public string ReceiptNo { get; set; }
    }

    // class for returning purchase history information and creating refund request
    public class CreateRefundModel
    {
        public int ItemId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Bank Name")]
        [Required]
        public int BankID { get; set; }

        [Display(Name = "Bank Account No.")]
        [Required]
        public string BankAccountNo { get; set; }

        public string ReferenceNo { get; set; }
    }

    // class for updating refund request
    public class EditRefundModel
    {
        public int ID { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Bank Name")]
        [Required]
        public int BankID { get; set; }

        [Display(Name = "Bank Account No.")]
        [Required]
        public string BankAccountNo { get; set; }

        public string ReferenceNo { get; set; }
    }

    // class for returning purchase history information and creating refund request
    public class ListPurchaseHistoryModel
    {
        public FilterPurchaseHistoryModel Filters { get; set; }

        public PurchaseHistoryModel Items { get; set; }

        public CreateRefundModel Refund { get; set; }
    }

    // return banks
    public class BankInformationModel
    {
        public int ID { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }
    }

}
