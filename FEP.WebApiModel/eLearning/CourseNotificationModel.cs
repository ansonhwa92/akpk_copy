using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;

namespace FEP.WebApiModel.eLearning
{
    public class NotificationModel
    {
        public int Id { get; set; } // Id of whatever object is according to Notifcation type

        public NotificationType NotificationType { get; set; }

        public NotificationCategory NotificationCategory { get; set; }
        public ParameterListToSend ParameterListToSend { get; set; }
        public DateTime StartNotificationDate { get; set; }

        public List<int> ReceiverId { get; set; }

        public Type Type { get; set; }
    }
}