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

    public class StaffLogInModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredLoginId", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldUserName", ResourceType = typeof(Language.Auth))]
        public string LoginId { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredPassword", ErrorMessageResourceType = typeof(Language.Auth))]
        [Display(Name = "FieldPassword", ResourceType = typeof(Language.Auth))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Status { get; set; }

        public string ReturnUrl { get; set; }

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
        public string avatar { get; set; }
        public List<string> access { get; set; }
        
    }
}