using System;

namespace AdministracionInstrumentos
{
    public class gic_Instumento : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Identificador unico del instrumento
        /// </summary>
        public int ins_IdInstrumento { get; set; }

        /// <summary>
        /// Nombre Instrumento
        /// </summary>
        public string ins_NombreInstrumento { get; set; }

        /// <summary>
        /// Estado Instrumento
        /// </summary>
        public string ins_Activo { get; set; }

        /// <summary>
        /// fecha Inicio Instrumento
        /// </summary>
        public DateTime ins_FechaInicio { get; set; }

        /// <summary>
        /// fecha Fin Instrumento
        /// </summary>
        public DateTime ins_FechaFin { get; set; }
    }
}