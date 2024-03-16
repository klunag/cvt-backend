using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ComentarioEliminacionRelacionDTO: BaseDTO
    {
        public int RelacionId { get; set; }
        public string Contenido { get; set; }
    }
}
