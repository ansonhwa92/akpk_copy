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
        [Display(Name = "Name", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string UserName { get; set; }
        public int CourseId { get; set; }
    }

    public class ReturnBriefCourseEnrollmentModel : BaseModel
    {
        [Display(Name = "Name", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Name { get; set; }

        [Display(Name = "Progress", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public string Progress { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public EnrollmentStatus Status { get; set; }

        [Display(Name = "DateEnrolled", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public DateTime? DateEnrolled { get; set; }
        [Display(Name = "Session", ResourceType = typeof(Language.eLearning.CourseEvent))]
        public int CourseEventId { get; set; }
    }

    public class ReturnListCourseEnrollmentModel
    {
        public FilterCourseEnrollmentModel Filters { get; set; }
        public ReturnBriefCourseEnrollmentModel CourseEnrollment { get; set; }
    }

}
