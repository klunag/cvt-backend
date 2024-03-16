using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SiemDTO
    {
        public string servidor { get; set; }
        public string ambiente { get; set; }
		public string ambienter { get; set; }
		public string aplicacion { get; set; }
		public string nombreaplicacion { get; set; }
		public string gestionadopor { get; set; }
        public string subsidiaria { get; set; }
		public string rolLider { get; set; }
        public string rolTTL { get; set; }
        public string rolUsuario { get; set; }
        public string rolExperto { get; set; }
        public string ip { get; set; } 
    }
}