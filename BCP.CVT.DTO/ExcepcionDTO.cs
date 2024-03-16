using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class ExcepcionDTO : BaseDTO
    {
        public int TipoExcepcionId { get; set; }
        public string CodigoAPT { get; set; }
        public int TecnologiaId { get; set; }
        //public int EquipoId { get; set; }
        public int? TipoRiesgoId { get; set; }
        public DateTime? FechaFinExcepcion { get; set; }
        public string UrlInformacion { get; set; }
        public string Comentario { get; set; }

        public string TipoRiesgoStr { get { return TipoRiesgoId.HasValue ? Utilitarios.GetEnumDescription2((ETipoRiesgo)(TipoRiesgoId)) : ""; } }
        public string AplicacionStr { get; set; }
        public string TecnologiaStr { get; set; }
        public string DominioStr { get; set; }
        public string SubdominioStr { get; set; }
        public string FechaFinExcepcionFormato { get { return FechaFinExcepcion.HasValue ? FechaFinExcepcion.Value.ToString("dd/MM/yyyy") : ""; } }
        public int? ArchivoId { get; set; }
        public string ArchivoStr { get; set; }

        public int EquipoId { get; set; }
        public string EquipoStr { get; set; }
        public string NombreAplicacion { get; set; }
        public string NombreAplicacionCompleto
        {
            get { return CodigoAPT != null && NombreAplicacion != null ? CodigoAPT + "-" + NombreAplicacion : ""; }
        }
    }
}
