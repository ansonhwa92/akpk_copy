using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    public enum SystemMode
    {
        Development = 0,
        Production = 1,
        Maintenance = 2
    }

    [Table("SystemSetting")]
    public class SystemSetting
    {
        [Key]
        [DatabaseG‌​enerated(DatabaseGen‌​eratedOption.None)]
        public int Id { get; set; }

        public string SystemTitle { get; set; }
        public string ShortTitle { get; set; }
        public string SystemFooter { get; set; }
        public string SystemVersion { get; set; }
        public string EmailFooter { get; set; }

    }

    
    [Table("AccountSetting")]
    public class AccountSetting
    {
        [Key]
        [DatabaseG‌​enerated(DatabaseGen‌​eratedOption.None)]
        public int Id { get; set; }
        public bool IsPasswordExpiry { get; set; }
        public bool IsLimitLoginAttempt { get; set; }
        public int? PasswordExpiryDuration { get; set; }//days
        public int? LoginAttemptLimit { get; set; }
        public int InactiveDuration { get; set; }//days      
        public bool IsContainLowerCase { get; set; }
        public bool IsContainUpperCase { get; set; }
        public bool IsContainNumeric { get; set; }
        public bool IsContainSymbol { get; set; }
        public bool IsLengthLimit { get; set; }
    }
}
