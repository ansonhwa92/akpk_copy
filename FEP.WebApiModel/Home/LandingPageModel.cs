using FEP.Model;
using FEP.WebApiModel.RnP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.Home
{
    public class LandingPageModel
    {
        public List<ReturnPublicationModel> PublicationTopList { get; set; }
    }
}
