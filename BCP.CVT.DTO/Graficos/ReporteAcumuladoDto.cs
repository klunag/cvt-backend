using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Graficos
{
    public class ReporteAcumuladoDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int Equipos { get; set; }
        public int Total { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaFin { get; set; }
       
    }
}
