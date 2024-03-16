using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class DatosAdicionalesEquiposDto
    {
        public string Nombre { get; set; }
        public string TipoEquipo { get; set; }
        public string Hostname { get; set; }
        public string Domain { get; set; }
        public string ManufacturerName { get; set; }
        public string Model { get; set; }
        public string IP { get; set; }
    }
}
