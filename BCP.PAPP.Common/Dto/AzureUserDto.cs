using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Dto
{
    public class AzureUserDto
    {
        public string Matricula { get; set; }
        public string Nombres { get; set; }
        public int Estado { get; set; }
        public string Grupo { get; set; }
        public bool FlagActivo { get; set; }
        public bool FlagValidar { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
