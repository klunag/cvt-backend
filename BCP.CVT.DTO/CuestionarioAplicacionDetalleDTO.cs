using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CuestionarioAplicacionDetalleDTO: BaseDTO
    {
        public int CuestionarioAplicacionId { get; set; }
        public int PreguntaId { get; set; }
        public int? AlternativaSeleccionada { get; set; }
        public int? Score { get; set; }
        public int CuestionarioId { get; set; }
    }
}
