using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model.eLearning
{
    public class CourseContent : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int? CourseModuleId { get; set; }        

        public int? CourseId { get; set; }        

        /// <summary>
        /// Whether the content is viewable or not
        /// </summary>
        public bool IsViewable { get; set; }

        public CourseContentType ContentType { get; set; }

        public ContentCompletionType CompletionType { get; set; }

        public int? Order { get; set; }

        public QuestionType? QuestionType { get; set; }

        public int? Timer { get; set; } //completiontype timer in sec

        public string Url { get; set; }
        public int? FileId { get; set; }
        public virtual ContentFile File { get; set; }

        // for rich text, used with summernote
        public string Text { get; set; }
        public ShowIFrameAs ShowIFrameAs { get; set; }

        public virtual ICollection<ContentFile> Images { get; set; }
        public virtual ICollection<ContentFile> Files { get; set; }
    }


    public enum ShowIFrameAs
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