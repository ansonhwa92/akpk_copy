using FEP.Intranet.Helper;
using FEP.Model;
using Mammoth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class FileApiUrl
    {
        public const string UploadInfo = "eLearning/File/UploadInfo";

        public const string Upload = "eLearning/File";

        public const string GetFileNameOnStorage = "elearning/File/GetFileNameOnStorage";
    }

    public class FileController : Controller
    {
        public string storageDir = "";

        public FileController()
        {
            storageDir = AppSettings.FileDocPath;
        }

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            if (file == null)
                return Json("error", "text/html");

            var fileName = System.IO.Path.GetFileName(file.FileName);
            var fileId = DateTime.Now.Ticks.ToString() + "_" + fileName;

            object result = null;

            try
            {
                var length = file.ContentLength;
                var bytes = new byte[length];
                file.InputStream.Read(bytes, 0, length);

                // Check location to save
                if (!Directory.Exists(storageDir))
                {
                    Directory.CreateDirectory(storageDir);
                }

                // save the file.
                var fileStream = new FileStream(Path.Combine(storageDir, fileId), FileMode.Create, FileAccess.ReadWrite);
                fileStream.Write(bytes, 0, length);
                fileStream.Close();

                result = new
                {
                    FileName = fileName,
                    FilePath = Path.Combine(storageDir, fileId),
                    FileSize = file.ContentLength,
                    FileNameOnStorage = fileId
                };

                result = JsonConvert.SerializeObject(result);
            }
            catch (System.IO.IOException)
            {
                // If file already exist, just ignore this exception.
            }
            catch (Exception ex)
            {
                //Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                result = null;
            }

            return Json(result, "text/html");
        }

        public FileResult Download(string fileNameOnStorage, string fileNameFromDb)
        {
            var fileFullPath = Path.Combine(storageDir, fileNameOnStorage);

            return File(fileFullPath, MimeMapping.GetMimeMapping(fileNameFromDb), fileNameFromDb);
        }

        public ActionResult GetImg(string fileName)
        {
            var path = Path.Combine(storageDir, fileName);

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "image/png");
        }

        public string DocToHTML(string fileType, string courseId, string fileName)
        {
            var fileFullPath = Path.Combine(storageDir, fileName);

            try
            {
                var converter = new DocumentConverter();
                var result = converter.ConvertToHtml(fileFullPath);
                var html = result.Value; // The generated HTML
                var warnings = result.Warnings;
                
                if(String.IsNullOrEmpty(html))
                    return "Error reading the document.";

                return html;
            }
            catch(Exception e)
            {
                return "Error reading the document.";
            }
        }


        // For ajax call getting the document based on contentid
        public async Task<string> GetDoc(string contentId)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Get,
                        FileApiUrl.GetFileNameOnStorage + $"?contentId={contentId}");

            if (response.isSuccess)
            {
                return DocToHTML("", "", response.Data.ToString());

            }
            else
            {
                return "Error reading the document.";

            }
        }


        // FOR API CALL
        public async Task<WebApiResponse<T>> UploadToApi<T>(HttpPostedFileBase file)
        {
            var res = new WebApiResponse<T>();

            T result;
            var url = GetWebApiUrl();

            try
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] Bytes = new byte[file.InputStream.Length + 1];
                        file.InputStream.Read(Bytes, 0, Bytes.Length);
                        var fileContent = new ByteArrayContent(Bytes);
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        { FileName = file.FileName };

                        content.Add(fileContent);
                        var requestUri = $"{url}{FileApiUrl.Upload}";

                        var response = await client.PostAsync(requestUri, content);

                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsAsync<T>();

                            res.isSuccess = true;
                            res.ErrorMessage = "";
                            res.Data = result;
                        }
                        else
                        {
                            var str = await response.Content.ReadAsStringAsync();
                            res.isSuccess = false;
                            res.Data = default(T);
                            res.ErrorMessage = str;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.isSuccess = false;
                res.Data = default(T);
                res.ErrorMessage = ex.Message;
            }

            return res;
            
        }

        private static string GetWebApiUrl()
        {
            return WebConfigurationManager.AppSettings["WebApiURL"] != null ? WebConfigurationManager.AppSettings["WebApiURL"].ToString() : "";
        }
    }


}