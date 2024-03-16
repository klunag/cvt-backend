using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class GuardicoreConsolidadoDto
    {
        public int idestado { get; set; }
        public string estado { get; set; }
        public int cantidadEstado { get; set; }
        public int cantidadSO { get; set; }
        public string nombre { get; set; }
        public string so { get; set; }
        public string fechaescaneo { get; set; }
        public string ip { get; set; }
        public string etiqueta { get; set; }
        public DateTime? fechaEscaneoConvert { get; set; } 
        public string SOTecnologia { get; set; }
    }
}
