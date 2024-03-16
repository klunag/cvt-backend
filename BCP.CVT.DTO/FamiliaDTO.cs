using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class FamiliaDTO : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int NumTecAsociadas { get; set; }

        public DateTime? FechaFinSoporte { get; set; }
        public DateTime? FechaExtendida { get; set; }
        public DateTime? FechaInterna { get; set; }

        public int? Existencia { get; set; }
        public int? Facilidad { get; set; }
        public int? Riesgo { get; set; }
        public decimal? Vulnerabilidad { get; set; }
        public string Fabricante { get; set; }

        public string FechaFinSoporteToString
        {
            get
            {
                return FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string FechaExtendidaToString
        {
            get
            {
                return FechaExtendida.HasValue ? FechaExtendida.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public string FechaInternaToString
        {
            get
            {
                return FechaInterna.HasValue ? FechaInterna.Value.ToString("dd/MM/yyyy") : "";
            }
        }
    }
}
