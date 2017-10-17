using Newtonsoft.Json;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace se_CodeFirst_3.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        //public ActionResult Index()
        //{
        //    var httpClient = new HttpClient();

        //    string baseUrl = "http://localhost:46810/";
        //    HttpResponseMessage response = httpClient.GetAsync("http://localhost:46810/api/users").Result;

        //    string a = "";
        //    if (response.IsSuccessStatusCode)
        //    {
        //        a = response.Content.ReadAsStringAsync().Result;
        //    }
        //    else
        //    {
        //        a = response.Headers.ToString();
        //    }

        //    ViewBag.Title = a;

        //    return View();
        //}

        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> EmpInfo = new List<ApplicationUser>();

            string baseUrl = "http://localhost:46810/";

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/users");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<ApplicationUser>>(EmpResponse);

                    ViewBag.Title = "Successful";
                }
                else
                {
                    ViewBag.Title = "Failed";
                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
