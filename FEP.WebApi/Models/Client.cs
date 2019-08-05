using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.WebApi.Models
{
    public class ClientModel
    {
        public string Id { get; set; }

        [Required]
        public string SecretKey { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}