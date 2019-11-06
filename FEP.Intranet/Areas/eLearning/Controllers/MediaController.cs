using FEP.Helper;
using FEP.Intranet.Helper;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{

    //https://mytechnetknowhows.wordpress.com/2017/05/18/asp-net-web-api-and-streaming-video-content/
    /// API Controller for streaming video files.
    public class MediaController : FEPController
    {
        public string storageDir = "";

        public MediaController()
        {
            storageDir = AppSettings.FileDocPath;
        }

        /// Gets the live video.
        [HttpGet]
        public ActionResult GetLiveMedia(string fileName)
        {
            //actual path
            string filePath = Path.Combine(storageDir, fileName);
            return new MediaFileActionResult(filePath);
        }

        /// Gets the live video using post.
        /// The request.
        public ActionResult GetLiveMediaPost(MediaFileDownloadRequest request)
        {
            string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(storageDir), request.FileName);
            return new MediaFileActionResult(filePath);
        }
    }

    public class MediaFileDownloadRequest
    {
        public string FileName { get; set;}
    }

    /// Action Result for Returning Stream
    public class MediaFileActionResult : ActionResult
    {
        private const long BufferLength = 65536;
        public MediaFileActionResult(string videoFilePath)
        {
            this.Filepath = videoFilePath;
        }

        public string Filepath { get; private set; }

         public override void ExecuteResult(ControllerContext context)
        {
            //The header information
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=Win8.mp4");

            var file = new FileInfo(this.Filepath);
            //Check the file exist,  it will be written into the response
            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }

            //HttpResponseMessage response = new HttpResponseMessage();
            //FileInfo fileInfo = new FileInfo(this.Filepath);
            //long totalLength = fileInfo.Length;
            //response.Content = new PushStreamContent((outputStream, httpContent, transportContext) =>
            //{
            //    OnStreamConnected(outputStream, httpContent, transportContext);
            //});

            //response.Content.Headers.ContentLength = totalLength;
            //return Task.FromResult(response);

        } 

        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    FileInfo fileInfo = new FileInfo(this.Filepath);
        //    long totalLength = fileInfo.Length;
        //    response.Content = new PushStreamContent((outputStream, httpContent, transportContext) =>
        //    {
        //        OnStreamConnected(outputStream, httpContent, transportContext);
        //    });

        //    response.Content.Headers.ContentLength = totalLength;
        //    return Task.FromResult(response);
        //}

        private async void OnStreamConnected(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[BufferLength];

                using (var nypdMedia = File.Open(this.Filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var videoLength = (int)nypdMedia.Length;
                    var videoBytesRead = 1;

                    while (videoLength > 0 && videoBytesRead > 0)
                    {
                        videoBytesRead = nypdMedia.Read(buffer, 0, Math.Min(videoLength, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, videoBytesRead);
                        videoLength -= videoBytesRead;
                    }
                }
            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                // Close output stream as we are done
                outputStream.Close();
            }
        }
    }

}