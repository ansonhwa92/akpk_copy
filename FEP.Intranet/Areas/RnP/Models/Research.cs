
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.RnP.Models
{
    public class SurveyModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        public SurveyType Type { get; set; }

        [Display(Name = "SurveyCategory", ResourceType = typeof(Language.RnPForm))]
        public SurveyCategory Category { get; set; }

        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Display(Name = "SurveyDescription", ResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        [Display(Name = "SurveyTargetGroup", ResourceType = typeof(Language.RnPForm))]
        public string TargetGroup { get; set; }

        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime StartDate { get; set; }

        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime EndDate { get; set; }

        [Display(Name = "SurveyActive", ResourceType = typeof(Language.RnPForm))]
        public bool Active { get; set; }

        [Display(Name = "SurveyPictures", ResourceType = typeof(Language.RnPForm))]
        public string Pictures { get; set; }

        [Display(Name = "SurveyProofOfApproval", ResourceType = typeof(Language.RnPForm))]
        public string ProofOfApproval { get; set; }
    }

    public class SurveyContentsModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

    public class SurveyFilterModel
    {
        [Display(Name = "SurveyType", ResourceType = typeof(Language.RnPForm))]
        public SurveyType Type { get; set; }

        [Display(Name = "SurveyCategory", ResourceType = typeof(Language.RnPForm))]
        public SurveyCategory Category { get; set; }

        [Display(Name = "SurveyTitle", ResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Display(Name = "SurveyTargetGroup", ResourceType = typeof(Language.RnPForm))]
        public string TargetGroup { get; set; }

        [Display(Name = "SurveyStartDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime StartDate { get; set; }

        [Display(Name = "SurveyEndDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime EndDate { get; set; }

        [Display(Name = "SurveyActive", ResourceType = typeof(Language.RnPForm))]
        public bool Active { get; set; }
    }

    public class SurveyListModel
    {
        public SurveyFilterModel Filter { get; set; }
        public List<SurveyModel> Surveys { get; set; }
    }

    public class SurveyApprovalModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyApprovalStatus", ResourceType = typeof(Language.RnPForm))]
        public SurveyApprovalStatus Status { get; set; }

        [Display(Name = "SurveyApprovalApprovalDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime ApprovalDate { get; set; }

        [Display(Name = "SurveyApprovalRemarks", ResourceType = typeof(Language.RnPForm))]
        public string Remarks { get; set; }

        [Display(Name = "SurveyApprovalRequireNext", ResourceType = typeof(Language.RnPForm))]
        public bool RequireNext { get; set; }    }

    public class SurveyResponseModel
    {
        public int ID { get; set; }

        [Display(Name = "SurveyResponseResponseDate", ResourceType = typeof(Language.RnPForm))]
        public DateTime ResponseDate { get; set; }

        [Display(Name = "SurveyResponseContents", ResourceType = typeof(Language.RnPForm))]
        public string Contents { get; set; }
    }

}
