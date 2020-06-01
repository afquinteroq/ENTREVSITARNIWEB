using AdministracionInstrumentos;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{
    public class CargueSoporteController : Controller
    {
        ~CargueSoporteController()
        {

        }


        public ActionResult CargueSoporte()
        {
            return View();
        }

        [HttpGet]
        [ActionName("idEncuesta")]
        public JsonResult idEncuesta(string codHogar)
        {

            string codigo = string.Empty;
            gic_Hogar hogar = new gic_Hogar();

            codigo = hogar.get_idEncuesta(codHogar);
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("conteoSoporte")]
        public JsonResult conteoSoporte(string codHogar)
        {
            int codigo = 0;
            gic_Hogar hogar = new gic_Hogar();
            codigo = hogar.existeColilla(codHogar);
            
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarSoporte(string codigoHogar)
        {
            gic_adminconfig adminConfig = new gic_adminconfig();
            List<gic_adminconfig> _Config = new List<gic_adminconfig>();
            _Config = adminConfig.GetAdminConfiguracion("path.colilla");
            string nombreArchivo = codigoHogar + ".pdf";
            string ruta =  Path.Combine(_Config.First().ADMINCFG_VALUE, nombreArchivo);;
            string contentType = "application/pdf";
            return File(ruta, contentType, codigoHogar + ".pdf");
        }
    }
}
