using System;

namespace AdministracionInstrumentos
{
    public class gic_Persona : gic_AdministradorDeCambios
    {
        /// <summary>
        /// Id Persona
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
        /// Tipo doc
        /// </summary>
        public string per_TipoDoc { get; set; }

        /// <summary>
        /// Numero documento
        /// </summary>
        public string per_NumeroDoc { get; set; }

        /// <summary>
        /// Fecha Nacimiento
        /// </summary>
        public DateTime per_FechaNacimiento { get; set; }

        /// <summary>
        /// Relacion
        /// </summary>
        public string per_Relacion { get; set; }

        /// <summary>
        /// Id Declaracion
        /// </summary>
        public Nullable<int> per_IdDeclaracion { get; set; }

        /// <summary>
        /// Id PersonaFuente
        /// </summary>
        public Nullable<int> per_IdPersonaFuente { get; set; }

        /// <summary>
        /// Tipo Victima
        /// </summary>
        public string per_TipoVictima { get; set; }

        /// <summary>
        /// Id Siniestro
        /// </summary>
        public Nullable<int> per_IdSiniestro { get; set; }

        /// <summary>
        /// Fuente
        /// </summary>
        public string per_Fuente { get; set; }

        /// <summary>
        /// Nombres
        /// </summary>
        public string per_Nombres { get; set; }

        /// <summary>
        /// Fecha Declaracion
        /// </summary>
        public DateTime per_FechaDeclaracion { get; set; }

        /// <summary>
        /// Fecha Hecho
        /// </summary>
        public DateTime per_FechaHecho { get; set; }

        /// <summary>
        /// Id Jefe Hogar
        /// </summary>
        public Nullable<int> per_IdJefeHogar { get; set; }

        /// <summary>
        /// Id Anexo
        /// </summary>
        public Nullable<int> per_IdAnexo { get; set; }

        /// <summary>
        /// Hecho
        /// </summary>
        public string per_Hecho { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        public string per_DeptoOcu { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        public string per_MunOcu { get; set; }

        /// <summary>
        /// Fecha Valoracion
        /// </summary>
        public DateTime per_FechaValoracion { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string per_Estado { get; set; }

        /// <summary>
        /// Parentesco
        /// </summary>
        public string per_Parentesco { get; set; }
    }
}