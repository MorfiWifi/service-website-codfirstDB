using Newtonsoft.Json;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace se_CodeFirst_3.Helper
{
    public class ConnectToWebApiHelper : IConnectToWebApiHelper
    {
        private string baseUrl = "http://localhost:46810/";

        public HttpClient CreateAndConfigureHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();

            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        //public async Task<List<IncomingCall>> GetListOfIncomingCalls(string path)
        //{
        //    List<IncomingCall> incomingCalls = new List<IncomingCall>();

        //    var client = CreateAndConfigureHttpClient();

        //    //Sending request to find web api REST service resource using HttpClient  
        //    HttpResponseMessage response = await client.GetAsync(path);//path like: "api/incomingcalls"

        //    //Checking the response is successful or not which is sent using HttpClient  
        //    if (response.IsSuccessStatusCode)
        //    {
        //        //Storing the response details recieved from web api   
        //        var result = response.Content.ReadAsStringAsync().Result;

        //        //Deserializing the response recieved from web api and storing into the Employee list  
        //        incomingCalls = JsonConvert.DeserializeObject<List<IncomingCall>>(result);
        //    }
        //    //returning the list:
        //    return incomingCalls;
        //}

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

        //public async Task<IncomingCall> GetIncomingCall(string path)
        //{
        //    var incomingCall = new IncomingCall();
        //    var client = CreateAndConfigureHttpClient();

        //    //Sending request to find web api REST service resource using HttpClient  
        //    HttpResponseMessage response = await client.GetAsync(path);//path like: "api/incomingcalls"

        //    //Checking the response is successful or not which is sent using HttpClient  
        //    if (response.IsSuccessStatusCode)
        //    {
        //        //Storing the response details recieved from web api   
        //        var result = response.Content.ReadAsStringAsync().Result;

        //        //Deserializing the response recieved from web api and storing into the Employee list  
        //        incomingCall = JsonConvert.DeserializeObject<IncomingCall>(result);
        //    }

        //    return incomingCall;
        //}

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

        //public IncomingCall CreateIncomingCall(string path, IncomingCall incomingCall)
        //{
        //    var client = CreateAndConfigureHttpClient();

        //    var postTask = client.PostAsJsonAsync<IncomingCall>(path, incomingCall);

        //    postTask.Wait();

        //    var result = postTask.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        return incomingCall;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

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

        //public IncomingCall ChangeIncomingCall(string path, IncomingCall incomingCall)
        //{
        //    var client = CreateAndConfigureHttpClient();

        //    var putTask = client.PutAsJsonAsync<IncomingCall>(path + incomingCall.Id, incomingCall);

        //    putTask.Wait();

        //    var result = putTask.Result;
        //    if (result.IsSuccessStatusCode)
        //    {
        //        return incomingCall;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

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
    }
}