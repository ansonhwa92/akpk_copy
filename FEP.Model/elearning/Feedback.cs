using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string Header { get; set; }
        public string Template { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User UpdateUser { get; set; }
    }

    [Table("FeedbackContent")]
    public class FeedbackContent
    {
        [Key]
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int FeedbackId { get; set; }

        public int ViewId { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("FeedbackId")]
        public virtual Feedback Feedback { get; set; }
        [ForeignKey("ViewId")]
        public virtual FeedbackView FeedbackView { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual User UpdateUser { get; set; }
    }

    [Table("FeedbackView")]
    public class FeedbackView
    {
        [Key]
        public int Id { get; set; }
        public string View { get; set; }
    }
}
