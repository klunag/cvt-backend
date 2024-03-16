using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ActivoDetalle
        {
            get
            {
                return (Activo ? "Activo" : "Inactivo");
            }
        }
        public string FechaCreacionFormato
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
                //return FechaCreacion != null ? FechaCreacion.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaCreacionStr
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string FechaModificacionFormato
        {
            get
            {
                return FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
            }
        }

        public string FechaCreacionFormat => FechaCreacion.ToString("dd/MM/yyyy");

        public bool? FlagEliminado { get; set; }

    }
}
