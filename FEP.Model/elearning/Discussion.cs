using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    public class Discussion : BaseEntity
    {
        public string Name { get; set; }
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int? ModuleId { get; set; }
        public virtual CourseModule Module { get; set; }
        public int UserId { get; set; }
        public int? GroupId { get; set; }
        public virtual DiscussionGroup Group { get; set; }

        // refer to first post in discussionpost
        public int FirstPost { get; set; }

        public bool Pinned { get; set; }

        public virtual ICollection<DiscussionPost> Posts { get; set; }

        public DiscussionVisibility DiscussionVisibility { get; set; }

    }

    public class DiscussionPost : BaseEntity
    {

        public int DiscussionId { get; set; }
        public virtual Discussion Discussion { get; set; }
        public int? ParentId { get; set; }
        public virtual DiscussionPost Parent { get; set; }

        [Index]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public virtual ICollection<FileDocument> Attachments { get; set; }
    }


    //public class PostQueue : BaseEntity
    //{
    //    [Index]
    //    public int DiscussionId { get; set; }
    //    public virtual Discussion Discussion { get; set; }
    //    [Index]
    //    public int DiscussionPostId { get; set; }
    //    public virtual DiscussionPost DiscussionPost { get; set; }

    //    [Index]
    //    public int UserId { get; set; }
    //    public virtual User User { get; set; }
    //}


    public enum DiscussionVisibility
    {
        [Display(Name = "Everybody can see this topic")]
        Everybody,
        [Display(Name = "Only group members can see this topic")]
        GroupOnly
    }


    // TODO:
    public class DiscussionGroup :  BaseEntity
    {

    }

    public class DiscussiongGroupMember : BaseEntity
    {

    }
}
