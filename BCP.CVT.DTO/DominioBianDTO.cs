using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class DominioBianDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }
        public string Correo { get; set; }
        public string NombreResponsable { get; set; }
        public int AreaBIANId { get; set; }

        //NotMapped
        public int EntidadRelacionId { get; set; }
        public int MantenimientoId { get; set; }
        public int Nivel { get; set; }

        //NotMapped
        public bool IsLastLevel => true;
    }
}
