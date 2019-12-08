using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        [Range(0.5, 1000, ErrorMessage = "Invalid Value")]
        public decimal? Duration { get; set; }

        public DurationType DurationType { get; set; }

        public CourseLanguage Language { get; set; }

        public bool IsFree { get; set; } = true;

        public decimal? Price { get; set; } = 0.0m;

        public virtual ICollection<CourseModule> Modules { get; set; }

        // For the front page, excluding modules, modules will be from Modules above
        //public virtual ICollection<CourseContent> FrontPageContents { get; set; }

        public CourseStatus Status { get; set; }

        public int? IntroMaterialId { get; set; }
        public virtual FileDocument IntroMaterial { get; set; }

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

        public int TotalModules { get; set; }

        // ----- Default Course Item
        /// <summary>
        /// Certificate for this course.
        /// </summary>
        public int? CourseCertificateId { get; set; }

        public virtual CourseCertificate DefaultCertificate { get; set; }
        public int? CourseCertificateTemplateId { get; set; }
        public virtual CourseCertificateTemplate CourseCertificateTemplate { get; set; }

        /// <summary>
        /// Trainers for this course.
        /// </summary>
        public virtual ICollection<Trainer> Trainers { get; set; }

        /// <summary>
        /// Default completion percentage before allowing learnet to withdraw.
        /// This value is required for Paid Course and will need to be copied to
        /// CourseEvent when CourseEvent is created
        /// </summary>
        public decimal DefaultAllowablePercentageBeforeWithdraw { get; set; } = 0.0m;

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

        public bool IsDeleted { get; set; }

        //Course Image
        public string IntroImageFileName { get; set; }

        public SkillLevel SkillLevel { get; set; }

        public int SLAReminderId { get; set; }

        public int TotalContents { get; set; }

        public void UpdateCourseStat()
        {
            TotalModules = this.Modules.Count();

            TotalContents = 0;
            foreach (var module in this.Modules)
            {
                if (module.ModuleContents != null)
                    TotalContents += module.ModuleContents.Count();
            }

            // TotalContents = this.Modules.Sum(x => x.ModuleContents?.Count()).Value;
        }
    }

    public enum SkillLevel
    {
        All,
        Beginner,
        Intermediate,
        Advanced,
    }

    /// <summary>
    /// LifeCycle of the course, in sequence.
    /// </summary>
    public enum CourseStatus
    {
        [Display(Name = "CourseStatusDraft", ResourceType = typeof(Language.eLearning.Enum))]
        Draft,  // 0

        [Display(Name = "CourseStatusTrial", ResourceType = typeof(Language.eLearning.Enum))]
        Trial,  // 1

        [Display(Name = "CourseStatusSubmitted", ResourceType = typeof(Language.eLearning.Enum))]
        Submitted, // 2

        [Display(Name = "CourseStatusVerified", ResourceType = typeof(Language.eLearning.Enum))]
        Verified, // 3

        [Display(Name = "CourseStatusApproved", ResourceType = typeof(Language.eLearning.Enum))]
        Approved, // Ready to publish 4

        [Display(Name = "CourseStatusAmendment", ResourceType = typeof(Language.eLearning.Enum))]
        Amendment, // 5

        [Display(Name = "CourseStatusPublished", ResourceType = typeof(Language.eLearning.Enum))]
        Published, // Fit for consumption 6

        [Display(Name = "CourseStatusFirstApproval", ResourceType = typeof(Language.eLearning.Enum))]
        FirstApproval, // 7

        [Display(Name = "CourseStatusSecondApproval", ResourceType = typeof(Language.eLearning.Enum))]
        SecondApproval,// 8

        [Display(Name = "CourseStatusThirdApproval", ResourceType = typeof(Language.eLearning.Enum))]
        ThirdApproval, //9

        [Display(Name = "CourseCancelled", ResourceType = typeof(Language.eLearning.Enum))]
        Cancelled,

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
        MultiLanguage,
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