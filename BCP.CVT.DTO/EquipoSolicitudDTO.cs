using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class EquipoSolicitudDTO : BaseDTO
    {
        public int? EquipoId { get; set; }
        public int EstadoSolicitud { get; set; }
        public string EstadoSolicitudToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((EstadoSolicitudActivosTSI)EstadoSolicitud);
            }
        }
        public string NombreUsuarioCreacion { get; set; }
        public string Comentarios { get; set; }
        public byte[] ArchivoConstancia { get; set; }
        public bool TieneArchivo
        {
            get
            {
                return (ArchivoConstancia != null);
            }
        }
        public string NombreUsuarioModificacion { get; set; }
        public DateTime? FechaAprobacionRechazo { get; set; }
        public string FechaAprobacionRechazoToString
        {
            get
            {
                return FechaAprobacionRechazo.HasValue ? FechaAprobacionRechazo.Value.ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
            }
        }
        public string AprobadoRechazadoPor { get; set; }
        public string NombreUsuarioAprobadoRechazo { get; set; }
        public int TipoEquipoActual { get; set; }
        public string TipoEquipoActualToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((ETipoEquipo)TipoEquipoActual);
            }
        }
        public int TipoEquipoSolicitado { get; set; }
        public string TipoEquipoSolicitadoToString
        {
            get
            {
                return Utilitarios.GetEnumDescription2((ETipoEquipo)TipoEquipoSolicitado);
            }
        }
        public DateTime? FechaFinSoporte { get; set; }
        public string FechaFinSoporteToString
        {
            get
            {
                return FechaFinSoporte.HasValue ? FechaFinSoporte.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public string NombreEquipo { get; set; }
        public string ComentariosDesestimacion { get; set; }
        public string ComentariosAprobacionRechazo { get; set; }
        public string NombreArchivo { get; set; }
        public string CorreoSolicitante { get; set; }
        public string CodigoAPT { get; set; }
        public string UsuarioAsignado { get; set; }
        public bool? FlagSeguridad { get; set; }
        public bool? FlagRegistroEquipo { get; set; }
        public string Perfil { get; set; }
        public string UsuarioConsulta { get; set; }
        public bool ApruebaSolicitud
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Perfil))
                {
                    if (Perfil.Contains("E195_Administrador"))
                        return true;
                    else if (UsuarioAsignado == UsuarioConsulta)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }
        public string TipoSolicitud
        {
            get
            {
                if (!FlagRegistroEquipo.HasValue)
                    return "Cambio de equipo a Appliance";
                else
                {
                    if (FlagRegistroEquipo.Value)
                        return "Registro de Appliance";
                    else
                        return "Cambio de equipo a Appliance";
                }
            }
        }

        public DateTime? FechaAprobacionRechazoCVT { get; set; }
        public string FechaAprobacionRechazoCvtToString
        {
            get
            {
                return FechaAprobacionRechazoCVT.HasValue ? FechaAprobacionRechazoCVT.Value.ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
            }
        }
        public string AprobadoRechazadoPorCVT { get; set; }
        public string AprobadorMatricula { get; set; }
    }

    public class ValidacionSolicitudDTO
    {
        public bool Procesar { get; set; }
        public string Mensaje { get; set; }
    }
}
