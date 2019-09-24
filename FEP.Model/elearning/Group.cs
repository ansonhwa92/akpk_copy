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
    public class Group : BaseEntity
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; } = false;
        public string EnrollmentCode { get; set; }
        public int? CourseEventId { get; set; }        
        public int? CourseId { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; }

    }
}
