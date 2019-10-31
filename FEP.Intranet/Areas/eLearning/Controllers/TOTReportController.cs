using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Models;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    [LogError(Model.Modules.Learning)]
    public class TOTReportController : FEPController
    {
        // GET: eLearning/TOTReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var filter = new FilterTOTReportModel();

            filter.Courses = new SelectList(await GetCourses(), "Id", "Name", 0);

            return View(new ListTOTReportModel { Filter = filter });

        }

        [HttpPost]
        public async Task<ActionResult> List(FilterTOTReportModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<DataTableResponse>(HttpVerbs.Post, $"eLearning/TOTReport/GetAll", filter);

            return Content(JsonConvert.SerializeObject(response.Data), "application/json");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsTOTReportModel>(HttpVerbs.Get, $"eLearning/TOTReport?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new Models.CreateTOTReportModel();
           
            model.Courses = new SelectList(await GetCourses(), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.CreateTOTReportModel model)
        {


            if (ModelState.IsValid)
            {
                var modelapi = new WebApiModel.eLearning.CreateTOTReportModel()
                {                   
                    CreatedBy = CurrentUser.UserId,
                    CourseId = model.CourseId,
                    ModuleId = model.ModuleId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Venue = model.Venue,
                    NoOfMale = model.NoOfMale,
                    NoOfFemale = model.NoOfFemale,
                    AgeRange = model.AgeRange,
                    SalaryRange = model.SalaryRange
                };

                //attachment
                if (model.AttachmentFiles.Count() > 0)
                {
                    var responseFile = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

                    if (responseFile != null)
                    {
                        modelapi.FilesId = responseFile.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/TOTReport", modelapi);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Create Train Of Trainer Report", model);

                    TempData["SuccessMessage"] = "Report successfully created";

                    return RedirectToAction("List");
                }
            }
            
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<Models.EditTOTReportModel>(HttpVerbs.Get, $"eLearning/TOTReport?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var data = response.Data;

            var model = new Models.EditTOTReportModel()
            {
                Id = data.Id,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                CourseId = data.CourseId,
                ModuleId = data.ModuleId,
                Venue = data.Venue,
                AgeRange = data.AgeRange,
                SalaryRange = data.SalaryRange,
                NoOfMale = data.NoOfMale,
                NoOfFemale = data.NoOfFemale,
                Attachments = response.Data.Attachments
            };

            model.Courses = new SelectList(await GetCourses(), "Id", "Name", 0);
            model.Modules = new SelectList(await GetModules(data.CourseId), "Id", "Name", 0);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.EditTOTReportModel model)
        { 

            if (ModelState.IsValid)
            {
                var modelapi = new WebApiModel.eLearning.EditTOTReportModel()
                {
                    Id = model.Id,
                    CourseId = model.CourseId,
                    ModuleId = model.ModuleId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Venue = model.Venue,
                    NoOfMale = model.NoOfMale,
                    NoOfFemale = model.NoOfFemale,
                    AgeRange = model.AgeRange,
                    SalaryRange = model.SalaryRange
                };

                //attachment
                if (model.AttachmentFiles.Count() > 0)
                {
                    var responseFile = await FileMethod.UploadFile(model.AttachmentFiles.ToList(), CurrentUser.UserId);

                    if (responseFile != null)
                    {
                        modelapi.FilesId = responseFile.Select(f => f.Id).ToList();
                    }
                }

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eLearning/TOTReport?id={model.Id}", modelapi);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Update Train Of Trainer Report", model);

                    TempData["SuccessMessage"] = "Report successfully updated";

                    return RedirectToAction("List");
                }
            }

            model.Courses = new SelectList(await GetCourses(), "Id", "Name", 0);
            model.Modules = new SelectList(await GetModules(model.CourseId), "Id", "Name", 0);


            return View(model);

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<DetailsTOTReportModel>(HttpVerbs.Get, $"eLearning/TOTReport?id={id}");

            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            var model = response.Data;

            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/EventSpeaker?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Train Of Trainer Report");

                TempData["SuccessMessage"] = "Report successfully deleted";

                return RedirectToAction("List", "TOTReport", new { area = "eLearning" });
            }

            TempData["ErrorMessage"] = "Fail to delete Report";

            return RedirectToAction("Details");
        }



        [NonAction]
        private async Task<IEnumerable<TOTCourseModel>> GetCourses()
        {

            var courses = Enumerable.Empty<TOTCourseModel>();

            var response = await WepApiMethod.SendApiAsync<List<TOTCourseModel>>(HttpVerbs.Get, $"eLearning/TOTReport/GetCourses");

            if (response.isSuccess)
            {
                courses = response.Data.OrderBy(o => o.Name);
            }

            return courses;
        }

        [NonAction]
        private async Task<IEnumerable<TOTModuleModel>> GetModules(int CourseId)
        {

            var courses = Enumerable.Empty<TOTModuleModel>();

            var response = await WepApiMethod.SendApiAsync<List<TOTModuleModel>>(HttpVerbs.Get, $"eLearning/TOTReport/GetCourses?id={CourseId}");

            if (response.isSuccess)
            {
                courses = response.Data.OrderBy(o => o.Name);
            }

            return courses;
        }

    }
}