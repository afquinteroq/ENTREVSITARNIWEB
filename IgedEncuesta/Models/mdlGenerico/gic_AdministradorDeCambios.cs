using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IgedEncuesta.Models.mdlGenerico;

namespace AdministracionInstrumentos
{
    [Serializable] 
    public class gic_AdministradorDeCambios
    {
        /// <summary>
        /// Administracion de cambios
        /// </summary>
        public string usu_UsuarioCreacion { get; set; }

        public int usu_IdUsuario { get; set; }
        /// <summary>
        /// Administracion de cambios
        /// </summary>
        public DateTime usu_FechaCreacion { get; set; }

        public int ent_idEntidad { get; set; }

    }


}
