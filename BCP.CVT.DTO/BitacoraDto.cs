using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BitacoraDto : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string DetalleBitacora { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public string NombreAplicacion { get; set; }

        public string TipoActivoName { get; set; }

        public string NombreArchivo { get; set; }
    }
    public class BitacoraDetalleDto
    {
        public string DetalleBitacora { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class BitacoraDto2
    {
        public string CodigoAPT { get; set; }
        public string NombreAplicacion { get; set; }

        public string NombreAplicacionLimitado { get; set; }

        public string TipoActivoName { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public int? assetType { get; set; }
    }
}
