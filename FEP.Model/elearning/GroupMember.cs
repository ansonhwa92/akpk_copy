using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    public class GroupMember : BaseEntity
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
