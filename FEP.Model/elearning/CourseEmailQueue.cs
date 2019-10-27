using System;
using System.Collections.Generic;

namespace FEP.Model.eLearning
{
    public class CourseEmailQueue : BaseEntity
    {
        public int? CourseId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationCategory { get; set; }
        public string Subject { get; set; }
        public string Parameters { get; set; }
        public string Receivers { get; set; }
        public int? NotificationId { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}