using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AlertaDetalleDTO : BaseDTO
    {
        public int AlertaId { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }
        public string FechaHoraCreacionFormato
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:dd tt");
            }
        }
        public int? Criticidad { get; set; }

        public AlertaDTO Alerta { get; set; }
    }
}
