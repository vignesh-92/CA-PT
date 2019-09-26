using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessProjectTracking.Models;
using BusinessProjectTracking.Helper;
using PT.BusinessObjects;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Configuration;
using Microsoft.Identity.Client;
using System.Net.Http;
using BusinessProjectTracking.Utils;
using System.Collections.Concurrent;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using BusinessProjectTracking.Filters;

namespace BusinessProjectTracking.Controllers
{
    [SessionTimeout]
    [RouteAuthenticaion]
    public class ProjectsController : Controller
    {
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUrl"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string AadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        public static string GraphResourceId = ConfigurationManager.AppSettings["ida:ResourceId"];
        public static string BasicSignInScope = ConfigurationManager.AppSettings["ida:BasicSignInScopes"];
        public static string Authority = ConfigurationManager.AppSettings["ida:Authority"];
        public static string TenantIdClaimType = ConfigurationManager.AppSettings["ida:TenantIdClaimType"];
        public static string AdminEmail = ConfigurationManager.AppSettings["Email"];
        public static string AdminPwd = ConfigurationManager.AppSettings["Pwd"];
        private ConcurrentDictionary<string, List<User>> userList = new ConcurrentDictionary<string, List<User>>();
        
        public ActionResult AddADAccount()
        {
            return View();
        }

		//Working SearchADUser(ASYNC)
		//public async Task<ActionResult> SearchADUser(string searchtext)
		//{
		//    string token = string.Empty;
		//    string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
		//    string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

		//    try
		//    {


		//        token = Utility.GetAccessToken();

		//        HttpClient client1 = new HttpClient();
		//        HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/" + searchtext + "/manager");
		//        request1.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		//        // Ensure a successful response
		//        HttpResponseMessage response1 = await client1.SendAsync(request1);
		//        if(response1.StatusCode.ToString() == "NotFound")
		//        {

		//        }
		//        else
		//        {
		//            response1.EnsureSuccessStatusCode();
		//            // Populate the data store with the first page of groups
		//            string json1 = await response1.Content.ReadAsStringAsync();
		//            ViewBag.Emailresult = json1;
		//        }

		//    }
		//    // If the tokens have expired or become invalid for any reason, ask the user to sign in again
		//    catch (MsalUiRequiredException ex)
		//    {
		//        return new RedirectResult("/Account/SignIn");

		//    }
		//    // Handle unexpected errors.
		//    catch (Exception ex)
		//    {

		//    }

		//    //  ViewBag.TenantId = tenantId;
		//    // return View();
		//    return Json(ViewBag.Emailresult, JsonRequestBehavior.AllowGet);
		//}

		public ActionResult SearchADUser(string searchtext)
		{
			//string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
			//string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

			try
			{
				string result = Utility.SearchADUser(searchtext);
				if (result != "")
				{
					ViewBag.Emailresult = result;
				}
			}
			catch (MsalUiRequiredException ex)
			{
				return new RedirectResult("/Account/SignIn");
			}
			catch (Exception e) {
			}
			return Json(ViewBag.Emailresult, JsonRequestBehavior.AllowGet);
		}

		//Working PrjManager and PrjOwner(ASYNC)
        //public async Task<ActionResult> LoadprjManger()
        //{
        //    string token = string.Empty;
        //    string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
        //    string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    try
        //    {
        //        // Get a token for the Microsoft Graph
        //        if (token == "")
        //        {
        //            // token = await GetGraphAccessToken(userId, new string[] { "user.readbasic.all" });
        //            token = Utility.GetAccessToken();
        //        }
        //        // Construct the query
        //        HttpClient client = new HttpClient();
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=JobTitle eq 'Project Manager' or JobTitle eq 'Team Lead' or JobTitle eq 'Delivery Manager (QA)' or JobTitle eq 'Process Manager' or JobTitle eq 'Technical Lead' or JobTitle eq 'QA Lead' or JobTitle eq 'Senior Technical Lead' or JobTitle eq 'Senior Project Lead' or JobTitle eq 'Client services Manager'  or JobTitle eq 'Senior Project Lead'");
        //        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        // Ensure a successful response
        //        HttpResponseMessage response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();

        //        // Populate the data store with the first page of groups
        //        string json = await response.Content.ReadAsStringAsync();
        //        UserResponse result = JsonConvert.DeserializeObject<UserResponse>(json);
        //        ViewBag.prjManager = json;                
               

