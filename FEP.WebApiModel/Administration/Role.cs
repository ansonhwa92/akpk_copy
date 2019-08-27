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
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class EditRoleModel
    {
        public int Id { get; set; }

        public string No { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class DeleteRoleModel
    {
        public int Id { get; set; }

        public string No { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class RoleAccessModel
    {
        public string RoleName { get; set; }
        public List<UserAccessModel> UserAccesses { get; set; }
    }

    public class AccessModel
    {
        public int RoleId { get; set; }

        public Modules? Module { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public Dictionary<UserAccess, string> Access { get; set; }
    }

    public class UpdateRoleAccessModel
    {
        public int RoleId { get; set; }
        public List<UserAccessModel> Access { get; set; }
    }

   
    public class UserAccessModel
    {
        public UserAccess UserAccess { get; set; }
        public bool IsCheck { get; set; }
    }

}
