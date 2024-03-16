
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionVinculadaDTO: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string VinculoCodigoAPT { get; set; }
        public int EquipoId { get; set; }
        public string DetalleVinculo { get; set; }

        public string CodigoAPTStr { get; set; }
        public string VinculoCodigoAPTStr { get; set; }
        public string EquipoStr { get; set; }
    }

    public class AplicacionRelacionDTO
    {
        public int Id { get; set; }
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string TipoActivo { get; set; }

        public string NombreCompleto => $"{CodigoAPT} - {Nombre}";
    }

    public class AplicacionTecnologiaDTO: BaseDTO
    {
        public string CodigoAPTArr { get; set; }
        public int TecnologiaId { get; set; }
    }
}
