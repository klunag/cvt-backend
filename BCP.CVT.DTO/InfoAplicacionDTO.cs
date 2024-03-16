using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class InfoAplicacionDTO : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string NombreAPT { get; set; }
        public DateTime? FechaValida { get; set; }
        public string UsuarioValida { get; set; } 
        public string gestionadopor { get; set; }
        public string TTL { get; set; }
        public string TL { get; set; }
        public string owner { get; set; }
        public string PO { get; set; }
        public string experto { get; set; }
        public string jefe { get; set; }
    }

    public class DetalleInforAplicacionNivel0 : BaseDTO
    {
        public int idSubdominio { get; set; }
        public string subdominio { get; set; }
        public int cantidad { get; set; }
        public string tipoComponente { get; set; }
        public string nombreComponente { get; set; }
        public string ambiente { get; set; }
        public string tecnologia { get; set; }
    }

    public class DetalleTotalNiveles : BaseDTO
    {
        public int idSubdominio { get; set; }
        public string subdominio { get; set; }
        public int cantidad { get; set; }
        public List<DetalleInforAplicacionNivel1> detalle { get; set; }
    }

    public class DetalleInforAplicacionNivel1 : BaseDTO
    {
        public string tipoComponente { get; set; }
        public string nombreComponente { get; set; }
        public string ambiente { get; set; }
        public string tecnologia { get; set; }
    }
}
