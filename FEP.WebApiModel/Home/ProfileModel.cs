using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Home
{
    public class AdminProfileModel
    {
        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }
        
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.User))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }
    }

    public class EditAdminProfileModel
    {
        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }
              
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.User))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }
    }

    public class CompanyProfileModel
    {

        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Company))]
        public string CompanyName { get; set; }

        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Company))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Company))]
        public SectorModel Sector { get; set; }

        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Company))]
        public MinistryModel Ministry { get; set; }

        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Company))]
        public string CompanyRegNo { get; set; }

        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Company))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Company))]
        public string PostCodeMalaysian { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Company))]
        public string PostCodeNonMalaysian { get; set; }

        [Display(Name = "FieldCity", ResourceType = typeof(Language.Company))]
        public string City { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Individual))]
        public StateModel State { get; set; }

        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Individual))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Company))]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Company))]
        public string Name { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Company))]
        public string ICNo { get; set; }

        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Company))]
        public string PassportNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Company))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Company))]
        public string Email { get; set; }
    }

    public class IndividualProfileModel
    {

        [Display(Name = "FieldName", ResourceType = typeof(Language.Individual))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Individual))]
        public string ICNo { get; set; }

        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Individual))]
        public string PassportNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        public string Email { get; set; }

        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Individual))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Individual))]
        public string PostCodeMalaysian { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Individual))]
        public string PostCodeNonMalaysian { get; set; }

        [Display(Name = "FieldCity", ResourceType = typeof(Language.Individual))]
        public string City { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Individual))]
        public StateModel State { get; set; }

        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Individual))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Individual))]
        public CountryModel Citizenship { get; set; }

    }

    public class EditIndividualProfileModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Individual))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Individual))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Individual))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Individual))]
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Individual))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Individual))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Individual))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Individual))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Individual))]
        public string PostCodeNonMalaysian { get; set; }

        [RegularExpression("[a-zA-Z]+")]
        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Individual))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Individual))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Individual))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Individual))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCitizenshipId", ErrorMessageResourceType = typeof(Language.Individual))]
        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Individual))]
        public int? CitizenshipId { get; set; }
        
        public IEnumerable<SelectListItem> Citizenships { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

    }

    public class EditCompanyProfileModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Company))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgencyName", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Company))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Company))]
        public int? SectorId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMinistryId", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Company))]
        public int? MinistryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyRegNo", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Company))]
        public string CompanyRegNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Company))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Company))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Company))]
        public string PostCodeNonMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Company))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Company))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Company))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Company))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Company))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Company))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Company))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Company))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Company))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Company))]
        public string MobileNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Company))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Company))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Company))]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Ministries { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Sectors { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
       
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
