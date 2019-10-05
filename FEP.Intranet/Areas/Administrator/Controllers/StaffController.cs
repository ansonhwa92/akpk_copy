using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
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
                await LogActivity(Modules.Admin, "Activate Staff Account", new { id = id });

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
                await LogActivity(Modules.Admin, "Disable Staff Account", new { id = id });

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
    }
}