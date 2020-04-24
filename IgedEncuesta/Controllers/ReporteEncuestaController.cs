using AdministracionInstrumentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{

    public class ReporteEncuestaController : Controller
    {
        //
        // GET: /ReporteEncuesta/
        ~ReporteEncuestaController()
        {

        }

        public ActionResult ReporteEncuesta()
        {
            string Usuario = string.Empty;
            Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
            gic_Hogar hogar = new gic_Hogar();
            List<gic_ReporteMiembros> lista = new List<gic_ReporteMiembros>();
            lista = hogar.get_reporteMiembrosXcodigo(Usuario);
            ViewBag.EncuestaActiva = encuestaActiva();
            return View(lista);
        }
       

        public FileResult DescargarSoporte(string ruta, string codigoHogar)
        {
            string contentType = "application/pdf";
            return File(ruta, contentType, codigoHogar + ".pdf");
        }

        public string encuestaActiva()
        {
            string codigo =string.Empty;
            gic_Hogar hogar = new gic_Hogar();
            string Usuario = string.Empty;
            Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
            codigo = hogar.encuestaActiva(Usuario);
            return codigo;
        }


    }
}
