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
   public class Learner : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int Point { get; set; } = 0;

        public int Badges { get; set; } = 0;

        public int CourseEnrolled { get; set; } = 0;
        public int CourseCompleted { get; set; } = 0;
        public int TrainingTime { get; set; } =  0;

    }
}
