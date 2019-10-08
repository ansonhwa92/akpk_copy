﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.FileDocument
{
    public class Attachment
    {
        public int Id { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
    }

}