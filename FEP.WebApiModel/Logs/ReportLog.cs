using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Logs
{
    public class ShareLogModel
    {       
        public LogCategory Category { get; set; }
        public int CategoryId { get; set; } //eventid, publicationid, researchid, courseid
        public DateTime CreatedDate { get; set; }

    }

    public class PageLogModel
    {       
        public LogCategory Category { get; set; }
        public int CategoryId { get; set; } //eventid, publicationid, researchid, courseid
        public DateTime CreatedDate { get; set; }
    }
}
