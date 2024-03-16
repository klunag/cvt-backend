using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class CuestionarioAplicacionDTO: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string GestorAplicacion { get; set; }
        public string Consultor { get; set; }
        public decimal? ConfidencialidadCalculo { get; set; }
        public decimal? IntegridadCalculo { get; set; }
        public decimal? DisponibilidadCalculo { get; set; }
        public decimal? PrivacidadCalculo { get; set; }

        public string ConfidencialidadDescripcion { get; set; }
        public string IntegridadDescripcion { get; set; }
        public string DisponibilidadDescripcion { get; set; }
        public string PrivacidadDescripcion { get; set; }
        public string Clasificacion { get; set; }
        public int? NivelCriticidad { get; set; }
        public string CriticidadFinal { get; set; }

        public int CuestionarioAplicacionDetalleId { get; set; }

        public List<CuestionarioAplicacionDetalleDTO> CuestionarioAplicacionDetalle { get; set; }
    }
}
