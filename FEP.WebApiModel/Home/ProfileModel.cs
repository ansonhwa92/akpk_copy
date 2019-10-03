using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Home
{
    public class CompanyProfileModel
    {
                
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }
        
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public int? SectorId { get; set; }

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
                
        //[Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        //public int StateId { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public string State { get; set; }

        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Auth))]
        public string CompanyPhoneNo { get; set; }
                
        [Display(Name = "Representative Name")]
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

    public class EditIndividualProfileModel
    {
        [Required]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        public string Email { get; set; }
    }

    public class EditCompanyProfileModel
    {
        [Required]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public int? SectorId { get; set; }

        [Required]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Auth))]
        public string CompanyRegNo { get; set; }

        [Required]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Auth))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Auth))]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Auth))]
        public string City { get; set; }

        [Required]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public string State { get; set; }
                
        [Required]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Auth))]
        public string CompanyPhoneNo { get; set; }

        [Required]
        [Display(Name = "Representative Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        public string Email { get; set; }

        //public IEnumerable<System.Web.Mvc.SelectListItem> States { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Sectors { get; set; }
    }

    public class ChangePasswordModel
    {

        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        [Remote("CheckCurrentPassword", "Home", ErrorMessage = "Password entered not matched with current password.")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Remote("ValidatePassword", "Home")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

}
