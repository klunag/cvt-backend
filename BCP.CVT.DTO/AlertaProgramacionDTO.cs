using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AlertaProgramacionDTO : BaseDTO
    {
        public int AlertaId { get; set; }
        public int FrecuenciaEnvio { get; set; }
        public int NroDias { get; set; }
        public DateTime FechaInicio { get; set; }
        public int HoraEnvio { get; set; }
        public DateTime? FechaUltimoEnvio { get; set; }
        public int? ComponenteImpactado { get; set; }
        public string Buzones { get; set; }
        public string Asunto { get; set; }
        public string FechaInicioStr
        {
            get
            {
                return FechaInicio.ToString("dd/MM/yyyy");
            }
        }
        public AlertaDTO Alerta { get; set; }
    }
}
