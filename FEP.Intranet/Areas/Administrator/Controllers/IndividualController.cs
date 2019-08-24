using FEP.Helper;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class IndividualController : FEPController
    {
        // GET: Administrator/Individual
        public ActionResult List()
        {            
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateIndividualModel model)
        {

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={null}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", "Email already registered in the system");
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={null}&icno={model.ICNo}");

            if (icnoResponse.Data)
            {
                ModelState.AddModelError("ICNo", "IC No already registered in the system");
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<CreateUserResponse>(HttpVerbs.Post, $"Administration/Individual", model);

                if (response != null)
                {

                    SendEmail("New FEP Account Created", "Sign in Id: " + model.Email + "\n" + "Password: " + response.Data.Password, new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    LogActivity("Create Individual User");

                    TempData["SuccessMessage"] = "User successfully registered. User will receive email with sign in details and link to activate the account.";

                    return RedirectToAction("List", "Individual", new { area = "Administrator" });

                }
                else
                {
                    TempData["SuccessMessage"] = "Fail to register user.";

                    return RedirectToAction("List", "Individual", new { area = "Administrator" });
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<EditIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditIndividualModel model)
        {
            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={model.Id}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", "Email already registered in the system");
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={model.Id}&icno={model.ICNo}");

            if (icnoResponse.Data)
            {
                ModelState.AddModelError("ICNo", "IC No already registered in the system");
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Individual?id={model.Id}", model);

                if (response.Data)
                {
                    LogActivity("Update Individual User", model);

                    TempData["SuccessMessage"] = "User record successfully updated.";

                    return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = model.Id });
                }
                else
                {
                    TempData["SuccessMessage"] = "Fail to update user record.";

                    return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = model.Id });
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActivateConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/User/Activate/?id={id}");

            if (response.Data)
            {
                LogActivity("Activate Individual User Account");

                TempData["SuccessMessage"] = "User account successfully activate.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {
                
                TempData["ErrorMessage"] = "Fail to activate user account.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/User/Deactivate/?id={id}");

            if (response.Data)
            {
                LogActivity("Disable Individual User Account");

                TempData["SuccessMessage"] = "User account successfully disable.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = "Fail to disable user account.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordConfirm(int? id, string Email)
        {
            if (id == null || string.IsNullOrEmpty(Email))
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<ResetPasswordResponseModel>(HttpVerbs.Post, $"Auth/ResetPassword", new { Email = Email });

            if (response.Data != null)
            {
                
                EmailAddress receiver = new EmailAddress()
                {
                    DisplayName = response.Data.Name,
                    Address = Email
                };

                StringBuilder body = new StringBuilder();
                body.Append("Dear " + response.Data.Name + ",");
                body.Append("<br />");
                body.Append("You can reset your password <a href = '" + BaseURL + Url.Action("SetPassword", "Auth", new { id = response.Data }) + "' > here </a>");

                SendEmail("FEP Password Reset", body.ToString(), receiver);

                LogActivity("Reset Individual User Account Password");

                TempData["SuccessMessage"] = "User account password successfully reset. User will receive email with link to reset account password.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = "Fail to reset user account password.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/User/Delete/?id={id}");

            if (response.Data)
            {
                LogActivity("Delete Individual User");

                TempData["SuccessMessage"] = "User account successfully delete.";

                return RedirectToAction("List", "Individual", new { area = "Administrator" });
            }
            else
            {

                TempData["ErrorMessage"] = "Fail to delete user account.";

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }

        }

    }
}