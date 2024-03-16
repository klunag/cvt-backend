using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class SolRolHistorialDTO : BaseDTO
    {
        public long? HistorialId { get; set; }
        public long? CabeceraId { get; set; }
        public long? SolicitudId { get; set; }
        public string Responsable { get; set; }
        public string Comentario { get; set; }
        public string ObservacionSeguridad { get; set; }
        public string ObservacionOwner { get; set; }
        public int? EstadoSolicitud { get; set; }
        public string EstadoSolicitudStr
        {
            get
            {

                return Utilitarios.GetEnumDescription2((EstadoSolicitudRolFuncion)EstadoSolicitud);
            }
        }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string FechaRevisionStr => FechaRevision.HasValue ? FechaRevision.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
        public string CreadoPor { get; set; }
        public string RevisadoPor { get; set; }
    }
}
