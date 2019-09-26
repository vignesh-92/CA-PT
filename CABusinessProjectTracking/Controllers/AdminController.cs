using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PT.BusinessObjects;
using BusinessProjectTracking.Helper;
using Newtonsoft.Json;
using BusinessProjectTracking.Models;

namespace BusinessProjectTracking.Controllers
{
    [RouteAuthenticaion]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult RolesAndPermissions()//string Email
		{
            RouteAuthenticaion routeAuthenticaion = new RouteAuthenticaion();

            if (!routeAuthenticaion.IsAuthorizedPage(MenuIDConstant.ROLESANDPERMISSION))
            {
                return RedirectToAction("UnAuthorized", "Home");
            }
            else
            {
                ViewBag.UserName = HttpContext.User.Identity.Name;
                return View();
            }
           
            
        }
        public JsonResult RoleList(rolelist rlst)
        {
			try
			{
				List<Permissions> list = null;
				DTO oDTO = null;
				RoleInfo roleInfo = null;
				var result = string.Empty;

				if (rlst != null)
				{
					if (rlst.operation == 1)//1-select,2-insert,3-update
					{
						list = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Permissions>>(rlst, CABPTMethodConstants.ROLELIST);
						result = JsonConvert.SerializeObject(list);
					}
					else if (rlst.operation == 3)
					{
						oDTO = CABusinessProjectTrackingAPIClient.GetHttpResponse<DTO>(rlst, CABPTMethodConstants.ROLELIST);
						result = JsonConvert.SerializeObject(oDTO);
					}
					else
					{
						roleInfo = CABusinessProjectTrackingAPIClient.GetHttpResponse<RoleInfo>(rlst, CABPTMethodConstants.ROLELIST);
						result = JsonConvert.SerializeObject(roleInfo);
					}
				}
				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				JsonExceptionResult ex = new JsonExceptionResult();
				return Json(JsonConvert.SerializeObject(new CABPTException(e, out ex)));
			}
        }

        public ActionResult AdminPermissions()
        {
            if (Request.Cookies["AUTH_PTEmail"] != null && Request.Cookies["AUTHADMIN"] != null && Request.Cookies["AUTHADMIN"].Value == "Admin")
                return View();
            else
                return RedirectToAction("", "");
        }

        public JsonResult SaveAdminuserPermission(Adminrolepermission reqObj)
        {
            try {

                int result = 0;

                result = CABusinessProjectTrackingAPIClient.GetHttpResponse<int>(reqObj, CABPTMethodConstants.ADMINROLEPERMISSION);
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                JsonExceptionResult ex = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out ex)));
            }
        }

        public JsonResult GetAdminroleusers()
        {
            try
            {
                List<Adminrolepermission> result = null;
                result = CABusinessProjectTrackingAPIClient.GetHttpResponse<List<Adminrolepermission>>(string.Empty,CABPTMethodConstants.GETADMINROLEPERMISSION);
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                JsonExceptionResult ex = new JsonExceptionResult();
                return Json(JsonConvert.SerializeObject(new CABPTException(e, out ex)));
            }
        }

    }
}