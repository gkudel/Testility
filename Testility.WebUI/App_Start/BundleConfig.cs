using System.Web;
using System.Web.Optimization;

namespace Testility.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/ide").Include(
                        ));

            bundles.Add(new StyleBundle("~/Content/ide").Include(
                    "~/Scripts/codemirror-2.37/lib/codemirror.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular-ui").Include(
                    "~/Scripts/angular/framework/angular.js",
                    "~/Scripts/angular/framework/angular-animate.js",
                    "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/solution").Include(
                "~/Scripts/angular/main.js",
                "~/Scripts/codemirror-2.37/lib/codemirror.js",
                "~/Scripts/codemirror-2.37/mode/clike/clike.js",
                "~/Scripts/angular/solution/solution.js",
                "~/Scripts/angular/solution/controllers/solutioncontroller.js",
                "~/Scripts/browser/browser.js"));

            bundles.Add(new ScriptBundle("~/bundles/unit-test").Include(
                   "~/Scripts/angular/main.js",
                   "~/Scripts/browser/browser.js",
                   "~/Scripts/angular/unit-test/unit-test.js"));

        }
    }
}
