using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AuditoriaDataDTO: BaseDTO
    {
        public int TotalFilas { get; set; }
        public string Entidad { get; set; }
        public string EntidadClave { get; set; }
        public string EntidadId { get; set; }
        public string CreadoPor { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaOrden { get; set; }

        public string FechaOrdenFormato
        {
            get
            {
                return FechaOrden.HasValue ? FechaOrden.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;                
            }
        }

        public string Accion { get; set; }        
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }        
        //public bool FlagEliminado { get; set; }
        public string Campo { get; set; }
        public string Identificador { get; set; }

        public string AccionStr {
            get {
                var str = string.Empty;
                switch (Accion)
                {
                    case "I":
                        str = "Inserción (I)";
                        break;
                    case "U":
                        str = "Actualización (U)";
                        break;
                }
                return str;
            }
        }
    }
}
