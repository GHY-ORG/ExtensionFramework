using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GHY_SSO
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Resources/Bootstrap/css/bootstrap.css",
                "~/Resources/Site/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Resources/JQuery/jquery-1.11.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Resources/Bootstrap/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/ie8").Include(
                "~/Resources/Bootstrap/js/modernizr-2.6.2.js",
                "~/Resources/Bootstrap/js/respond.js"));
        }
    }
}