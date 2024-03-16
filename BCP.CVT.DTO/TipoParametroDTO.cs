using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TipoParametroDTO : BaseDTO
    {
        //public int TipoParametroId { get; set; }
        public string Nombre { get; set; }

        public List<ParametroDTO> Parametros { get; set; }
    }
}
