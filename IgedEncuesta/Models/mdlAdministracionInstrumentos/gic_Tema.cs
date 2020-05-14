using IgedEncuesta.Models.mdlGenerico;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_Tema : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Identificador unico del tema
        /// </summary>
        public int tem_IdTema { get; set; }

        /// <summary>
        /// Nombre tema
        /// </summary>
        public string tem_NombreTema { get; set; }

        /// <summary>
        /// Estado tema
        /// </summary>
        public string tem_Activo { get; set; }

        public int tem_Orden { get; set; }

        /// <summary>
        /// Observacion tema
        /// </summary>
        public string tem_Observacion { get; set; }

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        /// Verifica si el tema ya fue finalizado.
        /// </summary>
        /// <param name="idTema">Id del tema a buscar</param>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> int si devuelve 0 no esta finalizado, si devuelve 1 ya se finalizó</returns>
        public int get_TemaFinalizado(int idTema, string codHogar)
        {
            int finalizado = 0;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("pCOD_HOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("pIDTEMA", 1, "System.Int32", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_TEMAFINALIZADO", ref param);
            try
            {

                #region DataReader
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        if (!DBNull.Value.Equals(dataReader["TOTAL"]))
                        {
                            finalizado = int.Parse(dataReader[0].ToString());
                        }
                    }
                    dataReader.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                
                datoConsulta.Dispose();
            }
            return finalizado;
        }

        /// <summary>
        /// Elimina la finalización del tema.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a finalizar</param>
        /// <param name="idTema">Id del tema a finalizar</param>
        /// <param name="usuario">nombre del usuario que finaliza el tema</param>
        public void eliminarFinalizarCapitulo(string codHogar, int idTema, string usuario)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pcod_hogar", 1, "System.String",codHogar));
            param.Add(baseDatos.asignarParametro("pidTema", 1, "System.String", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("pUsuario", 1, "System.String", usuario));
            try
            {
            datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_ELIMINARFINALIZARCAPITULO", ref param);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("ERROR AL ELIMINAR LA FINALIZACION DEL CAPITULO." + ex.Message);
            }
            finally
            {
                //datos.Dispose();
            }
        }

        /// <summary>
        /// Realiza la inserción para la finalización del tema.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a finalizar</param>
        /// <param name="idTema">Id del tema a finalizar</param>
        /// <param name="usuario">nombre del usuario que finaliza el tema</param>
        public void finalizarCapitulo(string codHogar, int idTema, string usuario)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pcodHogar", 1, "System.String", codHogar));
            param.Add(baseDatos.asignarParametro("pidTema", 1, "System.String", idTema.ToString()));
            param.Add(baseDatos.asignarParametro("pusuario", 1, "System.String", usuario));
           
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_FINALIZARCAPITULO", ref param);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);
            }
            finally
            {
                //datos.Dispose();

            }
        }

        /// <summary>
        /// Verifica si ya se llenaron los tres primeros capitulos.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> int si devuelve 0 no estan llenos, si devuelve 1 ya están</returns>
        public int get_VerficarCapitulosPrimeros(string codHogar)
        {
            int finalizado = 0;
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("pCOD_HOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
           
            try
            {
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_GET_HABILITARCAPITULOS", ref param);
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        if (!DBNull.Value.Equals(dataReader["TOTAL"]))
                        {
                            finalizado = int.Parse(dataReader[0].ToString());
                        }
                    }
                    dataReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally{
                
                datoConsulta.Dispose();
            }
            return finalizado;
        }

        /// <summary>
        /// Trae los temas por instrumento
        /// </summary>
        /// <param name="idInstr">Id del instrumento asociado al tema</param>
        /// <param name="tipo">tipo de instrumento, si es para consulta o edicion</param>
        /// <returns> Entidad List<gic_Tema></returns>
        public List<gic_Tema> getTemasxInstrumento(int idInstr, int tipo, string idPerfil)
        {
            string stored = "";
            if (tipo == 1)
            {
                stored = "GIC_CARACTERIZACION.GIC_SP_GET_TEMASXINS";
            }
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            List<gic_Tema> temas = new List<gic_Tema>();
            param.Add(baseDatos.asignarParametro("ID", 1, "System.String", idInstr.ToString()));
            param.Add(baseDatos.asignarParametro("IDPERFIL", 1, "System.Int32", idPerfil));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
               
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_Tema tema = new gic_Tema();
                        if (!DBNull.Value.Equals(dataReader["TEM_IDTEMA"]))
                        {
                            tema.tem_IdTema = int.Parse(dataReader["TEM_IDTEMA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_NOMBRETEMA"]))
                        {
                            tema.tem_NombreTema = dataReader["TEM_NOMBRETEMA"].ToString();
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_ACTIVO"]))
                        {
                            tema.tem_Activo = dataReader["TEM_ACTIVO"].ToString(); ;
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_ORDEN"]))
                        {
                            tema.tem_Orden = int.Parse(dataReader["TEM_ORDEN"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["TEM_OBSERVACION"]))
                        {
                            tema.tem_Observacion = dataReader["TEM_OBSERVACION"].ToString(); ;
                        }
                        temas.Add(tema);
                    }
                    dataReader.Close();
                  
                }
                return temas;
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
        /// Devuelve una lista con los temas que se han finalizados, incompletos y no diligenciados.
        /// </summary>
        /// <param name="codHogar">Codigo del hogar a buscar</param>
        /// <returns> Lista <gic_Tema> generados </returns>
        public List<gic_Tema> getTemasValidados(string codHogar)
        {
            string stored = "";
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            List<gic_Tema> temas = new List<gic_Tema>();
            param.Add(baseDatos.asignarParametro("pCOD_HOGAR", 1, "System.String", codHogar.ToString()));
            param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
            stored = "GIC_N_CARACTERIZACION.SP_VALIDARTEMASINGRESO";
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado(stored, ref param);
            try
            {
               
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        gic_Tema tema = new gic_Tema();
                        if (!DBNull.Value.Equals(dataReader["TEM_IDTEMA"]))
                        {
                            tema.tem_IdTema = int.Parse(dataReader["TEM_IDTEMA"].ToString());
                        }
                        if (!DBNull.Value.Equals(dataReader["VAL"]))
                        {
                            tema.tem_NombreTema = dataReader["VAL"].ToString();
                        }
                        temas.Add(tema);
                    }
                    dataReader.Close();
                }
                return temas;
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

        public int numeroCapitulosTerminados(string hogCodigo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                int numero = 0;
                string funcion = "GIC_N_CARACTERIZACION.FN_NUMERO_CAPITULOS_TER";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("TOTCAPITULOS", 4, "System.Int32", ""));
                param.Add(baseDatos.asignarParametro("HOGCODIGO", 1, "System.String", hogCodigo.ToString()));
                
                numero = int.Parse(datos.EjecutarFunciones(funcion, ref param));
                return numero;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                ////datos.Dispose();
            }
        }

    }
}