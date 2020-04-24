using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdministracionInstrumentos;


namespace IgedEncuesta.Controllers
{
    public class CerrarEncuestaController : Controller
    {
        //
        // GET: /CerrarEncuesta/

        public ActionResult CerrarEncuesta()
        {
            return View();
        }

        [HttpGet]
        [ActionName("cerrar")]

        public JsonResult cerrar(string codHogar)
        {
            gic_Hogar gic_hogar = new gic_Hogar();
            string resultado = gic_hogar.cerrarEncuesta(codHogar);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        

    }
}
