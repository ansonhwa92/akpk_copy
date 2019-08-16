using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.WebApi.Models
{
    public class CreateSurveyApiModel
    {
        public SurveyType Type { get; set; }
        public SurveyCategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetGroup { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Contents { get; set; }
        public bool Active { get; set; }
        public string ProofOfApproval { get; set; }
    }

    public class EditSurveyApiModel
    {
        public SurveyType Type { get; set; }
        public SurveyCategory Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TargetGroup { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Contents { get; set; }
        public bool Active { get; set; }
        public string ProofOfApproval { get; set; }
    }

}