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

        public ActionResult _View()
        {
            //var response = await WepApiMethod.SendApiAsync<List<CourseDiscussionModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion");

            //if (response.isSuccess)
            //    return View(response.Data);

            return View();
        }
    }
}