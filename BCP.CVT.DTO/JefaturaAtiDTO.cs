using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class JefaturaAtiDTO: TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }

        public string Correo { get; set; }
        public string NombreResponsable { get; set; }
    }
}
