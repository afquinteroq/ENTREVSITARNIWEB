using AdministracionInstrumentos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace IgedEncuesta.Controllers
{
    public class EncuestaController : Controller
    {

        private static readonly ILog log = LogManager.GetLogger("Web");
        List<PdfReader> memoriaReaderes = new List<PdfReader>();
        public EncuestaController()
        {
            XmlConfigurator.Configure();
        }

        ~EncuestaController()
        {

        }

        private string Usuario = string.Empty;
        private string codigoEncuesta = string.Empty;
        public ActionResult Encuesta(string codigoHogar = null, string tipo = null)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                gic_Tema tema = new gic_Tema();
                List<gic_Tema> temas = null;

                var perfiles = objSesion.getValorCampoSesion("PERFILES", userIdApp);
                temas = tema.getTemasxInstrumento(1, 1, perfiles);
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(temas);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "TEMAS", serializedData);
                List<gic_Tema> temasValidar = null;

                /************************************************************************
                PARA QUE NO VALIDE LA SESION , 
                *************************************************************************/
                if (codigoHogar == null)
                    codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                else
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "CODHOGAR", codigoHogar);
                
                codigoEncuesta = codigoHogar;
                temasValidar = tema.getTemasValidados(codigoEncuesta);
                
                var serializeTemas = Newtonsoft.Json.JsonConvert.SerializeObject(temasValidar);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "TEMASVALIDAR", serializeTemas);
                int terminoCap = tema.get_VerficarCapitulosPrimeros(codigoEncuesta);
                
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "CAPTERMI", terminoCap.ToString());
                //adicion para  guardar los valores en una cookie
                return View(temas);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / Encuesta , Error: " + e.Message.ToString());
                return null;
            }
        }

        //[ExpiraSesionFilter]
        public ActionResult cargarTema(string idTema)
        {

            try
            {

                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                List<gic_Tema> temas = new List<gic_Tema>();
                gic_Tema tema = new gic_Tema();
                var objeto = objSesion.getValorCampoSesion("TEMAS", userIdApp);
                temas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<gic_Tema>>(objeto);
                tema = temas.FirstOrDefault(x => x.tem_IdTema == int.Parse(idTema));
                var serializeTema = Newtonsoft.Json.JsonConvert.SerializeObject(tema);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "TEMA", serializeTema);
                int preguntaInicial = 1;
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "PREGUNTAINICIAL", preguntaInicial.ToString());
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "IDTEMA", idTema.ToString());
                string codigoEncuesta = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                int idInstrumento = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IdInstrumento"].ToString());
                gic_PreguntaRespuestasFlujo preguntasRespuestasFlujo = new gic_PreguntaRespuestasFlujo();
                preguntasRespuestasFlujo = PreguntaMostrar(codigoEncuesta, int.Parse(idTema), idInstrumento, preguntaInicial, "TEM");
                preguntasRespuestasFlujo.temaAmostrar = tema;
                return PartialView("_CargaEncuesta", preguntasRespuestasFlujo);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / cargarTema , Error: " + e.Message.ToString());
                return null;
            }

        }


        //Traer pregunta siguiente
        private gic_PreguntaRespuestasFlujo PreguntaMostrar(string codHogar, int idTema, int idInstrumento, int idPregunta, string flujo)
        {
            try
            {

                gic_PreguntaRespuestasFlujo preguntasRespuestasFlujo = new gic_PreguntaRespuestasFlujo();
                gic_Pregunta pregunta = new gic_Pregunta();
                gic_Tema tema = new gic_Tema();
                gic_Hogar hogar = new gic_Hogar();
                gic_ElementosPadre padreYorden = new gic_ElementosPadre();
                List<gic_PreguntasxPersona> colleccionPreguntas = new List<gic_PreguntasxPersona>();
                List<gic_RespuestaxEncuesta> coleccionRespuestas = new List<gic_RespuestaxEncuesta>();
                gic_RespuestaxEncuesta respuestaXencuesta = new gic_RespuestaxEncuesta();
                gic_PreguntasxPersona preguntaXpersonaGen = new gic_PreguntasxPersona();
                gic_RespuestaxEncuesta respuestaPreguntaGen = new gic_RespuestaxEncuesta();
                DataTable departamentos = new DataTable();
                List<SelectListItem> SelectDepartamentos = new List<SelectListItem>();
                List<gic_RespuestaNuevo> opcionesRespuestas = new List<gic_RespuestaNuevo>();
                gic_RespuestaNuevo objRespuestas = new gic_RespuestaNuevo();
                List<gic_RespuestaNuevo> opcionesRespuestasFiltrado = new List<gic_RespuestaNuevo>();
                DataTable municipios = new DataTable();
                DataTable direccionterritoriales = new DataTable();
                DataTable puntosatencion = new DataTable();
                DataTable municipiosatencion = new DataTable();
                List<SelectListItem> SelectMunicipios = new List<SelectListItem>();
                List<SelectListItem> SelecDireccionesTerritoriales = new List<SelectListItem>();
                List<SelectListItem> SelecPuntosDeAtencion = new List<SelectListItem>();
                List<SelectListItem> SelecMunicipioAtencion = new List<SelectListItem>();

                var watch = System.Diagnostics.Stopwatch.StartNew();
                int idPersonaEncuestada = hogar.get_IdPersonaEncuestada(codHogar);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                log.Info("metodo get_IdPersonaEncuestada , tiempoDuracion: " + elapsedMs);
                watch = System.Diagnostics.Stopwatch.StartNew();
                padreYorden = pregunta.getMaxPreguntaPadre(codHogar, idTema);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                log.Info("metodo getMaxPreguntaPadre , tiempoDuracion: " + elapsedMs);
                List<string> listaAutoCompletar = new List<string>();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                
                preguntasRespuestasFlujo.flujo = flujo;
                preguntasRespuestasFlujo.idPersonaEncuesta = idPersonaEncuestada;
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                if (idPregunta == 1)
                {
                    idPregunta = 1;
                }
                else if (padreYorden.el_IdRespuestaPadre != 0)
                {
                    idPregunta = padreYorden.el_IdRespuestaPadre;
                }
                if (flujo == "SIG" || flujo == "TEM")
                {
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    colleccionPreguntas = pregunta.getPreguntaSiguiente(codHogar, idTema, idInstrumento, idPregunta);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo getPreguntaSiguiente , tiempoDuracion: " + elapsedMs);
                    var serializePersonas = Newtonsoft.Json.JsonConvert.SerializeObject(colleccionPreguntas);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "COLLECIONPERSONAS", serializePersonas);
                    // Elimina la marca de capitulo terminado -- GDV
                    if (flujo == "SIG")
                    {
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        tema.eliminarFinalizarCapitulo(codHogar, idTema, Usuario);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        log.Info("metodo eliminarFinalizarCapitulo , tiempoDuracion: " + elapsedMs);
                        // Actualiza el estado de la encuesta a 'ACTIVA' -- GDV
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        hogar.actualizarEstadoEncuesta(codHogar, usuario, "5");
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        log.Info("metodo actualizarEstadoEncuesta , tiempoDuracion: " + elapsedMs);
                    }
                }
                else
                {

                    int preguntaActual = int.Parse(objSesion.getValorCampoSesion("PREGUNTAACTUAL", userIdApp));
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    colleccionPreguntas = pregunta.getPreguntaAnterior(codHogar, idTema, idInstrumento, preguntaActual);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo getPreguntaAnterior , tiempoDuracion: " + elapsedMs);

                    // Elimina la marca de capitulo terminado -- GDV
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    tema.eliminarFinalizarCapitulo(codHogar, idTema, Usuario);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo eliminarFinalizarCapitulo , tiempoDuracion: " + elapsedMs);
                    // Actualiza el estado de la encuesta a 'ACTIVA' -- GDV
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    hogar.actualizarEstadoEncuesta(codHogar, usuario, "5");
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo actualizarEstadoEncuesta , tiempoDuracion: " + elapsedMs);
                }
                // Trae todas las respuestas contestadas por tema.
                watch = System.Diagnostics.Stopwatch.StartNew();
                coleccionRespuestas = respuestaXencuesta.getRespuestasXcapitulo(codHogar, idTema);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;
                log.Info("metodo getRespuestasXcapitulo , tiempoDuracion: " + elapsedMs);

                int conteo = colleccionPreguntas.Count();
                //Verifica que devuelva una pregunta para tarer un objeto preguntaXpersona, para validaciones posteriores
                if (colleccionPreguntas.Count > 0)
                {
                    preguntaXpersonaGen = colleccionPreguntas.Find(x => x.pre_IdPregunta != 0);

                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "PREGUNTAACTUAL", preguntaXpersonaGen.pre_IdPregunta.ToString());
                    //Si el tipo es DP, se necesita cargar el combo de departamentos
                    if (preguntaXpersonaGen.pre_TipoCampo == "DP")
                    {
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        departamentos = pregunta.datosDepartamentos();
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        log.Info("metodo datosDepartamentos , tiempoDuracion: " + elapsedMs);
                        municipios = pregunta.datosMunicipios("0");
                        SelectMunicipios = pregunta.CrearLista(municipios, 2);
                        SelectDepartamentos = pregunta.CrearLista(departamentos, 1);
                        var serializeDepartamentos = Newtonsoft.Json.JsonConvert.SerializeObject(SelectDepartamentos);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTADEPARTAMENTOS", serializeDepartamentos);
                        var serializeMunicipios = Newtonsoft.Json.JsonConvert.SerializeObject(SelectMunicipios);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTAMUNICIPIOS", serializeMunicipios);
                    }

                    if (preguntaXpersonaGen.pre_TipoCampo == "DT")
                    {
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        departamentos = pregunta.datosDepartamentos();
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        log.Info("metodo datosDepartamentos , tiempoDuracion: " + elapsedMs);
                        direccionterritoriales = pregunta.datosDireccionTerritorial("0");
                        SelecDireccionesTerritoriales = pregunta.CrearLista(direccionterritoriales, 2);
                        SelectDepartamentos = pregunta.CrearLista(departamentos, 1);
                        puntosatencion = pregunta.datosPuntoAtencion("", "0");
                        municipiosatencion = pregunta.datosMunicipioAtencion("", "0");
                        SelecPuntosDeAtencion = pregunta.CrearLista(puntosatencion, 2);
                        SelecMunicipioAtencion = pregunta.CrearLista(municipiosatencion, 2);

                        var serializeDepartamentos = Newtonsoft.Json.JsonConvert.SerializeObject(SelectDepartamentos);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTADEPARTAMENTOSDT", serializeDepartamentos);

                        var serializeDireccionesTerritoriales = Newtonsoft.Json.JsonConvert.SerializeObject(SelecDireccionesTerritoriales);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTADT", serializeDireccionesTerritoriales);

                        var serializePuntosAtecion = Newtonsoft.Json.JsonConvert.SerializeObject(SelecPuntosDeAtencion);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTAPA", serializePuntosAtecion);

                        var serializeMunicipioAtecion = Newtonsoft.Json.JsonConvert.SerializeObject(SelecMunicipioAtencion);
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTAMU", serializeMunicipioAtecion);
                    }

                    //Obtiene las opciones de respuesta para la pregunta
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    opcionesRespuestas = pregunta.getRespuestasxPregunta(preguntaXpersonaGen.pre_IdPregunta, idInstrumento);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo getRespuestasxPregunta , tiempoDuracion: " + elapsedMs);
                    
                    var serializeOpcionesRespuesta = Newtonsoft.Json.JsonConvert.SerializeObject(opcionesRespuestas);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "OPCIONESRESPUESTA", serializeOpcionesRespuesta);
                    opcionesRespuestasFiltrado = objRespuestas.getRespuestasxPrexPersona(preguntaXpersonaGen.pre_IdPregunta, idInstrumento, codHogar, preguntaXpersonaGen.per_IdPersona);

                }
                //Valida que devuelva alguna respuesta contestada y no se reviente.
                //Ademas para las preguntas GENERALES, que son una sola respuesta lo 
                //filtra para que devuelva solo un objeto de tipo gic_RespuestaxEncuesta
                if (coleccionRespuestas != null)
                {

                    respuestaPreguntaGen = coleccionRespuestas.Find(x => x.res_IdRespuesta.pre_IdPregunta.pre_IdPregunta.Equals(preguntaXpersonaGen.pre_IdPregunta));
                    coleccionRespuestas = coleccionRespuestas.FindAll(x => x.res_IdRespuesta.pre_IdPregunta.pre_IdPregunta.Equals(preguntaXpersonaGen.pre_IdPregunta));
                }
                else
                {
                    respuestaPreguntaGen = null;
                }
                if (preguntaXpersonaGen.pre_TipoCampo == "AT")
                {
                    listaAutoCompletar = pregunta.obtenerSelectAutoCompletar(preguntaXpersonaGen.pre_IdPregunta);

                    var serializeOpcionesAuto = Newtonsoft.Json.JsonConvert.SerializeObject(listaAutoCompletar);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTAAUTO", serializeOpcionesAuto);

                }
                //valida si alguna respuesta tiene autocompletar
                if (opcionesRespuestas.Find(x => x.res_AutoCompletar == "SI") != null)
                {
                    listaAutoCompletar = pregunta.obtenerSelectAutoCompletar(preguntaXpersonaGen.pre_IdPregunta);

                    var serializeOpcionesAuto = Newtonsoft.Json.JsonConvert.SerializeObject(listaAutoCompletar);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "LISTAAUTO", serializeOpcionesAuto);
                }

                //Asigna los multples Modelos para la conformación de la encuesta.
                preguntasRespuestasFlujo.colleccionPreguntas = colleccionPreguntas;
                preguntasRespuestasFlujo.coleccionRespuestas = coleccionRespuestas;
                preguntasRespuestasFlujo.respuestaPreguntaGen = respuestaPreguntaGen;
                preguntasRespuestasFlujo.preguntaXpersonaGen = preguntaXpersonaGen;
                preguntasRespuestasFlujo.opcionesRespuesta = opcionesRespuestas;
                preguntasRespuestasFlujo.opcionesRespuestaFiltrado = opcionesRespuestasFiltrado;
                preguntasRespuestasFlujo.respuestasConcatenadas = concatenarIdsRespuestas(opcionesRespuestas);
                preguntasRespuestasFlujo.personasConcatenadas = concatenarIdPersonas(colleccionPreguntas);

                preguntasRespuestasFlujo.totalPreguntas = preguntasRespuestasFlujo.colleccionPreguntas.Count();

                preguntasRespuestasFlujo.habilitar = "0";

                if (preguntasRespuestasFlujo.colleccionPreguntas.Count() == 0 && idPregunta != 1)
                {

                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "PREGUNTAACTUAL", "0");
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    tema.finalizarCapitulo(codHogar, idTema, usuario);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo finalizarCapitulo , tiempoDuracion: " + elapsedMs);
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    int terminoCap = tema.get_VerficarCapitulosPrimeros(codHogar);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo get_VerficarCapitulosPrimeros , tiempoDuracion: " + elapsedMs);
                    preguntasRespuestasFlujo.totalCapitulosTerminados = terminoCap;

                    if (terminoCap == 3)
                    {   
                        preguntasRespuestasFlujo.habilitar = "1";

                    }
                }

                preguntasRespuestasFlujo.totalCapitulosTerminados = tema.numeroCapitulosTerminados(codHogar);
                preguntasRespuestasFlujo.codigoHogar = codHogar;
                return preguntasRespuestasFlujo;
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / PreguntaMostrar , Error: " + e.Message.ToString());
                return null;
            }

        }

        [ActionName("MunicipiosPorDepartamento")]
        public JsonResult MunicipiosPorDepartamento(string id)
        {
            try
            {
                DataTable municipios = null;
                List<SelectListItem> SelectMunicipios = null;
                gic_Pregunta pregunta = new gic_Pregunta();
                municipios = pregunta.datosMunicipios(id);
                SelectMunicipios = pregunta.CrearLista(municipios, 2);
                
                return this.Json(SelectMunicipios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / MunicipiosPorDepartamento , Error: " + e.Message.ToString());
                return null;
            }

        }


        [ActionName("dtsPorDepartamento")]
        public JsonResult dtsPorDepartamento(string id)
        {
            try
            {
                DataTable dts = null;
                List<SelectListItem> SelecsDTs = null;
                gic_Pregunta pregunta = new gic_Pregunta();
                dts = pregunta.datosDireccionTerritorial(id);
                SelecsDTs = pregunta.CrearLista(dts, 2);

                return this.Json(SelecsDTs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / dtsPorDepartamento , Error: " + e.Message.ToString());
                return null;
            }

        }

        [ActionName("dtsDepartamentoPorDT")]
        public JsonResult dtsDepartamentoPorDT(string id)
        {
            try
            {

                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                var codigohogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);

                DataTable dts = null; 
                List<SelectListItem> SelecsDTs = null;
                gic_Pregunta pregunta = new gic_Pregunta();
                dts = pregunta.datosDeptoPorDT(codigohogar, id);
                SelecsDTs = pregunta.CrearLista(dts, 2);

                return this.Json(SelecsDTs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / dtsPorDepartamento , Error: " + e.Message.ToString());
                return null;
            }

        }

        [ActionName("guardadoUltimoValor")]
        public JsonResult guardadoUltimoValor(string id)
        {
            try
            {
                //27/02/2020
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                var codigohogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                //
                string retorno;
                gic_Pregunta pregunta = new gic_Pregunta();
                pregunta.guardaUltimoValorSel(codigohogar, id);
                retorno = "ok";
                return this.Json(retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / dtsPorDepartamento , Error: " + e.Message.ToString());
                return null;
            }

        }


        [ActionName("puntoatencionpordt")]
        public JsonResult puntoatencionpordt(string id)
        {
            try
            {

                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                var codigohogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                DataTable dtPA = null;
                List<SelectListItem> SelecsPAs = null;
                gic_Pregunta pregunta = new gic_Pregunta();
                dtPA = pregunta.datosPuntoAtencion(codigohogar, id);
                SelecsPAs = pregunta.CrearLista(dtPA, 2);
                return this.Json(SelecsPAs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / puntoatencionpordt , Error: " + e.Message.ToString());
                return null;
            }

        }



        [ActionName("municipioporpuntoatencion")]
        public JsonResult municipioporpuntoatencion(string id)
        {
            try
            {
                //27/02/2020
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                var codigohogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                //
                DataTable dtMun = null;
                List<SelectListItem> SelecsPAs = null;
                gic_Pregunta pregunta = new gic_Pregunta();
                dtMun = pregunta.datosMunicipioAtencion(codigohogar, id);
                SelecsPAs = pregunta.CrearLista(dtMun, 2);
                return this.Json(SelecsPAs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / municipioporpuntoatencion , Error: " + e.Message.ToString());
                return null;
            }

        }

        [ActionName("AutoCompletarTexto")]
        public string AutoCompletarTexto(string term)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                var result = new List<string>();
                List<string> lista = null;
                var listaAutocompletarJson = objSesion.getValorCampoSesion("LISTAAUTO", userIdApp);
                if (String.IsNullOrEmpty(listaAutocompletarJson))
                    lista = new List<string>();
                else
                    lista = JsonConvert.DeserializeObject<List<string>>(listaAutocompletarJson);
                List<jsonValores> ent = new List<jsonValores>();
                foreach (var item in lista)
                {
                    jsonValores entr = new jsonValores();
                    entr.label = item;
                    entr.id = item;
                    entr.value = item;
                    result.Add(item);
                    ent.Add(entr);
                }
                ent = ent.Where(s => s.value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
                string json = JsonConvert.SerializeObject(ent, Formatting.None);
                return json;


            }
            catch (Exception ex)
            {
                log.Error("EncuestaController / AutoCompletarTexto , Error: " + ex.Message.ToString());
                return ex.Message.ToString();
            }
        }

        public string concatenarIdsRespuestas(List<gic_RespuestaNuevo> respuestas)
        {
            string respuestasCon = string.Empty;
            foreach (gic_RespuestaNuevo respuestaText in respuestas)
            {
                respuestasCon += respuestaText.res_IdRespuesta + ",";
            }
            if (respuestasCon != "")
            {
                respuestasCon = respuestasCon.Substring(0, respuestasCon.Length - 1);
            }
            return respuestasCon;
        }

        /// <summary>
        /// Concatena los Ids de las personas en un solo string.
        /// </summary>
        /// <param name="respuestas">Lista de personas a concatenar.</param>
        /// <returns>string unico con todas las personas, separados por coma.</returns>
        public string concatenarIdPersonas(List<gic_PreguntasxPersona> preguntaxPersona)
        {
            string personasCon = string.Empty;
            foreach (gic_PreguntasxPersona personas in preguntaxPersona)
            {
                personasCon += personas.per_IdPersona + ",";
            }
            if (personasCon != "")
            {
                personasCon = personasCon.Substring(0, personasCon.Length - 1);
            }
            return personasCon;
        }


        public ActionResult anterior()
        {

            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                int idInstrumento = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IdInstrumento"]);

                int idTema = int.Parse(objSesion.getValorCampoSesion("IDTEMA", userIdApp));
                gic_PreguntaRespuestasFlujo preguntasRespuestasFlujo = new gic_PreguntaRespuestasFlujo();
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                preguntasRespuestasFlujo = PreguntaMostrar(codigoHogar, idTema, idInstrumento, 0, "ANT");
                gic_Tema tema = new gic_Tema();

                var temaJson = objSesion.getValorCampoSesion("TEMA", userIdApp);
                if (!String.IsNullOrEmpty(temaJson))
                    tema = JsonConvert.DeserializeObject<gic_Tema>(temaJson);

                preguntasRespuestasFlujo.temaAmostrar = tema;
                return PartialView("_CargaEncuesta", preguntasRespuestasFlujo);
            }
            catch (Exception ex)
            {
                log.Error("EncuestaController / anterior , Error: " + ex.Message.ToString());
                return null;
            }

        }

        public ActionResult siguiente(string[] respuestas, string tipo, string idPregunt, string tipoPregunt)
        {
            try
            {
                

                int bandera = 0;
                string[] cadena;
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                List<gic_RespuestaxEncuesta> listaRespuestas = new List<gic_RespuestaxEncuesta>();
                gic_RespuestaxEncuesta respuestaInsertar = new gic_RespuestaxEncuesta();
                List<gic_RespuestaNuevo> opcionesRespuestas = new List<gic_RespuestaNuevo>();
                gic_PreguntaRespuestasFlujo preguntasRespuestasFlujo = new gic_PreguntaRespuestasFlujo();
                gic_Pregunta pregunta = new gic_Pregunta();
                gic_Tema tema = new gic_Tema();

                int idInstrumento = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IdInstrumento"]);

                int idTema = int.Parse(objSesion.getValorCampoSesion("IDTEMA", userIdApp));

                var temaJson = objSesion.getValorCampoSesion("TEMA", userIdApp);
                if (!String.IsNullOrEmpty(temaJson))
                    tema = JsonConvert.DeserializeObject<gic_Tema>(temaJson);

                List<gic_PreguntasxPersona> colleccionPregXPersonas = new List<gic_PreguntasxPersona>();
                
                var modeloPersonasJson = objSesion.getValorCampoSesion("COLLECIONPERSONAS", userIdApp);

                colleccionPregXPersonas = JsonConvert.DeserializeObject<List<gic_PreguntasxPersona>>(modeloPersonasJson);
                opcionesRespuestas = pregunta.getRespuestasxPregunta(int.Parse(idPregunt), idInstrumento);
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);

                bandera = 0;
                int idPersona = 0;

                foreach (string respuesta in respuestas)
                {
                    
                    cadena = respuesta.Split('_');

                    int i = 0;
                    respuestaInsertar = new gic_RespuestaxEncuesta();

                    string texto = string.Empty;

                    foreach (string valores in cadena)
                    {

                        if (i == 3)
                        {
                            respuestaInsertar.per_IdPersona = int.Parse(valores);

                            if (idPersona == 0)
                            {
                                idPersona = int.Parse(valores);
                                bandera = 1;
                            }
                            else if (idPersona != int.Parse(valores))
                            {
                                idPersona = int.Parse(valores);
                                bandera = 1;
                            }
                            else
                            {
                                bandera = 0;
                            }
                        }
                        else if (i == 5)
                        {
                            respuestaInsertar.res_IdRespuesta = new gic_Respuesta();
                            respuestaInsertar.res_IdRespuesta.res_IdRespuesta = int.Parse(valores);
                        }
                        else if (i == 7)
                        {
                            
                            try
                            {

                                var opcionesFecha = (tipo == "FE");

                                if (opcionesFecha != false)
                                {
                                    DateTime MyDateTime;
                                    MyDateTime = new DateTime();
                                    
                                    MyDateTime = DateTime.ParseExact(valores, "yyyy-MM-dd", null);
                                    texto = MyDateTime.ToString("dd/MM/yyyy");

                                    respuestaInsertar.rxp_TextoRespuesta = texto;
                                }
                                else
                                {

                                    respuestaInsertar.rxp_TextoRespuesta = valores;
                                }

                            }
                            catch (Exception eee)
                            {
                                log.Info("EncuestaController / siguiente , .... error: exception : " + eee.Message + "/r" + eee.StackTrace);
                                respuestaInsertar.rxp_TextoRespuesta = valores;
                            }
                        }
                        i++;
                    }
                    HttpCookie reqCookies = Request.Cookies["SesionIged"];
                    string usuario = reqCookies["USUARIO"].ToString();
                    respuestaInsertar.usu_FechaCreacion = DateTime.Now;
                    respuestaInsertar.usu_UsuarioCreacion = usuario;
                    respuestaInsertar.bandera = bandera;
                    respuestaInsertar.ins_IdInstrumento = idInstrumento;
                    respuestaInsertar.rxp_TipoPreguntaRespuesta = tipoPregunt;
                    respuestaInsertar.cod_Hogar = codigoHogar;

                    listaRespuestas.Add(respuestaInsertar);

                }
                int resultado = 0;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                gic_PreguntasxPersona personaxpreguntaFallo = new gic_PreguntasxPersona();
                if (listaRespuestas != null && listaRespuestas.Count() > 0)
                {
                    if (int.Parse(idPregunt) == 13)
                    {
                        actualizarNombres(listaRespuestas, idInstrumento, colleccionPregXPersonas);
                    }
                    else if (int.Parse(idPregunt) == 14)
                    {

                        resultado = VerificarEdad(listaRespuestas, colleccionPregXPersonas, out personaxpreguntaFallo);
                        if (resultado == 0)
                        {
                            var prueba = new { data = 1, data2 = "La edad de " + personaxpreguntaFallo.per_PrimerNombre + " " + personaxpreguntaFallo.per_SegundoNombre + " " + personaxpreguntaFallo.per_PrimerApellido + " " + personaxpreguntaFallo.per_SegundoApellido + " no concuerda con la fecha de nacimiento" };
                            return Json(prueba, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (int.Parse(idPregunt) == 27 || int.Parse(idPregunt) == 31)
                    {
                        actualizaDatosTablaPersona(listaRespuestas, int.Parse(idPregunt));
                    }

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    respuestaInsertar.insertaRespuestaXencuesta(listaRespuestas, int.Parse(idPregunt));
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    log.Info("metodo insertaRespuestaXencuesta , tiempoDuracion: " + elapsedMs);
                    preguntasRespuestasFlujo = PreguntaMostrar(codigoHogar, idTema, idInstrumento, 0, "SIG");


                    preguntasRespuestasFlujo.temaAmostrar = tema;
                    return PartialView("_CargaEncuesta", preguntasRespuestasFlujo);

                }
                else
                {
                    preguntasRespuestasFlujo = PreguntaMostrar(codigoHogar, idTema, idInstrumento, 0, "SIG");
                    preguntasRespuestasFlujo.temaAmostrar = tema;
                }

                return View();
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / siguiente , Error: " + e.StackTrace.ToString(), e);
                return null;
            }

        }

        public void actualizaDatosTablaPersona(List<gic_RespuestaxEncuesta> listaRespuestas, int idPregunta)
        {
            try
            {
                gic_RespuestaNuevo resObj = new gic_RespuestaNuevo();
                foreach (gic_RespuestaxEncuesta respuestaInsertar in listaRespuestas)
                {
                    if (idPregunta == 31)
                        resObj.actualizaDocumento(respuestaInsertar.rxp_TextoRespuesta, respuestaInsertar.per_IdPersona);
                    else if (idPregunta == 27)
                        resObj.actualizaFechaNacimiento(respuestaInsertar.rxp_TextoRespuesta, respuestaInsertar.per_IdPersona);
                }
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / actualizaDatosTablaPersona , Error: " + e.Message.ToString());
            }

        }

        public int actualizarNombres(List<gic_RespuestaxEncuesta> listaRespuestas, int idInstrumento, List<gic_PreguntasxPersona> colleccionPregXPersonas)
        {

            try
            {
                int resultado = 1;
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                gic_RespuestaNuevo resObj = new gic_RespuestaNuevo();
                foreach (gic_PreguntasxPersona personas in colleccionPregXPersonas)
                {
                    string nombre1 = string.Empty;
                    string nombre2 = string.Empty;
                    string apellido1 = string.Empty;
                    string apellido2 = string.Empty;
                    string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                    foreach (gic_RespuestaxEncuesta respuestaInsertar in listaRespuestas.Where(x => x.per_IdPersona == personas.per_IdPersona))
                    {
                        #region Validacion para borrado y actualizacion de los nombres
                        switch (respuestaInsertar.res_IdRespuesta.res_IdRespuesta)
                        {
                            case 19:
                                if (respuestaInsertar.rxp_TextoRespuesta == string.Empty)
                                {
                                    resObj.borrarNombresVacios(codigoHogar, personas.per_IdPersona, respuestaInsertar.res_IdRespuesta.res_IdRespuesta, idInstrumento);
                                }
                                else
                                {
                                    nombre1 = respuestaInsertar.rxp_TextoRespuesta.Trim().ToUpper();
                                }
                                break;
                            case 20:

                                if (respuestaInsertar.rxp_TextoRespuesta == string.Empty)
                                {
                                    resObj.borrarNombresVacios(codigoHogar, personas.per_IdPersona, respuestaInsertar.res_IdRespuesta.res_IdRespuesta, idInstrumento);
                                }
                                else
                                {
                                    nombre2 = respuestaInsertar.rxp_TextoRespuesta.Trim().ToUpper();
                                }
                                break;
                            case 21:
                                if (respuestaInsertar.rxp_TextoRespuesta == string.Empty)
                                {
                                    resObj.borrarNombresVacios(codigoHogar, personas.per_IdPersona, respuestaInsertar.res_IdRespuesta.res_IdRespuesta, idInstrumento);
                                }
                                else
                                {
                                    apellido1 = respuestaInsertar.rxp_TextoRespuesta.Trim().ToUpper();
                                }
                                break;
                            case 22:
                                if (respuestaInsertar.rxp_TextoRespuesta == string.Empty)
                                {
                                    resObj.borrarNombresVacios(codigoHogar, personas.per_IdPersona, respuestaInsertar.res_IdRespuesta.res_IdRespuesta, idInstrumento);
                                }
                                else
                                {
                                    apellido2 = respuestaInsertar.rxp_TextoRespuesta.Trim().ToUpper();
                                }
                                break;



                                #endregion
                        }

                    }

                    if (nombre1 != string.Empty || nombre2 != string.Empty || apellido1 != string.Empty || apellido2 != string.Empty)
                    {
                        resObj.actualizaNombres(nombre1, nombre2, apellido1, apellido2, personas.per_IdPersona);
                    }

                }




                return resultado;
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / actualizarNombres , Error: " + e.Message.ToString());
                return 0;
            }

        }

        public int VerificarEdad(List<gic_RespuestaxEncuesta> listaRespuestas, List<gic_PreguntasxPersona> colleccionPregXPersonas, out gic_PreguntasxPersona personaxpreguntaFallo)
        {
            try
            {
                int resultado = 0;
                int edad = 0;
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                gic_RespuestaNuevo resObj = new gic_RespuestaNuevo();
                gic_PreguntasxPersona personaxpregunta = new gic_PreguntasxPersona();
                personaxpreguntaFallo = new gic_PreguntasxPersona();
                foreach (gic_RespuestaxEncuesta respuestaInsertar in listaRespuestas)
                {
                    switch (respuestaInsertar.res_IdRespuesta.res_IdRespuesta)
                    {
                        case 23:
                            edad = resObj.get_Edadxpersona(respuestaInsertar.per_IdPersona, codigoHogar);
                            personaxpregunta = colleccionPregXPersonas.Where(x => x.per_IdPersona == respuestaInsertar.per_IdPersona).First();
                            personaxpreguntaFallo = personaxpregunta;
                            if (edad.ToString() != respuestaInsertar.rxp_TextoRespuesta.Trim().ToUpper())
                            {
                                resultado = 0;
                            }
                            else
                            {
                                personaxpreguntaFallo = personaxpregunta;
                                resultado = 1;
                            }
                            break;
                    }
                    break;
                }

                return resultado;
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / VerificarEdad , Error: " + e.Message.ToString());
                personaxpreguntaFallo = null;
                return 0;
            }

        }


        public void Download()
        {
            try
            {
                gic_adminconfig adminConfig = new gic_adminconfig();
                List<gic_adminconfig> _Config = null;
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                string valorColilla = System.Configuration.ConfigurationManager.AppSettings["nombreColilla"];
                _Config = adminConfig.GetAdminConfiguracion(valorColilla);
                string hogCodigo = codigoHogar;
                var byteArchivoOriginal = crearPdfConTexto(_Config.First().ADMINCFG_VALUE, hogCodigo);
                MostrarPdf(byteArchivoOriginal, hogCodigo);

            }
            catch (Exception e)
            {
                log.Error("EncuestaController / Download , Error: " + e.Message.ToString());

            }

        }

        public byte[] crearPdfConTexto(string oldFile, string textoInsertar)
        {

            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document doc = new Document(size);
            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();
                PdfContentByte cb = wri.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cb.SetFontAndSize(bf, 8);

                string text = "";

                cb.BeginText();
                text = textoInsertar;

                cb.ShowTextAligned(5, text, 450, 380, 0);
                cb.EndText();
                PdfImportedPage page = wri.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);
                doc.Close();
                wri.Close();
                reader.Close();
                memoriaReaderes.Add(reader);
                return output.ToArray();
            }

        }

        /// <summary>
        /// Muestra el archivo PDF.
        /// </summary>
        /// <param name="byteInformacion"> bytes informacion.</param>
        /// <param name="codigoHogar">El codigo hogar.</param>
        private void MostrarPdf(byte[] byteInformacion, string codigoHogar)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=CODHOGAR_" + codigoHogar + "_" + DateTime.Now + ".pdf");
            Response.BinaryWrite(byteInformacion);
            Response.Flush();
            Response.End();
            Response.Clear();
        }

        [HttpPost]
        public void cargarSoporte(string codHogar)
        {
            Encuesta objSesion = new Encuesta();
            try
            {
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                if (codHogar == null)
                    codHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);


                string codigoHogar = codHogar;
                gic_adminconfig adminConfig = new gic_adminconfig();
                List<gic_adminconfig> _Config = null;
                _Config = adminConfig.GetAdminConfiguracion("path.colilla");
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];
                string nombreArchivo = codigoHogar + ".pdf";
                gic_ArchivoColilla datosSol = new gic_ArchivoColilla();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(_Config.First().ADMINCFG_VALUE, nombreArchivo);
                    datosSol.arc_url = fileSavePath;
                    datosSol.usu_UsuarioCreacion = usuario;
                    datosSol.hog_codigo = codigoHogar;
                    datosSol.insertaArchivoColilla(datosSol);

                    httpPostedFile.SaveAs(fileSavePath);
                }

            }
            catch (Exception e)
            {
                log.Error("EncuestaController / cargarSoporte , Error: " + e.Message.ToString());
            }

        }

        [HttpPost]
        public void cargarConstanciaFirmada(string codHogar)
        {
            Encuesta objSesion = new Encuesta();
            try
            {
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                if (codHogar == null)
                    codHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);


                string codigoHogar = codHogar;
                gic_adminconfig adminConfig = new gic_adminconfig();
                List<gic_adminconfig> _Config = null;
                _Config = adminConfig.GetAdminConfiguracion("path.constanciasfirmadas");
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];
                string nombreArchivo = codigoHogar + ".pdf";
                gic_ArchivoColilla datosSol = new gic_ArchivoColilla();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(_Config.First().ADMINCFG_VALUE, "ConstanciaSAAH_" + nombreArchivo);
                    datosSol.arc_url = fileSavePath;
                    datosSol.usu_UsuarioCreacion = usuario;
                    datosSol.hog_codigo = codigoHogar;
                    datosSol.insertaConstanciaFirmada(datosSol);
                    httpPostedFile.SaveAs(fileSavePath);
                }

            }
            catch (Exception e)
            {
                log.Error("EncuestaController / cargarSoporte , Error: " + e.Message.ToString());
            }

        }


        [HttpPost]
        public void cargarSoporteTutor(string tipopersona)
        {
            Encuesta objSesion = new Encuesta();
            try
            {
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();


                gic_adminconfig adminConfig = new gic_adminconfig();
                List<gic_adminconfig> _Config = null;
                _Config = adminConfig.GetAdminConfiguracion("path.soportes");
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

                    //string sys = objSesion.getValorCampoSesion("SYSGUID", userIdApp);

                    httpPostedFile.SaveAs(fileSavePath);
                }

            }
            catch (Exception e)
            {
                log.Error("EncuestaController / cargarSoporte , Error: " + e.Message.ToString());
            }

        }

        [HttpPost]
        public void eliminarEncuesta()
        {

            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                gic_Hogar hogar = new gic_Hogar();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                hogar.eliminarEncuesta(codigoHogar, usuario);
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / eliminarEncuesta , Error: " + e.Message.ToString());
            }


        }

        [HttpPost]
        public JsonResult consultarCapitulosTerminados(string valor)
        {



            int totalCT = 0;
            string perfilusuario = string.Empty;
            try
            {
                if (valor.Equals("4"))
                {

                    Encuesta objSesion = new Encuesta();
                    string userIdApp;
                    userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                    string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                    perfilusuario = objSesion.getValorCampoSesion("PERFILES", userIdApp);
                    gic_Hogar hogar = new gic_Hogar();

                    totalCT = hogar.consultarEstadoEncuesta(codigoHogar, userIdApp, perfilusuario);

                    ViewBag.CerrarVentana = false;
                }
                else
                {
                    totalCT = 4;
                }


            }
            catch (Exception e)
            {
                totalCT = 0;
            }
            string retorno = totalCT.ToString() + '_' + perfilusuario;


            return Json(retorno, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult consultarperfil(string valor)
        {

            string perfilusuario = string.Empty;
            string retorno = string.Empty;
            try
            {

                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                perfilusuario = objSesion.getValorCampoSesion("PERFILES", userIdApp);

            }
            catch (Exception e)
            {
                perfilusuario = "0";
            }

            retorno = perfilusuario + '_' + perfilusuario;


            return Json(retorno, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult opcionesAbandonar(string valor)
        {

            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                string codigoHogar = objSesion.getValorCampoSesion("CODHOGAR", userIdApp);
                gic_Hogar hogar = new gic_Hogar();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                hogar.actualizarEstadoEncuesta(codigoHogar, usuario, valor);
                ViewBag.CerrarVentana = false;
                return View("Inicio");
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / opcionesAbandonar , Error: " + e.Message.ToString());
                return null;
            }


        }

        public void informePdf(string hogCodigo)
        {
            IgedEncuesta.Models.mdlGenerico.Tool.EscribeLog("from _informePdf .... " + hogCodigo, "prueba generacioninforme");
            try
            {
                string regex = @"(<.+?>|&nbsp;)";
                string estadoEncuesta;

                iTextSharp.text.Font font1 = new iTextSharp.text.Font(iTextSharp.text.FontFactory.GetFont("HELVETICA", 18f, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Font font2 = new iTextSharp.text.Font(iTextSharp.text.FontFactory.GetFont("TAHOMA", 14f, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Font font3 = new iTextSharp.text.Font(iTextSharp.text.FontFactory.GetFont("TAHOMA", 10f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Font font4 = new iTextSharp.text.Font(iTextSharp.text.FontFactory.GetFont("TAHOMA", 8f, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Font font5 = new iTextSharp.text.Font(iTextSharp.text.FontFactory.GetFont("TAHOMA", 10f, iTextSharp.text.BaseColor.BLACK));

                // Create a new pdf document object using the constructor. The parameters passed are document size, left margin, right margin, top margin and bottom margin.
                Document pdfDoc = new Document(PageSize.LETTER, 72, 72, 46, 46);

                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                //System.IO.MemoryStream msReport = new System.IO.MemoryStream();
                MemoryStream msReport = new MemoryStream();
                PdfWriter.GetInstance(pdfDoc, msReport);

                PdfPTable table = new PdfPTable(2);
                PdfPTable tblAnidada = new PdfPTable(2);

                table.DefaultCell.Border = PdfPCell.NO_BORDER;
                table.TotalWidth = 500f;

                //fix the absolute width of the table
                table.LockedWidth = true;

                //relative col widths in proportions - 1/3 and 2/3
                //float[] widths = new float[] { 2f, 4f, 6f };
                float[] widths = new float[] { 4, 26 };
                table.SetWidths(widths);
                table.HorizontalAlignment = 0;
                //leave a gap before and after the table
                table.SpacingBefore = 20f;
                table.SpacingAfter = 20f;
                table.WidthPercentage = 100;


                tblAnidada.SetWidths(new float[] { 10, 15 });

                pdfDoc.Open();
                string imagepath = Server.MapPath("/Content/Imagenes");

                using (FileStream fs = new FileStream(imagepath + "/Encabezado_Banner-03.png", FileMode.Open))
                {
                    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
                    png.ScaleToFit(500f, 500f);
                    pdfDoc.Add(png);
                }

                Paragraph heading = new Paragraph("INFORME ENTREVISTA HOGAR '" + hogCodigo + "'", font1);
                heading.SpacingAfter = 10f;
                heading.Alignment = Element.ALIGN_CENTER;

                pdfDoc.Add(heading);

                //clsInstrumentoNuevo objInsNuevo = new clsInstrumentoNuevo();
                gic_RespuestasxPersona respuestaXpersona = new gic_RespuestasxPersona();
                gic_Hogar hogar = new gic_Hogar();
                List<gic_RespuestasxPersona> respuestas = new List<gic_RespuestasxPersona>();
                List<gic_MiembroHogar> miembrosHogar = new List<gic_MiembroHogar>();

                respuestas = respuestaXpersona.get_RespuestasxPersona(hogCodigo);
                miembrosHogar = hogar.get_MiembrosHogar(hogCodigo);
                estadoEncuesta = hogar.get_estadoEncuesta(hogCodigo);

                string tema = "";
                string pregunta = "", nombre = "";

                PdfPCell cell = new PdfPCell(new Phrase("MIEMBROS DEL HOGAR.", font2));
                cell.Border = PdfPCell.TOP_BORDER;
                //cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingBottom = 10;
                cell.PaddingTop = 10;
                cell.Colspan = 2;
                table.AddCell(cell);


                foreach (gic_MiembroHogar miembro in miembrosHogar)
                {

                    cell = new PdfPCell(new Phrase(miembro.PRIMERNOMBRE + " " + miembro.SEGUNDONOMBRE + " " + miembro.PRIMERAPELLIDO + " " + miembro.SEGUNDOAPELLIDO + ". ", font4));
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Phrase.Leading = 8;
                    cell.PaddingBottom = 5;
                    cell.PaddingLeft = 20;
                    tblAnidada.AddCell(cell);

                    cell = new PdfPCell(new Phrase(miembro.ESTADO, font4));
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Phrase.Leading = 8;
                    cell.PaddingBottom = 5;
                    tblAnidada.AddCell(cell);

                }

                //cell = new PdfPCell(new Phrase(" ", font4));
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Phrase.Leading = 8;
                //cell.PaddingBottom = 2;
                //cell.PaddingLeft = 20;
                //cell.Colspan = 2;
                //table.AddCell(cell);

                PdfPCell celdaAnidacion = new PdfPCell(tblAnidada);
                celdaAnidacion.Colspan = 2;
                celdaAnidacion.Border = PdfPCell.NO_BORDER;
                celdaAnidacion.PaddingBottom = 10;
                celdaAnidacion.PaddingTop = 10;
                table.AddCell(celdaAnidacion);
                tblAnidada.DeleteBodyRows();

                #region insercion preguntas y respuestas.
                foreach (gic_RespuestasxPersona persxresp in respuestas)
                {
                    if (persxresp.tem_Orden == 1)
                    {
                        if (tema != persxresp.tem_NombreTema)
                        {
                            tema = persxresp.tem_NombreTema;
                            cell = new PdfPCell(new Phrase("CAPITULO  " + persxresp.tem_Orden.ToString() + ". " + tema, font2));
                            cell.Border = PdfPCell.TOP_BORDER;
                            //cell.Border = PdfPCell.NO_BORDER;
                            cell.PaddingBottom = 10;
                            cell.PaddingTop = 10;
                            cell.Colspan = 2;
                            table.AddCell(cell);
                        }


                        pregunta = Regex.Replace(persxresp.pre_Pregunta, regex, "").Trim();
                        //string step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
                        //string step2 = Regex.Replace(step1, @"\s{2,}", " ");
                        //if (pregunta.IndexOf("<font") != -1)  pregunta = pregunta.Substring(0, pregunta.IndexOf("<font"));

                        if (estadoEncuesta != "YA")
                        {
                            PdfPCell cellEstado1 = new PdfPCell(new Phrase("Estado de la entrevista:", font5));
                            cellEstado1.Border = PdfPCell.NO_BORDER;
                            cellEstado1.Phrase.Leading = 8;
                            cellEstado1.PaddingBottom = 5;
                            cellEstado1.PaddingLeft = 20;
                            tblAnidada.AddCell(cellEstado1);

                            PdfPCell cellEstado2 = new PdfPCell(new Phrase(estadoEncuesta, font5));
                            cellEstado2.Border = PdfPCell.NO_BORDER;
                            cellEstado2.Phrase.Leading = 8;
                            cellEstado2.PaddingBottom = 5;
                            tblAnidada.AddCell(cellEstado2);

                            estadoEncuesta = "YA";

                        }

                        PdfPCell cell1 = new PdfPCell(new Phrase(pregunta + ":", font5));
                        cell1.Border = PdfPCell.NO_BORDER;
                        cell1.Phrase.Leading = 8;
                        cell1.PaddingBottom = 5;
                        cell1.PaddingLeft = 20;
                        tblAnidada.AddCell(cell1);

                        PdfPCell cell2 = new PdfPCell(new Phrase(persxresp.res_Respuesta + ' ' + persxresp.rxp_TextoRespuesta, font5));
                        cell2.Border = PdfPCell.NO_BORDER;
                        cell2.Phrase.Leading = 8;
                        cell2.PaddingBottom = 5;
                        tblAnidada.AddCell(cell2);
                    }
                    else
                    {
                        if (tema != persxresp.tem_NombreTema)
                        {
                            if (tblAnidada.Rows.Count != 0)
                            {
                                celdaAnidacion = new PdfPCell(tblAnidada);
                                celdaAnidacion.Colspan = 2;
                                celdaAnidacion.PaddingBottom = 10;
                                celdaAnidacion.Border = PdfPCell.NO_BORDER;
                                table.AddCell(celdaAnidacion);
                                tblAnidada.DeleteBodyRows();
                            }




                            tema = persxresp.tem_NombreTema;
                            cell = new PdfPCell(new Phrase("CAPITULO " + persxresp.tem_Orden.ToString() + ". " + tema, font2));
                            cell.Border = PdfPCell.TOP_BORDER;
                            //cell.Border = PdfPCell.NO_BORDER;
                            cell.PaddingBottom = 10;
                            cell.PaddingTop = 10;
                            cell.Colspan = 2;
                            table.AddCell(cell);
                            pregunta = "";
                            //pdfDoc.NewPage();
                        }

                        if (pregunta != persxresp.pre_Pregunta)
                        {
                            if (tblAnidada.Rows.Count != 0)
                            {
                                celdaAnidacion = new PdfPCell(tblAnidada);
                                celdaAnidacion.Colspan = 2;
                                celdaAnidacion.Border = PdfPCell.NO_BORDER;
                                table.AddCell(celdaAnidacion);
                                tblAnidada.DeleteBodyRows();
                            }

                            //pregunta = persxresp.pre_Pregunta;
                            pregunta = Regex.Replace(persxresp.pre_Pregunta, regex, "").Trim();

                            //if (pregunta.IndexOf("<font") != -1)
                            //    pregunta = pregunta.Substring(0, pregunta.IndexOf("<font"));

                            cell = new PdfPCell(new Phrase("Pregunta. ", font3));
                            cell.Border = PdfPCell.NO_BORDER;
                            //cell.Phrase.Leading = 16;
                            cell.PaddingBottom = 10;
                            cell.PaddingTop = 10;
                            table.AddCell(cell);

                            PdfPCell cell2 = new PdfPCell(new Phrase(pregunta, font3));
                            cell2.Border = PdfPCell.NO_BORDER;
                            //cell2.Phrase.Leading = 16;
                            cell2.PaddingBottom = 10;
                            cell2.PaddingTop = 10;
                            table.AddCell(cell2);

                            pregunta = persxresp.pre_Pregunta;
                            nombre = "";
                        }

                        if (nombre != persxresp.per_Nombre)
                        {
                            nombre = persxresp.per_Nombre;

                            cell = new PdfPCell(new Phrase(persxresp.per_Nombre + ":", font4));
                            cell.Border = PdfPCell.NO_BORDER;
                            cell.Phrase.Leading = 8;
                            cell.PaddingBottom = 5;
                            cell.PaddingLeft = 20;
                            tblAnidada.AddCell(cell);

                            PdfPCell cell2 = new PdfPCell(new Phrase(persxresp.res_Respuesta + ' ' + persxresp.rxp_TextoRespuesta, font4));
                            cell2.Border = PdfPCell.NO_BORDER;
                            cell2.Phrase.Leading = 8;
                            cell2.PaddingBottom = 5;
                            tblAnidada.AddCell(cell2);

                        }

                    }


                }

                if (tblAnidada.Rows.Count != 0)
                {
                    celdaAnidacion = new PdfPCell(tblAnidada);
                    celdaAnidacion.Colspan = 2;
                    celdaAnidacion.PaddingBottom = 10;
                    celdaAnidacion.Border = PdfPCell.NO_BORDER;
                    table.AddCell(celdaAnidacion);
                    tblAnidada.DeleteBodyRows();
                }

                #endregion
                pdfDoc.Add(table);

                pdfDoc.Close();

                Response.Buffer = false;
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=informe_" + hogCodigo + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Write(pdfDoc);
                Response.BinaryWrite(msReport.ToArray());
                Response.End();
                //HttpContext.Response.AddHeader("content-disposition", "attachment; filename=form.pdf");
                // return File(msReport, "application/pdf");
                //mpeMsgBox2.Hide();
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / informePdf , Error: " + e.Message.ToString());
                // return null;
            }
            //return View();
        }

        public ActionResult direccionarEncuesta(string codigoHogar)
        {

            try
            {
                //Session["REPORTE"] = codigoHogar;
                return View("Encuesta");
            }
            catch (Exception e)
            {
                log.Error("EncuestaController / direccionarEncuesta , Error: " + e.Message.ToString());
                return null;
            }


        }
    }
    public class jsonValores
    {

        public string label { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }
}
