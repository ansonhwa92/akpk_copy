using FEP.Helper;
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
    public class RoleController : FEPController
    {
        
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

                    await LogActivity(Modules.Setting, "Create Role", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = "Fail to add new role";

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name, string Description)
        {

            var model = new EditRoleModel
            {
                Id = id,               
                No = No,
                Name = Name,
                Description = Description,
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

                    await LogActivity(Modules.Setting, "Update Role", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = "Fail to update role";

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name, string Description)
        {

            var model = new DeleteRoleModel
            {
                Id = id,
                No = No,
                Name = Name,
                Description = Description
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

                await LogActivity(Modules.Setting, "Delete Role", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Fail to delete role";

            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<ActionResult> Access(int id, Modules? module)
        {
            var model = new AccessModel();

            var responseRoles = await WepApiMethod.SendApiAsync<List<RoleModel>>(HttpVerbs.Get, $"Administration/Role");

            if (responseRoles.isSuccess)
            {
                model.Roles = responseRoles.Data;
            }
            
            var responseRole = await WepApiMethod.SendApiAsync<RoleAccessModel>(HttpVerbs.Get, $"Administration/Role/GetAccess?roleId={id}&module={module}");

            if (responseRole.isSuccess)
            {
                model.RoleId = id;
                model.RoleName = responseRole.Data.RoleName;
                model.Module = module;

                int i = 0;
                var name = "";
                var str = new StringBuilder();
                model.Access = new Dictionary<UserAccess, string>();

                foreach (var useraccess in responseRole.Data.UserAccesses.OrderBy(o => o.UserAccess))
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

                await LogActivity(Modules.Setting, "Update Role Access", model);

                return RedirectToAction("Access", new { id = model.RoleId });
            }
            
            TempData["ErrorMessage"] = "Fail to update role access.";
            return RedirectToAction("Access", new { id = model.RoleId });

        }

        [HttpGet]
        public async Task<ActionResult> UserRole(int? id)
        {
            var model = new UserRoleModel();

            var responseRoles = await WepApiMethod.SendApiAsync<List<RoleModel>>(HttpVerbs.Get, $"Administration/Role");

            if (responseRoles.isSuccess)
            {
                model.Roles = responseRoles.Data;
            }

            var responseRole = await WepApiMethod.SendApiAsync<RoleModel>(HttpVerbs.Get, $"Administration/Role?id={id}");

            if (!responseRole.isSuccess)
            {
                return HttpNotFound();
            }
                        
            model.RoleId = responseRole.Data.Id;
            model.RoleName = responseRole.Data.Name;
            model.Users = new ListUserModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(int RoleId, int[] Ids)
        {

            var model = new UpdateUserRoleModel {
                RoleId = RoleId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Administration/Role/AddUser", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully added to the role.";
                await LogActivity(Modules.Setting, "Add User To Role", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to add user.";
            }


            return RedirectToAction("UserRole", "Role", new { area = "Administrator", id = RoleId });

        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int RoleId, int[] Ids)
        {

            var model = new UpdateUserRoleModel
            {
                RoleId = RoleId,
                UserId = Ids.ToList()
            };

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Administration/Role/DeleteUser", model);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "User successfully remove from role.";
                await LogActivity(Modules.Setting, "Remove User From Role", model);
            }
            else
            {
                TempData["ErrorMessage"] = "Fail to remove user.";
            }


            return RedirectToAction("UserRole", "Role", new { area = "Administrator", id = RoleId });

        }


    }
}