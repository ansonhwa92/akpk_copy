using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;

namespace FEP.WebApiModel.eLearning
{
    public class NotificationModel : CreateAutoReminder
    {
        public int Id { get; set; } // Id of whatever object is according to Notifcation type
        public string Emails { get; set; }

        public Type Type { get; set; }
        public bool IsNeedRemainder { get; set; } = true;
        public ReceiverType ReceiverType { get; set; }
    }

    public enum ReceiverType
    {
        Emails,
        UserIds,
        Both
    }
}