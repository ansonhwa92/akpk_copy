using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Models
{
    public class LogInModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredLoginId", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldLoginId", ResourceType = typeof(Language.Auth))]
        public string LoginId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Status { get; set; }

        public string ReturnUrl { get; set; }

    }

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
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        public string RetypePassword { get; set; }
    }

    public class RegisterAgencyModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredCompanyName", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Language.Auth))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredSectorId", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldSectorId", ResourceType = typeof(Language.Auth))]
        public int SectorId { get; set; }
                
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
        public int StateId { get; set; }

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
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredRetypePassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldRetypePassword", ResourceType = typeof(Language.Auth))]
        public string RetypePassword { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Sectors { get; set; }
    }


    public class CurrentUserModel
    {
        public int userid { get; set; }
        public string loginid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string usertype { get; set; }
        public bool isenable { get; set; }
        public DateTime? validfrom { get; set; }
        public DateTime? validto { get; set; }
        public List<string> access { get; set; }
        
    }
}