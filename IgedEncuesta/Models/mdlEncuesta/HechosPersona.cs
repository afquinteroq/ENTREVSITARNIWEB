using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;

namespace IgedEncuesta.Models.mdlEncuesta
{
    public class HechosPersona : Models.mdlGenerico.mdlGenerico
    {
        public string PER_ID { get; set; }
        public string ID_CARACTERIZACION { get; set; }
        public string HV1 { get; set; }
        public string Fecha_HV1 { get; set; }
        public string HV2 { get; set; }
        public string Fecha_HV2 { get; set; }
        public string HV3 { get; set; }
        public string Fecha_HV3 { get; set; }
        public string HV4 { get; set; }
        public string Fecha_HV4 { get; set; }
        public string HV5 { get; set; }
        public string Fecha_HV5 { get; set; }
        public string HV6 { get; set; }
        public string Fecha_HV6 { get; set; }
        public string HV7 { get; set; }
        public string Fecha_HV7 { get; set; }
        public string HV8 { get; set; }
        public string Fecha_HV8 { get; set; }
        public string HV9 { get; set; }
        public string Fecha_HV9 { get; set; }
        public string HV10 { get; set; }
        public string Fecha_HV10 { get; set; }
        public string HV11 { get; set; }
        public string Fecha_HV11 { get; set; }
        public string HV12 { get; set; }
        public string Fecha_HV12 { get; set; }
        public string HV13 { get; set; }
        public string Fecha_HV13 { get; set; }        
        public string HV14 { get; set; }
        public string Fecha_HV14 { get; set; }
        public string FECHA_ULTI_ENCUESTA { get; set; }
        public string HABILITADO_CARAC { get; set; }
        public string COD_HOGAR { get; set; }
        public string ESTADO_ENCUESTA { get; set; }

        public string MAX_FECHA_HECHO { get; set; }


        public DataSet consultarPersonasModeloINntegrado(string IdPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_ID_PERSONA", 1, "System.Int32", IdPersona));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MENSAJE", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CARACTERIZACION.MI_ESTADO_RUV", ref param);


                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }
           
        }

        
        //08/05/2020 Se comento para probar, se debe verificar este metodo
        //public DataSet consultarFechaUltimaEncuesta(string IdPersona)
        //{
        //    List<Parametros> param = new List<Parametros>();
        //    DataSet dsSalida = new DataSet();
        //    AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
        //    try
        //    {
        //        datos.MotorBasedatos = true;
        //        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
        //        datos.Conexion = connString;
        //        param = new List<Parametros>();
        //        param.Add(asignarParametro("P_ID_PERSONA", 1, "System.Int32", IdPersona));
        //        param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
        //        param.Add(asignarParametro("S_MENSAJE", 2, "System.String", ""));
        //        dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CARACTERIZACION.MI_ULTIMA_ENCUESTA", ref param);
        //        return dsSalida;
        //    }
        //    finally
        //    {
        //        dsSalida.Dispose();
        //    }
          
        //}

