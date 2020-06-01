using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;


namespace IgedEncuesta.Models.mdlFuente
{
    public class FuentePersona : Models.mdlGenerico.mdlGenerico
    {
        public string CONS_PERSONA { get; set; }
        public string ID_TBPERSONA { get; set; }
        public string FUENTE { get; set; }
        public string TIPO_DOC { get; set; }
        public string NUMERO_DOC { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string GENERO { get; set; }
        public string NOMBRES_COMPLETOS { get; set; }
        public string FECHA_NACIMIENTO { get; set; }
        public string ESTADO_VALORACION { get; set; }
        public string HECHO_VICTIMIZANTE { get; set; }
        public string CODIGO_DECLARACION { get; set; }
        public string NUMERO_FORMULARIO { get; set; }
        public string FECHA_HECHO { get; set; }
        public string PARENTESCO { get; set; }
        
        public string ESTADO_ENCUESTA { get; set; }
        public string FECHA_ENCUESTA { get; set; }
        
        public string CODIGO_HOGAR { get; set; }
        public string VIGENCIA_ENCUESTA { get; set; }
        public string FECHA_EXPEDIENTE { get; set; }
        public string ESTADO_CEDULA { get; set; }



        public FuentePersona modeloRegistraduria(string documento)
        {
            
            IDataReader dataReader = null;
            DataSet ds = new DataSet();
            ds = consultarRegistraduria(documento);
            dataReader = ds.Tables[0].CreateDataReader();

            FuentePersona objFuente = new FuentePersona();
            while (dataReader.Read())
            {


                objFuente = new FuentePersona();
                
                objFuente.FUENTE = "REGISTRADURIA";
                objFuente.CONS_PERSONA = "";
                objFuente.ID_TBPERSONA = "";
                if (!DBNull.Value.Equals(dataReader["NUIP"])) objFuente.NUMERO_DOC = dataReader["NUIP"].ToString();
                if (!DBNull.Value.Equals(dataReader["ESTADO_CEDULA"])) objFuente.ESTADO_CEDULA = dataReader["ESTADO_CEDULA"].ToString();
                if (!DBNull.Value.Equals(dataReader["NOM1_RENEC"])) objFuente.PRIMER_NOMBRE = dataReader["NOM1_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["NOM1_RENEC"])) objFuente.PRIMER_NOMBRE = dataReader["NOM1_RENEC"].ToString();
                if (objFuente.PRIMER_NOMBRE == null)
                    return objFuente;
                if (!DBNull.Value.Equals(dataReader["NOM2_RENEC"])) objFuente.SEGUNDO_NOMBRE = dataReader["NOM2_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["APE1_RENEC"])) objFuente.PRIMER_APELLIDO = dataReader["APE1_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["APE2_RENEC"])) objFuente.SEGUNDO_APELLIDO = dataReader["APE2_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["NOMBRES"])) objFuente.NOMBRES_COMPLETOS = dataReader["NOMBRES"].ToString();
                if (!DBNull.Value.Equals(dataReader["GENERO"])) objFuente.GENERO = dataReader["GENERO"].ToString();
                if (!DBNull.Value.Equals(dataReader["FECHANACIMIENTO"])) objFuente.FECHA_NACIMIENTO = dataReader["FECHANACIMIENTO"].ToString().Substring(0,10);
            }

            return (objFuente);
        }
        
        public DataSet consultarRegistraduria(string documento)
        {

            List<Parametros> param = new List<Parametros>();

            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionRegistraduriaNUBE"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_ID_PERSONA", 1, "System.Int32", documento));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MENSAJE", 2, "System.String", ""));
                dsSalida = datos.ConsultarTablasConComando("SELECT T.NOM1_RENEC ||' '|| T.NOM2_RENEC ||' '|| T.APE1_RENEC ||' '|| T.APE2_RENEC NOMBRES, T.* FROM  TABLE(REGISTRADURIA.PKG_WS_RENEC.FUN_CONSULTA_RENEC(" + documento + ")) T");
                return dsSalida;
            }            
            finally
            {
                dsSalida.Dispose();
            }

        }        

