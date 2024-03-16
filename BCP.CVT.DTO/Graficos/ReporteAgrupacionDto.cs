using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Graficos
{
    public class ReporteAgrupacionDto
    {
        public string Agrupacion { get; set; }
        public int Obsoletos { get; set; }
        public int NoObsoletos { get; set; }
    }

    public class ReporteAgrupacionDetalleDto
    {
        public string CodigoApt { get; set; }
        public string Nombre { get; set; }
        public string Agrupacion { get; set; }
        public string EstadoAplicacion { get; set; }
        public string ClaveTecnologia { get; set; }
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public DateTime? FechaCalculoBase { get; set; }
        public string FechaCalculoBaseToString
        {
            get
            {
                return FechaCalculoBase.HasValue ? FechaCalculoBase.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }
        public int Obsoleto { get; set; }
        public int ObsoletoIndicador
        {
            get
            {
                return Obsoleto == 1 ? -1 : 1;
            }
        }
    }

    public class ReporteEvolucionInstalacionTecnologiasDto
    {
        public int NroSemana { get; set; }
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
    }

}
