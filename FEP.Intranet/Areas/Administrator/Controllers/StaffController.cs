using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class StaffController : FEPController
    {
       
        public async Task<ActionResult> List()
        {
            var filter = new FilterStaffModel();

            filter.Branchs = new SelectList(await GetBranches(), "Id", "Name", 0);
            filter.Departments = new SelectList(await GetDepartments(), "Id", "Name", 0);
            
            return View(new ListStaffModel { Filter = filter });
        }

        [HttpPost]
        public async Task<ActionResult> List(FilterStaffModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"Administration/Staff/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsStaffModel>(HttpVerbs.Get, $"Administration/Staff?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsStaffModel>(HttpVerbs.Get, $"Administration/Staff?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = new EditStaffModel
            {
                Id = response.Data.Id,               
                Name = response.Data.Name,
                ICNo = response.Data.ICNo,               
                Email = response.Data.Email,
                MobileNo = response.Data.MobileNo,
                BranchId = response.Data.Branch != null ? response.Data.Branch.Id : (int?) null,
                Department = response.Data.Department,
                Designation = response.Data.Designation,
                StaffId = response.Data.StaffId,                
                CountryCode = response.Data.CountryCode,              
                RoleIds = response.Data.Roles.Select(s => s.Id).ToArray(),
                Status = response.Data.Status
            };
                        
            model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);
            model.Branches = new SelectList(await GetBranches(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditStaffModel model)
        {
                       
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Staff?id={model.Id}", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Update Staff", model);

                    TempData["SuccessMessage"] = Language.Individual.AlertEditSuccess;

                    return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = Language.Individual.AlertEditFail;

                    return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = model.Id });
                }

            }
                      
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

            var response = await WepApiMethod.SendApiAsync<DetailsStaffModel>(HttpVerbs.Get, $"Administration/Staff?id={id}");

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

            if (response.isSuccess)
            {
                await LogActivity(Modules.Setting, "Activate Staff Account", new { id = id });

                TempData["SuccessMessage"] = Language.Staff.AlertActivateSuccess;

                return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Staff.AlertActivateFail;

                return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsStaffModel>(HttpVerbs.Get, $"Administration/Staff?id={id}");

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

            if (response.isSuccess)
            {
                await LogActivity(Modules.Setting, "Disable Staff Account", new { id = id });

                TempData["SuccessMessage"] = Language.Staff.AlertDeactivateSuccess;

                return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = id });
            }
            else
            {

                TempData["ErrorMessage"] = Language.Staff.AlertDeactivateFail;

                return RedirectToAction("Details", "Staff", new { area = "Administrator", @id = id });
            }

        }

        [HttpGet]
        public async Task<ActionResult> _Add()
        {
            var filter = new FilterStaffModel();

            filter.Branchs = new SelectList(await GetBranches(), "Id", "Name", 0);
            filter.Departments = new SelectList(await GetDepartments(), "Id", "Name", 0);

            return View(new ListStaffModel { Filter = filter });
        }

        [HttpGet]
        public async Task<ActionResult> _Select()
        {
            var filter = new FilterStaffModel();

            filter.Branchs = new SelectList(await GetBranches(), "Id", "Name", 0);
            filter.Departments = new SelectList(await GetDepartments(), "Id", "Name", 0);

            return View(new ListStaffModel { Filter = filter });
        }

        [HttpGet]
        public async Task<ActionResult> _Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsStaffModel>(HttpVerbs.Get, $"Administration/Staff?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            //model.Roles = new SelectList(await GetRoles(), "Id", "Name", 0);

            return View(model);

        }

        [NonAction]
        private async Task<IEnumerable<BranchModel>> GetBranches()
        {

            var branches = Enumerable.Empty<BranchModel>();

            var response = await WepApiMethod.SendApiAsync<List<BranchModel>>(HttpVerbs.Get, $"Administration/Branch");

            if (response.isSuccess)
            {
                branches = response.Data.OrderBy(o => o.Name);
            }

            return branches;

        }

        [NonAction]
        private async Task<IEnumerable<DepartmentModel>> GetDepartments()
        {

            var departments = Enumerable.Empty<DepartmentModel>();

            var response = await WepApiMethod.SendApiAsync<List<DepartmentModel>>(HttpVerbs.Get, $"Administration/Department");

            if (response.isSuccess)
            {
                departments = response.Data.OrderBy(o => o.Name);
            }

            return departments;

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

        
    }
}