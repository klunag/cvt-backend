using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class AplicacionTecnologiaDto: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Aplicacion { get; set; }
        public string Experto { get; set; }
        public string LP { get; set; }
        public string AppServer { get; set; }
		public string WebServer { get; set; }
        public string Middleware { get; set; }
        public string BD { get; set; }
        public string SO { get; set; }
    }
}
