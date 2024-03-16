using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class PreguntaPaeDTO: TreeElementDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //NotMapped
        public bool IsLastLevel => true;
        public int Score { get; set; }
    }
}
