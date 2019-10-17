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
    public class CourseDiscussionModel
    {
        public int Id { get; set; }
        [Display(Name = "Discussion Topic")]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatetedOn { get; set; }
        public int FirstPostId { get; set; }
        public string FirstPost { get; set; }
        public string DiscussionStatus { get; set; }
        public DiscussionVisibility DiscussionVisibility { get; set; }
    }

    public class CourseDiscussionVisibilityModel
    {
        public int Id { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
    }

    public class GroupCategoryModel
    {
        public int Id { get; set; }
        [Display(Name = "Group")]
        public string Name { get; set; }
    }

    public class CreateCourseDiscussionModel
    {
        //discussion topic
        public string Name { get; set; }

        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? GroupId { get; set; }
        public int? AttachmentId { get; set; }
        //discussion owner ?? createdby?
        public int UserId { get; set; }
        public int FirstPost { get; set; }
        public bool Pinned { get; set; }
        public DiscussionVisibility DiscussionVisibility { get; set; }
        public CreateCourseDiscussionPostModel Post { get; set; }

        //delete = true
        public bool? IsDeleted { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DiscussionVisibilities { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Groups { get; set; }
    }

    public class EditCourseDiscussionModel : BaseModel
    {
        //discussion topic
        public string Name { get; set; }

        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? GroupId { get; set; }

        //discussion owner ?? createdby?
        public int UserId { get; set; }


        public int FirstPost { get; set; }


        public bool Pinned { get; set; }


        public DiscussionVisibility DiscussionVisibility { get; set; }

        //delete = true
        public bool? IsDeleted { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DiscussionVisibilities { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Groups { get; set; }
    }

    public class CreateCourseDiscussionPostModel
    {
        public int DiscussionId { get; set; }
        public int? ParentId { get; set; }
        public int? UserId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? GotAttachment { get; set; }
    }

    public class CreateCourseDiscussionAttachmentModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; } //Byte
        public string FileType { get; set; }
        public string FileTag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileNameOnStorage { get; set; }
    }

    public class DiscussionView
    {
        public Discussion Discussion { get; set; }
        public List<DiscussionPostModel> Post { get; set; }
        public List<DiscussionAttachment> Attachment { get; set; }

        public CreateCourseDiscussionPostModel NewDiscussionReply { get; set; }
    }

    public class DiscussionPostModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DiscussionId { get; set; }
        public int? ParentId { get; set; }
        public int UserId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public bool? IsDeleted { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedByLevel { get; set; }
    }

    public class DiscussionAttachmentModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DiscussionId { get; set; }
        public int? ParentId { get; set; }
        public int UserId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public bool? IsDeleted { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedByLevel { get; set; }
    }
}
