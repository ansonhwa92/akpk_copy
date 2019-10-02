using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model.eLearning
{
    // WIP
    public class Question : BaseEntity
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        // objective questions
        public ICollection<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }

        public int MultipleChoiceAnswerId { get; set; }

        // For ordering and fill the gap answers
        public ICollection<OrderAnswer> OrderAnswers { get; set; }

        public string OrderAnswerString { get; set; }

        // free text
        public ICollection<FreeTextAnswer> FreeTextAnswers { get; set; }

        public string FreeTextAnswer { get; set; }
    }

    public class MultipleChoiceAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int Order { get; set; }
        public string Answer { get; set; }
    }

    public class OrderAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int Order { get; set; }
        public string Answer { get; set; }
    }

    public class FreeTextAnswer : BaseEntity
    {
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int Order { get; set; }
        public string Answer { get; set; }
        public int Point { get; set; }
    }



    public class ContentQuestion : BaseEntity
    {
        public int ContentId { get; set; }
        public int Order { get; set; }
        public int? CourseId { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}