        //    }
        //    // If the tokens have expired or become invalid for any reason, ask the user to sign in again
        //    catch (MsalUiRequiredException ex)
        //    {
        //        return new RedirectResult("/Account/SignIn");

        //    }
        //    // Handle unexpected errors.
        //    catch (Exception ex)
        //    {

        //    }

        //    //  ViewBag.TenantId = tenantId;
        //    // return View();
        //    return Json(ViewBag.prjManager, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<ActionResult> LoadprjOwner()
        //{
        //    string token = string.Empty;
        //    string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
        //    string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    try
        //    {
        //        // Get a token for the Microsoft Graph
        //        if (token == "")
        //        {
        //            // token = await GetGraphAccessToken(userId, new string[] { "user.readbasic.all" });
        //            token = Utility.GetAccessToken();
        //        }
        //        // Construct the query
        //        HttpClient client = new HttpClient();
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=JobTitle eq 'Project Manager'  or JobTitle eq 'President' or JobTitle eq 'Delivery Manager (QA)' or JobTitle eq 'Client services Manager' or JobTitle eq 'Solutions Architect' or JobTitle eq 'Process Manager' or startswith(JobTitle,'Senior Manager') or JobTitle eq 'Chairman' or JobTitle eq 'Chief Executive Officer'");
        //        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        // Ensure a successful response
        //        HttpResponseMessage response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();

        //        // Populate the data store with the first page of groups
        //        string json = await response.Content.ReadAsStringAsync();
        //        UserResponse result = JsonConvert.DeserializeObject<UserResponse>(json);
        //        ViewBag.prjManager = json;
        //        //  GetManager(token);

        //    }
        //    // If the tokens have expired or become invalid for any reason, ask the user to sign in again
        //    catch (MsalUiRequiredException ex)
        //    {
        //        return new RedirectResult("/Account/SignIn");

        //    }
        //    // Handle unexpected errors.
        //    catch (Exception ex)
        //    {

        //    }

        //    //  ViewBag.TenantId = tenantId;
        //    // return View();
        //    return Json(ViewBag.prjManager, JsonRequestBehavior.AllowGet);
        //}

		public ActionResult LoadprjOwner()
		{
			//string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
			//string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

			try
			{
				string result = Utility.LoadprjOwner();
				if (result != "") {
					UserResponse response = JsonConvert.DeserializeObject<UserResponse>(result);
					ViewBag.prjManager = result;
				}				
			}
			catch (MsalUiRequiredException ex)
			{
				return new RedirectResult("/Account/SignIn");
			}
			catch (Exception e) { }
			return Json(ViewBag.prjManager, JsonRequestBehavior.AllowGet);
		}

		//Working AddADAccountClick(ASYNC)
		//      public async Task<ActionResult> AddADAccountClick(string searchtext)
		//      {
		//	//string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;
		//	//string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

		//	try
		//	{
		//		// Get a token for the Microsoft Graph



		//		//string token = await GetGraphAccessToken(userId, new string[] { "user.readbasic.all" });
		//		string token = Utility.GetAccessToken();

		//		//  Construct the query
		//		HttpClient client = new HttpClient();
		//		//Using Mail ID.
		//		//HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=startswith(givenName,'" + searchtext + "' )");
		//		//Using Name.
		//		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$filter=startswith(displayName,'" + searchtext + "')");
		//		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		//		// Ensure a successful response
		//		HttpResponseMessage response = await client.SendAsync(request);
		//		response.EnsureSuccessStatusCode();

		//		// Populate the data store with the first page of groups
		//		string json = await response.Content.ReadAsStringAsync();
		//		UserResponse result = JsonConvert.DeserializeObject<UserResponse>(json);
		//		ViewBag.TenantId = result;


		//	}
		//	// If the tokens have expired or become invalid for any reason, ask the user to sign in again
		//	catch (MsalUiRequiredException ex)
		//	{
		//		return new RedirectResult("/Account/SignIn");

		//	}
		//	// Handle unexpected errors.
		//	catch (Exception ex)
		//	{

		//	}

		//	//ViewBag.TenantId = tenantId;
		//	// return View();
		//	return Json(ViewBag.TenantId, JsonRequestBehavior.AllowGet);
		//}

