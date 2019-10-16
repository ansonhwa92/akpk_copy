using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FEP.Model.eLearning
{
    /// <summary>
    /// This table should store progress for each item (Module, Test, Assigment) in the Course.
    /// May change.
    /// 
    /// EnrollmentId  LearnerId   ModuleId  ProgressT
    /// </summary>
    public class CourseProgress : BaseEntity
    {
        public int EnrollmentId { get; set; }

        public virtual Enrollment Enrollment { get; set; }

        [Required, Index]
        public int LearnerId { get; set; }

        public virtual Learner Learner { get; set; }
        
        public int ModuleId { get; set; }
       
        public ProgressItem ProgressItem { get; set; }
        public int ItemId { get; set; }
        public bool IsCompleted { get; set; }
        public decimal Score { get; set; } = 0.0m;


    }

    public enum ProgressItem
    {
        Module,
        Test,
        Asssigment
    }

 }
 