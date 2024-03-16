using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TreeElementDTO: BaseDTO
    {
        public int MantenimientoId { get; set; }
        public int EntidadRelacionId { get; set; }
        public int Nivel { get; set; }
        public bool IsSelected => false;
    }
}
