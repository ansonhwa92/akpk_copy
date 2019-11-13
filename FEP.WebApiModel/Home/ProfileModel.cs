using FEP.Model;
using FEP.WebApiModel.Administration;
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
    public class AdminProfileModel
    {
        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public string AvatarImageBase64 { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Profile))]
        public string Email { get; set; }
    }


    public class StaffProfileModel
    {

        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public string AvatarImageBase64 { get; set; }

        [Display(Name = "FieldStaffId", ResourceType = typeof(Language.Staff))]
        public string StaffId { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Staff))]
        public string Name { get; set; }

        [Display(Name = "FieldDepartment", ResourceType = typeof(Language.Staff))]
        public DepartmentModel Department { get; set; }

        [Display(Name = "FieldBranch", ResourceType = typeof(Language.Staff))]
        public BranchModel Branch { get; set; }

        [Display(Name = "FieldDesignation", ResourceType = typeof(Language.Staff))]
        public DesignationModel Designation { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Staff))]
        public string Email { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Staff))]
        public string ICNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Staff))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Staff))]
        public bool Status { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Individual))]
        public List<RoleModel> Roles { get; set; }
    }

    public class IndividualProfileModel
    {
        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public string AvatarImageBase64 { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Profile))]
        public string ICNo { get; set; }

        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Profile))]
        public string PassportNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Profile))]
        public string Email { get; set; }

        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Profile))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeMalaysian { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeNonMalaysian { get; set; }

        [Display(Name = "FieldCity", ResourceType = typeof(Language.Profile))]
        public string City { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public StateModel State { get; set; }

        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Profile))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Profile))]
        public CountryModel Citizenship { get; set; }

    }

    public class CompanyProfileModel
    {
        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public string AvatarImageBase64 { get; set; }

        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Profile))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Profile))]
        public SectorModel Sector { get; set; }

        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Profile))]
        public MinistryModel Ministry { get; set; }

        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Profile))]
        public string CompanyRegNo { get; set; }

        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Profile))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeMalaysian { get; set; }

        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeNonMalaysian { get; set; }

        [Display(Name = "FieldCity", ResourceType = typeof(Language.Profile))]
        public string City { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public StateModel State { get; set; }

        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Profile))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Profile))]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Profile))]
        public string ICNo { get; set; }

        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Profile))]
        public string PassportNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Profile))]
        public string Email { get; set; }
    }

    public class EditAdminProfileModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Profile))]
        public string Email { get; set; }
    }

    public class EditIndividualProfileModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }
        public bool IsMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Profile))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Profile))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Profile))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeNonMalaysian { get; set; }

        [RegularExpression("[a-zA-Z]+")]
        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Profile))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Profile))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCitizenshipId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Profile))]
        public int? CitizenshipId { get; set; }

        public IEnumerable<SelectListItem> Citizenships { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

    }

    public class EditCompanyProfileModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Profile))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgencyName", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Profile))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Profile))]
        public int? SectorId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMinistryId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Profile))]
        public int? MinistryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyRegNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Profile))]
        public string CompanyRegNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Profile))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Profile))]
        public string PostCodeNonMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Profile))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Profile))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Profile))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Profile))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Profile))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Profile))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Profile))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Profile))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        public IEnumerable<SelectListItem> Ministries { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Sectors { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

    }

    public class ChangePasswordModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredCurrentPassword", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCurrentPassword", ResourceType = typeof(Language.Profile))]
        [DataType(DataType.Password)]
        [Remote("CheckCurrentPassword", "Home", ErrorMessageResourceName = "ValidCheckCurrentPassword", ErrorMessageResourceType = typeof(Language.Profile))]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNewPassword", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldNewPassword", ResourceType = typeof(Language.Profile))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Profile))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ValidCompareRetypePassword", ErrorMessageResourceType = typeof(Language.Profile))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

    public class ChangeEmailModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredCurrentEmail", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldCurrentEmail", ResourceType = typeof(Language.Profile))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Profile))]
        [Remote("CheckCurrentEmail", "Home", ErrorMessageResourceName = "ValidCheckCurrentEmail", ErrorMessageResourceType = typeof(Language.Profile))]
        public string CurrentEmail { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredNewEmail", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldNewEmail", ResourceType = typeof(Language.Profile))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Profile))]
        public string Email { get; set; }

    }

    public class ProfileAvatarModel
    {
        public string AvatarImageBase64 { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredAvatar", ErrorMessageResourceType = typeof(Language.Profile))]
        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public HttpPostedFileBase AvatarFile { get; set; }
    }

    public class Image64Model
    {
        public string image64 { get; set; }
    }
}


