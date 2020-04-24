namespace AdministracionInstrumentos
{
    public class gic_InstrumentoxPregunta : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Relacion entidad pregunta
        /// </summary>
        public gic_Pregunta pre_IdPregunta { get; set; }

        /// <summary>
        /// Relacion entidad Instrumento
        /// </summary>
        public gic_Instumento ins_IdInstrumento { get; set; }

        /// <summary>
        /// Nombre tema
        /// </summary>
        public int ixp_Orden { get; set; }
    }
}