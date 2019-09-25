using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FEP.Model;
using FEP.Model.eLearning;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseContentsController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: eLearning/CourseContents
        public async Task<ActionResult> Index()
        {
            var moduleContents = db.ModuleContents.Include(c => c.Course).Include(c => c.CourseModule);
            return View(await moduleContents.ToListAsync());
        }

        // GET: eLearning/CourseContents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseContent courseContent = await db.ModuleContents.FindAsync(id);
            if (courseContent == null)
            {
                return HttpNotFound();
            }
            return View(courseContent);
        }

        // GET: eLearning/CourseContents/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title");
            ViewBag.CourseModuleId = new SelectList(db.CourseModules, "Id", "Title");
            return View();
        }

        // POST: eLearning/CourseContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,CourseModuleId,CourseId,IsViewable,ContentType,Title,CompletionType,Order,QuestionType,Timer,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] CourseContent courseContent)
        {
            if (ModelState.IsValid)
            {
                db.ModuleContents.Add(courseContent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseContent.CourseId);
            ViewBag.CourseModuleId = new SelectList(db.CourseModules, "Id", "Title", courseContent.CourseModuleId);
            return View(courseContent);
        }

        // GET: eLearning/CourseContents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseContent courseContent = await db.ModuleContents.FindAsync(id);
            if (courseContent == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseContent.CourseId);
            ViewBag.CourseModuleId = new SelectList(db.CourseModules, "Id", "Title", courseContent.CourseModuleId);
            return View(courseContent);
        }

        // POST: eLearning/CourseContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,CourseModuleId,CourseId,IsViewable,ContentType,Title,CompletionType,Order,QuestionType,Timer,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate")] CourseContent courseContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseContent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", courseContent.CourseId);
            ViewBag.CourseModuleId = new SelectList(db.CourseModules, "Id", "Title", courseContent.CourseModuleId);
            return View(courseContent);
        }

        // GET: eLearning/CourseContents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseContent courseContent = await db.ModuleContents.FindAsync(id);
            if (courseContent == null)
            {
                return HttpNotFound();
            }
            return View(courseContent);
        }

        // POST: eLearning/CourseContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseContent courseContent = await db.ModuleContents.FindAsync(id);
            db.ModuleContents.Remove(courseContent);
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
    }
}
