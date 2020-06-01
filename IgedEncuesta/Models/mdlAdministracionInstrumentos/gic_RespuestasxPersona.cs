using IgedEncuesta.Models.mdlGenerico;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;


namespace AdministracionInstrumentos
{
    public class gic_RespuestasxPersona
    {
        /// <summary>
        /// Id Respuesta por persona
        /// </summary>
        public int ins_IdInstrumento { get; set; }

        /// <summary>
        /// Cod Hogar
        /// </summary>
        public int tem_Orden { get; set; }

        /// <summary>
        /// Persona
        /// </summary>
        public string tem_NombreTema { get; set; }

        /// <summary>
        /// Respuesta
        /// </summary>
        public int pre_IdPregunta { get; set; }

        /// <summary>
        /// Respuesta texto
        /// </summary>
        public string pre_Pregunta { get; set; }

        /// <summary>
        /// Respuesta texto
        /// </summary>
        public string per_Nombre { get; set; }

        /// <summary>
        /// Cod Hogar
        /// </summary>
        public string res_Respuesta { get; set; }

        /// <summary>
        /// bandera
        /// </summary>
        public string rxp_TextoRespuesta { get; set; }

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        /// Devuelve las respuestas por persona
        /// </summary>
        /// <param name="cod_hogar">Codigo del hogar a buscar</param>
        /// <returns> List<gic_RespuestaxPersona> generada </returns>/// 
        public List<gic_RespuestasxPersona> get_RespuestasxPersona(string cod_hogar)
        {
            List<gic_RespuestasxPersona> respuestas = new List<gic_RespuestasxPersona>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            string stored = string.Empty;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_RESPUESTAS_ENCUESTA";
            param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", cod_hogar));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_RespuestasxPersona respuesta = new gic_RespuestasxPersona();
                        if (!DBNull.Value.Equals(dataReader["INS_IDINSTRUMENTO"]))
                        {
                            respuesta.ins_IdInstrumento = int.Parse(dataReader["INS_IDINSTRUMENTO"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_ORDEN"]))
                        {
                            respuesta.tem_Orden = int.Parse(dataReader["TEM_ORDEN"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_NOMBRETEMA"]))
                        {
                            respuesta.tem_NombreTema = dataReader["TEM_NOMBRETEMA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_IDPREGUNTA"]))
                        {
                            respuesta.pre_IdPregunta = int.Parse(dataReader["PRE_IDPREGUNTA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["PRE_PREGUNTA"]))
                        {
                            respuesta.pre_Pregunta = dataReader["PRE_PREGUNTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["NOMBRE"]))
                        {
                            respuesta.per_Nombre = dataReader["NOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RES_RESPUESTA"]))
                        {
                            respuesta.res_Respuesta = dataReader["RES_RESPUESTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["RXP_TEXTORESPUESTA"]))
                        {
                            respuesta.rxp_TextoRespuesta = dataReader["RXP_TEXTORESPUESTA"].ToString();
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
