using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CodigoCISDTO: BaseDTO
    {
        public string CodigoTemporal { get; set; }
        public string Descripcion { get; set; }
        public List<string> Servidores { get; set; }
        public List<string> ServidoresNoRegistrados { get; set; }
        public int ServidoresAsociados { get; set; }
        public string Servidor { get; set; }
        public string HostnameServidor { get; set; }
        public string Ambiente { get; set; }

    }
}
