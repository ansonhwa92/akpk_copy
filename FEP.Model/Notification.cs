using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    public enum NotificationType
    {
        [Display(Name = "Activate Account")]
        ActivateAccount,
        [Display(Name = "Reset Password")]
        ResetPassword,       

    }

    [Table("Notification")]
    public class Notification
    {
        [Key]
        public long Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("NotificationToSend")]
    public class NotificationToSend
    {
        [Key]
        public long Id { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentDate { get; set; }

        public virtual ICollection<NotificationToSendRecipient> Recipient { get; set; }
    }

    [Table("NotificationToSendRecipient")]
    public class NotificationToSendRecipient
    {
        [Key]
        public long Id { get; set; }
        public long NotificationToSendId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("NotificationToSendId")]
        public virtual NotificationToSend NotificationToSend { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("NotificationSetting")]
    public class NotificationSetting
    {
        [Key]
        public NotificationType NotificationType { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsSendNotification { get; set; }
        public string NotificationMessage { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }

    }
}
