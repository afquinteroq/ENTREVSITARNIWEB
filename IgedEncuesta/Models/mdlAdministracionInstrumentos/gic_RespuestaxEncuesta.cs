using IgedEncuesta.Models.mdlGenerico;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;
namespace AdministracionInstrumentos
{
    public class gic_RespuestaxEncuesta : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Id Respuesta por persona
        /// </summary>
        public int rxp_IdRespuestaxPersona { get; set; }

        /// <summary>
        /// Cod Hogar
        /// </summary>
        public string cod_Hogar { get; set; }

        /// <summary>
        /// Persona
        /// </summary>
        public int per_IdPersona { get; set; }

        /// <summary>
        /// Respuesta
        /// </summary>
        public gic_Respuesta res_IdRespuesta { get; set; }

        /// <summary>
        /// Respuesta texto
        /// </summary>
        public string rxp_TextoRespuesta { get; set; }

        /// <summary>
        /// Respuesta texto
        /// </summary>
        public string rxp_TipoPreguntaRespuesta { get; set; }

        /// <summary>
        /// Cod Hogar
        /// </summary>
        public int ins_IdInstrumento { get; set; }

        /// <summary>
        /// bandera
        /// </summary>
        public int bandera { get; set; }

        
        
        //
        /// <summary>
        /// 
        /// </summary>

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        /// Realiza la inserción de la respuestas por prwegunta.
        /// </summary>
        /// <param name="idPregunta">Id de la pregunta a la cual se le van asociar las respuestas</param>
        /// <param name="respuesta">Lista de respuestas a insertar</param>
        /// <returns> List<gic_RespuestaNuevo> generada </returns>
        public void insertaRespuestaXencuesta(List<gic_RespuestaxEncuesta> respuesta, int idPregunta)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            List<Parametros> param;
            try
            {
            foreach (gic_RespuestaxEncuesta m in respuesta)
            {
                param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pcod_hogar", 1, "System.String", m.cod_Hogar));
                param.Add(baseDatos.asignarParametro("pper_IdPersona", 1, "System.Int32", m.per_IdPersona.ToString()));
                param.Add(baseDatos.asignarParametro("pres_IdRespuesta", 1, "System.Int32", m.res_IdRespuesta.res_IdRespuesta.ToString()));
                param.Add(baseDatos.asignarParametro("prxp_TextoRespuesta", 1, "System.String", m.rxp_TextoRespuesta));
                param.Add(baseDatos.asignarParametro("prxp_TipoPreguntaRespuesta", 1, "System.String", m.rxp_TipoPreguntaRespuesta));
                param.Add(baseDatos.asignarParametro("pins_IdInstrumento", 1, "System.Int32", m.ins_IdInstrumento.ToString()));
                param.Add(baseDatos.asignarParametro("pusu_UsuarioCreacion", 1, "System.String", m.usu_UsuarioCreacion));
                param.Add(baseDatos.asignarParametro("pper_idPreguntaPadre", 1, "System.Int32", idPregunta.ToString()));
                param.Add(baseDatos.asignarParametro("pbandera", 1, "System.Int32", m.bandera.ToString()));                
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_SET_RESPUESTAS_DE_ENCUESTA", ref param);                
               
                }
            
                
            }
            catch (Exception ex)
            {
                
                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);
            }
            finally
            {
                //datos.Dispose();

            }
        }

        /// <summary>
        /// Devuelve las respuestas por  tema
        /// </summary>
        /// <param name="cod_hogar">Codigo del hogar a buscar</param>
        /// <param name="idTema">Id del tema a buscar</param>
        /// <returns> Lista de respuestas <gic_RespuestaxEncuesta></returns>
        public List<gic_RespuestaxEncuesta> getRespuestasXcapitulo(string cod_hogar, int idTema)
        {
            List<gic_RespuestaxEncuesta> respuestas = new List<gic_RespuestaxEncuesta>();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_GET_RESPUESTAXPREGUNTA";
            param.Add(baseDatos.asignarParametro("COD_HOGAR", 1, "System.String", cod_hogar));
            param.Add(baseDatos.asignarParametro("IDTEMA", 1, "System.Int32", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_RespuestaxEncuesta respuesta = new gic_RespuestaxEncuesta();
                        if (!DBNull.Value.Equals(dataReader["RXP_IDRESPUESTAXPERSONA"]))
                        {
                            respuesta.rxp_IdRespuestaxPersona = int.Parse(dataReader["RXP_IDRESPUESTAXPERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["HOG_CODIGO"]))
                        {
                            respuesta.cod_Hogar = dataReader["HOG_CODIGO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"]))
                        {
                            respuesta.per_IdPersona = int.Parse(dataReader["PER_IDPERSONA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_IDRESPUESTA"]))
                        {
                            respuesta.res_IdRespuesta = new gic_Respuesta();
                            respuesta.res_IdRespuesta.res_IdRespuesta = int.Parse(dataReader["RES_IDRESPUESTA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["RXP_TEXTORESPUESTA"]))
                        {
                            respuesta.rxp_TextoRespuesta = dataReader["RXP_TEXTORESPUESTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RXP_TIPOPREGUNTA"]))
                        {
                            respuesta.rxp_TipoPreguntaRespuesta = dataReader["RXP_TIPOPREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["INS_IDINSTRUMENTO"]))
                        {
                            respuesta.ins_IdInstrumento = int.Parse(dataReader["INS_IDINSTRUMENTO"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_IDPREGUNTA"]))
                        {
                            respuesta.res_IdRespuesta.pre_IdPregunta = new gic_Pregunta();
                            respuesta.res_IdRespuesta.pre_IdPregunta.pre_IdPregunta = int.Parse(dataReader["PRE_IDPREGUNTA"].ToString());
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
            }
        }
    }
}