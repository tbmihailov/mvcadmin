using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MvcAdminResearch.Areas.MvcAdmin
{
    internal static class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/MvcAdmin/bundles/jquery").Include(
                       "~/Areas/MvcAdmin/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Areas/MvcAdmin/bundles/jqueryui").Include(
                        "~/Areas/MvcAdmin/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/Areas/MvcAdmin/bundles/jqueryval").Include(
                        "~/Areas/MvcAdmin/Scripts/jquery.unobtrusive*",
                        "~/Areas/MvcAdmin/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Areas/MvcAdmin/bundles/modernizr").Include(
                        "~/Areas/MvcAdmin/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Areas/MvcAdmin/Content/themes/base/css").Include(
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.core.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.resizable.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.selectable.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.accordion.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.button.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.dialog.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.slider.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.tabs.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Areas/MvcAdmin/Content/themes/base/jquery.ui.theme.css"));

            //bootstrap bundles
            bundles.Add(new ScriptBundle("~/Areas/MvcAdmin/bundles/bootstrap").Include("~/Areas/MvcAdmin/Scripts/bootstrap*"));
            bundles.Add(new StyleBundle("~/Areas/MvcAdmin/Content/bootstrap").Include("~/Areas/MvcAdmin/Content/bootstrap.css", "~/Areas/MvcAdmin/Content/bootstrap-responsive.css", "~/Areas/MvcAdmin/Content/bootstrap-custom.css"));
        }
    }
}