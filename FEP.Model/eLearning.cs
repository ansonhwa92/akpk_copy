//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace FEP.Model.eLearning.Old
//{
//    [Table("LearningCourse")]
//    public class LearningCourse
//    {
//        [Key]
//        public int Id { get; set; }

//        public string Code { get; set; }

//        public string Title { get; set; }

//        public string Description { get; set; }

//        public string URL { get; set; }

//        public string Objective { get; set; }

//        public int CategoryId { get; set; }

//        public CourseStatus Status { get; set; }

//        public int DurationtoComplete { get; set; } //days

//        public string IntroVideoPath { get; set; }

//        public int LevelRequired { get; set; } // 1-20 learner level required to enroll

//        public bool IsHide { get; set; } //hide from public list

//        public bool IsPaid { get; set; }

//        public float Price { get; set; }

//        public int? CertificateId { get; set; }

//        public DateTime? LastUpdate { get; set; }

//        public int? ApprovalId1 { get; set; }

//        public int? ApprovalId2 { get; set; }

//        public int? ApprovalId3 { get; set; }

//        public int? ApprovalId4 { get; set; }

//        [ForeignKey("CategoryId")]
//        public virtual LearningCourseCategory Category { get; set; }

//        [ForeignKey("ApprovalId1")]
//        public virtual CourseApproval Approval1 { get; set; }

//        [ForeignKey("ApprovalId2")]
//        public virtual CourseApproval Approval2 { get; set; }

//        [ForeignKey("ApprovalId3")]
//        public virtual CourseApproval Approval3 { get; set; }

//        [ForeignKey("ApprovalId4")]
//        public virtual CourseApproval Approval4 { get; set; }

//        [ForeignKey("CertificateId")]
//        public virtual LearningCourseCertificate Certificate { get; set; }
//    }

//    public class LearningCourseCertificate
//    {
//        [Key]
//        public int Id { get; set; }

//        public string BackgroundImage { get; set; } //image64 base

//        public string Template { get; set; } //html text

//    }

//    //public enum CourseStatus
//    //{
//    //    Draft,
//    //    Trial,
//    //    Active
//    //}

//    [Table("LearningCourseCategory")]
//    public class LearningCourseCategory
//    {
//        [Key]
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public bool Display { get; set; }
//    }

//    [Table("Learner")]
//    public class Learner
//    {
//        [Key, ForeignKey("User")]
//        public int UserId { get; set; }

//        public int Point { get; set; }

//        public int Level { get; set; }

//        public virtual User User { get; set; }
//    }

//    [Table("CourseAdministrator")]
//    public class CourseAdministrator
//    {
//        [Key]
//        public int Id { get; set; }

//        public int UserId { get; set; }

//        public int CourseId { get; set; }

//        [ForeignKey("UserId")]
//        public virtual User Administrator { get; set; }

//        [ForeignKey("CourseId")]
//        public virtual LearningCourse Course { get; set; }
//    }

//    [Table("CourseLearner")]
//    public class CourseLearner
//    {
//        [Key]
//        public int Id { get; set; }

//        public int UserId { get; set; }

//        public int CourseId { get; set; }

//        public DateTime? EnrollDate { get; set; }

//        public DateTime? CompleteDate { get; set; }

//        [ForeignKey("UserId")]
//        public virtual User Learner { get; set; }

//        [ForeignKey("CourseId")]
//        public virtual LearningCourse Course { get; set; }

//    }

//    [Table("LearnerBadge")]
//    public class LearnerBadge
//    {
//        [Key]
//        public int Id { get; set; }
//        public int UserId { get; set; }
//        public GamificationBadgeType BadgeType { get; set; }
//        public DateTime AchieveDate { get; set; }

//        [ForeignKey("UserId")]
//        public virtual User Learner { get; set; }

//    }

//    [Table("CourseInstructor")]
//    public class CourseInstructor
//    {
//        [Key]
//        public int Id { get; set; }

//        public int UserId { get; set; }

//        public int CourseId { get; set; }

//        [ForeignKey("UserId")]
//        public virtual User Instructor { get; set; }

//        [ForeignKey("CourseId")]
//        public virtual LearningCourse Course { get; set; }
//    }

//    //[Table("CourseModule")]
//    //public class CourseModule
//    //{
//    //    [Key]
//    //    public int Id { get; set; }

//    //    public string Title { get; set; }

//    //    public int CourseId { get; set; }

//    //    [ForeignKey("CourseId")]
//    //    public virtual LearningCourse Course { get; set; }

//    //}

//    [Table("CourseContent")]
//    public class CourseContent
//    {
//        [Key]
//        public int Id { get; set; }

//        public int ModuleId { get; set; }

//        public CourseContentType Type { get; set; }

//        public string Title { get; set; }

//        public ContentCompletionType CompletionType { get; set; }

//        public int Order { get; set; }

//        public QuestionType? QuestionType { get; set; }

//        public int? Timer { get; set; } //completiontype timer in sec

