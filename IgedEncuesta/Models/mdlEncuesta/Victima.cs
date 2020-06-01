using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ObjetosTipos;
using System.Text;

namespace IgedEncuesta.Models.mdlEncuesta
{
    public class Victima : Models.mdlGenerico.mdlGenerico
    {

        public string CONS_PERSONA { get; set; }
        public string TIPO_DOC { get; set; }
        public string DOCUMENTO { get; set; }
        public string NOMBRES_COMPLETOS { get; set; }
        public string NOMBRE1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string APELLIDO1 { get; set; }
        public string APELLIDO2 { get; set; }
        public string GENERO_HOM { get; set; }
        public string PERT_ETNICA { get; set; }
        public string DISCAP { get; set; }
        public string F_NACIMIENTO { get; set; }
        public string SOBREVIVENCIA { get; set; }
        public string IDENTIFICADO { get; set; }
        public string HV1 { get; set; }
        public string FECHA_HECHO1 { get; set; }
        public string HV2 { get; set; }
        public string FECHA_HECHO2 { get; set; }
        public string HV3 { get; set; }
        public string FECHA_HECHO3 { get; set; }
        public string HV4 { get; set; }
        public string FECHA_HECHO4 { get; set; }
        public string HV5 { get; set; }
        public string FECHA_HECHO5 { get; set; }
        public string HV6 { get; set; }
        public string FECHA_HECHO6 { get; set; }
        public string HV7 { get; set; }
        public string FECHA_HECHO7 { get; set; }
        public string HV8 { get; set; }
        public string FECHA_HECHO8 { get; set; }
        public string HV9 { get; set; }
        public string FECHA_HECHO9 { get; set; }
        public string HV10 { get; set; }
        public string FECHA_HECHO10 { get; set; }
        public string HV11 { get; set; }
        public string FECHA_HECHO11 { get; set; }
        public string HV12 { get; set; }
        public string FECHA_HECHO12 { get; set; }
        public string HV13 { get; set; }
        public string FECHA_HECHO13 { get; set; }
        public string HV14 { get; set; }
        public string FECHA_HECHO14 { get; set; }
        public string TIPO_VICTIMA { get; set; }
        public bool JEFE_HOGAR { get; set; }
        public string ID_TBPERSONA { get; set; }
        public string FECHA_ULT_CARACTERIZACION { get; set; }
        public string DOCUMENTO_RUV { get; set; }
        public string DOCUMENTO_CARACTERIZACION { get; set; }
        public string HABILITADO_PARA_CARACTERIZACION { get; set; }
        public string CONS_PERSONA_ESTADO { get; set; }
        public string MI_IDPERSONA { get; set; }
        public string COD_HOGAR { get; set; }
        public string ESTADO_ENCUESTA { get; set; }
        public string ID_CARACTERIZACION { get; set; }

        public string TIPO_PERSONA { get; set; }
        public string HECHOS { get; set; }

        public string EDAD { get; set; }

        public string FECHA_HECHO { get; set; }


        public DataSet consultarRegistraduria(string documento)
        {

            List<Parametros> param;
            DataSet dsSalida;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionRegistraduria"].ConnectionString;
                datos.Conexion = connString;

                param = new List<Parametros>();
                param.Add(asignarParametro("P_ID_PERSONA", 1, "System.Int32", documento));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MENSAJE", 2, "System.String", ""));
                dsSalida = datos.ConsultarTablasConComando("SELECT * FROM  TABLE(REGISTRADURIA.PKG_WS_RENEC.FUN_CONSULTA_RENEC(" + documento + "))");
                return dsSalida;
            }
            finally
            {
                //dsSalida.Dispose();
                //datos.Dispose();
            }

        }

