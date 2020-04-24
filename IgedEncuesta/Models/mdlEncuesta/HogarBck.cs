using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ObjetosTipos;
using System.Configuration;
using IgedEncuesta.Models.mdlEncuesta;

namespace IgedEncuesta.Models.Hogar 
{
    public class Hogar : Models.mdlGenerico.mdlGenerico
    {
        public List<Victima> modeloVictimas(DataSet ds)
        {
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            List<Victima> maestroHogar = new List<Victima>();
            bool cargarVictima = false;

            maestroHogar = (List<Victima>)HttpContext.Current.Session["ModeloHogar"];
            while (dataReader.Read())
            {
                if (maestroHogar == null) cargarVictima = true;
                else cargarVictima = !maestroHogar.Any(x => x.CONS_PERSONA == dataReader["CONS_PERSONA"].ToString());

                //if (!DBNull.Value.Equals(dataReader["CONS_PERSONA"]))
                // {
                if (cargarVictima)
                {
                    Victima objVictima = new Victima();



                    if (!DBNull.Value.Equals(dataReader["CONS_PERSONA"])) objVictima.CONS_PERSONA = dataReader["CONS_PERSONA"].ToString();
                    if (!DBNull.Value.Equals(dataReader["TIPO_DOC"])) objVictima.TIPO_DOC = dataReader["TIPO_DOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["DOCUMENTO"])) objVictima.DOCUMENTO = dataReader["DOCUMENTO"].ToString();
                    if (!DBNull.Value.Equals(dataReader["NOMBRE1"])) objVictima.NOMBRE1 = dataReader["NOMBRE1"].ToString();
                    if (!DBNull.Value.Equals(dataReader["NOMBRE2"])) objVictima.NOMBRE2 = dataReader["NOMBRE2"].ToString();
                    if (!DBNull.Value.Equals(dataReader["APELLIDO1"])) objVictima.APELLIDO1 = dataReader["APELLIDO1"].ToString();
                    if (!DBNull.Value.Equals(dataReader["APELLIDO2"])) objVictima.APELLIDO2 = dataReader["APELLIDO2"].ToString();

                    objVictima.NOMBRES_COMPLETOS = objVictima.NOMBRE1 + ' ' + objVictima.NOMBRE2 + ' ' + objVictima.APELLIDO1 + ' ' + objVictima.APELLIDO2;

                    if (!DBNull.Value.Equals(dataReader["GENERO_HOM"])) objVictima.GENERO_HOM = dataReader["GENERO_HOM"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PERT_ETNICA"])) objVictima.PERT_ETNICA = dataReader["PERT_ETNICA"].ToString();
                    if (!DBNull.Value.Equals(dataReader["DISCAP"])) objVictima.DISCAP = dataReader["DISCAP"].ToString();
                    if (!DBNull.Value.Equals(dataReader["F_NACIMIENTO"]))
                        objVictima.F_NACIMIENTO = Convert.ToDateTime(dataReader["F_NACIMIENTO"].ToString()).ToString("dd/MM/yyyy");

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
                    objVictima.TIPO_VICTIMA = "INCLUIDA";

                    coleccion.Add(objVictima);
                }
                //    }
            }

            return (coleccion);
        }

        public string consultarCodigoHogar(string codigo)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("pCODIGO", 1, "System.String", codigo));
            param.Add(asignarParametro("TOTAL", 2, "System.Int32", ""));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_VALIDA_CODIGO", ref param);

            return param.Find(x => x.Nombre == "TOTAL").Valor; 
        }

        public string verificarCodigoMiembros(string codigo)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("pCODIGO", 1, "System.String", codigo));
            param.Add(asignarParametro("TOTAL", 2, "System.Int32", ""));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_VALIDA_MIEMBROS", ref param);

