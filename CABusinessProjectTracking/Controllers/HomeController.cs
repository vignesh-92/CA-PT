using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using BusinessProjectTracking.TokenStorage;
using BusinessProjectTracking.Helper;
using PT.BusinessObjects;
using System.Collections.Generic;
using System;
using BusinessProjectTracking.Models;
using Newtonsoft.Json;
using Microsoft.Owin.Security.Notifications;
using System.Threading.Tasks;
using BusinessProjectTracking.Utils;
using Microsoft.Identity.Client;
using System.Configuration;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using BusinessProjectTracking.Filters;

namespace BusinessProjectTracking.Controllers
{
   
   public class HomeController : Controller
    {
        
        public ActionResult UnAuthorized()
        {
           
            ViewBag.unauthorized = true;
            return View();
        }

        //private async Task OnAuthorizationCodeRecieved(AuthorizationCodeReceivedNotification context)
        //{
        //    Upon successful sign in, get & cache a token using MSAL
        //    string userId = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    Microsoft.Identity.Client.TokenCache userTokenCache = new MsalSessionTokenCache(userId, context.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase).GetMsalCacheInstance();
        //    ConfidentialClientApplication cc = new ConfidentialClientApplication(ClientId, RedirectUri, new Microsoft.Identity.Client.ClientCredential(ClientSecret), userTokenCache, null);
        //    Microsoft.Identity.Client.AuthenticationResult result = await cc.AcquireTokenByAuthorizationCodeAsync(context.Code, new[] { "user.readbasic.all" });
        //}

        public ActionResult Index()
        {          
            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
            {

                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);               

                return View();
            }
            else
            {

               
                string strEmail= HttpContext.User.Identity.Name;
                //var r= AddADAccountClick(strEmail);
                //Session["Email"] = strEmail;
                var r = Utility.AddADAccountClick(strEmail);
                ViewBag.TenantId = r;


                List<oMenuAccess> authobj = null;
                ViewData["email"] = r.value[0].userPrincipalName;
                HttpCookie mailCookie = new HttpCookie("AUTH_PTEmail");
                mailCookie.Value = r.value[0].userPrincipalName;
                mailCookie.Expires = DateTime.Now.AddDays(1d);
                Response.Cookies.Add(mailCookie);

                HttpCookie mailCookies = new HttpCookie("AUTHADMIN");

                mailCookies.Value = "";

                if (CheckIsAdminUser(r.value[0].userPrincipalName))
                {
                    mailCookies.Value = "Admin";
                }
                
                mailCookies.Expires = DateTime.Now.AddDays(1d);
                Response.Cookies.Add(mailCookies);

                //if (r.value[0].userPrincipalName.ToLower() != "pt.admin@customeranalytics.com")
                if (mailCookies.Value != "Admin")
                {
                    authobj =  Utility.getRoleAccessbyEmailID(r.value[0].userPrincipalName);

                    if (authobj != null)
                    {
                        return View(authobj);
                    }
                    else
                    {
                        return RedirectToAction("UnAuthorized");
                    }
                }
              else
                {
                    
                    return View();
                }
             
            }
        }
       
        [HttpGet]
        public void SignIn()
        {
            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }          
        }
        [SessionTimeout]
        // Remove all cache entries for this user and send an OpenID Connect sign-out request.
        public void SignOut()
        {
            Session["Name"] = null;

            if (Request.Cookies["AUTH_PTPR"] != null)
            {
                string rId = Convert.ToString(Request.Cookies["AUTH_PTPR"].Value);


                Response.Cookies["AUTH_PTPR"].Expires = DateTime.Now.AddHours(-1);
                Response.Cookies["AUTH_PT" + rId].Expires = DateTime.Now.AddHours(-1);
           
                
            }
            if (Response.Cookies["AUTH_PTEmail"] != null)
            {
                Response.Cookies["AUTH_PTEmail"].Expires = DateTime.Now.AddHours(-1);
                Response.Cookies["AUTHADMIN"].Expires = DateTime.Now.AddHours(-1);
            }
            string userObjectId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            string tenantId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            string authority = $"{ Startup.AadInstance }/{ tenantId }";

            AuthenticationContext authContext = new AuthenticationContext(
                authority,
                new SampleTokenCache(userObjectId));
            authContext.TokenCache.Clear();
            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        [SessionTimeout]
        [HttpGet]
		public JsonResult GetEmailIdFromName(string searchtext)
		{
			UserResponse res = null;
			try
			{
                //res = AddADAccountClick(searchtext);

                res = Utility.AddADAccountClick(searchtext);
                ViewBag.TenantId = res;

                return Json(JsonConvert.SerializeObject(res), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonExceptionResult jsonexc = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
            }
        }
        [SessionTimeout]
        [HttpPost]
        public bool CheckIsAdminUser(string Email)
        {
            try
            {
                string query = "?Email=" + HttpUtility.UrlEncode(Email);
                AdminUsers adminUsers = CABusinessProjectTrackingAPIClient.GetHttpResponse<AdminUsers>(query, CABPTMethodConstants.CheckIsAdmin);
                if (adminUsers.IsActive==false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {          
                return false;
            }
        }
    }
}