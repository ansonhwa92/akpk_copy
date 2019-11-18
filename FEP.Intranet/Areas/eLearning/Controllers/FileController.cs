using FEP.Intranet.Helper;
using Mammoth;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public const string EditorUpload = "eLearning/File";

        public const string GetFileNameOnStorage = "elearning/File/GetFileNameOnStorage";
        public const string GetFileFromApi = "elearning/File/GetFileFromApi";
        public const string GetHTML = "elearning/File/GetHTML";
    }

    [AllowAnonymous]
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

        [HttpPost]
        public async Task<JsonResult> EditorUpload(HttpPostedFileBase file)
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

                string ext = Path.GetExtension(file.FileName).ToLower();

                // for video, audio send them to api
                if (ext == ".mp4" || ext == ".ogg" || ext == ".mp3" || ext == ".wav")
                {
                    //result = "/Media/GetLiveMedia?fileName=" + fileId;
                    result = "/File/GetVideo?fileName=" + fileId;
                }
                else if(ext == ".pdf")
                {
                    result = "/File/GetPdf?fileName=" + fileId;
                }
                else
                {
                    result = "/File/GetImg?fileName=" + fileId;
                }
                
            }
            catch (System.IO.IOException)
            {
                // If file already exist, just ignore this exception.
            }
            catch (Exception ex)
            {
                result = null;
            }

            return Json(new { location = result });
        }

        public ActionResult GetVideo(string fileName)
        {
            var videoPath = Path.Combine(storageDir, fileName);
            FileStream fs = new FileStream(videoPath, FileMode.Open);

            return new FileStreamResult(fs, "video/mp4");
        }
        public ActionResult GetImg(string fileName)
        {
            var path = Path.Combine(storageDir, fileName);

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "image/png");
        }
        public ActionResult GetPdf(string fileName)
        {
            var path = Path.Combine(storageDir, fileName);

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/pdf");
        }
        public FileResult Download(string fileNameOnStorage, string fileNameFromDb)
        {
            var fileFullPath = Path.Combine(storageDir, fileNameOnStorage);

            return File(fileFullPath, MimeMapping.GetMimeMapping(fileNameFromDb), fileNameFromDb);
        }



        // For ajax call getting the document based on contentid
        // called when the select change
        public async Task<string> GetDoc(string contentId)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Get,
                        FileApiUrl.GetHTML + $"?contentId={contentId}");

            if (response.isSuccess)
            {
                return response.Data.ToString();
            }
            else
            {
                return "Error reading the document.";
            }
        }

        // ONLY TO BE USED FOR CONVERSION  WHEN FILE FIRST TIME UPLOADED
        // THE FILE WILL ALSO BE SENT TO API ONCE SAVED AND AVAILABLE FOR FUTURE USE
        public string DocToHTML(string fileType, string courseId, string fileName)
        {
            var fileFullPath = Path.Combine(storageDir, fileName);

            try
            {
                var converter = new DocumentConverter();
                var result = converter.ConvertToHtml(fileFullPath);
                var html = result.Value; // The generated HTML
                var warnings = result.Warnings;

                if (String.IsNullOrEmpty(html))
                    return "Error reading the document.";

                return html;
            }
            catch (Exception e)
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

        public static string Get(string uri)
        {
            string results = "N/A";

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                results = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            return results;
        }


        //public void GetMedia()
        //{
        //    //Create a stream for the file
        //    Stream stream = null;

        //    //This controls how many bytes to read at a time and send to the client
        //    int bytesToRead = 10000;

        //    // Buffer to read bytes in chunk size specified above
        //    byte[] buffer = new Byte[bytesToRead];

        //    // The number of bytes read
        //    try
        //    {
        //        //Create a WebRequest to get the file
        //        HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

        //        //Create a response for this request
        //        HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

        //        if (fileReq.ContentLength > 0)
        //            fileResp.ContentLength = fileReq.ContentLength;

        //        //Get the Stream returned from the response
        //        stream = fileResp.GetResponseStream();

        //        // prepare the response to the client. resp is the client Response
        //        var resp = System.Web.HttpContext.Current.Response;

        //        //Indicate the type of data being sent
        //        resp.ContentType = "application/octet-stream";

        //        //Name the file 
        //        resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
        //        resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

        //        int length;
        //        do
        //        {
        //            // Verify that the client is connected.
        //            if (resp.IsClientConnected)
        //            {
        //                // Read data into the buffer.
        //                length = stream.Read(buffer, 0, bytesToRead);

        //                // and write it out to the response's output stream
        //                resp.OutputStream.Write(buffer, 0, length);

        //                // Flush the data
        //                resp.Flush();

        //                //Clear the buffer
        //                buffer = new Byte[bytesToRead];
        //            }
        //            else
        //            {
        //                // cancel the download if client has disconnected
        //                length = -1;
        //            }
        //        } while (length > 0); //Repeat until no data is read
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //        {
        //            //Close the input stream
        //            stream.Close();
        //        }
        //    }
        //}
        //// EDITOR UPLOAD MEDIA VIDEO AND AUDIO TO API
        //public async Task<WebApiResponse<T>> EditorUploadToApi<T>(HttpPostedFileBase file)
        //{
        //    int MAX_CHUNK_SIZE = (1024 * 5000);

        //    HttpWebRequest webRequest = null;
        //    FileStream fileReader = null;
        //    Stream requestStream = null;

        //    byte[] fileData;

        //    fileReader = new FileStream(file, FileMode.Open, FileAccess.Read);

        //    webRequest = (HttpWebRequest)WebRequest.Create(uri);

        //    webRequest.Method = "POST";
        //    webRequest.ContentLength = fileReader.Length;
        //    webRequest.Timeout = 600000;
        //    webRequest.Credentials = CredentialCache.DefaultCredentials;
        //    webRequest.AllowWriteStreamBuffering = false;
        //    requestStream = webRequest.GetRequestStream();

        //    long fileSize = fileReader.Length;
        //    long remainingBytes = fileSize;
        //    int numberOfBytesRead = 0, done = 0;

        //    while (numberOfBytesRead < fileSize)
        //    {
        //        SetByteArray(out fileData, remainingBytes);

        //        done = WriteFileToStream(fileData, requestStream);

        //        numberOfBytesRead += done;
        //        remainingBytes -= done;
        //    }

        //    fileReader.Close();
        //    return true;
        //}

        //public int WriteFileToStream(byte[] fileData, Stream requestStream)
        //{
        //    int done = fileReader.Read(fileData, 0, fileData.Length);
        //    requestStream.Write(fileData, 0, fileData.Length);

        //    return done;
        //}

        //private void SetByteArray(out byte[] fileData, long bytesLeft)
        //{
        //    fileData = bytesLeft < MAX_CHUNK_SIZE ? new byte[bytesLeft] : new byte[MAX_CHUNK_SIZE];
        //}

        private static string GetWebApiUrl()
        {
            return WebConfigurationManager.AppSettings["WebApiURL"] != null ? WebConfigurationManager.AppSettings["WebApiURL"].ToString() : "";
        }
    }
}