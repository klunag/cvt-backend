using BCP.CVT.Cross;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.PAPP.Common.Custom
{
    public class ApplicationList
    {
        public int id { get; set; }
        public string applicationId { get; set; }
        public string description { get; set; }
        public string Rol { get; set; }
        public string name { get; set; }

        public string registerBy { get; set; }
        public string managedBy { get; set; }
        public string deliveryType { get; set; }

        public string assetTypeName { get; set; }
        public int? status { get; set; }

        public int gestionadoPorId { get; set; }
        public string statusDetail
        {
            get
            {
                return status.HasValue ? Utilitarios.GetEnumDescription2((ApplicationState)status.Value) : string.Empty;
            }
        }

      

        public DateTime? registerDate { get; set; }
        public DateTime? dateRegistrationSituationComplete { get; set; }
        public int? registrationSituation { get; set; }
        public string registrationSituationDetail 
        {
            get
            {
                return registrationSituation.HasValue ? registrationSituation == 1? "registro parcial" : "registro completo" : string.Empty;
            }
        }
        public bool? isApproved { get; set; }
        public bool? isObserved { get; set; }
        public string approvedDetail
        {
            get
            {
                return isApproved.HasValue ? (isApproved.Value ? "Aprobado" : "Por validar") : string.Empty;
            }
        }
        public string observedDetail
        {
            get
            {
                return isObserved.HasValue ? (isObserved.Value ? "Observado" : "") : string.Empty;
            }
        }

        public string commentDetail
        {
            get
            {
                return isApproved.HasValue && isApproved.Value ? approvedDetail : isObserved.HasValue && isObserved.Value ? observedDetail :  "Por validar";
            }
        }
        public DateTime? approvedDate { get; set; }
        
        public string approvedDateStr => approvedDate.HasValue ? approvedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty;
        public string tipoactivo { get; set; }

        public string solicitante { get; set; }

        public bool? aplicacionRevertida { get; set; }


        public DateTime? eliminacionFecha { get; set; }
        public string eliminacionUsuario { get; set; }

        public int TotalSolicitudesActivas { get; set; }

        public string OwnerEmail { get; set; }        
        public string Owner { get; set; }
        public string Motivo { get; set; }
    }

    public class SolicitudList {

        public DateTime? FechaRechazo { get; set; }

        public string NombreRechazo { get; set; }
        public int SolicitudAplicacionId { get; set; }
        public int TipoSolicitud { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public int AplicacionId { get; set; }
        public int EstadoSolicitud { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Observaciones { get; set; }
        public Nullable<bool> FlagAprobacion { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoSolicitudName { get; set; }
        public string TipoSolicitudName { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationId { get; set; }
        public int? TipoActivoId { get; set; }
        public string TipoActivo { get; set; }
        public string GestionadoPor { get; set; }

        public string ObservacionesRechazo { get; set; }

        public string NombreUsuarioAprobacion { get; set; }
        public string NombreUsuarioModificacion { get; set; }

        public Nullable<System.DateTime> FechaAprobacion { get; set; }
    }
}
