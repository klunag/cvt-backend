using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class AplicacionDTO : BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string CodigoAPTStr { get; set; }
        public string Nombre { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }

        public string Area { get; set; }
        public string Unidad { get; set; }
        public string DescripcionAplicacion { get; set; }
        public string EstadoAplicacion { get; set; }
        public string EstadoAplicacionProcedencia { get; set; } //-- ESTADO EXCEL EstadoAplicacionId INT REFERENCES EstadoAplicacion(EstadoAplicacionId) NULL
        public DateTime? FechaRegistroProcedencia { get; set; }
        public DateTime? FechaCreacionProcedencia { get; set; }

        public string FechaEliminacion { get; set; }

        public string TipoEliminacion { get; set; }
        public string UsuarioEliminacion { get; set; }

        public string FechaReactivacion { get; set; }

        public string EstadoReactivacion { get; set; }

        public string NombreSolicitanteReactivacion { get; set; }

        public string FechaRegistroMasivo { get; set; }
        public string FechaCreacionMasivo { get; set; }

        public int SolicitudId { get; set; }
        public string MotivoComentario { get; set; }

        public string AreaBIAN { get; set; }
        public string DominioBIAN { get; set; }
        public string JefaturaATI { get; set; }

        public string NombreEquipo_Squad { get; set; }
        public string GestionadoPor { get; set; }
        public string EntidadResponsable { get; set; } // COLUMNA Y BCP


        public int? CriticidadId { get; set; } // Criticidad
        public string CategoriaTecnologica { get; set; }
        public int? RoadMapId { get; set; }//RoadMap
        public string RoadMapToString { get; set; }//RoadMap

        public bool FlagActivoProcedencia { get; set; }

        public string Matricula { get; set; }
        public int? Obsolescente { get; set; }

        public string MesAnio { get; set; }

        public string PCI { get; set; }
        public string ListaPCI { get; set; }
        public bool? FlagRelacionar { get; set; }

        public string FlagRelacionarToString => FlagRelacionar.HasValue ? (FlagRelacionar.Value ? "Permitir" : "No permitir") : "No permitir";
        public string CriticidadToString { get; set; }

        public bool ValidarExcel { get; set; }
        public int TotalEquiposRelacionados { get; set; }


        public string FechaRegistroProcedenciaStr => FechaRegistroProcedencia.HasValue ? FechaRegistroProcedencia.Value.ToString("dd/MM/yyyy") : string.Empty;
        public string FechaCreacionProcedenciaStr => FechaCreacionProcedencia.HasValue ? FechaCreacionProcedencia.Value.ToString("dd/MM/yyyy") : string.Empty;

        public decimal? KPIObsolescencia { get; set; }
        public decimal? KPIObsolescenciaReal { get; set; }
        public decimal? KPIObsolescenciaProyeccion1 { get; set; }
        public decimal? KPIObsolescenciaProyeccion2 { get; set; }
        public string Equipo { get; set; }
        public string Ambiente { get; set; }
        public int EstadoRelacionId { get; set; }
        public string EstadoRelacionToString
        {
            get
            {
                EEstadoRelacion? estadoRelacion = (EEstadoRelacion?)EstadoRelacionId;
                return estadoRelacion.HasValue ? Utilitarios.GetEnumDescription2(estadoRelacion) : null;
            }
        }


        public string ResponsableMatricula { get; set; }
        public string ResponsableNombre { get; set; }
        public string ResponsableTipo { get; set; }
        public bool FlagEsAprobador
        {
            get
            {
                switch (TipoExperto)
                {
                    case (int)ETipoExperto.AnalistaCDS:
                        return false;
                    case (int)ETipoExperto.AprobadorCDS:
                        return false;
                    case (int)ETipoExperto.ChapterDesarrollo:
                        return false;
                    case (int)ETipoExperto.ChapterQA:
                        return false;
                    case (int)ETipoExperto.ExpertoAplicacion:
                        return true;
                    case (int)ETipoExperto.JefeCDS:
                        return false;
                    case (int)ETipoExperto.JefeEquipoPO:
                        return true;
                    case (int)ETipoExperto.LiderTecnico:
                        return false;
                    case (int)ETipoExperto.LiderTribuTTL:
                        return false;
                    case (int)ETipoExperto.LiderUsuario:
                        return true;
                    case (int)ETipoExperto.LiderUsuarioAutorizador:
                        return true;
                    case (int)ETipoExperto.Proveedor:
                        return false;
                    case (int)ETipoExperto.SupervidorBCP:
                        return false;
                    case (int)ETipoExperto.SupervidorProveedor:
                        return false;
                    default:
                        return false;
                }
            }
        }
        public int TipoExperto { get; set; }
        public string TipoExpertoToString { get; set; }
        public string ClaveTecnologia { get; set; }

        public string Owner_LiderUsuario_ProductOwner { get; set; }
        public string Gestor_UsuarioAutorizador_ProductOwner { get; set; }
        public string TribeTechnicalLead { get; set; }
        public string Experto_Especialista { get; set; }
        public string BrokerSistemas { get; set; }
        public string JefeEquipo_ExpertoAplicacionUserIT_ProductOwner { get; set; }

        //Asociacion de matriculas
        public string MatriculaOwner { get; set; }
        public string MatriculaGestor { get; set; }
        public string MatriculaTTL { get; set; }
        public string MatriculaExperto { get; set; }
        public string MatriculaBroker { get; set; }
        public string MatriculaJDE { get; set; }

        public int HdOwner { get; set; }
        public int HdGestor { get; set; }
        public int HdTTL { get; set; }
        public int HdExperto { get; set; }
        public int HdBroker { get; set; }
        public int HdJDE { get; set; }

        public int ApplicationId { get; set; }

        public AplicacionDetalleDTO AplicacionDetalle { get; set; }
        public List<ModuloAplicacionDTO> ModuloAplicacion { get; set; }

        public bool? FlagAprobado { get; set; }
        public string ClasificacionTecnica { get; set; }
        public string SubclasificacionTecnica { get; set; }

        //Campos carga masiva
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
        public string MotivoCreacion { get; set; }
        public string PlataformaBCP { get; set; }
        public string Proveedor { get; set; }
        public string Ubicacion { get; set; }
        public string Infraestructura { get; set; }
        public string RutaRepositorio { get; set; }
        public string Contingencia { get; set; }
        public string MetodoAutenticacion { get; set; }
        public string MetodoAutorizacion { get; set; }
        public string AmbienteInstalacion { get; set; }
        public string GrupoServiceDesk { get; set; }
        public string EntidadUso { get; set; }
        public string NombreEntidadUsuaria { get; set; }
        public string AplicacionReemplazo { get; set; }
        public string TipoDesarrollo { get; set; }
        public string PersonaSolicitud { get; set; }
        public string ModeloEntrega { get; set; }
        public string NombreInterfaz { get; set; }
        public string OOR { get; set; }
        public string RatificaOOR { get; set; }

        public string CriticidadAplicacionBIA { get; set; }
        public string ProductoMasRepresentativo { get; set; }
        public string MenorRTO { get; set; }
        public string MayorGradoInterrupcion { get; set; }
        public string NuevaCriticidadFinal { get; set; }
        public string TIERProduccion { get; set; }
        public string TIERPreProduccion { get; set; }

        public int FlagPC { get; set; }
        public string ArquitectoTI { get; set; }

        //Files
        public List<ArchivosCvtDTO> Files { get; set; }
        public List<InputValues> NewInputs { get; set; }
        public string GestorUserIT { get; set; }
        public int TipoFlujoId { get; set; }
        public int TipoSolicitudId { get; set; }
        public bool CargaResponsables { get; set; }

        public DateTime? FechaCreacionAplicacion { get; set; }
        public string FechaCreacionAplicacionStr => FechaCreacionAplicacion.HasValue ? FechaCreacionAplicacion.Value.ToString("dd/MM/yyyy") : string.Empty;

        public string FechaAprobacionRegistroCompleto { get; set; }
        public string CodigoAPTPadre { get; set; }
        public string TribeLeader { get; set; }
        public string MatriculaTL { get; set; }

        public bool ItemSelected { get; set; }

        public string interfaceId { get; set; }
        public string implementationType { get; set; }
        public string assetType { get; set; }
        public string Gerencia { get; set; }

        public string area { get; set; }
        public string unit { get; set; }
        public string Lider_Usuario { get; set; }
        public string Usuario_Autorizador_Gestor { get; set; }
        public string managed { get; set; }
        public string teamName { get; set; }
        public string Tribe_Lead { get; set; }
        public string Tribe_Technical_Lead { get; set; }
        public string Jefe_Equipo { get; set; }
        public string Broker_Sistemas { get; set; }
        public string Experto_Lider_tecnico { get; set; }
        public string BIANarea { get; set; }
        public string BIANdomain { get; set; }
        public string tobe { get; set; }
        public string mainOffice { get; set; }
        public string architect { get; set; }
        public string architectSolution { get; set; }
        public string functionalLayer { get; set; }
        public string userEntity { get; set; }
        public string applicationCriticalityBIA { get; set; }
        public string classification { get; set; }
        public string finalCriticality { get; set; }
        public string starProduct { get; set; }
        public string shorterApplicationResponseTime { get; set; }
        public string highestDegreeInterruption { get; set; }
        public string tierPreProduction { get; set; }
        public string tierProduction { get; set; }
        public string deploymentType { get; set; }
        public string technologyCategory { get; set; }
        public string webDomain { get; set; }
        public string technicalClassification { get; set; }
        public string technicalSubclassification { get; set; }
        public string developmentType { get; set; }
        public string developmentProvider { get; set; }
        public string infrastructure { get; set; }
        public string authenticationMethod { get; set; }
        public string authorizationMethod { get; set; }
        public string groupTicketRemedy { get; set; }
        public string summaryStandard { get; set; }
        public string complianceLevel { get; set; }
        public string isFormalApplication { get; set; }
        public string regularizationDate { get; set; }
        public string parentAPTCode { get; set; }
        public string replacementApplication { get; set; }
        public string solicitante { get; set; }
        public string registerDate { get; set; }
        public string registrationSituation { get; set; }
        public string dateFirstRelease { get; set; }
        public string NombreRol { get; set; }
        public string Email { get; set; }
        public decimal? IndiceObsolescencia_FLooking { get; set; }
        public string Agrupacion { get; set; }
        public string UnidadFondeo { get; set; }
        public string operatingHours { get; set; }
        public string FlagComponenteCross { get; set; }
    }

    public class GuardarEtiquetaAplicacionDTO: BaseDTO
    {
        public int EtiquetaId { get; set; }
        public string CodigoAPT { get; set; }
    }

    public class GuardarUnidadFondeoAplicacionDTO: BaseDTO
    {
        public int UnidadFondeoId { get; set; }
        public string CodigoAPT { get; set; }
    }
}
