using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgedEncuesta.Filters;
using System.Data;
using ObjetosTipos;
using IgedEncuesta.Models.mdlGenerico;
using IgedEncuesta.Models.mdlEncuesta;
using Newtonsoft.Json;
using Autenticacion;
using IgedEncuesta.Models.Hogar;
using log4net;
using log4net.Config;
using AdministracionInstrumentos;
using IgedEncuesta.Models.mdlFuente;
using System.Diagnostics;




//using IgedEncuesta.Models.mdlAcceso;

namespace IgedEncuesta.Controllers
{


    public class ConformacionHogarController : Controller
    {

        ~ConformacionHogarController()
        {

        }

        private static readonly ILog log = LogManager.GetLogger("Web");
        public ConformacionHogarController()
        {
            XmlConfigurator.Configure();
        }



        public ActionResult Inicio()
        {
            try
            {
                string userIdApp, app;

                //limpiarVariablesSesion();
                cargarOpciones();

                TipoDocumento objTipoDoc = new TipoDocumento();
                ViewBag.TiposDoc = new SelectList(objTipoDoc.tiposDocumento(), "ID", "TIPO_DOC");
                ViewBag.CerrarVentana = false;

                Session["VerAyudas"] = false;
                ViewBag.VerAyudas = false;
                //comentar desde acá para publicar
                // ESTOS SON DE ADMINSUAURIOS
                
                List<NivelAcceso> coleccionNivelAcceso = new List<NivelAcceso>();
                NivelAcceso nivelAcceso = new NivelAcceso();
                var SesionIged = new HttpCookie("SesionIged");
                SesionIged["UserIdApp"] = "50214";
                SesionIged["App"] = "309";
                SesionIged["TknApp"] = "TOKEN";
                coleccionNivelAcceso = nivelAcceso.consultarNivelAcceso("50214", "309");
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccionNivelAcceso);
                SesionIged["NombresUsuario"] = "Andres Fernando Quintero Quiceno";
                SesionIged["USUARIO"] = "AFQUINTEROQ";
                SesionIged["NIVELACCESO"] = serializedData;
                SesionIged.Expires = DateTime.Now.AddHours(8);
                Response.Cookies.Add(SesionIged);
                string na = Request.Cookies["SesionIged"]["NIVELACCESO"].ToString(); ;
                coleccionNivelAcceso = JsonConvert.DeserializeObject<List<Autenticacion.NivelAcceso>>(na);
                var cookie = new HttpCookie("nivelAcceso", serializedData);
                HttpContext.Response.Cookies.Add(cookie);                
                
                
                // ESTOS SON DE ADMINSUAURIOSPRUEBAS
                /*
                List<NivelAcceso> coleccionNivelAcceso = new List<NivelAcceso>();
                NivelAcceso nivelAcceso = new NivelAcceso();
                var SesionIged = new HttpCookie("SesionIged");
                SesionIged["UserIdApp"] = "49965";
                SesionIged["App"] = "87";
                SesionIged["TknApp"] = "TOKEN";
                coleccionNivelAcceso = nivelAcceso.consultarNivelAcceso("49965", "87");
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccionNivelAcceso);
                SesionIged["NombresUsuario"] = "Andres Fernando Quintero Quiceno";
                SesionIged["USUARIO"] = "AFQUINTEROQ";
                SesionIged["NIVELACCESO"] = serializedData;
                SesionIged.Expires = DateTime.Now.AddHours(8);
                Response.Cookies.Add(SesionIged);
                string na = Request.Cookies["SesionIged"]["NIVELACCESO"].ToString(); ;
                coleccionNivelAcceso = JsonConvert.DeserializeObject<List<Autenticacion.NivelAcceso>>(na);
                var cookie = new HttpCookie("nivelAcceso", serializedData);
                HttpContext.Response.Cookies.Add(cookie);
                /*
                //hasta acá

                /****************************************************************************************
                 *  CAMBIOS PARA VALIDAR PERFIL ANDAGUEDA JAIME LOBATON 
                 * ***************************************************************************************/
                string perfilValidar = System.Web.Configuration.WebConfigurationManager.AppSettings["PerfilValidar"];

                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                app = Request.Cookies["SesionIged"]["App"].ToString();
                cargaPerfilesUsuario(userIdApp, app);
                Encuesta objSesion = new Encuesta();
                ////borrado session para el usuario
                objSesion.eliminarSesionIdUsuario(userIdApp);
                ////CAMBIO SESIONES --insercion primera vez
                string variables = "ID_USUARIO,PERFILES,VALIDAPERFIL,MODELO,MODELOHOGAR,VALINCLUIDO,GRUPOVICTIMA,CODHOGAR,";
                variables = variables + "TEMASVALIDAR,CAPTERMI,TEMA,IDTEMA,FLUJO,COLLECIONPERSONAS,PREGUNTAACTUAL,LISTADEPARTAMENTOS,";
                variables = variables + "LISTAMUNICIPIOS,OPCIONESRESPUESTA,LISTAAUTO,TEMAS,PREGUNTAINICIAL,LISTADEPARTAMENTOSDT,LISTADT,LISTAPA,SYSGUID,LISTAMU,MODELOPERSONA,MODELOUNIFICADO";
                objSesion.insertarVariablesSesion(variables, userIdApp);
                string perfil = cargaPerfilesUsuario(userIdApp, app);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "PERFILES", perfil);
                if (perfil == perfilValidar)
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALIDAPERFIL", "SI");
                else
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALIDAPERFIL", "NO");
                //JOSE VASQUEZ. FECHA: NOV.18.2015
                //MODIFICACION PARA LOGICA DE ENCUESTA ACTIVA
                ViewBag.EncuestaActiva = encuestaActiva();

