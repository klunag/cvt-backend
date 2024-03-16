using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SubdominioEquivalenciaDTO: BaseDTO
    {
        public int DominioId { get; set; }
        public string DominioNombre { get; set; }
        public int SubdominioId { get; set; }
        public string SubdominioNombre { get; set; }
        public int EquivalenciaSubdomId { get; set; }
        public string EquivalenciaSubdomNombre { get; set; }
    }
}
