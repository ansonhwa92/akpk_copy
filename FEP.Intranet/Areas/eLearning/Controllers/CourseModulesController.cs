using AutoMapper;
using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
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
        public const string Start = "eLearning/CourseModules/Start";
    }

    [Authorize]
    public class CourseModulesController : FEPController
    {
        private DbEntities db = new DbEntities();
        private readonly IMapper _mapper;

        public CourseModulesController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditModuleModel, CourseModule>();

                cfg.CreateMap<CourseModule, CreateOrEditModuleModel>();

                //cfg.CreateMap<CreateOrEditModuleModel, CourseRuleModel>();
            });

            _mapper = config.CreateMapper();
        }

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
            }

            TempData["ErrorMessage"] = "Cannot add module. Please ensure all required fields are filled in.";

            return View(model);
        }

        // GET: eLearning/CourseModules/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Cannot find such module";

                return Redirect(Request.UrlReferrer.ToString());
            }
            CreateOrEditModuleModel courseModule = await TryGetModule(id.Value);

            if (courseModule == null)
            {
                TempData["ErrorMessage"] = "Cannot find such module";

                return Redirect(Request.UrlReferrer.ToString());
            }

            ViewBag.CourseTitle = courseModule.CourseTitle;

            return View(courseModule);
        }

        // GET: eLearning/CourseModules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModule courseModule = await db.CourseModules.FindAsync(id);
            if (courseModule == null)
            {
                return HttpNotFound();
            }

            Course course = await db.Courses.FindAsync(courseModule.CourseId);
            if (course == null)
            {
                return HttpNotFound();
            }

            CreateOrEditModuleModel model = _mapper.Map<CreateOrEditModuleModel>(courseModule);

            ViewBag.CourseTitle = course.Title;
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseModule.CourseId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateOrEditModuleModel model, string Submittype)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/CourseModules/Edit", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Edit Module: " + response.Data, model);

                    if (Submittype == "Save")
                    {
                        TempData["SuccessMessage"] = "Module titled " + response.Data + " updated successfully and saved as draft.";

                        return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.Id });
                    }
                    else
                    {
                        return RedirectToAction("Review", "Courses", new { area = "eLearning", @id = model.Id });
                    }
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to edit Course.";

                    return RedirectToAction("Details", "Courses", new { area = "eLearning", @id = model.Id });
                }
            }
            return View(model);
        }

        // GET: eLearning/CourseModules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = await WepApiMethod.SendApiAsync<CreateOrEditModuleModel>(HttpVerbs.Get, $"eLearning/CourseModules?id={id}");

            if (!course.isSuccess)
            {
                return HttpNotFound();
            }

            var vm = course.Data;

            if (vm == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseTitle = vm.Title;

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var module = db.CourseModules.Where(p => p.Id == id).FirstOrDefault();

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"eLearning/CourseModules/Delete?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Module: " + response.Data);

                TempData["SuccessMessage"] = "Module titled " + response.Data + " successfully deleted.";

                return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = module.CourseId });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete module.";

                return RedirectToAction("Details", "Courses", new { area = "eLearning", @id = id });
            }
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

                return RedirectToAction("Index", "Courses", new { area = "eLearning" });
            }

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

                return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = Id.Value });
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

        // Start the module
        public async Task<ActionResult> Start(int id)
        {
            //var content = await db.CourseContents.Where(x => x.CourseModuleId == id).OrderBy(x => x.Order).FirstOrDefaultAsync();

            //if (content == null)
            //    return HttpNotFound();

            var response = await WepApiMethod.SendApiAsync<CourseContent>(HttpVerbs.Get, ModuleApiUrl.Start + $"?id={id}");

            if (response.isSuccess)
            {
                return RedirectToAction("View", "CourseContents", new { area = "eLearning", @id = response.Data.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Could not find the next content";

                return RedirectToAction("View", "CourseModules", new { area = "eLearning", @id = id });
            }
        }
    }
}