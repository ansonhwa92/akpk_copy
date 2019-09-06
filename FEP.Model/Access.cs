﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{

    [Table("Access")]
    public class Access
    {
        [Key]
        public UserAccess UserAccess { get; set; }
        public Modules? Module { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RoleAccess> RoleAccess { get; set; }
    }

    public enum Modules
    {
        [Display(Name = "Home")]
        Home = 0,
        [Display(Name = "Event")]
        Event = 1,
        [Display(Name = "Research & Publication")]
        RnP = 2,
        [Display(Name = "Learning")]
        Learning = 3,
        [Display(Name = "User & Role")]
        Admin = 4,
        [Display(Name = "System Setting")]
        Setting = 5,
        [Display(Name = "Report")]
        Report = 6,

    }


    public enum UserAccess
    {        
        //home 0 - 1000
        [Display(Name = "Dashboard 1")]
        HomeDashboard1 = 1,
        [Display(Name = "Dashboard 2")]
        HomeDashboard2 = 2,
        [Display(Name = "Dashboard 3")]
        HomeDashboard3 = 3,

        //e-Event 1001 - 2000
        [Display(Name = "Event Management")]
        EventMenu = 1001,
        [Display(Name = "Internal Event Menu")]
        EventIntenalEventMenu = 1002,




        //research 2001 - 3000
        [Display(Name = "R&P Management Menu")]
        RnPMenu = 2001,
        [Display(Name = "Publication Menu")]
        RnPPublicationMenu = 2002,
        [Display(Name = "Survey Menu")]
        RnPSurveyMenu = 2003,

        [Display(Name = "Publication - List")]
        RnPPublicationList = 2101,
        [Display(Name = "Publication - View")]
        RnPPublicationView = 2102,
        [Display(Name = "Publication - Add")]
        RnPPublicationAdd = 2103,
        [Display(Name = "Publication - Edit")]
        RnPPublicationEdit = 2104,
        [Display(Name = "Publication - Delete")]
        RnPPublicationDelete = 2105,
        [Display(Name = "Publication - Submit")]
        RnPPublicationSubmit = 2106,
        [Display(Name = "Publication - Cancel")]
        RnPPublicationCancel = 2107,
        [Display(Name = "Publication - Withdraw")]
        RnPPublicationWithdraw = 2108,

        [Display(Name = "Publication - Verify")]
        RnPPublicationVerify = 2201,
        [Display(Name = "Publication - Approve (Level 1)")]
        RnPPublicationApprove1 = 2202,
        [Display(Name = "Publication - Approve (Level 2)")]
        RnPPublicationApprove2 = 2203,
        [Display(Name = "Publication - Approve (Level 3)")]
        RnPPublicationApprove3 = 2204,



        //elearning 3001 - 4000
        [Display(Name = "Learning Management Menu")]
        LearningMenu = 3001,






        //administration 4001 - 5000
        [Display(Name = "User & Role Management Menu")]
        AdminMenu = 4001,
        [Display(Name = "Individual Menu")]
        AdminIndividualMenu = 4002,
        [Display(Name = "Agency Menu")]
        AdminCompanyMenu = 4003,
        [Display(Name = "Staff Menu")]
        AdminStaffMenu = 4004,
        [Display(Name = "Individual List")]
        AdminIndividualList = 4005,
        [Display(Name = "Individual Edit")]
        AdminIndividualEdit = 4006,



        //setting 5001 - 6000
        [Display(Name = "System Settings Menu")]
        SettingMenu = 5001,
            






        //report 6001 - 7000
        [Display(Name = "Reports Menu")]
        ReportMenu = 6001,







    }
}
