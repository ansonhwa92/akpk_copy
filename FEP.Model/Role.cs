using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FEP.Model
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool Display { get; set; }

        public virtual ICollection<RoleAccess> RoleAccess { get; set; }
    }

    [Table("RoleAccess")]
    public class RoleAccess
    {
        [Key]
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        
        public UserAccess UserAccess { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }        

    }

    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

    }

}
