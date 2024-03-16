using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class MotivoDTO : BaseDTO
    {
        public int MotivoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CantidadTecnologiasAsociadas { get; set; }
    }
}
