using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.Notification;
using FEP.WebApiModel.SLAReminder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class CompanyController : FEPController
    {

        // GET: Administrator/Company
        public async Task<ActionResult> List()
        {
            var filter = new FilterCompanyModel();

            filter.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

            return View(new ListCompanyModel { Filter = filter });
        }

        [HttpPost]
        public async Task<ActionResult> List(FilterCompanyModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"Administration/Company/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new CreateCompanyModel();

            model.Type = CompanyType.Government;

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();
            model.CountryCode = countries.Where(c => c.Name == "Malaysia").Select(s => s.CountryCode).FirstOrDefault();

            model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);
            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Ministries = new SelectList(await GetMinistry(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCompanyModel model)
        {
            if (model.Type == CompanyType.Government)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");
                ModelState.Remove("CompanyName");
                ModelState.Remove("CompanyRegNo");
                ModelState.Remove("SectorId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else if (model.Type == CompanyType.MalaysianCompany)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");
                ModelState.Remove("AgencyName");
                ModelState.Remove("MinistryId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else
            {
                ModelState.Remove("ICNo");
                ModelState.Remove("PostCodeMalaysian");
                ModelState.Remove("StateId");
                ModelState.Remove("AgencyName");
                ModelState.Remove("MinistryId");
                ModelState.Remove("CompanyRegNo");
            }

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={null}&email={model.Email}");

            if (emailResponse.isSuccess)
            {
                ModelState.AddModelError("Email", Language.Administrator.Company.ValidIsExistEmail);
            }

            var icno = model.ICNo;

            if (model.Type == CompanyType.NonMalaysianCompany)
            {
                icno = model.PassportNo;
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={null}&icno={icno}");

            if (icnoResponse.isSuccess)
            {
                if (model.Type == CompanyType.NonMalaysianCompany)
                    ModelState.AddModelError("PassportNo", Language.Administrator.Company.ValidIsExistPassportNo);
                else
                    ModelState.AddModelError("ICNo", Language.Administrator.Company.ValidIsExistICNo);
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Post, $"Administration/Company", model);

                if (response.isSuccess)
                {

                    ParameterListToSend notificationParameter = new ParameterListToSend();
                    notificationParameter.UserFullName = model.Name;
                    notificationParameter.Link = $"<a href = '" + BaseURL + "/Auth/ActivateAccount/" + response.Data.UID + "' > here </a>";
                    notificationParameter.LoginDetail = $"Email: { model.Email }\nPassword: { response.Data.Password }";

                    CreateAutoReminder notification = new CreateAutoReminder
                    {
                        NotificationType = NotificationType.ActivateAccount,
                        NotificationCategory = NotificationCategory.Learning,
                        ParameterListToSend = notificationParameter,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = new List<int> { (int)response.Data.UserId }
                    };

                    var responseNotification = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", notification);

                    await LogActivity(Modules.Setting, "Create Agency User", model);

                    TempData["SuccessMessage"] = Language.Administrator.Company.AlertCreateSuccess;

                    return RedirectToAction("List", "Company", new { area = "Administrator" });

                }
                else
                {
                    TempData["SuccessMessage"] = Language.Administrator.Company.AlertCreateFail;

                    return RedirectToAction("List", "Company", new { area = "Administrator" });
                }

            }

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();
            model.CountryCode = countries.Where(c => c.Name == "Malaysia").Select(s => s.CountryCode).FirstOrDefault();

            model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);
            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Ministries = new SelectList(await GetMinistry(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
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

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = new EditCompanyModel
            {
                Id = response.Data.Id,
                Type = response.Data.Type,
                CompanyName = response.Data.CompanyName,
                AgencyName = response.Data.AgencyName,
                MinistryId = response.Data.Ministry != null ? response.Data.Ministry.Id : (int?)null,
                SectorId = response.Data.Sector != null ? response.Data.Sector.Id : (int?)null,
                CompanyRegNo = response.Data.CompanyRegNo,
                Address1 = response.Data.Address1,
                Address2 = response.Data.Address2,
                PostCodeMalaysian = response.Data.PostCodeMalaysian,
                PostCodeNonMalaysian = response.Data.PostCodeNonMalaysian,
                City = response.Data.City,
                StateId = response.Data.State.Id,
                State = response.Data.State.Name,
                CountryId = response.Data.Country.Id,
                CompanyPhoneNo = response.Data.CompanyPhoneNo,
                CountryCode = response.Data.CountryCode,
                Name = response.Data.Name,
                ICNo = response.Data.ICNo,
                PassportNo = response.Data.PassportNo,
                Email = response.Data.Email,
                MobileNo = response.Data.MobileNo,
                RoleIds = response.Data.Roles.Select(s => s.Id).ToArray(),
                Status = response.Data.Status
            };

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);
            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Ministries = new SelectList(await GetMinistry(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditCompanyModel model)
        {

            if (model.Type == CompanyType.Government)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");
                ModelState.Remove("CompanyName");
                ModelState.Remove("CompanyRegNo");
                ModelState.Remove("SectorId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else if (model.Type == CompanyType.MalaysianCompany)
            {
                ModelState.Remove("PassportNo");
                ModelState.Remove("PostCodeNonMalaysian");
                ModelState.Remove("State");
                ModelState.Remove("CountryId");
                ModelState.Remove("AgencyName");
                ModelState.Remove("MinistryId");

                model.CountryId = model.MalaysiaCountryId;
            }
            else
            {
                ModelState.Remove("ICNo");
                ModelState.Remove("PostCodeMalaysian");
                ModelState.Remove("StateId");
                ModelState.Remove("AgencyName");
                ModelState.Remove("MinistryId");
                ModelState.Remove("CompanyRegNo");
            }

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={model.Id}&email={model.Email}");

            if (emailResponse.isSuccess)
            {
                ModelState.AddModelError("Email", Language.Administrator.Company.ValidIsExistEmail);
            }

            var icno = model.ICNo;

            if (model.Type == CompanyType.NonMalaysianCompany)
            {
                icno = model.PassportNo;
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={model.Id}&icno={icno}");

            if (icnoResponse.isSuccess)
            {
                if (model.Type == CompanyType.NonMalaysianCompany)
                    ModelState.AddModelError("PassportNo", Language.Administrator.Company.ValidIsExistPassportNo);
                else
                    ModelState.AddModelError("ICNo", Language.Administrator.Company.ValidIsExistICNo);
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Company?id={model.Id}", model);

                if (response.Data)
                {
                    await LogActivity(Modules.Setting, "Update Agency User", model);

                    TempData["SuccessMessage"] = Language.Administrator.Company.AlertEditSuccess;

                    return RedirectToAction("Details", "Company", new { area = "Administrator", @id = model.Id });
                }
                else
                {
                    TempData["SuccessMessage"] = Language.Administrator.Company.AlertEditFail;

                    return RedirectToAction("Details", "Company", new { area = "Administrator", @id = model.Id });
                }

            }

            var countries = await GetCountries();

            model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

            model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);
            model.States = new SelectList(await GetStates(), "Id", "Name", 0);
            model.Ministries = new SelectList(await GetMinistry(), "Id", "Name", 0);
            model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
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

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

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

            if (response.Data)
            {
                await LogActivity(Modules.Setting, "Activate Agency User Account", new { id = id });

                TempData["SuccessMessage"] = Language.Administrator.Company.AlertActivateSuccess;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Administrator.Company.AlertActivateFail;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

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
                await LogActivity(Modules.Setting, "Disable Agency User Account", new { id = id });

                TempData["SuccessMessage"] = Language.Administrator.Company.AlertDeactivateSuccess;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Administrator.Company.AlertActivateFail;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

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

            var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Post, $"Auth/ResetPassword", new { Email = Email });

            if (response.isSuccess)
            {

                //StringBuilder body = new StringBuilder();
                //body.Append("Dear " + response.Data.Name + ",");
                //body.Append("<br />");
                //body.Append("You can reset your password <a href = '" + BaseURL + Url.Action("SetPassword", "Auth", new { id = response.Data }) + "' > here </a>");

                //await EmailMethod.SendEmail("FE Portal Password Reset By Admin", body.ToString(), new EmailAddress { DisplayName = response.Data.Name, Address = Email });

                ParameterListToSend notificationParameter = new ParameterListToSend();
                notificationParameter.UserFullName = response.Data.Name;
                notificationParameter.Link = $"<a href = '" + BaseURL + Url.Action("SetPassword", "Auth", new { id = response.Data.UID }) + "' > here </a>";

                CreateAutoReminder notification = new CreateAutoReminder
                {
                    NotificationType = NotificationType.ResetPassword,
                    NotificationCategory = NotificationCategory.Event,
                    ParameterListToSend = notificationParameter,
                    StartNotificationDate = DateTime.Now,
                    ReceiverId = new List<int> { (int)response.Data.UserId }
                };

                await LogActivity(Modules.Setting, "Reset Agency User Account Password", new { id = id });

                TempData["SuccessMessage"] = Language.Administrator.Company.AlertResetSuccess;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Administrator.Company.AlertResetFail;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

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
                await LogActivity(Modules.Setting, "Delete Agency User", new { id = id });

                TempData["SuccessMessage"] = Language.Administrator.Company.AlertDeleteSuccess;

                return RedirectToAction("List", "Company", new { area = "Administrator" });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Administrator.Company.AlertDeleteFail;

                return RedirectToAction("Details", "Company", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> _Add()
        {
            var filter = new FilterCompanyModel();

            filter.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

            return View(new ListCompanyModel { Filter = filter });
        }

        [HttpGet]
        public async Task<ActionResult> _DetailsModal(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        [NonAction]
        private async Task<IEnumerable<SectorModel>> GetSectors()
        {

            var sectors = Enumerable.Empty<SectorModel>();

            var response = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (response.isSuccess)
            {
                sectors = response.Data.OrderBy(o => o.Name);
            }

            return sectors;

        }

        [NonAction]
        private async Task<IEnumerable<MinistryModel>> GetMinistry()
        {
            var ministries = Enumerable.Empty<MinistryModel>();

            var response = await WepApiMethod.SendApiAsync<List<MinistryModel>>(HttpVerbs.Get, $"Administration/Ministry");

            if (response.isSuccess)
            {
                ministries = response.Data.OrderBy(o => o.Name);
            }

            return ministries;

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

        [HttpGet]
        public async Task<ActionResult> GetCountryCode(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<CountryModel>(HttpVerbs.Get, $"Administration/Country?id={Id}");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

    }
}