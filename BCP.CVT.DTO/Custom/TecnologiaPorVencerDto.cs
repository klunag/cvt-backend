using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class TecnologiaPorVencerDto: BaseDTO
    {
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string ClaveTecnologia { get; set; }
        public DateTime? FechaFin { get; set; }
        public int TotalAplicaciones { get; set; }
        public int TotalFilas { get; set; }
        public int NroMeses { get; set; }

        public string FechaFinStr => FechaFin.HasValue? FechaFin.Value.ToString("dd/MM/yyyy") : "-";
    }
}
