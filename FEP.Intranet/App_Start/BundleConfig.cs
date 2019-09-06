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
            BundleTable.EnableOptimizations = false;
            //var stylebundle = new StyleBundle("~/core/css")
            //   .Include("~/dist/assets/css/material-icons.css", new CssRewriteUrlTransform())
            //   .Include("~/dist/assets/css/material-icons.rtl.css", new CssRewriteUrlTransform())
            //   .Include("~/dist/assets/css/fontawesome.css", new CssRewriteUrlTransform())
            //   .Include("~/lib/line-awesome/css/line-awesome.min.css", new CssRewriteUrlTransform())
            //   .Include("~/lib/animate/animate.min.css", new CssRewriteUrlTransform())
            //   .Include("~/dist/assets/css/flatpickr.css", new CssRewriteUrlTransform())
            //   .Include("~/dist/assets/css/flatpickr.rtl.css", new CssRewriteUrlTransform())
            //   .Include("~/lib/owlcarousel/assets/owl.carousel.min.css", new CssRewriteUrlTransform())
            //   .Include("~/css/slick.css", new CssRewriteUrlTransform())
            //   .Include("~/css/jssocials.css", new CssRewriteUrlTransform())
            //   .Include("~/dist/assets/css/app.css", new CssRewriteUrlTransform())
            //   .Include("~/css/base.css", new CssRewriteUrlTransform())
            //   .Include("~/css/style.css", new CssRewriteUrlTransform())
            //   ;
            
            //stylebundle.Orderer = new NonOrderingBundleOrderer();
            //bundles.Add(stylebundle);

            var stylebundle = new StyleBundle("~/auth/style")
               .Include("~/dist/assets/vendor/perfect-scrollbar.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/material-icons.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/material-icons.rtl.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/fontawesome.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/fontawesome.rtl.css", new CssRewriteUrlTransform())
               .Include("~/lib/line-awesome/css/line-awesome.min.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/app.css", new CssRewriteUrlTransform())
               .Include("~/css/select2.min.css", new CssRewriteUrlTransform())
               .Include("~/css/base.css", new CssRewriteUrlTransform())
               .Include("~/css/style.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/flatpickr.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/flatpickr.rtl.css", new CssRewriteUrlTransform())
               ;
            
            stylebundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(stylebundle);

            stylebundle = new StyleBundle("~/layout1/style")
               .Include("~/dist/assets/vendor/perfect-scrollbar.css", new CssRewriteUrlTransform())

               .Include("~/dist/assets/css/material-icons.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/material-icons.rtl.css", new CssRewriteUrlTransform())

               .Include("~/dist/assets/css/fontawesome.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/fontawesome.rtl.css", new CssRewriteUrlTransform())

               .Include("~/lib/line-awesome/css/line-awesome.min.css", new CssRewriteUrlTransform())
               
               .Include("~/css/select2.min.css", new CssRewriteUrlTransform())

               .Include("~/dist/assets/css/flatpickr.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/flatpickr.rtl.css", new CssRewriteUrlTransform())

               .Include("~/dist/assets/css/app.css", new CssRewriteUrlTransform())
               .Include("~/dist/assets/css/app.rtl.css", new CssRewriteUrlTransform())

               .Include("~/dist/assets/vendor/datatables.css", new CssRewriteUrlTransform())

               .Include("~/css/base.css", new CssRewriteUrlTransform())
               .Include("~/css/custom-admin.css", new CssRewriteUrlTransform())               
               ;

            stylebundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(stylebundle);

            // survey
            stylebundle = new StyleBundle("~/layoutSurveyBuilder/style")
                .Include("~/dist/assets/vendor/perfect-scrollbar.css", new CssRewriteUrlTransform())

                .Include("~/dist/assets/css/material-icons.css", new CssRewriteUrlTransform())
                .Include("~/dist/assets/css/material-icons.rtl.css", new CssRewriteUrlTransform())

                .Include("~/dist/assets/css/fontawesome.css", new CssRewriteUrlTransform())
                .Include("~/dist/assets/css/fontawesome.rtl.css", new CssRewriteUrlTransform())

                .Include("~/lib/line-awesome/css/line-awesome.min.css", new CssRewriteUrlTransform())

                .Include("~/css/select2.min.css", new CssRewriteUrlTransform())

                .Include("~/dist/assets/css/flatpickr.css", new CssRewriteUrlTransform())
                .Include("~/dist/assets/css/flatpickr.rtl.css", new CssRewriteUrlTransform())

                .Include("~/dist/assets/css/app.css", new CssRewriteUrlTransform())
                .Include("~/dist/assets/css/app.rtl.css", new CssRewriteUrlTransform())

                .Include("~/dist/assets/vendor/datatables.css", new CssRewriteUrlTransform())

                .Include("~/css/base.css", new CssRewriteUrlTransform())
                .Include("~/css/custom-admin.css", new CssRewriteUrlTransform())
                ;

            stylebundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(stylebundle);

            //var scriptbundle = new ScriptBundle("~/core/js")
            //    .Include("~/dist/assets/vendor/jquery.min.js")
            //    .Include("~/dist/assets/vendor/popper.min.js")
            //    .Include("~/dist/assets/vendor/bootstrap.min.js")
            //    .Include("~/lib/easing/easing.min.js")
            //    .Include("~/dist/assets/vendor/flatpickr/flatpickr.min.js")
            //    .Include("~/dist/assets/js/flatpickr.js")
            //    .Include("~/js/jquery.simpleLoadMore.js")
            //    .Include("~/lib/wow/wow.min.js")
            //    .Include("~/js/jssocials.min.js")
            //    .Include("~/js/rellax.min.js")
            //    .Include("~/js/jquery.countTo.js")
            //    .Include("~/lib/owlcarousel/owl.carousel.min.js")
            //    .Include("~/js/slick.min.js")
            //    .Include("~/js/main.js")
            //    .Include("~/Scripts/jquery.validate.min.js")
            //    .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
            //    .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
            //    ;

            //scriptbundle.Orderer = new NonOrderingBundleOrderer();
            //bundles.Add(scriptbundle);

            var scriptbundle = new ScriptBundle("~/auth/script")
                .Include("~/dist/assets/vendor/jquery.min.js")
                .Include("~/dist/assets/vendor/popper.min.js")
                .Include("~/dist/assets/vendor/bootstrap.min.js")
                .Include("~/dist/assets/vendor/perfect-scrollbar.min.js")
                .Include("~/dist/assets/vendor/dom-factory.js")
                .Include("~/dist/assets/vendor/material-design-kit.js")
                .Include("~/dist/assets/js/app.js")
                .Include("~/dist/assets/js/hljs.js")
                .Include("~/dist/assets/js/app-settings.js")
                .Include("~/js/select2.min.js")
                .Include("~/dist/assets/vendor/flatpickr/flatpickr.min.js")
                .Include("~/dist/assets/js/flatpickr.js")
                .Include("~/js/script-admin.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js");

            scriptbundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(scriptbundle);
            
            scriptbundle = new Bundle("~/layout1/script")
                .Include("~/dist/assets/vendor/jquery.min.js")

                .Include("~/dist/assets/vendor/popper.min.js")
                .Include("~/dist/assets/vendor/bootstrap.min.js")

                .Include("~/dist/assets/vendor/perfect-scrollbar.min.js")

                .Include("~/dist/assets/vendor/dom-factory.js")
                .Include("~/dist/assets/vendor/material-design-kit.js")

                .Include("~/dist/assets/js/app.js")

                .Include("~/dist/assets/js/hljs.js")               
                .Include("~/js/select2.min.js")

                .Include("~/dist/assets/js/toggle-check-all.js")
                .Include("~/dist/assets/js/check-selected-row.js")

                .Include("~/dist/assets/vendor/flatpickr/flatpickr.min.js")
                .Include("~/dist/assets/js/flatpickr.js")
                
                .Include("~/dist/assets/vendor/datatables.js")
                
                .Include("~/js/script-admin.js")

                .Include("~/Scripts/moment.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js");

            scriptbundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(scriptbundle);

            // survey
            scriptbundle = new Bundle("~/layoutSurveyBuilder/script")
                .Include("~/dist/assets/vendor/jquery.min.js")

                .Include("~/dist/assets/vendor/popper.min.js")
                .Include("~/dist/assets/vendor/bootstrap.min.js")

                .Include("~/dist/assets/vendor/perfect-scrollbar.min.js")

                .Include("~/dist/assets/vendor/dom-factory.js")
                .Include("~/dist/assets/vendor/material-design-kit.js")

                .Include("~/dist/assets/js/app.js")

                .Include("~/dist/assets/js/hljs.js")
                .Include("~/js/select2.min.js")

                .Include("~/dist/assets/js/toggle-check-all.js")
                .Include("~/dist/assets/js/check-selected-row.js")

                .Include("~/dist/assets/vendor/flatpickr/flatpickr.min.js")
                .Include("~/dist/assets/js/flatpickr.js")

                .Include("~/dist/assets/vendor/datatables.js")

                .Include("~/js/script-admin.js")

                .Include("~/Scripts/moment.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js");

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
