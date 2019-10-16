using System.ComponentModel.DataAnnotations;

namespace FEP.Model.eLearning
{
    public class CourseCompletionCriteria : BaseEntity
    {
        public int CourseId { get; set; }

        public CompletionCriteriaType CompletionCriteriaType { get; set; }

        /// <summary>
        /// Contain list of modules to be completed that considered as course is complete
        /// </summary>
        public string ModulesCompleted { get; set; }

        /// <summary>
        /// Percentage of course that have been completd
        /// </summary>
        public decimal? PercentageCompletion { get; set; }

        /// <summary>
        /// Contain list of Tests to passed that considered as course is complete
        /// </summary>
        public string TestsPassed { get; set; }
    }


}