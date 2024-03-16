using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CriticidadDTO : BaseDTO
    {
        public string DetalleCriticidad { get; set; }
        public string PrefijoBase { get; set; }
        public decimal Prioridad { get; set; }
    }
}