        public Victima modeloRegistraduria(string documento)
        {

            IDataReader dataReader = null;
            DataSet ds;
            ds = consultarRegistraduria(documento);
            dataReader = ds.Tables[0].CreateDataReader();

            Victima objVictima = new Victima();
            while (dataReader.Read())
            {

                //NOM1_RENEC	NOM2_RENEC	APE1_RENEC	APE2_RENEC	DEPTO_EXP	MUN_EXP	F_EXP	COD_EST_CEDULA	ESTADO_CEDULA	NUM_RESOL	ANO_RESOL, SIN INFORMACION		0

                objVictima = new Victima();
                if (!DBNull.Value.Equals(dataReader["NOM1_RENEC"])) objVictima.NOMBRE1 = dataReader["NOM1_RENEC"].ToString();
                if (objVictima.NOMBRE1 == null)
                    return objVictima;
                if (!DBNull.Value.Equals(dataReader["NOM2_RENEC"])) objVictima.NOMBRE2 = dataReader["NOM2_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["APE1_RENEC"])) objVictima.APELLIDO1 = dataReader["APE1_RENEC"].ToString();
                if (!DBNull.Value.Equals(dataReader["APE2_RENEC"])) objVictima.APELLIDO2 = dataReader["APE2_RENEC"].ToString();
            }

            return (objVictima);
        }


        public List<Victima> consultarVictimasMI(string numDoc, string idUsuario, string idAplicacion)
        {
            DataSet dsSalida = null;
            List<Victima> coleccion = null;
            HechosPersona hechos = new HechosPersona();
            dsSalida = consultarPersonasModeloINntegrado(numDoc, idUsuario, idAplicacion);
            coleccion = modeloVictimasMI(dsSalida);

            StringBuilder vhechos = new StringBuilder("");

            StringBuilder fechashechos = new StringBuilder("");

            // Entra si encontró victimas registradas en el RUV para el número de documento suministrado
            if (coleccion.Count > 0)
            {
                foreach (Victima item in coleccion)
                {

                    try
                    {
                        DateTime parsedDate = DateTime.Parse(item.F_NACIMIENTO.ToString());
                        string fechaN = parsedDate.ToString("dd/MM/yyyy");
                        var array_fecha = fechaN.Split('/');

                        int dia = Convert.ToInt32((array_fecha[0]));
                        int mes = Convert.ToInt32((array_fecha[1]));
                        int anio = Convert.ToInt32((array_fecha[2]));


                        DateTime fechaNacimiento = new DateTime(anio, mes, dia);
                        DateTime ahora = DateTime.Today;
                        item.EDAD = calcularEdad(fechaNacimiento, ahora).ToString();
                    }
                    catch (Exception e)
                    {
                        item.EDAD = "0";
                    }

                    // Verifica si la victima identificada en RUV ya fue caracterizada en la tabla GIC_RUV_PERSONAS
                    hechos = hechos.hechosVictimizantes(item.MI_IDPERSONA);
                    if (hechos.PER_ID != null)
                        item.TIPO_VICTIMA = "INCLUIDO";
                    else
                        item.TIPO_VICTIMA = "NO INCLUIDO";
                    item.HV1 = hechos.HV1;

                    if (Convert.ToInt32(hechos.HV5) > 0)
                    {
                        vhechos.Append("Desplazamiento forzado");
                        if (!DBNull.Value.Equals(hechos.Fecha_HV5) && hechos.Fecha_HV5 != null)
                        {
                            try
                            {
                                item.FECHA_HECHO = hechos.Fecha_HV5.ToString().ToUpper().Substring(0, 10).Trim();
                                item.FECHA_HECHO5 = hechos.Fecha_HV5.ToString().Substring(0, 10);

                                fechashechos.Append(hechos.Fecha_HV5.ToString().ToUpper().Substring(0, 10).Trim());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                        }

                    }

                    if (Convert.ToInt32(hechos.HV1) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Acto terrosita");
                            fechashechos.Append(" - " + hechos.Fecha_HV1.ToString().ToUpper().Substring(0, 10).Trim());
                        }
                        if (!DBNull.Value.Equals(hechos.Fecha_HV1) && hechos.Fecha_HV1 != null)
                            try
                            {
                                item.FECHA_HECHO1 = hechos.Fecha_HV1.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }

                    if (Convert.ToInt32(hechos.HV2) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Amenaza");
                            fechashechos.Append(" - " + hechos.Fecha_HV2.ToString().ToUpper().Substring(0, 10).Trim());
                        }
                        if (!DBNull.Value.Equals(hechos.Fecha_HV2) && hechos.Fecha_HV2 != null)
                            try
                            {
                                item.FECHA_HECHO2 = hechos.Fecha_HV2.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV3) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {

                            vhechos.Append(" - " + "Delitos contra la libertad e integridad");
                            fechashechos.Append(" - " + hechos.Fecha_HV3.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV3) && hechos.Fecha_HV3 != null)
                            try
                            {
                                item.FECHA_HECHO3 = hechos.Fecha_HV3.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }
                    if (Convert.ToInt32(hechos.HV4) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Desaparición forzada");
                            fechashechos.Append(" - " + hechos.Fecha_HV4.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV4) && hechos.Fecha_HV4 != null)
                            try
                            {
                                item.FECHA_HECHO4 = hechos.Fecha_HV4.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                    }

                    if (Convert.ToInt32(hechos.HV6) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {

                            vhechos.Append(" - " + "Homicido");
                            fechashechos.Append(" - " + hechos.Fecha_HV6.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV6) && hechos.Fecha_HV6 != null)
                            try
                            {
                                item.FECHA_HECHO6 = hechos.Fecha_HV6.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }
                    if (Convert.ToInt32(hechos.HV7) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Minas antipersonal MUSE");
                            fechashechos.Append(" - " + hechos.Fecha_HV7.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV7) && hechos.Fecha_HV7 != null)
                            try
                            {
                                item.FECHA_HECHO7 = hechos.Fecha_HV7.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV8) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Secuestro");
                            fechashechos.Append(" - " + hechos.Fecha_HV8.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV8) && hechos.Fecha_HV8 != null)
                            try
                            {
                                item.FECHA_HECHO8 = hechos.Fecha_HV8.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }
                    if (Convert.ToInt32(hechos.HV9) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Tortura");
                            fechashechos.Append(" - " + hechos.Fecha_HV9.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV9) && hechos.Fecha_HV9 != null)
                            try
                            {
                                item.FECHA_HECHO9 = hechos.Fecha_HV9.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }
                    if (Convert.ToInt32(hechos.HV10) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Vinculación NNA");
                            fechashechos.Append(" - " + hechos.Fecha_HV10.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV10) && hechos.Fecha_HV10 != null)
                            try
                            {
                                item.FECHA_HECHO10 = hechos.Fecha_HV10.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV11) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Abandono o despojo forzado de tierras");
                            fechashechos.Append(" - " + hechos.Fecha_HV11.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV11) && hechos.Fecha_HV11 != null)
                            try
                            {
                                item.FECHA_HECHO11 = hechos.Fecha_HV11.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV12) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Perdida de bienes muebles e inmuebles");
                            fechashechos.Append(" - " + hechos.Fecha_HV12.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV12) && hechos.Fecha_HV12 != null)
                            try
                            {
                                item.FECHA_HECHO12 = hechos.Fecha_HV12.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV13) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {
                            vhechos.Append(" - " + "Otros");
                            fechashechos.Append(" - " + hechos.Fecha_HV13.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV13) && hechos.Fecha_HV13 != null)
                            try
                            {
                                item.FECHA_HECHO13 = hechos.Fecha_HV13.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                    }
                    if (Convert.ToInt32(hechos.HV14) > 0)
                    {
                        if (Convert.ToInt32(hechos.HV5) == 0)
                        {

                            vhechos.Append(" - " + "Sin información");
                            fechashechos.Append(" - " + hechos.Fecha_HV14.ToString().ToUpper().Substring(0, 10).Trim());
                        }

                        if (!DBNull.Value.Equals(hechos.Fecha_HV14) && hechos.Fecha_HV14 != null)
                            try
                            {
                                item.FECHA_HECHO12 = hechos.Fecha_HV12.ToString().Substring(0, 10);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                    }
                    try
                    {

                        item.FECHA_HECHO = fechashechos.ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    item.HV2 = hechos.HV2;
                    item.HV3 = hechos.HV3;
                    item.HV4 = hechos.HV4;
                    item.HV5 = hechos.HV5;
                    item.HV6 = hechos.HV6;
                    item.HV7 = hechos.HV7;
                    item.HV8 = hechos.HV8;
                    item.HV9 = hechos.HV9;
                    item.HV10 = hechos.HV10;
                    item.HV11 = hechos.HV11;
                    item.HV12 = hechos.HV12;
                    item.HV13 = hechos.HV13;
                    item.HV14 = hechos.HV14;


                    try
                    {
                        item.HECHOS = vhechos.ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }



                    item.ID_CARACTERIZACION = hechos.ID_CARACTERIZACION;
                    item.ID_TBPERSONA = hechos.ID_CARACTERIZACION;
                    item.FECHA_ULT_CARACTERIZACION = hechos.FECHA_ULTI_ENCUESTA;
                    item.COD_HOGAR = hechos.COD_HOGAR;
                    item.ESTADO_ENCUESTA = hechos.ESTADO_ENCUESTA;
                    if (hechos.HABILITADO_CARAC == null || hechos.HABILITADO_CARAC == "SI")
                        item.HABILITADO_PARA_CARACTERIZACION = "SI";
                    else
                        item.HABILITADO_PARA_CARACTERIZACION = hechos.HABILITADO_CARAC;

                }


            }

            return (coleccion.Distinct().ToList());

        }


        public DataSet consultarVictimasRUV(string numDoc, string opcionBusqueda)
        {
            
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("pDocumento", 1, "System.String", numDoc));
                param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
                param.Add(asignarParametro("p_Salida", 2, "System.Int32", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NUEVOS_INCLUIDOS.PR_GET_INCLUIDO", ref param);
            }
            finally
            {
                dsSalida.Dispose();

            }
            return dsSalida;
        }

        public List<Victima> modeloVictimas(DataSet ds)
        {
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            List<Victima> maestroHogar = null;


            maestroHogar = (List<Victima>)HttpContext.Current.Session["ModeloHogar"];
            while (dataReader.Read())
            {


                Victima objVictima = new Victima();
                if (!DBNull.Value.Equals(dataReader["MI_IDPERSONA"])) objVictima.MI_IDPERSONA = dataReader["MI_IDPERSONA"].ToString();
                if (!DBNull.Value.Equals(dataReader["CONS_PERSONA"])) objVictima.CONS_PERSONA = dataReader["CONS_PERSONA"].ToString();
                if (!DBNull.Value.Equals(dataReader["TIPO_DOC"])) objVictima.TIPO_DOC = dataReader["TIPO_DOC"].ToString();
                if (!DBNull.Value.Equals(dataReader["DOCUMENTO"])) objVictima.DOCUMENTO = dataReader["DOCUMENTO"].ToString();
                if (!DBNull.Value.Equals(dataReader["NOMBRE1"])) objVictima.NOMBRE1 = dataReader["NOMBRE1"].ToString();
                if (!DBNull.Value.Equals(dataReader["NOMBRE2"])) objVictima.NOMBRE2 = dataReader["NOMBRE2"].ToString();
                if (!DBNull.Value.Equals(dataReader["APELLIDO1"])) objVictima.APELLIDO1 = dataReader["APELLIDO1"].ToString();
                if (!DBNull.Value.Equals(dataReader["APELLIDO2"])) objVictima.APELLIDO2 = dataReader["APELLIDO2"].ToString();

                //------------------------------------------------
                //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                // ADICION CAMPO DOCUMENTO RUV
                //-----------------------------------------------
                objVictima.DOCUMENTO_RUV = objVictima.DOCUMENTO;
                //FIN MODIFICACION OCT.27.2015

                objVictima.NOMBRES_COMPLETOS = objVictima.NOMBRE1 + ' ' + objVictima.NOMBRE2 + ' ' + objVictima.APELLIDO1 + ' ' + objVictima.APELLIDO2;

                if (!DBNull.Value.Equals(dataReader["GENERO_HOM"])) objVictima.GENERO_HOM = dataReader["GENERO_HOM"].ToString();
                if (!DBNull.Value.Equals(dataReader["PERT_ETNICA"])) objVictima.PERT_ETNICA = dataReader["PERT_ETNICA"].ToString();
                if (!DBNull.Value.Equals(dataReader["DISCAP"])) objVictima.DISCAP = dataReader["DISCAP"].ToString();
                if (!DBNull.Value.Equals(dataReader["F_NACIMIENTO"])) objVictima.F_NACIMIENTO = dataReader["F_NACIMIENTO"].ToString().ToUpper().Replace("12:00:00 AM", "");

                if (!DBNull.Value.Equals(dataReader["SOBREVIVENCIA"])) objVictima.SOBREVIVENCIA = dataReader["SOBREVIVENCIA"].ToString();
                if (!DBNull.Value.Equals(dataReader["IDENTIFICADO"])) objVictima.IDENTIFICADO = dataReader["IDENTIFICADO"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV1"])) objVictima.HV1 = dataReader["HV1"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV2"])) objVictima.HV2 = dataReader["HV2"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV3"])) objVictima.HV3 = dataReader["HV3"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV4"])) objVictima.HV4 = dataReader["HV4"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV5"])) objVictima.HV5 = dataReader["HV5"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV6"])) objVictima.HV6 = dataReader["HV6"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV7"])) objVictima.HV7 = dataReader["HV7"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV8"])) objVictima.HV8 = dataReader["HV8"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV9"])) objVictima.HV9 = dataReader["HV9"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV10"])) objVictima.HV10 = dataReader["HV10"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV11"])) objVictima.HV11 = dataReader["HV11"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV12"])) objVictima.HV12 = dataReader["HV12"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV13"])) objVictima.HV13 = dataReader["HV13"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV14"])) objVictima.HV14 = dataReader["HV14"].ToString();
                objVictima.TIPO_VICTIMA = "INCLUIDO";
                //------------------------------------------------
                //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                // LAS VICTIMAS NO HAN SIDO CARACTERIZADAS HASTA BUSCAR EN CARACTERIZACION
                //-----------------------------------------------
                objVictima.HABILITADO_PARA_CARACTERIZACION = "SI";
                //FIN JOSE VASQUEZ OCT.28.2015

                coleccion.Add(objVictima);

            }

            return (coleccion);
        }


        public List<Victima> modeloVictimasMI(DataSet ds)
        {
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();

            while (dataReader.Read())
            {

                Victima objVictima = new Victima();
                if (!DBNull.Value.Equals(dataReader["PER_ID"])) objVictima.MI_IDPERSONA = dataReader["PER_ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_ID"])) objVictima.CONS_PERSONA = dataReader["PER_ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) objVictima.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_DOCUMENTO"])) objVictima.DOCUMENTO = dataReader["PER_DOCUMENTO"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_NOMBRE1"])) objVictima.NOMBRE1 = dataReader["PER_NOMBRE1"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_NOMBRE2"])) objVictima.NOMBRE2 = dataReader["PER_NOMBRE2"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_APELLIDO1"])) objVictima.APELLIDO1 = dataReader["PER_APELLIDO1"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_APELLIDO2"])) objVictima.APELLIDO2 = dataReader["PER_APELLIDO2"].ToString();
                objVictima.NOMBRES_COMPLETOS = objVictima.NOMBRE1 + ' ' + objVictima.NOMBRE2 + ' ' + objVictima.APELLIDO1 + ' ' + objVictima.APELLIDO2;

                if (!DBNull.Value.Equals(dataReader["PER_IDENTIDAD_GENERO"])) objVictima.GENERO_HOM = dataReader["PER_IDENTIDAD_GENERO"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_ETNIA"])) objVictima.PERT_ETNICA = dataReader["PER_ETNIA"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_DISCAPACIDAD"])) objVictima.DISCAP = dataReader["PER_DISCAPACIDAD"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) objVictima.F_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString().ToUpper().Replace("12:00:00 AM", "");
                if (!DBNull.Value.Equals(dataReader["PER_SEXO"])) objVictima.GENERO_HOM = dataReader["PER_SEXO"].ToString();
                /*
                if (!DBNull.Value.Equals(dataReader["EDAD"])) objVictima.EDAD = dataReader["EDAD"].ToString();
                if (!DBNull.Value.Equals(dataReader["HECHOS"])) objVictima.HECHOS = dataReader["HECHOS"].ToString();
                if (!DBNull.Value.Equals(dataReader["FECHA_HECHO"])) objVictima.FECHA_HECHO = dataReader["FECHA_HECHO"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV1"])) objVictima.HV1 = dataReader["HV1"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV2"])) objVictima.HV2 = dataReader["HV2"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV3"])) objVictima.HV3 = dataReader["HV3"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV4"])) objVictima.HV4 = dataReader["HV4"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV5"])) objVictima.HV5 = dataReader["HV5"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV6"])) objVictima.HV6 = dataReader["HV6"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV7"])) objVictima.HV7 = dataReader["HV7"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV8"])) objVictima.HV8 = dataReader["HV8"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV9"])) objVictima.HV9 = dataReader["HV9"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV10"])) objVictima.HV10 = dataReader["HV10"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV11"])) objVictima.HV11 = dataReader["HV11"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV12"])) objVictima.HV12 = dataReader["HV12"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV13"])) objVictima.HV13 = dataReader["HV13"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV14"])) objVictima.HV14 = dataReader["HV14"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV1_FECHA"])) objVictima.FECHA_HECHO1 = dataReader["HV1_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV2_FECHA"])) objVictima.FECHA_HECHO2 = dataReader["HV2_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV3_FECHA"])) objVictima.FECHA_HECHO3 = dataReader["HV3_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV4_FECHA"])) objVictima.FECHA_HECHO4 = dataReader["HV4_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV5_FECHA"])) objVictima.FECHA_HECHO5 = dataReader["HV5_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV6_FECHA"])) objVictima.FECHA_HECHO6 = dataReader["HV6_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV7_FECHA"])) objVictima.FECHA_HECHO7 = dataReader["HV7_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV8_FECHA"])) objVictima.FECHA_HECHO8 = dataReader["HV8_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV9_FECHA"])) objVictima.FECHA_HECHO9 = dataReader["HV9_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV10_FECHA"])) objVictima.FECHA_HECHO10 = dataReader["HV10_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV11_FECHA"])) objVictima.FECHA_HECHO11 = dataReader["HV11_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV12_FECHA"])) objVictima.FECHA_HECHO12 = dataReader["HV12_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV13_FECHA"])) objVictima.FECHA_HECHO13 = dataReader["HV13_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HV14_FECHA"])) objVictima.FECHA_HECHO14 = dataReader["HV14_FECHA"].ToString();
                if (!DBNull.Value.Equals(dataReader["ESTADO_VICTIMA"])) objVictima.TIPO_VICTIMA = dataReader["ESTADO_VICTIMA"].ToString().ToUpper();
                if (!DBNull.Value.Equals(dataReader["ID_CARACTERIZACION"])) objVictima.ID_CARACTERIZACION = dataReader["ID_CARACTERIZACION"].ToString();
                if (!DBNull.Value.Equals(dataReader["ID_CARACTERIZACION"])) objVictima.ID_TBPERSONA = dataReader["ID_CARACTERIZACION"].ToString();
                if (!DBNull.Value.Equals(dataReader["FECHA_ULTI_ENCUESTA"])) objVictima.FECHA_ULT_CARACTERIZACION = dataReader["FECHA_ULTI_ENCUESTA"].ToString();
                if (!DBNull.Value.Equals(dataReader["COD_HOGAR"])) objVictima.COD_HOGAR = dataReader["COD_HOGAR"].ToString();
                if (!DBNull.Value.Equals(dataReader["ESTADO_ENCUESTA"])) objVictima.ESTADO_ENCUESTA = dataReader["ESTADO_ENCUESTA"].ToString();
                if (!DBNull.Value.Equals(dataReader["HABILITADO_CARAC"])) objVictima.HABILITADO_PARA_CARACTERIZACION = dataReader["HABILITADO_CARAC"].ToString();
                */

                coleccion.Add(objVictima);

            }

            return (coleccion);
        }

        public DataSet consultarGrupoFamiliar(string cons_persona)
        {
            
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("p_conspersona", 1, "System.String", cons_persona));
                param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
                param.Add(asignarParametro("p_Salida", 2, "System.Int32", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NUEVOS_INCLUIDOS.PR_GET_GRUPO_FAMILIAR", ref param);
                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }

        public List<string> consultarVictimasPersonas(string consPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            IDataReader dataReader = null;
            List<string> idPersonas = new List<string>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", consPersona));
                param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET_RUV_PERSONA_X_RUV", ref param);

                if (dsSalida.Tables.Count > 0)
                {
                    dataReader = dsSalida.Tables[0].CreateDataReader();
                    while (dataReader.Read())
                        idPersonas.Add(dataReader["PER_IDPERSONA"].ToString());
                }
                return idPersonas;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }

        public void Insertar_TIntermedia_RUV_y_Personas()
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                if (!string.IsNullOrEmpty(CONS_PERSONA) && !string.IsNullOrEmpty(ID_TBPERSONA))
                {

                    datos.Conexion = connStringCar;

                    datos.Conexion = connStringCar;

                    param = new List<Parametros>();
                    if (!CONS_PERSONA.StartsWith("NI") && !string.IsNullOrEmpty(ID_TBPERSONA))
                    {
                        param.Add(asignarParametro("V_CONS_PERSONA", 1, "System.Int32", CONS_PERSONA));
                        param.Add(asignarParametro("V_PER_IDPERSONA", 1, "System.Int32", ID_TBPERSONA));
                        datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CARACTERIZACION.GIC_SP_INGRESO_RUV_PERSONA", ref param);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public List<Persona> consultarGpo_Familiar_x_Cons_Persona(string consPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            IDataReader dataReader = null;
            List<Persona> personas = new List<Persona>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("V_CONS_PERSONA", 1, "System.Int32", consPersona));
                param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET_GPO_FAM_X_CONS_PER", ref param);
                if (dsSalida.Tables.Count > 0)
                {
                    dataReader = dsSalida.Tables[0].CreateDataReader();
                    while (dataReader.Read())
                    {
                        Persona p = new Persona();
                        if (!DBNull.Value.Equals(dataReader["CONS_PERSONA"])) p.CONS_PERSONA = dataReader["CONS_PERSONA"].ToString();
                        if (String.IsNullOrEmpty(p.CONS_PERSONA) || p.CONS_PERSONA.StartsWith("NI"))
                            p.CONS_PERSONA_ESTADO = "NO INCLUIDO";

                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"])) p.ID_PERSONA = dataReader["PER_IDPERSONA"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"])) p.PRIMER_NOMBRE = dataReader["PER_PRIMERNOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"])) p.SEGUNDO_NOMBRE = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"])) p.PRIMER_APELLIDO = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"])) p.SEGUNDO_APELLIDO = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_ESTADO"])) p.ESTADO = dataReader["PER_ESTADO"].ToString();

                        if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) p.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"])) p.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString().ToUpper().Replace("12:00:00 AM", "");
                        p.NOMBRES_COMPLETOS = p.PRIMER_NOMBRE + ' ' + p.SEGUNDO_NOMBRE + ' ' + p.PRIMER_APELLIDO + ' ' + p.PRIMER_APELLIDO;
                        //------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                        // ADICION CAMPO FECHA DE CARACTERIZACION
                        //-----------------------------------------------
                        if (!DBNull.Value.Equals(dataReader["FECHA_CARACTERIZACION"])) p.FECHA_ULT_CARACTERIZACION = dataReader["FECHA_CARACTERIZACION"].ToString();
                        if (!DBNull.Value.Equals(dataReader["HABILITADO_CARAC"])) p.HABILITADO_PARA_CARACTERIZACION = dataReader["HABILITADO_CARAC"].ToString();

                        if (String.IsNullOrEmpty(p.FECHA_ULT_CARACTERIZACION))
                            p.HABILITADO_PARA_CARACTERIZACION = "SI";
                        //FIN MODIFICACION OCT.27.2015

                        //------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ NOV.03.2015
                        //RECUPERA LOS DATOS RUV DE LA PERSONA
                        //-----------------------------------------------
                        if (!DBNull.Value.Equals(dataReader["R_PRIMERNOMBRE"])) p.R_PRIMER_NOMBRE = dataReader["R_PRIMERNOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["R_SEGUNDONOMBRE"])) p.R_SEGUNDO_NOMBRE = dataReader["R_SEGUNDONOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["R_PRIMERAPELLIDO"])) p.R_PRIMER_APELLIDO = dataReader["R_PRIMERAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["R_SEGUNDOAPELLIDO"])) p.R_SEGUNDO_APELLIDO = dataReader["R_SEGUNDOAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["R_NUMERODOC"])) p.R_NUMERO_DOC = dataReader["R_NUMERODOC"].ToString();

                        //FIN MODIFICACION: JOSE VASQUEZ NOV.03.2015

                        personas.Add(p);
                    }
                }
                return personas;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        public List<string> consultarPersonasRUV(string idPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            IDataReader dataReader = null;
            List<string> idRUVs = new List<string>();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET_RUV_PERS_X_IDPERS", ref param);
                if (dsSalida.Tables.Count > 0)
                {
                    dataReader = dsSalida.Tables[0].CreateDataReader();
                    while (dataReader.Read())
                        idRUVs.Add(dataReader["CONS_PERSONA"].ToString());
                }
                return idRUVs;
            }
            finally
            {

                dsSalida.Dispose();
            }
        }

        public Persona consultaDatosPersona(string idPersona)
        {
            Persona p = new Persona();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_IDPERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("p_tipoDoc", 2, "System.String", ""));
                param.Add(asignarParametro("p_numDoc", 2, "System.String", ""));
                param.Add(asignarParametro("p_primerNombre", 2, "System.String", ""));
                param.Add(asignarParametro("p_segundoNombre", 2, "System.String", ""));
                param.Add(asignarParametro("p_primerApellido", 2, "System.String", ""));
                param.Add(asignarParametro("p_segundoApellido", 2, "System.String", ""));
                param.Add(asignarParametro("p_fechaNacimiento", 2, "System.String", ""));
                //------------------------------------------------
                //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                // ADICION CAMPO DOCUMENTO RUV
                //-----------------------------------------------
                param.Add(asignarParametro("p_fechaUltCaracterizacion", 2, "System.String", ""));
                param.Add(asignarParametro("p_habilitadoParaCarac", 2, "System.String", ""));
                param.Add(asignarParametro("PO_IDPERSONA", 2, "System.String", ""));
                param.Add(asignarParametro("p_estadoencuesta", 2, "System.String", ""));
                param.Add(asignarParametro("p_codigoencuesta", 2, "System.String", ""));
                //FIN: JOSE VASQUEZ OCT.27.2015

                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CARACTERIZACION.GIC_SP_GET_DATOS_PERSONA", ref param);

                p.TIPO_DOC = param.Find(x => x.Nombre == "p_tipoDoc").Valor;
                p.NUMERO_DOC = param.Find(x => x.Nombre == "p_numDoc").Valor;
                p.PRIMER_NOMBRE = param.Find(x => x.Nombre == "p_primerNombre").Valor;
                p.SEGUNDO_NOMBRE = param.Find(x => x.Nombre == "p_segundoNombre").Valor;
                p.PRIMER_APELLIDO = param.Find(x => x.Nombre == "p_primerApellido").Valor;
                p.SEGUNDO_APELLIDO = param.Find(x => x.Nombre == "p_segundoApellido").Valor;
                p.FECHA_NACIMIENTO = param.Find(x => x.Nombre == "p_fechaNacimiento").Valor;
                p.NOMBRES_COMPLETOS = p.PRIMER_NOMBRE + ' ' + p.SEGUNDO_NOMBRE + ' ' + p.PRIMER_APELLIDO + ' ' + p.SEGUNDO_APELLIDO;
                //------------------------------------------------
                // MODIFICACION: JOSE VASQUEZ OCT.27.2015
                // ADICION CAMPO DOCUMENTO RUV
                //-----------------------------------------------
                p.FECHA_ULT_CARACTERIZACION = param.Find(x => x.Nombre == "p_fechaUltCaracterizacion").Valor;
                p.HABILITADO_PARA_CARACTERIZACION = param.Find(x => x.Nombre == "p_habilitadoParaCarac").Valor;
                p.COD_HOGAR = param.Find(x => x.Nombre == "p_codigoencuesta").Valor;
                p.ESTADO_ENCUESTA = param.Find(x => x.Nombre == "p_estadoencuesta").Valor;
                // SI NO ENCUENTRA DATOS, ENTOCES ESTA HABILITADO PARA CARACTERIZACION
                if (String.IsNullOrEmpty(p.HABILITADO_PARA_CARACTERIZACION))
                    p.HABILITADO_PARA_CARACTERIZACION = "SI";
                //FIN: JOSE VASQUEZ OCT.27.2015

                //------------------------------------------------
                //MODIFICACION: JOSE VASQUEZ NOV.25.2015
                // ADICION VALIDACION ID_PERSONA
                //-----------------------------------------------
                p.ID_PERSONA = p.FECHA_NACIMIENTO = param.Find(x => x.Nombre == "PO_IDPERSONA").Valor;
                //FIN MODIFICACION: JOSE VASQUEZ NOV.25.2015
                return p;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        public DataSet consultarIdPersonasMi(string IdPersona, string idAplicacion, string idUsuario)
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
                param.Add(asignarParametro("P_IDPERSONA", 1, "System.Int32", IdPersona));
                param.Add(asignarParametro("P_APPLICATIONID", 1, "System.String", idAplicacion));
                param.Add(asignarParametro("P_USERID", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("P_IPADDRESS", 1, "System.String", ObtenerIP_Usuario()));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MSGERROR", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CONSULTAS.MI_PERSONAS_UNICA", ref param);

                return dsSalida;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        /***************************************************************************************************************************************
         * JAIME ANDRES LOBATON
         * *************************************************************************************************************************************/
        public DataSet consultarPersonasModeloINntegrado(string documento, string idUsuario, string idAplicacion)
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
                param.Add(asignarParametro("P_DOCUMENTO", 1, "System.String", documento));
                param.Add(asignarParametro("P_NOMBRE1", 1, "System.String", ""));
                param.Add(asignarParametro("P_NOMBRE2", 1, "System.String", ""));
                param.Add(asignarParametro("P_APELLIDO1", 1, "System.String", ""));
                param.Add(asignarParametro("P_APELLIDO2", 1, "System.String", ""));
                param.Add(asignarParametro("P_APPLICATIONID", 1, "System.String", idAplicacion));
                param.Add(asignarParametro("P_USERID", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("P_IPADDRESS", 1, "System.String", ObtenerIP_Usuario()));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MSGERROR", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CONSULTAS.MI_PERSONAS", ref param);

                return dsSalida;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        public DataSet consultarFichaUnicaFichaCaracterizacion(string documento/*, string idUsuario, string idAplicacion*/)
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
                param.Add(asignarParametro("P_DOCUMENTO", 1, "System.String", documento));
                param.Add(asignarParametro("P_TEMATICA", 1, "System.Int32", "518"));

                param.Add(asignarParametro("P_RESULT", 2, "System.String", ""));
                param.Add(asignarParametro("P_MENSAJE", 2, "System.String", ""));
                param.Add(asignarParametro("P_CURSOR", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("Rni_mi_pru.SP_ENTREVISTA_UNICA", ref param);
                string resultado = param.Find(x => x.Nombre == "P_RESULT").Valor;
                string mensaje = param.Find(x => x.Nombre == "P_MENSAJE").Valor;
                return dsSalida;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        public DataSet consultarGrupoFamiliarNuevo(string idPersonaModelo)
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
                param.Add(asignarParametro("P_ID_PERSONA", 1, "System.Int32", idPersonaModelo));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MENSAJE", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CARACTERIZACION.MI_PERSONAS_GRUPOS", ref param);
                return dsSalida;
            }
            finally
            {

                dsSalida.Dispose();
            }
        }

        public List<Victima> modeloGrupoFamilairMi(DataSet ds)


        {
            
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            

            while (dataReader.Read())
            {

                Victima objVictima = new Victima();
                if (!DBNull.Value.Equals(dataReader["PER_ID"])) objVictima.MI_IDPERSONA = dataReader["PER_ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_ID"])) objVictima.CONS_PERSONA = dataReader["PER_ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) objVictima.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_DOCUMENTO"])) objVictima.DOCUMENTO = dataReader["PER_DOCUMENTO"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_NOMBRE1"])) objVictima.NOMBRE1 = dataReader["PER_NOMBRE1"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_NOMBRE2"])) objVictima.NOMBRE2 = dataReader["PER_NOMBRE2"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_APELLIDO1"])) objVictima.APELLIDO1 = dataReader["PER_APELLIDO1"].ToString();
                if (!DBNull.Value.Equals(dataReader["PER_APELLIDO2"])) objVictima.APELLIDO2 = dataReader["PER_APELLIDO2"].ToString();
                objVictima.NOMBRES_COMPLETOS = objVictima.NOMBRE1 + ' ' + objVictima.NOMBRE2 + ' ' + objVictima.APELLIDO1 + ' ' + objVictima.APELLIDO2;
                if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) objVictima.F_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString().ToUpper().Replace("12:00:00 AM", "");
                if (!DBNull.Value.Equals(dataReader["ID_CARACTERIZACION"])) objVictima.ID_CARACTERIZACION = dataReader["ID_CARACTERIZACION"].ToString();
                try
                {
                    if (!DBNull.Value.Equals(dataReader["PER_SEXO"])) objVictima.GENERO_HOM = dataReader["PER_SEXO"].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                coleccion.Add(objVictima);

            }

            return (coleccion);
        }

        public List<Victima> consultarGrupoFamiliarMI(string idPersonaModelo)
        {
            DataSet dsSalida = null;
            List<Victima> coleccion = null;

            HechosPersona hechos = new HechosPersona();
            dsSalida = consultarGrupoFamiliarNuevo(idPersonaModelo);
            coleccion = modeloGrupoFamilairMi(dsSalida);

            // Entra si encontró victimas registradas en el RUV para el número de documento suministrado
            if (coleccion.Count > 0)
            {
                foreach (Victima item in coleccion)
                {

                    //

                    // Verifica si la victima identificada en RUV ya fue caracterizada en la tabla GIC_RUV_PERSONAS
                    hechos = hechos.hechosVictimizantes(item.MI_IDPERSONA);
                    if (hechos.PER_ID != null)
                        item.TIPO_VICTIMA = "INCLUIDO";
                    else
                        item.TIPO_VICTIMA = "NO INCLUIDO";
                    item.HV1 = hechos.HV1;
                    item.HV2 = hechos.HV2;
                    item.HV3 = hechos.HV3;
                    item.HV4 = hechos.HV4;
                    item.HV5 = hechos.HV5;
                    item.HV6 = hechos.HV6;
                    item.HV7 = hechos.HV7;
                    item.HV8 = hechos.HV8;
                    item.HV9 = hechos.HV9;
                    item.HV10 = hechos.HV10;
                    item.HV11 = hechos.HV11;
                    item.HV12 = hechos.HV12;
                    item.HV13 = hechos.HV13;
                    item.HV14 = hechos.HV14;
                    item.ID_CARACTERIZACION = hechos.ID_CARACTERIZACION;
                    item.ID_TBPERSONA = hechos.ID_CARACTERIZACION;
                    item.FECHA_ULT_CARACTERIZACION = hechos.FECHA_ULTI_ENCUESTA;
                    item.COD_HOGAR = hechos.COD_HOGAR;
                    item.ESTADO_ENCUESTA = hechos.ESTADO_ENCUESTA;
                    if (hechos.HABILITADO_CARAC == null || hechos.HABILITADO_CARAC == "SI")
                        item.HABILITADO_PARA_CARACTERIZACION = "SI";
                    else
                        item.HABILITADO_PARA_CARACTERIZACION = hechos.HABILITADO_CARAC;
                }


            }

            return (coleccion.Distinct().ToList());

        }


        /**************************************************
         * CONSULTA MI DIRECTAMENTE POR DOCUMENTO
         * 
         * **************************************************/
        public DataSet consultarPersonasMiDocumento(string documento, string idAplicacion, string idUsuario)
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
                param.Add(asignarParametro("P_IDPERSONA", 1, "System.Int32", documento));
                param.Add(asignarParametro("P_APPLICATIONID", 1, "System.String", idAplicacion));
                param.Add(asignarParametro("P_USERID", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("P_IPADDRESS", 1, "System.String", ObtenerIP_Usuario()));
                param.Add(asignarParametro("S_CURSOR", 2, "Cursor", ""));
                param.Add(asignarParametro("S_MSGERROR", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("MI_PKG_CONSULTAS.MI_PERSONAS_UNICA", ref param);
                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }


        public Victima consultaDatosRUV(string consPersona)
        {
            
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            IDataReader dataReader = null;
            Victima objVictima = new Victima();


            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
            datos.Conexion = connString;

            param = new List<Parametros>();
            param.Add(asignarParametro("pConsPersona", 1, "System.String", consPersona));
            param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
            param.Add(asignarParametro("p_Salida", 2, "System.Int32", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NUEVOS_INCLUIDOS.PR_GET_INCLUIDO_CONSPERSONA", ref param);

            if (dsSalida.Tables.Count > 0)
            {
                dataReader = dsSalida.Tables[0].CreateDataReader();
                while (dataReader.Read())
                {
                    objVictima = new Victima();

                    if (!DBNull.Value.Equals(dataReader["CONS_PERSONA"])) objVictima.CONS_PERSONA = dataReader["CONS_PERSONA"].ToString();
                    if (!DBNull.Value.Equals(dataReader["TIPO_DOC"])) objVictima.TIPO_DOC = dataReader["TIPO_DOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["DOCUMENTO"])) objVictima.DOCUMENTO = dataReader["DOCUMENTO"].ToString();
                    if (!DBNull.Value.Equals(dataReader["NOMBRE1"])) objVictima.NOMBRE1 = dataReader["NOMBRE1"].ToString();
                    if (!DBNull.Value.Equals(dataReader["NOMBRE2"])) objVictima.NOMBRE2 = dataReader["NOMBRE2"].ToString();
                    if (!DBNull.Value.Equals(dataReader["APELLIDO1"])) objVictima.APELLIDO1 = dataReader["APELLIDO1"].ToString();
                    if (!DBNull.Value.Equals(dataReader["APELLIDO2"])) objVictima.APELLIDO2 = dataReader["APELLIDO2"].ToString();

                    objVictima.NOMBRES_COMPLETOS = objVictima.NOMBRE1 + ' ' + objVictima.NOMBRE2 + ' ' + objVictima.APELLIDO1 + ' ' + objVictima.APELLIDO2;
                    //------------------------------------------------
                    //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                    // ADICION CAMPO DOCUMENTO RUV
                    //-----------------------------------------------
                    objVictima.DOCUMENTO_RUV = objVictima.DOCUMENTO;
                    //FIN MODIFICACION OCT.27.2015

                    if (!DBNull.Value.Equals(dataReader["GENERO_HOM"])) objVictima.GENERO_HOM = dataReader["GENERO_HOM"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PERT_ETNICA"])) objVictima.PERT_ETNICA = dataReader["PERT_ETNICA"].ToString();
                    if (!DBNull.Value.Equals(dataReader["DISCAP"])) objVictima.DISCAP = dataReader["DISCAP"].ToString();
                    if (!DBNull.Value.Equals(dataReader["F_NACIMIENTO"]))
                        objVictima.F_NACIMIENTO = (dataReader["F_NACIMIENTO"].ToString());

                    if (!DBNull.Value.Equals(dataReader["SOBREVIVENCIA"])) objVictima.SOBREVIVENCIA = dataReader["SOBREVIVENCIA"].ToString();
                    if (!DBNull.Value.Equals(dataReader["IDENTIFICADO"])) objVictima.IDENTIFICADO = dataReader["IDENTIFICADO"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV1"])) objVictima.HV1 = dataReader["HV1"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV2"])) objVictima.HV2 = dataReader["HV2"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV3"])) objVictima.HV3 = dataReader["HV3"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV4"])) objVictima.HV4 = dataReader["HV4"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV5"])) objVictima.HV5 = dataReader["HV5"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV6"])) objVictima.HV6 = dataReader["HV6"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV7"])) objVictima.HV7 = dataReader["HV7"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV8"])) objVictima.HV8 = dataReader["HV8"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV9"])) objVictima.HV9 = dataReader["HV9"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV10"])) objVictima.HV10 = dataReader["HV10"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV11"])) objVictima.HV11 = dataReader["HV11"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV12"])) objVictima.HV12 = dataReader["HV12"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV13"])) objVictima.HV13 = dataReader["HV13"].ToString();
                    if (!DBNull.Value.Equals(dataReader["HV14"])) objVictima.HV14 = dataReader["HV14"].ToString();

                    objVictima.TIPO_VICTIMA = "INCLUIDO";
                }
            }

            return objVictima;
        }

        public List<Persona> consultaPersonasCaracterizadas(string numeroDoc)
        {

            List<Persona> personas = new List<Persona>();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                IDataReader dataReader = null;
                datos.MotorBasedatos = true;
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_IDPERSONA", 1, "System.String", numeroDoc));
                param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CATEGORIZACION.GIC_OBTENER_PERSONAS", ref param);

                if (dsSalida.Tables.Count > 0)
                {
                    dataReader = dsSalida.Tables[0].CreateDataReader();
                    while (dataReader.Read())
                    {
                        Persona p = new Persona();
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"])) p.ID_PERSONA = dataReader["PER_IDPERSONA"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"])) p.PRIMER_NOMBRE = dataReader["PER_PRIMERNOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"])) p.SEGUNDO_NOMBRE = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"])) p.PRIMER_APELLIDO = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"])) p.SEGUNDO_APELLIDO = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_ESTADO"])) p.ESTADO = dataReader["PER_ESTADO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) p.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"])) p.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString();
                        p.NOMBRES_COMPLETOS = p.PRIMER_NOMBRE + ' ' + p.SEGUNDO_NOMBRE + ' ' + p.PRIMER_APELLIDO + ' ' + p.PRIMER_APELLIDO;
                        //------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                        // ADICION CAMPO FECHA DE CARACTERIZACION
                        //-----------------------------------------------
                        if (!DBNull.Value.Equals(dataReader["FECHA_CARACTERIZACION"])) p.FECHA_ULT_CARACTERIZACION = dataReader["FECHA_CARACTERIZACION"].ToString();
                        if (!DBNull.Value.Equals(dataReader["HABILITADO_CARAC"])) p.HABILITADO_PARA_CARACTERIZACION = dataReader["HABILITADO_CARAC"].ToString();

                        if (String.IsNullOrEmpty(p.FECHA_ULT_CARACTERIZACION))
                            p.HABILITADO_PARA_CARACTERIZACION = "SI";
                        //FIN MODIFICACION OCT.27.2015
                        personas.Add(p);
                    }
                }
                return personas;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        //07/11/2019 andrés quintero
        public List<Persona> gic_validar_persona_encuestada(string numeroDoc, string idPerfilUsuario)
        {

            List<Persona> personas = new List<Persona>();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = null;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                IDataReader dataReader = null;
                datos.MotorBasedatos = true;
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("P_IDPERSONA", 1, "System.String", numeroDoc));
                param.Add(asignarParametro("idPerfilUsuario", 1, "System.String", idPerfilUsuario));
                param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CATEGORIZACION.GIC_VALIDAR_PERSONA_ENCUESTADA", ref param);

                if (dsSalida.Tables.Count > 0)
                {
                    dataReader = dsSalida.Tables[0].CreateDataReader();
                    while (dataReader.Read())
                    {
                        Persona p = new Persona();
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"])) p.ID_PERSONA = dataReader["PER_IDPERSONA"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"])) p.PRIMER_NOMBRE = dataReader["PER_PRIMERNOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"])) p.SEGUNDO_NOMBRE = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"])) p.PRIMER_APELLIDO = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"])) p.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"])) p.SEGUNDO_APELLIDO = dataReader["PER_SEGUNDOAPELLIDO"].ToString();


                        personas.Add(p);
                    }
                }
                return personas;
            }
            finally
            {

                dsSalida.Dispose();
            }

        }

        public string consecutivoPersonaAleatorio(List<Victima> coleccion)
        {
            Random r = new Random();
            string numAleatorio = "NI" + r.Next(10000, 200000).ToString();
            while (coleccion.Any(x => x.CONS_PERSONA == numAleatorio))
            {
                numAleatorio = "NI" + r.Next(10000, 200000).ToString();
            }

            return (numAleatorio);
        }

        public string consecutivoPersonaAleatorio(List<Victima> coleccion1, List<Victima> coleccion2)
        {
            Random r = new Random();
            string numAleatorio = "NI" + r.Next(10000, 200000).ToString();
            while (coleccion1.Any(x => x.CONS_PERSONA == numAleatorio) || coleccion2.Any(x => x.CONS_PERSONA == numAleatorio))
            {
                numAleatorio = "NI" + r.Next(10000, 200000).ToString();
            }

            return (numAleatorio);
        }


        public List<Victima> RemueveRepetidos(List<Victima> coleccionVic, List<Persona> coleccionPer)
        {


            List<Victima> colVictimasRes = new List<Victima>();


            // Entra si encontró victimas registradas en el RUV para el número de documento suministrado
            if (coleccionPer.Count > 0)
            {
                foreach (Persona per in coleccionPer)
                {
                    if (!coleccionVic.Exists(x => x.DOCUMENTO == per.R_NUMERO_DOC && x.NOMBRE1 == per.R_PRIMER_NOMBRE
                        && x.NOMBRE2 == per.R_SEGUNDO_NOMBRE && x.APELLIDO1 == per.R_PRIMER_APELLIDO && x.APELLIDO2 == per.R_SEGUNDO_APELLIDO))
                    {

                        Victima item = new Victima();
                        item.TIPO_DOC = per.TIPO_DOC;
                        item.DOCUMENTO = per.NUMERO_DOC;
                        item.NOMBRE1 = per.PRIMER_NOMBRE;
                        item.NOMBRE2 = per.SEGUNDO_NOMBRE;
                        item.APELLIDO1 = per.PRIMER_APELLIDO;
                        item.APELLIDO2 = per.SEGUNDO_APELLIDO;
                        item.NOMBRES_COMPLETOS = per.PRIMER_NOMBRE + ' ' + per.SEGUNDO_NOMBRE + ' ' + per.PRIMER_APELLIDO + ' ' + per.SEGUNDO_APELLIDO;
                        item.F_NACIMIENTO = per.FECHA_NACIMIENTO;
                        item.ID_TBPERSONA = per.ID_PERSONA;
                        item.TIPO_VICTIMA = per.ESTADO;
                        //------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                        // ADICION CAMPO FECHA DE CARACTERIZACION
                        //-----------------------------------------------
                        item.FECHA_ULT_CARACTERIZACION = per.FECHA_ULT_CARACTERIZACION;
                        item.HABILITADO_PARA_CARACTERIZACION = per.HABILITADO_PARA_CARACTERIZACION;
                        item.DOCUMENTO_CARACTERIZACION = per.NUMERO_DOC;
                        item.DOCUMENTO_RUV = per.R_NUMERO_DOC;

                        //string numAleatorio = consecutivoPersonaAleatorio(colVictimasRes, coleccionVic);
                        item.CONS_PERSONA = per.CONS_PERSONA;
                        item.CONS_PERSONA_ESTADO = per.CONS_PERSONA_ESTADO;
                        //FIN MODIFICACION OCT.27.2015

                        colVictimasRes.Add(item);
                    }
                }
            }
            return colVictimasRes;

        }

        public int calcularEdad(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;
            return age;
        }
    }
}