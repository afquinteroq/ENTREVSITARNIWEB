using IgedEncuesta.Models.mdlGenerico;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_RespuestaNuevo : gic_AdministradorDeCambios
    {
        
        /// <summary>
        /// Identificador unico de la respuesta
        /// </summary>
        public int ins_IdInstrumento { get; set; }
        /// <summary>
        /// Identificador unico de la respuesta
        /// </summary>
        public int res_IdRespuesta { get; set; }
        /// <summary>
        /// Respuesta
        /// </summary>
        public string res_Respuesta { get; set; }
        /// <summary>
        /// Tipo Validador para evaluar la respuesta
        /// </summary>
        public string pre_Validador { get; set; }
        /// <summary>
        /// Activa
        /// </summary>
        public int pre_longcampo { get; set; }
        /// <summary>
        /// Relacion Gic_Pregunta
        /// </summary>
        public gic_Pregunta pre_IdPregunta { get; set; }
        /// <summary>
        /// Valor validador min
        /// </summary>
        public string pre_Validador_Min { get; set; }
        /// <summary>
        /// Valor validador maximo
        /// </summary>
        public string pre_Validador_Max { get; set; }
        /// <summary>
        /// Orden respuesta
        /// </summary>
        public int res_OrdenRespuesta { get; set; }
        /// <summary>
        ///Opcion para habilitar campo de texto
        /// </summary>
        public string pre_Campo_Tex { get; set; }
        /// <summary>
        /// Respuesta Obligatoria
        /// </summary>
        public string res_Obligatoria { get; set; }
        /// <summary>
        /// Respuesta Habilita opciones
        /// </summary>
        public string res_Habilita{ get; set; }
        /// <summary>
        /// Respuesta Finaliza el capitulo si se contesta
        /// </summary>
        public string res_Finaliza { get; set; }
        /// <summary>
        /// Respuesta que tiene opcion de autocompletar campo de texto
        /// </summary>
        public string res_AutoCompletar { get; set; }

        mdlGenerico baseDatos = new mdlGenerico();


        /// <summary>
        /// Obtiene las respuestas asociados a una pregunta.
        /// </summary>
        /// <param name="idPregunta">Id de la pregunta a la cual se le van asociar las respuestas</param>
        /// <param name="idInstrumento">Id del Instrumento para asociar la pregunta correspondiente</param>
        /// <returns> List<gic_RespuestaNuevo> generada </returns>
        public List<gic_RespuestaNuevo> getRespuestasxPregunta(int idPregunta, int idInstrumento)
        {
            List<gic_RespuestaNuevo> respuestas = new List<gic_RespuestaNuevo>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connString;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            try
            {
                param.Add(baseDatos.asignarParametro("pINS_IDINSTRUMENTO", 1, "System.Int32", idInstrumento.ToString()));
                param.Add(baseDatos.asignarParametro("pID_PREGUNTA", 1, "System.Int32", idPregunta.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_RESPUESTASXPREGUNTA", ref param);
                //using (IDataReader dataReader = dbahe.ExecuteReader("GIC_N_CARACTERIZACION.SP_GET_RESPUESTASXPREGUNTA", new object[] { idPregunta, new object[] { null } }))
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        //SELECT RI.INS_IDINSTRUMENTO, RI.RES_IDRESPUESTA, RE.RES_RESPUESTA, RI.PRE_VALIDADOR, RI.PRE_LONGCAMPO, 
                        //RI.PRE_VALIDADOR_MIN, RI.PRE_VALIDADOR_MAX, RI.RES_ORDENRESPUESTA, RI.PRE_CAMPOTEX
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
            catch
            {
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
                ////datos.Dispose();
            }
        }


        /// <summary>
        /// Realiza el borrado de los nombres que encuentre vacios.
        /// </summary>
        /// <param name="idPersona">Id de la persona a  realizar el calculo</param>
        /// <param name="hogCodigo">Codigo del hogar del hogar encuestado</param>
        /// <returns> Devuelve la edad de la persona</returns>
        public int get_Edadxpersona(int idPersona, string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int edad = 0;
                string funcion = "GIC_N_CARACTERIZACION.FN_GET_EDADXPERSONA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pIDPERSONA", 1, "System.Int32", idPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                edad = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return edad;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

        
        public int FN_GET_TOTALCUARTOSXFAMILIA(int idPersona, string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int totalcuartos = 0;
                string funcion = "GIC_N_CARACTERIZACION.FN_GET_TOTALCUARTOSXFAMILIA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pIDPERSONA", 1, "System.Int32", idPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                totalcuartos = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return totalcuartos;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                
            }
        }


        /// <summary>
        /// Obtiene las respuestas asociados a una pregunta.
        /// </summary>
        /// <param name="idPregunta">Id de la pregunta a la cual se le van asociar las respuestas</param>
        /// <param name="idInstrumento">Id del Instrumento para asociar la pregunta correspondiente</param>
        /// <returns> List<gic_RespuestaNuevo> generada </returns>
        public List<gic_RespuestaNuevo> getRespuestasxPrexPersona(int idPregunta, int idInstrumento, string codHogar, int idPersona)
        {
            List<gic_RespuestaNuevo> respuestas = new List<gic_RespuestaNuevo>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;//pPRE_IDPREGUNTA, pHOG_CODIGO
            param.Add(baseDatos.asignarParametro("pPRE_IDPREGUNTA", 1, "System.Int32", idPregunta.ToString()));
            param.Add(baseDatos.asignarParametro("pINS_IDINSTRUMENTO", 1, "System.Int32", idInstrumento.ToString()));
            param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("pPER_IDPERSONA", 1, "System.Int32", idPersona.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_RESPUESTASXPREMOD", ref param);
            try
            {
              //  using (IDataReader dataReader = dbahe.ExecuteReader("GIC_N_CARACTERIZACION.SP_GET_RESPUESTASXPREMOD", new object[] { idPregunta, idInstrumento, codHogar, idPersona, new object[] { null } }))
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
            catch
            {
                return null;
            }
            finally
            {
                ////datos.Dispose();
            }

        }


        /// <summary>
        /// Realiza el borrado de los nombres que encuentre vacios.
        /// </summary>
        /// <param name="hogCodigo">Codigo del hogar a buscar</param>
        /// <param name="idPersona">Id de la persona a bsucar</param>
        /// <param name="idRespuesta">Id de la respuesta asociado a esa pregunta</param>
        /// <param name="idInstrumento">Id del Instrumento para verficar la respuesta </param>
        public void borrarNombresVacios(string hogCodigo, int idPersona, int idRespuesta, int idInstrumento)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string funcion = "GIC_N_CARACTERIZACION.SP_BORRARNOMVACIOS";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("phogCodigo", 1, "System.String", hogCodigo.ToString()));
                param.Add(baseDatos.asignarParametro("pidRespuesta", 1, "System.Int32", idRespuesta.ToString()));
                param.Add(baseDatos.asignarParametro("pIdPersona", 1, "System.Int32", idPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pIdInstrumento", 1, "System.Int32", idInstrumento.ToString()));
                datos.InsertarConProcedimientoAlmacenado(funcion, ref param);
                /*dbCommand = dbahe.GetStoredProcCommand(funcion);
                dbahe.AddInParameter(dbCommand, "pIDPERSONA", DbType.Int32, idPersona);
                dbahe.AddInParameter(dbCommand, "pCODHOGAR", DbType.String, codHogar);
                dbahe.AddParameter(dbCommand, "RESULT", DbType.String, 1024, ParameterDirection.ReturnValue, false, 0, 0, null, DataRowVersion.Default, null);
                dbahe.ExecuteNonQuery(dbCommand);
                //Assert.IsTrue(((string)cmd.Parameters[2].Value) == "1 - Hello");
                edad = int.Parse(dbCommand.Parameters[2].Value.ToString());*/
                // return edad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

        /// <summary>
        /// Realiza la actualización de los nombres, al traer la pregunta nombres.
        /// </summary>
        /// <param name="nombre1">Primer nombre a actualizar</param>
        /// <param name="nombre2">Segundo nombre a actualizar</param>
        /// <param name="apellido1">Primer apellido a actualizar</param>
        /// <param name="apellido2">Segundo apellido a actualizar</param>
        /// <param name="idPersona">Id de la persona a actualizar</param>
        public void actualizaNombres(string nombre1, string nombre2, string apellido1, string apellido2, int idPersona)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string funcion = "GIC_N_CARACTERIZACION.SP_ACTUALIZARNOMBRES";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pNombre1", 1, "System.String", nombre1.ToString()));
                param.Add(baseDatos.asignarParametro("pNombre2", 1, "System.String", nombre2.ToString()));
                param.Add(baseDatos.asignarParametro("pApellido1", 1, "System.String", apellido1.ToString()));
                param.Add(baseDatos.asignarParametro("pApellido2", 1, "System.String", apellido2.ToString()));
                param.Add(baseDatos.asignarParametro("pIdPersona", 1, "System.Int32", idPersona.ToString()));
                datos.InsertarConProcedimientoAlmacenado(funcion, ref param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

        /// <summary>
        /// Realiza la actualización de la cedula.
        /// </summary>
        /// <param name="cedula">Primer nombre a actualizar</param>
        /// <param name="idPersona">Id de la persona a actualizar</param>
        public void actualizaDocumento(string cedula, int idPersona)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string funcion = "GIC_CARACTERIZACION.SP_ACTUALIZARDOCUMENTO";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pNumeroDoc", 1, "System.String", cedula.ToString()));
                param.Add(baseDatos.asignarParametro("pIdPersona", 1, "System.Int32", idPersona.ToString()));
                datos.InsertarConProcedimientoAlmacenado(funcion, ref param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

        /// <summary>
        /// Realiza la actualización de los nombres, al traer la pregunta nombres.
        /// </summary>
        /// <param name="fechaNacimiento">Primer nombre a actualizar</param>
        /// <param name="idPersona">Id de la persona a actualizar</param>
        public void actualizaFechaNacimiento(string fechaNacimiento, int idPersona)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string funcion = "GIC_CARACTERIZACION.SP_ACTUALIZARFECHA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pFecha", 1, "System.DateTime", fechaNacimiento.ToString()));
                param.Add(baseDatos.asignarParametro("pIdPersona", 1, "System.Int32", idPersona.ToString()));
                datos.InsertarConProcedimientoAlmacenado(funcion, ref param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ////datos.Dispose();
            }
        }


        /// <summary>
        /// Realiza el borrado de los nombres que encuentre vacios.
        /// </summary>
        /// <param name="idPersona">Id de la persona a  realizar el calculo</param>
        /// <param name="hogCodigo">Codigo del hogar del hogar encuestado</param>
        /// <returns> Devuelve la edad de la persona</returns>
        public int get_cuartosHogar(int idPersona, string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int edad = 0;
                string funcion = "GIC_N_CARACTERIZACION.FN_GET_CUARTOSPORHOGAR";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pIDPERSONA", 1, "System.Int32", idPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                edad = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return edad;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

        public String obtenerTipoPersona(String codHogar, int idPersona) {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string informanteencuesta = "0";
                string funcion = "GIC_N_CARACTERIZACION.FN_GET_TIPOPERSONA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pIDPERSONA", 1, "System.Int32", idPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                int counttipopersona = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                if (counttipopersona > 0) {
                    informanteencuesta = "SI";
                }
                return informanteencuesta;
            }
            catch (Exception)
            {
                return "NO";
            }
            finally
            {
                ////datos.Dispose();
            }
        }

    }
}
