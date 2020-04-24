using System;

namespace AdministracionInstrumentos
{
    public class gic_MiembroHogar
    {
        private string documento;

        public string DOCUMENTO
        {
            get { return documento; }
            set { documento = value; }
        }

        private string nombres;

        public string NOMBRES
        {
            get { return nombres; }
            set { nombres = value; }
        }

        private string fuente;

        public string FUENTE
        {
            get { return fuente; }
            set { fuente = value; }
        }

        private int numDeclaracion;

        public int NUM_DECLARACION
        {
            get { return numDeclaracion; }
            set { numDeclaracion = value; }
        }

        private DateTime fechaDeclaracion;

        public DateTime FECHA_DECLARACION
        {
            get { return fechaDeclaracion; }
            set { fechaDeclaracion = value; }
        }

        private DateTime fechaHecho;

        public DateTime FECHA_HECHO
        {
            get { return fechaHecho; }
            set { fechaHecho = value; }
        }

        private int idSiniestro;

        public int ID_SINIESTRO
        {
            get { return idSiniestro; }
            set { idSiniestro = value; }
        }

        private string idJefeHogar;

        public string ID_JEFE_HOGAR
        {
            get { return idJefeHogar; }
            set { idJefeHogar = value; }
        }

        private int idAnexo;

        public int ID_ANEXO
        {
            get { return idAnexo; }
            set { idAnexo = value; }
        }

        private int idRegPersona;

        public int ID_REG_PERSONA
        {
            get { return idRegPersona; }
            set { idRegPersona = value; }
        }

        private string relacion;

        public string RELACION
        {
            get { return relacion; }
            set { relacion = value; }
        }

        private string deptoOcu;

        public string DEPTO_OCU
        {
            get { return deptoOcu; }
            set { deptoOcu = value; }
        }

        private string munOcu;

        public string MUN_OCU
        {
            get { return munOcu; }
            set { munOcu = value; }
        }

        private DateTime fValoracion;

        public DateTime FECHA_VALORACION
        {
            get { return fValoracion; }
            set { fValoracion = value; }
        }

        private string estado;

        public string ESTADO
        {
            get { return estado; }
            set { estado = value; }
        }

        private string hecho;

        public string HECHO
        {
            get { return hecho; }
            set { hecho = value; }
        }

        private string pNombre;

        public string PRIMERNOMBRE
        {
            get { return pNombre; }
            set { pNombre = value; }
        }

        private string sNombre;

        public string SEGUNDONOMBRE
        {
            get { return sNombre; }
            set { sNombre = value; }
        }

        private string pApellido;

        public string PRIMERAPELLIDO
        {
            get { return pApellido; }
            set { pApellido = value; }
        }

        private string sApellido;

        public string SEGUNDOAPELLIDO
        {
            get { return sApellido; }
            set { sApellido = value; }
        }

        private DateTime fNacimiento;

        public DateTime FECHANACIMIENTO
        {
            get { return fNacimiento; }
            set { fNacimiento = value; }
        }

        private string tDoc;

        public string TIPODOC
        {
            get { return tDoc; }
            set { tDoc = value; }
        }

        private string usuCreacion;

        public string USUA_CREACION
        {
            get { return usuCreacion; }
            set { usuCreacion = value; }
        }

        private DateTime fCreacion;

        public DateTime FECHACREACION
        {
            get { return fCreacion; }
            set { fCreacion = value; }
        }

        private string tVictima;

        public string TIPOVICTIMA
        {
            get { return tVictima; }
            set { tVictima = value; }
        }
    }
}