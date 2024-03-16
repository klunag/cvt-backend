using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SolicitudCampoModificadoDTO: BaseDTO
    {
        public int AplicacionId { get; set; }
        public int SolicitudAplicacionId { get; set; }
        public int AtributoAplicacionId { get; set; }
        public int InfoCampoId { get; set; }
        public string AtributoAplicacionStr { get; set; }
        public string ValorActual { get; set; }
        public string ValorNuevo { get; set; }
        public string AtributoBD { get; set; }
        public int TablaProcedencia { get; set; }

        public int AtributoModuloId { get; set; }
        public string AtributoModuloStr { get; set; }
        public int ModuloId { get; set; }
        public string ModuloStr { get; set; }
    }
}
