using Newtonsoft.Json;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace se_CodeFirst_3.Helper
{
    public class ConnectToWebApiHelper : IConnectToWebApiHelper
    {
        private string baseUrl = "http://localhost:46810/";
        //private string baseUrl = "http://kasrazhino.company/";

        public HttpClient CreateAndConfigureHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();

            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //see if the user loged in or not; if not: change header of http to include token:
            if (HttpContext.Current.Session["loginToken"] != null)
            {
                string token = (string)HttpContext.Current.Session["loginToken"];

                //include token with each and every response from client:
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<List<T>> GetListOfItems<T>(string path)
        {
            List<T> itemsToReturn = new List<T>();
            var client = CreateAndConfigureHttpClient();

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                itemsToReturn = JsonConvert.DeserializeObject<List<T>>(result);
            }

            return itemsToReturn;
        }

        public async Task<List<T>> GetListOfItems<T>(string path, string itemToSearch)
        {
            List<T> itemsToReturn = new List<T>();
            var client = CreateAndConfigureHttpClient();

            HttpResponseMessage response = await client.GetAsync(path + itemToSearch);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                itemsToReturn = JsonConvert.DeserializeObject<List<T>>(result);
            }

            return itemsToReturn;

            //var client = CreateAndConfigureHttpClient();

            //var postTask = client.PostAsJsonAsync<T>(path, itemToSearch);

            //postTask.Wait();

            //var result = postTask.Result;
            //if (result.IsSuccessStatusCode)
            //{
            //    var result2 = result.Content.ReadAsStringAsync().Result;

            //    return JsonConvert.DeserializeObject<List<T>>(result2);
            //}
            //else
            //{
            //    return null;
            //}
        }

        public async Task<T> GetItem<T>(string path)
        {
            T itemToReturn = default(T);
            var client = CreateAndConfigureHttpClient();

            //Sending request to find web api REST service resource using HttpClient  
            HttpResponseMessage response = await client.GetAsync(path);
            
            //Checking the response is successful or not which is sent using HttpClient  
            if (response.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var result = response.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list  
                itemToReturn = JsonConvert.DeserializeObject<T>(result);
            }

            return itemToReturn;
        }

        public T CreateItem<T>(string path, T itemToCreate)
        {
            var client = CreateAndConfigureHttpClient();

            var postTask = client.PostAsJsonAsync<T>(path, itemToCreate);

            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return itemToCreate;
            }
            else
            {
                return default(T);
            }
        }

        public T ChangeItem<T>(string path, T itemToChange)
        {
            var client = CreateAndConfigureHttpClient();
            var putTask = client.PutAsJsonAsync<T>(path, itemToChange);

            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return itemToChange;
            }
            else
            {
                return default(T);
            }
        }

        public bool DeleteItem(string path, int id)
        {
            var client = CreateAndConfigureHttpClient();

            var deleteTask = client.DeleteAsync(path + id);

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteItem(string path, string id)
        {
            var client = CreateAndConfigureHttpClient();

            var deleteTask = client.DeleteAsync(path + id);

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Authorization Part::
        public async Task<Dictionary<string, string>> GetTokenDetails(string userName, string password)
        {
            Dictionary<string, string> tokenDetails = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", userName},
                       {"password", password},
                   };

                    var resp = client.PostAsync(baseUrl + "token", new FormUrlEncodedContent(login));
                    resp.Wait(TimeSpan.FromSeconds(10));

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                        {
                            tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.Result.Content.ReadAsStringAsync().Result);

                            //var users = await this.GetListOfItems<ApplicationUser>("api/users");

                            //var userID = (from item in users
                            //              where item.UserName == userName
                            //              select item.Id).SingleOrDefault();

                            //HttpContext.Current.Session["logedInUserId"] = userID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return tokenDetails;
        }

        public bool LogOut(string path)
        {
            var client = CreateAndConfigureHttpClient();

            string token = (string)HttpContext.Current.Session["loginToken"];

            //include token with each and every response from client:
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postTask = client.PostAsync(path, null);

            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}