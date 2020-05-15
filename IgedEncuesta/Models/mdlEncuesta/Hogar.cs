using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ObjetosTipos;
using IgedEncuesta.Models.mdlEncuesta;
using System.Reflection;


namespace IgedEncuesta.Models.Hogar
{
    public class Hogar : Models.mdlGenerico.mdlGenerico
    {
        public const string Hecho_Encendido = "1";
        public List<Victima> modeloVictimas(DataSet ds)
        {
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            List<Victima> maestroHogar = new List<Victima>();
            bool cargarVictima = false;
           
            while (dataReader.Read())
            {
                if (maestroHogar == null) cargarVictima = true;
                else cargarVictima = !maestroHogar.Any(x => x.CONS_PERSONA == dataReader["CONS_PERSONA"].ToString());
                
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
                        objVictima.F_NACIMIENTO = dataReader["F_NACIMIENTO"].ToString();

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
                
            }

            return (coleccion);
        }

        public string consultarCodigoHogar(string codigo)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("pCODIGO", 1, "System.String", codigo));
                param.Add(asignarParametro("TOTAL", 2, "System.Int32", ""));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_VALIDA_CODIGO", ref param);
                return param.Find(x => x.Nombre == "TOTAL").Valor;
            }
            finally
            {
                //datos.Dispose();
            }

        }

        public string verificarCodigoMiembros(string codigo)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("pCODIGO", 1, "System.String", codigo));
                param.Add(asignarParametro("TOTAL", 2, "System.Int32", ""));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_VALIDA_MIEMBROS", ref param);
                return param.Find(x => x.Nombre == "TOTAL").Valor;
            }
            finally
            {
                //datos.Dispose();
            }

        }

        public void insertarHogar(string usuario, string idUsuario)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("USUA_CREACION", 1, "System.String", usuario));
                param.Add(asignarParametro("ID_USUARIO", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("ID_TIPO_CARACTERIZACION", 1, "System.Int32", "2"));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_HOGAR", ref param);
            }
            finally
            {
                //datos.Dispose();
            }


        }

        public void actualizarEstadoEncuesta(string codigo, string codEstado, string usuario)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("HOGCODIGO", 1, "System.String", codigo));
                param.Add(asignarParametro("USUARIO", 1, "System.String", usuario));
                param.Add(asignarParametro("TIPO_APLAZAMIENTO", 1, "System.String", codEstado));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_N_CARACTERIZACION.SP_ACTUALIZAR_ESTADO_ENCUESTA", ref param);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public string obtenerIdHogar(string idUsuario)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("IDUSUARIO", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("IDHOGAR", 2, "System.String", ""));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_OBT_ULT_HOGARXUSUARIO", ref param);
                return param.Find(x => x.Nombre == "IDHOGAR").Valor;
            }
            finally
            {
                //datos.Dispose();
            }
        }
        //andrés quintero 17/11/2019
        public string updateArchivosSoportes(string sysguid, string hogarcodigo)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("pHOG_CODIGO", 1, "System.String", hogarcodigo));
                param.Add(asignarParametro("psys_guid", 1, "System.String", sysguid));
                param.Add(asignarParametro("pSalida", 2, "System.String", ""));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_N_CARACTERIZACION.SP_UPDATE_SOPORTES", ref param);
                return param.Find(x => x.Nombre == "pSalida").Valor;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);

            }
            finally
            {

            }
            
        }

        public void insertarPrueba(Victima objVictima)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            datos.Conexion = connStringCar;
            param = new List<Parametros>();
            param.Add(asignarParametro("FNACIMIENTO", 1, "System.DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            param.Add(asignarParametro("T_VICTIMA", 1, "System.String", ""));
            param.Add(asignarParametro("ID_SINIESTRO", 1, "System.Int32", null));
            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_PRUEBA", ref param);

        }

        public string insertarPersona(Victima objVictima, string usuario)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("PNOMBRE", 1, "System.String", (objVictima.NOMBRE1 != null) ? objVictima.NOMBRE1 : ""));
                param.Add(asignarParametro("SNOMBRE", 1, "System.String", (objVictima.NOMBRE2 != null) ? objVictima.NOMBRE2 : ""));
                param.Add(asignarParametro("PAPELLIDO", 1, "System.String", (objVictima.APELLIDO1 != null) ? objVictima.APELLIDO1 : ""));
                param.Add(asignarParametro("SAPELLIDO", 1, "System.String", (objVictima.APELLIDO2 != null) ? objVictima.APELLIDO2 : ""));                
                param.Add(asignarParametro("FNACIMIENTO", 1, "System.DateTime", objVictima.F_NACIMIENTO));
                param.Add(asignarParametro("TDOC", 1, "System.String", (objVictima.TIPO_DOC != null) ? objVictima.TIPO_DOC : ""));
                param.Add(asignarParametro("USUARIO", 1, "System.String", usuario));
                param.Add(asignarParametro("USU_FCREACION", 1, "System.DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                param.Add(asignarParametro("NDOCU", 1, "System.String", (objVictima.DOCUMENTO != null) ? objVictima.DOCUMENTO : ""));
                param.Add(asignarParametro("RELAC", 1, "System.String", ""));
                param.Add(asignarParametro("ID_DECLAR", 1, "System.Int32", null));
                param.Add(asignarParametro("ID_PERS_FUENTE", 1, "System.Int32", (objVictima.TIPO_VICTIMA != "NO INCLUIDO") ? objVictima.CONS_PERSONA : null));
                param.Add(asignarParametro("T_VICTIMA", 1, "System.String", ""));
                param.Add(asignarParametro("ID_SINIESTRO", 1, "System.Int32", null));
                param.Add(asignarParametro("FUENTEE", 1, "System.String", ""));
                param.Add(asignarParametro("ESTADO", 1, "System.String", (objVictima.TIPO_VICTIMA != null) ? objVictima.TIPO_VICTIMA : ""));
                param.Add(asignarParametro("IDPERMI", 1, "System.Int32", objVictima.MI_IDPERSONA));
                param.Add(asignarParametro("VALSECUENCIA", 2, "System.Int32", null));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_PERSONAS", ref param);
                return param.Find(x => x.Nombre == "VALSECUENCIA").Valor;
            }
            
            finally
            {
                //datos.Dispose();
            }
        }


        public void insertarMiembrosPorHogar(string idHogar, string idPersona, string jefeHogar, string usuario, string idUsuario)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("IDHOGAR", 1, "System.String", idHogar));
                param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("USUARIO", 1, "System.String", usuario));
                param.Add(asignarParametro("ID_USUARIO", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("ENCUESTADA", 1, "System.String", jefeHogar));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_MIEMBRO_HOGAR", ref param);
            }            
            finally
            {
                //datos.Dispose();
            }


        }

        public void insertarValidadorPorEstado(string idPersona, string idHogar, string estado, string tipopersona, string tipoperfil, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            try
            {
                datos.Conexion = connStringCar;
                param = new List<Parametros>();
                param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("CODHOGAR", 1, "System.String", idHogar));
                param.Add(asignarParametro("VALIDADOR", 1, "System.String", estado));
                param.Add(asignarParametro("VALIDADOR_TIPOPERSONA", 1, "System.String", tipopersona));
                param.Add(asignarParametro("VALIDADOR_TIPOPERFIL", 1, "System.String", tipoperfil));
                param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_HOGAR", ref param);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public void insertarValidadorPorParentesco(string idPersona, string codHogar, string validador, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            datos.Conexion = connStringCar;
            try
            {
                param = new List<Parametros>();
                param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("CODHOGAR", 1, "System.String", codHogar));
                param.Add(asignarParametro("VALIDADOR", 1, "System.String", validador));
                param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_PARENT", ref param);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public void insertarValidadorTipoPersona(string idPersona, string codHogar, string validador, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            datos.Conexion = connStringCar;
            try
            {
                param = new List<Parametros>();
                param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("CODHOGAR", 1, "System.String", codHogar));
                param.Add(asignarParametro("VALIDADOR", 1, "System.Int32", validador));
                param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_TIPOPER", ref param);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public void insertarValidadorTiPerfil(string idPersona, string codHogar, string validador, string idInstrumento)
        {
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            datos.Conexion = connStringCar;
            try
            {
                param = new List<Parametros>();
                param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
                param.Add(asignarParametro("CODHOGAR", 1, "System.String", codHogar));
                param.Add(asignarParametro("VALIDADOR", 1, "System.Int32", validador));
                param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", idInstrumento));
                datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_PERFIL", ref param);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        public void insertarHechosVictima(string idPersona, Victima vic, string codHogar)
        {
            /*  MODIFICACION : JOSE VASQUEZ FECHA:05.NOV.2015
             *  DESCRIPCION: SE HOMOLOGA E INSERTA LOS HECHOS VICTIMIZANTES
             *  1	Acto terrorista / Atentados / Combates / Enfrentamientos / Hostigamientos
                2	Amenaza
                3	Delitos contra la libertad y la integridad sexual en desarrollo del conflicto armado
                4	Desaparición forzada
                5	Desplazamiento forzado
                6	Homicidio
                7	Minas Antipersonal, Munición sin Explotar y Artefacto Explosivo improvisado
                8	Secuestro
                9	Tortura
                10	Vinculación de Niños Niñas y Adolescentes a Actividades Relacionadas con grupos armados
                11	Abandono o Despojo Forzado de Tierras
                12	Perdida de Bienes Muebles o Inmuebles
                13	Otros
                14	Sin informacion
             */
            List<Parametros> param = new List<Parametros>();
            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
            datos.Conexion = connStringCar;
            try
            {
                //Verifica el numero de hechos
                Type type = vic.GetType();


                //Recorre los hechos, cuando el hecho esta encendido en 1 se envia este hecho a la BD
                for (int i = 1; i <= 14; i++)
                {
                    param = new List<Parametros>();
                    param.Add(asignarParametro("IDPERSONA", 1, "System.Int32", idPersona));
                    param.Add(asignarParametro("CODHOGAR", 1, "System.String", codHogar));
                    

                    var HVValue =
                        vic.GetType()
                                   .GetProperty("HV" + i.ToString(),
                                      BindingFlags.FlattenHierarchy |
                                      BindingFlags.Instance |
                                      BindingFlags.Public)
                                   .GetValue(vic);
                    if (HVValue != null)
                    {
                        if (HVValue.ToString() == Hecho_Encendido)
                        {
                            var HVValuefechap = vic.GetType().GetProperty("FECHA_HECHO" + i.ToString(),
                                      BindingFlags.FlattenHierarchy |
                                      BindingFlags.Instance |
                                      BindingFlags.Public)
                                   .GetValue(vic);

                            if (HVValuefechap == null) {
                                HVValuefechap = "";
                            }

                            param.Add(asignarParametro("ID_HECHO", 1, "System.Int32", i.ToString()));                            
                            param.Add(asignarParametro("IDINSTRUMENTO", 1, "System.Int32", System.Configuration.ConfigurationManager.AppSettings["IdInstrumento"].ToString()));
                            param.Add(asignarParametro("FECHA_HECHO", 1, "System.String", HVValuefechap.ToString().Trim()));
                            datos.ConsultarConProcedimientoAlmacenadoValores("GIC_CATEGORIZACION.GIC_INSERT_VALIDADOR_HECHO_AUX", ref param);
                        }
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //datos.Dispose();
            }
          

        }
    }
}