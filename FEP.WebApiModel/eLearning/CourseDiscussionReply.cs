using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    public class CourseDiscussionReplyModel
    {
        public CreateCourseDiscussionReplyModel Reply { get; set; }
        public CreateCourseDiscussionReplyUpVoteModel Vote { get; set; }
        public CreateCourseDiscussionReplyAttachmentModel Attachment { get; set; }
    }

    public class CreateCourseDiscussionReplyModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpvoteNo { get; set; }
    }

    public class CreateCourseDiscussionReplyUpVoteModel
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateCourseDiscussionReplyAttachmentModel
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public int AttachmentId { get; set; }
    }
}
