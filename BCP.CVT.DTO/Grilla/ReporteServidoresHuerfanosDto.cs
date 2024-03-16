using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Grilla
{
    public class ReporteServidoresHuerfanosDto
    {
        public string Equipo { get; set; }
        public string TipoEquipo { get; set; }
        public string SistemaOperativo { get; set; }
        public string UltimaActualizacion { get; set; }
        public int TecnologiasInstaladas { get; set; }
        public bool? FlagTemporal { get; set; }
        public string FlagTemporalToString => FlagTemporal.HasValue ? (FlagTemporal.Value ? "Carga manual" : "Descubrimiento automático") : "Descubrimiento automático";
        public int EquipoId { get; set; }
        public int TecnologiaId { get; set; }
        public string ClaveTecnologica { get; set; }
        public int TotalFilas { get; set; }
    }
}
