using ObjetosTipos;
using System;
using System.Collections.Generic;

using IgedEncuesta.Models.mdlGenerico;


namespace AdministracionInstrumentos
{
    public class MI_LOG_ERRORES_INTEGRACION
    {

        String MI_FECHA_ERROR { get; set; }
        String MI_MSJ_ERROR { get; set; }
        String FUENTE { get; set; }
        String MI_ID_REGISTRO { get; set; }
        String  MI_DESTINATARIO { get; set; }
        String MI_PROCEDURE_ERROR { get; set; }

        /// <summary>
        /// Estado Instrumento
        /// </summary>
        mdlGenerico baseDatos = new mdlGenerico();


        public void insertaConstanciaFirmada(String error, String metodo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("MI_FECHA_ERROR", 1, "System.DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")/*"SYSDATE"*/));            
            param.Add(baseDatos.asignarParametro("MI_MSJ_ERROR", 1, "System.String", error));
            param.Add(baseDatos.asignarParametro("FUENTE", 1, "System.String", "APP WEB"));
            param.Add(baseDatos.asignarParametro("MI_ID_REGISTRO", 1, "System.String", null));
            param.Add(baseDatos.asignarParametro("MI_DESTINATARIO", 1, "System.String", null));
            param.Add(baseDatos.asignarParametro("MI_PROCEDURE_ERROR", 1, "System.String", metodo));
            param.Add(baseDatos.asignarParametro("P_HOG_CODIGO", 1, "System.String", ""));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("SP_GEN_LOG_ERROR", ref param);
            }
            catch (Exception ex)
            {

                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);
            }
            finally
            {
                //////datos.Dispose();
            }
        }
    }
}