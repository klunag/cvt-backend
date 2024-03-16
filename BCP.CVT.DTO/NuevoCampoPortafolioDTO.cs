using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class NuevoCampoPortafolioDTO: BaseDTO
    {
        public int InfoCampoPortafolioId { get; set; }
        public string CodigoAPT { get; set; }
        public string Valor { get; set; }
        public int? TipoInputId { get; set; }
        public bool? FlagMultiselect { get; set; }

        public string Codigo { get; set; }
    }
}
