using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;

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

                //NOM1_RENEC	NOM2_RENEC	APE1_RENEC	APE2_RENEC	DEPTO_EXP	MUN_EXP	F_EXP	COD_EST_CEDULA	ESTADO_CEDULA	NUM_RESOL	ANO_RESOL, SIN INFORMACION		0

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
                if (!DBNull.Value.Equals(dataReader["FECHANACIMIENTO"])) objFuente.FECHA_NACIMIENTO = dataReader["FECHANACIMIENTO"].ToString().Substring(0, 10);
            }

            return (objFuente);
        }

        public DataSet consultarRegistraduria(string documento)
        {

            List<Parametros> param = new List<Parametros>();

            DataSet dsSalida = new DataSet();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionRegistraduriaNUBE"].ConnectionString;
                datos.Conexion = connString;
                //string idAplicacion = WebConfigurationManager.AppSettings["IdAplicacion"];
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

        public DataSet retornaRenec(string numDoc)
        {

            try
            {
                
                AccesoDatos.AccesoDatos conexionBD = new AccesoDatos.AccesoDatos();
                conexionBD.MotorBasedatos = true;
                string cadenaConex = ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
                DataSet dataRenec = new DataSet();
                conexionBD.Conexion = cadenaConex;
                List<Parametros> parametro = new List<Parametros>();
                parametro.Add(asignarParametro("p_documento", 1, "System.String", numDoc));
                parametro.Add(asignarParametro("p_result", 2, "System.Int32", ""));
                parametro.Add(asignarParametro("p_mensaje", 2, "System.String", ""));
                parametro.Add(asignarParametro("p_cur_renec", 2, "Cursor", ""));
                dataRenec = conexionBD.ConsultarConProcedimientoAlmacenado("rni_mi_pru.pkg_consulta_caracterizacion.ps_validacion_rnec", ref parametro);
                return dataRenec;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return null;
            }
        }

        public DataSet retornaFuenteALL(string numDoc)
        {

            try
            {

                AccesoDatos.AccesoDatos conexionBD = new AccesoDatos.AccesoDatos();
                conexionBD.MotorBasedatos = true;
                string cadenaConex = ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
                DataSet dataFuenAll = new DataSet();
                conexionBD.Conexion = cadenaConex;
                List<Parametros> parametro = new List<Parametros>();
                parametro.Add(asignarParametro("p_documento", 1, "System.String", numDoc));
                parametro.Add(asignarParametro("p_result", 2, "System.Int32", ""));
                parametro.Add(asignarParametro("p_mensaje", 2, "System.String", ""));
                parametro.Add(asignarParametro("p_cur_fte", 2, "Cursor", ""));
                dataFuenAll = conexionBD.ConsultarConProcedimientoAlmacenado("rni_mi_pru.pkg_consulta_caracterizacion.ps_validacion_fuentes", ref parametro);
                return dataFuenAll;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return null;
            }
        }

        

           public DataSet retornaFuenteEnc(string numDoc)
           {

            try
            {

                AccesoDatos.AccesoDatos conexionBD = new AccesoDatos.AccesoDatos();
                conexionBD.MotorBasedatos = true;
                string cadenaConex = ConfigurationManager.ConnectionStrings["ConexionModeloIntegrado"].ConnectionString;
                DataSet dataFuenAll = new DataSet();
                conexionBD.Conexion = cadenaConex;
                List<Parametros> parametro = new List<Parametros>();
                parametro.Add(asignarParametro("p_documento", 1, "System.String", numDoc));
                parametro.Add(asignarParametro("p_result", 2, "System.Int32", ""));
                parametro.Add(asignarParametro("p_mensaje", 2, "System.String", ""));
                parametro.Add(asignarParametro("p_cursor", 2, "Cursor", ""));
                dataFuenAll = conexionBD.ConsultarConProcedimientoAlmacenado("rni_mi_pru.pkg_consulta_caracterizacion.sp_entrevistas_caract", ref parametro);
                return dataFuenAll;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return null;
            }
        }


        //public async Task<List<FuentePersona>> dataRegistraduria(string numDoc)
        //   {
        //    return await Task.Run(() =>
        //    {

        //        List<FuentePersona> arregloFuente = new List<FuentePersona>();
        //        DataSet dataRenec = new DataSet();
        //        dataRenec = retornaRenec(numDoc);
        //        IDataReader leeDatos = null;

        //        try
        //        {
        //            leeDatos = dataRenec.Tables[0].CreateDataReader();

        //            while (leeDatos.Read())
        //            {
        //                FuentePersona dataFuente = new FuentePersona();
        //                if (!DBNull.Value.Equals(leeDatos["FUENTE"]))
        //                    dataFuente.FUENTE = leeDatos["FUENTE"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["TIPO_DOC"]))
        //                    dataFuente.TIPO_DOC = leeDatos["TIPO_DOC"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["ESTADO_CEDULA"]))
        //                    dataFuente.ESTADO_CEDULA = leeDatos["ESTADO_CEDULA"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["NOMBRE_COMPLETO"]))
        //                    dataFuente.NOMBRES_COMPLETOS = leeDatos["NOMBRE_COMPLETO"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["PARENTESCO"]))
        //                    dataFuente.PARENTESCO = leeDatos["PARENTESCO"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["GENERO"]))
        //                    dataFuente.GENERO = leeDatos["GENERO"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["FECHANACIMIENTO"]))
        //                    dataFuente.FECHA_NACIMIENTO = leeDatos["FECHANACIMIENTO"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["ESTADO_VALORACION"]))
        //                    dataFuente.ESTADO_VALORACION = leeDatos["ESTADO_VALORACION"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["NUM_FUD"]))
        //                    dataFuente.NUMERO_FORMULARIO = leeDatos["NUM_FUD"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["NUM_DECL"]))
        //                    dataFuente.CODIGO_DECLARACION = leeDatos["NUM_DECL"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["F_OCU"]))
        //                    dataFuente.FECHA_HECHO = leeDatos["F_OCU"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["HECHO"]))
        //                    dataFuente.HECHO_VICTIMIZANTE = leeDatos["HECHO"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["ENCUESTA"]))
        //                    dataFuente.ESTADO_ENCUESTA = leeDatos["ENCUESTA"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["ESTADO_ENC"]))
        //                    dataFuente.ESTADO_ENCUESTA = leeDatos["ESTADO_ENC"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["FECH_ENC"]))
        //                    dataFuente.FECHA_ENCUESTA = leeDatos["FECH_ENC"].ToString();
        //                if (!DBNull.Value.Equals(leeDatos["VIGENCIA_ENC"]))
        //                    dataFuente.VIGENCIA_ENCUESTA = leeDatos["VIGENCIA_ENC"].ToString();

        //                arregloFuente.Add(dataFuente);
        //            }
        //            return arregloFuente;
        //        }
        //        catch (Exception e)
        //        {
        //            e.Message.ToString();
        //            return null;
        //        }
        //    });

        //}

        
        public async Task<List<FuentePersona>> dataRenec(string numDoc)
        {
            return await Task.Run(() =>
            {
               

                try
                {
                    List<FuentePersona> arregloFuente = new List<FuentePersona>();
                    DataSet dataFuenteAll = new DataSet();
                    dataFuenteAll = retornaRenec(numDoc);
                    IDataReader leeDatos = null;
                    leeDatos = dataFuenteAll.Tables[0].CreateDataReader();

                    while (leeDatos.Read())
                    {
                        FuentePersona dataFuente = new FuentePersona();
                        if (!DBNull.Value.Equals(leeDatos["FUENTE"]))
                            dataFuente.FUENTE = leeDatos["FUENTE"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["TIPO_DOC"]))
                            dataFuente.TIPO_DOC = leeDatos["TIPO_DOC"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_CEDULA"]))
                            dataFuente.ESTADO_CEDULA = leeDatos["ESTADO_CEDULA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NOMBRE_COMPLETO"]))
                            dataFuente.NOMBRES_COMPLETOS = leeDatos["NOMBRE_COMPLETO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["PARENTESCO"]))
                            dataFuente.PARENTESCO = leeDatos["PARENTESCO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["GENERO"]))
                            dataFuente.GENERO = leeDatos["GENERO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["FECHANACIMIENTO"]))
                            dataFuente.FECHA_NACIMIENTO = leeDatos["FECHANACIMIENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_VALORACION"]))
                            dataFuente.ESTADO_VALORACION = leeDatos["ESTADO_VALORACION"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NUM_FUD"]))
                            dataFuente.NUMERO_FORMULARIO = leeDatos["NUM_FUD"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NUM_DECL"]))
                            dataFuente.CODIGO_DECLARACION = leeDatos["NUM_DECL"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["F_OCU"]))
                            dataFuente.FECHA_HECHO = leeDatos["F_OCU"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["HECHO"]))
                            dataFuente.HECHO_VICTIMIZANTE = leeDatos["HECHO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ENCUESTA"]))
                            dataFuente.ESTADO_ENCUESTA = leeDatos["ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_ENC"]))
                            dataFuente.ESTADO_ENCUESTA = leeDatos["ESTADO_ENC"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["FECH_ENC"]))
                            dataFuente.FECHA_ENCUESTA = leeDatos["FECH_ENC"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["VIGENCIA_ENC"]))
                            dataFuente.VIGENCIA_ENCUESTA = leeDatos["VIGENCIA_ENC"].ToString();

                        arregloFuente.Add(dataFuente);
                        break;
                    }
                    return arregloFuente;
                }
                finally { }     
            });


        }

        public async Task<List<FuentePersona>> dataFuentesCarac(string numDoc)
        {
            return await Task.Run(() =>
            {

             List<FuentePersona> arregloFuente = new List<FuentePersona>();
                DataSet dataFuenteAll = new DataSet();
                dataFuenteAll = retornaFuenteALL(numDoc);
                IDataReader leeDatos = null;

                try
                {
                    leeDatos = dataFuenteAll.Tables[0].CreateDataReader();

                    while (leeDatos.Read())
                    {
                        FuentePersona dataFuente = new FuentePersona();
                        if (!DBNull.Value.Equals(leeDatos["FUENTE"]))
                            dataFuente.FUENTE = leeDatos["FUENTE"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["TIPO_DOCUMENTO"]))
                            dataFuente.TIPO_DOC = leeDatos["TIPO_DOCUMENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["DOCUMENTO"]))
                            dataFuente.NUMERO_DOC = leeDatos["DOCUMENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_CEDULA"]))
                            dataFuente.ESTADO_CEDULA = leeDatos["ESTADO_CEDULA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NOMBRE_COMPLETO"]))
                            dataFuente.NOMBRES_COMPLETOS = leeDatos["NOMBRE_COMPLETO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["RELACION"]))
                            dataFuente.PARENTESCO = leeDatos["RELACION"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["GENERO"]))
                            dataFuente.GENERO = leeDatos["GENERO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["F_NACIMIENTO"]))
                            dataFuente.FECHA_NACIMIENTO = leeDatos["F_NACIMIENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO"]))
                            dataFuente.ESTADO_VALORACION = leeDatos["ESTADO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NUM_FUD_NUM_CASO"]))
                            dataFuente.NUMERO_FORMULARIO = leeDatos["NUM_FUD_NUM_CASO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ID_DECLARACION"]))
                            dataFuente.CODIGO_DECLARACION = leeDatos["ID_DECLARACION"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["FECHA_SINIESTRO"]))
                            dataFuente.FECHA_HECHO = leeDatos["FECHA_SINIESTRO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["HECHO"]))
                            dataFuente.HECHO_VICTIMIZANTE = leeDatos["HECHO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ENCUESTA"]))
                            dataFuente.CODIGO_HOGAR = leeDatos["ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_ENCUESTA"]))
                            dataFuente.ESTADO_ENCUESTA = leeDatos["ESTADO_ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["FECHA_ENCUESTA"]))
                            dataFuente.FECHA_ENCUESTA = leeDatos["FECHA_ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["VIGENCIA_ENCUESTA"]))
                            dataFuente.VIGENCIA_ENCUESTA = leeDatos["VIGENCIA_ENCUESTA"].ToString();

                        arregloFuente.Add(dataFuente);
                    }
                    return arregloFuente;
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    return null;
                }
            });
        }

        public async Task<List<FuentePersona>> dataFuentesCaracEncues(string numDoc)
        {
            return await Task.Run(() =>
            {

                List<FuentePersona> arregloFuente = new List<FuentePersona>();
                DataSet dataFuenteAll = new DataSet();
                dataFuenteAll = retornaFuenteEnc(numDoc);
                IDataReader leeDatos = null;

                try
                {
                    leeDatos = dataFuenteAll.Tables[0].CreateDataReader();

                    while (leeDatos.Read())
                    {
                        FuentePersona dataFuente = new FuentePersona();
                        if (!DBNull.Value.Equals(leeDatos["PROGRAMA"]))
                            dataFuente.FUENTE = leeDatos["PROGRAMA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["PER_TIPODOC"]))
                            dataFuente.TIPO_DOC = leeDatos["PER_TIPODOC"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["PER_DOCUMENTO"]))
                            dataFuente.NUMERO_DOC = leeDatos["PER_DOCUMENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_CED"]))
                            dataFuente.ESTADO_CEDULA = leeDatos["ESTADO_CED"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NOMBRE_COMPLETO"]))
                            dataFuente.NOMBRES_COMPLETOS = leeDatos["NOMBRE_COMPLETO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["RELACION"]))
                            dataFuente.PARENTESCO = leeDatos["RELACION"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["PER_SEXO"]))
                            dataFuente.GENERO = leeDatos["PER_SEXO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["R_FECHANACIMIENTO"]))
                            dataFuente.FECHA_NACIMIENTO = leeDatos["R_FECHANACIMIENTO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_VALO"]))
                            dataFuente.ESTADO_VALORACION = leeDatos["ESTADO_VALO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NUM_FOR"]))
                            dataFuente.NUMERO_FORMULARIO = leeDatos["NUM_FOR"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["NUM_DECL"]))
                            dataFuente.CODIGO_DECLARACION = leeDatos["NUM_DECL"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["F_SINIESTRO"]))
                            dataFuente.FECHA_HECHO = leeDatos["F_SINIESTRO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["HECHO"]))
                            dataFuente.HECHO_VICTIMIZANTE = leeDatos["HECHO"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["CODIGO_HOGAR"]))
                            dataFuente.CODIGO_HOGAR = leeDatos["CODIGO_HOGAR"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["ESTADO_ENCUESTA"]))
                            dataFuente.ESTADO_ENCUESTA = leeDatos["ESTADO_ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["FECHA_ENCUESTA"]))
                            dataFuente.FECHA_ENCUESTA = leeDatos["FECHA_ENCUESTA"].ToString();
                        if (!DBNull.Value.Equals(leeDatos["VIGENCIA_ENCUESTA"]))
                            dataFuente.VIGENCIA_ENCUESTA = leeDatos["VIGENCIA_ENCUESTA"].ToString();

                        arregloFuente.Add(dataFuente);
                    }
                    return arregloFuente;
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    return null;
                }
            });
        }



        public DataSet consultarFuenteRUV(string numDocumento, string opcionBusqueda)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            DataSet dsSalida = new DataSet();
            datos.MotorBasedatos = true;
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings[/*"ConexionFuenteRUV" */"Conexioncar"].ConnectionString;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionFuenteRUV"].ConnectionString;
            datos.Conexion = connString;

            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")
                        dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.CM_FUN_HECHOS_PERSONA_RUV((select F.ID_PERSONA from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv(" + numDocumento + " )) F))) T");
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
                    //log.Debug(e.Message);
                    e.Message.ToString();
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
            DataSet dsSalida = new DataSet();
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
                    e.Message.ToString();
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
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings[/*"ConexionFuenteSIPODSIV"*/"Conexioncar"].ConnectionString;
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
                    //log.Debug(e.Message);
                    e.Message.ToString();

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
                    //dsSalida = datos.ConsultarTablasConComando("select 'SIRAV' FUENTE,   T.PRIMER_NOMBRE NOMBRE1, T.SEGUNDO_NOMBRE NOMBRE2, T.PRIMER_APELLIDO APELLIDO1, T.SEGUNDO_APELLIDO APELLIDO2, T.TIPO_DOCUMENTO TIPO_DOCUMENTO, T.DOCUMENTO DOCUMENTO, T.*  from SIRAVNegocio.dbo.f_DatosPersona_RNI('" + numDocumento + "') T");
                    dsSalida = datos.ConsultarTablasConComando("SELECT 'SIRAV' FUENTE, T.ID_PERSONA, T.PRIMERNOMBRE, T.SEGUNDONOMBRE, T.PRIMERAPELLIDO, T.SEGUNDOAPELLIDO, " +
                        " T.TIPO_DOC,  T.NUMERODOCUMENTO, T.F_NACIMIENTO, T.ESTADO, T.NUM_FUD_NUM_CASO, T.ID_DECLARACION,T.RELACION,T.GENERO, T.FECHASINIESTRO FECHA_SINIESTRO, T.HECHO FROM SIRAVNegocio.dbo.CM_FUN_HECHOS_PERSONA_SIRAV_DOC(" + numDocumento + ") T");


                if (dsSalida.Tables[0].Rows.Count > 0)
                {
                    String vacio = "VACIO";
                    Console.WriteLine(vacio);


                }
                else if (dsSalida.Tables[0].Rows.Count > 0)
                {
                    String NOMBRE = dsSalida.Tables[0].TableName;
                    Console.WriteLine(NOMBRE);
                    String idPersona = dsSalida.Tables[NOMBRE].Rows[0]["ID_PERSONA"].ToString();
                }


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
                    e.Message.ToString();

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
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings[/*"ConexionFuenteRUV" */"Conexioncar"].ConnectionString;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionRegistraduriaNUBE"].ConnectionString;
            datos.Conexion = connString;

            try
            {
                try
                {
                    if (opcionBusqueda == "DOCUMENTO")
                        //dsSalida = datos.ConsultarTablasConComando("select 'RUV' FUENTE, T.* from TABLE(PKG_VICTIMAS_RNI.CM_FUN_HECHOS_PERSONA_RUV((select F.ID_PERSONA from TABLE(PKG_VICTIMAS_RNI.cm_fun_persona_ruv(" + numDocumento + " )) F))) T");
                        dsSalida = datos.ConsultarTablasConComando("SELECT * FROM TABLE(registraduria.PKG_ACREDITACION.CM_FUN_ACREDIT_HECHOS_PERSONA(" + numDocumento + "))");

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
                    //log.Debug(e.Message);
                    e.Message.ToString();
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
    
   
   