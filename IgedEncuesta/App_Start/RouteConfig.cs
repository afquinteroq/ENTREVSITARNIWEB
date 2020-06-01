using System.Web.Mvc;
using System.Web.Routing;

namespace IgedEncuesta
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //descomentar defaults: new { controller = "Sesion", action = "Inicio", id = UrlParameter.Optional } para publicar

            //defaults: new { controller = "Sesion", action = "Inicio", id = UrlParameter.Optional }
            //defaults: new { controller = "Encuesta", action = "Encuesta", id = UrlParameter.Optional }
            //defaults: new { controller = "GeneracionPdfs", action = "GeneracionPdfs", id = UrlParameter.Optional }
            //defaults: new { controller = "ReporteEncuesta", action = "ReporteEncuesta", id = UrlParameter.Optional }
            //defaults: new { controller = "CargueSoporte", action = "CargueSoporte", id = UrlParameter.Optional }
            //defaults: new { controller = "GeneracionPdfs", action = "GeneracionTest", id = UrlParameter.Optional }
            //defaults: new { controller = "Sesion", action = "Inicio", id = UrlParameter.Optional }
            defaults: new { controller = "ConformacionHogar", action = "Inicio", id = UrlParameter.Optional }
            );
        }
    }
}