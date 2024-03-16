using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.PAPP.Common.Cross;

namespace BCP.CVT.DTO
{
    public class SolicitudFuncionDTO : BaseDTO
    {
        public int? EstadoSolicitud { get; set; }
        public string EstadoSolicitudStr
        {
            get
            {

                return Utilitarios.GetEnumDescription2((EstadoSolicitudMDR)EstadoSolicitud);
            }
        }
        public int? TipoSolicitud { get; set; }
        public string TipoSolicitudStr
        {
            get
            {
                if (TipoSolicitud != null)
                    return Utilitarios.GetEnumDescription2((TipoAsignacionMDR)TipoSolicitud);
                else return null;
            }
        }
        public DateTime? FechaRegistro { get; set; }
        public string FechaRegistroStr => FechaRegistro.HasValue ? FechaRegistro.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string RevisadoPor { get; set; }
        public string NombreResponsable { get; set; }

        public DateTime? FechaRevision { get; set; }
        public string FechaRevisionStr => FechaRevision.HasValue ? FechaRevision.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string RolCVT { get; set; }

        public int? RolId { get; set; }

        public string RolSeguridad { get; set; }

        public string Tribu { get; set; }

        public string Chapter { get; set; }

        public string Funcion { get; set; }

        public int? ProductoId { get; set; }

        public string Descripcion { get; set; }

        public string GrupoRed { get; set; }

        public string Producto { get; set; }

        public string Rol { get; set; }

        public string Comentario { get; set; }
    }
}
