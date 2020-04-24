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


//using IgedEncuesta.Models.mdlAcceso;

namespace IgedEncuesta.Controllers
{
    public class ConformacionHogarController : Controller
    {

        //public ActionResult Home(string u, string a, string t, string na, string p)
        //{
        //    TipoDocumento objTipoDoc = new TipoDocumento();
        //    SesionAplicacion objAplicacion = new SesionAplicacion();
        //    List<Parametros> param = new List<Parametros>();
        //    string tokenPortal;
        //    List<string> permisos = new List<string>();

        //    List<Token> tokens = new List<Token>();
        //    Token objToken = new Token();


        //    Session["UserIdApp"] = ViewBag.UserIdApp = u;
        //    Session["App"] = ViewBag.App = a;
        //    Session["TknApp"] = t;
        //    Session["IdNivelAcceso"] = na;
        //    Session["IdPortal"] = p;

        //    ViewBag.CerrarVentana = false;
        //    Session["VerAyudas"] = false;
        //    ViewBag.VerAyudas = false;

        //    cargarOpciones();

        //    ViewBag.TiposDoc = new SelectList(objTipoDoc.tiposDocumento(), "ID", "TIPO_DOC");

        //   //-------------------------
        //    try
        //    {
        //        if (Session["UserIdApp"] != null)
        //            tokens = objToken.consultarTokenAplicacionPadre(Session["UserIdApp"].ToString(), p) ?? new List<Token>();
        //        if (tokens.Any(x => x.idAplicacion == a)) // Verifica que ya exista un token para la aplicacion
        //        {
        //            objToken = tokens.Find(x => x.idAplicacion == a);
        //            if (t == objToken.token) // SE VALIDA SI EL TOKEN DE LA APLICACION CORRESPONDE AL TOKEN EN LA BD
        //            {
        //                string MAC = objAplicacion.GetMACAddress();

        //                objAplicacion.getActualizarTokenAplicacion(u, a, p, t, MAC, out param);

        //                if (param.Find(x => x.Nombre == "p_Salida").Valor == "1") // SE VALIDA SI SE GENERO BIEN UN NUEVO TOKEN DE APLICACION
        //                {
        //                    Session["TknApp"] = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;
        //                    ViewBag.tk = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;

        //                    tokenPortal = tokens.Find(x => x.idAplicacion == p).token;

        //                    // Crea la cookie de sesion para el módulo IGED
        //                    var SesionIged = new HttpCookie("SesionIged");
        //                    SesionIged["UserIdApp"] = u;
        //                    SesionIged["App"] = a;
        //                    SesionIged["TknApp"] = param.Find(x => x.Nombre == "p_TokenGenerado").Valor;
        //                    SesionIged["Fecha"] = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
        //                    SesionIged.Expires = DateTime.Now.AddHours(8);
        //                    Response.Cookies.Add(SesionIged);

        //                    ViewBag.CerrarVentana = false;
        //                    ViewBag.Lista = null;
        //                    cargarOpciones();
        //                    ViewBag.BusquedaMaestro = "SI";

        //                    param = new List<Parametros>();
        //                    objAplicacion.nombresCompletosUsuario(u, out param);
        //                    Session["NombresUsuario"] = param.Find(x => x.Nombre == "p_Nombres").Valor;

        //                    // Consulta las fuentes externas configuradas y disponibles en la base de datos
        //                    //listaFuentes = objFuente.listaFuentesExternas();
        //                    //Session["listaFuentes"] = listaFuentes;

        //                    //ConsultaIndividual objConsultaIndividual = new ConsultaIndividual();
        //                    //permisos = objConsultaIndividual.PermisosNivelAcceso(u, a);
        //                    //if (permisos.Contains("1")) Session["VerAyudas"] = true;

        //                    //Session["PerfilNivelAcceso"] = objConsultaIndividual.PerfilNivelAcceso(u, a);

        //                    return View("~/Views/Encuesta/ConformacionHogar.cshtml");
        //                }
        //                else
        //                    TempData["inv"] = "Token de Sesión para la aplicación invalido.";
        //            }
        //            else
        //                TempData["inv"] = "Token de Sesión para la aplicación invalido.";
        //        }
        //        else
        //            TempData["inv"] = "Token de Sesión para la aplicación invalido.";

        //        //-----------

