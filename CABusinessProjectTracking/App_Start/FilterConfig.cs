using BusinessProjectTracking.Filters;
using System.Web;
using System.Web.Mvc;

namespace BusinessProjectTracking
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           
        }
    }

}
