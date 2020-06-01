using IgedEncuesta.Models.mdlGenerico;
using log4net;
using log4net.Config;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_Pregunta : gic_AdministradorDeCambios
    {
        private static readonly ILog log = LogManager.GetLogger("Web");
        public gic_Pregunta()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Identificador unico de la pregunta
        /// </summary>
        public int pre_IdPregunta { get; set; }

        /// <summary>
        /// Pregunta
        /// </summary>
        public string pre_pregunta { get; set; }

        /// <summary>
        /// Tipo Campo
        /// </summary>
        public string pre_TipoCampo { get; set; }

        /// <summary>
        /// Obligatorio
        /// </summary>
        public string pre_Obligatorio { get; set; }

        /// <summary>
        /// Numero Pregunta
        /// </summary>
        public int pre_NumeroPregunta { get; set; }

        /// <summary>
        /// Relacion Gic_Tema
        /// </summary>
        public gic_Tema tem_IdTema { get; set; }

        /// <summary>
        /// Estado pregunta
        /// </summary>
        public string pre_Activa { get; set; }

        /// <summary>
        /// Id Padre Pregunta-depende
        /// </summary>
        public int pre_IdPadre { get; set; }

        /// <summary>
        /// Id Padre Pregunta-depende
        /// </summary>
        public string pre_TipoPregunta { get; set; }

        /// <summary>
        /// Tipo Validador
        /// </summary>
        public string pre_TipoValidador { get; set; }

        /// <summary>
        /// Longitud Campo
        /// </summary>
        public int pre_LongCampo { get; set; }

        /// <summary>
        ///  Validador
        /// </summary>
        public string pre_Validador { get; set; }

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        /// Obtiene la siguiente pregunta del flujo de cada formulario.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar de la encuesta</param>
        /// <param name="idTema">Id del tema donde se va a buscar la siguiente pregunta</param>
        /// <param name="idInstrumento">Id del Instrumento donde se va a buscar la siguiente pregunta</param>
        /// <param name="idPregunta">Id de la pregunta para saber el orden de ingreso </param>
        /// <param name="tipo">Tipo de flujo , para saber si es de CONSULTA O EDICION</param>
        /// <returns> List<gic_PreguntasxPersona> generada </returns>
        public List<gic_PreguntasxPersona> getPreguntaSiguiente(string codHogar, int idTema, int idInstrumento, int idPregunta/*, string tipo*/)
        {
            string stored = string.Empty;
            stored = "GIC_N_CARACTERIZACION.SP_BUSCAR_SIGUIENTE_PREGUNTA";
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", codHogar));
            param.Add(baseDatos.asignarParametro("pID_TEMA", 1, "System.Int32", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("pINS_IDINSTRUMENTO", 1, "System.Int32", idInstrumento.ToString()));
            param.Add(baseDatos.asignarParametro("pID_PREGUNTA", 1, "System.Int32", idPregunta.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            
            List<gic_PreguntasxPersona> preguntas = new List<gic_PreguntasxPersona>();
            try
            {
                #region DataReader
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_PreguntasxPersona pregunta = new gic_PreguntasxPersona();
                        if (!DBNull.Value.Equals(dataReader["PRE_IDPREGUNTA"]))
                        {
                            pregunta.pre_IdPregunta = int.Parse(dataReader["PRE_IDPREGUNTA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_PREGUNTA"]))
                        {
                            pregunta.pre_pregunta = dataReader["PRE_PREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_TIPOPREGUNTA"]))
                        {
                            pregunta.pre_TipoPregunta = dataReader["PRE_TIPOPREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_TIPOCAMPO"]))
                        {
                            pregunta.pre_TipoCampo = dataReader["PRE_TIPOCAMPO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HOG_CODIGO"]))
                        {
                            pregunta.cod_Hogar = dataReader["HOG_CODIGO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"]))
                        {
                            pregunta.per_IdPersona = int.Parse(dataReader["PER_IDPERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"]))
                        {
                            pregunta.per_PrimerNombre = dataReader["PER_PRIMERNOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"]))
                        {
                            pregunta.per_SegundoNombre = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"]))
                        {
                            pregunta.per_PrimerApellido = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"]))
                        {
                            pregunta.per_SegundoApellido = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["VALIDACION_PERSONA"]))
                        {
                            pregunta.validacion_Persona = int.Parse(dataReader["VALIDACION_PERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["ORDENPRIORIDAD"]))
                        {
                            pregunta.ordenPrioridad = dataReader["ORDENPRIORIDAD"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["IXP_ORDEN"]))
                        {
                            pregunta.ipx_Orden = int.Parse(dataReader["IXP_ORDEN"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"]))
                        {
                           // log.Info("gic_Pregunta / getPreguntaSiguiente , campo : PER_FECHANACIMIENTO --> " + dataReader["PER_FECHANACIMIENTO"].ToString());

                            //MODIFICACION JOSE VASQUEZ: FECHA 2015.NOV.26
                            //SE FORMATEA LA FECHA DESDE LA BASE DE DATOS A FTO: dd/MM/yyyy PARA PODERLA MANIPULAR DESDE CONTROLADOR
                            //pregunta.per_fechaNacimiento = DateTime.Parse(dataReader["PER_FECHANACIMIENTO"].ToString());
                            pregunta.per_fechaNacimiento = (DateTime)dataReader["PER_FECHANACIMIENTO"];
                            //FIN MODIFICACION JOSE VASQUEZ: FECHA 2015.NOV.26

                            //log.Info("gic_Pregunta / getPreguntaSiguiente , campo : PER_FECHANACIMIENTO -->> PASO CORRECTAMENTE LA CONVERSION");
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"]))
                        {
                            pregunta.per_NumeroDoc = dataReader["PER_NUMERODOC"].ToString();
                        }
                        preguntas.Add(pregunta);
                    }
                    dataReader.Close();
                    #endregion
                }
                return preguntas;
            }
            catch (Exception e)
            {
                log.Error("gic_Pregunta / getPreguntaSiguiente , Error: " + e.Message.ToString() + "/r" + e.StackTrace);
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
                ////datos.Dispose();
            }
        }

        public List<gic_RespuestaNuevo> getRespuestasxPregunta(int idPregunta, int idInstrumento)
        {
            List<gic_RespuestaNuevo> respuestas = new List<gic_RespuestaNuevo>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("pPRE_IDPREGUNTA", 1, "System.String", idPregunta.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_RESPUESTASXPREGUNTA", ref param);
            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        
                        gic_RespuestaNuevo respuesta = new gic_RespuestaNuevo();
                        respuesta.ins_IdInstrumento = idInstrumento;
                        if (!DBNull.Value.Equals(dataReader["RES_IDRESPUESTA"]))
                        {
                            respuesta.res_IdRespuesta = int.Parse(dataReader["RES_IDRESPUESTA"].ToString());
                           
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_IDPREGUNTA"]))
                        {
                            respuesta.pre_IdPregunta = new gic_Pregunta();
                            respuesta.pre_IdPregunta.pre_IdPregunta = int.Parse(dataReader["PRE_IDPREGUNTA"].ToString());
                            

                        }
                        if (!DBNull.Value.Equals(dataReader["RES_RESPUESTA"]))
                        {
                            respuesta.res_Respuesta = dataReader["RES_RESPUESTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_VALIDADOR"]))
                        {
                            respuesta.pre_Validador = dataReader["PRE_VALIDADOR"].ToString();
                           
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_LONGCAMPO"]))
                        {
                            respuesta.pre_longcampo = int.Parse(dataReader["PRE_LONGCAMPO"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_VALIDADOR_MIN"]))
                        {
                            respuesta.pre_Validador_Min = dataReader["PRE_VALIDADOR_MIN"].ToString(); ;
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_VALIDADOR_MAX"]))
                        {
                            respuesta.pre_Validador_Max = dataReader["PRE_VALIDADOR_MAX"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_ORDENRESPUESTA"]))
                        {
                            respuesta.res_OrdenRespuesta = int.Parse(dataReader["RES_ORDENRESPUESTA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_CAMPOTEX"]))
                        {
                            respuesta.pre_Campo_Tex = dataReader["PRE_CAMPOTEX"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_OBLIGATORIO"]))
                        {
                            respuesta.res_Obligatoria = dataReader["RES_OBLIGATORIO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_HABILITA"]))
                        {
                            respuesta.res_Habilita = dataReader["RES_HABILITA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_FINALIZA"]))
                        {
                            respuesta.res_Finaliza = dataReader["RES_FINALIZA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_AUTOCOMPLETAR"]))
                        {
                            respuesta.res_AutoCompletar = dataReader["RES_AUTOCOMPLETAR"].ToString();
                        }
                        respuestas.Add(respuesta);
                    }
                    dataReader.Close();
                }
                return respuestas;
            }
            catch (Exception e)
            {
                log.Error("gic_Pregunta / getRespuestasxPregunta , Error: " + e.Message.ToString() + "/r" + e.StackTrace);
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
                
            }
        }


        /// <summary>
        /// Devuelve el maximo de la pregunta insertada en la tabla gic_n_preguntasderivadas.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <param name="idTema">Id del tema a buscar</param>
        /// <returns> <gic_ElementosPadre> generada </returns>
        public gic_ElementosPadre getMaxPreguntaPadre(string codHogar, int idTema)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            gic_ElementosPadre padreYorden = new gic_ElementosPadre();
            param.Add(baseDatos.asignarParametro("pCOD_HOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("pIDTEMA", 1, "System.Int32", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_MAXPREGUNTAPADRE", ref param);

            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        padreYorden = new gic_ElementosPadre();
                        if (!DBNull.Value.Equals(dataReader["PADRE"]))
                        {
                            padreYorden.el_IdPreguntaPadre = int.Parse(dataReader[0].ToString());
                        }

                        if (!DBNull.Value.Equals(dataReader["ORDEN"]))
                        {
                            padreYorden.el_IdRespuestaPadre = int.Parse(dataReader[1].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PREGUNTAACTUAL"]))
                        {
                            padreYorden.el_IdTema = dataReader[2].ToString();
                        }
                    }
                    dataReader.Close();
                    
                }
            }
            catch (Exception e)
            {
                log.Error("gic_Pregunta / getMaxPreguntaPadre , Error: " + e.Message.ToString() + "/r" + e.StackTrace);
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
                
            }
            return padreYorden;
        }


        /// <summary>
        /// Devuelve la pregunta anterior en el flujo de cada tema.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <param name="idTema">Id del tema a buscar</param>
        /// <param name="idInstrumento">Id del Instrumento donde se va a buscar la siguiente pregunta</param>
        /// <param name="idPregunta">Id de la pregunta para saber el orden de ingreso </param>
        /// <param name="tipo">Tipo de flujo , para saber si es de CONSULTA O EDICION</param>
        /// <returns> List<gic_PreguntasxPersona> generada </returns>
        public List<gic_PreguntasxPersona> getPreguntaAnterior(string codHogar, int idTema, int idInstrumento, int idPregunta)
        {
            List<gic_PreguntasxPersona> preguntas = new List<gic_PreguntasxPersona>();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_BUSCAR_ANTERIOR_PREGUNTA";
            try
            {
                param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", codHogar));
                param.Add(baseDatos.asignarParametro("pID_TEMA", 1, "System.Int32", idTema.ToString()));
                param.Add(baseDatos.asignarParametro("pINS_IDINSTRUMENTO", 1, "System.Int32", idInstrumento.ToString()));
                param.Add(baseDatos.asignarParametro("pID_PREGUNTA", 1, "System.Int32", idPregunta.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
                #region DataReader
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                
                {
                    while (dataReader.Read())
                    {
                        gic_PreguntasxPersona pregunta = new gic_PreguntasxPersona();
                        if (!DBNull.Value.Equals(dataReader["PRE_IDPREGUNTA"]))
                        {
                            pregunta.pre_IdPregunta = int.Parse(dataReader["PRE_IDPREGUNTA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_PREGUNTA"]))
                        {
                            pregunta.pre_pregunta = dataReader["PRE_PREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_TIPOPREGUNTA"]))
                        {
                            pregunta.pre_TipoPregunta = dataReader["PRE_TIPOPREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_TIPOCAMPO"]))
                        {
                            pregunta.pre_TipoCampo = dataReader["PRE_TIPOCAMPO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HOG_CODIGO"]))
                        {
                            pregunta.cod_Hogar = dataReader["HOG_CODIGO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"]))
                        {
                            pregunta.per_IdPersona = int.Parse(dataReader["PER_IDPERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"]))
                        {
                            pregunta.per_PrimerNombre = dataReader["PER_PRIMERNOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"]))
                        {
                            pregunta.per_SegundoNombre = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"]))
                        {
                            pregunta.per_PrimerApellido = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"]))
                        {
                            pregunta.per_SegundoApellido = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["VALIDACION_PERSONA"]))
                        {
                            pregunta.validacion_Persona = int.Parse(dataReader["VALIDACION_PERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["ORDENPRIORIDAD"]))
                        {
                            pregunta.ordenPrioridad = dataReader["ORDENPRIORIDAD"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["IXP_ORDEN"]))
                        {
                            pregunta.ipx_Orden = int.Parse(dataReader["IXP_ORDEN"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"]))
                        {
                            pregunta.per_fechaNacimiento = DateTime.Parse(dataReader["PER_FECHANACIMIENTO"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"]))
                        {
                            pregunta.per_NumeroDoc = dataReader["PER_NUMERODOC"].ToString();
                        }
                        preguntas.Add(pregunta);
                    }
                    dataReader.Close();
                    #endregion
                }
                return preguntas;
            }
            catch (Exception e)
            {
                log.Error("gic_Pregunta / getPreguntaAnterior , Error: " + e.Message.ToString() + "/r" + e.StackTrace);
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
                
            }
        }


        public DataTable datosDepartamentos()
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pidCombo", 1, "System.Int32", "6"));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_SPC_COMBOS", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
            }
        }        

        public List<SelectListItem> CrearLista(DataTable lista, int tipo)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            SelectListItem primer_item = new SelectListItem();
            primer_item.Text = "Seleccione...";
            primer_item.Value = "";
            items.Add(primer_item);
            string valor = string.Empty;
            string id = string.Empty, columna = string.Empty;
            if (tipo == 1)
            {
                id = "RES_IDRESPUESTA";
                columna = "RES_RESPUESTA";
            }
            else
            {
                id = "ID";
                columna = "DESCRIPCION";
            }
            for (int i = 0; i < lista.Rows.Count; i++)
            {
                
                SelectListItem item = new SelectListItem();
                valor = lista.Rows[i][columna].ToString();
                item.Text = valor;
                item.Value = lista.Rows[i][id].ToString();
                items.Add(item);
            }
            return items;
        }

        public DataTable datosMunicipios(string idDepartamento)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("Id_depto", 1, "System.Int32", idDepartamento.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_SP_OBTMUNICIPIOPIOENCUESTA", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
                
            }          

        }

        public DataTable datosDireccionTerritorial(string idDepartamento)
        {
            if (idDepartamento == null || idDepartamento == "") {
                idDepartamento = "0";
            }
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("Id_depto", 1, "System.Int32", idDepartamento.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.GIC_SP_OBTDT", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {  
                datoConsulta.Dispose();
            }

        }


        public DataTable datosDeptoPorDT(string codigohogar,string idDT)
        {
            if (idDT == null || idDT == "")
            {
                idDT = "0";
            }
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pHogar_Codigo", 1, "System.String", codigohogar.ToString()));
                param.Add(baseDatos.asignarParametro("Id_dt", 1, "System.Int32", idDT.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.GIC_SP_OBDEPTOPORDT", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
                
            }

        }

        public DataTable guardaUltimoValorSel(string codigohogar, string idMA)
        {
            if (idMA == null || idMA == "")
            {
                idMA = "0";
            }
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pHogar_Codigo", 1, "System.String", codigohogar.ToString()));
                param.Add(baseDatos.asignarParametro("Id_MA", 1, "System.Int32", idMA.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.GIC_SP_GUARDAMUNATEN", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
                
            }

        }

        public DataTable datosMunicipioAtencion(string codigohogar, string idPuntoAtencion)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pHogar_Codigo", 1, "System.String", codigohogar.ToString()));
                param.Add(baseDatos.asignarParametro("Id_depto", 1, "System.Int32", idPuntoAtencion.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.GIC_SP_OBMUNICIPIOATECION", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
                
            }

        }

        public DataTable datosPuntoAtencion(string codigohogar, string idDepartamento)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            try
            {
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pHogar_Codigo", 1, "System.String", codigohogar.ToString()));
                param.Add(baseDatos.asignarParametro("Id_depto", 1, "System.Int32", idDepartamento.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.GIC_SP_OBTPUNTOATECION", ref param);
                return datoConsulta.Tables[0];
            }
            finally
            {
                datoConsulta.Dispose();
                
            }

        }

        

        /// <summary>
        /// Retorna la consulta que va a realziar la funcin de autocompletar.
        /// </summary>
        /// <param name="idPregunta">Id de la preguntaa la cual se le va a realziar la acción </param>
        /// <param name="filtro">filtro de la consulta</param>
        public List<string> obtenerSelectAutoCompletar(int idPregunta)
        {
            List<string> result = new List<string>();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "gic_n_caracterizacion.sp_cargautocompletar";
            param.Add(baseDatos.asignarParametro("pidpregunta", 1, "System.Int32", idPregunta.ToString()));
            param.Add(baseDatos.asignarParametro("cursor_out", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("gic_n_caracterizacion.sp_cargautocompletartotal", ref param);
            try
            {
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                
                {
                    while (dataReader.Read())
                    {
                        result.Add(dataReader["DATO"].ToString());
                    }
                    dataReader.Close();
                }
            }
            catch (Exception ex)
            {

                log.Error("gic_Pregunta / obtenerSelectAutoCompletar , Error: " + ex.Message.ToString() + "/r" + ex.StackTrace);
                throw ex;
            }
            finally
            {
                datoConsulta.Dispose();
                
            }
            return result;
        }


    }

    public class gic_PreguntaRespuestasFlujo : gic_AdministradorDeCambios
    {
        public List<gic_PreguntasxPersona> colleccionPreguntas { get; set; }
        public List<gic_RespuestaxEncuesta> coleccionRespuestas { get; set; }
        public gic_RespuestaxEncuesta respuestaPreguntaGen { get; set; }
        public gic_PreguntasxPersona preguntaXpersonaGen { get; set; }
        public gic_Tema temaAmostrar { get; set; }
        public List<gic_RespuestaNuevo> opcionesRespuesta { get; set; }
        public List<gic_RespuestaNuevo> opcionesRespuestaFiltrado { get; set; }
        public string respuestasConcatenadas { get; set; }
        public string personasConcatenadas { get; set; }
        public int idPersonaEncuesta { get; set; }
        public string habilitar { get; set; }
        public int totalCapitulosTerminados { get; set; }
        public string codigoHogar { get; set; }
        public int totalPreguntas { get; set; }
        public string flujo { get; set; }
        

    }
}