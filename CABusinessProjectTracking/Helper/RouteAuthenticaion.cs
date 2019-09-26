using BusinessProjectTracking.Models;
using PT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace BusinessProjectTracking.Helper
{
    public class RouteAuthenticaion : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new Exception("Invalid Action");
            }
            if (IsAuthorized(filterContext) == false)
            {
                HandleUnauthorizedRequest(filterContext);
            }

        }


        protected virtual bool IsAuthorized(AuthorizationContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["AUTH_PTEmail"] == null)
                {

                    string strEmail = HttpContext.Current.User.Identity.Name;
                    var r = Utility.AddADAccountClick(strEmail);
                    filterContext.Controller.ViewBag.TenantId = r;

                    List<oMenuAccess> authobj = null;
                    HttpCookie mailCookie = new HttpCookie("AUTH_PTEmail");
                    mailCookie.Value = r.value[0].userPrincipalName;
                    mailCookie.Expires = DateTime.Now.AddDays(1d);
                    mailCookie.Domain = "Admin";
                    HttpContext.Current.Request.Cookies.Add(mailCookie);

                    HttpCookie mailCookies = new HttpCookie("AUTHADMIN");

                    mailCookies.Expires = DateTime.Now.AddDays(1d);
                    HttpContext.Current.Request.Cookies.Add(mailCookies);
                    mailCookies.Value = "";

                    if (CheckIsAdminUser(r.value[0].userPrincipalName))
                    {
                        mailCookies.Value = "Admin";
                    }
                    // if (r.value[0].userPrincipalName.ToLower() != "pt.admin@customeranalytics.com")
                    if (mailCookies.Value != "Admin")
                    {
                        authobj = Utility.getRoleAccessbyEmailID(r.value[0].userPrincipalName);

                        if (authobj != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;

                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsAuthorizedPage(int authID)
        {
            if (HttpContext.Current.Request.Cookies["AUTH_PTEmail"] != null)
            {
                oMenuAccess authobj = null;
                string email = HttpContext.Current.Request.Cookies["AUTH_PTEmail"].Value;
                if (CheckIsAdminUser(email))
                {
                    return true;
                }
                else
                {
                    authobj = Utility.getRoleAccessbyEmailID(email).Where(x => x.MenuId == authID && x.roAccessgrant).FirstOrDefault();

                    if (authobj != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;

                    }
                }
            }
            return false;

        }
        [HttpPost]
        public bool CheckIsAdminUser(string Email)
        {
            try
            {
                string query = "?Email=" + HttpUtility.UrlEncode(Email);
                AdminUsers adminUsers = CABusinessProjectTrackingAPIClient.GetHttpResponse<AdminUsers>(query, CABPTMethodConstants.CheckIsAdmin);
                if (adminUsers.IsActive == false)
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
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "UnAuthorized" }));
        }

    }
}