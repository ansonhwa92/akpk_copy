using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Intranet.Areas.eLearning.Helper
{
    public static class YouTubeUrlHelper
    {
        private static string YouTubeEmbedUrl = "https://www.youtube.com/embed/";

        public static string ConvertToEmbeddedUrl(string url)
        {
            if (url.ToLower().Contains("youtube"))
            {
                if (!url.Contains("embed"))
                {
                    // get the last string after "/"
                    var words = url.Split('/');

                    var lastWords = words[words.Length - 1];

                    var lastWord = lastWords.Split(new string[] { "?v=" }, StringSplitOptions.None);

                    var videoId = lastWord[lastWord.Length - 1];

                    url =   YouTubeEmbedUrl + videoId;
                }

            }

            return url;
        }

    }
}
