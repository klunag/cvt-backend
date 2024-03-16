using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ProcesoDTO : BaseDTO
    {
        public int ProcesoId { get; set; }
        public int TipoTareaId { get; set; }
        public int? ResultadoEjecucionId { get; set; }
        public string LogResultados { get; set; }
        public string LogErrores { get; set; }
        public DateTime? FechaInicioEjecucion { get; set; }
        public DateTime? FechaFinEjecucion { get; set; }
        public bool FlagEjecutado { get; set; }
    }
}
