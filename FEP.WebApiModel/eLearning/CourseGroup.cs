using FEP.Model;
using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    public class ListCourseGroupModel
    {
        public int Id { get; set; }
        public int LearnerId { get; set; }
        public string LearnerName { get; set; } //group owner
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } //group creator
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; } //group name
        public bool IsVisible { get; set; }
        public string EnrollmentCode { get; set; }
        public int? CourseEventId { get; set; }
        public int? CourseId { get; set; }
    }

    public class CreateCourseGroupModel
    {
        [Display(Name = "Group Name")]
        public string Name { get; set; } //group name
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Group Code")]
        public string EnrollmentCode { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class JoinCourseGroupModel
    {
        [Display(Name = "Group Code")]
        public string EnrollmentCode { get; set; }
        public int UserId { get; set; }
    }

    public class EditCourseGroupModel
    {
        [Display(Name = "Group Name")]
        public string Name { get; set; } //group name
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Group Code")]
        public string EnrollmentCode { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Id { get; set; }
    }

    public class ListGroupMemberModel
    {
        public int GroupId { get; set; }
        public string UserName { get; set; }
        public int CreatedBy { get; set; }
        public int UserId { get; set; }
        public int? CourseEnrolled { get; set; }
        public int? CourseCompleted { get; set; }
        public bool isOwner { get; set; } = false;
        public bool isMember { get; set; } = false;
        public int PriorityOrder { get; set; } = 0;
    }

    public class ListCourseModel
    {
        public int EventId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime EventCreatedOn { get; set; }
        public int? GroupId { get; set; }
        public int ThisGroupId { get; set; }
    }

}
