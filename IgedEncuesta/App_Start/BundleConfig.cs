using System.Web.Optimization;

namespace IgedEncuesta
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/loading").Include(
                      "~/Scripts/loadingoverlay.js",
                      "~/Scripts/loadingoverlay.min.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/general").Include(
                        "~/Scripts/general.js"));
            

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/Jquery/css").Include(
                    "~/Content/themes/Jquery/jquery-ui.css",
                    "~/Content/themes/Jquery/jquery-ui.min.css",
                    "~/Content/themes/Jquery/jquery-ui.structure.css",
                    "~/Content/themes/Jquery/jquery-ui.structure.min.css",
                    "~/Content/themes/Jquery/jquery-ui.theme.css",
                    "~/Content/themes/Jquery/jquery-ui.theme.min.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                //bundles.Add(new StyleBundle("~/Styles5/css").Include(
                            "~/Content/themes/base/core.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                //"~/Content/themes/base/tooltip.css",
                        "~/Content/themes/base/tabs.css"

            ));
        }
    }
}