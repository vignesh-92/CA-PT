using System;
using PT.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace BusinessProjectTracking.Helper
{
	public class CABusinessProjectTrackingAPIClient
	{
		private static string BaseUrl = ConfigurationManager.AppSettings["WebAPIBaseURL"].ToString();
		private static string SubUrl = ConfigurationManager.AppSettings["WebAPISubURL"].ToString();
		private static string AuthorizedCode = ConfigurationManager.AppSettings["AuthCode"].ToString();
		private static string Email = ConfigurationManager.AppSettings["Email"].ToString();
		private static HttpClient client = new HttpClient();
		public CABusinessProjectTrackingAPIClient()
		{
			
		}

		public static string GetEmailId(string logUser, string actionName)
		{
			return "";
		}

		public static T GetHttpResponse<T>(Object query, string actionName/*, string logUser*/)
		{
			T resultContent = default(T);
			DTO modal = new DTO();
			modal.Status = JsonConvert.SerializeObject(query);

			using (var client = new HttpClient())
			{
				//string sEmailId = GetEmailId(logUser, actionName);
				string sEmailId = Email;
				client.BaseAddress = new Uri(BaseUrl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("AuthUser", Email);//logUser
				client.DefaultRequestHeaders.Add("AuthCode", CABusinessProjectTrackingCrypt.EncryptString(AuthorizedCode + "||" + sEmailId));
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
				//JsonConvert.SerializeObject(modal)
				HttpResponseMessage response = client.PostAsync(SubUrl + actionName, new StringContent(modal.Status, Encoding.UTF8, "application/json")).Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
					resultContent = response.Content.ReadAsAsync<T>().Result;
			}
			return resultContent;
		}

		public static T GetHttpResponse<T>(string querystring, string actionName/*, string logUser*/)
		{
			string loadUrl = querystring == string.Empty ? SubUrl + actionName : SubUrl + actionName + querystring;
			T resultContent = default(T);
			using (var client = new HttpClient())
			{
				//string sEmailId = GetEmailId(logUser, actionName);
				string sEmailId = Email;
				client.BaseAddress = new Uri(BaseUrl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("AuthUser", Email);//logUser
				client.DefaultRequestHeaders.Add("AuthCode", CABusinessProjectTrackingCrypt.EncryptString(AuthorizedCode + "||" + sEmailId));
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));

				HttpResponseMessage response = client.GetAsync(loadUrl).Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
					resultContent = response.Content.ReadAsAsync<T>().Result;
			}
			return resultContent;
		}

		public static String strGetHttpResponse(object query, string actionName/*, string logUser*/)
		{
			string resultContent = string.Empty;
			DTO modal = new DTO();
			modal.Status = JsonConvert.SerializeObject(query);

			using (var client = new HttpClient())
			{
				//string sEmailId = GetEmailId(logUser, actionName);
				string sEmailId = Email;
				client.BaseAddress = new Uri(BaseUrl);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("AuthUser", Email);//logUser
				client.DefaultRequestHeaders.Add("AuthCode", CABusinessProjectTrackingCrypt.EncryptString(AuthorizedCode + "||" + sEmailId));
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));

				HttpResponseMessage response = client.PostAsync(SubUrl + actionName, new StringContent(JsonConvert.SerializeObject(modal), Encoding.UTF8, "application/json")).Result;
				response.EnsureSuccessStatusCode();
				if (response.IsSuccessStatusCode)
					resultContent = response.Content.ReadAsStringAsync().Result;
			}
			return resultContent;
		}
	}
}