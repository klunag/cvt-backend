using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AuditoriaTipoActivoDTO: BaseDTO
    {
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public string Campo { get; set; }
        public string Usuario { get; set; }

        public string AccionStr
        {
            get
            {
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
