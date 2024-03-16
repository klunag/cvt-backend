using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ProcesoVitalDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? FlagProcesoVital { get; set; }

        public string FlagProcesoVitalStr => FlagProcesoVital.HasValue ? FlagProcesoVital.Value ? "Si" : "No" : "-";
    }
}
