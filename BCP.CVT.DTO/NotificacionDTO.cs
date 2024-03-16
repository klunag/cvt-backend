using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class NotificacionDTO : BaseDTO
    {
        public int TipoNotificacionId { get; set; }
        public string Nombre { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Cuerpo { get; set; }
        public string Asunto { get; set; }
        public bool? FlagEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string UsuarioNotificacion { get; set; }
        public string PasswordNotificacion { get; set; }
        public string Matricula { get; set; }
        public int NotificacionResponsableAplicacionId { get; set; }

        public string TipoNotificacionToString => Utilitarios.GetEnumDescription2((ETipoNotificacion) TipoNotificacionId);
        public string FlagEnviadoToString => FlagEnviado.HasValue? (FlagEnviado.Value? "Enviado" : "No enviado") : string.Empty;
        public string FechaEnvioToString => FechaEnvio.HasValue? FechaEnvio.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public List<string> ParaList => Para.Contains(";") ? Para.Split(';').ToList<string>() : new List<string> { Para };
        public List<string> CCList => string.IsNullOrEmpty(CC) ? null : CC.Contains(";") ? CC.Split(';').ToList<string>() : new List<string> { CC };

    }

    public class TipoNotificacionDto : BaseDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string BuzonSalida { get; set; }
        public string Para { get; set; }
        public string ConCopia { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public int? Frecuencia { get; set; }
        public string CuerpoAlternativo { get; set; }

        public DateTime? FechaInicio { get; set; }
        public int? HoraEnvio { get; set; }
        public DateTime? FechaUltimoEnvio { get; set; }

        public string FechaInicioStr => this.FechaInicio.HasValue ? this.FechaInicio.Value.ToString("dd/MM/yyyy") : "-";
        public string FechaUltimoEnvioFormato => this.FechaUltimoEnvio.HasValue ? this.FechaUltimoEnvio.Value.ToString("dd/MM/yyyy hh:mm:dd tt") : "-";
        public string FrecuenciaToString => (Frecuencia.HasValue ? Utilitarios.GetEnumDescription2((ETipoFrecuencia)Frecuencia.Value) : "-");
        public bool? Solicitante { get; set; }
        public bool? ArquitectoNegocio { get; set; }
        public bool? ATI { get; set; }
        public bool? LiderUsuario { get; set; }
        public bool? TL_TTL { get; set; }
        public bool? DevSec { get; set; }
        public bool? JefeEquipo { get; set; }
        public bool? UsuarioAutorizador { get; set; }
        public bool? Brokerr { get; set; }
        public bool? GobiernoIT { get; set; }
        public bool? AIO { get; set; }
        public bool? ArquitectoSolucion { get; set; }
    }

    public class NotificacionAplicacionDTO : BaseDTO
    {
        public int TipoNotificacionId { get; set; }
        public int SolicitudId { get; set; }
        public string Nombre { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Cuerpo { get; set; }
        public string Asunto { get; set; }
        public bool? FlagEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string UsuarioNotificacion { get; set; }
        public string PasswordNotificacion { get; set; }
        public string Matricula { get; set; }
        public int NotificacionResponsableAplicacionId { get; set; }

        public string Comentarios { get; set; }
        public int BandejaId { get; set; }
        public string CodigoAPT { get; set; }

        public string TipoNotificacionToString => Utilitarios.GetEnumDescription2((ETipoNotificacionAplicacion)TipoNotificacionId);
        public string FlagEnviadoToString => FlagEnviado.HasValue ? (FlagEnviado.Value ? "Enviado" : "No enviado") : string.Empty;
        public string FechaEnvioToString => FechaEnvio.HasValue ? FechaEnvio.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string ParaStr => !string.IsNullOrEmpty(Para) ? Para.Replace(";", "<br />") : "-";

        public List<string> ParaList => Para.Contains(";") ? Para.Split(';').ToList<string>() : new List<string> { Para };
        public List<string> CCList => string.IsNullOrEmpty(CC) ? null : CC.Contains(";") ? CC.Split(';').ToList<string>() : new List<string> { CC };
    }

    public class NotificacionExpertosDTO : BaseDTO
    {
        public string nombre { get; set; }
        public string perfiles { get; set; }
        public string correo { get; set; } 
    }
    
	public class TribuCoesDto
    {
        public string applicationId { get; set; }
        public string GestionadoPor { get; set; }
        public string TTL { get; set; }
        public string TL { get; set; }
        public string EmailTTL { get; set; }
        public string EmailTL { get; set; }
        public string Owner { get; set; }
        public string Gestores { get; set; }
        public string Expertos { get; set; }
        public string GestorRiesgo { get; set; }
    }

    public class OwnersAplicacionDto
    {
        public string Owner { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }

    public class OwnersDetalleDto
    {
        public string Gestores { get; set; }
        public string Expertos { get; set; }
        public string JefeEquipo { get; set; }
        public string Broker { get; set; }
        public string GestorRiesgo { get; set; }
    }
}