        public DataSet consultarFuenteRUV(string numDocumento, string opcionBusqueda)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionFuenteRUV"].ConnectionString;
            datos.Conexion = connString;

            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")                        
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.CM_FUN_HECHOS_PERSONA_RUV((select F.ID_PERSONA from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv("+ numDocumento + " )) F))) T");
                    else if (opcionBusqueda == "NOMBRES Y APELLIDOS")
                    {
                        var primerNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var primerApellido = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoApellido = numDocumento;
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_name('" + primerNombre + "','" + segundoNombre + "','" + primerApellido + "','" + segundoApellido + "')) T");
                    }
                    else if (opcionBusqueda == "DECLARACION RUV")
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_decla(1,'" + numDocumento + "')) T");
                    else if (opcionBusqueda == "NUMERO DE FORMULARIO FUD")
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_decla(2,'" + numDocumento + "')) T");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return (dsSalida);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }

        public DataSet consultarFuenteSIPOD(string numDocumento, string opcionBusqueda)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = null;
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionFuenteSIPODSIV"].ConnectionString;
            datos.Conexion = connString;
            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")
                        dsSalida = datos.ConsultarTablasConComando("select 'SIPOD' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.CM_FUN_HECHOS_PERSONA_sipod((select F.ID_PERSONA from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_sipod(" + numDocumento + " )) F))) T");
                    else if (opcionBusqueda == "NOMBRES Y APELLIDOS")
                    {
                        var primerNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var primerApellido = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoApellido = numDocumento;
                        dsSalida = datos.ConsultarTablasConComando("select 'SIPOD' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_sipod_name('" + primerNombre + "','" + segundoNombre + "','" + primerApellido + "','" + segundoApellido + "')) T");
                    }
                    else
                        dsSalida = datos.ConsultarTablasConComando("select 'SIPOD' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_sipod_decla(1,'" + numDocumento + "')) T");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return (dsSalida);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }

        public DataSet consultarFuenteSIV(string numDocumento, string opcionBusqueda)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionFuenteSIPODSIV"].ConnectionString;
            datos.Conexion = connString;

            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")
                        dsSalida = datos.ConsultarTablasConComando("select 'SIV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.CM_FUN_HECHOS_PERSONA_SIV((select F.ID_PERSONA from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_siv(" + numDocumento + ")) F))) T");
                    else if (opcionBusqueda == "NOMBRES Y APELLIDOS")
                    {
                        var primerNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var primerApellido = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoApellido = numDocumento;
                        dsSalida = datos.ConsultarTablasConComando("select 'SIV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_siv_name('" + primerNombre + "','" + segundoNombre + "','" + primerApellido + "','" + segundoApellido + "')) T");
                    }
                    else if (opcionBusqueda == "SOLICITUD SIV")
                        dsSalida = datos.ConsultarTablasConComando("select 'SIV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_siv_decla('1','" + numDocumento + "')) T");
                    else if (opcionBusqueda == "FICHA SIV")
                        dsSalida = datos.ConsultarTablasConComando("select 'SIV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_siv_decla('2','" + numDocumento + "')) T");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
                return (dsSalida);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }

        
        public DataSet consultarFuenteSIRAV(string numDocumento, string opcionBusqueda)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = false;
            string connStringFuenteSIRAV = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSirav"].ConnectionString;
            datos.Conexion = connStringFuenteSIRAV;

            try
            {
                if (opcionBusqueda == "DOCUMENTO")
                    dsSalida = datos.ConsultarTablasConComando("SELECT 'SIRAV' FUENTE, T.ID_PERSONA, T.PRIMERNOMBRE, T.SEGUNDONOMBRE, T.PRIMERAPELLIDO, T.SEGUNDOAPELLIDO, " +
                        " T.TIPO_DOC,  T.NUMERODOCUMENTO, T.F_NACIMIENTO, T.ESTADO, T.NUM_FUD_NUM_CASO, T.ID_DECLARACION,T.RELACION,T.GENERO, T.FECHASINIESTRO FECHA_SINIESTRO, T.HECHO FROM SIRAVNegocio.dbo.CM_FUN_HECHOS_PERSONA_SIRAV_DOC(" + numDocumento + ") T");
                    

                    //if (dsSalida.Tables[0].Rows.Count > 0)
                    //{
                    //    String vacio = "VACIO";
                    //    Console.WriteLine(vacio);


                    //}
                    //else if (dsSalida.Tables[0].Rows.Count > 0)
                    //{
                    //    String NOMBRE = dsSalida.Tables[0].TableName;
                    //    Console.WriteLine(NOMBRE);
                    //    String idPersona = dsSalida.Tables[NOMBRE].Rows[0]["ID_PERSONA"].ToString();                        
                    //}
                
                        
                else if (opcionBusqueda == "NOMBRES Y APELLIDOS")
                {
                    var primerNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                    numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                    var segundoNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                    numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                    var primerApellido = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                    numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                    var segundoApellido = numDocumento;
                    dsSalida = datos.ConsultarTablasConComando("select 'SIRAV' FUENTE, T.* from SIRAVNegocio.dbo.f_DatosPersona_RNI_name('" + primerNombre + "','" + segundoNombre + "','" + primerApellido + "','" + segundoApellido + "') T");
                }
                else if (opcionBusqueda == "RADICADO SIRAV")
                    dsSalida = datos.ConsultarTablasConComando("select 'SIRAV' FUENTE,* from SIRAVNegocio.dbo.f_DatosPersona_RNI_decla('1','" + numDocumento + "')");

                try
                {
                    return (dsSalida);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
                return (dsSalida);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }


        public DataSet consultarFuentesALL(string numDocumento, string opcionBusqueda)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionRegistraduriaNUBE"].ConnectionString;
            datos.Conexion = connString;

            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")
                        dsSalida = datos.ConsultarTablasConComando("SELECT * FROM TABLE(registraduria.PKG_ACREDITACION.CM_FUN_ACREDIT_HECHOS_PERSONA("+ numDocumento + "))");
                        
                    else if (opcionBusqueda == "NOMBRES Y APELLIDOS")
                    {
                        var primerNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoNombre = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var primerApellido = numDocumento.Substring(0, numDocumento.IndexOf('|'));
                        numDocumento = numDocumento.Substring(numDocumento.IndexOf('|') + 1);
                        var segundoApellido = numDocumento;
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_name('" + primerNombre + "','" + segundoNombre + "','" + primerApellido + "','" + segundoApellido + "')) T");
                    }
                    else if (opcionBusqueda == "DECLARACION RUV")
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_decla(1,'" + numDocumento + "')) T");
                    else if (opcionBusqueda == "NUMERO DE FORMULARIO FUD")
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv_decla(2,'" + numDocumento + "')) T");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return (dsSalida);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }


        /*
        public DataSet consultarHechosSIRAV(string idPersona)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = false;
            string connStringFuenteSIRAV = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSirav"].ConnectionString;
            datos.Conexion = connStringFuenteSIRAV;

            dsSalida = datos.ConsultarTablasConComando("select 'SIRAV' FUENTE,* from SIRAVNegocio.dbo.CM_FUN_HECHOS_PERSONA_SIRAV('" + idPersona + "')");            
            return (dsSalida);

        }*/


    }
}