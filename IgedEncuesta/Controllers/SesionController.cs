using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObjetosTipos;
using Autenticacion;

using System.IO;
using IgedEncuesta.Filters;

using IgedEncuesta.Models.mdlGenerico;

using AdministracionInstrumentos;
using log4net;
using log4net.Config;
using IgedEncuesta.Util;

namespace IgedEncuesta.Controllers
{
    public class SesionController : Controller
    {
        ~SesionController()
        {

        }

         private static readonly ILog log = LogManager.GetLogger("Web");
         public SesionController()
        {
            XmlConfigurator.Configure();
        }

        List<string> selectList = new List<string>();

        public ActionResult Inicio(string u, string a, string t, string na, string p)
        {

            List<NivelAcceso> coleccionNivelAcceso = new List<NivelAcceso>();
            NivelAcceso nivelAcceso = new NivelAcceso();
            SesionAplicacion objAplicacion = new SesionAplicacion();
            List<Parametros> param = new List<Parametros>();
            string tokenPortal;
            List<string> permisos = new List<string>();

            List<Token> tokens = new List<Token>();
            Token objToken = new Token();

            //comentar para probar
            Session["UserIdApp"] = ViewBag.UserIdApp = u;
            Session["App"] = ViewBag.App = a;
            Session["TknApp"] = t;
            Session["IdNivelAcceso"] = na;
            Session["IdPortal"] = p;
           //

            ViewBag.CerrarVentana = false;

            //-------------------------
            try
            {
                if (u != null || u != "")
                //   tokens = objToken.consultarTokenAplicacionPadre(Session["UserIdApp"].ToString(), p) ?? new List<Token>();
                    tokens = objToken.consultarTokenAplicacionPadre(u, p) ?? new List<Token>();

                if (tokens.Any(x => x.idAplicacion == a)) // Verifica que ya exista un token para la aplicacion
                {
                    objToken = tokens.Find(x => x.idAplicacion == a);
                    if (t == objToken.token) // SE VALIDA SI EL TOKEN DE LA APLICACION CORRESPONDE AL TOKEN EN LA BD
                    {
                        string MAC = objAplicacion.GetMACAddress();

                        objAplicacion.getActualizarTokenAplicacion(u, a, p, t, MAC, out param);

                        if (param.Find(x => x.Nombre == "p_Salida").Valor == "1") // SE VALIDA SI SE GENERO BIEN UN NUEVO TOKEN DE APLICACION
                        {
                           // Session["TknApp"] = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;
                            TempData["tk"] = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;
                            tokenPortal = tokens.Find(x => x.idAplicacion == p).token;

                            // Crea la cookie de sesion para el módulo IGED
                            var SesionIged = new HttpCookie("SesionIged");
                            SesionIged["UserIdApp"] = u;
                            SesionIged["App"] = a;
                            SesionIged["TknApp"] = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;
                            SesionIged["Fecha"] = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
                            SesionIged["IdNivelAcceso"] = na;
                            SesionIged["IdPortal"] = p;


                            ViewBag.CerrarVentana = false;
                            ViewBag.Lista = null;
                            //cargarOpciones();
                            ViewBag.BusquedaMaestro = "SI";

                            param = new List<Parametros>();
                            objAplicacion.nombresCompletosUsuario(u, out param);

                            //Session["NombresUsuario"] = param.Find(x => x.Nombre == "p_Nombres").Valor;
                            //Session["USUARIO"] = param.Find(x => x.Nombre == "p_Usuario").Valor;
                            //Session["NIVELACCESO"] = coleccionNivelAcceso;

                            SesionIged["NombresUsuario"] = param.Find(x => x.Nombre == "p_Nombres").Valor;
                            SesionIged["USUARIO"] = param.Find(x => x.Nombre == "p_Usuario").Valor;

                            coleccionNivelAcceso = nivelAcceso.consultarNivelAcceso(u, a);
                            var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccionNivelAcceso);
                            SesionIged["NIVELACCESO"] = serializedData;

                            SesionIged.Expires = DateTime.Now.AddHours(8);
                            Response.Cookies.Add(SesionIged);
                            Encuesta objSesion = new Encuesta();
                            //borrado session para el usuario
                            objSesion.eliminarSesionIdUsuario(u);
                            //CAMBIO SESIONES --insercion primera vez
                            string variables = "ID_USUARIO,PERFILES,VALIDAPERFIL,MODELO,MODELOHOGAR,VALINCLUIDO,GRUPOVICTIMA,CODHOGAR,";
                            variables = variables + "TEMASVALIDAR,CAPTERMI,TEMA,IDTEMA,FLUJO,COLLECIONPERSONAS,PREGUNTAACTUAL,LISTADEPARTAMENTOS,";
                            variables = variables + "LISTAMUNICIPIOS,OPCIONESRESPUESTA,LISTAAUTO,TEMAS,PREGUNTAINICIAL,LISTADEPARTAMENTOSDT,LISTADT,LISTAPA,SYSGUID,LISTAMU,MODELOPERSONA";
                            objSesion.insertarVariablesSesion(variables, u);
                            return View("Inicio");
                           // return View("~/Views/Encuesta/ConformacionHogar.cshtml");
                        }
                        else
                            TempData["inv"] = "Token de Sesión para la aplicación invalido.";
                    }
                    else
                        TempData["inv"] = "Token de Sesión para la aplicación invalido.";
                }
                else
                    TempData["inv"] = "Token de Sesión para la aplicación invalido.";

                //-----------

            }
            catch (Exception e)
            {
                TempData["inv"] = e.Message.ToString();
            }

