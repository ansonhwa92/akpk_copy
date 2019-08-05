using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public string Id { get; set; }

        public string SecretKey { get; set; }

        public string Name { get; set; }       
        public bool Active { get; set; }
               
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool Display { get; set; }
    }
}
