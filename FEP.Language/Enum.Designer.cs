﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Language {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Enum {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Enum() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Language.Enum", typeof(Enum).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Government Agency.
        /// </summary>
        public static string CompanyTypeGovernment {
            get {
                return ResourceManager.GetString("CompanyTypeGovernment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Malaysian Company.
        /// </summary>
        public static string CompanyTypeMalaysianCompany {
            get {
                return ResourceManager.GetString("CompanyTypeMalaysianCompany", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non Malaysian Company.
        /// </summary>
        public static string CompanyTypeNonMalaysianCompany {
            get {
                return ResourceManager.GetString("CompanyTypeNonMalaysianCompany", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activate Account.
        /// </summary>
        public static string NotificationTypeActivateAccount {
            get {
                return ResourceManager.GetString("NotificationTypeActivateAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reset Account Password.
        /// </summary>
        public static string NotificationTypeResetPassword {
            get {
                return ResourceManager.GetString("NotificationTypeResetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Agency.
        /// </summary>
        public static string UserTypeCompany {
            get {
                return ResourceManager.GetString("UserTypeCompany", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Individual.
        /// </summary>
        public static string UserTypeIndividual {
            get {
                return ResourceManager.GetString("UserTypeIndividual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Staff.
        /// </summary>
        public static string UserTypeStaff {
            get {
                return ResourceManager.GetString("UserTypeStaff", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System Admin.
        /// </summary>
        public static string UserTypeSystemAdmin {
            get {
                return ResourceManager.GetString("UserTypeSystemAdmin", resourceCulture);
            }
        }
    }
}
