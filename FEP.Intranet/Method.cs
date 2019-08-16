using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FEP.Helper;
using Newtonsoft.Json;

namespace FEP.Intranet
{
    public static class WepApiMethod
    {
        
        public static async Task<WebApiResponse<T>> SendApiAsync<T>(HttpVerbs httpVerbs, string requestURI, object obj = null)
        {
            var url = GetWebApiUrl();

            var res = new WebApiResponse<T>();

            if (string.IsNullOrEmpty(url))
            {
                res.isSuccess = false;
                res.Data = default(T);
                res.ErrorMessage = "Web API Url not found";
                return res;
            }

            try
            {

                using (var client = new HttpClient())
                {
                    T result;

                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var payload = JsonConvert.SerializeObject(obj);

                    HttpResponseMessage response = null;

                    if (httpVerbs == HttpVerbs.Post)
                    {
                        var content = new StringContent(payload, Encoding.UTF8, "application/json");
                        response = await client.PostAsJsonAsync(requestURI, content);
                    }
                    else if (httpVerbs == HttpVerbs.Get)
                    {
                        response = await client.GetAsync(requestURI);
                    }
                    else if (httpVerbs == HttpVerbs.Put)
                    {
                        var content = new StringContent(payload, Encoding.UTF8, "application/json");
                        response = await client.PutAsJsonAsync(requestURI, content);
                    }
                    else if (httpVerbs == HttpVerbs.Delete)
                    {
                        response = await client.DeleteAsync(requestURI);
                    }
                    
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

    public class WebApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

}