using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessProjectTracking.Models
{
	public class JsonExceptionResult
	{
		public string Status { get; set; }
		public string msg { get; set; }
	}
	public class CABPTException
	{
		public CABPTException(Exception ex, out JsonExceptionResult rtnmsg)
		{
			rtnmsg = GetExcepionMessage(ex);
		}
		public JsonExceptionResult GetExcepionMessage(Exception ex)
		{
			JsonExceptionResult rtnmth = new JsonExceptionResult();
			rtnmth.Status = "0";
			switch (ex.GetType().Name)
			{
				case "HttpRequestException":
					rtnmth.msg = ex.Message.ToString().Replace("Response status code does not indicate success: ", "").Replace(".", "");
					break;
				default:
					rtnmth.msg = ex.Message.ToString();
					break;
			}
			return rtnmth;
		}
	}
}