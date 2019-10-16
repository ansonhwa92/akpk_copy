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
    public class CourseModule : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        // order number of this module
        public int Order { get; set; } = 0;

        public string Objectives { get; set; }
        
        public int CourseId { get; set; }
        
        public virtual ICollection<CourseContent> ModuleContents { get; set; }

        public int TotalVideo { get; set; }       
        public int TotalAudio { get; set; }
        public int TotalDocument { get; set; }
        public int TotalContent { get; set; }
        public int TotalTest { get; set; }
        public int TotalRichText { get; set; }

        public int TotalAssignment { get; set; }

        public string IntroImageFileName { get; set; }

        public void UpdateTotals()
        {
            TotalRichText = this.ModuleContents.Where(x => x.ContentType == CourseContentType.RichText).Count();
            TotalVideo = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Video).Count();
            TotalAudio = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Audio).Count();
            TotalDocument = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Document).Count();
            TotalTest = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Test).Count();
            TotalAssignment = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Assignment).Count();

            TotalContent = this.ModuleContents.Count();        
        }
    }
}
