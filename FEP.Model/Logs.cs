using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("UserLog")]
    public class UserLog
    {
        [Key]
        public long Id { get; set; }
        public Modules Module { get; set; }
        public int? UserId { get; set; }
        public string IPAddress { get; set; }
        public string GeoLocation { get; set; }
        public DateTime LogDate { get; set; }
        public string Activity { get; set; }
        public string Details { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("ErrorLog")]
    public class ErrorLog
    {
        [Key]
        public long Id { get; set; }
        
        public Modules Module { get; set; }
        public int? UserId { get; set; }
        public string Source { get; set; }
        public string IPAddress { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