            return View("SesionInvalida");
        }

        [ExpiraSesionFilter]
        public void actualizarCookie()
        {
            SesionAplicacion objSesionAplicacion = new SesionAplicacion();
            ViewBag.CerrarVentana = false;
            ViewBag.tk = "";
            TempData["msg"] = TempData["err"] = "";

            try
            {
                if (Request.Cookies["SesionPortal"] != null)
                {
                    string valor = "", tokenPortal = "", TokenCookie = "";
                    string app = "", userIdApp = "", tknApp = "";

                    tokenPortal = Request.Cookies["SesionPortal"]["Token"].ToString();
                    valor = Request.Cookies["SesionPortal"]["Aplicaciones"].ToString();

                    app = Request.Cookies["SesionIged"]["App"].ToString();
                    userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                    tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();

                    //if (valor.IndexOf("|" + Session["App"].ToString() + "|") != -1) // SE VALIDA SI EXISTE TOKEN EN LA COOKIE PARA LA APLICACION QUE SE VA A CARGAR
                    if (valor.IndexOf("|" + app + "|") != -1) // SE VALIDA SI EXISTE TOKEN EN LA COOKIE PARA LA APLICACION QUE SE VA A CARGAR
                    {
                        ViewBag.tk = Request.Form["tk"];
                        //TokenCookie = objSesionAplicacion.TokenApp(valor, Session["App"].ToString());
                        TokenCookie = objSesionAplicacion.TokenApp(valor, app);
                        if (TokenCookie == Request.Form["tk"]) // SE VALIDA SI EL TOKEN DE LA APLICACION CORRESPONDE AL TOKEN DE LA COOKIE
                        {
                            //Response.Cookies.Add(objSesionAplicacion.actualizarAppCookie(Session["UserIdApp"].ToString(), Session["App"].ToString(), tokenPortal, Session["TknApp"].ToString(), valor));
                            Response.Cookies.Add(objSesionAplicacion.actualizarAppCookie(userIdApp, app, tokenPortal, tknApp, valor));
                        }
                        else
                        {
                            ViewBag.CerrarVentana = true;
                            TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                        }
                    }
                    else
                    {
                        ViewBag.CerrarVentana = true;
                        TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                    }
                }
                else
                {
                    ViewBag.CerrarVentana = true;
                    TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                }
            }
            catch (Exception e)
            {
                TempData["err"] = e.Message.ToString();
            }

        }

