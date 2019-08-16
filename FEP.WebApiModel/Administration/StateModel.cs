using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class StateModel
    {
        public int Id { get; set; }
        [Display(Name = "FieldStateName", ResourceType = typeof(Language.Administration))]
        public string Name { get; set; }
    }
}
