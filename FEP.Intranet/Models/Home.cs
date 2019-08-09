using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FEP.Intranet.Models
{
    public class ProfileModel
    {
        [Display(Name = "User Type")]
        public virtual UserType? UserType { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "IC No")]
        public string ICNo { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Last Change Password")]
        public DateTime? LastPasswordChange { get; set; }

        public int RemainingDays { get; set; }


        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
                
        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Company Registration No")]
        public string CompanyRegNo { get; set; }

        
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        
        [Display(Name = "Postcode")]
        public string PostCode { get; set; }

        
        [Display(Name = "City")]
        public string City { get; set; }

       
        [Display(Name = "State")]
        public string State { get; set; }

        
        [Display(Name = "Company Phone No")]
        public string CompanyPhoneNo { get; set; }

    }
}