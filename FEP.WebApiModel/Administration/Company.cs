using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Administration
{

    public class ListCompanyModel 
    {
        public FilterCompanyModel Filter { get; set; }
        public CompanyModel List { get; set; }
    }

    public class FilterCompanyModel : DataTableModel
    {

        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyName { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Company))]
        public string Email { get; set; }

        [Display(Name = "FieldCompanyType", ResourceType = typeof(Language.Administrator.Company))]
        public CompanyType? Type { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }

    }


    public class CompanyModel
    {

        public int Id { get; set; }

        [Display(Name = "FieldCompanyType", ResourceType = typeof(Language.Administrator.Company))]
        public CompanyType Type { get; set; }

        public string TypeDesc { get; set; }

        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyName { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Company))]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

    }

    public class CreateCompanyModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgencyName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Administrator.Company))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Administrator.Company))]
        public int? SectorId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMinistryId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Administrator.Company))]
        public int? MinistryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyRegNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyRegNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Company))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeNonMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Company))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Company))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Company))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Company))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Company))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Company))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Company))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Company))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Company))]
        public string Email { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Ministries { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Sectors { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

    }


    public class EditCompanyModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAgencyName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Administrator.Company))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Administrator.Company))]
        public int? SectorId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMinistryId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Administrator.Company))]
        public int? MinistryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyRegNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyRegNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Company))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeNonMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Company))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Company))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Company))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Company))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericCompanyPhoneNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyPhoneNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Company))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Company))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Company))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Company))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Company))]
        public string Email { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Administrator.Company))]
        public int[] RoleIds { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Company))]
        public bool Status { get; set; }

        public IEnumerable<SelectListItem> Ministries { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Sectors { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

    }

    public class DetailsCompanyModel
    {
        public int Id { get; set; }
                
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyName { get; set; }
               
        [Display(Name = "FieldAgencyName", ResourceType = typeof(Language.Administrator.Company))]
        public string AgencyName { get; set; }

        public CompanyType Type { get; set; }
                
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Administrator.Company))]
        public SectorModel Sector { get; set; }
                
        [Display(Name = "FieldMinistryId", ResourceType = typeof(Language.Administrator.Company))]
        public MinistryModel Ministry { get; set; }
                
        [Display(Name = "FieldCompanyRegNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyRegNo { get; set; }
                
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Company))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
                
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeMalaysian { get; set; }
                
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Company))]
        public string PostCodeNonMalaysian { get; set; }
                
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Company))]
        public string City { get; set; }

        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Company))]
        public StateModel State { get; set; }

        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Company))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }
                
        [Display(Name = "FieldCompanyPhoneNo", ResourceType = typeof(Language.Administrator.Company))]
        public string CompanyPhoneNo { get; set; }
               
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Company))]
        public string Name { get; set; }
                
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Company))]
        public string ICNo { get; set; }
               
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Company))]
        public string PassportNo { get; set; }
                
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Company))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Company))]
        public string Email { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Company))]
        public List<RoleModel> Roles { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Company))]
        public bool Status { get; set; }
    }
}
