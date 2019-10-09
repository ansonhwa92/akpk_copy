using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEP.Intranet.Areas.eLearning.Helper
{
    //https://www.slideshare.net/developers/oembed
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

    public static class SlideshareHelper
    {
        public static string Endpoint = "http://www.slideshare.net/api/oembed/2";

        static HttpClient client = new HttpClient();

 
        public static async Task<string> GetEmbedCode(string url)
        {
            OEmbedResponse data = null;

            var format = "format=json";
            var paramUrl = $"{Endpoint}?url={url}&{format}"; ;

            HttpResponseMessage response = await client.GetAsync(paramUrl);

            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsAsync<OEmbedResponse>();

                // change the widht and height, also add an id
                //var html = Regex.Replace(data.html, "width=\"([0-9]{1,4})\"", "id=\"myslide\" width=\"100%\"");
                //html = Regex.Replace(html, "height=\"([0-9]{1,4})\"", "height=\"500\"");

                //return html;

                //let get the src


                string src = Regex.Match(data.html, "<iframe.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                return src;

            }
            return "Error getting the slideshare.";
        }

    }


    //postman test : http://www.slideshare.net/api/oembed/2?url=https://www.slideshare.net/NVIDIA/top-5-stories-in-design-and-visualization-apr-9th-2018&format=json
    public class OEmbedResponse
    {
        public string version { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string author_name { get; set; }
        public string author_url { get; set; }
        public string provider_name { get; set; }
        public string provider_url { get; set; }
        public string html { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string thumbnail { get; set; }
        public string thumbnail_url { get; set; }
        public string thumbnail_width { get; set; }
        public string thumbnail_height { get; set; }
        public string total_slides { get; set; }
        public string conversion_version { get; set; }
        public string slide_image_baseurl { get; set; }
        public string slide_image_baseurl_suffix { get; set; }
        public string version_no { get; set; }
        public string slideshow_id { get; set; }



    }
}

//{
//    "version": "1.0",
//    "type": "rich",
//    "title": "Top 5 Stories in Design and Visualization - Apr. 9th, 2018",
//    "author_name": "NVIDIA",
//    "author_url": "https://www.slideshare.net/NVIDIA",
//    "provider_name": "SlideShare",
//    "provider_url": "https://www.slideshare.net/",
//    "html": "<iframe src=\"https://www.slideshare.net/slideshow/embed_code/key/rdOzN9kr1yK5eE\" width=\"427\" height=\"356\" frameborder=\"0\" marginwidth=\"0\" marginheight=\"0\" scrolling=\"no\" style=\"border:1px solid #CCC; border-width:1px; margin-bottom:5px; max-width: 100%;\" allowfullscreen> </iframe> <div style=\"margin-bottom:5px\"> <strong> <a href=\"https://www.slideshare.net/NVIDIA/top-5-stories-in-design-and-visualization-apr-9th-2018\" title=\"Top 5 Stories in Design and Visualization - Apr. 9th, 2018\" target=\"_blank\">Top 5 Stories in Design and Visualization - Apr. 9th, 2018</a> </strong> from <strong><a href=\"https://www.slideshare.net/NVIDIA\" target=\"_blank\">NVIDIA</a></strong> </div>\n\n",
//    "width": 425,
//    "height": 355,
//    "thumbnail": "//cdn.slidesharecdn.com/ss_thumbnails/top5stories4-9-final-180409160458-thumbnail.jpg?cb=1523406079",
//    "thumbnail_url": "https://cdn.slidesharecdn.com/ss_thumbnails/top5stories4-9-final-180409160458-thumbnail.jpg?cb=1523406079",
//    "thumbnail_width": 170,
//    "thumbnail_height": 128,
//    "total_slides": 9,
//    "conversion_version": 2,
//    "slide_image_baseurl": "//image.slidesharecdn.com/top5stories4-9-final-180409160458/85/slide-",
//    "slide_image_baseurl_suffix": "-425.jpg",
//    "version_no": "1523406079",
//    "slideshow_id": 93346111
//}
