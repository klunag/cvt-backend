using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AutorizadorDTO: BaseDTO
    {
        public int TecnologiaId { get; set; }
        public string AutorizadorId { get; set; }
        public string Matricula { get; set; }
        public string MatriculaBanco { get; set; }
        public string Nombres { get; set; }
    }
}
