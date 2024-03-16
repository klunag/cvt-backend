using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionPortafolioResponsablesDTO: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Matricula { get; set; }
        public string Colaborador { get; set; }
        public string CorreoElectronico { get; set; }
        public int PortafolioResponsableId { get; set; }
        public string PortafolioResponsableStr { get; set; }
        public int NroAplicaciones { get; set; }
        public bool? FlagSolicitud { get; set; }
    }
}
