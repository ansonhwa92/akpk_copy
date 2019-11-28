using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Administration
{

    public class ListIndividualModel
    {
        public FilterIndividualModel Filter { get; set; }
        public IndividualModel List { get; set; }
    }

    public class FilterIndividualModel : DataTableModel
    {

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Individual))]
        public string Name { get; set; }

        [Display(Name = "FieldICPassportNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string ICNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Individual))]
        public string Email { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string MobileNo { get; set; }


    }

    public class IndividualModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Individual))]
        public string Name { get; set; }

        [Display(Name = "FieldICPassportNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string ICNo { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Individual))]
        public string Email { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string MobileNo { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Individual))]
        public bool Status { get; set; }

    }

    public class CreateIndividualModel
    {

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Individual))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Administrator.Individual))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Individual))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeNonMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Individual))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Individual))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Individual))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Individual))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCitizenshipId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Administrator.Individual))]
        public int? CitizenshipId { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Individual))]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        public int[] RoleIds { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Citizenships { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> States { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }


    }

    public class CreateUserResponse
    {
        public string Password { get; set; }
        public string UID { get; set; }

    }

    public class EditIndividualModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Individual))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [StringLength(12, MinimumLength = 12, ErrorMessageResourceName = "ValidLengthICNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string ICNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassportNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string PassportNo { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericMobileNo", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredEmail", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]
        [EmailAddress(ErrorMessageResourceName = "ValidTypeEmail", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredAddress", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Individual))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [RegularExpression("([0-9]+)", ErrorMessageResourceName = "ValidNumericPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeMalaysian { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPostCode", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeNonMalaysian { get; set; }
                
        [Required(ErrorMessageResourceName = "ValidRequiredCity", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Individual))]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Individual))]
        public string State { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredStateId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Individual))]
        public int? StateId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCountryId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Individual))]
        public int CountryId { get; set; }

        public int MalaysiaCountryId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredCitizenshipId", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Administrator.Individual))]
        public int? CitizenshipId { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Individual))]
        [Required(ErrorMessageResourceName = "ValidRequiredRole", ErrorMessageResourceType = typeof(Language.Administrator.Individual))]
        public int[] RoleIds { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Individual))]
        public bool Status { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Citizenships { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> States { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }


    }

    public class DetailsIndividualModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldAvatar", ResourceType = typeof(Language.Profile))]
        public string AvatarImageBase64 { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Individual))]
        public string Name { get; set; }

        public bool IsMalaysian { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string ICNo { get; set; }

        [Display(Name = "FieldPassportNo", ResourceType = typeof(Language.Administrator.Individual))]
        public string PassportNo { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.Auth))]
        public string MobileNo { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.Auth))]        
        public string Email { get; set; }

        [Display(Name = "FieldAddress", ResourceType = typeof(Language.Administrator.Individual))]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
              
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeMalaysian { get; set; }
        
        [Display(Name = "FieldPostCode", ResourceType = typeof(Language.Administrator.Individual))]
        public string PostCodeNonMalaysian { get; set; }

        [Display(Name = "FieldCity", ResourceType = typeof(Language.Administrator.Individual))]
        public string City { get; set; }
               
        [Display(Name = "FieldStateId", ResourceType = typeof(Language.Administrator.Individual))]
        public StateModel State { get; set; }
                          
        [Display(Name = "FieldCountryId", ResourceType = typeof(Language.Administrator.Individual))]
        public CountryModel Country { get; set; }

        public int MalaysiaCountryId { get; set; }
              
        [Display(Name = "FieldCitizenship", ResourceType = typeof(Language.Administrator.Individual))]
        public CountryModel Citizenship { get; set; }
        

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.Administrator.Individual))]
        public bool Status { get; set; }

        [Display(Name = "FieldRole", ResourceType = typeof(Language.Administrator.Individual))]
        public List<RoleModel> Roles { get; set; }
        
    }

}
