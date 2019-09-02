using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateMessage { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }  
        public bool Display { get; set; }
    }
}
