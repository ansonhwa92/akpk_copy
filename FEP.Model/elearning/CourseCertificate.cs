﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    public class CourseCertificate : BaseEntity
    {
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
        public string Template { get; set; }
        public int CourseId { get; set; }
    }
}
