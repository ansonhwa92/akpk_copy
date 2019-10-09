using FEP.Intranet.Helper;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class FileUploadApiUrl
    {
        public const string FileUploadInfo = "eLearning/FileUpload/FileUploadInfo";
        public const string UploadFile = "eLearning/FileUpload";
    }

    public class FileUploadController : Controller
    {
        public string storageDir = "";

        public FileUploadController()
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

        
        // FILE UPLOAD FOR IMAGE, VIDEO, AUDIO
        // FOR API FILE UPLOAD CALL
        public async Task<WebApiResponse<T>> FileUploadToApi<T>(HttpPostedFileBase file)
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
                        var requestUri = $"{url}{FileUploadApiUrl.UploadFile}";

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

        public ActionResult GetImg(string fileName)
        {
            var path = Path.Combine(storageDir, fileName);

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "image/png");
        }

        private static string GetWebApiUrl()
        {
            return WebConfigurationManager.AppSettings["WebApiURL"] != null ? WebConfigurationManager.AppSettings["WebApiURL"].ToString() : "";
        }
    }


}