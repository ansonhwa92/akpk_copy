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
    [LogError(Modules.Learning)]
    public class TOTReportController : FEPController
    {
        // GET: eLearning/TOTReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {         
            return View();
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

            model.CreatedBy = model.CreatedBy + " (" + model.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") + ")";

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new Models.CreateTOTReportModel();
        
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.CreateTOTReportModel model)
        {
            
            if (model.StartDate.Value.Date == model.EndDate.Value.Date)
            {
                if(model.StartTime.Value.TimeOfDay > model.EndTime.Value.TimeOfDay )
                {
                    ModelState.AddModelError("EndTime", Language.TOT.ValidStartEndTime);
                }
            }


            if (ModelState.IsValid)
            {
                var modelapi = new WebApiModel.eLearning.CreateTOTReportModel()
                {                   
                    CreatedBy = CurrentUser.UserId,
                    Module = model.Module,
                    StartDate = model.StartDate.Value.Date + new TimeSpan(model.StartTime.Value.Hour,model.StartTime.Value.Minute,model.StartTime.Value.Second),
                    EndDate = model.EndDate.Value.Date + new TimeSpan(model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second),
                    Venue = model.Venue,
                    Organization = model.Organization,
                    AgeR1NoOfMale = model.AgeR1NoOfMale,
                    AgeR1NoOfFemale = model.AgeR1NoOfFemale,
                    AgeR2NoOfMale = model.AgeR2NoOfMale,
                    AgeR2NoOfFemale = model.AgeR2NoOfFemale,
                    AgeR3NoOfMale = model.AgeR3NoOfMale,
                    AgeR3NoOfFemale = model.AgeR3NoOfFemale,
                    AgeR4NoOfMale = model.AgeR4NoOfMale,
                    AgeR4NoOfFemale = model.AgeR4NoOfFemale,
                    AgeR5NoOfMale = model.AgeR5NoOfMale,
                    AgeR5NoOfFemale = model.AgeR5NoOfFemale,
                    SalaryR1NoOfMale = model.SalaryR1NoOfMale,
                    SalaryR1NoOfFemale = model.SalaryR1NoOfFemale,
                    SalaryR2NoOfMale = model.SalaryR2NoOfMale,
                    SalaryR2NoOfFemale = model.SalaryR2NoOfFemale,
                    SalaryR3NoOfMale = model.SalaryR3NoOfMale,
                    SalaryR3NoOfFemale = model.SalaryR3NoOfFemale,
                    SalaryR4NoOfMale = model.SalaryR4NoOfMale,
                    SalaryR4NoOfFemale = model.SalaryR4NoOfFemale,
                    SalaryR5NoOfMale = model.SalaryR5NoOfMale,
                    SalaryR5NoOfFemale = model.SalaryR5NoOfFemale,
                    SalaryR6NoOfMale = model.SalaryR6NoOfMale,
                    SalaryR6NoOfFemale = model.SalaryR6NoOfFemale
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

                var response = await WepApiMethod.SendApiAsync<TOTReport>(HttpVerbs.Post, $"eLearning/TOTReport", modelapi);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Learning, "Create Train Of Trainer Report", model);

                    TempData["SuccessMessage"] = Language.TOT.AlertCreateSuccess;

                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = Language.TOT.AlertCreateFail;
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
                StartTime = data.StartDate,
                EndDate = data.EndDate,
                EndTime = data.EndDate,               
                Module = data.Module,
                Venue = data.Venue,
                Organization = data.Organization,
                AgeR1NoOfMale = data.AgeR1NoOfMale,
                AgeR1NoOfFemale = data.AgeR1NoOfFemale,
                AgeR2NoOfMale = data.AgeR2NoOfMale,
                AgeR2NoOfFemale = data.AgeR2NoOfFemale,
                AgeR3NoOfMale = data.AgeR3NoOfMale,
                AgeR3NoOfFemale = data.AgeR3NoOfFemale,
                AgeR4NoOfMale = data.AgeR4NoOfMale,
                AgeR4NoOfFemale = data.AgeR4NoOfFemale,
                AgeR5NoOfMale = data.AgeR5NoOfMale,
                AgeR5NoOfFemale = data.AgeR5NoOfFemale,
                SalaryR1NoOfMale = data.SalaryR1NoOfMale,
                SalaryR1NoOfFemale = data.SalaryR1NoOfFemale,
                SalaryR2NoOfMale = data.SalaryR2NoOfMale,
                SalaryR2NoOfFemale = data.SalaryR2NoOfFemale,
                SalaryR3NoOfMale = data.SalaryR3NoOfMale,
                SalaryR3NoOfFemale = data.SalaryR3NoOfFemale,
                SalaryR4NoOfMale = data.SalaryR4NoOfMale,
                SalaryR4NoOfFemale = data.SalaryR4NoOfFemale,
                SalaryR5NoOfMale = data.SalaryR5NoOfMale,
                SalaryR5NoOfFemale = data.SalaryR5NoOfFemale,
                SalaryR6NoOfMale = data.SalaryR6NoOfMale,
                SalaryR6NoOfFemale = data.SalaryR6NoOfFemale,
                Attachments = data.Attachments
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Models.EditTOTReportModel model)
        {
            if (model.StartDate.Value.Date == model.EndDate.Value.Date)
            {
                if (model.StartTime.Value.TimeOfDay > model.EndTime.Value.TimeOfDay)
                {
                    ModelState.AddModelError("EndTime", Language.TOT.ValidStartEndTime);
                }
            }

            if (ModelState.IsValid)
            {
                var modelapi = new WebApiModel.eLearning.EditTOTReportModel()
                {
                    Id = model.Id,                   
                    Module = model.Module,
                    StartDate = model.StartDate.Value.Date + new TimeSpan(model.StartTime.Value.Hour, model.StartTime.Value.Minute, model.StartTime.Value.Second),
                    EndDate = model.EndDate.Value.Date + new TimeSpan(model.EndTime.Value.Hour, model.EndTime.Value.Minute, model.EndTime.Value.Second),
                    Venue = model.Venue,
                    Organization = model.Organization,
                    AgeR1NoOfMale = model.AgeR1NoOfMale,
                    AgeR1NoOfFemale = model.AgeR1NoOfFemale,
                    AgeR2NoOfMale = model.AgeR2NoOfMale,
                    AgeR2NoOfFemale = model.AgeR2NoOfFemale,
                    AgeR3NoOfMale = model.AgeR3NoOfMale,
                    AgeR3NoOfFemale = model.AgeR3NoOfFemale,
                    AgeR4NoOfMale = model.AgeR4NoOfMale,
                    AgeR4NoOfFemale = model.AgeR4NoOfFemale,
                    AgeR5NoOfMale = model.AgeR5NoOfMale,
                    AgeR5NoOfFemale = model.AgeR5NoOfFemale,
                    SalaryR1NoOfMale = model.SalaryR1NoOfMale,
                    SalaryR1NoOfFemale = model.SalaryR1NoOfFemale,
                    SalaryR2NoOfMale = model.SalaryR2NoOfMale,
                    SalaryR2NoOfFemale = model.SalaryR2NoOfFemale,
                    SalaryR3NoOfMale = model.SalaryR3NoOfMale,
                    SalaryR3NoOfFemale = model.SalaryR3NoOfFemale,
                    SalaryR4NoOfMale = model.SalaryR4NoOfMale,
                    SalaryR4NoOfFemale = model.SalaryR4NoOfFemale,
                    SalaryR5NoOfMale = model.SalaryR5NoOfMale,
                    SalaryR5NoOfFemale = model.SalaryR5NoOfFemale,
                    SalaryR6NoOfMale = model.SalaryR6NoOfMale,
                    SalaryR6NoOfFemale = model.SalaryR6NoOfFemale
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

                    TempData["SuccessMessage"] = Language.TOT.AlertEditSuccess;

                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = Language.TOT.AlertEditFail;
                }
            }

   
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
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eLearning/TOTReport?id={id}");

            if (response.isSuccess)
            {
                await LogActivity(Modules.Learning, "Delete Train Of Trainer Report");

                TempData["SuccessMessage"] = Language.TOT.AlertDeleteSuccess;

                return RedirectToAction("List", "TOTReport", new { area = "eLearning" });
            }

            TempData["ErrorMessage"] = Language.TOT.AlertDeleteFail;

            return RedirectToAction("Delete", new { id = id });
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

            var response = await WepApiMethod.SendApiAsync<List<TOTModuleModel>>(HttpVerbs.Get, $"eLearning/TOTReport/GetModules?courseId={CourseId}");

            if (response.isSuccess)
            {
                courses = response.Data.OrderBy(o => o.Name);
            }

            return courses;
        }


    }
}