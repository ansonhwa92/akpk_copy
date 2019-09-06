using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Setting
{
    public class AccountSettingModel
    {
        [Display(Name = "FieldIsPasswordExpiry", ResourceType = typeof(Language.AccountSetting))]
        public bool IsPasswordExpiry { get; set; }

        [Display(Name = "FieldPasswordExpiryDuration", ResourceType = typeof(Language.AccountSetting))]
        public int? PasswordExpiryDuration { get; set; }

        [Display(Name = "FieldIsLimitLoginAttempt", ResourceType = typeof(Language.AccountSetting))]
        public bool IsLimitLoginAttempt { get; set; }

        [Display(Name = "FieldLoginAttemptLimit", ResourceType = typeof(Language.AccountSetting))]
        public int? LoginAttemptLimit { get; set; }

        [Display(Name = "FieldIsContainLowerCase", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainLowerCase { get; set; }

        [Display(Name = "FieldIsContainUpperCase", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainUpperCase { get; set; }

        [Display(Name = "FieldIsContainNumeric", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainNumeric { get; set; }

        [Display(Name = "FieldIsContainSymbol", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainSymbol { get; set; }

        [Display(Name = "FieldIsLengthLimit", ResourceType = typeof(Language.AccountSetting))]
        public bool IsLengthLimit { get; set; }
    }

    public class EditAccountSettingModel
    {
        public int Id { get; set; }

        [Display(Name = "FieldIsPasswordExpiry", ResourceType = typeof(Language.AccountSetting))]
        public bool IsPasswordExpiry { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "ValidNumericPasswordExpiryDuration", ErrorMessageResourceType = typeof(Language.AccountSetting))]
        [Display(Name = "FieldPasswordExpiryDuration", ResourceType = typeof(Language.AccountSetting))]
        public int? PasswordExpiryDuration { get; set; }

        [Display(Name = "FieldIsLimitLoginAttempt", ResourceType = typeof(Language.AccountSetting))]
        public bool IsLimitLoginAttempt { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "ValidNumericLoginAttemptLimit", ErrorMessageResourceType = typeof(Language.AccountSetting))]
        [Display(Name = "FieldLoginAttemptLimit", ResourceType = typeof(Language.AccountSetting))]
        public int? LoginAttemptLimit { get; set; }

        [Display(Name = "FieldIsContainLowerCase", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainLowerCase { get; set; }

        [Display(Name = "FieldIsContainUpperCase", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainUpperCase { get; set; }

        [Display(Name = "FieldIsContainNumeric", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainNumeric { get; set; }

        [Display(Name = "FieldIsContainSymbol", ResourceType = typeof(Language.AccountSetting))]
        public bool IsContainSymbol { get; set; }

        [Display(Name = "FieldIsLengthLimit", ResourceType = typeof(Language.AccountSetting))]
        public bool IsLengthLimit { get; set; }
    }
}
