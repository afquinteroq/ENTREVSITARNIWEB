using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgedEncuesta.Models.mdlEncuesta
{
    public class Persona    
    {
        public string TIPO_DOC { get; set; }
        public string NUMERO_DOC { get; set; }
        public string PRIMER_NOMBRE { get; set; }
        public string SEGUNDO_NOMBRE { get; set; }
        public string PRIMER_APELLIDO { get; set; }
        public string SEGUNDO_APELLIDO { get; set; }
        public string NOMBRES_COMPLETOS { get; set; }
        public string FECHA_NACIMIENTO { get; set; }
        public string ESTADO { get; set; }
        public string ID_PERSONA { get; set; }
        public string FECHA_ULT_CARACTERIZACION { get; set; }
        public string HABILITADO_PARA_CARACTERIZACION { get; set; }
        public string R_NUMERO_DOC { get; set; }
        public string R_PRIMER_NOMBRE { get; set; }
        public string R_SEGUNDO_NOMBRE { get; set; }
        public string R_PRIMER_APELLIDO { get; set; }
        public string R_SEGUNDO_APELLIDO { get; set; }

    }
}