		public ActionResult AddADAccountClick(string searchtext)
		{
			try
			{
				var user = Utility.AddADAccountNameClickAsync(searchtext);
				ViewBag.TenantId = user;
			}
			// If the tokens have expired or become invalid for any reason, ask the user to sign in again
			catch (MsalUiRequiredException ex)
			{
				return new RedirectResult("/Account/SignIn");
			}
			//Handle unexpected errors.
			catch (Exception ex)
			{

			}
			return Json(ViewBag.TenantId, JsonRequestBehavior.AllowGet);
		}
		
		//private async Task<string> GetGraphAccessToken(string userId, string[] scopes)
		//{
		//    Microsoft.Identity.Client.TokenCache userTokenCache = new MsalSessionTokenCache(userId, HttpContext).GetMsalCacheInstance();
		//    ConfidentialClientApplication cc = new ConfidentialClientApplication(ClientId, RedirectUri, new Microsoft.Identity.Client.ClientCredential(ClientSecret), userTokenCache, null);
		//    Microsoft.Identity.Client.AuthenticationResult result = await cc.AcquireTokenSilentAsync(scopes, cc.Users.First());
		//    return result.AccessToken;
		//}

		//public string GetAccessToken()
		//{
		//    string gettoken = string.Empty;

		//    // Get a token for the Microsoft Graph
		//    var client = new HttpClient();       
		//    var uri = "https://login.microsoftonline.com/customeranalyticsglobal.onmicrosoft.com/oauth2/token?api-version=1.0";
		//    var pairs = new List<KeyValuePair<string, string>>
		//    {
		//        new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"),
		//        new KeyValuePair<string, string>("client_id", ClientId),
		//        new KeyValuePair<string, string>("client_secret", ClientSecret),
		//        new KeyValuePair<string, string>("grant_type", "client_credentials"),
		//        new KeyValuePair<string, string>("username", AdminEmail),
		//        new KeyValuePair<string, string>("password", AdminPwd),
		//        new KeyValuePair<string, string>("scope", "openid")
		//     };

		//    var content = new FormUrlEncodedContent(pairs);

		//    var response = client.PostAsync(uri, content).Result;

		//    string result = string.Empty;

		//    if (response.IsSuccessStatusCode)
		//    {

		//        result = response.Content.ReadAsStringAsync().Result;
		//        ADToken _ObjADToken = JsonConvert.DeserializeObject<ADToken>(result);
		//        gettoken=_ObjADToken.access_token;
		//        // Console.WriteLine(resulttok.access_token);
		//    }           
		//    return gettoken;
		//}


		//public async void GetManager(string token,string searchtext)
		//{
		//    string strRsultEmail = string.Empty;
		//    try
		//    {

		//        HttpClient client = new HttpClient();
		//        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/" + searchtext + "/manager");
		//        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		//        // Ensure a successful response
		//        HttpResponseMessage response = await client.SendAsync(request);
		//        response.EnsureSuccessStatusCode();

		//        // Populate the data store with the first page of groups
		//        string json = await response.Content.ReadAsStringAsync();
		//        var result = JsonConvert.DeserializeObject(json);

		//        JObject jObject = JObject.Parse(json);
		//        string strEmail = (string)jObject.SelectToken("mail");
		//        strRsultEmail = strEmail;
		//        if (strEmail!= "")
		//        {
		//            HttpClient client1 = new HttpClient();
		//            HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/" + strEmail + "/manager");
		//            request1.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		//            // Ensure a successful response
		//            HttpResponseMessage response1 = await client1.SendAsync(request1);
		//            response1.EnsureSuccessStatusCode();

		//            // Populate the data store with the first page of groups
		//            string json1 = await response1.Content.ReadAsStringAsync();
		//            var result1 = JsonConvert.DeserializeObject(json1);

		//            JObject jObject1 = JObject.Parse(json1);
		//            string strEmail1 = (string)jObject1.SelectToken("mail");
		//            strRsultEmail = strRsultEmail + "," + strEmail1;
		//            Session["sEmail"] = strRsultEmail;
		//            //ViewBag.strEmail = strRsultEmail;

		//            jObject.Merge(jObject1, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
		//        }
		//        ViewBag.strEmail = jObject;
		//        // Construct the query
		//        //HttpClient client = new HttpClient();
		//        //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users/"+ searchtext + "/manager");
		//        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		//        //string content = @"{}";

