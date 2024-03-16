using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CuestionarioPaeDTO: TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public decimal? SeccionCalculo { get; set; }
        public string SeccionCalculoStr => SeccionCalculo.HasValue ? string.Format("{0:0.00}", SeccionCalculo.Value) : "0.00";

    }
}
