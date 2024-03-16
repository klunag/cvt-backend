using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class ResponsableAplicacionDto
    {
        public string CodigoAPT { get; set; }
        public string MatriculaBroker { get; set; }
        public string MatriculaExperto { get; set; }
        public string MatriculaGestor { get; set; }
        public string MatriculaJDE { get; set; }
        public string MatriculaOwner { get; set; }
        public string MatriculaTTL { get; set; }
    }

    public class ResponsableDto
    {
        public string CodigoAPT { get; set; }
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public string Correo { get; set; }
    }
}
