using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class LogsDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public string Metodo { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }
        public int TotalFilas { get; set; }
    }
    
}
