using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class NotificacionResponsableAplicacionDto : BaseDTO
    {

        public int PortafolioResponsableId { get; set; }
        public string BuzonSalida { get; set; }
        public string ConCopia { get; set; }
        public DateTime FechaInicio { get; set; }
        public int HoraEnvio { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public bool FlagEnvio { get; set; }
        public bool? FlagNotificacionTTLJdE { get; set; }
        public string Matricula { get; set; }
        public string Correo { get; set; }
        public int TotalAplicaciones { get; set; }
        public string Responsable { get; set; }

        public string Para { get; set; }
        public string TipoActivoInformacion { get; set; }


        public string FechaInicioStr => FechaInicio.ToString("dd/MM/yyyy");
        public string NombreUsuario { get; set; }
        public List<string> UsuariosConCopia { get; set; }
        public int TipoUsuario { get; set; }
    }
}
