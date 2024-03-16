using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class HistorialModificacionDTO : BaseDTO
    {
        public string Entidad { get; set; }
        public string IdRegistro { get; set; }
        public string EntidadAsociado { get; set; }
        public string IdRegistroAsociado { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
    }
}
