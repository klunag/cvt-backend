using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TipoCicloVidaDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int NroPeriodo { get; set; }
        public bool FlagDefault { get; set; }
        public bool FlagTecnologia { get; set; }

    } 

}
