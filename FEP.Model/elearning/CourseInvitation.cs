using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.eLearning
{
    public class CourseInvitation : BaseEntity
    {
        public int CourseEventId { get; set; }
        public virtual CourseEvent CourseEvent  { get; set;}

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        // list of emails separated by comma
        public string Emails { get; set; }
        public string EmailSubject { get; set; }
        public NotificationType NotificationType { get; set; }

    }
}
