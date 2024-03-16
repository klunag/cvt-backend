using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AlertaDTO : BaseDTO
    {
        public int TipoAlertaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaUltimaEjecucion { get; set; }
        //OTROS

        public int NroAlertasDetalle { get; set; }
        public int NroAlertaCriticas { get; set; }
        public int NroAlertasNoCriticas { get; set; }
        //FUNCIONALES

    }    
}
