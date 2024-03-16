using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Graficos
{
    public class ReportePrincipalDTO
    {
        public int TotalServidores { get; set; }
        public int TotalServidoresObsoletos { get; set; }
        //public int AreaId { get; set; }
        public string Nombre { get; set; }
        public int Porcentaje
        {
            get
            {
                var resultado = (TotalServidores == 0) ? 0 : (int)Math.Round(((decimal)TotalServidoresObsoletos / (decimal)TotalServidores) * 100, 0);
                return resultado;
            }
        }
    }
}
