using System.Web.Optimization;

namespace Fred.ImportExport.Web
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.timeago.js",
                        "~/Scripts/jquery.timeago.fr-short.js"));

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
                      "~/Content/jquery-ui.css",
                      "~/Content/bootstrap-paper.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/reprisedonnees.css").Include(
             "~/vendor/bower_components/tableexport.js/dist/js/tableexport.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/reprisedonnees.js").Include(
              "~/vendor/bower_components/js-xlsx/dist/xlsx.core.min.js",
              "~/vendor/bower_components/file-saverjs/FileSaver.min.js",
              "~/vendor/bower_components/tableexport.js/dist/js/tableexport.min.js",
              "~/Scripts/fredie/exportexcel/reprise-donnees.js"));
        }
    }
}
