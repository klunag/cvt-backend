using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ParametroDTO : BaseDTO
    {
        //public int ParametroId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }

        public int? TipoParametroId { get; set; }
        public string TipoParametro { get; set; }

        public string FechaHoraModificacionFormato => FechaModificacion.HasValue ? FechaModificacion.Value.ToString("dd/MM/yyyy hh:mm:dd tt") : "-";

    }

    public class ParametroExpertos
    {
        public string Matricula { get; set; } 
        public string NombreExperto { get; set; }
        public string EmailExperto { get; set; }
        public int ProductoManagerId { get; set; }
    }

    public class MasterDetail
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string TipoId { get; set; }
        public bool Active { get; set; }
    }
}
