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

    public class CourseApproval : BaseEntity
    {
        public int ApproverId { get; set; }
        public virtual User Approver { get; set; }
        public DateTime ApprovalDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
        public bool IsNextLevelRequired { get; set; }
        public string Remark { get; set; }
    }
}
