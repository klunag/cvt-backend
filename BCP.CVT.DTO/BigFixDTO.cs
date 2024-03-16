using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BigFixDTO : BaseDTO
    {
        public string GeneradoPor { get; set; }
        public DateTime FechaPeticion { get; set; }
        public string Servidor { get; set; }
        public string Ruta { get; set; }
        public string Email { get; set; }
        public bool FlagProcesado { get; set; }
        public string IdBigFix { get; set; }
        public string FlagProcesadoToString
        {
            get
            {
                return FlagProcesado ? "Procesado" : "Pendiente de procesamiento";
            }
        }
        public string FechaPeticionToString
        {
            get
            {
                return FechaPeticion.ToString("dd/MM/yyyy hh:mm:ss");
            }
        }
    }
}
