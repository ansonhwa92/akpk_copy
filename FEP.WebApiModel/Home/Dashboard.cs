using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Home
{
    public class DashboardList
    {
        public DashboardList()
        {
            DashboardItemList = new List<DashboardItemList>();
        }
        public DashboardModule ModuleName { get; set; }
        public List<DashboardItemList> DashboardItemList { get; set; }

    }

    public class DashboardItemList
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public int Count { get; set; }
        public string RedirectLink { get; set; }
    }

    public enum DashboardModule
    {
        [Display(Name = "Individual", ResourceType = typeof(Language.Dashboard))]
        Individual,
        [Display(Name = "Agency")]
        Agency,
        [Display(Name = "Course")]
        Courses,
        [Display(Name = "KMC")]
        KMC,
        [Display(Name = "ToT")]
        ToT,
        [Display(Name = "Public Event")]
        PublicEvent,
        [Display(Name = "Media Interview")]
        MediaInterview,
        [Display(Name = "Exhibition")]
        Exhibition,
        [Display(Name = "Research")]
        Research,
        [Display(Name = "Publication")]
        Publication,
        [Display(Name = "Survey")]
        Survey
    }

    public class DashboardModuleByRole
    {
        public DashboardModule DefaultModule { get; set; }
        public List<DashboardModule> AvailableModule { get; set; }
    }

}
