using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class AplicacionPublicDto
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string EstadoAplicacion { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }        
        public string AreaBIAN { get; set; }
        public string DominioBIAN { get; set; }
        public string JefaturaATI { get; set; }        
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public List<AplicacionExpertoDTO> ResponsablesPortafolio { get; set; }
    }
}
