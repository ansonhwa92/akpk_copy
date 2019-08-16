using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Home
{
    public class CompanyProfileModel
    {
                
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }
        
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public int SectorId { get; set; }

        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public string Sector { get; set; }

        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Auth))]
        public string CompanyRegNo { get; set; }
        
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Auth))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
                
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Auth))]
        public string PostCode { get; set; }
                
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Auth))]
        public string City { get; set; }
                
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public int StateId { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public string State { get; set; }

        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Auth))]
        public string CompanyPhoneNo { get; set; }
                
        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }
                
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }
                
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]        
        public string Email { get; set; }
    }

    public class IndividualProfileModel
    {

        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        public string Email { get; set; }
    }
}
