using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class RelacionDetalleDTO : BaseDTO
    {
        public long RelacionDetalleId { get; set; }
        public long RelacionId { get; set; }
        public RelacionDTO Relacion { get; set; }
        public int TecnologiaId { get; set; }
        public int RelevanciaId { get; set; }

        public bool FlagSeleccionado { get; set; }

        public string TecnologiaStr { get; set; }

        public string RelevanciaStr { get; set; }

        public string CodigoAPTVinculoStr { get; set; }
        public string CodigoAPTVinculo { get; set; }
        public string DetalleVinculo { get; set; }
        public string Componente { get; set; }

        public int? SubdominioId { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string TipoTecnologia { get; set; }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteStr => FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
    }
}
