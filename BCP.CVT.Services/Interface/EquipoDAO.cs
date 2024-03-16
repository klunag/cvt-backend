using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract class EquipoDAO : ServiceProvider
    {
        public abstract bool ExisteEquipoById(int Id);

        public abstract List<CustomAutocomplete> GetPCIByFiltro(string filtro);
        public abstract List<CustomAutocomplete> GetEquipoByFiltro(string filtro, int idTipoEquipo);
        public abstract List<CustomAutocomplete> GetEquipoByFiltro_ComponenteDiagramaInfra(string filtro, string codigoApt);
        public abstract List<CustomAutocomplete> GetEquipoByFiltroActivo(string filtro, int idTipoEquipo);
        public abstract List<CustomAutocomplete> GetEquipoByFiltroSinServicioNube(string filtro);
        public abstract List<CustomAutocomplete> GetEstadoBajaServidorByFiltro();
        public abstract List<EquipoAutocomplete> GetEquipoDetalladoByFiltro(string filtro);
        public abstract List<EquipoDTO> GetEquiposSinSistemaOperativo(int idSubdominio, DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquiposSinTecnologias(DateTime fechaConsulta, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquiposSinRelaciones(int[] arrTipoEquipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocomplete> GetTicketByFiltro(string filtro, int idTipoEquipo);
        public abstract FiltrosDashboardEquipo ListFiltrosDashboard();
        public abstract FiltrosDashboardEquipo ListFiltrosDashboardByEquipo(int idEquipo);
        public abstract List<PlotlyDataDTO> GetReportAplicacionesXEquipo(FiltrosDashboardEquipo filtros);
        public abstract List<DashboardEquipoBase> GetReportTecnologiasXEquipo(FiltrosDashboardEquipo filtros);

        //Relacionados a las nuevas vistas


        public abstract List<EquipoDTO> GetEquipos(string nombre, string so, int ambiente, int tipo, int subdominioSO, int desId, int subsiId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquipos(string nombre, string IP, string so, string ambientesIds, string tiposIds, int subdominioSO, string desIds, string subsiIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquiposConsultor(string nombre, string IP, string so, string ambienteIds, string tipoIds, int subdominioSO, string desIds, string subsiIds, int? flagActivo, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, out int totalRows);
        public abstract List<TipoEquipoDTO> GetTipoEquipos();

        public abstract List<CustomAutocomplete> GetTipoEquipoByFiltro(string filtro);
        public abstract TipoEquipoDTO GetTipoEquipoById(int Id);

        #region GESTION EQUIPOS
        public abstract List<EquipoDTO> GetEquipo(string nombre, int tipoEquipoId, List<int> tipoEquipoIds, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetServidoresBaja(string lstServidores, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetListarPendienteBajaServidores(string nombre, string ticket, string fechaBusIni, string fechaBusFin, List<int> estadoSolicitudBaja, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract string InsertarBajaServidores(string lstServidores, string matricula, string nombreArchivo, out int totalRows);
        public abstract string ProcesarBajaServidores(string lstServidores, string matricula, bool isApprovedBaja, out int totalRows);
        public abstract List<EquipoDTO> GetEquipoUpdate(string nombre, int tipoEquipoId, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquipoConsultor(string nombre, int tipoEquipoId, int desId, int exCalculoId, int? flagActivo, int? subsidiariaId, int pageNumber, int pageSize, string sortName, string sortOrder, string matricula, out int totalRows);
        //public abstract List<CustomAutocomplete> GetEquipoByFiltro(string filtro);
        public abstract int AddOrEditEquipo(EquipoDTO objeto);
        public abstract EquipoDTO GetEquipoById(int id);
        public abstract EquipoDTO GetEquipoSoftwareBaseById(int id);

        public abstract List<CustomAutocomplete> GetSOTecnologias(string filtro);
        public abstract int AsignarSOTecnologias(EquipoTecnologiaDTO objeto);
        public abstract int AsignarFechaFin(EquipoDTO objeto);
        public abstract EquipoTecnologiaDTO GetSOById(int EquipoId);

        public abstract bool ExisteServidorByNombre(string nombre);
        public abstract EquipoDTO ExisteEquipoByHostname(string nombre);
        public abstract EquipoDTO ExisteEquipoAllByHostname(string nombre);
        public abstract AmbienteDTO ExisteAmbienteByNombre(string nombre);
        public abstract DominioServidorDTO ExisteDominioServidorByNombre(string nombre);
        public abstract bool ExisteEquipoByNombre(string nombre);
        public abstract int ExisteEquipoApplianceByNombre(string nombre);
        public abstract TipoEquipoDTO GetTipoEquipoByNombre(string nombre);
        public abstract bool ExisteTipoEquipoByNombre(string nombre);

        public abstract bool CambiarEstado(int id, bool estado, string usuario, string motivo);
        public abstract bool CambiarEstadoEquipoSoftwareBase(int id, bool estado, string usuario, string nombreUsuario);
        public abstract bool RemoveEquipoSoftwareBase(int id);
        public abstract int AddOrEditEquipoSoftwareBase(EquipoSoftwareBaseDTO objeto);

        //Appliance
        public abstract bool ExisteEquipoByNombre(string clave, int id);
        public abstract List<EquipoDTO> GetEquipoAppliance(PaginacionEquipo pag, out int totalRows);
        public abstract EquipoSoftwareBaseDTO GetEquipoSoftwareBaseConfiguradoById(int id);
        public abstract List<EquipoDTO> GetEquipoConfiguracionAppliance(PaginacionEquipo pag, int TipoAcceso, out int totalRows);
        public abstract List<EquipoDTO> GetESBSearch(PaginacionEquipo pag, out int totalRows);
        public abstract bool ExisteEquipoAsociadoById(int id);
        public abstract string ExisteEquipoAsociadoSoftwareBaseById(int id);

        #endregion
        #region VistaEquipo
        public abstract EquipoDTO GetEquipoDetalleById(int id);
        public abstract EquipoDTO GetEquipoDetalleAdicional(string hostname);
        #endregion
        public abstract List<EquipoDTO> GetServidoresRelacionadosByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetApisRelacionadasByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquipoByTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<TecnologiaDTO> GetEquipoTecnologiaByEqId(int equipoId);
        public abstract List<EquipoDTO> GetEquipoDetallado(string filtro, int tipoEquipoId, int desId, int exCalculoId, int pageNumber, int pageSize, string sortName, string sortOrder);
        public abstract List<EquipoDTO> GetEquipoXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<CustomAutocompleteFecha> GetTecnologiasNoRegistradasXEquipo(FiltrosDashboardEquipo filtros);

        #region EQUIPOS_EXCLUIDOS
        public abstract List<HistoricoExclusionDTO> GetEquipoExclusion(string nombre, int tipoExclusionId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        #endregion

        public abstract List<CustomAutocomplete> GetEquipoDetalleEscaneadasVsRegistradas(int id, DateTime fecha);
        public abstract List<EquipoDTO> GetEquipoXTecnologiaIdXFecha(string tecnologia, DateTime fechaConsulta, string subdominios, string owner, int IndexObs, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);
        public abstract List<EquipoDTO> GetEquiposDesactivados(string nombre, List<int> tipoEquipoIds, List<int> subsidiariaIds, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows);

        //RelacionPublic
        public abstract int ValidarEquipoByNombre(string Nombre);
        public abstract int ValidarRelevanciaByNombre(string nombre);
        public abstract int ValidarAmbienteByNombre(string nombre);
        public abstract string ValidarCodigoAPTByNombre(string nombre);

        //EquipoPublic
        public abstract int ValidarDominioServidorByNombre(string nombre);
        public abstract int ValidarTecnologiaByNombre(string nombre);
        public abstract int ValidarTipoEquipoByNombre(string nombre);
        public abstract int ValidarCaracteristicaEquipoByNombre(string nombre);

        public abstract FiltrosIndicadoresGerencialEquipo ListFiltrosIndicadores();

        public abstract FiltrosIndicadoresGerencialEquipo ListFiltrosEvolucionInstalacionEquipos();

        public abstract List<EquipoDTO> ListarEquiposXTecnologiaTipoEquipo(int tecnologiaId, int tipoEquipoId);
        public abstract List<EquipoDTO> ListarEquiposXProductoTipoEquipo(int productoId, int tipoEquipoId);
        public abstract List<DatosAdicionalesEquiposDto> ListarDatosAdicionales();
        public abstract List<CertificadoDigitalDTO> GetCertificadosDigitalesByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, out int totalRows);

        public abstract List<ClientSecretDTO> GetClientSecretsByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, out int totalRows);

        public abstract List<EquipoServidorVirtualDTO> GetEquipoServidorVirtual(int id, int pageNumber, int pageSize, out int totalRows);
        public abstract List<EquipoServidorVirtualMasDTO> GetEquipoServidorVirtualDetalle(int id, int pageNumber, int pageSize, out int totalRows);
        public abstract List<EquipoServidorVirtualDTO> GetReportEquipoServidorVirtual(int id);
    }
}
