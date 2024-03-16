using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ProductoArquetipoDTO : BaseDTO
    {
        public int ProductoId { get; set; }
        public int ArquetipoId { get; set; }
        public ArquetipoDTO Arquetipo { get; set; }
    }
}
