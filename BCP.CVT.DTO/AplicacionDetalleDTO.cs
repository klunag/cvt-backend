using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionDetalleDTO : BaseDTO
    {
        public string MotivoCreacion { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public int EstadoSolicitudId { get; set; }
        public string PersonaSolicitud { get; set; }
        public string ModeloEntrega { get; set; }
        public string PlataformaBCP { get; set; }
        //public string EntidadResponsable { get; set; }
        public string EntidadUso { get; set; }
        public string Proveedor { get; set; }
        public string Ubicacion { get; set; }
        public string Infraestructura { get; set; }
        public string RutaRepositorio { get; set; }
        public string Contingencia { get; set; }
        public string MetodoAutenticacion { get; set; }
        public string MetodoAutorizacion { get; set; }
        public string AmbienteInstalacion { get; set; }
        public string GrupoServiceDesk { get; set; }
        public bool? FlagOOR { get; set; }
        public bool? FlagRatificaOOR { get; set; }
        public string AplicacionReemplazo { get; set; }
        public string TipoDesarrollo { get; set; }

        public string CodigoInterfaz { get; set; }
        public string InterfazApp { get; set; }
        public string NombreServidor { get; set; }
        public string CompatibilidadWindows { get; set; }
        public string CompatibilidadNavegador { get; set; }
        public string CompatibilidadHV { get; set; }
        public string InstaladaDesarrollo { get; set; }
        public string InstaladaCertificacion { get; set; }
        public string InstaladaProduccion { get; set; }
        public string GrupoTicketRemedy { get; set; }
        public string NCET { get; set; }
        public string NCLS { get; set; }
        public string NCG { get; set; }
        public string ResumenSeguridadInformacion { get; set; }
        public string ProcesoClave { get; set; }
        public string Confidencialidad { get; set; }
        public string Integridad { get; set; }
        public string Disponibilidad { get; set; }
        public string Privacidad { get; set; }
        public string Clasificacion { get; set; }
        public string RoadmapPlanificado { get; set; }
        public string DetalleEstrategia { get; set; }
        public string EstadoRoadmap { get; set; }
        public string EtapaAtencion { get; set; }
        public string RoadmapEjecutado { get; set; }
        public string FechaInicioRoadmap { get; set; }
        public string FechaFinRoadmap { get; set; }
        public string CodigoAppReemplazo { get; set; }

        public string SWBase_SO { get; set; }
        public string SWBase_HP { get; set; }
        public string SWBase_LP { get; set; }
        public string SWBase_BD { get; set; }
        public string SWBase_Framework { get; set; }
        public string RET { get; set; }

        public int? EstadoId_SO { get; set; }
        public int? EstadoId_HP { get; set; }
        public int? EstadoId_LP { get; set; }
        public int? EstadoId_BD { get; set; }
        public int? EstadoId_FW { get; set; }

        public string CriticidadAplicacionBIA { get; set; }
        public string ProductoMasRepresentativo { get; set; }
        public string MenorRTO { get; set; }
        public string MayorGradoInterrupcion { get; set; }

        public bool? FlagFileCheckList { get; set; }
        public bool? FlagFileMatriz { get; set; }

        public string GestorAplicacionCTR { get; set; }
        public string ConsultorCTR { get; set; }
        public string ValorL_NC { get; set; }
        public string ValorM_NC { get; set; }
        public string ValorN_NC { get; set; }
        public string ValorPC_NC { get; set; }
        public string UnidadUsuario { get; set; }

        public string PersonaSolicitudStr => string.IsNullOrEmpty(PersonaSolicitud) ? UsuarioCreacion : PersonaSolicitud;
        public string FechaSolicitudStr => FechaSolicitud.HasValue ? FechaSolicitud.Value.ToString("dd/MM/yyyy") : FechaCreacionFormat;

        public string FlagRatificaOORStr => FlagRatificaOOR.HasValue ? (FlagRatificaOOR.Value ? "Si" : "No") : "-";
        public string FlagOORStr => FlagOOR.HasValue ? (FlagOOR.Value ? "Si" : "No") : "-";

        public string EstadoSolicitudStr
        {
            get
            {
                EEstadoSolicitudAplicacion? estadoSolicitud = Utilitarios.EnumToList<EEstadoSolicitudAplicacion>().Count(x => (int)x == EstadoSolicitudId) > 0 ? (EEstadoSolicitudAplicacion?)EstadoSolicitudId : null;
                return estadoSolicitud.HasValue ? Utilitarios.GetEnumDescription2((EEstadoSolicitudAplicacion)(EstadoSolicitudId)) : null;
            }
        }

    }
}