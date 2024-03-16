using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AuthAplicacionDTO : BaseDTO
    {
        public int AuthAplicacionId { get; set; }
        public string CodigoAplicacion { get; set; }
        public string ApiKey { get; set; } 
    }
}
