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
    public class ListErrorLogModel
    {
        public FilterErrorLogModel Filter { get; set; }
        public ErrorLogModel List { get; set; }
    }

    public class FilterErrorLogModel : DataTableModel
    {

        [Display(Name = "FieldModule", ResourceType = typeof(Language.ErrorLog))]
        public Modules? Module { get; set; }
                
        [Display(Name = "FieldUserName", ResourceType = typeof(Language.ErrorLog))]
        public string UserName { get; set; }

        [Display(Name = "FieldSource", ResourceType = typeof(Language.ErrorLog))]
        public string Source { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.ErrorLog))]
        public string Description { get; set; }              

        [Display(Name = "FieldDateFrom", ResourceType = typeof(Language.General))]
        public DateTime? LogDateFrom { get; set; }

        [Display(Name = "FieldDateTo", ResourceType = typeof(Language.General))]
        public DateTime? LogDateTo { get; set; }

        [Display(Name = "FieldIPAddress", ResourceType = typeof(Language.ErrorLog))]
        public string IPAddress { get; set; }


    }

    public class ErrorLogModel
    {
        public long Id { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.ErrorLog))]
        public Modules? Module { get; set; }

        [Display(Name = "FieldModule", ResourceType = typeof(Language.ErrorLog))]
        public string ModuleDesc { get; set; }

        public int? UserId { get; set; }

        [Display(Name = "FieldUserName", ResourceType = typeof(Language.ErrorLog))]
        public string UserName { get; set; }

        [Display(Name = "FieldSource", ResourceType = typeof(Language.ErrorLog))]
        public string Source { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.ErrorLog))]
        public string Description { get; set; }

        [Display(Name = "FieldDetails", ResourceType = typeof(Language.ErrorLog))]
        public string Details { get; set; }

        [Display(Name = "FieldLogDate", ResourceType = typeof(Language.ErrorLog))]
        public DateTime LogDate { get; set; }

        [Display(Name = "FieldIPAddress", ResourceType = typeof(Language.ErrorLog))]
        public string IPAddress { get; set; }

    }

    public class CreateErrorLogModel
    {
        public Modules Module { get; set; }
               
        public int? UserId { get; set; }
        
        public string Source { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }
        
        public string IPAddress { get; set; }

    }
}
