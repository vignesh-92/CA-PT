using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PT.BusinessObjects;
using BusinessProjectTracking.Helper;
using Newtonsoft.Json;
using BusinessProjectTracking.Models;
using System.Globalization;

namespace BusinessProjectTracking.Controllers
{
    [RouteAuthenticaion]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult UtilizationReports()
        {
          
            return View();
        }

        public ActionResult DeliverableReports()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult AllDeliverables(DeliverableRequest deliverableRequest)
        {
            if (deliverableRequest.FromDate != null)
            {
                deliverableRequest.FromDate = Convert.ToDateTime(Convert.ToDateTime(deliverableRequest.FromDate).ToString("dd-MM-yyyy"));
            }
            if (deliverableRequest.ToDate != null)
            {
                deliverableRequest.ToDate = Convert.ToDateTime(Convert.ToDateTime(deliverableRequest.ToDate).ToString("dd-MM-yyyy"));
            }
            if(Request.Cookies["AUTH_PTEmail"] != null &&  Request.Cookies["AUTHADMIN"] != null && Request.Cookies["AUTHADMIN"].Value == "Admin")
            {
                deliverableRequest.LoginMailId = "";
            }
            List<Deliverables> result = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Deliverables>>(deliverableRequest, CABPTMethodConstants.ALLDELIVERABLES);

            for (int i = 0; i < result.Count; i++)
            {
                if(result[i].PlannedDuration =="1 Days" || result[i].PlannedDuration == "0 Days")
                {
                    result[i].PlannedDuration = result[i].PlannedDuration.Replace("Days", "Day");
                }
                if (result[i].ActualDuration == "1 Days" || result[i].ActualDuration == "0 Days")
                {
                    result[i].ActualDuration = result[i].ActualDuration.Replace("Days", "Day");
                }

            }
            return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AllUtilizations(DeliverableRequest uobj)
        {
            if (Request.Cookies["AUTH_PTEmail"] != null && Request.Cookies["AUTHADMIN"] != null && Request.Cookies["AUTHADMIN"].Value == "Admin")
            {
                uobj.LoginMailId = "";
            }

            List<UtilizationResponse> result = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<UtilizationResponse>>(uobj, CABPTMethodConstants.ALLUTILIZATIONS);

            return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
        public ActionResult ProjectReports()
        {
           
            return View();
        }

		[HttpPost]
		public JsonResult ProjectReports(AllProjectRequest prtReq)
		{
			try
			{
				List<AllProjectResponse> projectsList = null;
				if (prtReq != null)
				{
                    if (Request.Cookies["AUTH_PTEmail"] != null && Request.Cookies["AUTHADMIN"] != null && Request.Cookies["AUTHADMIN"].Value == "Admin")
                    {
                        prtReq.LoginMailID = "";
                    }

                    projectsList = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<AllProjectResponse>>(prtReq, CABPTMethodConstants.ALLPROJECTS);
				}
				return Json(JsonConvert.SerializeObject(projectsList));
			}
			catch (Exception ex)
			{
				JsonExceptionResult jsonexc = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(ex, out jsonexc)));
			}
		}

        [HttpGet]
        public JsonResult ReportFilters()
        {
            ReportFilterList result = CABusinessProjectTrackingAPIClient.GetHttpResponse<ReportFilterList>(string.Empty, CABPTMethodConstants.REPORTFILTERS);
            return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
        }
    }
}