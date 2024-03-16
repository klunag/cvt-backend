using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class TipoDTO: BaseDTO
    {     
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int NumTecAsociadas { get; set; }
        public bool? FlagEstandar { get; set; }
        public bool? FlagMostrarEstado { get; set; }
    
        public string FlagEstandarToString => FlagEstandar.HasValue ? (FlagEstandar.Value ? "Sí" : "No") : "No";
    }
}
