using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model.eLearning
{
    public class CourseContent : BaseEntity
    {

        public string Description { get; set; }

        public int? CourseModuleId { get; set; }
        public virtual CourseModule CourseModule { get; set; }

        public int? CourseId { get; set; }
        public virtual Course Course{ get; set; }

        /// <summary>
        /// Whether the content is viewable or not
        /// </summary>
        public bool IsViewable { get; set; }

        public CourseContentType ContentType { get; set; }

        public string Title { get; set; }

        public ContentCompletionType CompletionType { get; set; }

        public int? Order { get; set; }

        public QuestionType? QuestionType { get; set; }

        public int? Timer { get; set; } //completiontype timer in sec
    }

    public class Video : CourseContent
    {
        public string VideoUrl { get; set; }
        public int? VideoFileId { get; set; }
        public virtual ContentFile VideoFile { get; set; }
    }

    public class Audio : CourseContent
    {
        public string AudioUrl { get; set; }
        public int? AudioFileId { get; set; }
        public virtual ContentFile AudioFile { get; set; }
    }

    public class IFrame : CourseContent
    {
        public string IFrameUrl { get; set; }
        public ShowAs ShowAs { get; set; }
    }

    public class Document : CourseContent
    {
        public string DocumentUrl { get; set; }
        public int? DocumentFileId { get; set; }
        public virtual ContentFile DocumentFile { get; set; }
    }

    public class Section : CourseContent
    {
        public string Name { get; set; }
    }

    public class RichText : CourseContent
    {
        public string HtmlText { get; set; }
        public virtual ICollection<ContentFile> Images { get; set; }
    }

    public class ModuleTest : CourseContent
    {
    }

    public class Assignment : CourseContent
    {
    }

    public class InstructorLed : CourseContent
    {
    }

    public enum ShowAs
    {
        Embedded,
        Popup,
    }

    public enum QuestionType
    {
        [Display(Name = "Multiple choice")]
        MultipleChoice,

        [Display(Name = "Fill in the blank")]
        FillInBlank,

        [Display(Name = "Ordering ")]
        Ordering,

        [Display(Name = "Drag and drop ")]
        DragDrop,

        [Display(Name = "Free text")]
        FreeText,

        Random
    }

    public enum CourseContentType
    {
       // Section,
        RichText,
        WebLink,
        Document,
        Audio,
        Video,
        IFrame,

        // FLAG ISSUE : HOW TO DO CONTENT FOR TEST? THIS IS ONE BIG ITEM
        Test,

        Assignment,
        Instructor
    }

    public enum ContentCompletionType
    {
        [Display(Name = "Multiple choice")]
        ClickButton,

        AnswerQuestion,
        Timer
    }
}