            return param.Find(x => x.Nombre == "TOTAL").Valor;
        }

        public void insertarHogar()
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionCaracterizacion"].ConnectionString;
            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("USUA_CREACION", 1, "System.String", HttpContext.Current.Session["Usuario"].ToString()));
            param.Add(asignarParametro("ID_USUARIO", 1, "System.Int32", HttpContext.Current.Session["UserIdApp"].ToString()));
            param.Add(asignarParametro("ID_TIPO_CARACTERIZACION", 1, "System.Int32", "2"));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_HOGAR", ref param);

        }

        public void actualizarEstadoEncuesta(string codigo, string codEstado)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("HOGCODIGO", 1, "System.String", codigo));
            param.Add(asignarParametro("USUARIO", 1, "System.String", HttpContext.Current.Session["Usuario"].ToString()));
            param.Add(asignarParametro("TIPO_APLAZAMIENTO", 1, "System.String", codEstado));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_N_CARACTERIZACION.SP_ACTUALIZAR_ESTADO_ENCUESTA", ref param);

        }

        public string obtenerIdHogar()
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("IDUSUARIO", 1, "System.Int32", HttpContext.Current.Session["UserIdApp"].ToString()));
            param.Add(asignarParametro("IDHOGAR", 2, "System.String", ""));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_OBT_ULT_HOGARXUSUARIO", ref param);

            return param.Find(x => x.Nombre == "IDHOGAR").Valor; 

        }

        public string insertarPersona(Victima objVictima)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("PNOMBRE", 1, "System.String", (objVictima.NOMBRE1 != null) ? objVictima.NOMBRE1 : ""));
            param.Add(asignarParametro("SNOMBRE", 1, "System.String", (objVictima.NOMBRE2 != null) ? objVictima.NOMBRE2 : ""));
            param.Add(asignarParametro("PAPELLIDO", 1, "System.String", (objVictima.APELLIDO1 != null) ? objVictima.APELLIDO1 : ""));
            param.Add(asignarParametro("SAPELLIDO", 1, "System.String", (objVictima.APELLIDO2 != null) ? objVictima.APELLIDO2 : ""));
            param.Add(asignarParametro("FNACIMIENTO", 1, "System.DateTime", objVictima.F_NACIMIENTO));
            param.Add(asignarParametro("TDOC", 1, "System.String", objVictima.TIPO_DOC));
            param.Add(asignarParametro("USUARIO", 1, "System.String", HttpContext.Current.Session["Usuario"].ToString()));
            param.Add(asignarParametro("USU_FCREACION", 1, "System.DateTime", DateTime.Now.ToString()));
            //param.Add(asignarParametro("USU_FCREACION", 1, "System.DateTime", "15/10/2015"));
            param.Add(asignarParametro("NDOCU", 1, "System.String", objVictima.DOCUMENTO));
            param.Add(asignarParametro("RELAC", 1, "System.String", ""));
            param.Add(asignarParametro("ID_DECLAR", 1, "System.Int32", null));
            param.Add(asignarParametro("ID_PERS_FUENTE", 1, "System.Int32", (objVictima.TIPO_VICTIMA != "NO INCLUIDO") ? objVictima.CONS_PERSONA : null));
            param.Add(asignarParametro("T_VICTIMA", 1, "System.String", ""));
            param.Add(asignarParametro("ID_SINIESTRO", 1, "System.Int32", null));
            param.Add(asignarParametro("FUENTEE", 1, "System.String", ""));
            param.Add(asignarParametro("ESTADO", 1, "System.String", objVictima.TIPO_VICTIMA));
            param.Add(asignarParametro("VALSECUENCIA", 2, "System.Int32", null));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_PERSONAS", ref param);

            return param.Find(x => x.Nombre == "VALSECUENCIA").Valor;

        }

        public void insertarMiembrosPorHogar(string idHogar, string idPersona, string jefeHogar)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("IDHOGAR", 1, "System.String", idHogar));
            param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", idPersona));
            param.Add(asignarParametro("USUARIO", 1, "System.String", HttpContext.Current.Session["Usuario"].ToString()));
            param.Add(asignarParametro("ID_USUARIO", 1, "System.Int32", HttpContext.Current.Session["UserIdApp"].ToString()));
            param.Add(asignarParametro("ENCUESTADA", 1, "System.String", jefeHogar));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_MIEMBRO_HOGAR", ref param);

        }

        public void insertarValidadorPorEstado(string idPersona, string idHogar, string estado, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
            param.Add(asignarParametro("CODHOGAR", 1, "System.String", idHogar));
            param.Add(asignarParametro("VALIDADOR", 1, "System.String", estado));
            param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_HOGAR", ref param);

        }

        public void insertarValidadorPorParentesco(string idPersona, string codHogar, string validador, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();

            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
            param.Add(asignarParametro("CODHOGAR", 1, "System.String", codHogar));
            param.Add(asignarParametro("VALIDADOR", 1, "System.String", validador));
            param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_HOGAR", ref param);

        }
    }
}