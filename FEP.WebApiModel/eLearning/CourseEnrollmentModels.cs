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
    }

    public class ReturnListCourseEnrollmentModel
    {
        public FilterCourseEnrollmentModel Filters { get; set; }
        public ReturnBriefCourseEnrollmentModel CourseEnrollment { get; set; }
    }


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
