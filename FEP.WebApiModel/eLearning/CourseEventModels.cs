using FEP.Helper;
using FEP.Model.eLearning;
using FEP.WebApiModel.Administration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FEP.WebApiModel.eLearning
{
    public class CourseEventModel : BaseModel
    {
        public string EnrollmentCode { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        // The coursestatus at the point of this event, can be either Approved or Draft, changed afterward
        public CourseEventStatus Status { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
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
        public Group Group {get; set;}
    }

    public class UpdateLearnerEnrollmentModel
    {
        public int CourseEventId { get; set; }
        public List<int> UserId { get; set; }
    }

}