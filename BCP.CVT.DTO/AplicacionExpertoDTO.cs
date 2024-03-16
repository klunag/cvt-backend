using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionExpertoDTO: BaseDTO
    {
        public int? AplicacionExpertoId { get; set; }
        public string CodApp { get; set; }
        public string Matricula { get; set; }
        public string Nombres { get; set; }
        //public bool FlagEliminado { get; set; }
        public int? TipoExpertoId { get; set; }
        public string TipoExpertoToString { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
