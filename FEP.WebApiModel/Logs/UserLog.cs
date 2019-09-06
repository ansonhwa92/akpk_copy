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
    public class ListUserLogModel
    {
        public FilterUserLogModel Filter { get; set; }
        public UserLogModel List { get; set; }
    }

    public class FilterUserLogModel : DataTableModel
    {

        [Display(Name = "FieldModule", ResourceType = typeof(Language.UserLog))]
        public Modules? Module { get; set; }
                
        [Display(Name = "FieldUserName", ResourceType = typeof(Language.UserLog))]
        public string UserName { get; set; }

        [Display(Name = "FieldActivity", ResourceType = typeof(Language.UserLog))]
        public string Activity { get; set; }

        [Display(Name = "FieldDetails", ResourceType = typeof(Language.UserLog))]
        public string Details { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateFrom", ResourceType = typeof(Language.General))]
        public DateTime? LogDateFrom { get; set; }

        [UIHint("Date")]
        [Display(Name = "FieldDateTo", ResourceType = typeof(Language.General))]
        public DateTime? LogDateTo { get; set; }

        [Display(Name = "FieldIPAddress", ResourceType = typeof(Language.UserLog))]
        public string IPAddress { get; set; }


    }

    public class UserLogModel
    {
        public long Id { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.UserLog))]
        public Modules? Module { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.UserLog))]
        public string ModuleDesc { get; set; }

        public int? UserId { get; set; }

        [Display(Name = "FieldUserName", ResourceType = typeof(Language.UserLog))]
        public string UserName { get; set; }

        [Display(Name = "FieldActivity", ResourceType = typeof(Language.UserLog))]
        public string Activity { get; set; }

        [Display(Name = "FieldDetails", ResourceType = typeof(Language.UserLog))]
        public string Details { get; set; }

        [Display(Name = "FieldLogDate", ResourceType = typeof(Language.UserLog))]
        public DateTime LogDate { get; set; }

        [Display(Name = "FieldIPAddress", ResourceType = typeof(Language.UserLog))]
        public string IPAddress { get; set; }

    }

    public class CreateUserLogModel
    {                
        public Modules Module { get; set; }

        public int? UserId { get; set; }
    
        public string Activity { get; set; }
                
        public string Details { get; set; }
                         
        public string IPAddress { get; set; }

        public string GeoLocation { get; set; }

    }
}
