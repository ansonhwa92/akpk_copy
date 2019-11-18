using FEP.Model;
using FEP.Model.eLearning;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.eLearning
{
    public class CreateOrEditContentModel : BaseModel
    {
        [Required]
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Content))]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Description", ResourceType = typeof(Language.eLearning.Course))]
        public string Description { get; set; }


        [AllowHtml]
        [Display(Name = "RichText", ResourceType = typeof(Language.eLearning.Enum))]
        public string Text { get; set; }

        public int ParentId { get; set; }

        public int CourseModuleId { get; set; }

        public int CourseId { get; set; }
        public string PageTitle { get; set; }

        /// <summary>
        /// Whether the content is viewable or not
        /// </summary>
        public bool IsViewable { get; set; } = true;

        public CourseContentType ContentType { get; set; }

        [AllowHtml]
        public string Url { get; set; }
     
        public HttpPostedFileBase File { get; set; }
        public int? FileDocumentId { get; set; }
        public FileDocument FileDocument { get; set; }
        public string UploadFileName { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }

        public CreateContentFrom CreateContentFrom { get; set; }

        public VideoType VideoType { get; set; }
        public AudioType AudioType { get; set; }
        public DocumentType DocumentType { get; set; }

        public int Order { get; set; }

        public int? ContentFileId { get; set; }

        public ShowIFrameAs ShowIFrameAs { get; set; }

        
        [Display(Name = "CompletionCriteria", ResourceType = typeof(Language.eLearning.Content))]
        public ContentCompletionType CompletionType { get; set; }

        [Display(Name = "CompleteATimer", ResourceType = typeof(Language.eLearning.Content))]
        public int? Timer { get; set; } //completiontype timer in sec

        [Display(Name = "CompleteAQuestion", ResourceType = typeof(Language.eLearning.Content))]
        public int? ContentQuestionId { get; set; }
        public string Question { get; set; }
        public string IntroImageFileName { get; set; }

        public string ModuleName { get; set; }

        public CourseStatus Status { get; set; }

        [Range(5, 5000)]
        public int? Height { get; set; } = 100;
        [Range(5, 5000)]
        public int? Width { get; set; } = 100;
    }


    public class ContentCompletionModel : BaseEntity
    {
        public string Title { get; set; }
        public int ContentId { get; set; }
        public int? CourseModuleId { get; set; }
        public int? CourseId { get; set; }

        public int UserId { get; set; }

        public ContentCompletionType CompletionType { get; set; }
        public int? Timer { get; set; }


        public int? QuestionId { get; set; }
        public string Question { get; set; }
        public ICollection<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }    
        public int? MultipleChoiceAnswerId { get; set; }
        // For ordering and fill the gap answers
        public ICollection<OrderAnswer> OrderAnswers { get; set; }
        public string OrderAnswerString { get; set; }
        public ICollection<FreeTextAnswer> FreeTextAnswers { get; set; }
        public string FreeTextAnswer { get; set; }

        public int? nextModuleId { get; set; }
        public int? nextContentId { get; set; }

        public bool IsCompleted { get; set; }


    }

    public class DeleteContentModel
    {
        public string Title { get; set; }
        public int? CourseModuleId { get; set; }
        public int? CourseId { get; set; }

    }

    public class AudioListModel
    {
        public int Id { get; set; }
        public int FileDocumentId { get; set; }
        public int ContentId { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public string FileNameOnStorage { get; set; }
    }

    public class DocumentListModel : AudioListModel
    {        
    }
}