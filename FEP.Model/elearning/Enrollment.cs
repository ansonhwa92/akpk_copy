using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FEP.Model.eLearning
{
    public class Enrollment : BaseEntity
    {
        [Required, Index]
        public int CourseEventId { get; set; }

        [Index]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        [Index]
        public int LearnerId { get; set; }

        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public DateTime? WithdrawDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        /// <summary>
        /// The learner score for this course, manually calculated
        /// </summary>
        public decimal? Score { get; set; }
        public int TotalModulesCompleted { get; set; }
        public int TotalTestCompleted { get; set; }
        public int TotalAssignmentCompleted { get; set; }

        /// <summary>
        /// Store progress of each mdule, test, assigment
        /// </summary>
        public virtual ICollection<CourseProgress> CourseProgress { get; set; }


        public void GetProgress()
        {
            TotalModulesCompleted = this.CourseProgress.Where(x => x.ProgressItem == ProgressItem.Module).Count();
            TotalTestCompleted = this.CourseProgress.Where(x => x.ProgressItem == ProgressItem.Test).Count();
            TotalAssignmentCompleted = this.CourseProgress.Where(x => x.ProgressItem == ProgressItem.Asssigment).Count();

        }
    }

    public enum EnrollmentStatus
    {
        [Display(Name = "Not Enrolled")]
        None,

        [Display(Name = "Invited but not enrolled")]
        Invited,

        Enrolled,
        Withdraw,

        [Display(Name = "Invitation cancelled after invited and enrolled")]
        Cancelled,

        [Display(Name = "Removed from the course")]
        Removed,

        [Display(Name = "Completed the course")]
        Completed,

        [Display(Name = "Failed the course")]
        Failed
    }
}