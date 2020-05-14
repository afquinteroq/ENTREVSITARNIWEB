
using IgedEncuesta.Models.mdlGenerico;
using ObjetosTipos;
using System;
using System.Collections.Generic;
using System.Data;
namespace AdministracionInstrumentos
{
    public class gic_ArchivoColilla : gic_AdministradorDeCambios
    {
        public string idtemporal { get; set; }
        public string tipopersona { get; set; }
        /// <summary>
        /// Identificador unico del archivo
        /// </summary>
        public int arc_id { get; set; }

        /// <summary>
        /// Codigo del hogar
        /// </summary>
        public string hog_codigo { get; set; }

        /// <summary>
        /// Url del archivo
        /// </summary>
        public string arc_url { get; set; }


        /// <summary>
        /// Estado Instrumento
        /// </summary>
        mdlGenerico baseDatos = new mdlGenerico(); 

        /// <summary>
        /// Realiza la inserción del detalle de la colilla cargada.
        /// </summary>
        /// <param name="archivo">gic_ArchivoColilla detalle de la colilla a insertar</param>
        public void insertaArchivoColilla(gic_ArchivoColilla archivo)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", archivo.hog_codigo));
            param.Add(baseDatos.asignarParametro("pARC_URL", 1, "System.String", archivo.arc_url));
            param.Add(baseDatos.asignarParametro("pUSU_CREACION", 1, "System.String", archivo.usu_UsuarioCreacion));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_INSERTA_ARCHIVO", ref param);
            }
            catch (Exception ex)
            {

                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);
            }
            finally
            {
                //////datos.Dispose();
            }
        }

        public void insertaConstanciaFirmada(gic_ArchivoColilla archivo)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", archivo.hog_codigo));
            param.Add(baseDatos.asignarParametro("pARC_URL", 1, "System.String", archivo.arc_url));
            param.Add(baseDatos.asignarParametro("pUSU_CREACION", 1, "System.String", archivo.usu_UsuarioCreacion));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_INSERTA_CONSTA_FIRMADA_SAAH", ref param);
            }
            catch (Exception ex)
            {

                throw new System.ArgumentException("ERROR AL ALMACENAR INFORMACION." + ex.Message);
            }
            finally
            {
               
            }
        }

        public String insertaArchivoSoportes(gic_ArchivoColilla archivo,string guid)
        {
            
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.Conexion = baseDatos.connStringCar;
            baseDatos = new mdlGenerico();
            datos.MotorBasedatos = true;
            List<Parametros> param = new List<Parametros>();
            param.Add(baseDatos.asignarParametro("pHOG_CODIGO", 1, "System.String", archivo.hog_codigo));
            param.Add(baseDatos.asignarParametro("pGuid", 1, "System.String", guid));            
            param.Add(baseDatos.asignarParametro("pARC_URL", 1, "System.String", archivo.arc_url));
            param.Add(baseDatos.asignarParametro("pUSU_CREACION", 1, "System.String", archivo.usu_UsuarioCreacion));
            param.Add(baseDatos.asignarParametro("pTipopersona", 1, "System.String", archivo.tipopersona));            
            param.Add(baseDatos.asignarParametro("pSalida", 2, "System.String", ""));
            try
            {
                datos.InsertarConProcedimientoAlmacenado("GIC_N_CARACTERIZACION.SP_INSERTA_SOPORTES", ref param);
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

    }

    public class gic_adminconfig
    {
        /// <summary>
        /// identificador unico
        /// </summary>
        public int ADMINCFG_ID { get; set; }
        /// <summary>
        /// Valor a buscar
        /// </summary>
        public string ADMINCFG_NAME { get; set; }
        /// <summary>
        /// valor a retornar
        /// </summary>
        public string ADMINCFG_VALUE { get; set; }
        /// <summary>
        /// Determina si lo que se encuentra en ADMINCFG_NAME es numerico o estring
        /// </summary>
        public string ADMINCFG_TYPE { get; set; }
        /// <summary>
        /// estado
        /// </summary>
        public int ADMINCFG_STATE { get; set; }

        mdlGenerico baseDatos = new mdlGenerico();




        public List<gic_adminconfig> GetAdminConfiguracion(string Admincfg_name)
        {
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            baseDatos = new mdlGenerico();
            datos.Conexion = baseDatos.connStringCar;
            datos.MotorBasedatos = true;
            DataSet datoConsulta = new DataSet();
            try
            {
                gic_adminconfig _Entidad = null;
                List<gic_adminconfig> ListaEntidad = new List<gic_adminconfig>();
                List<Parametros> param = new List<Parametros>();
                IDataReader dataReader = null;
                param.Add(baseDatos.asignarParametro("pADMINCFG_NAME", 1, "System.String", Admincfg_name.ToString()));
                param.Add(baseDatos.asignarParametro("cur_OUT", 2, "Cursor", ""));
                datoConsulta = datos.ConsultarConProcedimientoAlmacenado("GIC_ADMIN_CRUCES.GIC_SP_GET_ADMINCONFIG", ref param);
              
                using (dataReader = datoConsulta.Tables[0].CreateDataReader())
                {
                    while (dataReader.Read())
                    {
                        _Entidad = new gic_adminconfig();
                        if (!dataReader.IsDBNull(0))
                            _Entidad.ADMINCFG_ID = int.Parse(dataReader["ADMINCFG_ID"].ToString());
                        if (!dataReader.IsDBNull(1))
                            _Entidad.ADMINCFG_NAME = dataReader["ADMINCFG_NAME"].ToString();
                        if (!dataReader.IsDBNull(2))
                            _Entidad.ADMINCFG_TYPE = dataReader["ADMINCFG_TYPE"].ToString();
                        if (!dataReader.IsDBNull(3))
                            _Entidad.ADMINCFG_VALUE = dataReader["ADMINCFG_VALUE"].ToString();

                        ListaEntidad.Add(_Entidad);
                    }
                }
                return ListaEntidad;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
           
            }
            finally
            {
                datoConsulta.Dispose();
             
            }
        }


    }
}