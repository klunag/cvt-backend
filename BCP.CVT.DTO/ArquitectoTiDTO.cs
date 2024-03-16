using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ArquitectoTiDTO: TreeElementDTO
    {
        public string Responsable { get; set; } //Matricula
        public string Correo { get; set; }
        public string NombreResponsable { get; set; }
        public bool? FlagValidarMatricula { get; set; }
        public string Matricula { get; set; }
        //NotMapped
        public bool IsLastLevel => true;
        public bool? FlagEncargadoJefatura { get; set; }
        public string Nombre { get; set; }
    }
}
