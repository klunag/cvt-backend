using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TecnologiaAplicacionDTO : BaseDTO
    {
        public int TecnologiaId { get; set; }
        public int AplicacionId { get; set; }
        public string CodigoAPT { get; set; }
        public TecnologiaDTO Tecnologia { get; set; }
        public AplicacionDTO Aplicacion { get; set; }
    }
}
