using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/FileUpload")]
    public class FileUploadController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        public string storageDir = "";

        public FileUploadController()
        {
            //storageDir = AppSettings.FileDocPath;
            storageDir = WebConfigurationManager.AppSettings["FilePath"] != null ?
                            WebConfigurationManager.AppSettings["FilePath"].ToString() : "D://FEPDoc";
        }

        /// <summary>
        /// For upload
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/FileUpload/")]
        [HttpGet]
        public HttpResponseMessage Get(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string fullPath = Path.Combine(storageDir, fileName);


                if (File.Exists(fullPath))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    var fileStream = new FileStream(fullPath, FileMode.Open);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileName;

                    return response;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// For upload
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/FileUpload/")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var streamProvider = new MultipartFormDataStreamProvider(storageDir);
            var fileReadToProvider = await Request.Content.ReadAsMultipartAsync(streamProvider);

            List<FileUploadModel> result = new List<FileUploadModel>();

            foreach (MultipartFileData fileData in streamProvider.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return BadRequest("This request is not properly formatted");
                }

                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fileLocalName = DateTime.Now.Ticks.ToString() + "_" + fileName;

                // Check location to save
                if (!Directory.Exists(storageDir))
                {
                    Directory.CreateDirectory(storageDir);
                }

                File.Move(fileData.LocalFileName, Path.Combine(storageDir, fileLocalName));

                result.Add(new FileUploadModel
                {
                    FileName = fileName,
                    FilePath = Path.Combine(storageDir, fileLocalName),
                    FileNameOnStorage = fileLocalName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = -1,
                });
            }

            return Ok(result);
        }

        [Route("api/eLearning/FileUpload/FileUploadInfo")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> FileUploadInfo([FromBody] FileUploadModel request)
        {
            if (ModelState.IsValid)
            {
                var fileDocument = new FileUpload
                {
                    CreatedBy = request.CreatedBy,
                    CreatedDate = request.CreatedDate,
                    FileName = request.FileName,
                    FileNameOnStorage = request.FileNameOnStorage,
                    FilePath = request.FilePath,
                    FileSize = request.FileSize,
                    FileTag = request.FileTag,
                    //FileType = request.FileType,
                    User = request.User
                };

                db.FileUploads.Add(fileDocument);

                await db.SaveChangesAsync();

                return Ok(fileDocument.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
