using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.eLearning
{
    public static class RoleNames
    {
        public static string eLearningAdmin = "Admin eLearning";
        public static string eLearningTrainer = "Trainer";

        public static string eLearningVerifier = "Verifier eLearning";
        public static string eLearningApprover1 = "Approver eLearning 1";
        public static string eLearningApprover2 = "Approver eLearning 2";
        public static string eLearningApprover3 = "Approver eLearning 3";
        public static string eLearningLearner = "Learner";

        public static string eLearningFacilitator = "Facilitator";
    }

    public static class Constants
    {
        // This is for content that belong to a course, not a module.
        public static int DefaultModule = -9999;
    }

    public enum Gamification
    {
        [Display(Name ="User Login")]
        UserLogin,

        [Display(Name = "User Enrolled")]
        UserEnrolled,

        [Display(Name = "Course Complete")]
        CourseComplete
    }

}
