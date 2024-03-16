using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class HistoricoExclusionDTO: BaseDTO
    {
        public int EquipoId { get; set; }
        public string EquipoStr { get; set; }

        public int TipoExclusionId { get; set; }
        public string TipoExclusionStr { get; set; }

        public string MotivoExclusion { get; set; }
    }
}
