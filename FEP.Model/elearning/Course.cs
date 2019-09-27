using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model.eLearning
{
    [Table("Course")]
    public class Course : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual RefCourseCategory Category { get; set; }

        [Index]
        [MaxLength(20)]
        public string Code { get; set; }

        // should be HTML string
        public string Objectives { get; set; }       

        public CourseMedium Medium { get; set; }
        public CourseScheduleType ScheduleType { get; set; }

        // Total duration of the course, 10 days? 10 weeeks. Used with DurationType
        public int? Duration { get; set; }

        public DurationType DurationType { get; set; }

        public CourseLanguage Language { get; set; }


        public bool IsFree { get; set; } = true;

        public decimal? Price { get; set; } = 0.0m;

        public virtual ICollection<CourseModule> Modules { get; set; }

        // For the front page, excluding modules, modules will be from Modules above
        public virtual ICollection<CourseContent> FrontPageContents { get; set; }

        public CourseStatus Status { get; set; }

        public string IntroVideoPath { get; set; }

        /// <summary>
        /// Used the course is offered multiple times to different set of people and to public,
        /// When publishing a course for public, get the new course event if available, otherwise create a new course event for public.
        /// When publishing a course for private groups, create  new course event.
        /// </summary>
        public virtual ICollection<CourseEvent> CourseEvents { get; set; }

        // ----- START RULES AND PATH
        public TraversalRule TraversalRule { get; set; }

        public virtual ICollection<GamificationCriteria> GamificationCriteria { get; set; }

        public ScoreCalculation ScoreCalculation { get; set; } = (int)ScoreCalculation.Average;
        // ----- END RULES AND PATH

        //----- START APPROVAlS
        public ICollection<CourseApprovalLog> CourseApprovalLog { get; set; }
        //public int? VerifierApprovalId { get; set; }
        //public virtual CourseApproval VerifierApproval { get; set; }

        //public int? FirstApprovalId { get; set; }

        //public virtual CourseApproval FirstApproval { get; set; }

        //public int? SecondApprovalId { get; set; }
        //public virtual CourseApproval SecondApproval { get; set; }
        //public int? ThirdApprovalId { get; set; }

        //public virtual CourseApproval ThirdApproval { get; set; }
        
        public int TotalModules { get; set; }
  


        //----- END APPROVAlS

        // ----- Default Course Item
        /// <summary>
        /// Default certificate for this course. May change for Targetted users.
        /// This value may be copied over to CourseEvent when CourseEvent is created
        /// </summary>
        public int? CertificateId { get; set; }
        public virtual CourseCertificate DefaultCertificate { get; set; }

        /// <summary>
        /// Default trainers for this course. May change for Targetted users.
        /// This value may be copied over to CourseEvent when CourseEvent is created
        /// </summary>
        public virtual ICollection<Trainer> DefaultTrainers { get; set; }

        /// <summary>
        /// Default completion percentage before allowing learnet to withdraw.
        /// This value is required for Paid Course and will need to be copied to
        /// CourseEvent when CourseEvent is created
        /// </summary>
        public decimal DefaultAllowablePercentageBeforeWithdraw { get; set; } = 50.0m;

        public ViewCategory ViewCategory { get; set; }


        // ----- START COURSE COMPLETION CRITERIA
        public CompletionCriteriaType CompletionCriteriaType { get; set; } = CompletionCriteriaType.General;

        /// <summary>
        /// Contain list of modules to be completed that considered as course is complete
        /// </summary>
        public string ModulesCompleted { get; set; }

        /// <summary>
        /// Percentage of course that have been completd
        /// </summary>
        public decimal? PercentageCompletion { get; set; }

        /// <summary>
        /// Contain list of Tests to passed that considered as course is complete
        /// </summary>
        public string TestsPassed { get; set; }


        // ----- END COURSE COMPLETION CRITERIA


        public string LearningPath { get; set; }


        public string CreatedByName { get; set; }
        /// Evertyhing below is not used, here for reference/future use.
        //public CourseLevel RequiredLevel { get; set; }

        public bool Display { get; set; }


    }



    /// <summary>
    /// LifeCycle of the course, in sequence.
    /// </summary>
    public enum CourseStatus
    {
        Draft,
        Trial,
        Submitted,
        VerifierApproval,
        Verified,
        VerifierRejected,  // Should go back to draft
        FirstApproval,
        ApproverRejected, // Should go back to draft
        SecondApproval,
        ThirdApproval,
        
        Approved, // Ready to publish
        Published, // Fit for consumption
        Hidden, //The course is hidden froom search and view, however the point given is still valid
        Deleted,

    }

    /// <summary>
    /// How the course is conducted
    /// </summary>

    public enum CourseMedium
    {
        [Display(Name = "CourseMediumOnline", ResourceType = typeof(Language.eLearning.Course))]
        Online,

        Offline,
        Hybrid
    }

    public enum CourseScheduleType
    {
        Flexible,
        Fix,
        NoTimeLimit
    }

    /// <summary>
    /// The meaurement type for duration
    /// </summary>

    public enum DurationType
    {
        Hour,
        Day,
        Week,
        Month,
    }

    /// <summary>
    /// Type of level for each course. Also used for Required Level to take the course
    /// </summary>
    public enum CourseLevel
    {
        Beginner,
        Intermidiate,
        Advance
    }

    public enum CourseLanguage
    {
        English,
        Malay,
        Chinese,
        Tamil,
        Others
    }

    /// <summary>
    /// How learners are allowed to navigate through the course
    /// </summary>

    public enum TraversalRule
    {
        [Display(Name = "Free viewing of any unit")]
        Free,

        [Display(Name = "All modules must be seen and completed sequentially")]
        Sequential,
    }

    /// <summary>
    /// how elearning calculates the course’s score
    /// </summary>
    public enum ScoreCalculation
    {
        [Display(Name = "Average over all tests & assignments")]
        Average,

        [Display(Name = "Only Tests")]
        TestsOnly,

        [Display(Name = "Specific tests & assignments")]
        Specific,
    }


    public enum CompletionCriteriaType
    {
        [Display(Name = "All modules must be completed")]
        General,

        [Display(Name = "Certain modules must be completed")]
        ActivityCompletion,

        [Display(Name = "Perentage of modules must be completed")]
        PercentageCompletion,

        [Display(Name = "Tests passed")]
        TestsPassed,
    }
}