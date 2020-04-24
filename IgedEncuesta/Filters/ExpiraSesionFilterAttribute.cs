using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Autenticacion;


namespace IgedEncuesta.Filters
{
    public class ExpiraSesionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string rutaFinSesion = ConfigurationManager.AppSettings["CierreSesion"].ToString();

            if (HttpContext.Current.Session["TknApp"] == null) // Verifica que ya exista un token para la aplicacion
            {
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.HttpContext.Response.Redirect(rutaFinSesion, true);
                return;
            }
            else
            {
                List<Token> tokens = new List<Token>();
                Token objToken = new Token();


                // Gets object from session
                string userId, idPortal;
                userId = HttpContext.Current.Session["UserIdApp"].ToString();
                idPortal = HttpContext.Current.Session["IdPortal"].ToString();

                tokens = objToken.consultarTokenAplicacionPadre(userId, idPortal);
                if (!tokens.Any(x => x.idAplicacion == idPortal))
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.Redirect(rutaFinSesion, true);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}