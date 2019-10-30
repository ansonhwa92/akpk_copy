using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FEP.WebApiModel.eLearning
{
    public class CourseEventModel : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string EnrollmentCode { get; set; }

        [Required]
        public int CourseId { get; set; }

        // The coursestatus at the point of this event, can be either Approved or Draft, changed afterward
        public CourseEventStatus Status { get; set; }

        public bool HasGroup { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public ViewCategory ViewCategory { get; set; }

        // Required if published for trial
        public string TrialRemark { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Value must be between 0 to 100.")]
        public decimal AllowablePercentageBeforeWithdraw { get; set; } = 0.0m;
    }

    public class LearnerEnrollmentModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public CourseStatus CourseStatus { get; set; }

        public string EnrollmentCode { get; set; }
        public int CourseEventId { get; set; }
        public CourseEvent CourseEvent { get; set; }

        public int LearnerId { get; set; }
        public Learner Learner { get; set; }

        public ListUserModel Users { get; set; }
        public List<Enrollment> Enrollments { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }

    public class UpdateLearnerEnrollmentModel
    {
        public int CourseEventId { get; set; }
        public List<int> UserId { get; set; }
    }

    public class InviteLearnerModel
    {
        public int CourseId { get; set; }
        public int CourseEventId { get; set; }
        public string LearnerEmails { get; set; } //emails separated by comma
        public int GroupId { get; set; }
        public string EnrollmentCode { get; set; }
        public string CourseTitle { get; set; }
        public string CourseCode { get; set; }
        public string CreatedBy { get; set; }
    }


    public class ReturnListCourseEventModel
    {
        public FilterCourseEventModel Filters { get; set; }
        public ReturnBriefCourseEventModel CourseEvents { get; set; }
    }

    // class for setting and returning filters for the datatable list of publications
    public class FilterCourseEventModel : DataTableModel
    {
        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }

        [Display(Name = "EnrollmentCode", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string EnrollmentCode { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Name { get; set; }
        public int CourseId { get; set; }

    }

    public class ReturnBriefCourseEventModel : BaseModel
    {
        [Display(Name = "Name", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Name { get; set; }

        [Display(Name = "EnrollmentCode", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string EnrollmentCode { get; set; }

        [Display(Name = "NumberOfLearners", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int NumberOfLearners { get; set; }

        [Display(Name = "Group", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Group { get; set; }
        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }
    }



    public class TrxResult<T>
    {
        public int CourseId { get; set; }
        public int ObjectId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}