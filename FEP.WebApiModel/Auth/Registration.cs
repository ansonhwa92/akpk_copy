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

        public bool IsMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Auth))]        
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Auth))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]        
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceName = "ValidCompareRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessageResourceName = "FieldRequiredIsTermAgreed", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldIsTermAgreed", ResourceType = typeof(Language.Auth))]
        public bool IsTermAgreed { get; set; }

        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Auth))]
        public int? CitizenshipId { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Citizenships { get; set; }

    }


    public class RegisterAgencyModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }

        public bool IsLocal { get; set; }

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
                
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public string State { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Auth))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Auth))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Auth))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Auth))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Auth))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
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

        [Range(typeof(bool), "true", "true", ErrorMessageResourceName = "FieldRequiredIsTermAgreed", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldIsTermAgreed", ResourceType = typeof(Language.Auth))]
        public bool IsTermAgreed { get; set; }
        
        public IEnumerable<System.Web.Mvc.SelectListItem> States { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Sectors { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Countries { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldResetPasswordEmail", ResourceType = typeof(Language.Auth))]        
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

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