//        [ForeignKey("ModuleId")]
//        public virtual CourseModule Module { get; set; }
//    }

//    public enum ContentCompletionType
//    {
//        ClickButton,
//        AnswerQuestion,
//        Timer
//    }

//    public enum QuestionType
//    {
//        MultipleChoice,
//        FillInBlank,
//        Ordering,
//        DragDrop,
//        FreeText,
//        Random
//    }

//    public enum CourseContentType
//    {
//        Section,
//        RichText,
//        WebLink,
//        Document,
//        Audio,
//        Video,
//        Test,
//        Assignment,
//        Instructor
//    }


//    [Table("CourseFile")]
//    public class CourseFile
//    {
//        [Key]
//        public int Id { get; set; }

//        public int CourseId { get; set; }

//        public int FileId { get; set; }

//        public CourseMaterialType FileType { get; set; }

//        [ForeignKey("CourseId")]
//        public virtual LearningCourse Course { get; set; }

//        [ForeignKey("FileId")]
//        public FileDocument FileDocument { get; set; }
//    }

//    public enum CourseMaterialType
//    {
//        Document,
//        Image,
//        Slide,
//        Video,
//        Audio,
//        Flash
//    }

//    //[Table("CourseApproval")]
//    //public class CourseApproval
//    //{
//    //    [Key]
//    //    public int Id { get; set; }
//    //    public int ApproverId { get; set; }
//    //    public DateTime? ApprovedDate { get; set; }
//    //    public string Remark { get; set; }
//}

//[Table("CourseWithdraw")]

//public class CourseWithdraw
//{
//    [Key]
//    public int Id { get; set; }

//    public int UserId { get; set; }

//    public int CourseId { get; set; }

//    public string Reason { get; set; }

//    public int? ApprovalId1 { get; set; }

//    public int? ApprovalId2 { get; set; }

//    public int? ApprovalId3 { get; set; }

//    public int? ApprovalId4 { get; set; }

//    [ForeignKey("UserId")]
//    public virtual User Learner { get; set; }

//    [ForeignKey("CourseId")]
//    public virtual LearningCourse Course { get; set; }

//    [ForeignKey("ApprovalId1")]
//    public virtual CourseApproval Approval1 { get; set; }

//    [ForeignKey("ApprovalId2")]
//    public virtual CourseApproval Approval2 { get; set; }

//    [ForeignKey("ApprovalId3")]
//    public virtual CourseApproval Approval3 { get; set; }

//    [ForeignKey("ApprovalId4")]
//    public virtual CourseApproval Approval4 { get; set; }

//}

//[Table("GamificationPoint")]
//public class GamificationPoint
//{
//    [Key]
//    public int Id { get; set; }

//    public GamificationPointType Type { get; set; }

//    public int Value { get; set; }

//    public bool IsEnable { get; set; }

//}

//public enum GamificationPointType
//{
//    Login,
//    TopicCompletion,
//    CourseCompletion,
//    TestCompletion,
//    AssignmentCompletion,
//    CommentSend,
//    Upvote,

//}

//[Table("GamificationLevel")]
//public class GamificationLevel
//{
//    [Key]
//    public int Id { get; set; }

//    public GamificationLevelType Type { get; set; }

//    public int Value { get; set; }

//    public bool IsEnable { get; set; }

//}

//public enum GamificationLevelType
//{
//    PointAchieve,
//    BadgeCollect,
//    CourseCompletion,
//}

//[Table("GamificationBadge")]
//public class GamificationBadge
//{
//    [Key]
//    public int Id { get; set; }

//    public GamificationBadgeType Type { get; set; }

//    public string Title { get; set; }

//    public int Value { get; set; }

//    public string IconPath { get; set; }

//    public bool IsEnable { get; set; }

//}

//public enum GamificationBadgeType
//{
//    Activity1,
//    Activity2,
//    Activity3,
//    Activity4,
//    Activity5,
//    Activity6,
//    Activity7,
//    Activity8,
//    Learning1,
//    Learning2,
//    Learning3,
//    Learning4,
//    Learning5,
//    Learning6,
//    Learning7,
//    Learning8,
//    Test1,
//    Test2,
//    Test3,
//    Test4,
//    Test5,
//    Test6,
//    Test7,
//    Test8,
//    Assignment1,
//    Assignment2,
//    Assignment3,
//    Assignment4,
//    Assignment5,
//    Assignment6,
//    Assignment7,
//    Assignment8,
//    Perfect1,
//    Perfect2,
//    Perfect3,
//    Perfect4,
//    Perfect5,
//    Perfect6,
//    Perfect7,
//    Perfect8,
//    Survey1,
//    Survey2,
//    Survey3,
//    Survey4,
//    Survey5,
//    Survey6,
//    Survey7,
//    Survey8,
//    Communication1,
//    Communication2,
//    Communication3,
//    Communication4,
//    Communication5,
//    Communication6,
//    Communication7,
//    Communication8,
//    Certification1,
//    Certification2,
//    Certification3,
//    Certification4,
//    Certification5,
//    Certification6,
//    Certification7,
//    Certification8,