        // GET: 
        //[ExpiraSesionFilter]
        public ActionResult CerrarModulo()
        {
            List<Parametros> param = new List<Parametros>();
            SesionAplicacion objSesionAplicacion = new SesionAplicacion();
            TipoDocumento objTipoDoc = new TipoDocumento();
            string userIdApp, app, tknApp;

            //  cargarOpciones();

            app = Request.Cookies["SesionIged"]["App"].ToString();
            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();

            Encuesta objSesion = new Encuesta();
            objSesion.eliminarSesionIdUsuario(userIdApp);
            ViewBag.TiposDoc = new SelectList(objTipoDoc.tiposDocumento(), "ID", "TIPO_DOC");

            try
            {
                //if (app != null && userIdApp != null && tknApp != null)
                //{
                //    //objSesionAplicacion.EliminarTokenAplicacion(Session["UserIdApp"].ToString(), Session["App"].ToString(), Session["TknApp"].ToString(), out param);
                //    objSesionAplicacion.EliminarTokenAplicacion(userIdApp, app, tknApp, out param);

                //    if (param.Find(x => x.Nombre == "p_Salida").Valor != "1")
                //    {
                //        TempData["msg"] = "No se pudo realizar el cierre de Sesion debido a un problema con la Base de Datos";
                //        return RedirectToAction("Home", "IgedEncuesta");
                //    }
                //}
                //else
                //{
                    //userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                    //app = Request.Cookies["SesionIged"]["App"].ToString();
                    //tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();

                    objSesionAplicacion.EliminarTokenAplicacion(userIdApp, app, tknApp, out param);
               // }

                if (Request.Cookies["SesionIged"] != null)
                    Response.Cookies["SesionIged"].Expires = DateTime.Now.AddDays(-1);

                Session.Abandon();
                ViewBag.CerrarVentana = true;
               // return View("~/Views/Encuesta/ConformacionHogar.cshtml");
                return View("Inicio");
            }
            catch (Exception e)
            {
                TempData["err"] = e.Message.ToString();
                ViewBag.CerrarVentana = true;
                return View("Inicio");
                //return RedirectToAction("Inicio", "Reportes");
            }
        }

        [ExpiraSesionFilter]
        public void actualizarFechaHoraCookie()
        {
            SesionAplicacion objAplicacion = new SesionAplicacion();
            ViewBag.CerrarVentana = false;
            ViewBag.tk = "";
            string userIdApp, app, tknApp;

            app = Request.Cookies["SesionIged"]["App"].ToString();
            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();

            try
            {
                if (Request.Cookies["SesionPortal"] != null)
                {
                    string valor = "", tokenPortal = "", TokenCookie = "";

                    tokenPortal = Request.Cookies["SesionPortal"]["Token"].ToString();
                    valor = Request.Cookies["SesionPortal"]["Aplicaciones"].ToString();

//                    if (valor.IndexOf("|" + Session["App"].ToString() + "|") != -1) // SE VALIDA SI EXISTE TOKEN EN LA COOKIE PARA LA APLICACION QUE SE VA A CARGAR
                    if (valor.IndexOf("|" + app + "|") != -1) // SE VALIDA SI EXISTE TOKEN EN LA COOKIE PARA LA APLICACION QUE SE VA A CARGAR
                    {
                        //ViewBag.tk = Session["TknApp"];
                        ViewBag.tk = tknApp;

                        //TokenCookie = objAplicacion.TokenApp(valor, Session["App"].ToString());
                        TokenCookie = objAplicacion.TokenApp(valor, app);

                        if (TokenCookie == ViewBag.tk) // SE VALIDA SI EL TOKEN DE LA APLICACION CORRESPONDE AL TOKEN DE LA COOKIE
                        {

                            //Response.Cookies.Add(objAplicacion.actualizarAppCookie(Session["UserIdApp"].ToString(), Session["App"].ToString(), tokenPortal, Session["TknApp"].ToString(), valor));
                            Response.Cookies.Add(objAplicacion.actualizarAppCookie(userIdApp, app, tokenPortal, tknApp, valor));
                        }
                        else
                        {
                            ViewBag.CerrarVentana = true;
                            TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                        }
                    }
                    else
                    {
                        ViewBag.CerrarVentana = true;
                        TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                    }
                }
                else
                {
                    ViewBag.CerrarVentana = true;
                    TempData["msg"] = "No se pudo realizar esta operación debido a que esta sesión no es valida";
                }
            }
            catch (Exception e)
            {
                TempData["err"] = e.Message.ToString();
            }

        }

