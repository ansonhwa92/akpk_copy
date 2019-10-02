﻿using System;
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
using FEP.WebApiModel.SLAReminder;
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
        [HasAccess(UserAccess.RnPSurveyView)]
        public ActionResult Index()
        {
            return View();
        }

        // Show select survey type form
        // After type selection, user automatically redirected to creation page
        // GET: RnP/Survey/SelectCategory
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public ActionResult SelectType()
        {
            //ViewBag.Categories = new List<SurveyType>();
            ViewBag.SurveyTypes = new SurveyType();
            return View();
        }

        // Show create form (blank form so no api call needed)
        // GET: RnP/Survey/Create
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> Create(int? typeid)
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

            var response = await WepApiMethod.SendApiAsync<List<UpdateSurveyTemplateModel>>(HttpVerbs.Get, $"RnP/Survey/GetTemplates");

            if (response.isSuccess)
            {
                ViewBag.Templates = response.Data;
            }
            else
            {
                ViewBag.Templates = null;
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
        public async Task<ActionResult> Create(UpdateSurveyModel model, int TemplateSelection)
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

                    await LogActivity(Modules.RnP, "Create New Survey: " + title);

                    TempData["SuccessMessage"] = "New Survey titled " + title + " created successfully and saved as draft.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    return RedirectToAction("Build", "Survey", new { area = "RnP", @id = newid, @templateid = TemplateSelection });
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
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetSingle?id={id}");

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
                RequireLogin = survey.RequireLogin,
                Pictures = survey.Pictures,
                ProofOfApproval = survey.ProofOfApproval,
                CreatorId = survey.CreatorId
            };

            if (survey.Type == 0)
            {
                ViewBag.TypeName = "Public Mass";
            }
            else
            {
                ViewBag.TypeName = "Targeted Groups";
            }

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

                    await LogActivity(Modules.RnP, "Edit Survey: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey titled " + response.Data + " updated successfully and saved as draft.";

                    // dashboard

                    //SendEmail("New Survey Created", "A new Survey has been created." + "\n" + "Please etc.", new EmailAddress() { Address = model.Email, DisplayName = model.Name });

                    // sms

                    // NOTE: when editing, user never gets the select template option anymore (TBD more)

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
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> Build(int? id, int? templateid)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetSingle?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var survey = resPub.Data;
            if (survey == null)
            {
                return HttpNotFound();
            }

            string actualcontents;

            if ((templateid != null) && (templateid != 0))
            {
                var resptemp = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Get, $"RnP/Survey/GetTemplate?id={templateid}");

                if (resptemp.isSuccess)
                {
                    if (resptemp.Data != null)
                    {
                        actualcontents = resptemp.Data;
                    }
                    else
                    {
                        actualcontents = survey.Contents;
                    }
                }
                else
                {
                    actualcontents = survey.Contents;
                }
            } else
            {
                actualcontents = survey.Contents;
            }

            var vmcontents = new UpdateSurveyContentsModel
            {
                ID = survey.ID,
                Contents = actualcontents
            };

            return View(vmcontents);
        }

        // Process build form
        // After editing build, user automatically redirected to review page if successful, and details page if failed.
        // This is only when user clicks submit! (for review)
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

                    await LogActivity(Modules.RnP, "Build Survey: " + response.Data, model);

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

        // Process save template form
        // After editing build, user can save it as template (this is called via ajax).
        // POST: Survey/SaveTemplate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTemplate(UpdateSurveyTemplateModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SaveTemplate", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Modules.RnP, "Survey design saved as Template named: " + response.Data, model);

                    TempData["SuccessMessage"] = "Survey design saved successfully as a Template named: " + response.Data + ".";

                    // dashboard
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to save Survey design as template.";
                }
            }

            return View(model);
        }

        // Show review form
        // User is redirected here after saving as draft at builder creation or builder editing page
        // GET: RnP/Survey/Review/5
        [HasAccess(UserAccess.RnPSurveyEdit)]
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
                RequireLogin = survey.RequireLogin,
                Pictures = survey.Pictures,
                ProofOfApproval = survey.ProofOfApproval,
                CreatorId = survey.CreatorId
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
        public async Task<ActionResult> Review(UpdateSurveyReviewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Submit", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string type = resparray[1];
                    string refno = resparray[2];

                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Modules.RnP, "Submit Survey: " + title, model);

                    TempData["SuccessMessage"] = "Survey titled " + title + " submitted successfully for verification.";

                    await SendNotification(model.Survey.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Survey_Creation, title, type, refno, "Survey Submitted", SurveyApprovalStatus.None, false);

                    // dashboard

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit Survey.";

                    return RedirectToAction("Review", "Survey", new { area = "RnP", @id = model.Survey.ID });
                }
            }

            return View(model);
        }

        // Process submission from details page
        // Called for direct submission via id
        // GET: RnP/Survey/SubmitByID/5
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> SubmitByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SubmitByID?id={id}");

            if (response.isSuccess)
            {
                string[] resparray = response.Data.Split('|');
                string title = resparray[0];
                string type = resparray[1];
                string refno = resparray[2];

                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                await LogActivity(Modules.RnP, "Submit Survey: " + title);

                TempData["SuccessMessage"] = "Survey titled " + title + " submitted successfully for verification.";

                await SendNotification(id.Value, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Survey_Creation, title, type, refno, "Survey Submitted", SurveyApprovalStatus.None, false);

                // dashboard

                return RedirectToAction("Index", "Survey", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to submit Survey.";

                return RedirectToAction("Details", "Survey", new { area = "RnP", @id = id });
            }
        }

        // Process publishing from details page
        // Called for direct publishing via id
        // GET: RnP/Survey/PublishByID/5
        [HasAccess(UserAccess.RnPSurveyPublish)]
        public async Task<ActionResult> PublishByID(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/PublishByID?id={id}");

            if (response.isSuccess)
            {
                string[] resparray = response.Data.Split('|');
                string title = resparray[0];
                string type = resparray[1];
                string refno = resparray[2];

                // log trail/system success notification/dashboard notification/email/sms upon submission
                // log trail/system success/dashboard notification upon saving as draft

                await LogActivity(Modules.RnP, "Publish Survey: " + title);

                TempData["SuccessMessage"] = "Survey titled " + title + " published successfully.";

                await SendNotification(id.Value, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Survey_Publication, title, type, refno, "Survey Published", SurveyApprovalStatus.None, false);

                // dashboard

                return RedirectToAction("Index", "Survey", new { area = "RnP" });
            }
            else
            {
                TempData["SuccessMessage"] = "Failed to publish Survey.";

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
        [HasAccess(UserAccess.RnPSurveyView)]
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

            // redirect for eavluation if applicable

            if ((CurrentUser.HasAccess(UserAccess.RnPSurveyVerify)) || (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove1)) || (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove2)) || (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove3)))
            {
                // if approvers, check pending approval

                var resApp = await WepApiMethod.SendApiAsync<SurveyApprovalHistoryModel>(HttpVerbs.Get, $"RnP/Survey/GetNextApproval?id={id}");

                if (resApp.isSuccess)
                {
                    var nextapp = resApp.Data;

                    if (nextapp != null)
                    {
                        if (((nextapp.Level == SurveyApprovalLevels.Verifier) && (CurrentUser.HasAccess(UserAccess.RnPSurveyVerify))) || ((nextapp.Level == SurveyApprovalLevels.Approver1) && (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove1))) || ((nextapp.Level == SurveyApprovalLevels.Approver2) && (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove2))) || ((nextapp.Level == SurveyApprovalLevels.Approver3) && (CurrentUser.HasAccess(UserAccess.RnPSurveyApprove3))))
                        {
                            if ((survey.Status == SurveyStatus.Submitted) || (survey.Status == SurveyStatus.Verified))
                            {
                                return RedirectToAction("Evaluate", "Survey", new { area = "RnP", @id = id });
                            }
                        }
                    }
                }

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
                RequireLogin = survey.RequireLogin,
                Pictures = survey.Pictures,
                ProofOfApproval = survey.ProofOfApproval,
                CreatorId = survey.CreatorId,
                CreatorName = survey.CreatorName
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
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resSurvey = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetSingle?id={id}");

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

                await LogActivity(Modules.RnP, "Delete Survey: " + response.Data);

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
        [HasAccess(UserAccess.RnPSurveyEdit)]
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

                await LogActivity(Modules.RnP, "Delete Survey: " + response.Data);

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
        [HasAccess(UserAccess.RnPSurveyEdit)]
        public async Task<ActionResult> Cancel(UpdateSurveyCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/Cancel", model);

                if (response.isSuccess)
                {
                    string[] resparray = response.Data.Split('|');
                    string title = resparray[0];
                    string type = resparray[1];
                    string refno = resparray[2];

                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Modules.RnP, "Cancel Survey: " + title, model);

                    TempData["SuccessMessage"] = "Survey titled " + title + " successfully cancelled.";

                    await SendNotification(model.ID, NotificationCategory.ResearchAndPublication, NotificationType.Submit_Survey_Cancellation, title, type, refno, "Survey Cancelled", SurveyApprovalStatus.None, false);

                    // dashboard

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
        //[HasAccess(UserAccess.RnPSurveyEdit)]
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
                RequireLogin = surveyapproval.Survey.RequireLogin,
                Contents = surveyapproval.Survey.Contents,
                Pictures = surveyapproval.Survey.Pictures,
                ProofOfApproval = surveyapproval.Survey.ProofOfApproval,
                DateAdded = surveyapproval.Survey.DateAdded,
                CreatorId = surveyapproval.Survey.CreatorId,
                RefNo = surveyapproval.Survey.RefNo,
                Status = surveyapproval.Survey.Status,
                DmsPath = surveyapproval.Survey.DmsPath,
                CreatorName = surveyapproval.Survey.CreatorName
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
                    string[] resparray = response.Data.Split('|');
                    int sid = int.Parse(resparray[0]);
                    string title = resparray[1];
                    string type = resparray[2];
                    string refno = resparray[3];
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    if (model.Approval.Status == SurveyApprovalStatus.Approved)
                    {
                        if (model.Approval.Level == SurveyApprovalLevels.Verifier)
                        {
                            await LogActivity(Modules.RnP, "Verify Survey: " + title, model);
                            TempData["SuccessMessage"] = "Survey titled " + title + " updated as Verified.";

                            await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Survey_Creation, title, type, refno, "Verified and Pending Approval", model.Approval.Status, model.Approval.RequireNext);
                            // dashboard
                        }
                        else
                        {
                            await LogActivity(Modules.RnP, "Approve Survey: " + title, model);
                            TempData["SuccessMessage"] = "Survey titled " + title + " updated as Approved.";

                            if (model.Approval.Level == SurveyApprovalLevels.Approver1)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_1, title, type, refno, "Approved by 1st-Level Approver and Pending 2nd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_1, title, type, refno, "Approved by 1st-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == SurveyApprovalLevels.Approver2)
                            {
                                if (model.Approval.RequireNext)
                                {
                                    await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_2, title, type, refno, "Approved by 2nd-Level Approver and Pending 3rd-Level Approval", model.Approval.Status, model.Approval.RequireNext);
                                }
                                else
                                {
                                    await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_2, title, type, refno, "Approved by 2nd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                                }
                            }
                            else if (model.Approval.Level == SurveyApprovalLevels.Approver3)
                            {
                                await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_3, title, type, refno, "Approved by 3rd-Level Approver", model.Approval.Status, model.Approval.RequireNext);
                            }
                            // dashboard
                        }
                    }
                    else
                    {
                        await LogActivity(Modules.RnP, "Survey Requires Amendment: " + title, model);
                        TempData["SuccessMessage"] = "Survey titled " + title + " updated as Requires Amendment.";

                        if (model.Approval.Level == SurveyApprovalLevels.Verifier)
                        {
                            await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Verify_Survey_Creation, title, type, refno, "Amendment Requested by Verifier", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == SurveyApprovalLevels.Approver1)
                        {
                            await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_1, title, type, refno, "Amendment Requested by 1st-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == SurveyApprovalLevels.Approver2)
                        {
                            await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_2, title, type, refno, "Amendment Requested by 2nd-Level Approver", model.Approval.Status, false);
                        }
                        else if (model.Approval.Level == SurveyApprovalLevels.Approver3)
                        {
                            await SendNotification(sid, NotificationCategory.ResearchAndPublication, NotificationType.Approve_Survey_Creation_3, title, type, refno, "Amendment Requested by 3rd-Level Approver", model.Approval.Status, false);
                        }
                        // dashboard
                    }

                    return RedirectToAction("Index", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to process Survey.";

                    return RedirectToAction("Evaluate", "Survey", new { area = "RnP", @id = model.Approval.SurveyID });
                }
            }

            return View(model);
        }

        // Show tester form
        // Retrieve survey info based on id first
        // GET: RnP/Survey/Test/5
        public async Task<ActionResult> Test(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetSingle?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var surveyinfo = resPub.Data;
            if (surveyinfo == null)
            {
                return HttpNotFound();
            }

            if (surveyinfo.Type == 0)
            {
                ViewBag.TypeName = "Public Mass";
            }
            else
            {
                ViewBag.TypeName = "Targeted Groups";
            }

            var uid = 0;

            if (CurrentUser.UserId.HasValue)
            {
                uid = CurrentUser.UserId.Value;
            }

            var vmresp = new UpdateSurveyResponseModel
            {
                SurveyID = surveyinfo.ID,
                Type = SurveyResponseTypes.Testing,
                UserId = uid,
                Contents = ""
            };

            var vmtest = new ReturnSurveyResponseModel
            {
                Survey = surveyinfo,
                Response = vmresp
            };

            return View(vmtest);
        }

        // Process survey answers (testing) submission
        // Redirects to thank you page?
        // POST: Survey/SubmitAnswers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitTest(UpdateSurveyResponseModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SubmitTest", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Modules.RnP, "Test answers submitted", model);    // for Survey titled: " + response.Data, model);

                    TempData["SuccessMessage"] = "Test answers submitted successfully"; // for Survey titled: " + response.Data + ".";

                    // dashboard

                    return RedirectToAction("Thankyou", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit test answers for survey.";

                    return RedirectToAction("Test", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Show answer form
        // Retrieve survey info based on id first
        // GET: RnP/Survey/Answer/5
        public async Task<ActionResult> Answer(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnSurveyModel>(HttpVerbs.Get, $"RnP/Survey/GetSingle?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var surveyinfo = resPub.Data;
            if (surveyinfo == null)
            {
                return HttpNotFound();
            }

            if (surveyinfo.Type == 0)
            {
                ViewBag.TypeName = "Public Mass";
            }
            else
            {
                ViewBag.TypeName = "Targeted Groups";
            }

            var uid = 0;

            if (CurrentUser.UserId.HasValue)
            {
                uid = CurrentUser.UserId.Value;
            }

            var vmresp = new UpdateSurveyResponseModel
            {                
                SurveyID = surveyinfo.ID,
                Type = SurveyResponseTypes.Actual,
                UserId = uid,
                Contents = ""
            };

            var vmtest = new ReturnSurveyResponseModel
            {
                Survey = surveyinfo,
                Response = vmresp
            };

            return View(vmtest);
        }

        // Process survey answers (actual) submission
        // Redirects to thank you page?
        // POST: Survey/SubmitAnswers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitAnswers(UpdateSurveyResponseModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"RnP/Survey/SubmitAnswers", model);

                if (response.isSuccess)
                {
                    // log trail/system success notification/dashboard notification/email/sms upon submission
                    // log trail/system success/dashboard notification upon saving as draft

                    await LogActivity(Modules.RnP, "Answers submitted for Survey", model);      // titled: " + response.Data, model);

                    TempData["SuccessMessage"] = "Answers submitted successfully for Survey";   // titled: " + response.Data + ".";

                    // dashboard

                    return RedirectToAction("Thankyou", "Survey", new { area = "RnP" });
                }
                else
                {
                    TempData["SuccessMessage"] = "Failed to submit answers for survey.";

                    return RedirectToAction("Answer", "Survey", new { area = "RnP", @id = model.ID });
                }
            }

            return View(model);
        }

        // Private functions

        // get notification receiver IDs
        // called by SendNotification
        private async Task<List<int>> GetNotificationReceivers(NotificationCategory ncat, NotificationType ntype, SurveyApprovalStatus status, bool forward)
        {
            List<int> result = new List<int> { };
            var response = await WepApiMethod.SendApiAsync<List<int>>(HttpVerbs.Get, $"RnP/Survey/GetNotificationReceivers/?cat={ncat}&type={ntype}&status={status}&forward={forward}");
            if (response.isSuccess)
            {
                result = response.Data;
            }
            else
            {
                await LogError(Modules.RnP, "Failed to get Auto Notification receivers");
            }
            return result;
        }

        // save notification ID
        // called by SendNotification
        private async Task<bool> SaveNotificationID(int id, int notification_id)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"RnP/Survey/SaveNotificationID?id={id}&notificationid={notification_id}");
            if (!response.isSuccess)
            {
                await LogError(Modules.RnP, "Failed to save Auto Notification ID (API Error)");
            }
            else
            {
                if (response.Data == false)
                {
                    await LogError(Modules.RnP, "Failed to save Auto Notification ID (Survey Error)");
                }
            }
            return response.isSuccess;
        }

        // Send notifications
        private async Task<bool> SendNotification(int id, NotificationCategory ncat, NotificationType ntype, string title, string type, string code, string approvalmessage, SurveyApprovalStatus appstatus, bool forward)
        {
            try
            {
                var receivers = await GetNotificationReceivers(ncat, ntype, appstatus, forward);
                //Console.Write(receivers);
                if (receivers.Count > 0)
                {
                    ParameterListToSend paramToSend = new ParameterListToSend();
                    paramToSend.SurveyTitle = title;
                    paramToSend.SurveyType = type;
                    paramToSend.SurveyCode = code;
                    paramToSend.SurveyApproval = approvalmessage;

                    CreateAutoReminder reminder = new CreateAutoReminder
                    {
                        NotificationType = ntype,
                        NotificationCategory = ncat,
                        ParameterListToSend = paramToSend,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = receivers
                        // new List<int> { 2, 3, 4, 5 }
                    };
                    //Console.Write(reminder);
                    try
                    {
                        var response = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", reminder);
                        if (response.isSuccess)
                        {
                            int saveThisID = response.Data.SLAReminderStatusId;
                            //save saveThisID back into survey table
                            var ressave = await SaveNotificationID(id, saveThisID);
                            return true;
                        }
                        else
                        {
                            await LogError(Modules.RnP, "Failed to generate Auto Notification (API Call Returned Failure)");
                            return false;
                        }
                    }
                    catch
                    {
                        await LogError(Modules.RnP, "Failed to generate Auto Notification (API Call Failed)");
                        return false;
                    }
                }
                else
                {
                    await LogError(Modules.RnP, "Failed to generate Auto Notification (No Receivers Found)");
                    return false;
                }
            }
            catch
            {
                await LogError(Modules.RnP, "Failed to generate Auto Notification");
                return false;
            }
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

