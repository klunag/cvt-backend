﻿using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Grilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class TecnologiaDAO : ServiceProvider
    {
        public abstract List<RelacionTecnologiaDTO> GetAllTecnologia();
        public abstract List<TecnologiaG> GetTec(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract bool CambiarEstado(int id, bool estado, string usuario);
        public abstract int AddOrEditTecnologia(TecnologiaDTO objeto);
        public abstract int AddOrEditTecnologiaPowerApps(DtoTecnologia objeto);

        public abstract TecnologiaDTO GetTecById(int id);
        public abstract List<SubdominioDTO> GetSubByDom(int domId);
        public abstract List<CustomAutocomplete> GetAllTecnologia(string filtro);
        public abstract List<CustomAutocomplete> GetTecnologiaEstandarByFiltro(string filtro, string subdominioList, string soPcUsuarioList = null);
        public abstract List<TecnologiaDTO> GetTecnologiasByFiltro(string filtro);
        public abstract List<CustomAutocompleteRelacion> GetAllTecnologiaByClaveTecnologia(string filtro, string subdominioIds = null);
        public abstract List<CustomAutocompleteRelacion> GetTecnologiaEstandarByClaveTecnologia(string filtro, bool? getAll = false);
        public abstract List<CustomAutocomplete> GetTecnologiaByFabricanteNombre(string filtro);
        public abstract List<CustomAutocomplete> GetTecnologiaByNombre(string filtro);
        public abstract List<CustomAutocompleteTecnologia> GetTecnologiaArquetipoByClaveTecnologia(string filtro); //TODO
        public abstract List<CustomAutocomplete> GetAllTecnologiaByClaveTecnologia(string filtro, int? id, string dominioIds = null, string subDominioIds = null);
        public abstract List<CustomAutocomplete> GetTecnologiaForBusqueda(string filtro, int? id, bool flagActivo);
        public abstract List<CustomAutocompleteTecnologiaVinculada> GetAllTecnologia(string filtro, int? id, int[] idsTec);
        public abstract bool ExisteTecnologiaById(int Id);
        public abstract TecnologiaAutocomplete GetTecnologiaById(int Id);
        public abstract bool ExisteClaveTecnologia(string clave, int? id, int? flagActivo);
        public abstract bool ExisteEquivalencia(string equivalencia);
        public abstract bool ExisteEquivalenciaByTecnologiaId(int Id);
        public abstract bool ExisteRelacionByTecnologiaId(int Id);

        //public abstract List<TecnologiaDTO> GetTecnologias
        #region Usuario STD
        public abstract List<TecnologiaG> GetTecSTD(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, string dueno, string equipo, List<int> tipoTecIds, List<int> estObsIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaG> GetReporteTecnologia(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, string dueno, string equipo, List<int> tipoTecIds, List<int> estObsIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract bool CambiarEstadoSTD(int id, int estadoTec, string obs, string usuario);
        public abstract List<TecnologiaEquivalenciaDTO> GetTecEqByTec(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaDTO> GetTec();
        public abstract bool AsociarTecEq(int tecId, string equivalencia, string usuario);
        public abstract bool CambiarFlagEquivalencia(int id, string usuario);
        public abstract bool MigrarEquivalenciasTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario);
        public abstract bool MigrarDataTecnologia(int TecnologiaEmisorId, int TecnologiaReceptorId, string Usuario);
        #endregion

        #region Dashboard
        public abstract FiltrosDashboardTecnologia ListFiltrosDashboard();
        public abstract FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominio(List<int> idsSubdominio);
        public abstract FiltrosDashboardTecnologia ListFiltrosDashboardXDominio(List<int> idsDominio);
        public abstract FiltrosDashboardTecnologia ListFiltrosDashboardXGestionadoPor(List<int> idsGestionadoPor);
        public abstract FiltrosDashboardTecnologia ListFiltrosDashboardXSubdominioXFabricante(List<int> idsSubdominio, List<string> idsFabricante);
        public abstract List<DashboardBase> GetReport(FiltrosDashboardTecnologia filtros, bool isExportar = false);
        public abstract List<ReporteDetalladoSubdominioDto> GetReportEquipos(FiltrosDashboardTecnologia filtros);
        #endregion

        #region ALERTAS
        public abstract List<TecnologiaG> GetTecnologiasXEstado(List<int> idEstados, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaG> GetTecnologiasSinEquivalencia(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaG> GetTecnologiasSinFechasSoporte(int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        #endregion

        public abstract List<TecnologiaDTO> GetTecnologiaByEquipoId(int equipoId, string fecha, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetSistemasOperativoByFiltro(string filtro);
        public abstract List<TecnologiaDTO> GetTecnologiasXAplicacionByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<TecnologiaG> GetTecnologiaVinculadaXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaDTO> GetTecnologiaEstandar(string _subdominioIds = null, string _dominioIds = null);
        public abstract List<TecnologiaDTO> GetTecnologiaEstandar_2(string _tecnologia, string _tipoTecIds, string _estadoTecIds, bool _getAll, string _subdominioIds = null, string _dominioIds = null, string _aplica = null, string _compatibilidadSO = null, string _compatibilidadCloud = null);
        public abstract TecnologiaEstandarDTO GetTecnologiaEstandarById(int id);
        public abstract List<TecnologiaEstandarDTO> GetListadoTecnologiaEstandar(PaginacionEstandar pag, out int totalRows);

        public abstract DashboardTecnologiaEquipoData GetReporteTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros);
        public abstract DashboardTecnologiaEquipoData GetReporteTecnologiaEquiposFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros);

        public abstract List<EquipoDTO> GetListadoTecnologiaEquipos(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);
        public abstract List<EquipoDTO> GetListadoTecnologiaEquiposFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);
        public abstract List<AplicacionDTO> GetListadoTecnologiaAplicaciones(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);
        public abstract List<AplicacionDTO> GetListadoTecnologiaAplicacionesFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);
        public abstract List<TecnologiaDTO> GetListadoTecnologiasVinculadas(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);
        public abstract List<TecnologiaDTO> GetListadoTecnologiasVinculadasFabricanteNombre(FiltrosDashboardTecnologiaEquipos filtros, out int totalRows);

        public abstract List<TecnologiaG> GetTecnologiasPendientes();

        public abstract List<TecnologiaPorVencerDto> GetTecnologiasPorVencer(string subdominio, string tecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaPorVencerDto> GetTecnologiasVencidas(string subdominio, string tecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        public abstract List<TecnologiaG> GetTecnologiaUpdate(Paginacion pag, out int totalRows);
        public abstract EntidadRetorno ActualizarRetorno(ActualizarTecnologia pag);

        public abstract int AddOrEditNewTecnologia(TecnologiaDTO objeto);
        public abstract List<TecnologiaG> GetNewTec(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaG> GetNewTecSP(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null);
        public abstract List<TecnologiaG> GetBajasOwnerTecSP(List<int> domIds, List<int> subdomIds, int? productoId, string codigo,List<int> resolucionCambio, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null);
        public abstract TecnologiaDTO GetNewTecById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false);
        public abstract bool ExisteClaveTecnologia(string claveTecnologia, int id);

        #region TecnologiaByProducto
        public abstract List<TecnologiaDTO> GetTecnologiaByProducto(int productoId);
        public abstract List<TecnologiaDTO> GetTecnologiaByProductoWithPagination(int productoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract int EditTecnologiaFromProducto(TecnologiaDTO objeto);
        public abstract bool DeleteTecnologiaById(int id, string userName);
        #endregion

        #region TecnologiaAplicacion
        public abstract List<TecnologiaAplicacionDTO> GetTecnologiaAplicaciones(int productoTecnologiaId);
        public abstract bool DeleteTecnologiaAplicacionById(int id, string userName);
        public abstract bool GuardarMasivoTecnologiaAplicacion(List<TecnologiaAplicacionDTO> lista, int[] listaEliminar, string userName);
        #endregion

        #region Motivo
        public abstract bool ExisteTecnologiaAsociadaAlMotivo(int motivoId);
        #endregion

        #region Tecnología Owner
        public abstract List<TecnologiaOwnerDto> BuscarTecnologiaXOwner(string correo, int perfilId, string dominioIds, string subDominioIds, string productoStr, int? tribuCoeId, int? squadId, bool? flagTribuCoe, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaOwnerDto> BuscarTecnologiaXOwnerProducto(string correo, int perfilId, int productoId);
        #endregion

        #region Tecnología Owner Consolidado
        public abstract List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescencia(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId, int? nivel, string ownerParentIds, string TipoEquipoIds, string FechaFiltro);
        public abstract List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescenciaReporte(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId);
        public abstract List<TecnologiaOwnerConsolidadoObsolescenciaDTO> ListarTecnologiaOwnerConsolidadoObsolescenciaReporte2(string dominioIds, string subDominioIds, string productoStr, string tecnologiaStr, string ownerStr, string squadId, string TipoEquipoIds, string FechaFiltro);
        #endregion

        #region Tecnología SoportadoPor Consolidado
        public abstract List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescencia(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int? nivel, string soportadoPorParents);
        public abstract List<TecnologiaUdFConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaUdF(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, int? nivel, string soportadoPorParents, string unidadFondeo, bool flagProyeccion, string fechaProyeccion);

        public abstract List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaReporte(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor);
        public abstract List<TecnologiaGestionadoPorConsolidadoObsolescenciaDTO> ListarTecnologiaSoportadoPorConsolidadoObsolescenciaReporte2(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor);
        public abstract List<TecnologiaUdFConsolidadoObsolescenciaDTO> ListarTecnologiaUdfConsolidadoObsolescenciaReporte(string correoOwner, int perfilId, string dominioIds, string subDominioIds, string aplicacionStr, string gestionadoPor, string unidadFondeo, bool flagProyeccion, string fechaProyeccion);

        #endregion

        #region Flujo de Aprobación de Tecnologías
        public abstract TecnologiaDTO GetDatosTecnologiaById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false);
        public abstract OwnerDTO GetDatosPersonaPorMatricula(string codigo, int idTipo);
        public abstract TecnologiaDTO GetDatosResponsablePorId(string codigo, int idTipo);
        public abstract bool VerificarDiferenciaDeDatos(TecnologiaDTO objeto);
        public abstract List<SolicitudFlujoDTO> GetFlujosSolicitudes(PaginacionSolicitud pag, out int totalRows);
        public abstract SolicitudTecnologiaCamposDTO InsertarTecnologiaCampoActualizacion(List<ConfiguracionTecnologiaCamposDTO> diccionario, int idCampo, string valorAnterior, string valorNuevo, string usuarioMatricula, string valorCampoPropuesto);
        public abstract List<ConfiguracionTecnologiaCamposDTO> GetConfiguracionTecnologiaCampos();
        public abstract List<TecnologiaAplicacionDTO> GetTecnologiaAplicacionesPorTecnologia(int idTecnologia);
        public abstract List<SolicitudTecnologiaCamposDTO> InsertarTecnologiaCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, TecnologiaDTO objTec, TecnologiaDTO objeto, List<TecnologiaAplicacionDTO> aplicacionLista);
        public abstract List<SolicitudTecnologiaCamposDTO> InsertarTecnologiaEquivalenciaCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, TecnologiaDTO objTec, TecnologiaDTO objeto);
        public abstract TecnologiaDTO AcondicionamientoTecnologia(TecnologiaDTO objeto);
        public abstract int EditNewTecnologia(FlujoActualizacionTecnologiaCamposDTO objeto);
        public abstract FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaActualizar(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula);
        public abstract FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaEquivalencias(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula);
        public abstract FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudParaDesactivacion(int idSolicitud, int idTipoSolicitud, int idTecnologia, string matricula);
        public abstract DataResultAplicacion ObservarSolicitud(int idSolicitud, string matricula, string comentario, int idTecnologia, int productoId, string email);
        public abstract TecnologiaDTO GetEquivalenciasPropuestaById(int id, bool withEquivalencias = false);
        public abstract int EnviarSolicitudAprobacionTecnologia(TecnologiaDTO objeto);
        public abstract int EnviarSolicitudAprobacionEquivalencia(TecnologiaDTO objeto);
        public abstract int EnviarSolicitudAprobacionDesactivacion(TecnologiaDTO technologyDto);
        public abstract bool ExisteInstanciasByTecnologiaId(int Id);
        public abstract TecnologiaOwnerDto BuscarInstanciasXTecnologia(int idTecnologia);
        public abstract List<SolicitudFlujoDetalleDTO> GetDetalleSolicitudFlujo(PaginacionSolicitud pag, out int totalRows);
        public abstract List<TecnologiaEquivalenciaDTO> GetEquivalenciasPorTecnologia(int idTecnologia);
        public abstract List<TecnologiaInstanciaDTO> GetInstanciasPorTecnologia(int idTecnologia);
        public abstract List<TecnologiaRelacionDTO> GetRelacionesPorTecnologia(int idTecnologia);
        public abstract List<TecnologiaG> GetTecnologiaModificar(int? productoId, List<int> domIds, List<int> subdomIds, string nombre, string codigo, string tribuCoeStr, string squadStr, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, string perfil, out int totalRows);
        public abstract List<TecnologiaG> GetTecnologiaConsolidadaModificar(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string codigo, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, string perfil, string matricula, out int totalRows, string tribuCoeStr = null, string squadStr = null);
        #endregion

        public abstract List<CustomAutocomplete> GetAllTipoEquipoyFiltro(string filtro);
        public abstract List<TecnologiaG> PostListTecnologiasByProducto(PaginacionNewTec pag, out int totalRows);
        public abstract List<ProductoList> GetProductoModificar(int? productoId, List<int> domIds, List<int> subdomIds, string nombre, string codigo, string tribuCoeStr, string squadStr, List<int> tipoTecIds, List<int> estObsIds, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, string perfil, out int totalRows);
        public abstract ProductoDTO GetDatosProductoById(int id, bool withAutorizadores = false, bool withArquetipos = false, bool withAplicaciones = false, bool withEquivalencias = false);
        public abstract int EnviarSolicitudAprobacionProducto(TecnologiaDTO objeto);
        public abstract List<SolicitudTecnologiaCamposDTO> InsertarProductoCampos(List<ConfiguracionTecnologiaCamposDTO> configuracion, ProductoDTO objPro, TecnologiaDTO objeto);
        public abstract List<SolicitudFlujoDTO> GetBandejaSolicitudes(PaginacionSolicitud objeto, out int totalRows);
        public abstract FlujoActualizacionTecnologiaCamposDTO ObtenerDatosDeSolicitudProductoParaActualizar(int idSolicitud, int idTipoSolicitud, int idProducto, string matricula);
        public abstract TecnologiaDTO GetDatosTecnologiaId(int id);

        #region Gestion de Umbrales
        public abstract List<CustomAutocomplete> GetListaAutoCompletarTecnologia(string filtro, int? id, string dominioIds = null, string subDominioIds = null);
        public abstract List<TecnologiaG> GetTecnologiasGUmbrales(int? productoId, List<int> domIds, List<int> subdomIds, string filtro, string aplica, string codigo, string dueno, List<int> tipoTecIds, List<int> estObsIds, bool withApps, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows, string responsableTribu = null, string responsableSquad = null);
        #endregion

        public abstract List<CustomAutocomplete> GetAllTecnologiaAzure();
    }
}
