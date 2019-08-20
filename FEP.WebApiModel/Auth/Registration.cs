using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace FEP.WebApiModel.Auth
{
    public class RegisterIndividualModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please agree to Terms of Use")]
        [Display(Name = "I agree to the Terms of Use")]
        public bool IsTermAgreed { get; set; }

    }


    public class RegisterAgencyModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public int SectorId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyRegNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Auth))]
        public string CompanyRegNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Auth))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Auth))]
        public string PostCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Auth))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Auth))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please agree to Terms of Use")]
        [Display(Name = "I agree to the Terms of Use")]
        public bool IsTermAgreed { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> States { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Sectors { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Please enter your email")]
        public string Email { get; set; }
    }

    public class ResetPasswordResponseModel
    {
        public string Name { get; set; }
        public string UID { get; set; }
    }

    public class SetPasswordModel
    {
        [Required]
        public int PasswordResetId { get; set; }

        [Required]
        public string UID { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
