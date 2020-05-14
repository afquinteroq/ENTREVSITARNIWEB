using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;
using IgedEncuesta.Models.mdlGenerico;
namespace AdministracionInstrumentos
{
    public class gic_Hogar
    {
        private string usuarioCreacion;

        public string USUARIO_CREACION
        {
            get { return usuarioCreacion; }
            set { usuarioCreacion = value; }
        }

        private int idUsuario;

        public int ID_USUARIO
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        private int idTipoCaracterizacion;

        public int ID_TIPO_CARACTERIZACION
        {
            get { return idTipoCaracterizacion; }
            set { idTipoCaracterizacion = value; }
        }

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        /// Obtiene el id de la persona encuestada
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> int id persona</returns>
        public int get_IdPersonaEncuestada(string codHogar)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            baseDatos = new mdlGenerico();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("pCOD_HOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET_PERSONAENCUESTADA", ref param);
            int idPersona = 0;
            try
            {
               
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        if (!DBNull.Value.Equals(dataReader["PER_IDPERSONA"]))
                        {
                            idPersona = int.Parse(dataReader[0].ToString());
                        }
                    }
                    dataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                datoConsulta.Dispose();
            }
            return idPersona;

        }

        /// <summary>
        /// Agregado Andrés Quintero el 14/05/2019 Consulta el estado de la encuesta en la tabla GIC_N_ESTADO_ENCUESTA
        /// </summary>
        /// <param name="hogCodigo">Codigo del hogar </param>
        /// <param name="usuario">Usuario</param>
        /// <returns>  </returns>
        public int consultarEstadoEncuesta(string hogCodigo, string idusuario, string perfilusuario)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo));
            param.Add(baseDatos.asignarParametro("IDUSUARIO", 1, "System.String", idusuario));
            param.Add(baseDatos.asignarParametro("IDPERFILUSUARIO", 1, "System.String", perfilusuario));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            int totalCT = 0;
            try
            {
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_CONSULTAR_ESTADO_ENCUESTA", ref param);
                
                try
                {
                                       
                    using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                    {
                        while (dataReader.Read())
                        {
                            if (!DBNull.Value.Equals(dataReader["TOTAL"]))
                            {
                                totalCT = int.Parse(dataReader[0].ToString());
                            }
                        }
                        dataReader.Close();
                    }
                }
                catch (Exception ex)
                {   
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("ERROR AL ACTUALIZAR EL ESTADO DE LA ENCUESTA." + ex.Message);
            }
            finally
            {
                Console.WriteLine("");
            }
            return totalCT;
        }

        /// <summary>
        /// Actualiza el estado de la encuesta en la tabla GIC_N_ESTADO_ENCUESTA
        /// </summary>
        /// <param name="hogCodigo">Codigo del hogar </param>
        /// <param name="usuario">Usuario</param>
        /// <returns>  </returns>
        public void actualizarEstadoEncuesta(string hogCodigo, string usuario, string tipoAplazamiento)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo));
            param.Add(baseDatos.asignarParametro("USUARIO", 1, "System.String", usuario));
            param.Add(baseDatos.asignarParametro("TIPO_APLAZAMIENTO", 1, "System.String", tipoAplazamiento));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_ACTUALIZAR_ESTADO_ENCUESTA", ref param);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("ERROR AL ACTUALIZAR EL ESTADO DE LA ENCUESTA." + ex.Message);
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        public int validaMiembrosCodigo(string codHogar)
        {
            int total = 0;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pCODIGO", 1, "System.String", codHogar));
            param.Add(baseDatos.asignarParametro("TOTAL", 2, "System.Int32", ""));
            try
            {
                datos.ConsultarConProcedimientoAlmacenado("GIC_CATEGORIZACION.GIC_VALIDA_MIEMBROS", ref param);

                total = int.Parse(param.Find(x => x.Nombre == "TOTAL").Valor);                
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("");
            }

            return total;
        }

        /// <summary>
        /// Elimina la encuesta de la base de datos
        /// </summary>
        /// <param name="hogCodigo">Codigo del hogar </param>
        /// <returns>  </returns>
        public void eliminarEncuesta(string hogCodigo, string usuario)
        {

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo));
            param.Add(baseDatos.asignarParametro("USUARIO", 1, "System.String", usuario));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_ELIMINAR_ENCUESTA", ref param);
            }
            catch (Exception ex)
            {

                throw new System.ArgumentException("ERROR AL ACTUALIZAR EL ESTADO DE LA ENCUESTA." + ex.Message);
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Devuelve las respuestas por persona
        /// </summary>
        /// <param name="cod_hogar">Codigo del hogar a buscar</param>
        /// <returns> List<gic_RespuestaxPersona> generada </returns>/// 
        public List<gic_MiembroHogar> get_MiembrosHogar(string cod_hogar)
        {
            List<gic_MiembroHogar> miembros = new List<gic_MiembroHogar>();
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            stored = "GIC_CATEGORIZACION.GIC_MIEMBROS_HOGAR";
            param.Add(baseDatos.asignarParametro("COD_HOGAR", 1, "System.String", cod_hogar));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
                
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_MiembroHogar miembroHogar = new gic_MiembroHogar();
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERNOMBRE"]))
                        {
                            miembroHogar.PRIMERNOMBRE = dataReader["PER_PRIMERNOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDONOMBRE"]))
                        {
                            miembroHogar.SEGUNDONOMBRE = dataReader["PER_SEGUNDONOMBRE"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_PRIMERAPELLIDO"]))
                        {
                            miembroHogar.PRIMERAPELLIDO = dataReader["PER_PRIMERAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_SEGUNDOAPELLIDO"]))
                        {
                            miembroHogar.SEGUNDOAPELLIDO = dataReader["PER_SEGUNDOAPELLIDO"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["PER_ESTADO"]))
                        {
                            miembroHogar.ESTADO = dataReader["PER_ESTADO"].ToString();
                        }
                        miembros.Add(miembroHogar);
                    }
                    dataReader.Close();
                }
                return miembros;
            }
            catch
            {
                return null;
            }
            finally
            {
                datoConsulta.Dispose();
            }
        }

        /// <summary>
        /// Devuelve el estado de una encuesta
        /// </summary>
        /// <param name="usuario">Código del hogar</param>
        /// <returns> Devuelve el estado de la encuesta</returns>
        public string get_estadoEncuesta(string hogCodigo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                string estado = "";
                string funcion = "GIC_N_CARACTERIZACION.FN_ESTADO_ENCUESTA";

                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.String", ""));
                param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo.ToString()));
                estado = datos.EjecutarFunciones(funcion, ref param);
                return estado;
                
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        public string generarCodigoFormularioNuevo(string usuario, int idUsuario)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                string codigo = "";
                string funcion = "GIC_N_CARACTERIZACION.GET_CODIGOHOGAR";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("returnvalue", 4, "System.String", ""));
                param.Add(baseDatos.asignarParametro("USUA_CREACION", 1, "System.String", usuario.ToString()));
                param.Add(baseDatos.asignarParametro("ID_USUARIO", 1, "System.String", idUsuario.ToString()));
                codigo = datos.EjecutarFunciones(funcion, ref param);
                return codigo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        
        
        
        
        /// <summary>
        /// Obtiene los miembros del hogar por codigo para un usuario.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> DataTable miembors del hogar</returns>
        public List<gic_ReporteMiembros> get_reporteMiembrosXcodigo(string usuarioCreacion)
        {
            string stored = string.Empty;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
        
            List<gic_ReporteMiembros> coleccion = new List<gic_ReporteMiembros>();
            IDataReader dataReader = null;
            stored = "GIC_N_CARACTERIZACION.SP_REPORTE_MIEMBROSXCODIGO_2";
            param.Add(baseDatos.asignarParametro("pUSUARIO", 1, "System.String", usuarioCreacion));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);            

                try
                {
                    
                    using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                    {
                        while (dataReader.Read())
                        {
                            gic_ReporteMiembros reporte = new gic_ReporteMiembros();
                            if (!DBNull.Value.Equals(dataReader["CODIGO"]))
                            {
                                reporte.codigo = dataReader["CODIGO"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["MIEMBROS DEL HOGAR"]))
                            {
                                reporte.miembrosHogar = dataReader["MIEMBROS DEL HOGAR"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["FECHA DE CREACION"]))
                            {
                                reporte.fechaCreacion = DateTime.Parse(dataReader["FECHA DE CREACION"].ToString());
                            }
                            if (!DBNull.Value.Equals(dataReader["ESTADO"]))
                            {
                                reporte.estado = dataReader["ESTADO"].ToString();
                            }
                            if (!DBNull.Value.Equals(dataReader["SOPORTE"]))
                            {
                                reporte.colilla = dataReader["SOPORTE"].ToString();
                            }
                            coleccion.Add(reporte);
                            
                        }
                        dataReader.Close();
                    }
                    return coleccion;
                }
                catch (Exception)
                {
                    Console.WriteLine("");
                    throw;
                }
            finally
            {
                
                datoConsulta.Dispose();
            }
            
        }

        /// <summary>
        /// Trael el codigo del hogar si el usuario tiene una encuesta en estado 'ACTIVA'
        /// </summary>
        /// <param name="usuario">Usuario logueado en la aplicación</param>
        /// <returns> Devuelve el código del hogar de la encuesta en estado 'ACTIVA'</returns>
        public string encuestaActiva(string usuario)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                string hogCodigo = "";
                string funcion = "GIC_N_CARACTERIZACION.FN_ENCUESTAACTIVA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.String", ""));
                param.Add(baseDatos.asignarParametro("USUARIO", 1, "System.String", usuario.ToString()));
                hogCodigo = datos.EjecutarFunciones(funcion, ref param);
                return hogCodigo;
            }
            catch (Exception)
            {

                return "";
            }
            finally
            {
                Console.WriteLine("");
            }
        }


        /// <summary>
        /// Devuelve el id de una encuesta
        /// </summary>
        /// <param name="usuario">Código del hogar</param>
        /// <returns> Devuelve el id de la encuesta</returns>
        public string get_idEncuesta(string hogCodigo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                string estado = "";
                string funcion = "GIC_N_CARACTERIZACION.FN_ID_ENCUESTA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.String", ""));
                param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo.ToString()));
                estado = datos.EjecutarFunciones(funcion, ref param);
                return estado;
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        public string get_idpersonaXTipoPersona(string idpersona, string hogcodigo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                string idPersonaJefe = "";
                string funcion = "GIC_N_CARACTERIZACION.FN_RETORNA_TIPO_PERSONA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("pIdPersona", 1, "System.Int32", idpersona));
                param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", hogcodigo));                

                idPersonaJefe = datos.EjecutarFunciones(funcion, ref param);
                return idPersonaJefe;
                 
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                Console.WriteLine("");
            }
        }

        public int existeColilla(string codHogar)
        {
            int conteo = 0;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("CODHOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("CONTEO", 2, "System.Int32", ""));
            try
            {
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_CATEGORIZACION.GIC_EXISTE_COLILLA", ref param);
                conteo = int.Parse(param.Find(x => x.Nombre == "CONTEO").Valor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                datoConsulta.Dispose();
            }

            return conteo;
        }

        public int existeConstanciaFirmada(string codHogar)
        {
            int conteo = 0;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("CODHOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("CONTEO", 2, "System.Int32", ""));
            try
            {
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_CATEGORIZACION.GIC_EXISTE_CONSTANCIAFIRMADA", ref param);
                conteo = int.Parse(param.Find(x => x.Nombre == "CONTEO").Valor);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                datoConsulta.Dispose();
            }

            return conteo;
        }

        

        public  string cerrarEncuesta(string hogCodigo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {

                string estado = "";
                string funcion = "GIC_N_CARACTERIZACION.CERRAR_ENCUESTA";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", hogCodigo.ToString()));
                param.Add(baseDatos.asignarParametro("RESULT", 2, "System.String", ""));                
                estado = datos.ActualizarConProcedimientoAlmacenado(funcion, ref param).ToString();
                return estado;                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        ////////////
        /// <summary>
        /// Obtiene los miembros del hogar por codigo para un usuario.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> DataTable miembors del hogar</returns>
        /// 
        
        
    }
    public class gic_ReporteMiembros
    {
        //MIEMBROS DEL HOGAR, FECHA DE CREACION, ESTADO, COLILLA
        public string codigo { get; set; }

        public string miembrosHogar { get; set; }

        public DateTime fechaCreacion { get; set; }

        public string estado { get; set; }

        public string colilla { get; set; }
    }

}