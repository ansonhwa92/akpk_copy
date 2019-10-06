using FEP.Helper;
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
        public const string Post = "eLearning/ContentCompletions/";
    }

    public class ContentCompletionsController : FEPController
    {
        private DbEntities db = new DbEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Post(ContentCompletionModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<ContentCompletionModel>(HttpVerbs.Post, ContentCompletionsApiUrl.Post, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Content Completed";

                    await LogActivity(Modules.Learning, "User : " + user + " complete this content : " + model.Title);

                    //int nextContent = int.Parse(response.Data.ToString());
                    var nextContent = response.Data;

                    //if (nextContent < 0) // go to index, no more content this module
                    if (nextContent == null)
                        return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.CourseModuleId });
                    else
                        return RedirectToAction("View", "CourseContents", new { area = "eLearning", @id = nextContent.Id });
                }
            }
            TempData["ErrorMessage"] = "Cannot complete content.";

            return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.CourseModuleId });
        }

        [HttpGet]
        public ActionResult Get(int contentId)
        {
            var response = Task.Run(() => WepApiMethod.SendApiAsync<ContentCompletionModel>(HttpVerbs.Get, ContentCompletionsApiUrl.Get + $"?contentId={contentId}").GetAwaiter().GetResult()).Result;

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