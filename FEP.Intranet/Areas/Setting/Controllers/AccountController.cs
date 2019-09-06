using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Setting.Controllers
{
    public class AccountController : FEPController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            var response = await WepApiMethod.SendApiAsync<AccountSettingModel>(HttpVerbs.Get, $"Setting/AccountSetting");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var response = await WepApiMethod.SendApiAsync<EditAccountSettingModel>(HttpVerbs.Get, $"Setting/AccountSetting");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;
                        
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditAccountSettingModel model)
        {
            
            if (model.IsPasswordExpiry && model.PasswordExpiryDuration == null)
            {
                ModelState.AddModelError("PasswordExpiryDuration", Language.AccountSetting.ValidRequiredPasswordExpiryDuration);
            }

            if (model.IsLimitLoginAttempt && model.LoginAttemptLimit == null)
            {
                ModelState.AddModelError("LoginAttemptLimit", Language.AccountSetting.ValidRequiredLoginAttemptLimit);
            }
            
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Setting/AccountSetting", model);

                if (response.isSuccess)
                {
                    LogActivity(Modules.Setting, "Update Account Setting", model);

                    TempData["SuccessMessage"] = Language.AccountSetting.AlertSuccessUpdate;

                    return RedirectToAction("Index", "Account", new { area = "Setting" });
                }
                else
                {
                    TempData["SuccessMessage"] = Language.AccountSetting.AlertFailUpdate;                                        
                }

            }

            return View(model);
        }
    }
}