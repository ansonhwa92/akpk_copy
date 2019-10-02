using FEP.Intranet.Helper;
using FEP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
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



        //[HttpPost]
        //public JsonResult UploadFile(HttpPostedFileBase file)
        //{

        //    if (file == null)
        //        return Json("error", "text/html");


        //    var length = Request.ContentLength;
        //    var bytes = new byte[length];
        //    Request.InputStream.Read(bytes, 0, length);

        //    var fileName = Request.Headers["X-File-Name"];
        //    var fileSize = Request.Headers["X-File-Size"];
        //    var fileType = Request.Headers["X-File-Type"];

        //    var saveToFileLoc = "\\\\adcyngctg\\HRMS\\Images\\" + fileName;

        //    // save the file.
        //    var fileStream = new FileStream(saveToFileLoc, FileMode.Create, FileAccess.ReadWrite);
        //    fileStream.Write(bytes, 0, length);
        //    fileStream.Close();

        //    return string.Format("{0} bytes uploaded", bytes.Length);
        //}
    }
}