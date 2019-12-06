using FEP.Helper;
using FEP.Intranet.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseFeedbackController : FEPController
    {
        DbEntities db = new DbEntities();

        private string StorageRoot
        {
            //get { return Path.Combine(Server.MapPath("~/Attachments")); }
            // get { return AppSettings.FileDocPath + "DiscussionAttachment"; }
            get { return HostingEnvironment.MapPath(@"/App_Data/" + "DiscussionAttachment"); }
        }
        // GET: eLearning/CourseDiscussion

        private long RequestLimit
        {
            get
            {
                const long DefaultAllowedContentLengthBytes = 1073741824;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(System.Web.HttpRuntime.AppDomainAppPath + "/web.config"))
                {
                    System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    xmlDocument.LoadXml(reader.ReadToEnd());

                    if (xmlDocument.GetElementsByTagName("requestLimits").Count > 0)
                    {
                        var maxAllowedContentLength = xmlDocument.GetElementsByTagName("requestLimits")[0].Attributes.Cast<System.Xml.XmlAttribute>().FirstOrDefault(atributo => atributo.Name.Equals("maxAllowedContentLength"));
                        return Convert.ToInt64(maxAllowedContentLength.Value);
                    }
                    else
                        return DefaultAllowedContentLengthBytes;
                }
            }
        }

        //public ActionResult Detail()
        //{
        //    ViewBag.FeedbackId = 1;
        //    return View();
        //}

        [HttpGet]
        public async Task<ActionResult> _View(int id)
        {
            FeedbackModel model = new FeedbackModel();
            var GetThisFeedback = await WepApiMethod.SendApiAsync<FeedbackModel>(HttpVerbs.Get, $"eLearning/Feedback/GetFeedback?id={id}");

            if (GetThisFeedback.isSuccess)
            {
                ViewBag.FeedbackId = id;
                model = GetThisFeedback.Data;
                model.Visibilities = new SelectList(await GetVisibility(), "Id", "Type", 1);
                return PartialView(model);

            }
            return PartialView(model);
        }

        [NonAction]
        private async Task<IEnumerable<FeedbackVisibility>> GetVisibility()
        {
            var sectors = Enumerable.Empty<FeedbackVisibility>();
            var response = await WepApiMethod.SendApiAsync<List<FeedbackVisibility>>(HttpVerbs.Get, $"eLearning/Feedback/GetVisibility");

            if (response.isSuccess)
            {
                sectors = response.Data;
            }
            return sectors;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _View(FeedbackModel model)
        {
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    if (model.NewPost != null)
                    {
                        var response = await WepApiMethod.SendApiAsync<FeedbackModel>(HttpVerbs.Post, $"eLearning/Feedback/PostNew", model);
                        if (response.isSuccess)
                        {

                            model = response.Data;
                            model.Visibilities = new SelectList(await GetVisibility(), "Id", "Type", 1);
                            //IEnumerable<SelectListItem> getget = new SelectList(await GetVisibility(), "Id", "Type", 1);
                            //model.Visibilities = new List<SelectListItem>();
                            return PartialView(model);
                            //return RedirectToAction("_View", "CourseFeedback", new { area = "eLearning", @id = model.id });
                        }
                    }
                }
            }

            TempData["ErrorMessage"] = "Cannot add content.";

            // await GetAllQuestions(model.CourseId);

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> _DeletePost(int? id, int? Uid)
        {
            FeedbackModel model = new FeedbackModel();
            if (id != null && Uid != null)
            {
                var response = await WepApiMethod.SendApiAsync<FeedbackModel>(HttpVerbs.Get, $"eLearning/Feedback/DeletePost?id={id.Value}&UpdateId={Uid.Value}");
                if (response.isSuccess)
                {

                    model = response.Data;
                    TempData["SuccessMessage"] = "Success delete feedback post";
                    return Json(1 , JsonRequestBehavior.AllowGet);
                    //IEnumerable<SelectListItem> getget = new SelectList(await GetVisibility(), "Id", "Type", 1);
                    //model.Visibilities = new List<SelectListItem>();
                    //return PartialView(model);
                   // return RedirectToAction("_View", "CourseFeedback", new { area = "eLearning", @id = model.id });
                }
            }
            TempData["ErrorMessage"] = "Cannot delete  feedback post";

            // await GetAllQuestions(model.CourseId);
            return Json(0, JsonRequestBehavior.AllowGet);
            //return PartialView(model);
        }
    }
}