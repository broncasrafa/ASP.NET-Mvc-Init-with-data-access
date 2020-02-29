using System.Web;
using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/assets/scripts/libs/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/assets/scripts/libs/jquery.validate/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Content/assets/scripts/libs/jquery.unobtrusive-ajax/jquery.unobtrusive-ajax*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/assets/scripts/libs/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/assets/scripts/libs/bootstrap3/bootstrap.js",
                      "~/Content/assets/scripts/libs/jquery.blockUI/jquery.blockUI.js",
                      "~/Content/assets/scripts/libs/toastr/toastr.js",
                      "~/Content/assets/scripts/app/functions.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/assets/css/bootstrap3/bootstrap.css",
                      "~/Content/assets/css/app/site.css",
                      "~/Content/assets/css/toastr/toastr.css",
                      "~/Content/assets/css/jquery.blockUI/jquery.blockUI.css"));
        }
    }
}
