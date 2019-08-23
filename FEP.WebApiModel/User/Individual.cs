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

        [Display(Name = "Status")]
        public bool Status { get; set; }

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
        [DataType(DataType.EmailAddress)]
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
        public int Id { get; set; }

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

        [Display(Name = "Status")]
        public bool Status { get; set; }


    }

    public class DetailsIndividualModel
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

        [Display(Name = "Status")]
        public bool Status { get; set; }

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
