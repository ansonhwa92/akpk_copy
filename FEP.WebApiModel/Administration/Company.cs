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

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        public int? SectorId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }

    }


    public class CompanyModel
    {

        public int Id { get; set; }

        [Display(Name = "FieldCompanyType", ResourceType = typeof(Language.Company))]
        public CompanyType Type { get; set; }

        public string TypeDesc { get; set; }

        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Company))]
        public string CompanyName { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Company))]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

    }

    public class CreateCompanyModel
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

        [Display(Name = "Role")]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Company))]
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

        [Display(Name = "Role")]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Company))]
        public int[] RoleIds { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Individual))]
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

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Individual))]
        public List<RoleModel> Roles { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Individual))]
        public bool Status { get; set; }
    }
}
