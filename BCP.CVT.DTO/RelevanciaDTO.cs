using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RelevanciaDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public decimal? Peso { get; set; }
        public int? CodigoInterno { get; set; }
    }
}
