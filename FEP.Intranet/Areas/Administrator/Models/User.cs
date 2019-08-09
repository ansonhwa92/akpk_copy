using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Areas.Administrator.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.User))]
        public string ICNo { get; set; }

        public UserType? UserType { get; set; }

        [Display(Name = "FieldUserType", ResourceType = typeof(Language.User))]
        public string UserTypeDesc { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }

        [Display(Name = "FieldStatus", ResourceType = typeof(Language.User))]
        public string StatusDesc { get; set; }
    }

    public class UserListModel
    {
        public UserFilterModel Filter { get; set; }
        public UserModel List { get; set; }        
    }

    public class UserFilterModel
    {
        [Display(Name = "FieldName", ResourceType = typeof(Language.User))]
        public string Name { get; set; }

        [Display(Name = "FieldICNo", ResourceType = typeof(Language.User))]
        public string ICNo { get; set; }

        [Display(Name = "FieldUserType", ResourceType = typeof(Language.User))]
        public UserType? UserType { get; set; }
        
        [Display(Name = "FieldEmail", ResourceType = typeof(Language.User))]
        public string Email { get; set; }
    }

}