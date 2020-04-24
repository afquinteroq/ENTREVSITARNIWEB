using System;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_estadoPreguntas
    {
        /// <summary>
        /// Relacion entidad pregunta
        /// </summary>
        public string idTema { get; set; }

        /// <summary>
        /// Relacion entidad Instrumento
        /// </summary>
        public string idControl { get; set; }

        /// <summary>
        /// Nombre tema
        /// </summary>
        public string control { get; set; }
    }
}