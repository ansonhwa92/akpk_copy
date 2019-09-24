using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseCategoryController : FEPController
    {
        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<CourseCategoryModel>>(HttpVerbs.Get, $"eLearning/eLearning.CourseCategory");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<CourseCategoryModel>());
        }

        public ActionResult _Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateCourseCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/eLearning.CourseCategory/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/eLearning.CourseCategory", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.eLearning.CourseCategory.AlertSuccessCreate;

                    LogActivity(Modules.Learning, "Create Parameter Course Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.eLearning.CourseCategory.AlertFailCreate;

            return RedirectToAction("List");

        }

        public ActionResult _Edit(int id, string No, string Name)
        {

            var model = new EditCourseCategoryModel
            {
                Id = id,
                No = No,
                Name = Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditCourseCategoryModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/eLearning.CourseCategory/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eLearning/eLearning.CourseCategory?id={model.Id}", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.eLearning.CourseCategory.AlertSuccessUpdate;

                    LogActivity(Modules.Learning, "Update Parameter Course Category", model);

                    return RedirectToAction("List");
                }
            }

            TempData["ErrorMessage"] = Language.eLearning.CourseCategory.AlertFailUpdate;

            return RedirectToAction("List");

        }

        public ActionResult _Delete(int id, string No, string Name)
        {

            var model = new DeleteCourseCategoryModel
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

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eLearning/eLearning.CourseCategory?id={id}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = Language.eLearning.CourseCategory.AlertSuccessDelete;

                LogActivity(Modules.Learning, "Delete Parameter Course Category", new { id = id });

                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = Language.eLearning.CourseCategory.AlertFailDelete;

            return RedirectToAction("List");

        }
    }
}