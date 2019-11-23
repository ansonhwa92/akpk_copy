using FEP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace FEP.WebApi.Api.Systems
{
    [Route("api/System/File")]
    public class FileDocumentController : ApiController
    {
        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromUri]int userId, [FromUri]string directory = "", [FromUri]string fileType = "", [FromUri]string fileTag = "")
        {

            HttpRequestMessage request = this.Request;

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (!db.User.Any(u => u.Id == userId))
            {
                return BadRequest("User not exist");
            }

            string root = GetFilePath();

            if (string.IsNullOrEmpty(root))
            {
                root = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            }

            if (directory == null)
                directory = "";

            Directory.CreateDirectory(Path.Combine(root, directory));

            var provider = new MultipartFormDataStreamProvider(root);

            await request.Content.ReadAsMultipartAsync(provider);

            var filedocs = new List<FileDocument>();

            foreach (var file in provider.FileData)
            {
                var path = file.LocalFileName;
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');

                var filepath = Path.Combine(directory, DateTime.Now.ToString("yyyyMMddHHmmss_") + filename);

                byte[] content = File.ReadAllBytes(path);

                File.Move(path, Path.Combine(root, filepath));

                var filedoc = new FileDocument
                {
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    FileName = filename,
                    FilePath = filepath,
                    Directory = directory,
                    FileSize = content.Length,
                    FileType = fileType,
                    FileTag = fileTag,
                    Display = true
                };

                filedocs.Add(filedoc);
            }

            if (filedocs.Count > 0)
            {
                db.FileDocument.AddRange(filedocs);
                db.SaveChanges();
            }

            return Ok(filedocs);

        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var filedoc = db.FileDocument.Where(f => f.Id == id && f.Display).FirstOrDefault();

            if (filedoc != null)
            {
                return Ok(filedoc);
            }

            return NotFound();

        }

        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]int? userId = null, [FromUri]string directory = "", [FromUri]string fileType = "", [FromUri]string fileTag = "")
        {
            var filedocs = db.FileDocument.Where(f => f.Display
                && (userId == null || f.CreatedBy == userId)
                && (directory == "" || f.Directory == directory)
                && (fileType == "" || f.FileType == fileType)
                && (fileTag == "" || f.FileTag == fileTag)).ToList();

            return Ok(filedocs);

        }

        [HttpGet]
        [Route("api/System/File/Download")]
        public HttpResponseMessage Download(int id)
        {

            var filedoc = db.FileDocument.Where(f => f.Id == id && f.Display).FirstOrDefault();

            if (filedoc == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            string root = GetFilePath();

            if (string.IsNullOrEmpty(root))
            {
                root = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            }

            //Set the File Path.
            string filePath = Path.Combine(root, filedoc.FilePath);

            //Check whether File exists.
            if (!File.Exists(filePath))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filePath);

            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            { 
                FileName = filedoc.FileName                
            };
           
            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filedoc.FileName));

            return response;
        }

        [HttpGet]
        [Route("api/System/File/Stream")]
        public HttpResponseMessage Stream(int id)
        {

            var filedoc = db.FileDocument.Where(f => f.Id == id && f.Display).FirstOrDefault();

            if (filedoc == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            string root = GetFilePath();

            if (string.IsNullOrEmpty(root))
            {
                root = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            }

            //Set the File Path.
            string filePath = Path.Combine(root, filedoc.FilePath);

            //Check whether File exists.
            if (!File.Exists(filePath))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filePath);

            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = filedoc.FileName;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filedoc.FileName));
            return response;
        }

        [NonAction]
        private string GetFilePath()
        {
            return WebConfigurationManager.AppSettings["FilePath"] != null ? WebConfigurationManager.AppSettings["FilePath"].ToString() : "";
        }

    }
}
