using FEP.Model;
using FEP.Model.eLearning;
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

        [Display(Name = "CompletionCriteria", ResourceType = typeof(Language.eLearning.Content))]
        public ContentCompletionType CompletionType { get; set; }

        [Display(Name = "CompleteATimer", ResourceType = typeof(Language.eLearning.Content))]
        public int? Timer { get; set; } //completiontype timer in sec

        [Display(Name = "CompleteAQuestion", ResourceType = typeof(Language.eLearning.Content))]
        public int? ContentQuestionId { get; set; }
        
        public string Url { get; set; }
     
        public HttpPostedFileBase FileName { get; set; }
        public int? FileDocumentId { get; set; }
        public FileDocument FileDocument { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }

        public CreateContentFrom CreateContentFrom { get; set; }

        public VideoType VideoType { get; set; }

        public int Order { get; set; }

        public int? ContentFileId { get; set; }
    }
}