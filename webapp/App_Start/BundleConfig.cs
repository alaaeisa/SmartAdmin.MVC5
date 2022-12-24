#region Using

using SmartAdminMvc.App_Helpers;
using System.Web.Optimization;

#endregion

namespace SmartAdminMvc
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();



            //bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new StyleBundle("~/content/smartadmin").Include(
               "~/Content/css/bootstrap.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/demo.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/invoice.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/lockscreen.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/smartadmin-production-plugins.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/smartadmin-production.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/smartadmin-rtl.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/smartadmin-skins.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/css/your_style.css", new CssRewriteUrlTransformWrapper()).Include("~/Content/css/font-awesome.css", new CssRewriteUrlTransformWrapper()));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.js",
                "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/scripts/bootstrap/bootstrap.min.js",
                "~/scripts/notification/SmartNotification.min.js",
                "~/scripts/smartwidgets/jarvis.widget.min.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/scripts/plugin/select2/select2.min.js",
                "~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/scripts/plugin/fastclick/fastclick.min.js",
                "~/scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                "~/scripts/plugin/moment/moment.min.js",
                "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js",
                "~/scripts/plugin/highChartCore/highcharts-custom.min.js",
                "~/scripts/plugin/highchartTable/jquery.highchartTable.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/scripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));


            BundleTable.EnableOptimizations = false;

            //BundleTable.EnableOptimizations = true;
            //#if DEBUG
            //            BundleTable.EnableOptimizations = false;
            //#else
            //                            BundleTable.EnableOptimizations = true;
            //#endif

        }
    }
}