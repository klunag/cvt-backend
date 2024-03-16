using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class DominioDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreEquipo { get; set; }
        public string MatriculaDueno { get; set; }
        public bool CalculoObs { get; set; }
        public int NumSubdominios { get; set; } 
    }
}
