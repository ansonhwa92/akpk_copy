using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.User))]
        public string CompanyName { get; set; }

        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.User))]
        public string CompanyRegNo { get; set; }

        [Display(Name = "FieldSector", ResourceType = typeof(Language.User))]
        public string Sector { get; set; }

        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.User))]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

    }

    public class CompanyListModel
    {
        public CompanyFilterModel Filter { get; set; }
        public CompanyModel List { get; set; }        
    }

    public class CompanyFilterModel
    {
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.User))]
        public string CompanyName { get; set; }

        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.User))]
        public string CompanyRegNo { get; set; }

        [Display(Name = "FieldSector", ResourceType = typeof(Language.User))]
        public int SectorId { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }
    }

}