using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{

    //https://mytechnetknowhows.wordpress.com/2017/05/18/asp-net-web-api-and-streaming-video-content/
    /// API Controller for streaming video files.
    public class VideoController : ApiController
    {
        private const string videoFilePath = "D://FEPDoc";

        /// Gets the live video.
        [HttpGet]
        [Route("api/eLearning/Video")]
        public IHttpActionResult GetLiveVideo(string videoFileId, string fileName)
        {
            //string filePath = Path.Combine(HttpContext.Current.Server.MapPath(videoFilePath), fileName);

            //actual path
            string filePath = Path.Combine(videoFilePath, fileName);
            return new VideoFileActionResult(filePath);
        }

        /// Gets the live video using post.
        /// The request.
        [HttpPost]
        [Route("api/eLearning/Video")]
        public IHttpActionResult GetLiveVideoPost(VideoFileDownloadRequest request)
        {
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath(videoFilePath), request.FileName);
            return new VideoFileActionResult(filePath);
        }
    }

    public class VideoFileDownloadRequest
    {
        public string FileName { get; set;}
    }

    /// Action Result for Returning Stream
    public class VideoFileActionResult : IHttpActionResult
    {
        private const long BufferLength = 65536;
        public VideoFileActionResult(string videoFilePath)
        {
            this.Filepath = videoFilePath;
        }

        public string Filepath { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            FileInfo fileInfo = new FileInfo(this.Filepath);
            long totalLength = fileInfo.Length;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext) =>
            {
                OnStreamConnected(outputStream, httpContent, transportContext);
            });

            response.Content.Headers.ContentLength = totalLength;
            return Task.FromResult(response);
        }

        private async void OnStreamConnected(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[BufferLength];

                using (var nypdVideo = File.Open(this.Filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var videoLength = (int)nypdVideo.Length;
                    var videoBytesRead = 1;

                    while (videoLength > 0 && videoBytesRead > 0)
                    {
                        videoBytesRead = nypdVideo.Read(buffer, 0, Math.Min(videoLength, buffer.Length));
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