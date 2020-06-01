using System;
using System.Collections.Generic;
using IgedEncuesta.Models.mdlGenerico;
using System.Data;
using ObjetosTipos;


namespace AdministracionInstrumentos
{
    public class Encuesta
    {

        mdlGenerico baseDatos = new mdlGenerico();

        /// <summary>
        ///  Obtiene el campo a actualizar o a insertar 
        /// </summary>
        /// <param name="idUsuario">IdUsuario con el cuals e trae el registro </param>
        /// <param name="campo"> Campo el cual se va a traer</param>
        /// <returns>Valor del campo a devolver</returns>
        public string get_CampoSesion(string idUsuario, string campo)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            baseDatos = new mdlGenerico();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = null;
            List<Parametros> param = new List<Parametros>();
            IDataReader dataReader = null;
            param.Add(baseDatos.asignarParametro("p_IdUsuario", 1, "System.Int32", idUsuario));
            param.Add(baseDatos.asignarParametro("p_Campo", 1, "System.String", campo));
            param.Add(baseDatos.asignarParametro("CURSOR_OUT", 2, "Cursor", ""));
            datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_SESIONES.GIC_OBTENER_VARIABLES", ref param);
            string campoDevuelto = string.Empty;
            try
            {
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        if (!DBNull.Value.Equals(dataReader[campo]))
                        {
                            campoDevuelto = dataReader[0].ToString();
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
            return campoDevuelto;

        }

        public string getValorCampoSesion(string campoSesion,string idUsuario)
        {
            AdmonSesion.AdmonSesion datos = new AdmonSesion.AdmonSesion();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            string campoDevuelto = string.Empty;
            try
            {
                campoDevuelto= datos.leerSesion("GIC_VARIABLE_SESION", campoSesion, idUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
               
            }
            return campoDevuelto;

        }


        /// <summary>
        ///  realiza la operacion de insercion o actualizacion para la tabla de sesiones
        /// </summary>
        /// <param name="tipoGuardado">1 si es insercion; 2 si es actualizacion</param>
        /// <param name="idUsuario">usuario al cual se le van aregistrar los campos</param>
        /// <param name="campo"> campo al cual se le va a realizar la operacion</param>
        /// <param name="valor">valor que se va actualizar</param>
        /// <param name="tipoDato">si el valor es numerico es 2;cadena es 1 </param>
        /// <returns></returns>
        public string guardarCampoSesion(int tipoGuardado, int idUsuario, string campo, string valor, string tipoDato)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            string codigo = "";
            try
            {

                string funcion = "GIC_SESIONES.GIC_GUARDAR_VARIABLES";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("p_TipoGuardado", 1, "System.Int32", tipoGuardado.ToString()));
                param.Add(baseDatos.asignarParametro("p_IdUsuario", 1, "System.Int32", idUsuario.ToString()));
                param.Add(baseDatos.asignarParametro("p_Campo", 1, "System.String", campo));
                param.Add(baseDatos.asignarParametro("p_Valor", 1, "System.String", valor));
                param.Add(baseDatos.asignarParametro("p_Tipo", 1, "System.Int32", tipoDato));
                param.Add(baseDatos.asignarParametro("p_Salida", 2, "System.Int32", ""));
                datos.InsertarConProcedimientoAlmacenado(funcion, ref param);
                 codigo = param.Find(x => x.Nombre == "p_Salida").Valor;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                ////datos.Dispose();
            }

            return codigo;
        }

        /// <summary>
        ///  realiza la operacion de insercion o actualizacion para la tabla de sesiones CAMPOS CLOB
        /// </summary>
        /// <param name="tipoGuardado">1 si es insercion; 2 si es actualizacion</param>
        /// <param name="idUsuario">usuario al cual se le van aregistrar los campos</param>
        /// <param name="campo"> campo al cual se le va a realizar la operacion</param>
        /// <param name="valor">valor que se va actualizar</param>
        /// <param name="tipoDato">si el valor es numerico es 2;cadena es 1 </param>
        /// <returns></returns>
        public bool guardarCampoSesion(int idUsuario, string campo, string valor)
        {
            AdmonSesion.AdmonSesion datos = new AdmonSesion.AdmonSesion();
            bool respuesta = false;
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            try
            {
             respuesta = datos.actualizarSesion("GIC_VARIABLE_SESION", campo, idUsuario.ToString(), valor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                ////datos.Dispose();
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina la sesion de un usuario 
        /// </summary>
        /// <param name="idUsuario"></param>
        public void eliminarSesionIdUsuario(string idUsuario)
        {
            AdmonSesion.AdmonSesion datos = new AdmonSesion.AdmonSesion();
            try
            {
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                datos.eliminarSesion("GIC_VARIABLE_SESION",idUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                ////datos.Dispose();
            }

        }

        public void insertarVariablesSesion(string variables, string idUsuario)
        {
            AdmonSesion.AdmonSesion datos = new AdmonSesion.AdmonSesion();
            try
            {
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                datos.insertarSesion("GIC_VARIABLE_SESION", idUsuario, variables);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                ////datos.Dispose();
            }

        }

        public string obtenerFechaUltimaTRansaccion(string idUsuario)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            string fecha = "";
            try
            {

                string funcion = "FECHA_SESION";
                datos.Conexion = baseDatos.connStringCar;
                datos.MotorBasedatos = true;
                List<Parametros> param = new List<Parametros>();
                param.Add(baseDatos.asignarParametro("RESULT", 4, "System.String", ""));
                param.Add(baseDatos.asignarParametro("p_IdUsuario", 1, "System.Int32", idUsuario.ToString()));
                fecha = datos.EjecutarFunciones(funcion, ref param);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return fecha;
            }
            finally
            {
                ////datos.Dispose();
            }

            return fecha;
        }

    }
}
