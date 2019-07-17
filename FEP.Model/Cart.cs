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

        public string ProformaInvoiceNo { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public float TotalPrice { get; set; }
    }

    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        public string Description { get; set; }

        public PurchaseType PurchaseType { get; set; }

        public float Price { get; set; }

        [ForeignKey("PurchaseOrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }


    public enum PurchaseType
    {
        Event,
        Course,
        Publication
    }
}
