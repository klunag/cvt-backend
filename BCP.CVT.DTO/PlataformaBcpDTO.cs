using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class PlataformaBcpDTO: TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //NotMapped
        public bool IsLastLevel => true;
    }
}