        public HechosPersona ultimaEncuesta(DataSet ds) { 
         HechosPersona hechos = new HechosPersona();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            while (dataReader.Read())
            {
                if (!DBNull.Value.Equals(dataReader["PER_ID"])) hechos.PER_ID = dataReader["PER_ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["ID_CARACTERIZACION"])) hechos.ID_CARACTERIZACION = dataReader["ID_CARACTERIZACION"].ToString();
                if (!DBNull.Value.Equals(dataReader["FECHA_ULTI_ENCUESTA"])) hechos.FECHA_ULTI_ENCUESTA = dataReader["FECHA_ULTI_ENCUESTA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HABILITADO_CARAC"])) hechos.HABILITADO_CARAC = dataReader["HABILITADO_CARAC"].ToString();
                if (!DBNull.Value.Equals(dataReader["COD_HOGAR"])) hechos.COD_HOGAR = dataReader["COD_HOGAR"].ToString();
                if (!DBNull.Value.Equals(dataReader["ESTADO_ENCUESTA"])) hechos.ESTADO_ENCUESTA = dataReader["ESTADO_ENCUESTA"].ToString();
            }
            return hechos;
        }


        public HechosPersona hechosVictimizantes(string IdPersona)
        {

            DataSet dsHechos = null;
            DataSet dsUltimaFecha = new DataSet();
            dsHechos = consultarPersonasModeloINntegrado(IdPersona);
            HechosPersona hechos = new HechosPersona();
            HechosPersona hechosUltima = null;
            IDataReader dataReader = null;
           
            if (dsHechos.Tables.Count > 0)
            {
                dataReader = dsHechos.Tables[0].CreateDataReader();
                while (dataReader.Read())
                {
                    if (!DBNull.Value.Equals(dataReader["PER_ID"])) hechos.PER_ID = dataReader["PER_ID"].ToString();
                    if (!DBNull.Value.Equals(dataReader["ID_CARACTERIZACION"])) hechos.ID_CARACTERIZACION = dataReader["ID_CARACTERIZACION"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV1"])) hechos.HV1 = dataReader["HV1"].ToString();                    
                    if (!DBNull.Value.Equals(dataReader["HV2"])) hechos.HV2 = dataReader["HV2"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV3"])) hechos.HV3 = dataReader["HV3"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV4"])) hechos.HV4 = dataReader["HV4"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV5"])) hechos.HV5 = dataReader["HV5"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV6"])) hechos.HV6 = dataReader["HV6"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV7"])) hechos.HV7 = dataReader["HV7"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV8"])) hechos.HV8 = dataReader["HV8"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV9"])) hechos.HV9 = dataReader["HV9"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV10"])) hechos.HV10 = dataReader["HV10"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV11"])) hechos.HV11 = dataReader["HV11"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV12"])) hechos.HV12 = dataReader["HV12"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV13"])) hechos.HV13 = dataReader["HV13"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV14"])) hechos.HV14 = dataReader["HV14"].ToString();

                    try
                    {
                        if (!DBNull.Value.Equals(dataReader["HV5"]))
                        {
                            if (!DBNull.Value.Equals(dataReader["HV5_FECHA"])) hechos.Fecha_HV5 = dataReader["HV5_FECHA"].ToString();
                        }
                        
                            if (!DBNull.Value.Equals(dataReader["HV1"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV1_FECHA"])) hechos.Fecha_HV1 = dataReader["HV1_FECHA"].ToString();
                                
                            }
                            if (!DBNull.Value.Equals(dataReader["HV2"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV2_FECHA"])) hechos.Fecha_HV2 = dataReader["HV2_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV3"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV3_FECHA"])) hechos.Fecha_HV3 = dataReader["HV3_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV4"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV4_FECHA"])) hechos.Fecha_HV4 = dataReader["HV4_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV6"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV6_FECHA"])) hechos.Fecha_HV6 = dataReader["HV6_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV7"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV7_FECHA"])) hechos.Fecha_HV7 = dataReader["HV7_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV8"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV8_FECHA"])) hechos.Fecha_HV8 = dataReader["HV8_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV9"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV9_FECHA"])) hechos.Fecha_HV9 = dataReader["HV9_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV10"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV10_FECHA"])) hechos.Fecha_HV10 = dataReader["HV10_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV11"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV11_FECHA"])) hechos.Fecha_HV11 = dataReader["HV11_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV12"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV12_FECHA"])) hechos.Fecha_HV12 = dataReader["HV12_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV13"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV13_FECHA"])) hechos.Fecha_HV13 = dataReader["HV13_FECHA"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["HV14"]))
                            {
                                
                                if (!DBNull.Value.Equals(dataReader["HV14_FECHA"])) hechos.Fecha_HV14 = dataReader["HV14_FECHA"].ToString();
                            }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            //Pendiente validar estas reglas, para validar si se puedo borrar
            if (dsUltimaFecha.Tables.Count > 0) {
                dataReader = dsUltimaFecha.Tables[0].CreateDataReader();
                while (dataReader.Read())
                {
                    hechosUltima = ultimaEncuesta(dsUltimaFecha);
                    if (!DBNull.Value.Equals(hechosUltima.FECHA_ULTI_ENCUESTA)) hechos.FECHA_ULTI_ENCUESTA = hechosUltima.FECHA_ULTI_ENCUESTA;
                    if (!DBNull.Value.Equals(hechosUltima.HABILITADO_CARAC)) hechos.HABILITADO_CARAC = hechosUltima.HABILITADO_CARAC;
                    if (!DBNull.Value.Equals(hechosUltima.ESTADO_ENCUESTA)) hechos.ESTADO_ENCUESTA = hechosUltima.ESTADO_ENCUESTA;
                    if (!DBNull.Value.Equals(hechosUltima.COD_HOGAR)) hechos.COD_HOGAR = hechosUltima.COD_HOGAR;
                }
            }

            return hechos;
        }
      
    }
}