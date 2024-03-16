using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RelacionPublicDTO : BaseDTO
    {
        public int RelacionId => 0;
        public string Equipo { get; set; }
        public int EquipoId { get; set; }
        public int TipoEquipo { get; set; }
        public int TipoRelacion { get; set; }
        public string CodigoAPT { get; set; }
        public string Relevancia { get; set; }
        public int RelevanciaId { get; set; }
        public string Ambiente { get; set; }
        public int AmbienteId { get; set; }
        public string CreadoPor { get; set; }
    }
}
