using FEP.Helper;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.Notification;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Model;

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

            var model = response.Data;
            
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new CreateIndividualModel();

            model.IsMalaysian = true;

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateIndividualModel model)
        {
            if (model.IsMalaysian)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("CitizenshipId");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else
            {
                ModelState.Remove("ICNo");
                ModelState.Remove("PostCodeMalaysian");
                ModelState.Remove("StateId");
                model.StateId = null;
            }

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={null}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", Language.Individual.ValidIsExistEmail);
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={null}&icno={model.ICNo}");

            if (icnoResponse.Data)
            {
                if (model.IsMalaysian)
                    ModelState.AddModelError("ICNo", Language.Individual.ValidIsExistICNo);
                else
                    ModelState.AddModelError("PassportNo", Language.Individual.ValidIsExistPassportNo);
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<CreateUserResponse>(HttpVerbs.Post, $"Administration/Individual", model);

                if (response.isSuccess)
                {

                    StringBuilder body = new StringBuilder();
                    body.Append("Dear " + model.Name + ",");
                    body.Append("<br />");
                    body.Append("You can sign to FEP Portal <a href = '" + BaseURL + Url.Action("Login", "Auth", new { area = "" }) + "' > here </a>. Sign in Id: " + model.Email + "\n" + "Password: " + response.Data.Password);

                    await EmailMethod.SendEmail("New FE Portal Account Created", body.ToString(), new EmailAddress { DisplayName = model.Name, Address = model.Email });

                    await LogActivity(Modules.Admin, "Create Individual User", model);

                    TempData["SuccessMessage"] = Language.Individual.AlertCreateSuccess;

                    return RedirectToAction("List", "Individual", new { area = "Administrator" });

                }
                else
                {
                    TempData["ErrorMessage"] = Language.Individual.AlertCreateFail;

                    return RedirectToAction("List", "Individual", new { area = "Administrator" });
                }
            }

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpGet]        
        public async Task<ActionResult> Edit(int? id)
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

            var model = new EditIndividualModel
            {
                Id = response.Data.Id,
                IsMalaysian = response.Data.IsMalaysian,
                CitizenshipId = response.Data.Citizenship != null ? response.Data.Citizenship.Id : (int?) null,
                Name = response.Data.Name,
                ICNo = response.Data.ICNo,
                PassportNo = response.Data.PassportNo,
                Email = response.Data.Email,
                MobileNo = response.Data.MobileNo,
                Address1 = response.Data.Address1,
                Address2 = response.Data.Address2,
                PostCodeMalaysian = response.Data.PostCodeMalaysian,
                PostCodeNonMalaysian = response.Data.PostCodeNonMalaysian,
                City = response.Data.City,
                StateId = response.Data.State.Id,
                State = response.Data.State.Name,
                CountryId = response.Data.Country.Id,
                RoleIds = response.Data.Roles.Select(s => s.Id).ToArray(),
                Status = response.Data.Status
            }; 

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]        
        public async Task<ActionResult> Edit(EditIndividualModel model)
        {
            if (model.IsMalaysian)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("CitizenshipId");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else
            {
                ModelState.Remove("ICNo");
                ModelState.Remove("PostCodeMalaysian");
                ModelState.Remove("StateId");
                model.StateId = null;
            }

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={model.Id}&email={model.Email}");

            if (emailResponse.isSuccess)
            {
                ModelState.AddModelError("Email", Language.Individual.ValidIsExistEmail);
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={model.Id}&icno={model.ICNo}");

            if (icnoResponse.isSuccess)
            {
                if (model.IsMalaysian)
                    ModelState.AddModelError("ICNo", Language.Individual.ValidIsExistICNo);
                else
                    ModelState.AddModelError("PassportNo", Language.Individual.ValidIsExistPassportNo);
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Individual?id={model.Id}", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Admin, "Update Individual User", model);

                    TempData["SuccessMessage"] = Language.Individual.AlertEditSuccess;

                    return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = Language.Individual.AlertEditFail;

                    return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = model.Id });
                }

            }

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

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

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
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

            if (response.isSuccess)
            {
                await LogActivity(Modules.Admin, "Activate Individual User Account");

                TempData["SuccessMessage"] = Language.Individual.AlertActivateSuccess;

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Individual.AlertActivateFail;

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

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
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

            if (response.isSuccess)
            {
                await LogActivity(Modules.Admin, "Disable Individual User Account", new { id = id });

                TempData["SuccessMessage"] = Language.Individual.AlertDeactivateSuccess;

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {
                TempData["ErrorMessage"] = Language.Individual.AlertDeactivateFail;

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

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
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

            if (response.isSuccess)
            {

                StringBuilder body = new StringBuilder();
                body.Append("Dear " + response.Data.Name + ",");
                body.Append("<br />");
                body.Append("You can reset your password <a href = '" + BaseURL + Url.Action("SetPassword", "Auth", new { id = response.Data }) + "' > here </a>");

                await EmailMethod.SendEmail("FE Portal Password Reset by Admin", body.ToString(), new EmailAddress { DisplayName = response.Data.Name, Address = Email });

                await LogActivity(Modules.Admin, "Reset Individual User Account Password", new { id = id });

                TempData["SuccessMessage"] = Language.Individual.AlertResetSuccess;

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Individual.AlertResetFail;

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

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
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

            if (response.isSuccess)
            {
                await LogActivity(Modules.Admin, "Delete Individual User", new { id = id });

                TempData["SuccessMessage"] = Language.Individual.AlertDeleteSuccess;

                return RedirectToAction("List", "Individual", new { area = "Administrator" });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Individual.AlertDeleteFail;

                return RedirectToAction("Details", "Individual", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public ActionResult _Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> _Details(int? id)
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

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);

        }
               
        [NonAction]
        private async Task<IEnumerable<RoleModel>> GetRoles()
        {
            var roles = Enumerable.Empty<RoleModel>();

            var response = await WepApiMethod.SendApiAsync<List<RoleModel>>(HttpVerbs.Get, $"Administration/Role");

            if (response.isSuccess)
            {
                roles = response.Data.OrderBy(o => o.Name);
            }

            return roles;

        }

        [NonAction]
        private async Task<IEnumerable<StateModel>> GetStates()
        {
            var states = Enumerable.Empty<StateModel>();

            var response = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (response.isSuccess)
            {
                states = response.Data.OrderBy(o => o.Name);
            }

            return states;

        }

        [NonAction]
        private async Task<IEnumerable<CountryModel>> GetCountries()
        {
            var countries = Enumerable.Empty<CountryModel>();

            var response = await WepApiMethod.SendApiAsync<List<CountryModel>>(HttpVerbs.Get, $"Administration/Country");

            if (response.isSuccess)
            {
                countries = response.Data.OrderBy(o => o.Name);
            }

            return countries;

        }
    }

}