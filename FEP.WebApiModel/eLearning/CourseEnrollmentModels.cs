using FEP.Helper;
using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    // class for setting and returning filters for the datatable list of publications
    public class FilterCourseEnrollmentModel : DataTableModel
    {
        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }

        [Display(Name = "StudentName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string StudentName { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "SessionName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string SessionName { get; set; }

        public string Status { get; set; }
    }

    public class ReturnBriefCourseEnrollmentModel : BaseModel
    {
        [Display(Name = "StudentName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string StudentName { get; set; }

        [Display(Name = "PercentageCompleted", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string PercentageCompleted { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public EnrollmentStatus Status { get; set; }

        [Display(Name = "DateEnrolled", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string DateEnrolled { get; set; }

        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }

        [Display(Name = "SessionName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string SessionName { get; set; }

        [Display(Name = "CompletionDate", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string CompletionDate { get; set; }
    }

    public class ReturnListCourseEnrollmentModel
    {
        public FilterCourseEnrollmentModel Filters { get; set; }
        public ReturnBriefCourseEnrollmentModel CourseEnrollment { get; set; }
    }

    //**Wawa update start**//
    public class UserCourseEnrollmentModel : BaseModel
    {
        [Display(Name = "StudentName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string StudentName { get; set; }

        [Display(Name = "PercentageCompleted", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string CoursePercentageCompleted { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public EnrollmentStatus Status { get; set; }

        [Display(Name = "DateEnrolled", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string DateEnrolled { get; set; }

        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }

        public string CourseTitle { get; set; }

        [Display(Name = "SessionName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string SessionName { get; set; }

        [Display(Name = "CompletionDate", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string CompletionDate { get; set; }

        public bool IsUserEnrolled { get; set; }

        public int CourseProgressCount { get; set; }

        //set for continue 
        public int ProgressCourseContentId { get; set; }

        public ICollection<ReturnCourseProgressModel> CourseProgress { get; set; }
        public ICollection<EnrollmentHistory> EnrollmentHistory { get; set; }
    }

    public class ReturnCourseProgressModel
    {
        public int EnrollmentId { get; set; }
        public int Order { get; set; }
        public string ModuleName { get; set; }
        public bool IsCompleted { get; set; }
        public decimal Score { get; set; } = 0.0m;
        //public string ModuleCompletionDate { get; set; }
    }

    //**Wawa update end**//

    // class for setting and returning filters for the datatable list of publications
    public class FilterCourseEnrollmentHistoryModel : DataTableModel
    {
        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }

        [Display(Name = "StudentName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string StudentName { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }

    public class ReturnBriefCourseEnrollmentHistoryModel : BaseModel
    {
        [Display(Name = "CourseName", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string CourseTitle { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Name { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public EnrollmentStatus Status { get; set; }

        public string StatusDate { get; set; }

        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }
    }

    public class ReturnListCourseEnrollmentHistoryModel
    {
        public FilterCourseEnrollmentHistoryModel Filters { get; set; }
        public ReturnBriefCourseEnrollmentHistoryModel CourseEnrollment { get; set; }
    }
}