namespace FEP.Model.eLearning
{
    public class TrainerCourse : BaseEntity
    {
        public int TrainerId { get; set; }
        public virtual Trainer Trainer { get; set; }
        public int CourseEventId { get; set; }
        public CourseEvent CourseEvent { get; set; }
        public int CourseId { get; set; }
    }
}