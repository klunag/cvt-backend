using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RelacionTecnologiaDTO: BaseDTO
    {
        public string Tecnologia { get; set; }
        public int DominioId { get; set; }
        public string Dominio { get; set; }
        public string SubdominioId { get; set; }
        public string Subdominio { get; set; }
        public string Familia { get; set; }
    }
}
