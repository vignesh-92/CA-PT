using System.Web;
using System.Web.Optimization;

namespace BusinessProjectTracking
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
          "~/Content/bootstrap.min.css",
		  "~/Content/bootstrap-multiselect.css",
          //"~/Content/bootstrap-datetimepicker.css",
          "~/Content/datepicker3.min.css",
          "~/Content/font-awesome.min.css",
          "~/Content/jquery.dataTables.min.css",          
          "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/jquery.tabletojson.js",
                        "~/Scripts/jquery.tabletojson.min.js"
                        //"~/Scripts/dataTables.responsive.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                 "~/Scripts/Common.js"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
					  "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/bootstrap-datepicker.min.js",                      
                      "~/Scripts/script.js",
                      "~/Scripts/respond.js"));


        }
    }
}
