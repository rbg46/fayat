using System.Web.Optimization;

namespace Fred.Web.App_Start.Bundles
{
    public class VendorBundleFactory : BundleFactory
    {
        public VendorBundleFactory(BundleCollection bundles)
          : base(bundles)
        {
            JsBundleName("~/vendor/js");
            Js("~/vendor/bower_components/modernizr/modernizr.js", // modernizr
                                                                   // jquery
              "~/vendor/bower_components/jquery/jquery.min.js",
              "~/vendor/bower_components/jquery-ui/ui/minified/jquery-ui.custom.min.js",
              // Bootstrap
              "~/vendor/bower_components/bootstrap/dist/js/bootstrap.min.js",
              "~/vendor/bower_components/respond/respond.min.js",
              "~/vendor/bower_components/bootstrap-colorselector/dist/bootstrap-colorselector.min.js",
              // angular
              "~/vendor/bower_components/angular/angular.min.js",
              "~/vendor/bower_components/angular-resource/angular-resource.min.js",
              "~/vendor/bower_components/angular-route/angular-route.min.js",
              "~/vendor/bower_components/angular-animate/angular-animate.min.js",
              "~/vendor/bower_components/angular-sanitize/angular-sanitize.min.js",
              "~/vendor/bower_components/angular-bootstrap/ui-bootstrap.min.js",
              "~/vendor/bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js",
              "~/vendor/bower_components/angular-modal-service/dst/angular-modal-service.min.js",
              "~/vendor/bower_components/angular-ui-notification/dist/angular-ui-notification.min.js",
              "~/vendor/bower_components/ngstorage/ngStorage.min.js",
              "~/vendor/bower_components/moment/min/moment-with-locales.min.js",
              "~/vendor/bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",

              // bootbox
              "~/vendor/bower_components/bootbox/bootbox.js",
              "~/vendor/bower_components/ngBootbox/dist/ngBootbox.min.js",
              // progress bar
              "~/vendor/bower_components/ngprogress/build/ngprogress.min.js",
              // Google Map
              "~/vendor/bower_components/ngmap/build/scripts/ng-map.min.js",
              //mask sur les inputs text
              "~/vendor/bower_components/angular-ui-mask/dist/mask.min.js"
              );

            CssBundleName("~/vendor/css");
            Css("~/vendor/bower_components/bootstrap/dist/css/bootstrap.min.css",
               "~/vendor/bower_components/bootstrap-colorselector/dist/bootstrap-colorselector.min.css",
              "~/vendor/bower_components/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css",

              // angular
              "~/vendor/bower_components/angular-ui-notification/dist/angular-ui-notification.min.css",
               // progress bar
               "~/vendor/bower_components/ngprogress/ngProgress.css");
        }
    }
}