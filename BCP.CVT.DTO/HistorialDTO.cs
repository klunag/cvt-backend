using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class HistorialDTO : BaseDTO
    {
        public string EntidadAsociado { get; set; }
        public string AsociadoId { get; set; }
        public string Data { get; set; }
    }
}
