using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SolicitudAprobadoresDTO: BaseDTO
    {
        public int SolicitudAplicacionId { get; set; }
        public string Matricula { get; set; }
        public bool? FlagAprobado { get; set; }
        public int? EstadoAprobacion { get; set; }
    }
}
