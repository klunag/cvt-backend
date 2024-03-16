using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ConfiguracionDTO: BaseDTO
    {
        public string Parametro { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
}
