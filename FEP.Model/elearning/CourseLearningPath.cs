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
    public class CourselearningPath : BaseEntity
    {
        public int CourseId { get; set; }
        public int RequiredCourseId { get; set; }
        public virtual Course RequiredCourse { get; set; }

    }

}
