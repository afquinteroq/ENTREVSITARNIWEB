using ObjetosTipos;
using System;
using System.Collections.Generic;

using System.Data;
using System.Text;

namespace IgedEncuesta.Models.mdlConstancia
{
    public class ConstanciaSAAH : Models.mdlGenerico.mdlGenerico
    {
        public string TIPO_DOC { get; set; }
        public string NUMERO_DOC { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string NOMBRES_COMPLETOS { get; set; }
        public string FECHA_NACIMIENTO { get; set; }
        public string ESTADO_RUV { get; set; }
        public string ID_PERSONA { get; set; }              
        public string COD_HOGAR { get; set; }
        public string ESTADO_ENCUESTA { get; set; }
        public string TIPO_PERSONA { get; set; }
        public string HECHO_VICTIMIZANTE { get; set; }

        public string HECHO_VICTIMIZANTE_1 { get; set; }
        public string HECHO_VICTIMIZANTE_2 { get; set; }
        public string HECHO_VICTIMIZANTE_3 { get; set; }
        public string HECHO_VICTIMIZANTE_4 { get; set; }
        public string HECHO_VICTIMIZANTE_5 { get; set; }
        public string HECHO_VICTIMIZANTE_6 { get; set; }
        public string HECHO_VICTIMIZANTE_7 { get; set; }
        public string HECHO_VICTIMIZANTE_8 { get; set; }
        public string HECHO_VICTIMIZANTE_9 { get; set; }
        public string HECHO_VICTIMIZANTE_10 { get; set; }
        public string HECHO_VICTIMIZANTE_11 { get; set; }
        public string HECHO_VICTIMIZANTE_12 { get; set; }
        public string HECHO_VICTIMIZANTE_13 { get; set; }
        public string HECHO_VICTIMIZANTE_14 { get; set; }
        public string DEPTO_ATENCION { get; set; }
        public string MUN_ATENCION { get; set; }
        public string PUNTO_ATENCION { get; set; }
        public string FECHA_ATENCION { get; set; }
        public string DEPTO_RESIDENCIA { get; set; }
        public string MUN_RESIDENCIA { get; set; }
        public string NOVEDAD_RUV { get; set; }
        public string NECESIDAD_IDENTIFICADA { get; set; }
        public string MEDIDA_ASISTENCIA { get; set; }

        IgedEncuesta.Models.mdlGenerico.mdlGenerico baseDatos = new IgedEncuesta.Models.mdlGenerico.mdlGenerico();

        public List<ConstanciaSAAH> get_ModeloConstancia(string cod_hogar)
        {
            ConstanciaSAAH constanciasaah = new ConstanciaSAAH();
            StringBuilder NECESIDAD_IDENTIFICADAT = new StringBuilder("");
            StringBuilder MEDIDA_ASISTENCIAT = new StringBuilder("");
            List<ConstanciaSAAH> lconstancia = new List<ConstanciaSAAH>();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_CONSTANCIA";
            param.Add(baseDatos.asignarParametro("COD_HOGAR", 1, "System.String", cod_hogar));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        constanciasaah = new ConstanciaSAAH();

                        if (!DBNull.Value.Equals(dataReader["HOG_CODIGO"]))
                        {
                            constanciasaah.COD_HOGAR = dataReader["HOG_CODIGO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["TIPO_PERSONA"]))
                        {
                            constanciasaah.TIPO_PERSONA = dataReader["TIPO_PERSONA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"]))
                        {
                            constanciasaah.PRIMER_NOMBRE = dataReader["PER_PRIMERNOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"]))
                        {
                            constanciasaah.SEGUNDO_NOMBRE = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"]))
                        {
                            constanciasaah.PRIMER_APELLIDO = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"]))
                        {
                            constanciasaah.SEGUNDO_APELLIDO = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"]))
                        {
                            constanciasaah.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"]))
                        {
                            constanciasaah.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["ESTADO_ENCUESTA"]))
                        {
                            constanciasaah.ESTADO_ENCUESTA = dataReader["ESTADO_ENCUESTA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["DEPTO_ATENCION"]))
                        {
                            constanciasaah.DEPTO_ATENCION = dataReader["DEPTO_ATENCION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["MUN_ATENCION"]))
                        {
                            constanciasaah.MUN_ATENCION = dataReader["MUN_ATENCION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PUNTO_ATENCION"]))
                        {
                            constanciasaah.PUNTO_ATENCION= dataReader["PUNTO_ATENCION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["FECHA_ATENCION"]))
                        {
                            constanciasaah.FECHA_ATENCION = dataReader["FECHA_ATENCION"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["DEPTO_RESIDENCIA"]))
                        {
                            constanciasaah.DEPTO_RESIDENCIA = dataReader["DEPTO_RESIDENCIA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["MUN_RESIDENCIA"]))
                        {
                            constanciasaah.MUN_RESIDENCIA = dataReader["MUN_RESIDENCIA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["ESTADO_RUV"]))
                        {
                            constanciasaah.ESTADO_RUV = dataReader["ESTADO_RUV"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_1"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_1 = dataReader["HECHO_VICTIMIZANTE_1"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_2"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_2 = dataReader["HECHO_VICTIMIZANTE_2"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_3"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_3 = dataReader["HECHO_VICTIMIZANTE_3"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_4"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_4 = dataReader["HECHO_VICTIMIZANTE_4"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_5"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_5 = dataReader["HECHO_VICTIMIZANTE_5"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_6"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_6 = dataReader["HECHO_VICTIMIZANTE_6"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_7"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_7 = dataReader["HECHO_VICTIMIZANTE_7"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_8"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_8 = dataReader["HECHO_VICTIMIZANTE_8"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_9"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_9 = dataReader["HECHO_VICTIMIZANTE_9"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_10"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_10 = dataReader["HECHO_VICTIMIZANTE_10"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_11"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_11 = dataReader["HECHO_VICTIMIZANTE_11"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_12"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_12 = dataReader["HECHO_VICTIMIZANTE_12"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_13"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_13 = dataReader["HECHO_VICTIMIZANTE_13"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["HECHO_VICTIMIZANTE_14"]))
                        {
                            constanciasaah.HECHO_VICTIMIZANTE_14 = dataReader["HECHO_VICTIMIZANTE_14"].ToString();
                        }

                        if (!DBNull.Value.Equals(dataReader["NOVEDAD_RUV"]))
                        {
                            constanciasaah.NOVEDAD_RUV = dataReader["NOVEDAD_RUV"].ToString();
                        }
                        ///NECESIDAD IDENTIFICADA
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA"]))
                        {
                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA"].ToString());
                        }

                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_106"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_106"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }

                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_248"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_248"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3823"]))
                        {
                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3823"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_276"]))
                        {


                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_276"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3859"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3859"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }

                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3832"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3832"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3833"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3833"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3834"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3834"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3835"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3835"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3836"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3836"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3837"]))
                        {
                            NECESIDAD_IDENTIFICADAT.Append(";");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3837"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3838"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3838"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3839"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3839"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3840"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3840"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3841"]))
                        {
                            NECESIDAD_IDENTIFICADAT.Append(";");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3841"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3842"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3842"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("; ");
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }

                        if (!DBNull.Value.Equals(dataReader["NECESIDAD_IDENTIFICADA_3864"]))
                        {

                            NECESIDAD_IDENTIFICADAT.Append(dataReader["NECESIDAD_IDENTIFICADA_3864"].ToString());
                            NECESIDAD_IDENTIFICADAT.Append("\n");
                        }

                        //////////
                        /////////MEDIDA DE ASISTENCIA

                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA"]))
                        {
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA"].ToString());
                            MEDIDA_ASISTENCIAT.Append(";");
                            MEDIDA_ASISTENCIAT.Append("\n");
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_106"]))
                        {

                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_106"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_248"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_248"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3823"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3823"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_276"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_276"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3859"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3859"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3832"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3832"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3833"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3833"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3834"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3834"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3835"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3835"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3836"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3836"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3837"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3837"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3838"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3838"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3839"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3839"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3840"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3840"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3841"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3841"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3842"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3842"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["MEDIDA_ASISTENCIA_3864"]))
                        {
                            MEDIDA_ASISTENCIAT.Append("; ");
                            MEDIDA_ASISTENCIAT.Append("\n");
                            MEDIDA_ASISTENCIAT.Append(dataReader["MEDIDA_ASISTENCIA_3864"].ToString());
                        }                        

                        constanciasaah.NECESIDAD_IDENTIFICADA = NECESIDAD_IDENTIFICADAT.ToString();
                        constanciasaah.MEDIDA_ASISTENCIA = MEDIDA_ASISTENCIAT.ToString();

                        NECESIDAD_IDENTIFICADAT = new StringBuilder("");
                        MEDIDA_ASISTENCIAT = new StringBuilder("");


                        lconstancia.Add(constanciasaah);
                    }
                    dataReader.Close();
                }
                return lconstancia;
            }
            catch(Exception e)
            {   
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
          
            }
        }
            public int fn_getCodigoHogar(string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int totalHogar = 0;
                string funcion = "GIC_CATEGORIZACION.FN_GET_HOGAR";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                totalHogar = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return totalHogar;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int FN_UPDATE_HOGAR_SAAH(string codHogar, string estado, string userIdApp, string Usuario)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int val = 0;
                string funcion = "GIC_CATEGORIZACION.FN_UPDATE_HOGAR_SAAH";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                param.Add(baseDatos.asignarParametro("pESTADO", 1, "System.String", estado));
                param.Add(baseDatos.asignarParametro("pIDUSUARIO", 1, "System.String", userIdApp));
                param.Add(baseDatos.asignarParametro("pUSUARIO", 1, "System.String", Usuario));
                

                val = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public int FN_GET_HOGAR_CERRAD_CONSTANCIA(string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int totalHogar = 0;
                string funcion = "GIC_N_CARACTERIZACION.FN_GET_HOGAR_CERRAD_CONSTANCIA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));                
                param.Add(baseDatos.asignarParametro("pCODHOGAR", 1, "System.String", codHogar));
                totalHogar = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return totalHogar;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}