using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ObjetosTipos;
using System.Configuration;


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
        public string HV2 { get; set; }
        public string HV3 { get; set; }
        public string HV4 { get; set; }
        public string HV5 { get; set; }
        public string HV6 { get; set; }
        public string HV7 { get; set; }
        public string HV8 { get; set; }
        public string HV9 { get; set; }
        public string HV10 { get; set; }
        public string HV11 { get; set; }
        public string HV12 { get; set; }
        public string HV13 { get; set; }
        public string HV14 { get; set; }
        public string TIPO_VICTIMA { get; set; }
        public bool JEFE_HOGAR { get; set; }
        public string ID_TBPERSONA { get; set; }
        public string FECHA_ULT_CARACTERIZACION { get; set; }
        public string DOCUMENTO_RUV { get; set; }
        public string DOCUMENTO_CARACTERIZACION { get; set; }
        public string HABILITADO_PARA_CARACTERIZACION { get; set; }



        public List<Victima> consultarVictimas(string numDoc, string opcionBusqueda)
        {
            DataSet dsSalida = new DataSet();
            List<Victima> coleccion = new List<Victima>();
            List<Victima> coleccion2 = new List<Victima>();
            List<string> idPersonas = new List<string>();
            List<string> idRUVs = new List<string>();
            Persona p = new Persona();
            Victima v = new Victima();
            List<Persona> personas = new List<Persona>();
            int i;

            dsSalida = consultarVictimasRUV(numDoc, opcionBusqueda);
            coleccion = modeloVictimas(dsSalida);

            // Entra si encontró victimas registradas en el RUV para el número de documento suministrado
            if (coleccion.Count > 0)
            {
                foreach (Victima item in coleccion)
                {
                    // Verifica si la victima identificada en RUV ya fue caracterizada en la tabla GIC_RUV_PERSONAS
                    idPersonas = consultarVictimasPersonas(item.CONS_PERSONA);
                    i = 0;
                    // Actualiza los datos de la Victima con los datos de la caracterización que se realizó
                    foreach (string persona in idPersonas)
                    {
                        i++;
                        p = consultaDatosPersona(persona);
                        //-----------------------------------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                        // CORRECION HALLAZGO: SI NO HAY REGISTRO EN P NO DEBE HACER NADA NO HAY DATOS
                        //---------------------------------------------------------------------------
                        if (!String.IsNullOrEmpty(p.ID_PERSONA))
                        {
                        //---- FIN MODIFICACION JOSE VASQUEZ OCT.28.2015
                            if (i == 1)
                            {
                                item.TIPO_DOC = p.TIPO_DOC;
                                item.DOCUMENTO = p.NUMERO_DOC;
                                item.NOMBRE1 = p.PRIMER_NOMBRE;
                                item.NOMBRE2 = p.SEGUNDO_NOMBRE;
                                item.APELLIDO1 = p.PRIMER_APELLIDO;
                                item.APELLIDO2 = p.SEGUNDO_APELLIDO;
                                item.NOMBRES_COMPLETOS = p.NOMBRES_COMPLETOS;
                                item.F_NACIMIENTO = p.FECHA_NACIMIENTO;
                                item.ID_TBPERSONA = persona;
                                //------------------------------------------------
                                //MODIFICACION: JOSE VASQUEZ OCT.27.2015
                                // ADICION CAMPO FECHA DE CARACTERIZACION
                                //-----------------------------------------------
                                item.FECHA_ULT_CARACTERIZACION = p.FECHA_ULT_CARACTERIZACION;
                                item.HABILITADO_PARA_CARACTERIZACION = p.HABILITADO_PARA_CARACTERIZACION;
                                item.DOCUMENTO_CARACTERIZACION = p.NUMERO_DOC;
                                //FIN MODIFICACION OCT.27.2015

                            }
                            else
                            {
                                Victima nuevoItem = new Victima();
                                nuevoItem.TIPO_DOC = p.TIPO_DOC;
                                nuevoItem.DOCUMENTO = p.NUMERO_DOC;
                                nuevoItem.NOMBRE1 = p.PRIMER_NOMBRE;
                                nuevoItem.NOMBRE2 = p.SEGUNDO_NOMBRE;
                                nuevoItem.APELLIDO1 = p.PRIMER_APELLIDO;
                                nuevoItem.APELLIDO2 = p.SEGUNDO_APELLIDO;
                                nuevoItem.NOMBRES_COMPLETOS = p.NOMBRES_COMPLETOS;
                                nuevoItem.F_NACIMIENTO = p.FECHA_NACIMIENTO;
                                nuevoItem.PERT_ETNICA = item.PERT_ETNICA;
                                nuevoItem.SOBREVIVENCIA = item.SOBREVIVENCIA;
                                nuevoItem.TIPO_VICTIMA = item.TIPO_VICTIMA;
                                nuevoItem.IDENTIFICADO = item.IDENTIFICADO;
                                nuevoItem.CONS_PERSONA = item.CONS_PERSONA;
                                nuevoItem.DISCAP = item.DISCAP;
                                nuevoItem.GENERO_HOM = item.GENERO_HOM;
                                nuevoItem.HV1 = item.HV1;
                                nuevoItem.HV2 = item.HV2;
                                nuevoItem.HV3 = item.HV3;
                                nuevoItem.HV4 = item.HV4;
                                nuevoItem.HV5 = item.HV5;
                                nuevoItem.HV6 = item.HV6;
                                nuevoItem.HV7 = item.HV7;
                                nuevoItem.HV8 = item.HV8;
                                nuevoItem.HV9 = item.HV9;
                                nuevoItem.HV10 = item.HV10;
                                nuevoItem.HV11 = item.HV11;
                                nuevoItem.HV12 = item.HV12;
                                nuevoItem.HV13 = item.HV13;
                                nuevoItem.HV14 = item.HV14;
                                nuevoItem.ID_TBPERSONA = persona;
                                //------------------------------------------------
                                //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                                // ITEMS NUEVOS HAN SIDO CARACTERIZADAS HASTA BUSCAR EN CARACTERIZACION
                                //-----------------------------------------------
                                nuevoItem.HABILITADO_PARA_CARACTERIZACION = "SI";
                                //FIN JOSE VASQUEZ OCT.28.2015
                                coleccion2.Add(nuevoItem);
                            }
                        }
                    }

                }
            }


            coleccion.AddRange(coleccion2);

            // Se busca el número de documento en la tabla personas para mostrar las victimas creadas como no 
            // incluidas en procesos de caracterización anteriores
            personas = consultaPersonasCaracterizadas(numDoc);

            foreach (Persona item in personas)
            {

                idRUVs = consultarPersonasRUV(item.ID_PERSONA);

                // Verifica si la persona tiene una relacion con una victimaen la tabla GIC_RUV_PERSONAS
                if (idRUVs.Count > 0)
                {
                    foreach (string idRUV in idRUVs)
                    {
                        // Si la victima no se ingreso en el proceso de identificación anterior se procede a traer los datos del RUV
                        // y adicionar a la victima a la colección
                        if (!coleccion.Exists(x => x.CONS_PERSONA == idRUV && x.ID_TBPERSONA == item.ID_PERSONA))
                        {
                            v = new Victima();
                            v = consultaDatosRUV(idRUV);

                            v.TIPO_DOC = item.TIPO_DOC;
                            v.DOCUMENTO = item.NUMERO_DOC;
                            v.NOMBRE1 = item.PRIMER_NOMBRE;
                            v.NOMBRE2 = item.SEGUNDO_NOMBRE;
                            v.APELLIDO1 = item.PRIMER_APELLIDO;
                            v.APELLIDO2 = item.SEGUNDO_APELLIDO;
                            v.NOMBRES_COMPLETOS = item.NOMBRES_COMPLETOS;
                            v.F_NACIMIENTO = item.FECHA_NACIMIENTO;
                            v.PERT_ETNICA = v.PERT_ETNICA;
                            v.SOBREVIVENCIA = v.SOBREVIVENCIA;
                            v.TIPO_VICTIMA = v.TIPO_VICTIMA;
                            v.IDENTIFICADO = v.IDENTIFICADO;
                            v.CONS_PERSONA = v.CONS_PERSONA;
                            v.DISCAP = v.DISCAP;
                            v.GENERO_HOM = v.GENERO_HOM;
                            v.HV1 = v.HV1;
                            v.HV2 = v.HV2;
                            v.HV3 = v.HV3;
                            v.HV4 = v.HV4;
                            v.HV5 = v.HV5;
                            v.HV6 = v.HV6;
                            v.HV7 = v.HV7;
                            v.HV8 = v.HV8;
                            v.HV9 = v.HV9;
                            v.HV10 = v.HV10;
                            v.HV11 = v.HV11;
                            v.HV12 = v.HV12;
                            v.HV13 = v.HV13;
                            v.HV14 = v.HV14;
                            v.ID_TBPERSONA = item.ID_PERSONA;
                            //------------------------------------------------
                            //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                            // ITEMS NUEVOS HAN SIDO CARACTERIZADAS HASTA BUSCAR EN CARACTERIZACION
                            //-----------------------------------------------
                            v.HABILITADO_PARA_CARACTERIZACION = "SI";
                            //FIN JOSE VASQUEZ OCT.28.2015

                            coleccion.Add(v);
                        }
                    }
                }
                else
                {
                    // Si la persona no tiene ninguna relacion con una victima del RUV se agrega a la colección como VICTIMA  NO INCLUIDA
                    v = new Victima();
                    v.TIPO_DOC = item.TIPO_DOC;
                    v.DOCUMENTO = item.NUMERO_DOC;
                    v.NOMBRE1 = item.PRIMER_NOMBRE;
                    v.NOMBRE2 = item.SEGUNDO_NOMBRE;
                    v.APELLIDO1 = item.PRIMER_APELLIDO;
                    v.APELLIDO2 = item.SEGUNDO_APELLIDO;
                    v.NOMBRES_COMPLETOS = item.NOMBRES_COMPLETOS;
                    v.F_NACIMIENTO = item.FECHA_NACIMIENTO;
                    v.TIPO_VICTIMA = "NO INCLUIDO";
                    v.ID_TBPERSONA = item.ID_PERSONA;
                    //------------------------------------------------
                    //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                    // ITEMS NUEVOS HAN SIDO CARACTERIZADAS HASTA BUSCAR EN CARACTERIZACION
                    //-----------------------------------------------
                    v.HABILITADO_PARA_CARACTERIZACION = "SI";
                    //FIN JOSE VASQUEZ OCT.28.2015
                    // v.CONS_PERSONA = consecutivoPersonaAleatorio(coleccion);

                    coleccion.Add(v);
                }
            }


            
            return (coleccion);

        }

        public DataSet consultarVictimasRUV(string numDoc, string opcionBusqueda)
        {
            List<Victima> coleccion = new List<Victima>();
            List<Parametros> param = new List<Parametros>();
            Victima usuario = new Victima();
            DataSet dsSalida = new DataSet();


            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
            datos.Conexion = connString;

            param = new List<Parametros>();
            param.Add(asignarParametro("pDocumento", 1, "System.String", numDoc));
            param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
            param.Add(asignarParametro("p_Salida", 2, "System.Int32", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NUEVOS_INCLUIDOS.PR_GET_INCLUIDO", ref param);

            //   coleccion = modeloHogar(dsSalida);

            return dsSalida;
        }

        public List<Victima> modeloVictimas(DataSet ds)
        {
            List<Victima> coleccion = new List<Victima>();
            IDataReader dataReader = null;
            dataReader = ds.Tables[0].CreateDataReader();
            List<Victima> maestroHogar = new List<Victima>();
            bool cargarVictima = true;
            
            maestroHogar = (List<Victima>)HttpContext.Current.Session["ModeloHogar"];
            while (dataReader.Read())
            {
                //if (maestroHogar == null) cargarVictima = true;
           //     else cargarVictima = !maestroHogar.Any(x => x.CONS_PERSONA == dataReader["CONS_PERSONA"].ToString());
    
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
                        objVictima.TIPO_VICTIMA = "INCLUIDO";
                        //------------------------------------------------
                        //MODIFICACION: JOSE VASQUEZ OCT.28.2015
                        // LAS VICTIMAS NO HAN SIDO CARACTERIZADAS HASTA BUSCAR EN CARACTERIZACION
                        //-----------------------------------------------
                        objVictima.HABILITADO_PARA_CARACTERIZACION = "SI";
                        //FIN JOSE VASQUEZ OCT.28.2015

                        coleccion.Add(objVictima);
                    }
            //    }
            }

            return (coleccion);
        }

        public DataSet consultarGrupoFamiliar(string cons_persona)
        {
            List<Victima> coleccion = new List<Victima>();
            List<Parametros> param = new List<Parametros>();
            Victima usuario = new Victima();
            DataSet dsSalida = new DataSet();


            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionVictimas"].ConnectionString;
            datos.Conexion = connString;

            param = new List<Parametros>();
            param.Add(asignarParametro("p_conspersona", 1, "System.String", cons_persona));
            param.Add(asignarParametro("p_cursorSalida", 2, "Cursor", ""));
            param.Add(asignarParametro("p_Salida", 2, "System.Int32", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("PKG_NUEVOS_INCLUIDOS.PR_GET_GRUPO_FAMILIAR", ref param);

            //   coleccion = modeloHogar(dsSalida);

            return dsSalida;
        }


        public List<string> consultarVictimasPersonas(string consPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            IDataReader dataReader = null;
            List<string> idPersonas = new List<string>();

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", consPersona));
            param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET__RUV_PERSONA_X_RUV", ref param);

            if (dsSalida.Tables.Count > 0)
            {
                dataReader = dsSalida.Tables[0].CreateDataReader();
                while (dataReader.Read())
                    idPersonas.Add(dataReader["PER_IDPERSONA"].ToString());
            }

            return idPersonas;
        }

        public List<Persona> consultarGpo_Familiar_x_Cons_Persona(string consPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            IDataReader dataReader = null;
            List<string> idPersonas = new List<string>();
            List<Persona> personas = new List<Persona>();

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("CONS_PERSONA", 1, "System.Int32", consPersona));
            param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET_GPO_FAM_X_CONS_PER", ref param);

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
                    //if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) p.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"])) p.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = Convert.ToDateTime(dataReader["PER_FECHANACIMIENTO"].ToString()).ToString("dd/MM/yyyy");
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

        
        public List<string> consultarPersonasRUV(string idPersona)
        {
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            IDataReader dataReader = null;
            List<string> idRUVs = new List<string>();

            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            datos.MotorBasedatos = true;
            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("ID_PERSONA", 1, "System.Int32", idPersona));
            param.Add(asignarParametro("cur_OUT ", 2, "Cursor", ""));
            dsSalida = datos.ConsultarConProcedimientoAlmacenado("GIC_CARACTERIZACION.GIC_SP_GET__RUV_PERS_X_IDPERS", ref param);

            if (dsSalida.Tables.Count > 0)
            {
                dataReader = dsSalida.Tables[0].CreateDataReader();
                while (dataReader.Read())
                    idRUVs.Add(dataReader["CONS_PERSONA"].ToString());
            }

            return idRUVs;
        }

        public Persona consultaDatosPersona(string idPersona)
        {
            Persona p = new Persona();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();


            AccesoDatos.ConsultasParticulares datos = new AccesoDatos.ConsultasParticulares();
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
            //MODIFICACION: JOSE VASQUEZ OCT.27.2015
            // ADICION CAMPO DOCUMENTO RUV
            //-----------------------------------------------
            p.FECHA_ULT_CARACTERIZACION = param.Find(x => x.Nombre == "p_fechaUltCaracterizacion").Valor;
            p.HABILITADO_PARA_CARACTERIZACION = param.Find(x => x.Nombre == "p_habilitadoParaCarac").Valor;
            // SI NO ENCUENTRA DATOS, ENTOCES ESTA HABILITADO PARA CARACTERIZACION
           if (String.IsNullOrEmpty(p.HABILITADO_PARA_CARACTERIZACION)) 
                 p.HABILITADO_PARA_CARACTERIZACION = "SI";
            //FIN: JOSE VASQUEZ OCT.27.2015

            return p;
        }

        public Victima consultaDatosRUV(string consPersona)
        {
            List<Victima> coleccion = new List<Victima>();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
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

                    objVictima.TIPO_VICTIMA = "INCLUIDO";
                }
            }

            return objVictima;
        }

        public List<Persona> consultaPersonasCaracterizadas(string numeroDoc)
        {
            
            List<Persona> personas = new List<Persona>();
            List<Parametros> param = new List<Parametros>();
            DataSet dsSalida = new DataSet();
            AccesoDatos.AccesoDatos datos = new AccesoDatos.AccesoDatos();
            IDataReader dataReader = null;

            datos.MotorBasedatos = true;
            datos.Conexion = connStringCar;

            param = new List<Parametros>();
            param.Add(asignarParametro("P_IDPERSONA", 1, "System.Int32", numeroDoc));
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
                    //if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = dataReader["PER_FECHANACIMIENTO"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_TIPODOC"])) p.TIPO_DOC = dataReader["PER_TIPODOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_NUMERODOC"])) p.NUMERO_DOC = dataReader["PER_NUMERODOC"].ToString();
                    if (!DBNull.Value.Equals(dataReader["PER_FECHANACIMIENTO"])) p.FECHA_NACIMIENTO = Convert.ToDateTime(dataReader["PER_FECHANACIMIENTO"].ToString()).ToString("dd/MM/yyyy");
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
                    if (!coleccionVic.Exists(x => x.DOCUMENTO  == per.R_NUMERO_DOC  && x.NOMBRE1  == per.R_PRIMER_NOMBRE
                        && x.NOMBRE2 == per.R_SEGUNDO_NOMBRE && x.APELLIDO1 == per.R_PRIMER_APELLIDO && x.APELLIDO2 == per.R_SEGUNDO_APELLIDO))
                    {

                        Victima item = new Victima();
                        item.TIPO_DOC = per.TIPO_DOC;
                        item.DOCUMENTO = per.NUMERO_DOC;
                        item.NOMBRE1 = per.PRIMER_NOMBRE;
                        item.NOMBRE2 = per.SEGUNDO_NOMBRE;
                        item.APELLIDO1 = per.PRIMER_APELLIDO;
                        item.APELLIDO2 = per.SEGUNDO_APELLIDO;
                        item.NOMBRES_COMPLETOS = per.PRIMER_NOMBRE + ' ' +per.SEGUNDO_NOMBRE + ' ' + per.PRIMER_APELLIDO + ' ' + per.SEGUNDO_APELLIDO;
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

                        string numAleatorio = consecutivoPersonaAleatorio(colVictimasRes, coleccionVic);
                        item.CONS_PERSONA = numAleatorio;
                        //FIN MODIFICACION OCT.27.2015

                        colVictimasRes.Add(item);
                    }
                }
            }
            return colVictimasRes;

        }
    }
}