using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    public class RefCourseCategory : LookupEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        /// <summary>
        /// Whether this item should be shown at selections
        /// </summary>
        public bool IsDisplayed { get; set; }

    }
}
