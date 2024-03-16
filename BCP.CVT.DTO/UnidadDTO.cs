using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class UnidadDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Responsable { get; set; }
        public string ResponsableMatricula { get; set; }
        public string ResponsableCorreo { get; set; }
        public string CodigoSIGA { get; set; }

        public string label { get; set; }

        public string mail { get; set; }

        public string matricula { get; set; }

        public string displayName { get; set; }

        public int id { get; set; }



        public string value { get; set; }

        public string Unidad { get; set; }

        public string Situacion { get; set; }
        public bool FlagEditar { get; set; }
        //NotMapped
        public int MantenimientoId { get; set; }
        public int EntidadRelacionId { get; set; }
        public int Nivel { get; set; }
        public bool IsSelected => false;
    }
}
