using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionApisDTO //: BaseDTO
    {
        public string Id { get; set; }
        public string nombre { get; set; }
        public string gestionadoPor { get; set; }
        public string squad { get; set; }
        public string ownerLiderUsuario { get; set; }
        public string expertoLidertecnico { get; set; }
        public string aplicacionOwner { get; set; }
        public int PuedeGestionar { get; set; }
        public bool FlagComponenteCross { get; set; }
        public int TotalRows { get; set; }
        public int gestorQa { get; set; }

    }
}