//}

//public class LearningGroup
//{
//    [Key]
//    public int Id { get; set; }

//    public string Name { get; set; }

//    public string SharedKey { get; set; } //allow other user self register in the group

//    public DateTime CreatedDate { get; set; }
//}

//public class LearningGroupMember
//{

//    [Key]
//    public int Id { get; set; }

//    public int UserId { get; set; }

//    public int GroupId { get; set; }

//    public DateTime CreatedDate { get; set; }

//    [ForeignKey("UserId")]
//    public virtual User Member { get; set; }

//    [ForeignKey("GroupId")]
//    public virtual LearningGroup Group { get; set; }

//}

//public class LearningDiscussion
//{

//    [Key]
//    public int Id { get; set; }

//    public string Topic { get; set; }

//    public LearningViewCategory ViewCategory { get; set; }

//    public int CreatedBy { get; set; }

//    public DateTime CreatedDate { get; set; }

//    public DateTime LatestReply { get; set; }

//    [ForeignKey("CreatedBy")]
//    public virtual User User { get; set; }

//}

//public class LearningDiscussionView
//{

//    [Key]
//    public int Id { get; set; }

//    public int TopicId { get; set; }

//    public LearningViewType Type { get; set; }

//    public int? GroupId { get; set; }

//    public int? CourseId { get; set; }

//    public int? UserId { get; set; }

//    [ForeignKey("TopicId")]
//    public virtual LearningDiscussion Topic { get; set; }

//    [ForeignKey("UserId")]
//    public virtual User User { get; set; }

//    [ForeignKey("GroupId")]
//    public virtual LearningGroup Group { get; set; }

//    [ForeignKey("CourseId")]
//    public virtual LearningCourse Course { get; set; }

//}


//public class LearningDiscussionReply
//{
//    [Key]
//    public int Id { get; set; }

//    public int TopicId { get; set; }

//    public string Message { get; set; }

//    public int CreatedBy { get; set; }

//    public DateTime CreatedDate { get; set; }

//    public int UpvoteNo { get; set; }

//    [ForeignKey("CreatedBy")]
//    public virtual User User { get; set; }

//    [ForeignKey("TopicId")]
//    public virtual LearningDiscussion Topic { get; set; }
//}

//public class LearningDiscussionReplyUpvote
//{
//    public int Id { get; set; }

//    public int ReplyId { get; set; }
//    public int CreatedBy { get; set; }

//    public DateTime CreatedDate { get; set; }

//    [ForeignKey("CreatedBy")]
//    public virtual User User { get; set; }

//    [ForeignKey("ReplyId")]
//    public virtual LearningDiscussionReply Reply { get; set; }
//}


//public class LearningDiscussionAttachment
//{
//    [Key]
//    public int Id { get; set; }

//    public int ReplyId { get; set; }

//    public int AttachmentId { get; set; }

//    [ForeignKey("ReplyId")]
//    public virtual LearningDiscussionReply Reply { get; set; }

//    [ForeignKey("AttachmentId")]
//    public virtual FileDocument Attachment { get; set; }
//}

//public class LearningDiscussionComment
//{
//    [Key]
//    public int Id { get; set; }

//    public int ReplyId { get; set; }

//    public string Message { get; set; }

//    public int CreatedBy { get; set; }

//    public DateTime CreatedDate { get; set; }

//    [ForeignKey("ReplyId")]
//    public virtual LearningDiscussionReply Reply { get; set; }

//    [ForeignKey("CreatedBy")]
//    public virtual User User { get; set; }
//}

//public class LearningEvent
//{
//    [Key]
//    public int Id { get; set; }

//    public string Name { get; set; }

//    public string Description { get; set; }

//    public DateTime EventDate { get; set; }

//    public int Duration { get; set; } //minute

//    public LearningViewCategory ViewCategory { get; set; }

//    public string ColorCode { get; set; } //appear at calendar

//    public int CreatedBy { get; set; }

//    public DateTime CreatedDate { get; set; }

//}

//public enum LearningViewCategory
//{
//    Private,
//    Public,
//    Selected
//}


//public class LearningEventAudience
//{
//    [Key]
//    public int Id { get; set; }

//    public int LearningEventId { get; set; }

//    public LearningViewType Type { get; set; }

//    public int? GroupId { get; set; }

//    public int? CourseId { get; set; }

//    public int? UserId { get; set; }

//    [ForeignKey("LearningEventId")]
//    public virtual LearningEvent Event { get; set; }

//    [ForeignKey("UserId")]
//    public virtual User User { get; set; }

//    [ForeignKey("GroupId")]
//    public virtual LearningGroup Group { get; set; }

//    [ForeignKey("CourseId")]
//    public virtual LearningCourse Course { get; set; }

//}

//public enum LearningViewType
//{
//    User,
//    Group,
//    Course
//}

//}
