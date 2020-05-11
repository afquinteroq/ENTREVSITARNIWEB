using System.Web.Mvc;
using AdministracionInstrumentos;
using log4net;


namespace IgedEncuesta.Controllers
{
    public class CargueConstanciaSAAHFirmadaController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger("Web");
        //
        // GET: /CargueConstanciaSAAHFirmada/
        public ActionResult CargueConstanciaSAAHFirmada()
        {
            return View();
        }
        /*public void cargarConstanciaFirmadaSAAh(string tipopersona)
        {

            Encuesta objSesion = new Encuesta();
            try
            {
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();


                gic_adminconfig adminConfig = new gic_adminconfig();
                List<gic_adminconfig> _Config = new List<gic_adminconfig>();
                _Config = adminConfig.GetAdminConfiguracion("path.constanciasfirmadas");
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImageTutor"];
                Guid g = Guid.NewGuid();
                string sguid = g.ToString();
                string nombreArchivo = sguid + ".pdf";
                gic_ArchivoColilla datosSol = new gic_ArchivoColilla();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(_Config.First().ADMINCFG_VALUE, nombreArchivo);
                    datosSol.arc_url = fileSavePath;
                    datosSol.usu_UsuarioCreacion = usuario;
                    datosSol.tipopersona = tipopersona;

                    var guid = datosSol.insertaArchivoSoportes(datosSol, sguid);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "SYSGUID", guid);

                    string sys = objSesion.getValorCampoSesion("SYSGUID", userIdApp);

                    httpPostedFile.SaveAs(fileSavePath);
                }

            }
            catch (Exception e)
            {
                log.Error("EncuestaController / cargarSoporte , Error: " + e.Message.ToString());
            }


        }*/

        [HttpGet]
        [ActionName("conteoConstanciaFirmada")]
        public JsonResult conteoConstanciaFirmada(string codHogar)
        {
            int codigo = 0;
            gic_Hogar hogar = new gic_Hogar();            
            codigo = hogar.existeConstanciaFirmada(codHogar);
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }


    }
}
