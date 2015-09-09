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
                        "~/Scripts/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/lib/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap.js",
                      "~/Scripts/lib/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/iosOverlay.css",
                      "~/Content/codemirror-3.0/codemirror.css",
                      "~/Content/angular-block-ui.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/lib/angular/angular.js",
                    "~/Scripts/lib/angular/angular-animate.js",
                    "~/Scripts/lib/angular-ui/angular-ui.js",
                    "~/Scripts/lib/angular-ui/ui-bootstrap-tpls.js",
                    "~/Scripts/lib/angular-ui/angular-block-ui.js",
                    "~/Scripts/lib/iosOverlay.js",
                    "~/Scripts/lib/spin.min.js",
                    "~/Scripts/lib/angular/restangular.js",
                    "~/Scripts/lib/angular/lodash.js",
                    "~/Scripts/app/testility.module.js",
                    "~/Scripts/app/testility.value.js",
                    "~/Scripts/app/testility.config.js",
                    "~/Scripts/app/validation/validation.js",
                    "~/Scripts/app/browser/browser.module.js",
                    "~/Scripts/app/browser/browser.directive.js",
                    "~/Scripts/app/browser/browser.service.js",
                    "~/Scripts/app/browser/browser.controller.js",
                    "~/Scripts/app/messaging/messaging.js",
                    "~/Scripts/app/dialogbox/dialogbox.js",
                    "~/Scripts/app/spiner/spiner.js"));

            bundles.Add(new ScriptBundle("~/bundles/solution").Include(
                "~/Scripts/lib/codemirror-3.0/codemirror.js",
                "~/Scripts/lib/codemirror-3.0/util/matchbrackets.js",
                "~/Scripts/lib/codemirror-3.0/mode/clike.js",
                "~/Scripts/app/solution/services/solutionservice.js",
                "~/Scripts/app/solution/services/setupservice.js",
                "~/Scripts/app/solution/services/unittestservice.js",
                "~/Scripts/app/solution/controllers/solutioncontroller.js"));
        }
    }
}
