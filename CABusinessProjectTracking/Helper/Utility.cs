using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BusinessProjectTracking.Models;
using Newtonsoft.Json;
using System.Configuration;
using PT.BusinessObjects;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace BusinessProjectTracking.Helper
{
    public class Utility
    {
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string TenantIdClaimType = ConfigurationManager.AppSettings["ida:TenantIdClaimType"];
        public static string AdminEmail = ConfigurationManager.AppSettings["Email"];
        public static string AdminPwd = ConfigurationManager.AppSettings["Pwd"];

        public static UserResponse AddADAccountClick(string searchtext)
        {
            UserResponse resultContent = null;

            try
            {

                string token = Utility.GetAccessToken();
                string loadUrl = "https://graph.microsoft.com/v1.0/users?$filter=displayName eq '" + searchtext + "'";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(loadUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = client.GetAsync(loadUrl).Result;
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                        resultContent = response.Content.ReadAsAsync<UserResponse>().Result;
                }
                //ViewBag.TenantId = resultContent;

            }
            catch (Exception ex)
            {


            }
            return resultContent;
            // return Json(ViewBag.TenantId, JsonRequestBehavior.AllowGet);
        }

        public static string GetAccessToken()
        {
            string gettoken = string.Empty;

            // Get a token for the Microsoft Graph
            var client = new HttpClient();
            var uri = "https://login.microsoftonline.com/customeranalyticsglobal.onmicrosoft.com/oauth2/token?api-version=1.0";
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("username", AdminEmail),
                new KeyValuePair<string, string>("password", AdminPwd),//"Qaqu3031"
                new KeyValuePair<string, string>("scope", "openid")
             };

            var content = new FormUrlEncodedContent(pairs);

            var response = client.PostAsync(uri, content).Result;

            string result = string.Empty;

            if (response.IsSuccessStatusCode)
            {

                result = response.Content.ReadAsStringAsync().Result;
                ADToken _ObjADToken = JsonConvert.DeserializeObject<ADToken>(result);
                gettoken = _ObjADToken.access_token;
                // Console.WriteLine(resulttok.access_token);
            }
            return gettoken;
        }

        public static List<oMenuAccess> getRoleAccessbyEmailID(string EmailID)
        {
            List<RolesAccess> accessList = null;

            List<MenuAccess> menuList = null;
            List<oMenuAccess> authobj = null;


            string query = "?Email=" + HttpUtility.UrlEncode(EmailID);

            accessList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<RolesAccess>>(query, CABPTMethodConstants.ROLEMENUMAPPING);

            if (accessList != null)
            {

                for (int i = 0; i < accessList.Count; i++)
                {

                    if (accessList[i].roleid == accessList[i].levelid)
                    {
                        HttpCookie roleCookie = new HttpCookie("AUTH_PTPR");
                        roleCookie.Value = accessList[i].projectId.ToString();
                        HttpCookie roleCookie_ = new HttpCookie("RoleID");
                        roleCookie_.Value = accessList[i].roleid.ToString();
                        //roleCookie.Values.Add("R", accessList[i].projectId.ToString());
                        roleCookie.Expires = DateTime.Now.AddDays(1d);
                        HttpContext.Current.Response.Cookies.Add(roleCookie);
                        HttpCookie menuCookie = new HttpCookie("AUTH_PT" + accessList[i].projectId);
                        menuList = accessList[i].AccessList;
                        for (int j = 0; j < menuList.Count; j++)
                        {
                            menuCookie.Values.Add(menuList[j].MenuId.ToString(), Convert.ToBoolean(menuList[j].roAccessgrant).ToString());
                        }
                        menuCookie.Expires = DateTime.Now.AddDays(1d);
                        HttpContext.Current.Response.Cookies.Add(menuCookie);

                        authobj = new List<oMenuAccess>();
                        authobj = JsonConvert.DeserializeObject<List<oMenuAccess>>(JsonConvert.SerializeObject(accessList[i].AccessList));
                    }

                }
            }
            return authobj;
        }

		public static UserResponse AddADAccountNameClickAsync(string searchtext)
		{
			//string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
			//string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

			UserResponse result = null;
			try
			{
				string token = GetAccessToken();
				//string loadUrl = "https://graph.microsoft.com/v1.0/users?$filter=startswith(givenName,'" + searchtext + "')";				
				string loadUrl = "https://graph.microsoft.com/v1.0/users?$filter=startswith(displayName,'" + searchtext + "')";

				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(loadUrl);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					HttpResponseMessage response = client.GetAsync(loadUrl).Result;
					response.EnsureSuccessStatusCode();
					if (response.IsSuccessStatusCode)
						result = response.Content.ReadAsAsync<UserResponse>().Result;
				}
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		public static string SearchADUser(string searchtext)
		{
			string result = string.Empty;
			try
			{
				string token = GetAccessToken();
				string loadurl = "https://graph.microsoft.com/v1.0/users/" + searchtext + "/manager";
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(loadurl);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					HttpResponseMessage response = client.GetAsync(loadurl).Result;
					if (response.StatusCode.ToString() == "NotFound")
					{
					}
					else
					{
						response.EnsureSuccessStatusCode();
						result = response.Content.ReadAsStringAsync().Result;
					}
				}
			}
			catch (Exception e)
			{
			}
			return result;
		}

		public static string LoadprjOwner()
		{
			string result = string.Empty;
			try
			{
				string token = GetAccessToken();
				string loadurl = "https://graph.microsoft.com/v1.0/users?$filter=JobTitle eq 'Project Manager'  or JobTitle eq 'President' or JobTitle eq 'Delivery Manager (QA)' or JobTitle eq 'Client services Manager' or JobTitle eq 'Solutions Architect' or JobTitle eq 'Process Manager' or startswith(JobTitle,'Senior Manager') or JobTitle eq 'Chairman' or JobTitle eq 'Chief Executive Officer'";
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(loadurl);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					HttpResponseMessage response = client.GetAsync(loadurl).Result;
					response.EnsureSuccessStatusCode();
					if (response.IsSuccessStatusCode)
						result = response.Content.ReadAsStringAsync().Result;
				}
			}
			catch (Exception e)
			{
			}
			return result;
		}
	}
}