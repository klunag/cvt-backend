using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class AplicacionCargaDto: BaseDTO
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string TipoActivoInformacion { get; set; }
        public string GerenciaCentral { get; set; }
        public string Division { get; set; }
        public string Area { get; set; }
        public string Unidad { get; set; }
        public string DescripcionAplicacion { get; set; }
        public string EstadoAplicacion { get; set; }

        public string CodigoInterfaz { get; set; }
        public string InterfazApp { get; set; }
        public string NombreServidor { get; set; }
        public string CompatibleWindows7 { get; set; }
        public string CompatibleNavegador { get; set; }
        public string CompatibleHV { get; set; }
        public string InstaladaDesarrollo { get; set; }
        public string InstaladaCertificacion { get; set; }
        public string InstaladaProduccion { get; set; }
        public string GTR { get; set; }
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
        public string DetalleEstrategiaRoadmapPlanificado { get; set; }
        public string EstadoRoadmap { get; set; }
        public string EtapaAtencionRoadmap { get; set; }
        public string RoadmapEjecutado { get; set; }
        public string FechaInicioRoadmapEjecutado { get; set; }
        public string FechaFinRoadmapEjecutado { get; set; }
        public string CodigoAplicacionReemplazo { get; set; }

        public string SWBaseSO { get; set; }
        public string SWBaseHP { get; set; }
        public string SWBaseLP { get; set; }
        public string SWBaseBD { get; set; }
        public string SWBaseFramework { get; set; }
        public string RET { get; set; }
        public string MotivoCreacion { get; set; }
        public string PlataformaBCP { get; set; }
        public string ProveedorDesarrollo { get; set; }
        public string Ubicacion { get; set; }
        public string Infraestructura { get; set; }
        public string Repositorio { get; set; }
        public string Contingencia { get; set; }
        public string MetodoAutenticacion { get; set; }
        public string MetodoAutorizacion { get; set; }
        public string AmbienteInstalacion { get; set; }
        public string GrupoServiceDesk { get; set; }
        public string EntidadUsuaria { get; set; }
        public string NombreEntidadUsuaria { get; set; }
        public string AplicativoReemplazo { get; set; }
        public string TipoDesarrollo { get; set; }
        public string PersonaSolicitud { get; set; }
        public string ModeloEntrega { get; set; }
        public string NombreInterface { get; set; }
        public string OOR { get; set; }
        public string RatificaOOR { get; set; }
        public string ClasificacionTecnica { get; set; }

        public string MatriculaOwner { get; set; }
        public string MatriculaGestor { get; set; }
        public string MatriculaTTL { get; set; }
        public string MatriculaExperto { get; set; }
        public string MatriculaBroker { get; set; }
        public string MatriculaJDE { get; set; }

        public string Owner_LiderUsuario_ProductOwner { get; set; }
        public string Gestor_UsuarioAutorizador_ProductOwner { get; set; }
        public string TribeTechnicalLead { get; set; }
        public string Experto_Especialista { get; set; }
        public string BrokerSistemas { get; set; }
        public string JefeEquipo_ExpertoAplicacionUserIT_ProductOwner { get; set; }

        public string FechaRegistroProcedencia { get; set; }
        public string FechaCreacionProcedencia { get; set; }

        public int TotalFilas { get; set; }
        public string Criticidad { get; set; }
        public string AreaBIAN { get; set; }
        public string DominioBIAN { get; set; }
        public string JefaturaATI { get; set; }

        public string NombreEquipo_Squad { get; set; }
        public string GestionadoPor { get; set; }
        public string EntidadResponsable { get; set; }
        public string CategoriaTecnologica { get; set; }
        public string RLSI { get; set; }
        public string Roadmap { get; set; }

        //Nuevos campos
        public bool ValidarExcel { get; set; }
        public int? FilaExcel { get; set; }
        public bool? FlagRegistroValido { get; set; }

        public string GestorUserIT { get; set; }
        public string ArquitectoTI { get; set; }
        public string CriticidadAplicacionBIA { get; set; }
        public string ProductoMasRepresentativo { get; set; }
        public string MenorRTO { get; set; }
        public string MayorGradoInterrupcion { get; set; }
        public string SubclasificacionTecnica { get; set; }


        public DateTime? FechaRegistroAplicacion { get; set; }
        public DateTime? FechaCreacionAplicacion { get; set; }
        public string FechaRegistroAplicacionStr => FechaRegistroAplicacion.HasValue ? FechaRegistroAplicacion.Value.ToString("dd/MM/yyyy") : "";
        public string FechaCreacionAplicacionStr => FechaCreacionAplicacion.HasValue ? FechaCreacionAplicacion.Value.ToString("dd/MM/yyyy") : "";

        public string GestorAplicacionCTR { get; set; }
        public string ConsultorCTR { get; set; }
    }
}
