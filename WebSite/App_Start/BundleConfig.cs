using System.Web.Optimization;

namespace WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region NormalLayout
            bundles.Add(new ScriptBundle("~/Scripts/basic").Include("~/Scripts/jquery-2.2.2.min.js")    
                                                                       .Include("~/Scripts/jquery-ui-{version}.js")
                                                                       .Include("~/Scripts/jquery.validate*")
                                                                       .Include("~/Scripts/modernizr-{version}.js")
                                                                       .Include("~/Scripts/bootstrap.js")
                                                                       .Include( "~/Scripts/jquery.unobtrusive-ajax.min.js")
                                                                       .Include("~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/css/main").Include("~/Content/css/Site.css"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include("~/Content/css/bootstrap.min.css"));
                                               
            #endregion


            #region EmptyLayout

            bundles.Add(new StyleBundle("~/css/empty").Include("~/Content/css/bootstrap.css")
                                                     .Include("~/Content/css/EmptyLayout.css"));

            bundles.Add(new ScriptBundle("~/Scripts/empty").Include("~/Scripts/jquery-2.2.2.min.js")
                                                            .Include("~/Scripts/bootstrap.min.js")
                                                          .Include("~/Scripts/EmptyLayoutScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*")
                         .Include("~/Scripts/jsjquery.validate.unobtrusive.bootstrap.tooltip.js"));

            #endregion
        }
    }
}
