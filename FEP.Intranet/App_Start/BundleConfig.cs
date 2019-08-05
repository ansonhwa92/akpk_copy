using System.Web;
using System.Web.Optimization;
using System.Collections.Generic;

namespace FEP.Intranet
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            var stylebundle = new StyleBundle("~/core/css")
               .Include("~/assets/vendor/perfect-scrollbar.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/material-icons.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/material-icons.rtl.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/fontawesome.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/fontawesome.rtl.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/app.css", new CssRewriteUrlTransform())
               .Include("~/assets/css/app.rtl.css", new CssRewriteUrlTransform())
               ;


            stylebundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(stylebundle);
            
            var scriptbundle = new ScriptBundle("~/core/js")
                .Include("~/assets/vendor/jquery.min.js")
                .Include("~/assets/vendor/popper.min.js")
                .Include("~/assets/vendor/bootstrap.min.js")
                .Include("~/assets/vendor/perfect-scrollbar.min.js")
                .Include("~/assets/vendor/dom-factory.js")
                .Include("~/assets/vendor/material-design-kit.js")
                .Include("~/assets/js/app.js")
                .Include("~/assets/js/hljs.js")
                .Include("~/assets/js/app-settings.js")
                .Include("~/assets/js/settings.js")
                .Include("~/assets/vendor/moment.min.js")
                .Include("~/assets/vendor/moment-range.min.js")
                .Include("~/assets/vendor/Chart.min.js")
                .Include("~/assets/js/chartjs-rounded-bar.js")
                .Include("~/assets/js/chartjs.js")
                .Include("~/assets/js/bootstrap-notify.min.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
                ;

            scriptbundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(scriptbundle);

        }

    }

    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
