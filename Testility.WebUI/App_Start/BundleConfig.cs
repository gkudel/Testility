using System.Web;
using System.Web.Optimization;

namespace Testility.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bower_components").Include(
                // bower:js
                "~/bower_components/modernizr/modernizr.js",
                "~/bower_components/jquery/dist/jquery.js",
                "~/bower_components/angular/angular.js",
                "~/bower_components/angular-animate/angular-animate.js",
                "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.js",
                "~/bower_components/codemirror/lib/codemirror.js",
                "~/bower_components/angular-ui-codemirror/ui-codemirror.js",
                "~/bower_components/bootstrap/dist/js/bootstrap.js",
                "~/bower_components/iOS-Overlay/js/iosOverlay.js",
                "~/bower_components/jquery.validation/dist/jquery.validate.js",
                "~/bower_components/Microsoft.jQuery.Unobtrusive.Validation/jquery.validate.unobtrusive.js",
                "~/bower_components/respond/dest/respond.src.js",
                "~/bower_components/lodash/lodash.js",
                "~/bower_components/restangular/dist/restangular.js",
                // endbower
                "~/bower_components/iOS-Overlay/js/spin.min.js",
                "~/bower_components/codemirror/mode/clike/clike.js",
                "~/bower_components/codemirror/addon/edit/matchbrackets.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/iosOverlay.css",
                      "~/Content/codemirror-3.0/codemirror.css",
                      "~/Content/angular-block-ui.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/testility").Include(
                    "~/Scripts/app/testility.module.js",
                    "~/Scripts/app/testility.value.js",
                    "~/Scripts/app/testility.config.js",
                    "~/Scripts/app/browser/browser.module.js",
                    "~/Scripts/app/browser/browser.directive.js",
                    "~/Scripts/app/browser/browser.service.js",
                    "~/Scripts/app/browser/browser.controller.js",
                    "~/Scripts/app/validation/validation.js",
                    "~/Scripts/app/messaging/messaging.js",
                    "~/Scripts/app/dialogbox/dialogbox.js",
                    "~/Scripts/app/spiner/spiner.js"));

            bundles.Add(new ScriptBundle("~/bundles/solution").Include(
                "~/Scripts/app/solution/services/solutionservice.js",
                "~/Scripts/app/solution/services/setupservice.js",
                "~/Scripts/app/solution/services/unittestservice.js",
                "~/Scripts/app/solution/controllers/solutioncontroller.js"));
        }
    }
}
