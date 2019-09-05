using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using Newtonsoft.Json;
//using FEP.WebApiModel;
using FEP.Model;
using FEP.WebApiModel.RnP;
using System.Net;

namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class SurveyController : FEPController
    {

        /*
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
        */

        // GET: RnP/Survey
        public ActionResult Index()
        {
            return View();
        }

        // Show select survey type form
        // After type selection, user automatically redirected to creation page
        // GET: RnP/Survey/SelectCategory
        public ActionResult SelectType()
        {
            //ViewBag.Categories = new List<SurveyType>();
            ViewBag.SurveyTypes = new SurveyType();
            return View();
        }

        // Show create form (blank form so no api call needed)
        // GET: RnP/Survey/Create
        public ActionResult Create(int? typeid)
        {
            var model = new UpdateSurveyModel();
            if (typeid != null)
            {
                //ViewBag.TypeId = typeid;
                if (typeid == 0)
                {
                    ViewBag.TypeName = "Public Mass";
                    model.Type = SurveyType.Public;
                }
                else if (typeid == 1)
                {
                    ViewBag.TypeName = "Targeted Groups";
                    model.Type = SurveyType.Targeted;
                }
                else
                {
                    ViewBag.TypeName = "Public Mass";
                    model.Type = SurveyType.Public;
                }
            }
            else
            {
                ViewBag.TypeName = "Public Mass";
                model.Type = SurveyType.Public;
            }
            return View(model);
        }

        // Process creation form
        // After creation, user automatically redirected to survey builder page if successful, and list page if failed.
        // This function always saves as draft!
        // POST: RnP/Survey/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UpdateSurveyModel model)
        {
            /*
            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Survey/TitleExists?id={null}&title={model.Title}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title", "A Survey with the same Title already exists in the system");
            }
            */

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Create", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string newid = resparray[0];
                    string title = resparray[1];

                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Create New Survey: " + title);

                    TempData["SuccessMessage"] = "New Survey titled " + title + " created successfully and saved as draft.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Build", "Survey", new { area = "RnP", @id = newid });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to create new Survey.";

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
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

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

        // Process edit form
        // After editing, user automatically redirected to builder page if successful, and details page if failed.
        // POST: Survey/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateSurveyModel model)
        {
            /*
            var dupTitleResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"RnP/Survey/TitleExists?id={model.ID}&title={model.Title}");

            if (dupTitleResponse.Data)
            {
                ModelState.AddModelError("Title", "A Survey with the same Title already exists in the system");
            }
            */

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Edit", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Edit Survey: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey titled " + response.Data + " updated successfully and saved as draft.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Build", "Survey", new { area = "RnP", @id = model.ID });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to edit Survey.";

                    return RedirectToAction("Details", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Show build survey form
        // Retrieve saved-as-draft survey info based on id first
        // GET: RnP/Survey/Build
        public async Task<ActionResult> Build(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resPub.Data;
            if (survey == null)
            {
                return HttpNotFound();
            }

            var vmcontents = new UpdateSurveyContentsModel
            {
                ID = survey.ID,
                Contents = survey.Contents
            };

            return View(vmcontents);
        }

        // Process build form
        // After editing build, user automatically redirected to review page if successful, and details page if failed.
        // POST: Survey/Build/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Build(UpdateSurveyContentsModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Build", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Build Survey: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey titled " + response.Data + " built successfully and saved as draft.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Review", "Survey", new { area = "RnP", @id = model.ID });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to build Survey.";

                    return RedirectToAction("Details", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Show review form
        // User is redirected here after saving as draft at builder creation or builder editing page
        // GET: RnP/Survey/Review/5
        public async Task<ActionResult> Review(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetForReview?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

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

            var vmcontents = new UpdateSurveyContentsModel
            {
                ID = survey.ID,
                Contents = survey.Contents
            };

            var vmreview = new UpdateSurveyReviewModel
            {
                Survey = vmsurvey,
                Contents = vmcontents
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<SurveyApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Survey/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            return View(vmreview);
        }

        // Process review form (i.e. submit)
        // After submitting, user automatically redirected to list page if successful, and back to review page if failed.
        // POST: Survey/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Review(UpdateSurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Submit", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Submit Survey: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey titled " + response.Data + " submitted successfully for verification.";

                    // dashboard

                    //SendEmail("New Survey Submitted", "A new Survey has been submitted for verification." + "\n" + "Please login to AKPK-FEP and review the submission.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit Survey.";

                    return RedirectToAction("Review", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Process submission from details page
        // Called for direct submission via id
        // GET: RnP/Survey/SubmitByID/5
        public async Task<ActionResult> SubmitByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SubmitByID?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Submit Survey: " + response.Data);

                TempData["SuccessMessage"] = "Survey titled " + response.Data + " submitted successfully for verification.";

                // dashboard

                //SendEmail("New Survey Submitted", "A new Survey has been submitted for verification." + "\n" + "Please login to AKPK-FEP and review the submission.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Survey", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to submit Survey.";

                return RedirectToAction("Details", "Survey", new { area = "RnP", @id = id });
            }
        }

        // Show view form
        // From here user can do one of the following:
        // 1. Admin can edit the survey if it's not submitted yet (redirection button)
        // 2. Admin can delete the survey if it's not submitted yet (direct prompt) (KIV)
        // 3. Admin can submit the survey if it's not been submitted yet
        // 4. Admin can cancel the survey if the status is Pending Amendment
        // GET: RnP/Survey/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetForView?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

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

            var vmcontents = new UpdateSurveyContentsModel
            {
                ID = survey.ID,
                Contents = survey.Contents
            };

            var vmautofields = new ReturnSurveyAutofieldsModel
            {
                ID = survey.ID,
                DateAdded = survey.DateAdded,
                CreatorId = survey.CreatorId,
                RefNo = survey.RefNo,
                Status = survey.Status,
                DateCancelled = survey.DateCancelled,
                InviteCount = survey.InviteCount,
                SubmitCount = survey.SubmitCount,
                DmsPath = survey.DmsPath
            };

            var vmcancellation = new UpdateSurveyCancellationModel
            {
                ID = survey.ID,
                CancelRemark = survey.CancelRemark
            };

            var vmview = new UpdateSurveyViewModel
            {
                Survey = vmsurvey,
                Contents = vmcontents,
                Auto = vmautofields,
                Cancellation = vmcancellation
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<SurveyApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Survey/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            return View(vmview);
        }

        // Show delete form (only from list page)
        // GET: RnP/Survey/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resSurvey.Data;

            if (survey == null)
            {
                return HttpNotFound();
            }

            return View(survey);
        }

        // Process delete form
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"RnP/Survey/Delete?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Delete Survey: " + response.Data);

                TempData["SuccessMessage"] = "Survey titled " + response.Data + " successfully deleted.";

                // dashboard

                //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Survey", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Survey.";

                return RedirectToAction("Details", "Survey", new { area = "RnP", @id = id });
            }
        }

        // Process deletion from review page (confirmation by prompt)
        // GET: RnP/Survey/Discard/5
        public async Task<ActionResult> Discard(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Delete, $"RnP/Survey/Delete?id={id}");

            if (response.isSuccess)
            {
                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                LogActivity("Delete Survey: " + response.Data);

                TempData["SuccessMessage"] = "Survey titled " + response.Data + " successfully deleted.";

                // dashboard

                //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                // sms

                return RedirectToAction("Index", "Survey", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to delete Survey.";

                return RedirectToAction("Review", "Survey", new { area = "RnP", @id = id });
            }
        }

        // Process cancel form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(UpdateSurveyCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Cancel", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    LogActivity("Cancel Survey: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey titled " + response.Data + " successfully cancelled.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to cancel Survey.";

                    return RedirectToAction("Details", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Show verifier/approver evaluation form
        // From here user can do the following:
        // 1. Verifier/Approver can approve and submit for next approval (if applicable) if status is applicable
        // 2. Verifier/Approver can reject and require amendment if status is applicable
        // GET: RnP/Survey/Evaluate/5
        public async Task<ActionResult> Evaluate(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyApprovalModel>(HttpVerbs.Get, $"RnP/Survey/GetForEvaluation?id={id}");

            if (!resSurvey.isSuccess)
            {
                return HttpNotFound();
            }

            var surveyapproval = resSurvey.Data;

            if (surveyapproval == null)
            {
                return HttpNotFound();
            }

            var survey = new ReturnSurveyModel
            {
                ID = surveyapproval.Survey.ID,                
                Type = surveyapproval.Survey.Type,
                Category = surveyapproval.Survey.Category,
                Title = surveyapproval.Survey.Title,
                Description = surveyapproval.Survey.Description,
                TargetGroup = surveyapproval.Survey.TargetGroup,
                StartDate = surveyapproval.Survey.StartDate,
                EndDate = surveyapproval.Survey.EndDate,
                Contents = surveyapproval.Survey.Contents,
                Pictures = surveyapproval.Survey.Pictures,
                ProofOfApproval = surveyapproval.Survey.ProofOfApproval,
                DateAdded = surveyapproval.Survey.DateAdded,
                CreatorId = surveyapproval.Survey.CreatorId,
                RefNo = surveyapproval.Survey.RefNo,
                Status = surveyapproval.Survey.Status,
                DmsPath = surveyapproval.Survey.DmsPath
            };

            var sapproval = new ReturnUpdateSurveyApprovalModel
            {
                ID = surveyapproval.Approval.ID,
                SurveyID = surveyapproval.Approval.SurveyID,
                Level = surveyapproval.Approval.Level,
                ApproverId = surveyapproval.Approval.ApproverId,
                Status = surveyapproval.Approval.Status,
                Remarks = surveyapproval.Approval.Remarks,
                RequireNext = surveyapproval.Approval.RequireNext
            };

            var sevaluation = new ReturnSurveyApprovalModel
            {
                Survey = survey,
                Approval = sapproval
            };

            var resHis = await WepApiMethod.SendApiAsync<IEnumerable<SurveyApprovalHistoryModel>>(HttpVerbs.Get, $"RnP/Survey/GetHistory?id={id}");

            if (resHis.isSuccess)
            {
                ViewBag.History = resHis.Data;
            }

            return View(sevaluation);
        }

        // Process evaluation form
        // User (verifier/approver) is redirected to Index afterwards because their "work is done"
        // POST: Survey/Evaluate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Evaluate(ReturnSurveyApprovalModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Evaluate", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    if (model.Approval.Status == SurveyApprovalStatus.Approved)
                    {
                        if (model.Approval.Level == SurveyApprovalLevels.Verifier)
                        {
                            LogActivity("Verify Survey: " + response.Data, model);
                            TempData["SuccessMessage"] = "Survey titled " + response.Data + " updated as Verified.";
                            //SendEmail("Survey Approved", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                            // sms
                        }
                        else
                        {
                            LogActivity("Approve Survey: " + response.Data, model);
                            TempData["SuccessMessage"] = "Survey titled " + response.Data + " updated as Approved.";
                            //SendEmail("Survey Approved", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                            // sms
                        }
                    }
                    else
                    {
                        LogActivity("Survey Requires Amendment: " + response.Data, model);
                        TempData["SuccessMessage"] = "Survey titled " + response.Data + " updated as Requires Amendment.";
                        //SendEmail("Survey Approved", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });
                        // sms
                    }

                    // dashboard

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to process Survey.";

                    return RedirectToAction("Evaluate", "Survey", new { area = "RnP", @id = model.Survey.ID });
                }
            }

            return View(model);
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

