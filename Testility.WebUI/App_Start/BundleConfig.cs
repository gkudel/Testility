using System.Web;
using System.Web.Optimization;

namespace Testility.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bower_components")
                // bower:js                                                                                                                                               
                .Include("~/bower_components/modernizr/modernizr.js")
                .Include("~/bower_components/jquery/dist/jquery.js")
                .Include("~/bower_components/angular/angular.js")
                .Include("~/bower_components/angular-animate/angular-animate.js")
                .Include("~/bower_components/angular-bootstrap/ui-bootstrap-tpls.js")
                .Include("~/bower_components/angular-mocks/angular-mocks.js")
                .Include("~/bower_components/codemirror/lib/codemirror.js")
                .Include("~/bower_components/codemirror/mode/clike/clike.js")
                .Include("~/bower_components/codemirror/addon/edit/matchbrackets.js")
                .Include("~/bower_components/angular-ui-codemirror/ui-codemirror.js")
                .Include("~/bower_components/bootstrap/dist/js/bootstrap.js")
                .Include("~/bower_components/bootstrap-filestyle/src/bootstrap-filestyle.js")
                .Include("~/bower_components/iOS-Overlay/js/iosOverlay.js")
                .Include("~/bower_components/iOS-Overlay/js/spin.min.js")
                .Include("~/bower_components/flow.js/dist/flow.js")
                .Include("~/bower_components/ng-flow/dist/ng-flow.js")
                .Include("~/bower_components/respond/dest/respond.src.js")
                .Include("~/bower_components/lodash/lodash.js")
                .Include("~/bower_components/restangular/dist/restangular.js")
                // endbower
                );

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css")
                        // bower:css                                                                                                               
                        
                        .Include("~/bower_components/codemirror/lib/codemirror.css")
                        .Include("~/bower_components/bootstrap/dist/css/bootstrap.css")
                        .Include("~/bower_components/iOS-Overlay/css/iosOverlay.css")
                        // endbower
                        );

            bundles.Add(new ScriptBundle("~/bundles/jQueryValidate")
                .Include("~/bower_components/jquery.validation/dist/jquery.validate.js")
                .Include("~/bower_components/Microsoft.jQuery.Unobtrusive.Validation/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/testility").Include(
                    "~/Scripts/app/testility.module.js",
                    "~/Scripts/app/testility.value.js",
                    "~/Scripts/app/testility.config.js",
                    "~/Scripts/app/testility.filter.js", 
                    "~/Scripts/app/util/util.module.js",
                    "~/Scripts/app/util/util.directive.js",
                    "~/Scripts/app/browser/browser.module.js",
                    "~/Scripts/app/browser/browser.directive.js",
                    "~/Scripts/app/browser/browser.service.js",
                    "~/Scripts/app/browser/browser.controller.js",
                    "~/Scripts/app/dialogbox/dialogbox.module.js",
                    "~/Scripts/app/dialogbox/dialogbox.directive.js",
                    "~/Scripts/app/dialogbox/dialogbox.service.js",
                    "~/Scripts/app/dialogbox/dialogbox.controller.js",
                    "~/Scripts/app/messaging/messaging.module.js",
                    "~/Scripts/app/messaging/messaging.directive.js",
                    "~/Scripts/app/messaging/messaging.service.js",
                    "~/Scripts/app/validation/data.validation.module.js",
                    "~/Scripts/app/validation/data.validation.directive.js",
                    "~/Scripts/app/spiner/spiner.module.js",
                    "~/Scripts/app/spiner/spiner.service.js",
                    "~/Scripts/app/solution/solution.module.js",
                    "~/Scripts/app/solution/solution.service.js",
                    "~/Scripts/app/solution/solution.controller.js",
                    "~/Scripts/app/references/references.module.js",
                    "~/Scripts/app/references/references.controller.js"));
        }
    }
}
