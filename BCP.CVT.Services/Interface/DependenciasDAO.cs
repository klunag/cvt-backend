using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class DependenciasDAO: ServiceProvider
    {
        public abstract List<DependenciasDTO> GetListDependencias(PaginacionDependencias pag, out int totalRows);
        public abstract List<DependenciasComponentesDTO> GetListDependenciasComponentes(PaginacionDependenciasComponentes pag, out int totalRows);
        public abstract List<DependenciasDTO> GetListDependenciasDetalle(PaginacionDependencias pag, out int totalRows);
        public abstract CombosDependenciasDTO GetCombosData();
        public abstract List<DependenciasDTO> GetListAccesoApps(string matricula);
        public abstract List<CustomAutocomplete> GetEquipoByFiltro(string filtro);
        public abstract List<DependenciasDTO> GetExportDependencias(string codigoAPT, string tipoConexionId, int tipoRelacionamientoId, int tipoEtiquetaId, int tipoConsultaId);
        public abstract List<DependenciasDTO> GetExportDependenciasDetalle(string codigoAPT, string tipoConexionId, int tipoRelacionamientoId, int tipoEtiquetaId, int tipoConsultaId);
        public abstract List<DependenciasComponentesDTO> GetListExportDependenciasComponentes(string codigoAPT, int equipoId, string tipoCompId, int tipoRelacionamientoId);

        #region Vista Grafica Dependencias
        public abstract List<DataComponente> GetDataComponente(int TipoComponente, int ComponenteId);
        public abstract List<ApsRelacionComponente> GetAppsRelacionComponente(int TipoComponente, int EquipoId, int TipoRelacionId);
        public abstract List<AppsDependenciaImpacto> GetAppsDependenciaImpacto(string codigoAPT, string TipoBusqueda, int TipoRelacionId, int TipoEtiquetaId);
        public abstract List<RelacionAppToApp> GetRelacionApps(int RelacionId);
        #endregion

        #region Diagrama de Infraestructura
        public abstract List<ServidoresRelacionadosDTO> GetListServers(Paginacion pag, out int totalRows);
        public abstract List<ApisRelacionadasDTO> GetListApis(Paginacion pag, out int totalRows);
        public abstract List<ServiciosNubeRelacionadosDTO> GetListServiciosNube(Paginacion pag, out int totalRows);
        public abstract List<RelacionReglasGeneralesDTO> GetReglasGenerales();
        public abstract List<RelacionReglasPorAppTablaDTO> GetReglasPorApp(Paginacion pag);
        public abstract List<RelacionReglasPorAppDiagramaDTO> ListarReglasPorApp(Paginacion pag);
        public abstract List<CustomAutocompleteComponentes> GetComponenteByFiltro_RelacionamientoComponentes(string filtro, string codigoApt);
        public abstract string ActualizarEstadoServidores(Paginacion pag);
        #endregion

    }
}
