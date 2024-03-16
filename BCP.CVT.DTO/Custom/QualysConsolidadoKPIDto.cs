using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class QualysConsolidadoKPIDto
    {
        public int CantidadEquiposExistentes { get; set; }
        public int CantidadEquiposCVTEncontrados { get; set; }
        public int CantidadEquiposCVTNoEncontrados { get; set; }
        public int CantidadTecnologiaAsignada { get; set; }
        public int CantidadTecnologiaNoAsignada { get; set; }
        public int CantidadProductoAsignado { get; set; }
        public int CantidadProductoNoAsignado { get; set; }
        public int CantidadTecnologiasInstaladasExistentes { get; set; }
        public int CantidadTecnologiasInstaladasRegistradas { get; set; }
        public int CantidadTecnologiasInstaladasNoRegistradas { get; set; }
    }
}
