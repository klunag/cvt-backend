using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoPublicDTO : BaseDTO
    {
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string Ambiente { get; set; }
        public int AmbienteId { get; set; }
        public string DominioServidor { get; set; }
        public int DominioServidorId { get; set; }
        public string TipoEquipo { get; set; }
        public int TipoEquipoId { get; set; }
        public string CaracteristicaEquipo { get; set; }
        public int CaracteristicaEquipoId { get; set; }
        public string Tecnologia { get; set; }
        public int TecnologiaId { get; set; }
    }
}
