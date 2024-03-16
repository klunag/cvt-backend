using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class DependenciasAppsDTO
    {
        public int ConfiguracionId { get; set; }
        public DateTime FechaUltProceso { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string CodigoAPT { get; set; }
        public string NombreAPT { get; set; }
        public bool FlagProceso { get; set; }
        public bool FlagBigData { get; set; }
        public int TotalFilas { get; set; }

        public string FechaUltProcesoStr
        {
            get
            {
                return FechaUltProceso.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }

    }

    public class EtiquetasDTO
    {
        public int EtiquetaId { get; set; }
        public string Descripcion { get; set; }
        public bool FlagDefault { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TotalFilas { get; set; }
        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }

    }
    public class TiposRelacionDTO
    {
        public int TipoRelacionId { get; set; }
        public string Descripcion { get; set; }
        public bool PorDefecto { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TotalFilas { get; set; }
        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }

        public bool ParaAplicaciones { get; set; }

    }
}
