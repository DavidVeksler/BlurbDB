using System.Web;
using System.Web.Optimization;

namespace E1Blurbs.Web.UI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/basic").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-form.js",
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/koUtilities").Include(
                         "~/Scripts/app/utilities/knockout-2.3.0.js",
                         "~/Scripts/app/utilities/blurbs*"));

             bundles.Add(new ScriptBundle("~/bundles/blurbs").Include(
                         "~/Scripts/app/blurbs*",
                         "~/Scripts/animatescroll.js"));

            //bundles.Add(new ScriptBundle("~/bundles/import").Include(
            //            "~/Scripts/app/import*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/sitecss").Include(
                        "~/Content/css/bootstrap-min.css",
                        "~/Content/css/bootstrap-theme-min.css",
                        "~/Content/css/bootstrap-select.css",
                        "~/Content/css/site.css",
                        "~/Content/css/site-responsive.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}