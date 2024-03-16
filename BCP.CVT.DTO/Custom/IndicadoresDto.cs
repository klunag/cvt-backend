using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class IndicadoresDto
    {
        public int Equipos { get; set; }
        public int Tecnologias { get; set; }
        public int Aplicaciones { get; set; }
        public int Relaciones { get; set; }
        public int NroMesesTecnologiaPorVencer { get; set; }
        public List<TecnologiaPorVencerDto> TecnologiaDetalle { get; set; }
    }
}
