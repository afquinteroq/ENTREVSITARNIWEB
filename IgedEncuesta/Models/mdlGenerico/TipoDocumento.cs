using System;
using System.Collections.Generic;
using System.Data;
using ObjetosTipos;


namespace IgedEncuesta.Models.mdlGenerico
{
    public class TipoDocumento : Models.mdlGenerico.mdlGenerico
    {
        public string ID { get; set; }
        public string TIPO_DOC { get; set; }

        public DataSet consultarBDTiposDoc()
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionCaracterizacion"].ConnectionString;
                datos.Conexion = connString;
                param = new List<Parametros>();
                param.Add(asignarParametro("pidcombo", 1, "System.Decimal", "7"));
                param.Add(asignarParametro("cur_out", 2, "Cursor", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_SPC_COMBOS", ref param);
                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }

        public List<TipoDocumento> modeloTipoDocumento(DataSet ds)
        {
            List<TipoDocumento> coleccion = new List<TipoDocumento>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            TipoDocumento objVictimaS = new TipoDocumento();
            objVictimaS.ID = "0";
            objVictimaS.TIPO_DOC = "Seleccione";
            coleccion.Add(objVictimaS);
            while (dataReader.Read())
            {
                TipoDocumento objVictima = new TipoDocumento();
                if (!DBNull.Value.Equals(dataReader["ID"])) objVictima.ID = dataReader["ID"].ToString();
                if (!DBNull.Value.Equals(dataReader["DESCRIPCION"])) objVictima.TIPO_DOC = dataReader["DESCRIPCION"].ToString();

                coleccion.Add(objVictima);
            }

            return (coleccion);
        }


        public List<TipoDocumento> tiposDocumento()
        {
            DataSet dsSalida = new DataSet();
            TipoDocumento objTipoDoc = new TipoDocumento();
            List<TipoDocumento> lstTipoDoc = new List<TipoDocumento>();
            try
            {
                dsSalida = objTipoDoc.consultarBDTiposDoc();
                lstTipoDoc = objTipoDoc.modeloTipoDocumento(dsSalida);
                return (lstTipoDoc);
            }
            finally
            {
                dsSalida.Dispose();
            }
        }
    }

    public class Perfiles : Models.mdlGenerico.mdlGenerico
    {
        public string IDPERFIL { get; set; }
        public string PERFIL { get; set; }

        public DataSet consultarPerfiles(string idUsuario, string idAplicacion)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            try
            {
                datos.MotorBasedatos = true;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionSeguridad"].ConnectionString;
                datos.Conexion = connString;
                //NO_CONSULTA_NOVEDADES(P_OBJ IN VARCHAR2,S_MSGERROR OUT VARCHAR2,S_CURSOR OUT SYS_REFCURSOR)
                param = new List<Parametros>();
                param.Add(asignarParametro("p_IdUsuario", 1, "System.Int32", idUsuario));
                param.Add(asignarParametro("p_IdAplicacion", 1, "System.Int32", idAplicacion));
                param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
                param.Add(asignarParametro("p_Mensaje", 2, "System.String", ""));
                dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NOVEDADESUSUARIOS.PR_GEN_CONPERFILES", ref param);
                return dsSalida;
            }
            finally
            {
                dsSalida.Dispose();
            }

        }

        public Perfiles listaPerfiles(string idUsuario, string idAplicacion)
        {
            DataSet ds = new DataSet();
            Perfiles objPerfiles = new Perfiles();
            try
            {
                ds = objPerfiles.consultarPerfiles(idUsuario, idAplicacion);
                IDataReader dataReader = null;
                dataReader = ds.Tables[0].CreateDataReader();
                while (dataReader.Read())
                {
                    objPerfiles = new Perfiles();
                    if (!DBNull.Value.Equals(dataReader["ID_PERFIL"])) objPerfiles.IDPERFIL = dataReader["ID_PERFIL"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PERFIL"])) objPerfiles.PERFIL = dataReader["PERFIL"].ToString();
                }
                return objPerfiles;
            }
            finally
            {
                ds.Dispose();
            }

        }

    }
}