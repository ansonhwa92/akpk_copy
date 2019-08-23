using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.User
{

    public class ListIndividualModel
    {
        public FilterIndividualModel Filter { get; set; }
        public IndividualModel List { get; set; }
    }

    public class FilterIndividualModel : DataTableModel
    {
       
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "IC No/Passport No")]
        public string ICNo { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

    }

    public class IndividualModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "IC No/Passport No")]
        public string ICNo { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

    }

    public class CreateIndividualModel
    {
        
        [Display(Name = "Name")]
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

    }

    public class CreateUserResponse
    {
        public string Password { get; set; }
        public string UID { get; set; }

    }


    public class EditIndividualModel
    {

        [Display(Name = "Name")]
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

        [Display(Name = "Address")]
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
        public string Postcode { get; set; }

        [Display(Name = "Name")]
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

    }

    public class StaffModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

    }


    

}
