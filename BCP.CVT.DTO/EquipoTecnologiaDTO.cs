using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoTecnologiaDTO: BaseDTO
    {
        public int EquipoId { get; set; }
        public int TecnologiaId { get; set; }
        public string TecnologiaStr { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
    }
}
