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
    public class ContentFile : BaseEntity
    {
        /// <summary>
        /// Order of appearance in the content
        /// </summary>
        public int Order { get; set; }
        public int ModuleContentId { get; set; }
        public virtual CourseContent ModuleContent { get; set; }

        public FileType FileType { get; set; }

        public string FileName { get; set; }

        public int FileDocumentId { get; set; }
        public virtual FileDocument FileDocument { get; set; }

        public int CourseId { get; set; }

    }

    //public enum CourseMaterialType
    //{
    //    Document,
    //    Image,
    //    Slide,
    //    Video,
    //    Audio,
    //    Flash
    //}
}
