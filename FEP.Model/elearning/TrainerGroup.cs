namespace FEP.Model.eLearning
{
    public class TrainerGroup : BaseEntity
    {
        public int TrainerId { get; set; }
        public virtual Trainer Trainer { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}