                return View("~/Views/Encuesta/ConformacionHogar.cshtml");
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / Inicio , Error: " + e.Message.ToString());
                return null;
            }
        }

        public string cargaPerfilesUsuario(string userIdApp, string app)
        {
            Perfiles objPerfil = new Perfiles();
            objPerfil = objPerfil.listaPerfiles(userIdApp, app);
            return objPerfil.IDPERFIL;
        }

        //[ExpiraSesionFilter]
        public void cargarOpciones()
        {
            try
            {
                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem { Text = "DOCUMENTO", Value = "0" });  //ALFA NUMERICO
                //li.Add(new SelectListItem { Text = "CEDULA EXTRANJERIA", Value = "1" });  //ALFA NUMERICO 
                //------------------------------------------------------
                //COMENTA JOSE VASQUEZ   FECHA: OCT-27-2015
                //REQUERIMIENTO: SE DEBE DEJAR SOLO LA BUSQUEDA POR DOCUMENTO.
                //li.Add(new SelectListItem { Text = "NOMBRES Y APELLIDOS", Value = "1" });   //ALFANUMERICO 
                //FIN CAMBIO - FECHA: OCT-27-2015


                //ViewBag.Opciones = li;
                TempData["Opciones"] = li;
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / cargarOpciones  , Error: " + e.Message.ToString());
            }


        }


        public ActionResult prueba()
        {
            return View();
        }

        //[ExpiraSesionFilter]
        public ActionResult cargarMaestroVictima(string numeroDocumento, string opcionBusqueda)
        {
            try
            {
                //log.Info("ConformacionHogarController / cargarMaestroVictima , Entre : ");
                FuentePersona objConsultaFuentePersona = new FuentePersona();
                DataSet dsconsultaunificada = new DataSet();
                IDataReader dataReaderconsultaunificadaTUP = null;
                System.Collections.ArrayList arrayColeccion = new System.Collections.ArrayList();
                Victima objConsultaVictima = new Victima();
                Encuesta objSesion = new Encuesta();
                string userIdApp, app;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                app = Request.Cookies["SesionIged"]["App"].ToString();
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                List<Victima> coleccion = new List<Victima>();                

                DataSet datasetRegistraduria = new DataSet();

                bool cargarModelo = true;

                if (cargarModelo)
                {
                    switch (opcionBusqueda)
                    {
                        case "DOCUMENTO":
                            coleccion = objConsultaVictima.consultarVictimasMI(numeroDocumento, userIdApp, app);
                            //dsconsultaunificada = objConsultaFuentePersona.modeloRegistraduria(numeroDocumento);
                            ////25/04/2020 consultar metodo unificado, serializar y guardar en base de datos
                            /*var serializeMODELOUNIFICADO = Newtonsoft.Json.JsonConvert.SerializeObject(dsconsultaunificada);
                            objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOUNIFICADO", serializeMODELOUNIFICADO);*/
                            //
                            /*dataReaderconsultaunificadaTUP = dsconsultaunificada.Tables[1].CreateDataReader();
                            Victima victima = null;
                            while (dataReaderconsultaunificadaTUP.Read())
                            {
                                victima = new Victima();
                                victima.TIPO_DOC = dataReaderconsultaunificadaTUP["PER_TIPODOC"].ToString();
                                victima.DOCUMENTO = dataReaderconsultaunificadaTUP["PER_DOCUMENTO"].ToString();
                                victima.NOMBRE1 = dataReaderconsultaunificadaTUP["PER_NOMBRE1"].ToString();
                                victima.NOMBRE2 = dataReaderconsultaunificadaTUP["PER_NOMBRE2"].ToString();
                                victima.APELLIDO1 = dataReaderconsultaunificadaTUP["PER_APELLIDO1"].ToString();
                                victima.APELLIDO2 = dataReaderconsultaunificadaTUP["PER_APELLIDO2"].ToString();
                                victima.NOMBRES_COMPLETOS = victima.NOMBRE1 + ' ' + victima.NOMBRE2 + ' ' + victima.APELLIDO1 + ' ' + victima.APELLIDO2;
                                victima.F_NACIMIENTO = dataReaderconsultaunificadaTUP["PER_FECHANACIMIENTO"].ToString();
                                victima.EDAD = dataReaderconsultaunificadaTUP["EDAD"].ToString();
                                victima.GENERO_HOM = dataReaderconsultaunificadaTUP["PER_SEXO"].ToString();
                                victima.DISCAP = dataReaderconsultaunificadaTUP["PER_DISCAPACIDAD"].ToString();
                                victima.GENERO_HOM = dataReaderconsultaunificadaTUP["PER_IDENTIDAD_GENERO"].ToString();
                                victima.PERT_ETNICA = dataReaderconsultaunificadaTUP["PER_ETNIA"].ToString();
                                victima.HECHOS = dataReaderconsultaunificadaTUP["HECHOS"].ToString();
                                victima.FECHA_HECHO = dataReaderconsultaunificadaTUP["FECHA_HECHO"].ToString();
                                victima.FECHA_ULT_CARACTERIZACION = dataReaderconsultaunificadaTUP["FECHA_ULTI_ENCUESTA"].ToString();
                                victima.HABILITADO_PARA_CARACTERIZACION = dataReaderconsultaunificadaTUP["HABILITADO_CARAC"].ToString();
                                victima.COD_HOGAR = dataReaderconsultaunificadaTUP["COD_HOGAR"].ToString();
                                victima.ID_CARACTERIZACION = dataReaderconsultaunificadaTUP["ID_CARACTERIZACION"].ToString();
                                victima.TIPO_VICTIMA = dataReaderconsultaunificadaTUP["ESTADO_VICTIMA"].ToString().Trim();
                                if (dataReaderconsultaunificadaTUP["ESTADO_VICTIMA"].ToString().Equals("INCLUIDO"))
                                    victima.HABILITADO_PARA_CARACTERIZACION = "SI";
                                victima.CONS_PERSONA = dataReaderconsultaunificadaTUP["PER_ID"].ToString().Trim();

                                if (dataReaderconsultaunificadaTUP["PER_DOCUMENTO"].ToString().Length > 0 && dataReaderconsultaunificadaTUP["PER_NOMBRE1"].ToString().Length > 0
                                    && dataReaderconsultaunificadaTUP["PER_APELLIDO1"].ToString().Length > 0 && dataReaderconsultaunificadaTUP["PER_APELLIDO1"] != null)
                                    coleccion.Add(victima);


                            }*/
                            break;
                    }

                    
                    ViewBag.Lista = coleccion;
                    
                    var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccion);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELO", serializedData);
                    

                }

                return PartialView("_MaestroVictima", coleccion);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / cargarMaestroVictima , Error: " + e.Message.ToString());
                return null;
            }
        }

        public ActionResult cargarFuentePersona(string numeroDocumento, string opcionBusqueda)
        {
            try
            {
                
                FuentePersona objConsultaFuentePersona = new FuentePersona();
                Encuesta objSesion = new Encuesta();
                string userIdApp, app;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                app = Request.Cookies["SesionIged"]["App"].ToString();
                ViewBag.CerrarVentana = false;
                List<FuentePersona> coleccionFuente = new List<FuentePersona>();
                
                DataSet fichaunica_caracterizacion = new DataSet();
                DataSet dsSalidaRENEC = new DataSet();
                DataSet dsSalidaPMI = new DataSet();
                DataSet dsSalidaRUV = new DataSet();
                DataSet dsSalidaSIRAV = new DataSet();                
                DataSet dsconsultaunificada = new DataSet();
                IDataReader dataReaderconsultaunificada = null;


                bool cargarModelo = true;
                Victima objConsultaVictima = new Victima();
                FuentePersona objFuente = null;

                if (cargarModelo)
                {
                    
                    switch (opcionBusqueda)
                    {
                        case "DOCUMENTO":
                    
                            try
                            {
                                //coleccionFuente.Add(objConsultaFuentePersona.modeloRegistraduria(numeroDocumento));
                                //25/04/2020 leer modelo serializado de la base de datos
                                var MODELOUNIFICADO = objSesion.getValorCampoSesion("MODELOUNIFICADO", userIdApp);
                                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(MODELOUNIFICADO);
                                dsconsultaunificada = dataSet;
                               //dsconsultaunificada = objConsultaFuentePersona.modeloRegistraduria(numeroDocumento);
                                dataReaderconsultaunificada = dsconsultaunificada.Tables[0].CreateDataReader();

                                while (dataReaderconsultaunificada.Read())
                                {
                                    try
                                    {
                                        objFuente = new FuentePersona();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["FUENTE"])) objFuente.FUENTE = dataReaderconsultaunificada["FUENTE"].ToString();
                                        objFuente.CONS_PERSONA = "";
                                        objFuente.ID_TBPERSONA = "";
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["NUIP"])) objFuente.NUMERO_DOC = dataReaderconsultaunificada["NUIP"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["ESTADO_CEDULA"])) objFuente.ESTADO_CEDULA = dataReaderconsultaunificada["ESTADO_CEDULA"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["NOMBRE_completo"])) objFuente.NOMBRES_COMPLETOS = dataReaderconsultaunificada["NOMBRE_completo"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["GENERO"])) objFuente.GENERO = dataReaderconsultaunificada["GENERO"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["FECHANACIMIENTO"])) objFuente.FECHA_NACIMIENTO = dataReaderconsultaunificada["FECHANACIMIENTO"].ToString().Substring(0, 10);
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["ESTADO_VALORACION"])) objFuente.ESTADO_VALORACION = dataReaderconsultaunificada["ESTADO_VALORACION"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["NUM_FUD"])) objFuente.NUMERO_FORMULARIO = dataReaderconsultaunificada["NUM_FUD"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["NUM_DECL"])) objFuente.CODIGO_DECLARACION = dataReaderconsultaunificada["NUM_DECL"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["F_OCU"])) objFuente.FECHA_HECHO = dataReaderconsultaunificada["F_OCU"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["HECHO"])) objFuente.HECHO_VICTIMIZANTE = dataReaderconsultaunificada["HECHO"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["ENCUESTA"])) objFuente.CODIGO_HOGAR = dataReaderconsultaunificada["ENCUESTA"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["ESTADO_ENC"])) objFuente.ESTADO_ENCUESTA = dataReaderconsultaunificada["ESTADO_ENC"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["FECH_ENC"])) objFuente.FECHA_ENCUESTA = dataReaderconsultaunificada["FECH_ENC"].ToString();
                                        if (!DBNull.Value.Equals(dataReaderconsultaunificada["VIGENCIA_ENC"])) objFuente.VIGENCIA_ENCUESTA = dataReaderconsultaunificada["VIGENCIA_ENC"].ToString();
                                    }
                                    catch (Exception e)
                                    {
                                        e.Message.ToString();
                                    }

                                    coleccionFuente.Add(objFuente);

                                }

                                
                            }
                            catch (Exception e)
                            {
                                e.Message.ToString();
                            }

                            break;

                    }
                    ViewBag.Lista = coleccionFuente;
                }

                return PartialView("_FuentePersona", coleccionFuente);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / cargarMaestroFuente , Error: " + e.Message.ToString());
                return null;
            }
        }


        //[HttpPost]
        //[ExpiraSesionFilter]
        public ActionResult SelectVictima(bool isChecked, String id)
        {
            try
            {
                var selectList = (List<string>)TempData["SelectList"] ?? new List<string>();

                if (isChecked && !selectList.Contains(id))
                {
                    selectList.Clear();
                    selectList.Add(id);
                }

                TempData["SelectList"] = selectList;
                ViewBag.SelectList = JsonConvert.SerializeObject(selectList);
                
                TempData.Keep();
                return Json(selectList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / SelectVictima  , Error: " + e.Message.ToString());
                return null;
            }

        }

        //[ExpiraSesionFilter]
        public ActionResult agregarVictima(string idVictima)
        {
            List<Victima> modeloHogar = new List<Victima>();
            try
            {
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                Encuesta objSesion = new Encuesta();
                if (idVictima != null)
                {
                    Victima objConsultaVictima = new Victima();
                    //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                    ViewBag.CerrarVentana = false;
                    Victima victima = new Victima();
                    // TempData["ModeloHogar"] = new List<Victima>();
                    List<Victima> coleccion = new List<Victima>();


                    var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                    if (String.IsNullOrEmpty(modeloHogarJson))
                        modeloHogar = new List<Victima>();
                    else
                        modeloHogar = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);

                    //nueva lista victimas 
                    //coleccion = (List<Victima>)TempData["Modelo"];
                    //var modeloVictimas = objSesion.get_CampoSesion(userIdApp, "MODELO");
                    var modeloVictimas = objSesion.getValorCampoSesion("MODELO", userIdApp);
                    coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloVictimas);

                    victima = coleccion.First(x => x.CONS_PERSONA == idVictima.Substring(0, idVictima.IndexOf('|')));
                    modeloHogar.Insert(0, victima);
                    // modeloHogar.Add(victima);

                    //TempData["ModeloHogar"] = modeloHogar;
                    var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(modeloHogar);
                    //objSesion.guardarCampoSesion(2, int.Parse(userIdApp), "MODELOHOGAR", serializedData, "1");
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                    if (modeloHogar.Where(x => x.TIPO_VICTIMA == "NO INCLUIDO").Count() == modeloHogar.Count())
                        //TempData["VALINCLUIDO"] = "SI";
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "SI");
                    else
                        //TempData["VALINCLUIDO"] = "NO";
                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "NO");

                }

                /////andres quintero 16/10/2019
                List<SelectListItem> lst = new List<SelectListItem>();

                lst.Add(new SelectListItem() { Text = "Seleccione tipo persona", Value = "0" });
                lst.Add(new SelectListItem() { Text = "Autorizado", Value = "1" });
                lst.Add(new SelectListItem() { Text = "Tutor", Value = "2" });
                lst.Add(new SelectListItem() { Text = "CUIDADOR PERMANENTE", Value = "3" });
                lst.Add(new SelectListItem() { Text = "MIEMBRO HOGAR", Value = "4" });

                ViewBag.Opciones = lst;
                ////

                return PartialView("_GrupoVictima", modeloHogar);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / agregarVictima  , Error: " + e.Message.ToString());
                return null;
            }

        }

        //[ExpiraSesionFilter]
        public ActionResult excluirVictima(string idVictima)
        {
            string userIdApp;
            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            try
            {
                Victima objConsultaVictima = new Victima();
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                Victima victima = new Victima();
                Encuesta objSesion = new Encuesta();
                List<Victima> modeloHogar = new List<Victima>();
                var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                if (String.IsNullOrEmpty(modeloHogarJson))
                    modeloHogar = new List<Victima>();
                else
                {
                    modeloHogar = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);
                }


                victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima);

                modeloHogar.Remove(victima);

                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(modeloHogar);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);

                List<SelectListItem> lst = new List<SelectListItem>();

                lst.Add(new SelectListItem() { Text = "Seleccione tipo persona", Value = "0" });
                lst.Add(new SelectListItem() { Text = "Autorizado", Value = "1" });
                lst.Add(new SelectListItem() { Text = "Tutor", Value = "2" });
                lst.Add(new SelectListItem() { Text = "CUIDADOR PERMANENTE", Value = "3" });
                lst.Add(new SelectListItem() { Text = "MIEMBRO HOGAR", Value = "4" });

                ViewBag.Opciones = lst;
                ////

                return PartialView("_GrupoVictima", modeloHogar);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / excluirVictima , Error: " + e.Message.ToString());
                return null;
            }

        }

        //Andrés quintero 14/12/2019 borrarlo se creo para probar
        /*public List<Victima> intenter(string idVictima, string idPersona) {
            Encuesta objSesion = new Encuesta();
            string userIdApp;
            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
            List<Victima> coleccion = new List<Victima>();
            var modeloVictimas = objSesion.getValorCampoSesion("MODELO", userIdApp);
            coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloVictimas);
            Victima victima = new Victima();
            try {
                victima = coleccion.First(x => x.CONS_PERSONA == idVictima.Substring(0, idVictima.IndexOf('|')) && (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona);
            }
            catch (Exception e)
            {
                coleccion = null;
            }
            
            return coleccion;
        }*/

        //[ExpiraSesionFilter]
        public ActionResult actualizarMaestroHogar(string idVictima, string idPersona, string opcion)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                List<Victima> modeloHogar = new List<Victima>();
                if (idVictima != null)
                {
                    Victima objConsultaVictima = new Victima();
                    ViewBag.CerrarVentana = false;
                    DataSet dsSalida = new DataSet();
                    Victima victima = new Victima();
                    var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                    if (String.IsNullOrEmpty(modeloHogarJson))
                        modeloHogar = new List<Victima>();
                    else
                        modeloHogar = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);

                    if (opcion == "1")
                    {
                        List<Victima> coleccion = new List<Victima>();
                        var modeloVictimas = objSesion.getValorCampoSesion("MODELO", userIdApp);
                        coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloVictimas);

                        if (coleccion.Count > 0)
                        {

                            Boolean b = true;
                            foreach (var aPart in coleccion)
                            {
                                if (aPart.CONS_PERSONA == null)
                                {
                                    b = false;
                                }
                            }

                            if (b == true)
                            {
                                //Verifica si es un no incluido
                                if (idVictima.Substring(0, idVictima.IndexOf('|')) != "")
                                    try
                                    {
                                        victima = coleccion.First(x => x.CONS_PERSONA == idVictima.Substring(0, idVictima.IndexOf('|')) && (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona);
                                    }
                                    catch (Exception e)
                                    {
                                        e.Message.ToString();
                                    }

                                else
                                    victima = coleccion.First(x => (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona && x.TIPO_VICTIMA == "NO INCLUIDO");
                                modeloHogar.Insert(0, victima);
                            }
                        }

                    }
                    else if (opcion == "2")
                    {
                        if (idVictima != "")
                            victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima && (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona);
                        else
                            victima = modeloHogar.First(x => (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona && x.TIPO_VICTIMA == "NO INCLUIDO");

                        modeloHogar.Remove(victima);

                    }

                    var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(modeloHogar);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                    if (modeloHogar.Where(x => x.TIPO_VICTIMA == "NO INCLUIDO").Count() == modeloHogar.Count())

                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "SI");
                    else

                        objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "NO");

                }

                /////andres quintero 16/10/2019
                List<SelectListItem> lst = new List<SelectListItem>();
                lst.Add(new SelectListItem() { Text = "Seleccione tipo persona", Value = "0" });
                lst.Add(new SelectListItem() { Text = "Autorizado", Value = "1" });
                lst.Add(new SelectListItem() { Text = "Tutor", Value = "2" });
                lst.Add(new SelectListItem() { Text = "CUIDADOR PERMANENTE", Value = "3" });
                lst.Add(new SelectListItem() { Text = "MIEMBRO HOGAR", Value = "4" });
                ViewBag.Opciones = lst;


                return PartialView("_GrupoVictima", modeloHogar);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / actualizarMaestroHogar , Error: " + e.Message.ToString());
                return null;
            }

        }

        public ActionResult agregarVictimaNoIncluida(string datosVictima)
        {


            Encuesta objSesion = new Encuesta();
            string userIdApp;
            userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            try
            {
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                Victima victima = new Victima();
                List<Victima> coleccion = new List<Victima>();
                List<Victima> modeloHogar = new List<Victima>();
                var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                if (String.IsNullOrEmpty(modeloHogarJson))
                    modeloHogar = new List<Victima>();
                else
                {
                    modeloHogar = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);
                }

                victima.TIPO_DOC = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                victima.DOCUMENTO = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                victima.NOMBRE1 = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                victima.NOMBRE2 = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                victima.APELLIDO1 = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                victima.APELLIDO2 = datosVictima.Substring(0, datosVictima.IndexOf('|'));
                //victima.F_NACIMIENTO = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                datosVictima = datosVictima.Substring(datosVictima.IndexOf('|') + 1);
                DateTime date = Convert.ToDateTime(datosVictima);
                String format = "dd/MM/yyyy";
                datosVictima = date.ToString(format);
                victima.F_NACIMIENTO = datosVictima;

                victima.NOMBRES_COMPLETOS = victima.NOMBRE1 + ' ' + victima.NOMBRE2 + ' ' + victima.APELLIDO1 + ' ' + victima.APELLIDO2;
                victima.TIPO_VICTIMA = "NO INCLUIDO";

                Random r = new Random();
                string numAleatorio = "NI" + r.Next(10000, 200000).ToString();
                while (modeloHogar.Any(x => x.CONS_PERSONA == numAleatorio))
                {
                    numAleatorio = "NI" + r.Next(10000, 200000).ToString();
                }

                victima.CONS_PERSONA = numAleatorio;

                modeloHogar.Insert(0, victima);

                //TempData["ModeloHogar"] = modeloHogar;
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(modeloHogar);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                if (modeloHogar.Where(x => x.TIPO_VICTIMA == "NO INCLUIDO").Count() == modeloHogar.Count())
                    //TempData["VALINCLUIDO"] = "SI";
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "SI");
                else
                    //TempData["VALINCLUIDO"] = "NO";
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "VALINCLUIDO", "NO");

                /////andres quintero 16/10/2019
                List<SelectListItem> lst = new List<SelectListItem>();

                lst.Add(new SelectListItem() { Text = "Seleccione tipo persona", Value = "0" });
                lst.Add(new SelectListItem() { Text = "Autorizado", Value = "1" });
                lst.Add(new SelectListItem() { Text = "Tutor", Value = "2" });
                lst.Add(new SelectListItem() { Text = "CUIDADOR PERMANENTE", Value = "3" });
                lst.Add(new SelectListItem() { Text = "MIEMBRO HOGAR", Value = "4" });

                ViewBag.Opciones = lst;
                ////

                return PartialView("_GrupoVictima", modeloHogar);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / agregarVictimaNoIncluida , Error: " + e.Message.ToString());
                return null;
            }
        }

        //[ExpiraSesionFilter]
        public ActionResult grupoFamiliar(string consPersona)
        {
            try
            {
                Victima objConsultaVictima = new Victima();
                List<Victima> coleccion = new List<Victima>();
                List<Victima> coleccion2 = new List<Victima>();
                List<Victima> coleccionCarac = new List<Victima>();

                List<Persona> coleccion3 = new List<Persona>();
                List<string> idPersonas = new List<string>();
                Persona p = new Persona();
                int i = 0;


                DataSet dsSalida = new DataSet();
                DataSet dsSalida_Caracterizacion = new DataSet();

                dsSalida = objConsultaVictima.consultarGrupoFamiliar(consPersona);
                coleccion = objConsultaVictima.modeloVictimas(dsSalida);
                //JOSE VASQUEZ 03.NOV.2015
                //RECUPERA EL GRUPO FAMILIAR X CONS_PERSONA DESDE CARACTERIZACION
                coleccion3 = objConsultaVictima.consultarGpo_Familiar_x_Cons_Persona(consPersona);
                coleccionCarac = objConsultaVictima.RemueveRepetidos(coleccion, coleccion3);

                //FIN JOSE VASQUEZ 03.NOV.2015

                /************   BLOQUE DE CODIGO PARA VERIFICAR SI CADA MIEMBRO DEL GRUPO FAMILIAR REGISTRADO EN EL RUV TIENE ASOCIADA UNA PERSONA EN
                 ************   LA TABLA GIC_PERSONAS DE CARACTERIZACION ***/

                // Entra si encontró victimas registradas en el RUV para el número de documento suministrado
                if (coleccion.Count > 0)
                {
                    foreach (Victima item in coleccion)
                    {
                        // Verifica si la victima identificada en RUV ya fue caracterizada en la tabla GIC_RUV_PERSONAS
                        idPersonas = objConsultaVictima.consultarVictimasPersonas(item.CONS_PERSONA);
                        i = 0;
                        // Actualiza los datos de la Victima con los datos de la caracterización que se realizó
                        foreach (string persona in idPersonas)
                        {
                            i++;
                            p = objConsultaVictima.consultaDatosPersona(persona);
                            if (i == 1)
                            {
                                item.TIPO_DOC = p.TIPO_DOC;
                                item.DOCUMENTO = p.NUMERO_DOC;
                                item.NOMBRE1 = p.PRIMER_NOMBRE;
                                item.NOMBRE2 = p.SEGUNDO_NOMBRE;
                                item.APELLIDO1 = p.PRIMER_APELLIDO;
                                item.APELLIDO2 = p.SEGUNDO_APELLIDO;
                                item.NOMBRES_COMPLETOS = p.NOMBRES_COMPLETOS;
                                item.F_NACIMIENTO = p.FECHA_NACIMIENTO;
                                item.ID_TBPERSONA = persona;
                            }
                            else
                            {
                                Victima nuevoItem = new Victima();
                                nuevoItem.TIPO_DOC = p.TIPO_DOC;
                                nuevoItem.DOCUMENTO = p.NUMERO_DOC;
                                nuevoItem.NOMBRE1 = p.PRIMER_NOMBRE;
                                nuevoItem.NOMBRE2 = p.SEGUNDO_NOMBRE;
                                nuevoItem.APELLIDO1 = p.PRIMER_APELLIDO;
                                nuevoItem.APELLIDO2 = p.SEGUNDO_APELLIDO;
                                nuevoItem.NOMBRES_COMPLETOS = p.NOMBRES_COMPLETOS;
                                nuevoItem.F_NACIMIENTO = p.FECHA_NACIMIENTO;
                                nuevoItem.PERT_ETNICA = item.PERT_ETNICA;
                                nuevoItem.SOBREVIVENCIA = item.SOBREVIVENCIA;
                                nuevoItem.TIPO_VICTIMA = item.TIPO_VICTIMA;
                                nuevoItem.IDENTIFICADO = item.IDENTIFICADO;
                                nuevoItem.CONS_PERSONA = item.CONS_PERSONA;
                                nuevoItem.DISCAP = item.DISCAP;
                                nuevoItem.GENERO_HOM = item.GENERO_HOM;
                                nuevoItem.HV1 = item.HV1;
                                nuevoItem.HV2 = item.HV2;
                                nuevoItem.HV3 = item.HV3;
                                nuevoItem.HV4 = item.HV4;
                                nuevoItem.HV5 = item.HV5;
                                nuevoItem.HV6 = item.HV6;
                                nuevoItem.HV7 = item.HV7;
                                nuevoItem.HV8 = item.HV8;
                                nuevoItem.HV9 = item.HV9;
                                nuevoItem.HV10 = item.HV10;
                                nuevoItem.HV11 = item.HV11;
                                nuevoItem.HV12 = item.HV12;
                                nuevoItem.HV13 = item.HV13;
                                nuevoItem.HV14 = item.HV14;
                                nuevoItem.ID_TBPERSONA = persona;
                                coleccion2.Add(nuevoItem);
                            }

                        }

                    }
                }
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                coleccion.AddRange(coleccion2);
                coleccion.AddRange(coleccionCarac);

                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccion);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "GRUPOVICTIMA", serializedData);
                return PartialView("_GrupoFamiliar", coleccion);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / grupoFamiliar , Error: " + e.Message.ToString());
                return null;
            }

        }


        public ActionResult grupoFamiliarModeloIntegrado(string consPersona)
        {
            try
            {
                Victima objConsultaVictima = new Victima();
                List<Victima> coleccion = new List<Victima>();
                Persona p = new Persona();
                DataSet dsSalida = new DataSet();
                //dsSalida = objConsultaVictima.consultarGrupoFamiliarMI(consPersona);
                coleccion = objConsultaVictima.consultarGrupoFamiliarMI(consPersona);
                /************************************************************************************************************************************/
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                //TempData["GrupoVictima"] = coleccion;
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccion);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "GRUPOVICTIMA", serializedData);
                return PartialView("_GrupoFamiliar", coleccion);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / grupoFamiliar , Error: " + e.Message.ToString());
                return null;
            }

        }

        //[ExpiraSesionFilter]
        public ActionResult agregarMaestroHogarGF(string idVictima, string idPersona)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                List<Victima> modeloHogar = new List<Victima>();
                if (idVictima != null)
                {
                    Victima objConsultaVictima = new Victima();
                    //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                    //ViewBag.CerrarVentana = false;
                    //DataSet dsSalida = new DataSet();
                    //  DataSet dsSalida2 = new DataSet();
                    //  List<Parametros> param = new List<Parametros>();
                    Victima victima = new Victima();

                    //if (TempData["ModeloHogar"] == null)
                    //    TempData["ModeloHogar"] = new List<Victima>();
                    var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                    if (String.IsNullOrEmpty(modeloHogarJson))
                        modeloHogar = new List<Victima>();
                    else
                        modeloHogar = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);

                    //modeloHogar = (List<Victima>)TempData["ModeloHogar"];

                    // if (opcion == "1")
                    // {
                    List<Victima> coleccion = new List<Victima>();
                    var modeloVictimas = objSesion.getValorCampoSesion("GRUPOVICTIMA", userIdApp);
                    coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloVictimas);
                    //coleccion = (List<Victima>)TempData["GrupoVictima"];
                    //JOSE VASQUEZ . 03.NOV.2015
                    //VALIDACION CUANDO INCLUYE PERSONAS DE CARACTERIZACION QUE NO TIENE CONS_PERSONA
                    if (idVictima == String.Empty)
                        idVictima = "-1";
                    victima = coleccion.First(x => x.CONS_PERSONA == idVictima);
                    var validaRepetido = modeloHogar.Find(x => x.CONS_PERSONA == idVictima);
                    if (validaRepetido == null)
                        modeloHogar.Insert(0, victima);

                    //TempData["ModeloHogar"] = modeloHogar;
                    var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(modeloHogar);
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                }
                //  return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / agregarMaestroHogarGF , Error: " + e.Message.ToString());
                return null;
            }

        }

        //[ExpiraSesionFilter]
        public ActionResult actualizarJefeHogar(string consPersona)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                IEnumerable<Victima> coleccion = new List<Victima>();
                Victima item = new Victima();

                //coleccion = (IEnumerable<Victima>)TempData["ModeloHogar"];
                var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                if (String.IsNullOrEmpty(modeloHogarJson))
                    coleccion = new List<Victima>();
                else
                    coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);


                coleccion.All(x => { x.JEFE_HOGAR = false; return true; });
                foreach (var victima in coleccion.Where(w => w.CONS_PERSONA == consPersona))
                    victima.JEFE_HOGAR = true;


                //TempData["ModeloHogar"] = coleccion;
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccion);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                return Json('1', JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / actualizarJefeHogar , Error: " + e.Message.ToString());
                return null;
            }

        }

        //creado el 18/10/2019
        public ActionResult asignarTipoPersona(String consPersona, String idTipoPersona)
        {
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                IEnumerable<Victima> coleccion = new List<Victima>();
                Victima item = new Victima();

                var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                if (String.IsNullOrEmpty(modeloHogarJson))
                    coleccion = new List<Victima>();
                else
                    coleccion = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);


                foreach (var victima in coleccion.Where(w => w.CONS_PERSONA == consPersona))
                    victima.TIPO_PERSONA = idTipoPersona;

                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(coleccion);
                objSesion.guardarCampoSesion(int.Parse(userIdApp), "MODELOHOGAR", serializedData);
                return Json('1', JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / actualizarJefeHogar , Error: " + e.Message.ToString());
                return null;
            }

        }


        //creado por andrés quintero el 07/11/2019

        public ActionResult consultarPersona(String consPersona, String documento, String tipopersona)
        {
            try
            {

                Victima victima = new Victima();
                Encuesta objSesion = new Encuesta();
                List<Persona> lpersona = new List<Persona>();
                String userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                String perfilusuario = objSesion.getValorCampoSesion("PERFILES", userIdApp);
                lpersona = victima.gic_validar_persona_encuestada(documento, perfilusuario);
                int total = lpersona.Count();
                if (total > 0)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / actualizarJefeHogar , Error: " + e.Message.ToString());

                var st = new StackTrace(e, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MI_LOG_ERRORES_INTEGRACION obj = new MI_LOG_ERRORES_INTEGRACION();
                obj.insertaConstanciaFirmada(e.StackTrace.ToString() + ": " + e.Message.ToString() + ": linea: " + line, "consultarPersona(String consPersona, String documento, String tipopersona)");
                return null;
            }

        }


        //[ExpiraSesionFilter]
        public ActionResult verificarCodigoHogar(string codHogar)
        {
            try
            {
                Hogar objHogar = new Hogar();
                string mensaje = "";

                if (objHogar.consultarCodigoHogar(codHogar) == "1")
                {
                    if (objHogar.verificarCodigoMiembros(codHogar) == "0")
                    {
                        // generarEncuesta(codigo);
                    }
                    else
                    {
                        mensaje = "Este código de hogar ya tiene asociado un grupo familiar, por favor verifique la información.";
                    }
                }
                else
                {
                    mensaje = "El código de hogar suministrado no existe, por favor verifique la información.";
                }
                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error("ConformacionHogarController / verificarCodigoHogar , Error: " + e.Message.ToString());
                return null;
            }

        }

        public string encuestaActiva()
        {
            string hogCodigo = "", usuario = "", IdUsuario = ""; ;
            AdministracionInstrumentos.gic_Hogar objInsNuevo = new AdministracionInstrumentos.gic_Hogar();
            Encuesta objSesion = new Encuesta();
            usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
            IdUsuario = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
            //if (Session["USUARIO"] != null)
            if (usuario != null && usuario != "")
            {
                hogCodigo = objInsNuevo.encuestaActiva(usuario);
                objSesion.guardarCampoSesion(int.Parse(IdUsuario), "CODHOGAR", hogCodigo);
                //TempData["CODHOGAR"] = hogCodigo;

            }
            TempData.Keep();
            return hogCodigo;
        }

        //[ExpiraSesionFilter]
        public ActionResult iniciarEntrevista(string codHogar)
        {
            string mensaje = "", idHogar = "", idPersona = "";
            try
            {
                Encuesta objSesion = new Encuesta();
                string userIdApp; string Usuario;
                userIdApp = Request.Cookies["SesionIged"]["UserIdApp"].ToString();
                Usuario = Request.Cookies["SesionIged"]["USUARIO"].ToString();
                Hogar objHogar = new Hogar();
                string sys = objSesion.getValorCampoSesion("SYSGUID", userIdApp);
                string PERFILES = objSesion.getValorCampoSesion("PERFILES", userIdApp);

                IEnumerable<Victima> modelo = new List<Victima>();
                Victima objVictima = new Victima();

                var modeloHogarJson = objSesion.getValorCampoSesion("MODELOHOGAR", userIdApp);
                if (String.IsNullOrEmpty(modeloHogarJson))
                    modelo = new List<Victima>();
                else
                    modelo = JsonConvert.DeserializeObject<List<Victima>>(modeloHogarJson);

                if (codHogar == "")
                {
                    // Inserta un Código de Hogar nuevo si no se suministro
                    objHogar.insertarHogar(Usuario, userIdApp);
                }
                else
                {
                    // Acá actualiza el estado del hogar a 'ACTIVA' si este viene como código generado de manera manual                    
                    objSesion.guardarCampoSesion(int.Parse(userIdApp), "CODHOGAR", codHogar);
                    objHogar.actualizarEstadoEncuesta(codHogar, "5", Usuario);
                }

                //Obtiene el Codigo del Hogar
                if (codHogar == "")
                {
                    idHogar = objHogar.obtenerIdHogar(userIdApp);
                    //ACA ACTUALIZAR EL CODIGO DE HOGAr
                    objHogar.updateArchivosSoportes(sys, idHogar);
                }
                else
                {
                    idHogar = codHogar;
                    objHogar.updateArchivosSoportes(sys, idHogar);
                }

                objSesion.guardarCampoSesion(int.Parse(userIdApp), "CODHOGAR", idHogar);
                // Recorre el modelo de conformacion del hogar                
                foreach (Victima item in modelo)
                {
                    // Asigna fecha por defecto en el caso de que la victima no tenga una registrada
                    if (item.F_NACIMIENTO == "" || item.F_NACIMIENTO == null) item.F_NACIMIENTO = "01/01/1920";

                    //prueba jose 06.nov.2015
                    //objHogar.insertarPrueba(item);


                    // Insertar tabla Persona y Miembros del hogar
                    if (string.IsNullOrEmpty(item.ID_TBPERSONA))
                    {
                        idPersona = objHogar.insertarPersona(item, Usuario);
                        item.ID_TBPERSONA = idPersona;
                    }
                    else
                        idPersona = item.ID_TBPERSONA;

                    //JOSE VASQUEZ FECHA: NOV.11.2015
                    //Insertar Relación tabla intermedia RUV y Personas (Caracterizacion)
                    //item.Insertar_TIntermedia_RUV_y_Personas();
                    //FIN CAMBIO JOSE VASQUEZ FECHA: NOV.11.2015
                    // log.Error("MIEMBORA HOGAR IDPERSONA " + idPersona);
                    //log.Error("MIEMBORA HOGAR HOGAR " + idHogar);
                    if (item.TIPO_PERSONA.Equals(""))
                    {
                        item.JEFE_HOGAR = false;
                    }
                    if (item.TIPO_PERSONA.Equals("1"))
                    {
                        item.JEFE_HOGAR = true;
                    }
                    if (item.TIPO_PERSONA.Equals("2"))
                    {
                        item.JEFE_HOGAR = true;
                    }
                    if (item.TIPO_PERSONA.Equals("3"))
                    {
                        item.JEFE_HOGAR = true;
                    }
                    if (item.TIPO_PERSONA.Equals("4"))
                    {
                        item.JEFE_HOGAR = false;
                    }

                    objHogar.insertarMiembrosPorHogar(idHogar, idPersona, item.JEFE_HOGAR ? "SI" : "", Usuario, userIdApp);

                    // Insertar Validador Estado Victima
                    objHogar.insertarValidadorPorEstado(idPersona, idHogar, item.TIPO_VICTIMA, "1");

                    // Insertar Validador Por Parentesco
                    //andrés quintero
                    /*
                    string tipoPersona = item.TIPO_PERSONA;
                    if (!tipoPersona.Equals("") && (tipoPersona.Equals("1") || tipoPersona.Equals("2") || tipoPersona.Equals("3"))) {
                        item.JEFE_HOGAR = true;
                    }
                    */

                    //JOSE VASQUEZ FECHA: NOV.05.2015
                    //Insertar hechos victimizantes
                    objHogar.insertarHechosVictima(idPersona, item, idHogar);
                    //FIN CAMBIO JOSE VASQUEZ FECHA: NOV.05.2015

                    //Andrés Quintero 21/10/2019
                    //Inserta tipo persona
                    if (item.TIPO_PERSONA.Equals("1"))
                    {
                        objHogar.insertarValidadorTipoPersona(idPersona, idHogar, "5001", "1");
                    }
                    if (item.TIPO_PERSONA.Equals("2"))
                    {
                        objHogar.insertarValidadorTipoPersona(idPersona, idHogar, "5002", "1");
                    }
                    if (item.TIPO_PERSONA.Equals("3"))
                    {
                        objHogar.insertarValidadorTipoPersona(idPersona, idHogar, "5003", "1");
                    }
                    if (item.TIPO_PERSONA.Equals("4"))
                    {
                        objHogar.insertarValidadorTipoPersona(idPersona, idHogar, "5004", "1");
                    }

                    objHogar.insertarValidadorTiPerfil(idPersona, idHogar, PERFILES, "1");


                }
                mensaje = "1";
                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                gic_Hogar hogar = new gic_Hogar();
                HttpCookie reqCookies = Request.Cookies["SesionIged"];
                string usuario = reqCookies["USUARIO"].ToString();
                if (idHogar != "")
                {
                    hogar.actualizarEstadoEncuesta(idHogar, usuario, "6");
                }
                log.Error("ConformacionHogarController / iniciarEntrevista , Error: " + e.Message.ToString());
                var st = new StackTrace(e, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                MI_LOG_ERRORES_INTEGRACION obj = new MI_LOG_ERRORES_INTEGRACION();
                obj.insertaConstanciaFirmada(e.StackTrace.ToString() + ": " + e.Message.ToString() + ": linea: " + line, "iniciarEntrevista(string codHogar)");
                return null;
            }

        }

        public JsonResult validacionRegistraduria(string id)
        {
            Victima objConsultaVictima = new Victima();
            try
            {
                objConsultaVictima = new Victima();
                //if (id == "10305310261")
                //{
                //    objConsultaVictima.NOMBRE1 = "JAIME";
                //    objConsultaVictima.NOMBRE2 = "ANDRES";
                //    objConsultaVictima.APELLIDO1 = "LOBATON";
                //    objConsultaVictima.APELLIDO2 = "BARAJAS";
                //    objConsultaVictima.DOCUMENTO = "10305310261";
                //}
                //else {
                //    objConsultaVictima = objConsultaVictima.modeloRegistraduria(id);
                //}
                objConsultaVictima = objConsultaVictima.modeloRegistraduria(id);
            }
            catch
            {
                objConsultaVictima = new Victima();
            }
            return Json(objConsultaVictima, JsonRequestBehavior.AllowGet);
        }
    }
}