		//        //var httpContent = new StringContent(content, Encoding.GetEncoding("utf-8"), "application/json");

		//        //request.Content = httpContent;

		//        //// Ensure a successful response
		//        //HttpResponseMessage response = await client.SendAsync(request);
		//        //response.EnsureSuccessStatusCode();

		//    }

		//    // If the tokens have expired or become invalid for any reason, ask the user to sign in again
		//    catch (Exception ex)
		//    {


		//    }
		//    //return ViewBag.strEmail;
		//}

		public ActionResult HomeDashboard()
        {          
            return View();
        }

        [HttpPost]
        public JsonResult CurrentRelease(ReleaseRequest rreq)
        {
            ReleaseResponse release = null;
            try
            {
              if(rreq !=null)
                {
                    if (Request.Cookies["AUTH_PTEmail"] != null && Request.Cookies["AUTHADMIN"] != null && Request.Cookies["AUTHADMIN"].Value== "Admin")
                    {
                        rreq.emailid = "";
                    }

                    release = CABusinessProjectTrackingAPIClient.GetHttpResponse<ReleaseResponse>(rreq, CABPTMethodConstants.CURRENTRELEASE);
                }
                return Json(JsonConvert.SerializeObject(release));
            }

            catch (Exception ex)
            {
                JsonExceptionResult jsonexc = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
            }
        }

		//GET: AllProjects
		//[HttpGet]
		//public ActionResult Index()
		//{
		//	return View();
		//}

		//[HttpPost]
		//public JsonResult Index(AllProjectRequest prtReq)
		//{
		//	try
		//	{
		//		List<AllProjectResponse> projectsList = null;
		//		if (prtReq != null)
		//		{
		//			projectsList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<AllProjectResponse>>(prtReq, CABPTMethodConstants.ALLPROJECTS);					
		//		}
		//		return Json(JsonConvert.SerializeObject(projectsList));
		//	}
		//	catch (Exception ex)
		//	{
		//		JsonExceptionResult jsonexc = new JsonExceptionResult();
		//		return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
		//	}
		//}

        [Authorize]
        public ActionResult AddNewProjects()
        {
            
            RouteAuthenticaion routeAuthenticaion = new RouteAuthenticaion();

            if (!routeAuthenticaion.IsAuthorizedPage(MenuIDConstant.ADDNEWPROJECT))
            {
                return RedirectToAction("UnAuthorized", "Home");
            }
            else
            {
                return View();
            }
            
        }

		[HttpPost]
		public JsonResult AddedProjectDetails(AddProject prtdetails)
		{
			try
			{
				AddProjectResponse result = null;
				if (prtdetails != null)
				{
					result = CABusinessProjectTrackingAPIClient.GetHttpResponse<AddProjectResponse>(prtdetails, CABPTMethodConstants.ADDNEWPROJECT);
				}
				return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				JsonExceptionResult jsonexc = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
			}
		}