        //    }
        //    catch (Exception e)
        //    {
        //        TempData["inv"] = e.Message.ToString();
        //    }

        //    return View("SesionInvalida");
        //}


        public ActionResult Inicio() {

            cargarOpciones();
            TipoDocumento objTipoDoc = new TipoDocumento();
            ViewBag.TiposDoc = new SelectList(objTipoDoc.tiposDocumento(), "ID", "TIPO_DOC");
            ViewBag.CerrarVentana = false;
            Session["VerAyudas"] = false;
            ViewBag.VerAyudas = false;
            return View("~/Views/Encuesta/ConformacionHogar.cshtml");
        }

        [ExpiraSesionFilter]
        public void cargarOpciones()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "DOCUMENTO", Value = "0" });  //ALFA NUMERICO 
            //------------------------------------------------------
            //COMENTA JOSE VASQUEZ   FECHA: OCT-27-2015
            //REQUERIMIENTO: SE DEBE DEJAR SOLO LA BUSQUEDA POR DOCUMENTO.
            //li.Add(new SelectListItem { Text = "NOMBRES Y APELLIDOS", Value = "1" });   //ALFANUMERICO 
            //FIN CAMBIO - FECHA: OCT-27-2015


            //ViewBag.Opciones = li;
            Session["Opciones"] = li;

        }

        [ExpiraSesionFilter]
        public ActionResult cargarMaestroVictima(string numeroDocumento, string opcionBusqueda)
        {
            Victima objConsultaVictima = new Victima();
            ViewBag.CerrarVentana = false;
            DataSet dsSalida = new DataSet();
            IEnumerable<Victima> coleccion = new List<Victima>();
            TempData["msg"] = "";
            TempData["err"] = "";
            bool cargarModelo = false;

            if (Session["SelectList"] == null)
                Session["SelectList"] = new List<string>();



            if (Session["NumeroDocumento"] != null)
            {
                    Session["NumeroDocumento"] = numeroDocumento;
                    cargarModelo = true;
            }
            else
            {
                Session["NumeroDocumento"] = numeroDocumento;
                cargarModelo = true;
            }

            if (cargarModelo)
            {
                switch (opcionBusqueda)
                {
                    case "DOCUMENTO":
                        coleccion = objConsultaVictima.consultarVictimas(numeroDocumento, opcionBusqueda);

                        break;
                    //case "NOMBRES Y APELLIDOS":
                    //    dsSalida = objConsultaIndividual.consultarFuentesOracle(numeroDocumento, Session["UserIdApp"].ToString(), opcionBusqueda);
                    //    dsSalida2 = objConsultaIndividual.consultarFuenteSIRAV(numeroDocumento, opcionBusqueda);
                    //    coleccion = objMaestroConsulta.cargarModelo(dsSalida, dsSalida2);
                    //    break;
                }

                ViewBag.Lista = coleccion;
                //  objConsultaIndividual.insertarAuditoria("1", Session["UserIdApp"].ToString(), opcionBusqueda + ";" + numeroDocumento);
                //Session.Add("Modelo", coleccion);
                Session["Modelo"] = coleccion;
                Session["SelectList"] = null;
            }

            return PartialView("_MaestroVictima", (IEnumerable < Victima >)Session["Modelo"]);
        }

        //[HttpPost]
        [ExpiraSesionFilter]
        public ActionResult SelectVictima(bool isChecked, String id)
        {
            //List<SelectListItem> list = List(Session["SelectList"]) ?? new List<SelectListItem>();

            var selectList = (List<string>)Session["SelectList"] ?? new List<string>();
            //var selectList = (List)HttpContext.Session["SelectList"] ?? new List();

            if (isChecked && !selectList.Contains(id))
            {
                selectList.Clear();
                selectList.Add(id);
            }

            Session["SelectList"] = selectList;
            ViewBag.SelectList = JsonConvert.SerializeObject(selectList);
            //ViewBag.SelectList = selectList;

            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        [ExpiraSesionFilter]
        public ActionResult agregarVictima(string idVictima)
        {
            
            if (idVictima != null)
            {
                Victima objConsultaVictima = new Victima();
                //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                //  DataSet dsSalida2 = new DataSet();
                //  List<Parametros> param = new List<Parametros>();
                Victima victima = new Victima();

                if (Session["ModeloHogar"] == null)
                    Session["ModeloHogar"] = new List<Victima>();

                List<Victima> coleccion = new List<Victima>();
                List<Victima> modeloHogar = new List<Victima>();

                modeloHogar = (List<Victima>)Session["ModeloHogar"];

                coleccion = (List<Victima>)Session["Modelo"];
                victima = coleccion.First(x => x.CONS_PERSONA == idVictima.Substring(0, idVictima.IndexOf('|')));
                modeloHogar.Insert(0, victima);
                // modeloHogar.Add(victima);

                Session["ModeloHogar"] = modeloHogar;
                Session["SelectList"] = null;
            }

            return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
        }

        [ExpiraSesionFilter]
        public ActionResult excluirVictima(string idVictima)
        {

            Victima objConsultaVictima = new Victima();
            //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
            ViewBag.CerrarVentana = false;
            DataSet dsSalida = new DataSet();
            //  DataSet dsSalida2 = new DataSet();
            //  List<Parametros> param = new List<Parametros>();
            Victima victima = new Victima();

            if (Session["ModeloHogar"] == null)
                Session["ModeloHogar"] = new List<Victima>();

            // List<Victima> coleccion = new List<Victima>();
            List<Victima> modeloHogar = new List<Victima>();

            modeloHogar = (List<Victima>)Session["ModeloHogar"];

            // coleccion = (List<Victima>)Session["Modelo"];
            victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima);
            // modeloHogar.Insert(0, victima);
            // modeloHogar.Add(victima);

            //Find a Single Student in the List.
            // Student s = students.Where(s => s.StudentId == myStudentId).Single();
            //Remove that student from the list.
            modeloHogar.Remove(victima);

            Session["ModeloHogar"] = modeloHogar;
            // Session["SelectList"] = null;

            return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
        }

        [ExpiraSesionFilter]
        public ActionResult actualizarMaestroHogar(string idVictima, string idPersona, string opcion)
        {
            if (idVictima != null)
            {
                Victima objConsultaVictima = new Victima();
                //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                //  DataSet dsSalida2 = new DataSet();
                //  List<Parametros> param = new List<Parametros>();
                Victima victima = new Victima();

                if (Session["ModeloHogar"] == null)
                    Session["ModeloHogar"] = new List<Victima>();

                // List<Victima> coleccion = new List<Victima>();
                List<Victima> modeloHogar = new List<Victima>();

                modeloHogar = (List<Victima>)Session["ModeloHogar"];

                if (opcion == "1")
                {
                    List<Victima> coleccion = new List<Victima>();
                    coleccion = (List<Victima>)Session["Modelo"];

                    //Verifica si es un no incluido
                    if (idVictima.Substring(0, idVictima.IndexOf('|')) != "")
                        victima = coleccion.First(x => x.CONS_PERSONA == idVictima.Substring(0, idVictima.IndexOf('|')) && (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA ) == idPersona);
                    else
                        victima = coleccion.First(x => (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona && x.TIPO_VICTIMA == "NO INCLUIDO");
                    modeloHogar.Insert(0, victima);
                    Session["SelectList"] = null;
                }
                else if (opcion == "2")
                {
                    // coleccion = (List<Victima>)Session["Modelo"];
                   // if (modeloHogar.Any(x => x.CONS_PERSONA == idVictima))
                  //  {
                        if (idVictima != "")
                            victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima && (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona);
                        else
                            victima = modeloHogar.First(x => (x.ID_TBPERSONA == null ? "" : x.ID_TBPERSONA) == idPersona && x.TIPO_VICTIMA == "NO INCLUIDO");
                        // modeloHogar.Insert(0, victima);
                        // modeloHogar.Add(victima);

                        //Find a Single Student in the List.
                        // Student s = students.Where(s => s.StudentId == myStudentId).Single();
                        //Remove that student from the list.
                        modeloHogar.Remove(victima);
                 //   }
                }
                Session["ModeloHogar"] = modeloHogar;
                // Session["SelectList"] = null;
            }
            return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
        }
        //[ExpiraSesionFilter]
        //public JsonResult excluirVictima(string idVictima)
        //{
        //    Victima objConsultaVictima = new Victima();
        //    //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
        //    ViewBag.CerrarVentana = false;
        //    DataSet dsSalida = new DataSet();
        //    //  DataSet dsSalida2 = new DataSet();
        //    //  List<Parametros> param = new List<Parametros>();
        //    Victima victima = new Victima();

        //    if (Session["ModeloHogar"] == null)
        //        Session["ModeloHogar"] = new List<Victima>();

        //    // List<Victima> coleccion = new List<Victima>();
        //    List<Victima> modeloHogar = new List<Victima>();

        //    modeloHogar = (List<Victima>)Session["ModeloHogar"];

        //    // coleccion = (List<Victima>)Session["Modelo"];
        //    victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima);
        //    // modeloHogar.Insert(0, victima);
        //    // modeloHogar.Add(victima);

        //    //Find a Single Student in the List.
        //    // Student s = students.Where(s => s.StudentId == myStudentId).Single();
        //    //Remove that student from the list.
        //    modeloHogar.Remove(victima);

        //    Session["ModeloHogar"] = modeloHogar;
        //    // Session["SelectList"] = null;

        //    return Json(idVictima);
        //}


        //[ExpiraSesionFilter]
        //public JsonResult tiposDocumento()
        //{

        //    DataSet dsSalida = new DataSet();
        //    TipoDocumento objTipoDoc = new TipoDocumento();
        //    List<TipoDocumento> lstTipoDoc = new List < TipoDocumento>();

        //    dsSalida = objTipoDoc.consultarBDTiposDoc();
        //    lstTipoDoc = objTipoDoc.modeloTipoDocumento(dsSalida);

        //    return Json(lstTipoDoc, JsonRequestBehavior.AllowGet);
        //}

        [ExpiraSesionFilter]
        public ActionResult agregarVictimaNoIncluida(string datosVictima)
        {


//                Victima objConsultaVictima = new Victima();
                //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                ViewBag.CerrarVentana = false;
                DataSet dsSalida = new DataSet();
                //  DataSet dsSalida2 = new DataSet();
                //  List<Parametros> param = new List<Parametros>();
                Victima victima = new Victima();

                if (Session["ModeloHogar"] == null)
                    Session["ModeloHogar"] = new List<Victima>();

                List<Victima> coleccion = new List<Victima>();
                List<Victima> modeloHogar = new List<Victima>();

                modeloHogar = (List<Victima>)Session["ModeloHogar"];

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
                victima.APELLIDO2 = datosVictima;

                victima.NOMBRES_COMPLETOS = victima.NOMBRE1 + ' ' + victima.NOMBRE2 + ' ' + victima.APELLIDO1 + ' ' + victima.APELLIDO2;
                victima.TIPO_VICTIMA = "NO INCLUIDO";

                Random r = new Random();
                string numAleatorio = "NI" + r.Next(10000, 200000).ToString();
                while (modeloHogar.Any(x => x.CONS_PERSONA ==  numAleatorio))
                {
                    numAleatorio = "NI" + r.Next(10000, 200000).ToString();
                }

                victima.CONS_PERSONA = numAleatorio;

                modeloHogar.Insert(0, victima);
                // modeloHogar.Add(victima);

                Session["ModeloHogar"] = modeloHogar;
              //  Session["SelectList"] = null;


            return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
        }

        [ExpiraSesionFilter]
        public ActionResult grupoFamiliar(string consPersona)
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

            coleccion.AddRange(coleccion2);
            coleccion.AddRange(coleccionCarac);
            /************************************************************************************************************************************/
            
            Session["GrupoVictima"] = coleccion;
            
            return PartialView("_GrupoFamiliar", coleccion);
        }

        [ExpiraSesionFilter]
        public ActionResult agregarMaestroHogarGF(string idVictima, string idPersona)
        {
            if (idVictima != null)
            {
                Victima objConsultaVictima = new Victima();
                //  MaestroConsulta objMaestroConsulta = new MaestroConsulta();
                //ViewBag.CerrarVentana = false;
                //DataSet dsSalida = new DataSet();
                //  DataSet dsSalida2 = new DataSet();
                //  List<Parametros> param = new List<Parametros>();
                Victima victima = new Victima();

                if (Session["ModeloHogar"] == null)
                    Session["ModeloHogar"] = new List<Victima>();

                // List<Victima> coleccion = new List<Victima>();
                List<Victima> modeloHogar = new List<Victima>();

                modeloHogar = (List<Victima>)Session["ModeloHogar"];

               // if (opcion == "1")
               // {
                    List<Victima> coleccion = new List<Victima>();
                    coleccion = (List<Victima>)Session["GrupoVictima"];
                //JOSE VASQUEZ . 03.NOV.2015
                //VALIDACION CUANDO INCLUYE PERSONAS DE CARACTERIZACION QUE NO TIENE CONS_PERSONA
                    if (idVictima == String.Empty)
                        idVictima = "-1";
                    victima = coleccion.First(x => x.CONS_PERSONA == idVictima);
                    modeloHogar.Insert(0, victima);
                  //  Session["SelectList"] = null;
             //   }
              //  else
               // {
                    // coleccion = (List<Victima>)Session["Modelo"];
                //    if (modeloHogar.Any(x => x.CONS_PERSONA == idVictima))
              //      {
                       // victima = modeloHogar.First(x => x.CONS_PERSONA == idVictima);
                        // modeloHogar.Insert(0, victima);
                        // modeloHogar.Add(victima);

                        //Find a Single Student in the List.
                        // Student s = students.Where(s => s.StudentId == myStudentId).Single();
                        //Remove that student from the list.
               //         modeloHogar.Remove(victima);
                //    }
              //  }
                Session["ModeloHogar"] = modeloHogar;
                // Session["SelectList"] = null;
            }
          //  return PartialView("_GrupoVictima", (IEnumerable<Victima>)Session["ModeloHogar"]);
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        [ExpiraSesionFilter]
        public ActionResult actualizarJefeHogar(string consPersona)
        {
            IEnumerable<Victima> coleccion = new List<Victima>();
            Victima item = new Victima();

            coleccion = (IEnumerable<Victima>)Session["ModeloHogar"];
            coleccion.All(x => { x.JEFE_HOGAR = false; return true; });

            foreach (var victima in coleccion.Where(w => w.CONS_PERSONA == consPersona))
                victima.JEFE_HOGAR = true;

            Session["ModeloHogar"] = coleccion;

            //item = coleccion.FirstOrDefault(x => x.CONS_PERSONA == consPersona);
            //item.JEFE_HOGAR = true;
            

            return Json('1', JsonRequestBehavior.AllowGet);
            //return PartialView("_MaestroVictima", (IEnumerable<Victima>)Session["Modelo"]);
        }

        [ExpiraSesionFilter]
        public ActionResult verificarCodigoHogar(string codHogar)
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

        [ExpiraSesionFilter]
        public ActionResult iniciarEntrevista(string codHogar)
        {
            Hogar objHogar = new Hogar();
            string mensaje = "", idHogar = "", idPersona = "";

            IEnumerable<Victima> modelo = new List<Victima>();
            Victima objVictima = new Victima();

            modelo = (IEnumerable<Victima>)Session["ModeloHogar"];

            if (codHogar == "")
            {
                // Inserta un Código de Hogar nuevo si no se suministro
                objHogar.insertarHogar();
            }
            else
            {
                // Acá actualiza el estado del hogar a 'ACTIVA' si este viene como código generado de manera manual
                Session["CODHOGAR"] = codHogar;
                objHogar.actualizarEstadoEncuesta(Session["CODHOGAR"].ToString(), "5");
            }

            //Obtiene el Codigo del Hogar
            if (codHogar == "")
                idHogar = objHogar.obtenerIdHogar();
            else
                idHogar = codHogar;

            Session["CODHOGAR"] = idHogar;
            // Recorre el modelo de conformacion del hogar
            foreach (Victima item in modelo)
            {
                // Asigna fecha por defecto en el caso de que la victima no tenga una registrada
                if (item.F_NACIMIENTO == "" || item.F_NACIMIENTO == null) item.F_NACIMIENTO = "01/01/0001";

                // Insertar tabla Persona y Miembros del hogar
                idPersona = objHogar.insertarPersona(item);
                objHogar.insertarMiembrosPorHogar(idHogar, idPersona, item.JEFE_HOGAR ? "SI" : "");

                // Insertar Validador Estado Victima
                objHogar.insertarValidadorPorEstado(idPersona, idHogar,  item.TIPO_VICTIMA, "1");
                
                // Insertar Validador Por Parentesco
                objHogar.insertarValidadorPorParentesco(idPersona, idHogar, item.JEFE_HOGAR ? "JEFE" : "NO JEFE", "1");



            }


            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
    }
}
