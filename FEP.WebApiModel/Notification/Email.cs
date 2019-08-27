using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Notification
{
    public class EmailRecipientModel
    {
        public EmailRecipientModel()
        {
            To = new EmailAddress();
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailAddress To { get; set; }
        public int? UserId { get; set; }
        public DateTime? SendDate { get; set; }        
    }

    public class EmailRecipientsModel
    {
        public EmailRecipientsModel()
        {
            To = new List<EmailAddress>();
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAddress> To { get; set; }
        public List<int> UserId { get; set; }
        public DateTime? SendDate { get; set; }
    }

}
