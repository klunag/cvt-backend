using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class ConsolidadoDto
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string EstadoAplicacion { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string Experto_Especialista { get; set; }
        public string Gestor_UsuarioAutorizador_ProductOwner { get; set; }
        public string JefeEquipo_ExpertoAplicacionUserIT_ProductOwner { get; set; }
        public string Owner_LiderUsuario_ProductOwner { get; set; }
        public string GestionadoPor { get; set; }
        public string EstadoRelacion { get; set; }
        public string AmbienteRelacion { get; set; }
        public string TipoEquipo { get; set; }
        public string Equipo { get; set; }
        public string SO { get; set; }
        public string CaracteristicaEquipo { get; set; }
        public string Temporal { get; set; }
        public string Dominio { get; set; }
    }
}
