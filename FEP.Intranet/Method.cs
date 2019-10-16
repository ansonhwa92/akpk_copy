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
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Notification;
using Newtonsoft.Json;

namespace FEP.Intranet
{
    public static class WepApiMethod
    {
        public enum APIEngine
        {
            IntranetAPI,
            EmailSMSAPI
        }

        public static async Task<WebApiResponse<T>> SendApiAsync<T>
            (HttpVerbs httpVerbs, string requestURI, object obj = null, APIEngine APIEngine = APIEngine.IntranetAPI)
        {
            var url = GetWebApiUrl();
            if (APIEngine == APIEngine.EmailSMSAPI)
            {
                url = GetEmailSMSApiUrl();
            }

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
                        if (APIEngine == APIEngine.EmailSMSAPI)
                            response = await client.PostAsJsonAsync(requestURI, obj);
                        else
                            response = await client.PostAsync(requestURI, content);
                    }
                    else if (httpVerbs == HttpVerbs.Get)
                    {
                        response = await client.GetAsync(requestURI);
                    }
                    else if (httpVerbs == HttpVerbs.Put)
                    {
                        var content = new StringContent(payload, Encoding.UTF8, "application/json");
                        response = await client.PutAsync(requestURI, content);
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

        public static async Task<WebApiResponse<T>> SendApiAsync<T>(string requestURI, List<HttpPostedFileBase> files)
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

            if (files.Count == 0)
            {
                res.isSuccess = false;
                res.Data = default(T);
                res.ErrorMessage = "File not found";
                return res;
            }

            try
            {

                using (var client = new HttpClient())
                {
                    T result;

                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = null;

                    var content = new MultipartFormDataContent();

                    foreach (var file in files)
                    {
                        byte[] Bytes = new byte[file.InputStream.Length + 1];
                        file.InputStream.Read(Bytes, 0, Bytes.Length);
                        var fileContent = new ByteArrayContent(Bytes);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                        content.Add(fileContent);
                    }

                    response = await client.PostAsync(requestURI, content);

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
        private static string GetEmailSMSApiUrl()
        {
            string theURL = WebConfigurationManager.AppSettings["EmailSMSURL"] != null ? WebConfigurationManager.AppSettings["EmailSMSURL"].ToString() : "";
            return theURL;
        }

    }

    public class WebApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public static class FileMethod
    {
        public static async Task<List<FileDocument>> UploadFile(List<HttpPostedFileBase> files, int? userId = null, string directory = "", string fileType = "", string fileTag = "")
        {
            var responseFile = await WepApiMethod.SendApiAsync<List<FileDocument>>($"System/File?userId={userId}&directory={directory}&fileType={fileType}&fileTag={fileTag}", files.ToList());

            if (responseFile.isSuccess)
            {
                return responseFile.Data.ToList();
            }

            return null;
        }

    }

    public static class ADMethod
    {

        public static async Task<bool> Login(string userName, string password)
        {

            var url = GetWebApiUrl();

            var requestURI = "AD/ADAuth";

            try
            {

                using (var client = new HttpClient())
                {
                    
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var payload = JsonConvert.SerializeObject(new { ipUsername = userName, ipPassword = password });

                    HttpResponseMessage response = null;


                    var content = new StringContent(payload, Encoding.UTF8, "application/json");

                    response = await client.PostAsync(requestURI, content);


                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                }

            }
            catch (Exception ex)
            {

            }

            return false;

        }

        private static string GetWebApiUrl()
        {
            return WebConfigurationManager.AppSettings["ADApiURL"] != null ? WebConfigurationManager.AppSettings["ADApiURL"].ToString() : "";
        }


    }



    public static class EmailMethod
    {

        public static async Task<long?> SendEmail(string Subject, string Body, EmailAddress To, DateTime? SendDate = null)
        {

            try
            {

                SendDate = SendDate ?? DateTime.Now;

                var email = new EmailRecipientModel
                {
                    Subject = Subject,
                    Body = Body,
                    To = To,
                    SendDate = SendDate
                };

                var response = await WepApiMethod.SendApiAsync<long?>(HttpVerbs.Post, $"Notification/Email/SendEmailToRecepient", email);

                if (response.isSuccess)
                {
                    return response.Data;
                }


            }
            catch (Exception ex)
            {
                //LogError(ex.Message, ex.InnerException + " | " + ex.StackTrace);
                return null;
            }

            return null;
        }


        public static async Task<long?> SendEmail(string Subject, string Body, int UserId, DateTime? SendDate = null)
        {
            try
            {
                SendDate = SendDate ?? DateTime.Now;

                var userResponse = await WepApiMethod.SendApiAsync<UserModel>(HttpVerbs.Get, $"Administration/User?id={UserId}");

                if (userResponse.isSuccess)
                {

                    var user = userResponse.Data;

                    var email = new EmailRecipientModel
                    {
                        Subject = Subject,
                        Body = Body,
                        To = new EmailAddress { DisplayName = user.Name, Address = user.Email },
                        SendDate = DateTime.Now
                    };

                    var emailResponse = await WepApiMethod.SendApiAsync<long?>(HttpVerbs.Post, $"Notification/Email/SendEmailToRecepient", email);

                    if (emailResponse.isSuccess)
                    {
                        return emailResponse.Data;
                    }

                }

            }
            catch (Exception ex)
            {
                //LogError(ex.Message, ex.InnerException + " | " + ex.StackTrace);
                return null;
            }

            return null;
        }


        public static async Task<long?> SendEmail(string Subject, string Body, List<EmailAddress> To, DateTime? SendDate = null)
        {
            try
            {
                SendDate = SendDate ?? DateTime.Now;

                var email = new EmailRecipientsModel
                {
                    Subject = Subject,
                    Body = Body,
                    To = To,
                    SendDate = DateTime.Now
                };

                var response = await WepApiMethod.SendApiAsync<long?>(HttpVerbs.Post, $"Notification/Email/SendEmailToRecepients", email);

                if (response.isSuccess)
                {
                    return response.Data;
                }

            }
            catch (Exception ex)
            {
                //LogError(ex.Message, ex.InnerException + " | " + ex.StackTrace);
                return null;
            }

            return null;
        }


        public static async Task<long?> SendEmail(string Subject, string Body, List<int> RecipientsId, DateTime? SendDate = null)
        {
            try
            {
                SendDate = SendDate ?? DateTime.Now;

                var userResponse = await WepApiMethod.SendApiAsync<List<UserModel>>(HttpVerbs.Post, $"Administration/UserGetUsers", RecipientsId);

                if (userResponse.isSuccess)
                {

                    var users = userResponse.Data;

                    var email = new EmailRecipientsModel
                    {
                        Subject = Subject,
                        Body = Body,
                        To = users.Select(s => new EmailAddress { Address = s.Email, DisplayName = s.Name }).ToList(),
                        SendDate = DateTime.Now
                    };

                    var emailResponse = await WepApiMethod.SendApiAsync<long?>(HttpVerbs.Post, $"Notification/Email/SendEmailToRecepients", email);

                    if (emailResponse.isSuccess)
                    {
                        return emailResponse.Data;
                    }

                }



            }
            catch (Exception ex)
            {
                //LogError(ex.Message, ex.InnerException + " | " + ex.StackTrace);
                return null;
            }

            return null;
        }

    }

}