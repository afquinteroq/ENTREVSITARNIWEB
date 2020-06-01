using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;


namespace IgedEncuesta.Models.mdlGenerico
{
    public class GIC_REPORTEXHOGAR
    {
        public string nombre_completo { get; set; }
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public string brinda_informacion { get; set; }
        public string codigohogar { get; set; }
        public string estado_encuesta { get; set; }
        public string fecha_creacion_encuesta { get; set; }
        public string usuario_creacion_encuesta  {  get; set; }



        public List<GIC_REPORTEXHOGAR> getReporteXHogar(string usuarioCreacion)
        {
            mdlGenerico baseDatos = new mdlGenerico();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            List<Parametros> param = new List<Parametros>();

            List<GIC_REPORTEXHOGAR> coleccion = new List<GIC_REPORTEXHOGAR>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_REPORTE_XHOGAR";
            param.Add(baseDatos.asignarParametro("pUSUARIO", 1, "System.String", usuarioCreacion));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);

            try
            {

                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        GIC_REPORTEXHOGAR reporte = new GIC_REPORTEXHOGAR();
                        if (!DBNull.Value.Equals(dataReader["NOMBRE"]))
                        {
                            reporte.nombre_completo = dataReader["NOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["TIPO_DOCUMENTO"]))
                        {
                            reporte.tipo_documento = dataReader["TIPO_DOCUMENTO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["NUMERO_DOCUMENTO"]))
                        {
                            reporte.numero_documento = dataReader["NUMERO_DOCUMENTO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_ENCUESTADA"]))
                        {
                            reporte.brinda_informacion = dataReader["PER_ENCUESTADA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["ESTADO_ENCUESTA"]))
                        {
                            reporte.estado_encuesta = dataReader["ESTADO_ENCUESTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["FECHA_CREACION"]))
                        {
                            reporte.fecha_creacion_encuesta = dataReader["FECHA_CREACION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["USUARIO_CREACION"]))
                        {
                            reporte.usuario_creacion_encuesta = dataReader["USUARIO_CREACION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HOG_CODIGO"]))
                        {
                            reporte.codigohogar = dataReader["HOG_CODIGO"].ToString();
                        }

                        coleccion.Add(reporte);

                    }
                    dataReader.Close();
                }
                return coleccion;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                
                datoConsulta.Dispose();
            }

        }

    }
}