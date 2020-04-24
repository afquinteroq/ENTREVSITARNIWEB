using System;

namespace AdministracionInstrumentos
{
    [Serializable]
    public class gic_PreguntasxPersona : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Identificador unico de la pregunta
        /// </summary>
        public int pre_IdPregunta { get; set; }

        /// <summary>
        /// Pregunta
        /// </summary>
        public string pre_pregunta { get; set; }

        /// <summary>
        /// Id Padre Pregunta-depende
        /// </summary>
        public string pre_TipoPregunta { get; set; }

        /// <summary>
        /// Tipo Campo
        /// </summary>
        public string pre_TipoCampo { get; set; }

        /// <summary>
        /// Codigo hogar
        /// </summary>
        public string cod_Hogar { get; set; }

        /// <summary>
        /// Identificador unico de la persona
        /// </summary>
        public int per_IdPersona { get; set; }

        /// <summary>
        /// Primer Nombre
        /// </summary>
        public string per_PrimerNombre { get; set; }

        /// <summary>
        /// Segundo Nombre
        /// </summary>
        public string per_SegundoNombre { get; set; }

        /// <summary>
        /// Primer apellido
        /// </summary>
        public string per_PrimerApellido { get; set; }

        /// <summary>
        /// Segundo apellido
        /// </summary>
        public string per_SegundoApellido { get; set; }

        /// <summary>
        /// Validacion persona
        /// </summary>
        public int validacion_Persona { get; set; }

        /// <summary>
        /// Orden Prioirdad
        /// </summary>
        public string ordenPrioridad { get; set; }

        /// <summary>
        /// Orden Pregunta
        /// </summary>
        public int ipx_Orden { get; set; }

        /// <summary>
        /// Fecha Nacimiento
        /// </summary>
        public DateTime per_fechaNacimiento { get; set; }        

        /// <summary>
        /// Numero documento
        /// </summary>
        public string per_NumeroDoc { get; set; }
    }
}