using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.Intranet.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
//using FEP.WebApiModel;
using FEP.Model;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.RnP;
using System.Net;

namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class SurveyController : FEPController
    {

        // GET: RnP/Survey
        public async Task<ActionResult> Index()
        {
            var resSurveys = await WepApiMethod.SendApiAsync<IEnumerable<ReturnSurveyModel>>(HttpVerbs.Get, $"RnP/Survey");

            if (resSurveys.isSuccess)
            {
                var surveys = resSurveys.Data;
                if (surveys == null)
                {
                    return HttpNotFound();
                }
                return View(surveys);
            }
            return View();
        }

        // Show create form
        // GET: RnP/Survey/Create
        public ActionResult Create()
        {
            var model = new UpdateSurveyModel();
            return View(model);
        }

        // Process create form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UpdateSurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/CreateSurvey", model);

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification to others upon submission

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });

                }
            }

            return View(model);
        }

        // Show edit form
        // GET: RnP/Survey/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (resSurvey.isSuccess)
            {
                var survey = resSurvey.Data;
                if (survey == null)
                {
                    return HttpNotFound();
                }
                var vmsurvey = new UpdateSurveyModel
                {
                    ID = survey.ID,
                    Type = survey.Type,
                    Category = survey.Category,
                    Title = survey.Title,
                    Description = survey.Description,
                    TargetGroup = survey.TargetGroup,
                    StartDate = survey.StartDate,
                    EndDate = survey.EndDate,
                    Pictures = survey.Pictures,
                    ProofOfApproval = survey.ProofOfApproval
                };
                return View(vmsurvey);
            }
            return View();
        }

        // Process edit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateSurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/EditSurvey", model);

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification to others upon submission

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });

                }
            }

            return View();
        }

        // Show view form
        // GET: RnP/Survey/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (resSurvey.isSuccess)
            {
                var survey = resSurvey.Data;
                if (survey == null)
                {
                    return HttpNotFound();
                }
                return View(survey);
            }
            return View();
        }

        // Show delete form
        // GET: RnP/Survey/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (resSurvey.isSuccess)
            {
                var survey = resSurvey.Data;
                if (survey == null)
                {
                    return HttpNotFound();
                }
                return View(survey);
            }
            return View();
        }

        // Process delete form
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(UpdateSurveyModel model)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/DeleteSurvey?id={id}");

                if (response.isSuccess)
                {

                    // success notification
                    // email/sms/system notification

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });

                }
            }

            return View();
        }

        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}

