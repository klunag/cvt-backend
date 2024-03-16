using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class ReporteEvolucionTecnologiaDto
    {
        public int TecnologiaId { get; set; }
        public string ClaveTecnologia { get; set; }
        public int EstadoActual { get; set; }
        public int EstadoIndicador1 { get; set; }
        public int EstadoIndicador2 { get; set; }
        public string RoadmapConfigurado { get; set; }
        public string RoadmapSugerido { get; set; }
    }
}
