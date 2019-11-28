using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Administration
{
    public class RoleModel
    {
        public int Id { get; set; }

        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class CreateRoleModel
    {
        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Role))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Role))]
        public string Name { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.Administrator.Role))]
        public string Description { get; set; }
    }

    public class EditRoleModel
    {
        public int Id { get; set; }

        public string No { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.Administrator.Role))]
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Role))]
        public string Name { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.Administrator.Role))]
        public string Description { get; set; }
    }

    public class DeleteRoleModel
    {
        public int Id { get; set; }

        public string No { get; set; }
        
        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Role))]
        public string Name { get; set; }

        [Display(Name = "FieldDescription", ResourceType = typeof(Language.Administrator.Role))]
        public string Description { get; set; }
    }

    public class RoleAccessModel//api
    {
        public string RoleName { get; set; }
        public List<UserAccessModel> UserAccesses { get; set; }
    }

    public class RoleUserModel//api
    {
        public string RoleName { get; set; }
        public List<UserModel> Users { get; set; }
    }

    public class AccessModel//view
    {
        public List<RoleModel> Roles { get; set; }

        public int RoleId { get; set; }

        public Modules? Module { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Language.Administrator.Role))]
        public string RoleName { get; set; }

        public Dictionary<UserAccess, string> Access { get; set; }
    }

    public class UpdateRoleAccessModel//api
    {
        public int RoleId { get; set; }
        public List<UserAccessModel> Access { get; set; }
    }

    public class UpdateUserRoleModel
    {
        public int RoleId { get; set; }
        public List<int> UserId { get; set; }
    }

   
    public class UserAccessModel//view
    {
        public UserAccess UserAccess { get; set; }
        public bool IsCheck { get; set; }
    }

    public class UserRoleModel//view
    {
        public List<RoleModel> Roles { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ListUserModel Users { get; set; }

    }

}
