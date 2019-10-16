using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Notification
{

    public class ListNotificationModel
    {
        public FilterNotificationModel Filter { get; set; }
        public NotificationModel List { get; set; }
    }

    public class FilterNotificationModel : DataTableModel
    {
        public int UserId { get; set; }

        [Display(Name = "FieldCategory", ResourceType = typeof(Language.Notification))]
        public NotificationCategory? Category { get; set; }

        [Display(Name = "FieldMessage", ResourceType = typeof(Language.Notification))]
        public string Message { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateFrom", ResourceType = typeof(Language.General))]
        public DateTime? DateFrom { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateTo", ResourceType = typeof(Language.General))]
        public DateTime? DateTo { get; set; }

        
               
    }

    public class NotificationModel
    {
        public long Id { get; set; }
        public NotificationType? NotificationType { get; set; }

        [Display(Name = "FieldCategory", ResourceType = typeof(Language.Notification))]
        public NotificationCategory Category { get; set; }

        [Display(Name = "FieldMessage", ResourceType = typeof(Language.Notification))]
        public string Message { get; set; }
        public string Link { get; set; }

        [Display(Name = "FieldDate", ResourceType = typeof(Language.Notification))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Notification))]
        public bool IsRead { get; set; }

        public string CategoryDesc { get; set; }
    }

    public class CreateNotificationModel
    {       
        public int UserId { get; set; }
        public NotificationType? NotificationType { get; set; }        
        public NotificationCategory Category { get; set; }        
        public string Message { get; set; }
        public string Link { get; set; }        
        public DateTime? SendDate { get; set; }        
    }


}
