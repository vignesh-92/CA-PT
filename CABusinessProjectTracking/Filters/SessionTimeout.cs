using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace BusinessProjectTracking.Filters
{
    public class SessionTimeout : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;

            if (controller != null)
            {
                //if (session != null && session["authstatus"] == null)
                //{
                //    filterContext.Result =
                //           new RedirectToRouteResult(
                //               new RouteValueDictionary{{ "controller", "Home" },
                //                          { "action", "SignIn" } });

                //   // filterContext.Result = new RedirectToRouteResult("SystemLogin", routeValues);
                //}
            }

            base.OnActionExecuting(filterContext);     
        }
    }
}