using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;

namespace FEP.WebApiModel
{
    public class ResearchApiModel
    {
        public int ID { get; set; }
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
        public DateTime? DateAdded { get; set; }
        public SurveyStatus Status { get; set; }
        public List<SurveyApproval> Approvals { get; set; }
    }

    public class ResearchResponseApiModel
    {

    }
}
