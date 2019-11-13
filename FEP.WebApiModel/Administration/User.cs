using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Helper;
using FEP.Model;

namespace FEP.WebApiModel.Administration
{

    public class ListUserModel
    {
        public FilterUserModel Filter { get; set; }
        public UserModel List { get; set; }
    }

    public class FilterUserModel : DataTableModel
    {

        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        [Display(Name = "FieldUserType", ResourceType = typeof(Language.User))]
        public UserType? UserType { get; set; }

    }


    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.User))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }

        [Display(Name = "FieldUserType", ResourceType = typeof(Language.User))]
        public UserType UserType { get; set; }

        [Display(Name = "FieldUserType", ResourceType = typeof(Language.User))]
        public string UserTypeDesc { get; set; }
    }

    public class EditUserModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        [Display(Name = "FieldMobileNo", ResourceType = typeof(Language.User))]
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }
        public string Avatar { get; set; }

    }

    public class DetailsUserModel
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string ICNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }
        public UserType UserType { get; set; }        
        public bool Display { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }              
        public bool IsEnable { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public int LoginAttempt { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public List<UserAccess> UserAccesses { get; set; }
        public string AvatarImageBase64 { get; set; }
    }

   
}
