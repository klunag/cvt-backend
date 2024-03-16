using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ArchivosCvtDTO : BaseDTO
    {
        public int TablaProcedenciaId { get; set; }
        public string EntidadId { get; set; }
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }

        public string TablaProcedenciaStr { get; set; }
        public string NombreRef { get; set; }
        public string DescripcionRef { get; set; }
        public int? AppId { get; set; }
        public int? SolId { get; set; }


        public string Matricula { get; set; }
        public string NombresCompletos { get; set; }
    }
}
