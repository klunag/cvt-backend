using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ArquetipoDTO: BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int NumTecAsociadas { get; set; }
        public int? EstadoAprob { get; set; }
        //public string NombreDiag { get; set; }
        //public string DirFisicaDiag { get; set; }
        //public HttpPostedFile DiagArqFile { get; set; }
        public int? ArchivoId { get; set; }
        public string ArchivoStr { get; set; }

        public int EntornoId { get; set; }
        public int TipoArquetipoId { get; set; }
        public string Codigo { get; set; }
        public bool Automatizado { get; set; }

        public string TipoArquetipoStr { get; set; }
        public string EntornoStr { get; set; }
    }
}
