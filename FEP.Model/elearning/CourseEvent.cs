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


    /// <summary>
    /// This table contains the histury for the course being offered for trial or for public/private groups.
    /// </summary>
    public class CourseEvent : BaseEntity, IValidatableObject
    {
        /// <summary>
        /// Specific code for enrollment, defined during creation of group of Learners
        /// </summary>
        [Required]
        [Index(IsUnique = true)] 
        [MaxLength(150)]
        public string EnrollmentCode { get; set; }

        [Index]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        // The coursestatus at the point of this event, can be either Approved or Draft, changed afterward
        public CourseEventStatus Status { get; set; }

        // Date the course is available, mandatoryif published to a group
        public DateTime? Start { get; set; }

        // Date the course is available. After this date enrillment is not possible, but the course is still viewable by enrolled learners
        public DateTime? End { get; set; }
        public DateTime? LastDateToCancel { get; set; }

        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        // This event is for public or private group
        public ViewCategory ViewCategory { get; set; }

        // Required if published for trial
        public string TrialRemark { get; set; }

        // ----- Start value from Course
        // TODO : May need to have conditional validation at ModelView
        //https://stackoverflow.com/questions/2417113/asp-net-mvc-conditional-validation 
        public decimal AllowablePercentageBeforeWithdraw { get; set; } = 0.0m;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.Course.IsFree && AllowablePercentageBeforeWithdraw <= 0.0m)
                yield return new ValidationResult("Allowable Percentage Before Withdraw value must be set.");

        }
    }

    public enum ViewCategory
    {
        Private,
        Public,
    }


    public enum CourseEventStatus
    {
        Trial,
        TrialEnded,
        AvailableToPublic,
        AvailableToPrivate,
        AvailableToPublicAndPrivate,

        Suspended,
        Cancelled        
    }
}
