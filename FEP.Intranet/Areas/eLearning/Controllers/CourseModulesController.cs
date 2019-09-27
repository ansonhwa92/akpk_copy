using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Data.Entity;
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
    }

    [Authorize]
    public class CourseModulesController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: eLearning/CourseModules
        public async Task<ActionResult> Index()
        {
            var courseModules = db.CourseModules;
            return View(await courseModules.ToListAsync());
        }

        // GET: eLearning/CourseModules/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(courseModule);
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
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseModule.CourseId);
            return View(courseModule);
        }

        // POST: eLearning/CourseModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Order,Objectives,CourseId,TotalVideo,TotalAudio,TotalTest,TotalAssignment,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] CourseModule courseModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseModule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseModule.CourseId);
            return View(courseModule);
        }

        // GET: eLearning/CourseModules/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(courseModule);
        }

        // POST: eLearning/CourseModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseModule courseModule = await db.CourseModules.FindAsync(id);
            db.CourseModules.Remove(courseModule);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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

            return View(model);
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