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
        public int Order { get; set; }

        public string Objectives { get; set; }
        
        public int CourseId { get; set; }
        
        public virtual ICollection<CourseContent> ModuleContents { get; set; }

        public int TotalVideo { get; set; }

        
        public int TotalAudio { get; set; }

        
        public int TotalTest { get; set; }

        public int TotalAssignment { get; set; }


        public void GetModuleStatistic()
        {
            TotalVideo = this.ModuleContents.Where(x => x.ContentType == CourseContentType.Video).Count();

        }
    }
}
