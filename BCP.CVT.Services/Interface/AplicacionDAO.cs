using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{ 
    public abstract class AplicacionDAO : ServiceProvider
    {
        public abstract int AddOrEdit(AplicacionDTO objRegistro);
        public abstract AplicacionPublicDto GetAplicacionByCodigo(string CodigoAPT);
        public abstract List<AplicacionDTO> GetAplicacion();

        public abstract List<ConfiguracionColumnaAplicacionDTO> GetConfiguracionColumnaAplicacion();
        public abstract List<AplicacionDTO> GetAplicacion(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionVistaConsultor(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionConfiguracion(PaginacionAplicacion pag, out int totalRows, out string arrCodigoAPTs);
        public abstract List<AplicacionDTO> GetAplicacionConfiguracionExportar(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionPortafolio(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionResponsables(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionConsultor(PaginacionAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetAplicacionConsultorByFilter(PaginacionAplicacion pag, out int totalRows);
        public abstract bool AddOrEditAplicacionExperto(ParametroList objRegistro);
        public abstract bool AddOrEditComponentesApp(ComponenteConectaCon objRegistro);
        public abstract bool AddOrEditAplicacionExpertoConsultor(ParametroConsultor objRegistro);
        public abstract bool AddOrEditAplicacionExpertoPortafolio(ParametroList objRegistro);
        public abstract List<AplicacionExpertoDTO> GetAplicacionExperto(string Id);
        public abstract List<AplicacionExpertoDTO> GetAplicacionExpertoPortafolio(string Id);
        public abstract bool CambiarFlagRelacionar(AplicacionDTO objRegistro);
        public abstract bool CambiarFlagExperto(int Id, string user, int fuente = 0);
        public abstract List<TipoExpertoDTO> GetTipoExperto();
        public abstract FiltrosAplicacion GetFiltros();

        public abstract FiltrosAplicacion GetFiltros2();
        public abstract FiltrosReporteAplicacion GetFiltrosReporteAplicacion();
        // AUTOCOMPLETE
        public abstract List<CustomAutocomplete> GetAplicacionByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetNombreAplicacionByFiltro(string filtro);
        public abstract List<CustomAutocompleteAplicacion> GetAplicacionByFiltro(string filtro, bool flagRelacionar);
        public abstract List<CustomAutocomplete> GetAplicacionAprobadaByFiltro(string filtro, bool? flagAprobado);
        public abstract List<CustomAutocomplete> GetAplicacionAprobadaCatalogoByFiltro(string filtro);
        public abstract bool ExisteAplicacionById(string Id);
        public abstract bool ExisteCodigoSIGAByFilter(string codigo);
        public abstract bool ExisteAplicacionById(string Id, int AppId);
        public abstract bool ExisteAplicacionByCodigoNombre(string filtro, int idAplicacion);
        public abstract bool ExisteAplicacionById(string Id, bool FlagRelacionar);
        public abstract bool ExisteAplicacionByNombre(string nombre, int id);
        public abstract bool ExisteCodigoInterfaz(string filtro, int id);
        public abstract List<string> GetCodigoInterfazUsados( int id);
        public abstract List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetOwnerByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetExpertoByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetGerenciaByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetDivisionByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetGestionadoByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetGestorByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetTTLByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetBrokerByFiltro(string filtro);
        // Método para generar reporte de responsables de portafolio
        public abstract List<AplicacionDTO> GetResponsablePortafolio(out int totalRows);
        public abstract List<CustomAutocomplete> GetProductoManagerFiltro(string filtro);

        #region Dashboard
        public abstract FiltrosDashboardAplicacion ListFiltros();
        public abstract FiltrosDashboardAplicacion ListFiltrosResumen();
        public abstract FiltrosDashboardAplicacion ListFiltrosXGestionadoPor(List<string> gestionadoPor);
        public abstract FiltrosDashboardAplicacion ListAplicacionesXFiltros(FiltrosDashboardAplicacion filtros);
        public abstract DashboardAplicacionData GetReporte(FiltrosDashboardAplicacion filtros);
        #endregion

        public abstract AplicacionDTO GetAplicacionDetalleById(string codigoAPT);
        public abstract List<AplicacionExpertoDTO> GetAplicacionExpertoByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetTipoExpertoByFiltro(string filtro);        
        public abstract List<CustomAutocomplete> GetTipoExpertoPortafolioByFiltro(string filtro);        
        public abstract List<CustomAutocomplete> GetUnidadByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetAreaByFiltro(string filtro);
        public abstract List<AplicacionDTO> GetAplicacionesExperto(string matricula);

        #region GestionAplicacion
        public abstract List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows);
        public abstract FiltrosAplicacion GetFiltrosGestionAplicacion();
        public abstract DataResultAplicacion AddOrEditAplicacion(AplicacionDTO objeto);
        public abstract AplicacionDTO GetAplicacionById(int Id, int TipoSolicitudId = (int)ETipoSolicitudAplicacion.CreacionAplicacion);
        public abstract int GetEstadoSolicitudAppById(int Id);
        public abstract bool CambiarEstadoApp(int id, int estadoTec, string obs, string usuario);
        public abstract bool CambiarEstado(int id, string estado, string usuario);

        public abstract List<ConfiguracionColumnaAplicacionDTO> GetColumnaAplicacion(PaginacionAplicacion pag, out int totalRows);
        public abstract int AddOrEditColumnaApp(ConfiguracionColumnaAplicacionDTO objRegistro);
        public abstract int ReordenarColumnaApp(List<ConfiguracionColumnaAplicacionOrdenDTO> registros, string username);
        public abstract ConfiguracionColumnaAplicacionDTO GetColumnaAppById(int id);

        public abstract DataTable GetPublicacionAplicacion(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetPublicacionAplicacion3(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract DataTable GetPublicacionAplicacionPortafolioAplicaciones(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract DataTable GetAplicacionesDesestimadas(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract DataTable GetFormatosRegistro(PaginacionReporteAplicacion pag, out int totalRows);

        public abstract DataTable GetPublicacionAplicacion2(PaginacionReporteAplicacion pag, out int totalRows);

        public abstract DataTable GetPublicacionAplicacionAsignada(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract DataTable GetPublicacionAplicacionCatalogo(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract DataTable GetPublicacionAplicacionDesestimada(PaginacionReporteAplicacion pag, out int totalRows);


        public abstract List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJS(string tablaProcedencia);

        public abstract List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJS2(string tablaProcedencia);
        public abstract List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJSCatalogo(string tablaProcedencia);
        public abstract List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJSAppsDesestimadas(string tablaProcedencia);
        public abstract bool ExisteOrden(int ordenNuevo, int ordenActual);
        public abstract bool ExisteNombre(string nombre, int id);
        public abstract int AddOrEditAPR(AplicacionPortafolioResponsablesDTO objeto, bool _flagSolicitud = false);

        public abstract List<ModuloAplicacionDTO> GetModuloAplicacion(PaginacionAplicacion pag, out int totalRows);
        public abstract List<ParametroDTO> GetParametroAplicacion(PaginacionAplicacion pag, out int totalRows);
        public abstract bool CambiarEstadoModulo(int Id, bool estado, string usuario);
        public abstract string GetCodigoModulo(string codigoAPT);
        public abstract int AddOrEditParametro(ParametroDTO objRegistro);

        public abstract ModuloAplicacionDTO GetModuloById(int Id);
        public abstract int AddOrEditModuloAplicacion(ModuloAplicacionDTO objRegistro);

        #endregion

        public abstract List<AplicacionCargaDto> GetReporteAplicacionData(PaginacionAplicacion pag, out int totalRows);

        public abstract List<AplicacionDTO> GetAplicacionPortafolioUpdate(PaginacionAplicacion pag, out int totalRows);

        public abstract DataResults GetResultsCargaMasivaPortafolio();
        public abstract DataResults GetResultsCargaMasivaPortafolio2();


        public abstract void GetResultsCargaMasivaPortafolio(PortafolioBackupDTO objRegistro);

        public abstract List<PortafolioBackupDTO> GetAplicacionPortafolioBackups(PaginacionPortafolioBackup pag, out int totalRows);
        public abstract PortafolioBackupDTO GetAplicacionPortafolioBackupById(int idBackup);
        public abstract List<AplicacionPortafolioResponsablesDTO> ObtenerAplicacionPortafolioResponsableXListaMatricula(List<string> listaMatriculas);

        //public abstract List<AplicacionDTO> GetPublicacionAplicacion3(PaginacionReporteAplicacion pag, out int totalRows);
        public abstract List<AplicacionDTO> GetPublicacionAplicacion4(PaginacionReporteAplicacion pag, out int totalRows);

        public abstract List<GrupoTicketRemedyDto> GetGrupo(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<CodigoReservadoDTO> GetCodigoReservado(string codigo, int tipoCodigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CodigoReservadoDTO> GetCodigoReutilizado(string codigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract bool GetCodigoReutilizadoExists(string codigo);

        public abstract GrupoTicketRemedyDto GetGrupoById(int id);

        public abstract CodigoReservadoDTO GetCodigoReservadoById(int id); 
        public abstract CodigoReservadoDTO GetCodigoReutilizadoById(int id);

        public abstract bool CambiarEstadoGrupo(int id, bool estado, string usuario);

        public abstract bool CambiarEstadoCodigo(int id, bool? estado, string usuario);
        public abstract bool GetCambiarEstadoCodigoReutilizado(int id, bool? estado, bool? eliminado, string codigo, string comentarios, string usuario);
        public abstract bool GetEliminarCodigoReutilizado(int id, bool? estado, bool? eliminado, string codigo, string comentarios, string usuario);

        public abstract int AddOrEditGrupo(GrupoTicketRemedyDto objeto);

        public abstract int AddOrEditCodigo(CodigoReservadoDTO objeto);
        public abstract int AddOrEditCodigoReutilizado(CodigoReservadoDTO objeto);

        public abstract List<EtiquetaAplicacionDTO> GetListEtiquetas();
        public abstract List<UnidadFondeoDTO> GetListUnidadFondeo();
        public abstract List<SegundoNivelDTO> GetListSegundoNivel();
        public abstract EtiquetaAplicacionDTO GetAplicacionEtiqueta(string codigoAPT);
        public abstract UnidadFondeoDTO GetAplicacionUnidadFondeo(string codigoAPT);
        public abstract bool GuardarEtiqueta(GuardarEtiquetaAplicacionDTO item);
        public abstract bool GuardarUnidadFondeo(GuardarUnidadFondeoAplicacionDTO item);

        public abstract List<CustomAutocompleteAplicacion> ListAplicaciones_ByPerfil(int perfil, string aplicacion, string usuario);

        public abstract List<AplicacionDTO> DiagramaInfra_GetAplicacionConfiguracion(PaginacionAplicacion pag, out int totalRows, out string arrCodigoAPTs);

    }
}
