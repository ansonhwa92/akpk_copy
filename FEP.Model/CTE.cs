using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model
{

    [Table("Months")]
    public class Months
    {
        [Key]
        public int Month { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
    }
}
