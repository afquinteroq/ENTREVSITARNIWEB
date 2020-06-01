using IgedEncuesta.Models.mdlGenerico;
using System.Collections.Generic;

using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{
    public class CargaMiebroXHogarController : Controller
    {
        //
        // GET: /CargaMiebroXHogar/

        public ActionResult Index()
        {

            return View();
        }


        public ActionResult obtMiembroXHogar(string codHogar)
        {

            if (!codHogar.Equals("") && codHogar != null){
                GIC_REPORTEXHOGAR hogar = new GIC_REPORTEXHOGAR();
                List<GIC_REPORTEXHOGAR> lista = new List<GIC_REPORTEXHOGAR>();
                lista = hogar.getReporteXHogar(codHogar);
                ViewBag.resultado = lista[0].nombre_completo;
                return PartialView("_MiembrosXHogar", lista);
            }

            return View();

        }

        }
}
