using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class ContentCompletionsApiUrl
    {
        public const string Get = "eLearning/ContentCompletions/";
        public const string Post = "eLearning/ContentCompletions";
    }

    public class ContentCompletionsController : FEPController
    {
        private DbEntities db = new DbEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Post(ContentCompletionModel model)
        {
            bool CanViewAsLearner = false;

            if (CurrentUser.HasAccess(UserAccess.CourseCreate) || CurrentUser.HasAccess(UserAccess.CourseEdit) ||
                CurrentUser.HasAccess(UserAccess.CourseVerify) || CurrentUser.HasAccess(UserAccess.CourseApproval1) ||
                CurrentUser.HasAccess(UserAccess.CourseApproval2) || CurrentUser.HasAccess(UserAccess.CourseApproval3) ||
                CurrentUser.HasAccess(UserAccess.CourseDiscussionCreate))
            {
                CanViewAsLearner = true;
            }

            if (ModelState.IsValid)
            {
                model.UserId = CurrentUser.UserId.Value;

                var response = await WepApiMethod.SendApiAsync<ContentCompletionModel>(HttpVerbs.Post, ContentCompletionsApiUrl.Post + $"?CanViewAsLearner={CanViewAsLearner}", model);

                if (response.isSuccess)
                {
                    //TempData["SuccessMessage"] = "Content Completed";

                    await LogActivity(Modules.Learning, $"User : {CurrentUser?.Name} complete this content : {model.Title}");

                    var nextContent = response.Data.nextContentId;
                    var nextModule = response.Data.nextModuleId;
                    var courseId = response.Data.CourseId;

                    //if (nextContent < 0) // go to index, no more content this module
                    if (nextContent == null)
                    {
                        if (nextModule == null) // No more module and content, lets go to the course page
                        {
                            TempData["SuccessMessage"] = "Congratulations, you have completed this course.";

                            return RedirectToAction("Content", "Courses", new { area = "eLearning", @id = courseId });
                        }
                        else  // go to next module
                        {
                            return RedirectToAction("View", "CourseModules", new { area = "eLearning", @id = nextModule.Value });
                        }
                    }
                    else // go to next content
                        return RedirectToAction("View", "CourseContents", new { area = "eLearning", @id = nextContent.Value });
                }
            }
            TempData["ErrorMessage"] = "Cannot complete content.";

            return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.CourseModuleId });
        }

        [ChildActionOnly]
        public ActionResult Get(int contentId)
        {
            int currentUserId = -1;
            if (CurrentUser.UserId != null)
                currentUserId = CurrentUser.UserId.Value;

            var response = AsyncHelpers.RunSync<WebApiResponse<ContentCompletionModel>>(() => WepApiMethod.SendApiAsync<ContentCompletionModel>(HttpVerbs.Get,
                ContentCompletionsApiUrl.Get + $"?contentId={contentId}&userId={currentUserId}"));

            // response.Data.UserId = currentUserId;

            if (response.isSuccess)
            {
                return PartialView("_contentCompletionView", response.Data);
            }

            return null;
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