using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ICNo { get; set; }
        public string MobileNo { get; set; }
        public UserType UserType { get; set; }        
        public bool Display { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual IndividualProfile IndividualProfile { get; set; }
        public virtual CompanyProfile CompanyProfile { get; set; }
        public virtual StaffProfile StaffProfile { get; set; }
    }

    [Table("UserAccount")]
    public class UserAccount
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string HashPassword { get; set; }
        public string Salt { get; set; }
        public bool IsEnable { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public int LoginAttempt { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

    }

    [Table("ActivateAccount")]
    public class ActivateAccount
    {
        [Key]
        public int Id { get; set; }
        public string UID { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActivate { get; set; }
        public DateTime? ActivateDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("PasswordReset")]
    public class PasswordReset
    {
        [Key]
        public int Id { get; set; }
        public string UID { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReset { get; set; }
        public DateTime? ResetDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    [Table("IndividualProfile")]
    public class IndividualProfile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public bool IsMalaysian { get; set; }
        public int? CitizenshipId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }//for malaysian
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("CitizenshipId")]
        public virtual Country Citizenship { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }

    [Table("CompanyProfile")]
    public class CompanyProfile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public CompanyType Type { get; set; }
        public string CompanyName { get; set; }
        public int? SectorId { get; set; }
        public int? MinistryId { get; set; }
        public string CompanyRegNo { get; set; }//form malaysian
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }//for malaysian
        public string StateName { get; set; }//for non malaysian
        public int CountryId { get; set; }//non malaysian
        public string CompanyPhoneNo { get; set; }
        
        public virtual User User { get; set; }

        [ForeignKey("SectorId")]
        public virtual Sector Sector { get; set; }

        [ForeignKey("MinistryId")]
        public virtual Ministry Ministry { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        

    }

    public enum CompanyType
    {
        [Display(Name = "CompanyTypeGovernment", ResourceType = typeof(Language.Enum))]
        Government,
        [Display(Name = "CompanyTypeMalaysianCompany", ResourceType = typeof(Language.Enum))]
        MalaysianCompany,
        [Display(Name = "CompanyTypeNonMalaysianCompany", ResourceType = typeof(Language.Enum))]
        NonMalaysianCompany
    }

    [Table("StaffProfile")]
    public class StaffProfile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }
        public int DesignationId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }

    }

    [Table("Branch")]
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }

    [Table("Department")]
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    [Table("Designation")]
    public class Designation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    [Table("Sector")]
    public class Sector
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    [Table("Ministry")]
    public class Ministry
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    [Table("State")]
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    [Table("Country")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Display { get; set; }
    }

    public enum UserType
    {
        [Display(Name = "UserTypeSystemAdmin", ResourceType = typeof(Language.Enum))]
        SystemAdmin = 0,
        [Display(Name = "UserTypeIndividual", ResourceType = typeof(Language.Enum))]
        Individual = 1,
        [Display(Name = "UserTypeAgency", ResourceType = typeof(Language.Enum))]
        Company = 2,
        [Display(Name = "UserTypeStaff", ResourceType = typeof(Language.Enum))]
        Staff = 3
    }
}
