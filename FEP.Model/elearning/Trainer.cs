using System.Collections.Generic;

namespace FEP.Model.eLearning
{
    public class Trainer : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<TrainerCourse> Courses { get; set; }
        public virtual ICollection<TrainerGroup> Groups { get; set; }
    }
}