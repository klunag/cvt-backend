using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class GerenciaDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }
        public string ResponsableMatricula { get; set; }
        public string ResponsableCorreo { get; set; }
        public string CodigoSIGA { get; set; }
        public bool FlagEditar { get; set; }
        //NotMapped
        public int MantenimientoId { get; set; }
        public int EntidadRelacionId { get; set; }
        public int Nivel { get; set; }
        public bool IsSelected => false;
    }
}
