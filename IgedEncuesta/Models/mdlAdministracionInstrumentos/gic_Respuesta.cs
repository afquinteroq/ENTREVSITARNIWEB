using System;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_Respuesta : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Identificador unico de la respuesta
        /// </summary>
        public int res_IdRespuesta { get; set; }

        /// <summary>
        /// Respuesta
        /// </summary>
        public string res_Respuesta { get; set; }

        /// <summary>
        /// Tipo Respuesta
        /// </summary>
        public string res_TipoRespuesta { get; set; }

        /// <summary>
        /// Activa
        /// </summary>
        public string res_Activa { get; set; }

        /// <summary>
        /// Relacion Gic_Pregunta
        /// </summary>
        public gic_Pregunta pre_IdPregunta { get; set; }

        /// <summary>
        /// Obligatorio
        /// </summary>
        public string res_Padre { get; set; }

        /// <summary>
        /// Orden Respuesta
        /// </summary>
        public int res_OrdenRespuesta { get; set; }
    }
}