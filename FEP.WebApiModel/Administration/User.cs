using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;

namespace FEP.WebApiModel.Administration
{
    public class UserModel
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
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
    }

   
}
