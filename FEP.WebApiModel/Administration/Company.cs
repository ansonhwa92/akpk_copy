using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Administration
{

    public class ListCompanyModel 
    {
        public FilterCompanyModel Filter { get; set; }
        public CompanyModel List { get; set; }
    }

    public class FilterCompanyModel : DataTableModel
    {

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        public int? SectorId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }

    }


    public class CompanyModel
    {

        public int Id { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

    }

    public class CreateCompanyModel
    {
                
        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        [Required]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        [Required]
        public int SectorId { get; set; }

        [Display(Name = "Company Address")]
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "State")]
        [Required]
        public string State { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string PostCode { get; set; }

        [Display(Name = "Company Phone No")]
        [Required]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "Representative Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "IC No/Passport No")]
        [Required]
        public string ICNo { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        [Required]
        public string MobileNo { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }

        [Display(Name = "Role")]
        [Required]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }


    public class EditCompanyModel
    {
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        [Required]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        [Required]
        public int SectorId { get; set; }

        [Display(Name = "Company Address")]
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "State")]
        [Required]
        public string State { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        [Required]
        public string PostCode { get; set; }

        [Display(Name = "Company Phone No")]
        [Required]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "Representative Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "IC No/Passport No")]
        [Required]
        public string ICNo { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        [Required]
        public string MobileNo { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public IEnumerable<SelectListItem> Sectors { get; set; }

        [Display(Name = "Role")]
        [Required]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }

    public class DetailsCompanyModel
    {
        public int Id { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration No")]
        public string CompanyRegNo { get; set; }

        [Display(Name = "Sector")]
        public int SectorId { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Company Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        public string PostCode { get; set; }

        [Display(Name = "Company Phone No")]
        public string CompanyPhoneNo { get; set; }

        [Display(Name = "Representative Name")]
        public string Name { get; set; }

        [Display(Name = "IC No/Passport No")]
        public string ICNo { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Role")]
        public int[] RoleIds { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
