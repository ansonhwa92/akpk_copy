using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;
using FEP.Model.eLearning;
using FEP.Model;

namespace FEP.WebApiModel.eLearning
{

    public class CourseApprovalLogModel : BaseModel
    {
        public int CourseId { get; set; }
        public string CreatedByName { get; set; }
        public ApprovalLevel ApprovalLevel { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public int? ApproverId { get; set; }
        public virtual User Approver { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
        public bool IsNextLevelRequired { get; set; }

        public string Remark { get; set; }
        public CourseStatus Status { get; set; }
        public string CourseTitle { get; set; }
    }
}
