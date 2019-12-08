using FEP.Helper;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.eLearning
{
    /// <summary>
    /// Model for Home/Index page - listing of all Courses
    /// </summary>
    public class ReturnBriefCourseModel : BaseModel
    {
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Language.eLearning.Course))]
        public string Price { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.Course))]
        public CourseStatus Status { get; set; }
    }

    public class ReturnListCourseModel
    {
        public FilterCourseModel Filters { get; set; }
        public ReturnBriefCourseModel Courses { get; set; }
    }

    // class for setting and returning filters for the datatable list of publications
    public class FilterCourseModel : DataTableModel
    {
        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }
    }

    public class CreateOrEditCourseModel : BaseModel
    {
        [Required]
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description", ResourceType = typeof(Language.eLearning.Course))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        public RefCourseCategory Category { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }

        [AllowHtml]
        [Display(Name = "Objectives", ResourceType = typeof(Language.eLearning.Course))]
        public string Objectives { get; set; }

        [Display(Name = "Medium", ResourceType = typeof(Language.eLearning.Course))]
        public CourseMedium Medium { get; set; }

        [Display(Name = "ScheduleType", ResourceType = typeof(Language.eLearning.Course))]
        public CourseScheduleType ScheduleType { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Language.eLearning.Course))]
        public CourseLanguage Language { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(Language.eLearning.Course))]
        [Range(0.5, 1000, ErrorMessage = "Invalid Value")]
        public decimal Duration { get; set; }

        [Display(Name = "DurationType", ResourceType = typeof(Language.eLearning.Course))]
        public DurationType DurationType { get; set; }

        [Display(Name = "IsFree", ResourceType = typeof(Language.eLearning.Course))]
        public bool IsFree { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Language.eLearning.Course))]
        public decimal Price { get; set; }

        [Display(Name = "ViewCategory", ResourceType = typeof(Language.eLearning.Course))]
        public ViewCategory ViewCategory { get; set; }

        // --- CourseRule --

        [Display(Name = "TraversalRule", ResourceType = typeof(Language.eLearning.Course))]
        public TraversalRule TraversalRule { get; set; }

        [Display(Name = "ScoreCalculation", ResourceType = typeof(Language.eLearning.Course))]
        public ScoreCalculation ScoreCalculation { get; set; }

        [Display(Name = "CompletionRule", ResourceType = typeof(Language.eLearning.Course))]
        public CompletionCriteriaType CompletionCriteriaType { get; set; } = CompletionCriteriaType.General;

        /// <summary>
        /// Contain list of modules to be completed that considered as course is complete
        /// </summary>
        public string ModulesCompleted { get; set; }

        /// <summary>
        /// Percentage of course that have been completd
        /// </summary>
        [Display(Name = "PercentageCompletion", ResourceType = typeof(Language.eLearning.Course))]
        public decimal? PercentageCompletion { get; set; }

        /// <summary>
        /// Contain list of Tests to passed that considered as course is complete
        /// </summary>
        public string TestsPassed { get; set; }

        [Display(Name = "LearningPath", ResourceType = typeof(Language.eLearning.Course))]
        public string LearningPath { get; set; }

        [Display(Name = "CreatedByName", ResourceType = typeof(Language.eLearning.Course))]
        public string CreatedByName { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.Course))]
        public CourseStatus Status { get; set; }

        public ICollection<CourseApprovalLog> CourseApprovalLogs { get; set; }

        //public ICollection<CourseContent> FrontPageContents { get; set; }
        public ICollection<CourseModule> Modules { get; set; }

        public bool IsDeleted { get; set; }

        public int CourseEventId { get; set; }

        public HttpPostedFileBase File { get; set; }
        public string IntroImageFileName { get; set; }

        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }

        public CourseApprovalLogModel CourseApprovalLogModel { get; set; }

        // enrollmentcode, used when the user received a link with enrollmentcode
        public string EnrollmentCode { get; set; }

        // check for if user is enrolled
        public bool IsUserEnrolled { get; set; }

        public TrainerCourse Instructor { get; set; }
    }

    public class CourseListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CourseRuleModel : BaseModel
    {
        [Required]
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Display(Name = "TraversalRule", ResourceType = typeof(Language.eLearning.Course))]
        public TraversalRule TraversalRule { get; set; }

        [Display(Name = "ScoreCalculation", ResourceType = typeof(Language.eLearning.Course))]
        public ScoreCalculation ScoreCalculation { get; set; }

        [Display(Name = "CompletionRule", ResourceType = typeof(Language.eLearning.Course))]
        public CompletionCriteriaType CompletionCriteriaType { get; set; } = CompletionCriteriaType.General;

        /// <summary>
        /// Contain list of modules to be completed that considered as course is complete
        /// </summary>
        public string ModulesCompleted { get; set; }

        /// <summary>
        /// Percentage of course that have been completd
        /// </summary>
        [Display(Name = "PercentageCompletion", ResourceType = typeof(Language.eLearning.Course))]
        public decimal? PercentageCompletion { get; set; }

        /// <summary>
        /// Contain list of Tests to passed that considered as course is complete
        /// </summary>
        public string TestsPassed { get; set; }

        [Display(Name = "LearningPath", ResourceType = typeof(Language.eLearning.Course))]
        public string LearningPath { get; set; }

    }

    // firus
    public class CourseAdditionalInputModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Contents { get; set; }

        public string PageTitle { get; set; }
    }


    // firus
    public class CourseAdditionalInputResponseModel
    {
        public int Id { get; set; }

        public int InputId { get; set; }

        public int UserId { get; set; }

        public string Answers { get; set; }

        public int CourseId { get; set; }

        public string EnrollmentCode { get; set; }
    }

    public class TrainerCourseModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public ListUserModel Users { get; set; }
        public List<TrainerCourse> TrainerCourses { get; set; }
    }

    public class UpdateTrainerCourseModel
    {
        public int CourseId { get; set; }
        public List<int> UserId { get; set; }
    }

    public class ChangeCourseStatusModel : BaseModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CourseEventId { get; set; }
        public string Message { get; set; }
    }

    public class OrderModel
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public List<string> Order { get; set; }
    }

    public class DashboardCourseModel : BaseModel
    {
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Language.eLearning.Course))]
        public string Description { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Language.eLearning.Course))]
        public string Price { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.Course))]
        public CourseStatus Status { get; set; }

        public int TotalModules { get; set; }

        public int TotalStudent { get; set; }

        public string IntroImageFileName { get; set; }

        public string InstructorBy { get; set; }

    }

    // FOR ELEARNING HOME PAGE
    // Class for returning elearning for user browsing
    public class BrowseElearningModel
    {
        public string Keyword { get; set; }

        public string Sorting { get; set; }

        public int LastIndex { get; set; }

        public int ItemCount { get; set; }

        public List<ReturnElearningModel> Courses { get; set; }
    }

    public class ReturnElearningModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseLanguage Language { get; set; }
        public decimal Price { get; set; }
        public string Instructor { get; set; }
        public int TotalModules { get; set; }
        public int TotalStudent { get; set; }
        public CourseStatus Status { get; set; }
        public string IntroImageFileName { get; set; }

    }

    public class ReturnMyCoursesModel
    {
        public string Keyword { get; set; }

        public string Sorting { get; set; }

        public int LastIndex { get; set; }

        public int ItemCount { get; set; }

        public List<ReturnElearningModel> CoursesList { get; set; }

        //public int Id { get; set; }
        //public int CategoryId { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public CourseLanguage Language { get; set; }
        //public decimal Price { get; set; }
        //public string Instructor { get; set; }
        //public int TotalStudent { get; set; }
        //public CourseStatus Status { get; set; }
        //public string IntroImageFileName { get; set; }

    }
}