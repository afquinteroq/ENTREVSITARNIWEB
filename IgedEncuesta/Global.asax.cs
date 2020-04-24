using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autenticacion;
using AccesoDatos;
using ObjetosTipos;
using System.Configuration;

namespace IgedEncuesta
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //void Session_End(object sender, EventArgs e)
        //{
        //    SesionAplicacion objSesionAplicacion = new SesionAplicacion();
        //    string userIdApp, app, tknApp;
        //    List<Parametros> param = new List<Parametros>();

        //    string rutaFinSesion = ConfigurationManager.AppSettings["CierreSesion"].ToString();

        //    userIdApp = TempData["UserIdApp"].ToString();
        //    app = TempData["App"].ToString();
        //    tknApp = TempData["TknApp"].ToString();

        //    Session.Clear();

        //    objSesionAplicacion.EliminarTokenAplicacion(userIdApp, app, tknApp, out param);
        //}
    }
}