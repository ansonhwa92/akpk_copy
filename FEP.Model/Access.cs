using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    public enum UserAccess
    {        
        [Display(Name = "Home")]
        Home,
        [Display(Name = "System")]
        System,

    }
}
