using System;
using System.Collections.Generic;
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

        public List<DashboardItemList> DashboardItemList { get; set; }
    }

    public class DashboardItemList
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public int Count { get; set; }
    }
}
