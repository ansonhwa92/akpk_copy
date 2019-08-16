using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.WebApi.Models
{
    public class CreatePublicationApiModel
    {
        public int CategoryID { get; set; }
        public string Author { get; set; }
        public string Coauthor { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public bool Free { get; set; }
        public bool Hardcopy { get; set; }
        public bool Digitalcopy { get; set; }
        public bool HDcopy { get; set; }
        public float HPrice { get; set; }
        public float DPrice { get; set; }
        public float HDPrice { get; set; }
        public string ProofOfApproval { get; set; }
        public int StockBalance { get; set; }
    }

    public class EditPublicationApiModel
    {
        public int CategoryID { get; set; }
        public string Author { get; set; }
        public string Coauthor { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public bool Free { get; set; }
        public bool Hardcopy { get; set; }
        public bool Digitalcopy { get; set; }
        public bool HDcopy { get; set; }
        public float HPrice { get; set; }
        public float DPrice { get; set; }
        public float HDPrice { get; set; }
        public string ProofOfApproval { get; set; }
        public int StockBalance { get; set; }
    }

}