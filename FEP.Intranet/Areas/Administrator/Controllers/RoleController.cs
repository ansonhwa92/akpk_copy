using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class RoleController : Controller
    {
        // GET: Administrator/Role
        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<RoleModel>>(HttpVerbs.Get, $"Administration/Role");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<RoleModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateRoleModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Role/IsRoleNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = "Role Name already registered in the system";
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"Administration/Role", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Role successfully added";

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = "Fail to add new role";

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name)
        {

            var model = new EditRoleModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditRoleModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/Role/IsRoleNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = "Role Name already registered in the system";
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/Role", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Role successfully updated";

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = "Fail to update role";

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name)
        {

            var model = new DeleteRoleModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Delete(int id)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"Administration/Role?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "Role successfully deleted";

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Fail to delete role";

            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<ActionResult> Access(int id, Modules? module)
        {
            var response = await WepApiMethod.SendApiAsync<RoleAccessModel>(HttpVerbs.Get, $"Administration/Role/GetAccess?roleId={id}&module={module}");

            var model = new AccessModel();

            if (response.isSuccess)
            {
                model.RoleId = id;
                model.RoleName = response.Data.RoleName;
                model.Module = module;

                int i = 0;
                var name = "";
                var str = new StringBuilder();
                model.Access = new Dictionary<UserAccess, string>();

                foreach (var useraccess in response.Data.UserAccesses.OrderBy(o => o.UserAccess))
                {
                    str = new StringBuilder();

                    name = "Access[" + i.ToString() + "]";

                    str.Append("<div class='custom-control custom-checkbox'>");
                    str.Append("<input type='hidden' name='" + name + ".UserAccess' value='" + useraccess.UserAccess.ToString() + "' />");
                    str.Append("<input type='checkbox' name='" + name + ".IsCheck' value='true' " + (useraccess.IsCheck ? "checked" : "") + " class='custom-control-input js-check-selected-row' id='" + useraccess.UserAccess.ToString() + "' />");
                    str.Append("<label class='custom-control-label' for='" + useraccess.UserAccess.ToString() + "'>");
                    str.Append("<span class='text-hide'>Check</span>");
                    str.Append("</label>");
                    str.Append("</div>");

                    model.Access.Add(useraccess.UserAccess, str.ToString());

                    ++i;
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Access(UpdateRoleAccessModel model)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Administration/Role/UpdateAccess", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "Role access successfully updated.";
                return RedirectToAction("Access", new { id = model.RoleId });
            }
            
            TempData["ErrorMessage"] = "Fail to update role access.";
            return RedirectToAction("Access", new { id = model.RoleId });

        }


        

    }
}