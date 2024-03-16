using BCP.CVT.DTO;
using BCP.CVT.Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Dto;
using BCP.PAPP.Common.Custom;
using System.Data;
using BCP.CVT.DTO.Custom;

namespace BCP.CVT.Services.Interface.PortafolioAplicaciones
{
    public abstract partial class ApplicationDAO : ServiceProvider
    {
        public abstract DataResultAplicacion AddApplicationStepOne(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion AddApplicationReversion(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion EditApplicationStepTwo(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion EditApplicationStepTwo2(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion EditApplicationStepTwo2UserIT(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion EditApplicationStepTwo3(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion EditApplicationEvalArchitect(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationEvalAIO(ApplicationDto objeto);

        public abstract bool GetPersona(string nombre);
        public abstract int? GetAreaId(string name);

        public abstract List<PCIDto> GetPCI(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract PCIDto GetPCIById(int id);
        public abstract List<CheckListCross> GetCheckListCross();
        public abstract bool CambiarEstadoPCI(int id, bool? estado, string usuario);

        public abstract int AddOrEditPCI(PCIDto objeto);
        public abstract DataResultAplicacion EditApplicationDevSecOps(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationArchitectIT(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationArchitectSolution(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationOwner(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationTeamLeader(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationEvalUserIT(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationTTL(ApplicationDto objeto);
        public abstract DataResultAplicacion EditApplicationAdmin(ApplicationDto objeto, List<ApplicationManagerCatalogDto> usuarios);
        public abstract DataResultAplicacion RefuseFlow(ApplicationDto objeto);
        public abstract DataResultAplicacion RefuseUser(ApplicationDto objeto);

        public abstract DataTable GetPublicacionAplicacionAsignada(PaginacionReporteAplicacion pag, out int totalRows);

        public abstract bool ExistsApplicationById(string id);

        public abstract bool ExistsApplicationInterfaceById(string id);
        public abstract bool ExistsApplicationByName(string name);

        public abstract bool ExisteCodigoAPT(string codigo);
        public abstract string ExisteCodigoInterfaz(string codigo);

        public abstract bool ValidarModificacion(ApplicationDto objeto);

        public abstract bool ValidarModificacion2(ApplicationDto objeto);
        public abstract int GetMainOfficeId(int architectId);
        public abstract void ChangeStatusApplication(int id, int status, string user, string comments, string previousState);

        public abstract void ChangeStatusApplicationEliminada(int id, string user);

        public abstract void ReverseStatusApplication(int id, int status, string user, string comments);

        public abstract int RemoveApplication(int id, int status, string user, string comments, string matricula, string estadoAnterior, string NombreUsuarioModificacion, string email,
                                            bool flagRequiereConformidad, string ticketEliminacion, string expertoNombre, string expertoMatricula, string expertoCorreo, int tipoEliminacion
                                            ,byte[] ConformidadGST,string archivoConformidadNombre, byte[] TicketEliminacion,string archivoTicketEliminacionNombre,byte[] Ratificacion, string archivoRatificacionNombre);

        public abstract int RemoveApplicationAdmin(int id, int status, string user, string comments, string matricula, string estadoAnterior, string NombreUsuarioModificacion, string email,
                                      bool flagRequiereConformidad, string ticketEliminacion, string expertoNombre, string expertoMatricula, string expertoCorreo, int tipoEliminacion);

        public abstract int UpdateSolicitud(int id, int status, string user, string comments, string matricula, string estadoAnterior, string NombreUsuarioModificacion, string email,
                                     bool flagRequiereConformidad, string ticketEliminacion, string expertoNombre, string expertoMatricula, string expertoCorreo, int tipoEliminacion);

        public abstract int ApproveRemoveApplication(int solId, int flowId, string matricula, string NombreUsuarioModificacion, string Email);


        public abstract int RefuseRemoveApplication(int solId, int flowId, string matricula, string NombreUsuarioModificacion, string Email, string Comments);

        public abstract int ObservarRemoveApplication(int solId, int flowId, string matricula, string NombreUsuarioModificacion, string Email, string Comments);

        public abstract void SubirArchivosRemove(SolicitudArchivosDTO obj);

        public abstract DataResultAplicacion ApprovedApplication(int id, string user, string email, List<ApplicationManagerCatalogDto> usuarios, string Matricula, string NombreUsuarioModificacion);

        public abstract DataResultAplicacion ApprovedSolicitud(int id, string user);

        public abstract DataResultAplicacion ApprovedSolicitudEliminacion(int id, string user, string matricula, string nombre);

        public abstract DataResultAplicacion ObservedSolicitudEliminacion(int id, string user, string matricula, string nombre, string comment);

        public abstract DataResultAplicacion RefusedSolicitud(int id, string user, string comments, string matricula, string nombre);

        public abstract DataResultAplicacion RefusedSolicitudEliminacion(int id, string user, string comments);
        public abstract void AddOrEditPortafolioManager(ApplicationManagerCatalogDto objeto);

        public abstract List<ApplicationList> GetApplicationByUser(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationList> GetApplicationUserIT(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationList> GetApplicationReversion(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationFlowDto> GetApplicationFlow(PaginationApplication filter, out int totalRows);
        public abstract ApplicationDetail GetApplicationById(int id);
        public abstract SolicitudDto GetSolicitudById(int id);
        public abstract SolicitudDto GetSolicitudById2(int id);

        public abstract ApplicationDetail GetApplicationById2(int id);

        public abstract ApplicationDetail GetApplicationUnidadById(int id, int UnidadId);

        public abstract ApplicationDetail GetApplicationOwnerById(int id, string responsable);

        public abstract List<CustomAutocompleteApplication> GetUnidadesOwnerById(string responsable);

        public abstract ApplicationDetail GetApplicationJefaturaATIById(int id);

        public abstract ApplicationDetail GetFullApplicationById(int id);
        public abstract List<ApplicationFlowList> GetApplicationFlowByUser(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationFlowList> GetApplicationFlowByUserIT(PaginationApplication filter, out int totalRows);
        public abstract List<ApplicationFlowList> GetApplicationFlowByApp(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationFlowList> GetApplicationFlowByAppEliminacion(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationFlowList> GetApplicationFlowByAppModificacion(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationFlowList> GetApplicationFlowByAppPendiente(PaginationApplication filter, out int totalRows);
        public abstract ApplicationDetail GetApplicationByCodigo(string codigo);

        public abstract List<ApplicationManagerCatalogDto> GetApplicationManagerCatalogByRolesIds(List<int> idsRol, int appId);
        public abstract List<ApplicationManagerCatalogDto> GetOnlyApplicationManagerCatalogByRolesIds(List<int> idsRol, int appId);
        
        public abstract ApplicationFlowDto GetApplicationFlowById(int id);
        public abstract void ValidateRegister(int id);
        public abstract List<ApplicationList> GetRequestAppCreation(PaginationApplication filter, out int totalRows);
        public abstract List<ApplicationList> GetRequestAppAsignada(PaginationApplication filter, out int totalRows);

        public abstract List<ApplicationList> GetRequestAppAsignadaNoEliminadas(PaginationApplication filter, out int totalRows);
        public abstract List<SolicitudList> GetSolicitud(PaginationSolicitud filter, out int totalRows);

        public abstract List<ApplicationManagerCatalogDto> GetRolesByApplication(int appId);

        public abstract List<ApplicationManagerCatalogDto> GetExpertsByApplication(int appId);
        public abstract List<ApplicationManagerCatalogDto> GetRolesByInitialApplication(int appId);
        public abstract List<ApplicationManagerCatalogDto> GetOwnersByApplication(int appId);

        public abstract List<ApplicationManagerCatalogDto> GetExpertosByApplication(int appId);
        public abstract List<CustomAutocomplete> GetApplicationVigenteByFilter(string filter, bool? flagAprobado, string codigoAPT);
        public abstract List<CustomAutocomplete> GetApplicationReplaceByFilter(string filter, bool? flagAprobado, string codigoAPT);

        public abstract List<CustomAutocompleteRemedy> GetGroupRemedyByFilter(string filter, bool? flagAprobado);
        public abstract List<ApplicationList> GetApplicationsToNotify();
        public abstract List<ApplicationList> GetApplicationsToNotifyAdmins();

        public abstract List<ApplicationList> GetOwnersEliminacionPendientes();
        public abstract List<ApplicationList> GetPortafolioEliminacionPendientes();
        public abstract List<ApplicationList> GetSolicitanteEliminacionPendientes();
        public abstract List<ApplicationList> GetPortafolioReactivacionPendientes();
        public abstract List<ApplicationList> GetSolicitanteReactivacionPendientes();

        public abstract List<ApplicationFlowList> GetApplicationsFlowToNotify(int id);

        public abstract void DeleteApplication(int id, string user);
        public abstract void DeleteApplication2(int id, string user, string email, string comments, string Matricula, string NombreUsuarioModificacion);

        public abstract List<ApplicationDetail> GetApplicationList(PaginacionAplicacion pag, out int totalRows);
        public abstract List<ApplicationDetail> GetApplicationForUpdate();

        public abstract List<ApplicationDetail> GetApplicationForUpdateSP();

        public abstract int? GetManagedId(string name);
        public abstract int? GetDeploymentTypeId(string name);
        public abstract int? GetStatusId(string name);
        public abstract int? GetUnitId(string name);
        public abstract int? GetInfrastructureId(string name);
        public abstract int? GetAssetTypeId(string name);
        public abstract int? GetBianDomainId(string name);
        public abstract int? GetMainOfficeId(string name);
        public abstract int? GetTechnologyCategoryId(string name);
        public abstract int? GetTechnicalSubclassificationId(string name);
        public abstract int? GetTechnicalClassificationId(string name);
        public abstract int? GetGroupTicketRemedyId(string name);

        public abstract int? GetTobeId(string name);
        public abstract int? GetAuthorizationMethodId(string name);
        public abstract int? GetAuthenticationMethodId(string name);


        public abstract int? GetAssetClassificationId(string name);

        public abstract int? GetAssetBIACriticalityId(string name);

        public abstract int? GetFinalCriticality(int? clasificacion, int? bia);
        public abstract int? GetImplementationTypeId(string name);

        public abstract int? GetDevTypeId(string name);

        public abstract string GetUserEntityId(string name);

        public abstract string GetPCIId(string name);
        public abstract int? GetGereciaId(string name);
        public abstract int? GetDivisionId(string name);

        public abstract int? GetBIANAreaId(string name);

        public abstract string GetMatriculaByName(string name);
        public abstract string GetEmailByName(string name);

        public abstract int? GetapplicationCriticalityBIAId(string name);
        //public abstract int? GetClassificationId(string name);

        //public abstract int? GetFinalCriticalityId(string name);

        public abstract List<InfoCampoPortafolioToolbox> GetAppToolbox();

        public abstract void EliminarPortafolio();

        public abstract int AddOrEditConsulta(ConsultaDTO objeto);

        public abstract int EliminarConsulta(ConsultaDTO objeto);

        public abstract int EditConsulta(ConsultaDTO objeto);

        public abstract int AddRespuesta(ConsultaDTO objeto);

        public abstract ConsultaDTO GetConsultaById(int id);

        public abstract List<ConsultaDTO> GetConsultasByUser(int tipoid, int respondido, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, out int totalRows);
        public abstract List<ConsultaDTO> GetConsultas(int tipoid, int respondido, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, string desde, string hasta, out int totalRows);

        public abstract List<ApplicationList> GetApplicationByUserWithActive(PaginationApplication filter, out int totalRows);
        public abstract List<ApplicationFlowList> GetApplicationFlowByUserExportar(PaginationApplication filter, out int totalRows);

        public abstract bool GetApplicationEliminadaStatus(string codApplication, DateTime fecha);

        public abstract string GetApplicationEliminadaGetDescription(string codApplication);

        public abstract List<UnidadDTO> GetUsuarioUnidad(string name);

        public abstract List<OwnerDTO> GetOwner(string name);

        public abstract OwnerDTO GetPersonaSIGA(string matricula);

        public abstract PaginationApplication BuscarEnUnidad(string name);

        public abstract List<ApplicationList> GetApplicationEliminadasAprobadas(PaginationApplication filter, out int totalRows);

        public abstract int AprobarSolicitudReversionEliminacion(SolicitudDto objRegistro);
        public abstract int RechazarSolicitudReversionEliminacion(SolicitudDto objRegistro);

        public abstract void ActualizarSolicitudArchivos(SolicitudArchivosDTO obj, int accion);

        public abstract List<ParametricaDetalle> GetAllParametricaDetalle();

        public abstract List<GestionadoPor> GetAllGestionadoPor();

        public abstract List<Unidad> GetAllUnit();

        public abstract List<TipoActivoInformacion> GetAllAssetType();
        public abstract List<DominioBian> GetAllBianDomain();

        public abstract List<JefaturaAti> GetAllMainOffice();

        public abstract List<ClasificacionTecnica> GetAllClasificacionTecnica();

        public abstract List<SubClasificacionTecnica> GetAllSubClasificacionTecnica();

        public abstract List<GrupoRemedy> GetAllGrupoRemedy();

        public abstract List<PlataformaBcp> GetAllTobe();
        public abstract List<Area> GetAllAreas();
        public abstract List<AreaBian> GetAllAreaBian();
        public abstract List<ApplicationManagerCatalog> GetAllApplicationManager();

        public abstract List<Division> GetAllDivision();

        public abstract List<Gerencia> GetAllGerencia();
        public abstract OwnerDTO GetCorreoPorMatricula(string matricula);

        public abstract List<UsuarioSigaDto> GetUsuarios(string filtro);

    }
}