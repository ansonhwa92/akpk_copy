using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class ModuleApiUrl
    {
        public const string Create = "eLearning/CourseModules/Create";
        public const string GetModule = "eLearning/CourseModules";

        public const string Content = "eLearning/CourseModules/Content";
    }

    [Authorize]
    public class CourseModulesController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: eLearning/CourseModules/Create
        public ActionResult Create(int? id, string courseTitle)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CreateOrEditModuleModel model = new CreateOrEditModuleModel
            {
                CourseId = id.Value,
                CourseTitle = courseTitle
            };

            return View(model);
        }

        // POST: eLearning/CourseModules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrEditModuleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, ModuleApiUrl.Create, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Module successfully added.";

                    await LogActivity(Modules.Learning, "Create Module : " + model.Title);

                    var id = response.Data;

                    if (!String.IsNullOrEmpty(id))
                        return RedirectToAction("Content", "CourseModules", new { id = id });
                    else
                        return RedirectToAction("Index", "Courses");
                }

                //// --- FOR TESTING ONLY ----
                ///

                //var courseModules = db.CourseModules.Where(x => x.CourseId == model.CourseId);

                //var module = new CourseModule
                //{
                //    CourseId = model.CourseId,
                //    Objectives = model.Objectives,
                //    Description = model.Description,
                //    Title = model.Title,
                //    Order = courseModules != null ? (courseModules.Count() + 1) : 0
                //};

                //var vm = db.CourseModules.Add(module);

                //await db.SaveChangesAsync();

                //return RedirectToAction("Content", "Courses", new { id = vm.Id });
            }

            TempData["ErrorMessage"] = "Cannot add module. Please ensure all required fields are filled in.";

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> Content(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get Model
            var model = await TryGetModule(id.Value);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such module.";

                return RedirectToAction("Index", "CourseModules");
            }

            model.ModuleContents = model.ModuleContents.OrderBy(x => x.Order).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Content(int? Id, int CreatedBy, string order)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (String.IsNullOrEmpty(order))
            {
                RedirectToAction("Content", new { id = Id.Value });
            }

            var response = await WepApiMethod.SendApiAsync<CreateOrEditModuleModel>(HttpVerbs.Post,
                ModuleApiUrl.Content + $"?Id={Id}&CreatedBy={CreatedBy}&order={order}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "Changes saved";

                var model = response.Data;

                await LogActivity(Modules.Learning, "Update Module Content : " + model.Title);

                return View(model);
            }

            TempData["ErrorMessage"] = "Error in saving order.";

            // get Model
            var vm = await TryGetModule(Id.Value);

            if (vm == null)
            {
                TempData["ErrorMessage"] = "No such module.";

                return RedirectToAction("Content", "CourseModules", new { id = Id.Value });
            }
            
            vm.ModuleContents = vm.ModuleContents.OrderBy(x => x.Order).ToList();

            return View(vm);
        }

        public async Task<CreateOrEditModuleModel> TryGetModule(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditModuleModel>(HttpVerbs.Get, ModuleApiUrl.GetModule + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }
    }
}