using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }        
        public virtual UserAccount UserAccount { get; set; }
        public bool Display { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    [Table("UserAccount")]
    public class UserAccount
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string HashPassword { get; set; }
        public string Salt { get; set; }
        public bool IsEnable { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public int LoginAttempt { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual User User { get; set; }        

    }
        
    public enum UserType
    {
        [Display(Name = "UserTypeSystemAdmin", ResourceType = typeof(Language.Enum))]
        SystemAdmin = 0,
        [Display(Name = "UserTypeIndividual", ResourceType = typeof(Language.Enum))]
        Individual = 1,
        [Display(Name = "UserTypeCompany", ResourceType = typeof(Language.Enum))]
        Company = 2,
        [Display(Name = "UserTypeStaff", ResourceType = typeof(Language.Enum))]
        Staff = 3,
        
    }
      
}
