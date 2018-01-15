using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using se_CodeFirst_3.Helper;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace se_CodeFirst_3.Controllers.api
{
    public class TokenController : ApiController
    {
        //ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpPost]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetToken(LogInViewModel logInViewModel)
        {
            //IncomingCall incomingCall = new IncomingCall();
            /// Found IT PART >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            /// 
            //string IP1 =  HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //string IP2 =  HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //Debug.WriteLine("IP1 : " + IP1);
            //Debug.WriteLine("IP2 : " + IP2);
            //string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            // Debug.WriteLine("Request HOST : " + Request.Headers.Host);
            try
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                //ApplicationUserManager application =new  ApplicationUserManager;


                // string pass = user.PasswordHash;
                //SHA256 hash = SHA256.Create();
                //byte[] bytes = hash.ComputeHash(Encoding.ASCII.GetBytes(pass));
                //string passss = Encoding.UTF8.GetString(bytes);
                //Debug.WriteLine("PASS HASH : "+ pass);

                //hash.ComputeHash(Encoding.ASCII.GetBytes(pass));
                ApplicationUser user = db.Users.Where(x => x.UserName == logInViewModel.UserName).First();
                ApplicationUser user2 = new ApplicationUser();
                userManager.Create(user2);
                user2.UserName = logInViewModel.UserName;
                Debug.WriteLine("Check Ress : ");

                //userManager.AddPassword(user2.Id, logInViewModel.Password);
                Debug.WriteLine("PASS HASH : " + user2.PasswordHash);
                if (!userManager.CheckPassword(user, logInViewModel.Password))
                {
                    return BadRequest("Didnt Catch That!");
                }
                if (user == null)
                {
                    return BadRequest("Didnt Catch That!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEP : > > " + ex.Message);
                return BadRequest("Didnt Catch That!");
            }
            ClaimsIdentity identity = new ClaimsIdentity("Khers_Bear");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("USERNAME", logInViewModel.UserName);
            dic.Add("PASSWORD", logInViewModel.Password);
            AuthenticationProperties properties = new AuthenticationProperties(dic);
            AuthenticationTicket authenticationTicket = new AuthenticationTicket(identity, properties);
            authenticationTicket.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
            //authenticationTicket.Properties.ExpiresUtc.Value.AddDays(1);
            string MyToken = Startup.OAuthOptions.AccessTokenFormat.Protect(authenticationTicket);

            Debug.WriteLine("My OWN CRATED TOKEN : " + MyToken);

            //incomingCall.Message = MyToken;
            return Ok(MyToken);
            //return MyToken;

            //AuthenticationTicket ticket = Startup.OAuthOptions.AccessTokenFormat.Unprotect(MyToken);
            //string UserName = ticket.Properties.Dictionary["USERNAME"];
            //string Pass = ticket.Properties.Dictionary["PASSWORD"];

            //Debug.WriteLine("I GOT USERNAME : " + UserName);
            //Debug.WriteLine("I GOT PASS : " + Pass);


            /////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            /////



            //HttpRequestMessage httpRequestMessage = Request;
            //string mess = Request.Headers.GetCookies().ToString();
            //Debug.WriteLine(mess);
            //Debug.WriteLine(Request.Headers.ToString());

            //Debug.WriteLine("Call Model Token Is HERE!");
            //LogInViewModel logInViewModel = new LogInViewModel
            //{
            //    UserName = "admin",
            //    Password = "bbBB11!!"
            //};
            //Dictionary<string, string> token = helper.GetTokenDetails(logInViewModel.UserName, logInViewModel.Password);
            //Debug.WriteLine("TOKEN LENGHT : " + token.Count);
            //Debug.WriteLine("Token KEYS  0 : " + token.Keys.ElementAt(0));
            //Debug.WriteLine("Token VALUES  0 : " + token.Values.ElementAt(0));



            //IncomingCall incomingCall = new IncomingCall
            //{
            //    Message = token.Values.ElementAt(0)
            //};

            //ResolveToken(token.Values.ElementAt(0));

            //return MyToken;
        }

        public bool CheckToken(string Token)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                AuthenticationTicket ticket = Startup.OAuthOptions.AccessTokenFormat.Unprotect(Token);
                string UserName = ticket.Properties.Dictionary["USERNAME"];
                string Pass = ticket.Properties.Dictionary["PASSWORD"];
                if (ticket.Properties.ExpiresUtc < DateTimeOffset.UtcNow)
                    return false;

                Debug.WriteLine("I GOT USERNAME : " + UserName);
                Debug.WriteLine("I GOT PASS : " + Pass);

                ApplicationUser user = db.Users.Where(x => x.UserName == UserName).First();
                if (user != null && userManager.CheckPassword(user, Pass))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }


    }
}