using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("EmailToSend")]
    public class EmailToSend
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentDate { get; set; }

        public virtual ICollection<EmailToSendAddress> EmailAddress { get; set; }

    }

    [Table("EmailToSendAddress")]
    public class EmailToSendAddress
    {
        [Key]
        public long Id { get; set; }
        public long EmailToSendId { get; set; }
        public bool IsCC { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }

        [ForeignKey("EmailToSendId")]
        public virtual EmailToSend EmailToSend { get; set; }

    }





}
