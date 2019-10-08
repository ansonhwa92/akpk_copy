using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model.eLearning
{
    public class CourseContent : BaseEntity
    {
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        public string Description { get; set; }

        public int CourseModuleId { get; set; }

        public int CourseId { get; set; }

        // Content order in module
        public int Order { get; set; }

        [Required]
        public CourseContentType ContentType { get; set; }

        // For if contenttype = video
        public VideoType? VideoType { get; set; }
        // For if contenttype = audio
        public AudioType? AudioType { get; set; }

        // For if contenttype = video/youtube url, iframe
        public string Url { get; set; }

        //  upload file
        public int? ContentFileId { get; set; }

        public virtual ContentFile ContentFile { get; set; }

        // for rich text
        public string Text { get; set; }

        // for Iframe
        public ShowIFrameAs ShowIFrameAs { get; set; }

        // START content completion criteria

        [Display(Name = "CompletionCriteria", ResourceType = typeof(Language.eLearning.Content))]
        public ContentCompletionType CompletionType { get; set; } = ContentCompletionType.ClickButton;

        public QuestionType? QuestionType { get; set; }

        public int? QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public int Timer { get; set; } //completiontype timer in sec

        // END Content completion
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
        [Display(Name = "RichText", ResourceType = typeof(Language.eLearning.Enum))]
        RichText,

        [Display(Name = "Document", ResourceType = typeof(Language.eLearning.Enum))]
        Document,

        [Display(Name = "Audio", ResourceType = typeof(Language.eLearning.Enum))]
        Audio,

        [Display(Name = "Video", ResourceType = typeof(Language.eLearning.Enum))]
        Video,

        [Display(Name = "IFrame", ResourceType = typeof(Language.eLearning.Enum))]
        IFrame,

        // FLAG ISSUE : HOW TO DO CONTENT FOR TEST? THIS IS ONE BIG ITEM
        [Display(Name = "Test", ResourceType = typeof(Language.eLearning.Enum))]
        Test,

        [Display(Name = "Assignment", ResourceType = typeof(Language.eLearning.Enum))]
        Assignment,

        [Display(Name = "Instructor", ResourceType = typeof(Language.eLearning.Enum))]
        Instructor
    }

    public enum ContentCompletionType
    {
        [Display(Name = "Multiple choice")]
        ClickButton,

        AnswerQuestion,
        Timer
    }

    public enum CreateContentFrom
    {
        CourseFrontPage,
        Module
    }

    public enum VideoType
    {
        [Display(Name = "ExternalVideo", ResourceType = typeof(Language.eLearning.Enum))]
        ExternalVideo,

        [Display(Name = "UploadVideo", ResourceType = typeof(Language.eLearning.Enum))]
        UploadVideo,

        //Presentation
    }

    public enum AudioType
    {
        [Display(Name = "SavedAudio", ResourceType = typeof(Language.eLearning.Enum))]
        SavedAudio,

        [Display(Name = "UploadAudio", ResourceType = typeof(Language.eLearning.Enum))]
        UploadAudio,

        //Presentation
    }
}