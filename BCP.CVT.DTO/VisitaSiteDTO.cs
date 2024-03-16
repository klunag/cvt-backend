using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class VisitaSiteDTO: BaseDTO
    {
        public long VisitaSiteId { get; set; }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string UrlSite { get; set; }
    }
}
