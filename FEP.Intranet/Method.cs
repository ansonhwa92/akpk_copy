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
            if(APIEngine == APIEngine.EmailSMSAPI)
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
                        if(APIEngine == APIEngine.EmailSMSAPI)
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