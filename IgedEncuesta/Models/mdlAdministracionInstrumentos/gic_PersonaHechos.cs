namespace AdministracionInstrumentos
{
    public class gic_PersonaHechos : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Numero de documento
        /// </summary>
        public string per_numeroDoc { get; set; }

        /// <summary>
        /// Hecho
        /// </summary>
        public string per_hecho { get; set; }

        /// <summary>
        /// Fuente
        /// </summary>
        public string per_fuente { get; set; }
    }
}