		[HttpPost]
		public JsonResult AddNewClient(Clients client)
		{
			AddNewClientResponse clientRes = null;
			try
			{			
				clientRes = CABusinessProjectTrackingAPIClient.GetHttpResponse<AddNewClientResponse>(client, CABPTMethodConstants.ADDNEWCLIENT);
				return Json(JsonConvert.SerializeObject(clientRes), JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				JsonExceptionResult jsonexc = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
			}
		}

		public ActionResult ProjectDetails()
        {
			ViewBag.UserName = HttpContext.User.Identity.Name;
            return View();
        }

        public ActionResult Deliverables()
        {
            return View();
        }
        
        public ActionResult DeliverablesDetails(int pid,int did,int? ddetid,string str)
        {
            ViewData["pid"] = pid;
            ViewData["did"] = did;
			ViewData["ddetid"] = ddetid;
			ViewData["AddNewDeliverableDetail"] = str;
            ViewBag.UserName = HttpContext.User.Identity.Name;

            return View();
        }

        public ActionResult DeliverablesTracking()
        {
            return View();
        }

		[HttpPost]
		public JsonResult DeliverableTrackingDetailsByID(ProjectDeliverableDetailRequest obj)
		{
			DeliverableDetailsResponse result;
			try
			{
				result = CABusinessProjectTrackingAPIClient.GetHttpResponse<DeliverableDetailsResponse>(obj, CABPTMethodConstants.DELIVERABLETRACKINGBYID);

                for (int i = 0; i < result.DeliverableTrackingList.Count; i++)
                {
                    if (result.DeliverableTrackingList[i].PlannedDuration == "1 Days" || result.DeliverableTrackingList[i].PlannedDuration == "0 Days")
                    {
                        result.DeliverableTrackingList[i].PlannedDuration = result.DeliverableTrackingList[i].PlannedDuration.Replace("Days", "Day");
                    }
                    if (result.DeliverableTrackingList[i].ActualDuration == "1 Days" || result.DeliverableTrackingList[i].ActualDuration == "0 Days")
                    {
                        result.DeliverableTrackingList[i].ActualDuration = result.DeliverableTrackingList[i].ActualDuration.Replace("Days", "Day");
                    }

                }

                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult jsonex = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out jsonex)));
			}
		}

        //     public JsonResult DeliverablesDetails(Deliverables deliverable)
        //     {
        //return Json(JsonConvert.SerializeObject(deliverable), JsonRequestBehavior.AllowGet);
        //     }

        [HttpPost]
        public JsonResult DeliverablesUtildtl(pdDTO del)
        {
            DeliverablesUtilResponse resobj = null;

            try
            {
              
                if (del != null)
                {
                    resobj = CABusinessProjectTrackingAPIClient.GetHttpResponse<DeliverablesUtilResponse>(del, CABPTMethodConstants.DELIVERABLEUTIL);
                }
                return Json(JsonConvert.SerializeObject(resobj));
            }
            catch (Exception ex)
            {
                JsonExceptionResult jsonexc = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
            }
        }

		public ActionResult AddDeliverables()
        {
            return View();
        }

		[HttpPost]
		public JsonResult AddedDeliverables(AddDeliverables delDetails)
		{
			try
			{
				DTO oDTO = null;
				if (delDetails != null)
				{
					oDTO = CABusinessProjectTrackingAPIClient.GetHttpResponse<DTO>(delDetails, CABPTMethodConstants.ADDDELIVERABLE);
				}
				return Json(JsonConvert.SerializeObject(oDTO), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult jsonec = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out jsonec)));
			}
		}


		[HttpGet]
		public JsonResult ProjectFilters()
		{
			ReportFilterList reportFilterList = null;
			try
			{
				reportFilterList = CABusinessProjectTrackingAPIClient.GetHttpResponse<ReportFilterList>(string.Empty, CABPTMethodConstants.REPORTFILTERS);
				return Json(JsonConvert.SerializeObject(reportFilterList), JsonRequestBehavior.AllowGet);
			}			
			catch (Exception e)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
			}
		}


        public JsonResult StageList()
        {
            List<Stage> list = null;

            try
            {
                list = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Stage>>(string.Empty, CABPTMethodConstants.StageList);
                return Json(JsonConvert.SerializeObject(list), JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
            }
        }

        public JsonResult StateList(int stageid)
        {
            List<State> list = null;

            try
            {
                string queryString = "?StageId=" + stageid;
                list = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<State>>(queryString, CABPTMethodConstants.StateList);
                return Json(JsonConvert.SerializeObject(list), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
            }
        }

		[HttpGet]
		public JsonResult ProjectResources(int ProjectId)
		{
			List<AddProjectUser> result = null;
			try
			{
				string queryString = "?ProjectId=" + ProjectId;
				result = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<AddProjectUser>>(queryString, CABPTMethodConstants.ProjectResources);
				return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
			}
		}

		[HttpPost]
        public JsonResult UpdateDeliverableDetails(UpdateDeliverableDtl dboj)
        {
            UpdateDeliverableDtlResponse oDTO = null;
            try
            {
                if (dboj != null)
                {
                    oDTO = CABusinessProjectTrackingAPIClient.GetHttpResponse<UpdateDeliverableDtlResponse>(dboj, CABPTMethodConstants.UPDATEDELIVERALBEDETAILS);
                }
                return Json(JsonConvert.SerializeObject(oDTO), JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
            }
        }

        public JsonResult UpdateResourceUtil(DTO dTO)
        {
            DTO oDTO = null;
            try
            {
                if (dTO.Status != null)
                {
                    oDTO = CABusinessProjectTrackingAPIClient.GetHttpResponse<DTO>(dTO, CABPTMethodConstants.UPDATERESOURCEUTIL);
                }
                return Json(JsonConvert.SerializeObject(oDTO), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
            }
        }

        //public JsonResult DeliverableInfoByProjectId(DeliverablesbyProjectRequest devobj)
        //{
        //	Deliverables deliverables;

		[HttpPost]
		public JsonResult DetailsByIds(ProjectDeliverableDetailRequest obj)
		{
			ProjectDeliverableDetailResponse result;
			try
			{
				result = CABusinessProjectTrackingAPIClient.GetHttpResponse<ProjectDeliverableDetailResponse>(obj, CABPTMethodConstants.PROJECTSBYID);

                for (int i = 0; i < result.DeliverableList.Count; i++)
                {
                    if (result.DeliverableList[i].PlannedDuration == "1 Days" || result.DeliverableList[i].PlannedDuration == "0 Days")
                    {
                        result.DeliverableList[i].PlannedDuration = result.DeliverableList[i].PlannedDuration.Replace("Days", "Day");
                    }
                    if (result.DeliverableList[i].ActualDuration == "1 Days" || result.DeliverableList[i].ActualDuration == "0 Days")
                    {
                        result.DeliverableList[i].ActualDuration = result.DeliverableList[i].ActualDuration.Replace("Days", "Day");
                    }

                }
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(ex, out exception)));
			}
		}

        //	try
        //	{
        //		deliverables = CABusinessProjectTrackingAPIClient.GetHttpResponse<Deliverables>(devobj, CABPTMethodConstants.DELIVERABLEBYID);
        //		return Json(JsonConvert.SerializeObject(deliverables), JsonRequestBehavior.AllowGet);
        //	}
        //	catch (Exception ex)
        //	{
        //		JsonExceptionResult exception = new JsonExceptionResult();
        //		return Json(JsonConvert.SerializeObject(new CABPTException(ex, out exception)));
        //	}
        //}

		public JsonResult UpdateProjectDetail(UpdateProject obj)
		{
			DTO result = null;
			try
			{
               
                result = CABusinessProjectTrackingAPIClient.GetHttpResponse<DTO>(obj, CABPTMethodConstants.UPDATEPROJECTDETAIL);
				return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(ex, out exception)));
			}
		}
        public JsonResult GetUserRole(string tblName)
        {
            UserRoleLookup result = null;
            try
            {
                string queryString = "?tblName=" + tblName;
                result = CABusinessProjectTrackingAPIClient.GetHttpResponse<UserRoleLookup>(queryString, CABPTMethodConstants.GETUSERROLE);
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(ex, out exception)));
            }
        }
        [HttpPost]
        public JsonResult AddAdAccount(ADAccountRequest reqObj)
		{
			ADAccountResponse result = null;
			try
            {
                //AddProjectUserResponse result = null;
                //var jsonSerialiser = new JavaScriptSerializer();
                //var json = jsonSerialiser.Serialize(tblJson);
                //var user = JsonConvert.DeserializeObject<List<AddProjectUser>>(tblJson);
                //AddProjectUser obj = JsonConvert.DeserializeObject<AddProjectUser>(tblJson);
                result = CABusinessProjectTrackingAPIClient.GetHttpResponse<ADAccountResponse>(reqObj, CABPTMethodConstants.ADDUSERROLE);
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                JsonExceptionResult exception = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(ex, out exception)));
            }
        }


        [HttpPost]
		public JsonResult StateList(Stage stage)
		{
			List<State> stateList = null;
			try
			{
				stateList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<State>>(stage, CABPTMethodConstants.STATELIST);
				return Json(JsonConvert.SerializeObject(stateList), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
			}
		}

		[HttpPost]
		public JsonResult DeliverableDetailsUpdate(DeliverableDetailUpdateRequest obj)
		{
			DTO oDTO = null;
			try
			{
				oDTO = CABusinessProjectTrackingAPIClient.GetHttpResponse<DTO>(obj, CABPTMethodConstants.DELIVERABLEDETAILSUPDATE);
				return Json(JsonConvert.SerializeObject(oDTO), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult exception = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out exception)));
			}
		}

	}
}