using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class SolicitudDto : BaseDTO
    {

        public DateTime? FechaRechazo { get; set; }

        public string NombreRechazo { get; set; }

        public int TipoSolicitud { get; set; }
         
        public int? assetType { get; set; }
        public int AplicacionId { get; set; }
        public int EstadoSolicitud { get; set; }
        public string Observaciones { get; set; }

        public string ObservacionesRechazo { get; set; }

        public string NombreSolicitante { get; set; }

        public string ObservacionesAprobacion { get; set; }
        public string Comentarios { get; set; }
        public string Respuesta { get; set; }
        public byte[] ArchivosAsociados { get; set; }

        public byte[] ConformidadGSTFile { get; set; }
        public byte[] TicketEliminacionFile { get; set; }
        public byte[] RatificacionFile { get; set; }

        public string NombreArchivos { get; set; }
        public bool? FlagAprobacion { get; set; }

        public int? TipoEliminacion { get;set; }

        public string ExpertoNombre { get; set; }

        public string Solicitante { get; set; }
        public bool? FlagRequiereConformidad { get; set; }


        //Adicionales
        public string CodigoAplicacion { get; set; }
        public string NombreAplicacion { get; set; }
        public string TipoSolicitudToString => Utilitarios.GetEnumDescription2((TipoSolicitud)TipoSolicitud);
        public string EstadoSolicitudToString => Utilitarios.GetEnumDescription2((EstadoSolicitud)EstadoSolicitud);

        public List<SolicitudCampoModificadoDTO> AtributoDetalle { get; set; }
        public List<SolicitudCampoModificadoDTO> ModuloDetalle { get; set; }
        public List<int> RemoveAtributoIds { get; set; }
        public List<int> RemoveAtributoModuloIds { get; set; }

        public bool? FlagAprobadorBandeja { get; set; }
        public int? EstadoAprobacionBandeja { get; set; }

        public string TicketEliminacion { get; set; }
        public string TicketConformidadRatificacion { get; set; }
        public string ArquitectoTI { get; set; }

        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }
        public string DescripcionAplicacion { get; set; }

        public string AreaBian { get; set; }
        public string DominioBian { get; set; }
        public string JefaturaATI { get; set; }
        public string PlataformaBCP { get; set; }

        public string ModeloEntrega { get; set; }

        public string LiderUsuario_PO { get; set; }
        public string UsuarioAutorizador_PO { get; set; }
        public string Experto_Especialista { get; set; }
        public string JefeEquipo_PO { get; set; }
        public string TTL { get; set; }

        public string GestionadoPor { get; set; }

        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public string TipoActivoInformacion { get; set; }

        public string NombreUsuarioAprobacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string FechaAprobacionFormato
        {
            get
            {
                return FechaAprobacion.HasValue ? FechaAprobacion.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
            }
        }


        public string NombreUsuarioModificacion { get; set; }

        public string Email { get; set; }
        public string NombreUsuario
        {
            get
            {
                if (this.EstadoSolicitud == 1)
                    return string.IsNullOrWhiteSpace(NombreUsuarioAprobacion) ? NombreUsuarioModificacion : NombreUsuarioAprobacion;
                else if (this.EstadoSolicitud == 2)
                    return string.Empty;
                else if (this.EstadoSolicitud == 3)
                    return string.IsNullOrWhiteSpace(NombreUsuarioAprobacion) ? NombreUsuarioModificacion : NombreUsuarioAprobacion;
                else if (this.EstadoSolicitud == 4)
                    return string.Empty;                
                else
                    return NombreUsuarioModificacion;                
            }
        }
   

        public List<DataAprobadorAplicacion> AprobadorDetalle { get; set; }

        //public string FlagAprobadorBandejaStr => FlagAprobadorBandeja.HasValue ? FlagAprobadorBandeja.Value ? "Aprobado" : "Desaprobado" : "No revisado";
        public string EstadoAprobacionBandejaStr => EstadoAprobacionBandeja.HasValue ? Utilitarios.GetEnumDescription2((EEstadoSolicitudAplicacion)EstadoAprobacionBandeja.Value) : "";
        public string EstadoFakeSolicitudPortafolioStr => FlagAprobadorBandeja.HasValue || EstadoSolicitud == (int)EEstadoSolicitudAplicacion.Observado ? "En proceso de revisión" : EstadoSolicitudToString;

        public string NombreConformidadGST { get; set; }
        public string NombreTicketEliminacion { get; set; }
        public string NombreRatificacion { get; set; }
        public DateTime? dateApproved { get; set; }
        public DateTime? dateRejected { get; set; }
        public DateTime? dateTransfer { get; set; }
        public string dateApprovedStr => dateApproved.HasValue ? dateApproved.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateRejectedStr => dateRejected.HasValue ? dateRejected.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string dateTransferStr => dateTransfer.HasValue ? dateTransfer.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;

        public string Matricula { get; set; }
        public string Usuario { get; set; }
        public string UserName { get; set; }
        public string EstadoSolicitudEliminacion
        {
            get
            {
                switch (this.EstadoSolicitud)
                {                                         
                    case 4:
                        return  "Pendiente Owner / Líder usuario";                        
                    default:
                        return Utilitarios.GetEnumDescription2((EstadoSolicitud)this.EstadoSolicitud);
                }
            }
        }
    }

    public class SolicitudDetalleDto
    {
        public int SolicitudId { get; set; }

        public int? ColumnaId { get; set; }
        public string ColumnaDetalle { get; set; }
        public string ValorAnterior { get; set; }
        public string NuevoValor { get; set; }
        public int Campo { get; set; }

        public string DetalleActual { get; set; }
        public string DetalleNuevo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int TipoRegistro { get; set; }

        public string Observaciones { get; set; }

        public string FechaCreacionToString
        {
            get
            {
                return FechaCreacion.HasValue ? FechaCreacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
            }
        }

        public string TipoRegistroDetalle
        {
            get
            {
                if (TipoRegistro != 0)
                    return Utilitarios.GetEnumDescription2((Flow)TipoRegistro);
                else
                    return string.Empty;
            }
        }

        public int ConformidadGST { get; set; }
        public int TicketEliminacion { get; set; }
        public int Ratificacion { get; set; }

        public string NombreConformidadGST { get; set; }
        public string NombreTicketEliminacion { get; set; }
        public string NombreRatificacion { get; set; }
    }
}
