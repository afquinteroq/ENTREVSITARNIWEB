using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ObjetosTipos;
using AdministracionInstrumentos;

namespace IgedEncuesta.Models.mdlEncuesta
{
    public class ConsultaUnificada : Models.mdlGenerico.mdlGenerico
    {
        public DataSet consultUnificadaFuentes_y_RUV(string documento)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_DOCUMENTO", 1, "System.String", documento));
                param.Add(asignarParametro("P_RESULT", 2, "System.String", ""));
                param.Add(asignarParametro("P_MENSAJE", 2, "System.String", ""));
                param.Add(asignarParametro("P_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("P_CUR_TUP", 2, "Cursor", ""));                
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("RNI_MI_PRU.SP_ENTREVISTA_UNICA_UNIFICADO", ref param);
                string resultado = param.Find(x => x.Nombre == "P_RESULT").Valor;
                string mensaje = param.Find(x => x.Nombre == "P_MENSAJE").Valor;

                return dsSalida;
            }
            catch (Exception e) {
                e.Message.ToString();
                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }
    }
}