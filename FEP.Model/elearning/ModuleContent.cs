using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model.eLearning
{
    public class ModuleContent : BaseEntity
    {

        public string Description { get; set; }

        public int CourseModuleId { get; set; }
        public virtual CourseModule CourseModule { get; set; }

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

    public class Video : ModuleContent
    {
        public string VideoUrl { get; set; }
        public int? VideoFileId { get; set; }
        public virtual ContentFile VideoFile { get; set; }
    }

    public class Audio : ModuleContent
    {
        public string AudioUrl { get; set; }
        public int? AudioFileId { get; set; }
        public virtual ContentFile AudioFile { get; set; }
    }

    public class IFrame : ModuleContent
    {
        public string IFrameUrl { get; set; }
        public ShowAs ShowAs { get; set; }
    }

    public class Document : ModuleContent
    {
        public string DocumentUrl { get; set; }
        public int? DocumentFileId { get; set; }
        public virtual ContentFile DocumentFile { get; set; }
    }

    public class Section : ModuleContent
    {
        public string Name { get; set; }
    }

    public class RichText : ModuleContent
    {
        public string HtmlText { get; set; }
        public virtual ICollection<ContentFile> Images { get; set; }
    }

    public class ModuleTest : ModuleContent
    {
    }

    public class Assignment : ModuleContent
    {
    }

    public class InstructorLed : ModuleContent
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