        [ExpiraSesionFilter]
        public FileResult DescargarManual()
        {
            //byte[] fileBytes = System.IO.File.ReadAllBytes("C:\prueba.pdf");
            //var response = new FileContentResult(fileBytes, "application/octet-stream");
            //response.FileDownloadName = "Manual.pdf";
            //return response;

            //[1] get the filepath and file name from the database using Linq to Entities
            //Attachment attachment = datacontext.Attachments.Where(x => x.attach_id == id).FirstOrDefault();
            //string strFilePath = attachment.FilePathAndNameOnServer_Generated;
            //string strFileName = attachment.file_name;

            //[2] read the file into a FielStream and return it as a byte array

            string filePath = Server.MapPath(Url.Content("~/Archivos/GuiaIGEDEncuesta.pdf"));

            using (FileStream fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] returnBytes;
                returnBytes = new byte[fileStream.Length];
                fileStream.Read(returnBytes, 0, returnBytes.Length);
                return File(returnBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "GuiaIGEDEncuesta.pdf");
            }
        }
       

        public ActionResult FinSesion()
        {
            SesionAplicacion objSesionAplicacion = new SesionAplicacion();
            string userIdApp, app, tknApp;
            List<Parametros> param = new List<Parametros>();


            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            app = Request.Cookies["SesionIged"]["App"].ToString();
            tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();

            Response.Cookies["SesionIged"].Expires = DateTime.Now.AddDays(-1);

            objSesionAplicacion.EliminarTokenAplicacion(userIdApp, app, tknApp, out param);

            Session.Abandon();

            if (!Request.IsAjaxRequest())
            {
                return PartialView("_FinSesion");
            }
            else
                return PartialView("_FinSesion2");
        }

        public ActionResult VerificarFinSesion()
        {
            try{
                log.Info("metodo VerificarFinSesion , tiempoDuracion: ");
                List<Parametros> param = new List<Parametros>();
                SesionAplicacion objSesionAplicacion = new SesionAplicacion();
                string userIdApp, app, tknApp;

                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                app = Request.Cookies["SesionIged"]["App"].ToString();
                tknApp = Request.Cookies["SesionIged"]["TknApp"].ToString();
                log.Info("metodo VerificarFinSesion , tknApp: " + tknApp);
                Encuesta objSesion = new Encuesta();
                string fecha = objSesion.obtenerFechaUltimaTRansaccion(userIdApp);
                log.Info("metodo VerificarFinSesion , fecha: " + fecha);
                if (fecha != "")
                {
                    DateTime fechUltimaTransaccion = new DateTime();
                    fechUltimaTransaccion = DateTime.Parse(fecha);
                    log.Info("metodo VerificarFinSesion , fechUltimaTransaccion: " + fechUltimaTransaccion);
                    //diferecnia entre las dos fechas
                    TimeSpan ts = DateTime.Now - fechUltimaTransaccion;
                    int diferenciaMinutos = ts.Minutes;
                    int valorVerificar = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["Timeout"]);
                    int diferencia = valorVerificar - diferenciaMinutos;
                    if (diferencia < 0)
                    {
                        objSesionAplicacion.EliminarTokenAplicacion(userIdApp, app, tknApp, out param);
                        //SE ELIMINAN LAS VARIABLEA DE SESION 
                        objSesion.eliminarSesionIdUsuario(userIdApp);
                        if (param.Find(x => x.Nombre == "p_Salida").Valor == "1")
                        {

                            Session.Abandon();

                            // Invalidate the Cache on the Client Side
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetNoStore();
                            return Json("cerrada", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json("abierta", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Info("metodo VerificarFinSesion , ERROR: " + ex.ToString());
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);

            }


        }

     
    }
}
