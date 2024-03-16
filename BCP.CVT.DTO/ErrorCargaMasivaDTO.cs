using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ErrorCargaMasivaDTO: BaseDTO
    {
        public int? TipoErrorId { get; set; }
        public string Detalle { get; set; }
        public int? FilaExcel { get; set; }

        public string TipoErrorIdStr => TipoErrorId.HasValue ? Utilitarios.GetEnumDescription2((ETipoErrorCargaMasiva)TipoErrorId.Value) : "-";
    }
}
