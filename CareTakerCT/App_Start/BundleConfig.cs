﻿using System.Web;
using System.Web.Optimization;

namespace CareTakerCT
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new Bundle("~/bundles/calendar").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/fullcalendar.js",
                      "~/Scripts/calendar.js"));

            bundles.Add(new Bundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new Bundle("~/bundles/chart.js").Include(
                      "~/Scripts/moment.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/fullcalendar.css"));


            bundles.Add(new Bundle("~/bundles/map").Include(
          "~/Scripts/map.js"));
        }
    }
}
